using System.Text;

namespace ResolverReach;

/// <summary>
/// Builds the application's deterministic DoH probe and validates the entire
/// DNS message envelope (header, question, every RR section and EDNS options).
/// Unknown RR payloads remain opaque as required for forward compatibility,
/// while common address/name-bearing RDATA formats receive structural checks.
/// </summary>
internal static class DnsWire
{
    private const int HeaderLength = 12;
    private const int MaximumMessageLength = 65535;

    public static byte[] BuildExampleAQuery(out ushort transactionId)
    {
        // RFC 8484 recommends ID zero for cache friendliness. There is no UDP
        // transaction multiplexing here, so a random DNS ID adds no correlation value.
        transactionId = 0;

        const string domain = "example.com";
        string[] labels = domain.Split('.');
        int encodedNameLength = labels.Sum(label => 1 + Encoding.ASCII.GetByteCount(label)) + 1;
        byte[] packet = new byte[HeaderLength + encodedNameLength + 4];

        packet[0] = 0;
        packet[1] = 0;
        packet[2] = 0x01; // RD
        packet[3] = 0;
        packet[4] = 0;
        packet[5] = 1;    // QDCOUNT = 1

        int offset = HeaderLength;
        foreach (string label in labels)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(label);
            packet[offset++] = (byte)bytes.Length;
            Buffer.BlockCopy(bytes, 0, packet, offset, bytes.Length);
            offset += bytes.Length;
        }

        packet[offset++] = 0;     // root label
        packet[offset++] = 0;
        packet[offset++] = 1;     // QTYPE = A
        packet[offset++] = 0;
        packet[offset] = 1;       // QCLASS = IN
        return packet;
    }

    public static bool TryValidateResponse(
        byte[] message,
        ushort expectedId,
        out int rcode,
        out int answerCount,
        out string error)
    {
        rcode = -1;
        answerCount = 0;
        error = string.Empty;

        if (message.Length < HeaderLength)
        {
            error = "Response is shorter than the 12-byte DNS header.";
            return false;
        }

        if (message.Length > MaximumMessageLength)
        {
            error = "Response exceeds the maximum DNS message length.";
            return false;
        }

        if (ReadUInt16(message, 0) != expectedId)
        {
            error = "DNS transaction ID did not match the request.";
            return false;
        }

        ushort flags = ReadUInt16(message, 2);
        if ((flags & 0x8000) == 0)
        {
            error = "DNS message did not have the response (QR) bit set.";
            return false;
        }

        if ((flags & 0x7800) != 0)
        {
            error = "DNS response used an unexpected opcode.";
            return false;
        }

        if ((flags & 0x0200) != 0)
        {
            error = "DNS response was truncated (TC bit set).";
            return false;
        }

        if ((flags & 0x0040) != 0)
        {
            error = "DNS response set a reserved header flag.";
            return false;
        }

        if (ReadUInt16(message, 4) != 1)
        {
            error = "DNS response did not echo exactly one question.";
            return false;
        }

        int offset = HeaderLength;
        if (!TryReadName(message, ref offset, out string questionName))
        {
            error = "DNS question name was malformed.";
            return false;
        }

        if (!string.Equals(questionName, "example.com", StringComparison.OrdinalIgnoreCase))
        {
            error = "DNS response question did not match example.com.";
            return false;
        }

        if (offset > message.Length - 4)
        {
            error = "DNS response ended before the question type and class.";
            return false;
        }

        ushort questionType = ReadUInt16(message, offset);
        ushort questionClass = ReadUInt16(message, offset + 2);
        offset += 4;
        if (questionType != 1 || questionClass != 1)
        {
            error = "DNS response question was not type A / class IN.";
            return false;
        }

        int answers = ReadUInt16(message, 6);
        int authorities = ReadUInt16(message, 8);
        int additionals = ReadUInt16(message, 10);
        answerCount = answers;

        long totalRecords = (long)answers + authorities + additionals;
        if (totalRecords > (message.Length - offset) / 11)
        {
            error = "DNS section counts cannot fit in the remaining message.";
            return false;
        }

        int extendedRcode = 0;
        bool sawOpt = false;
        if (!TryReadRecords(message, ref offset, answers, section: 0, ref extendedRcode, ref sawOpt, out error) ||
            !TryReadRecords(message, ref offset, authorities, section: 1, ref extendedRcode, ref sawOpt, out error) ||
            !TryReadRecords(message, ref offset, additionals, section: 2, ref extendedRcode, ref sawOpt, out error))
        {
            return false;
        }

        if (offset != message.Length)
        {
            error = "DNS message contains trailing bytes after all declared sections.";
            return false;
        }

        rcode = (extendedRcode << 4) | (flags & 0x000f);
        return true;
    }

    private static bool TryReadRecords(
        byte[] message,
        ref int offset,
        int count,
        int section,
        ref int extendedRcode,
        ref bool sawOpt,
        out string error)
    {
        error = string.Empty;
        for (int i = 0; i < count; i++)
        {
            if (!TryReadName(message, ref offset, out string ownerName))
            {
                error = "A resource-record owner name was malformed.";
                return false;
            }

            if (offset > message.Length - 10)
            {
                error = "A resource record ended before its fixed header.";
                return false;
            }

            ushort type = ReadUInt16(message, offset);
            uint ttl = ReadUInt32(message, offset + 4);
            ushort dataLength = ReadUInt16(message, offset + 8);
            offset += 10;

            int dataStart = offset;
            int dataEnd = dataStart + dataLength;
            if (dataEnd < dataStart || dataEnd > message.Length)
            {
                error = "A resource-record RDLENGTH extends past the DNS message.";
                return false;
            }

            if (type == 41) // OPT pseudo-RR
            {
                if (section != 2)
                {
                    error = "An EDNS OPT record appeared outside the additional section.";
                    return false;
                }

                if (sawOpt)
                {
                    error = "DNS response contains more than one EDNS OPT record.";
                    return false;
                }

                if (ownerName.Length != 0)
                {
                    error = "EDNS OPT owner name must be the DNS root.";
                    return false;
                }

                sawOpt = true;
                extendedRcode = (int)(ttl >> 24);
                if (!TryValidateEdnsOptions(message, dataStart, dataEnd))
                {
                    error = "An EDNS option has a truncated or invalid length.";
                    return false;
                }
            }
            else if (!TryValidateKnownRdata(message, type, dataStart, dataEnd, out error))
            {
                return false;
            }

            offset = dataEnd;
        }

        return true;
    }

    private static bool TryValidateKnownRdata(
        byte[] message,
        ushort type,
        int start,
        int end,
        out string error)
    {
        error = string.Empty;
        int length = end - start;

        if (type == 1 && length != 4)
        {
            error = "A DNS A record must contain exactly four address bytes.";
            return false;
        }

        if (type == 28 && length != 16)
        {
            error = "A DNS AAAA record must contain exactly sixteen address bytes.";
            return false;
        }

        // NS, CNAME, PTR and DNAME contain one domain name.
        if (type is 2 or 5 or 12 or 39)
        {
            int cursor = start;
            if (!TryReadName(message, ref cursor, out _) || cursor != end)
            {
                error = "A name-bearing DNS RDATA field has an invalid encoding or length.";
                return false;
            }
        }
        else if (type == 15) // MX: preference + exchange name
        {
            if (length < 3)
            {
                error = "A DNS MX record is too short.";
                return false;
            }

            int cursor = start + 2;
            if (!TryReadName(message, ref cursor, out _) || cursor != end)
            {
                error = "A DNS MX exchange name has an invalid encoding or length.";
                return false;
            }
        }
        else if (type == 6) // SOA: two names + five 32-bit integers
        {
            int cursor = start;
            if (!TryReadName(message, ref cursor, out _) ||
                !TryReadName(message, ref cursor, out _) ||
                cursor > end - 20 || cursor + 20 != end)
            {
                error = "A DNS SOA record has an invalid names or integer-field length.";
                return false;
            }
        }
        else if (type == 33) // SRV: priority, weight, port, target name
        {
            if (length < 7)
            {
                error = "A DNS SRV record is too short.";
                return false;
            }

            int cursor = start + 6;
            if (!TryReadName(message, ref cursor, out _) || cursor != end)
            {
                error = "A DNS SRV target name has an invalid encoding or length.";
                return false;
            }
        }

        // Other RR-specific RDATA is deliberately opaque: its exact grammar
        // depends on the RR type, but its declared byte range is validated.
        return true;
    }

    private static bool TryValidateEdnsOptions(byte[] message, int start, int end)
    {
        int cursor = start;
        while (cursor < end)
        {
            if (cursor > end - 4)
                return false;

            ushort optionLength = ReadUInt16(message, cursor + 2);
            cursor += 4;
            if (optionLength > end - cursor)
                return false;

            cursor += optionLength;
        }

        return cursor == end;
    }

    public static string RcodeName(int rcode) => rcode switch
    {
        0 => "NOERROR",
        1 => "FORMERR",
        2 => "SERVFAIL",
        3 => "NXDOMAIN",
        4 => "NOTIMP",
        5 => "REFUSED",
        6 => "YXDOMAIN",
        7 => "YXRRSET",
        8 => "NXRRSET",
        9 => "NOTAUTH",
        10 => "NOTZONE",
        11 => "DSOTYPENI",
        16 => "BADVERS/BADSIG",
        17 => "BADKEY",
        18 => "BADTIME",
        19 => "BADMODE",
        20 => "BADNAME",
        21 => "BADALG",
        22 => "BADTRUNC",
        23 => "BADCOOKIE",
        _ => $"RCODE {rcode}"
    };

    private static ushort ReadUInt16(byte[] buffer, int offset) =>
        (ushort)((buffer[offset] << 8) | buffer[offset + 1]);

    private static uint ReadUInt32(byte[] buffer, int offset) =>
        ((uint)buffer[offset] << 24) |
        ((uint)buffer[offset + 1] << 16) |
        ((uint)buffer[offset + 2] << 8) |
        buffer[offset + 3];

    private static bool TryReadName(byte[] message, ref int offset, out string name)
    {
        var labels = new List<string>();
        int cursor = offset;
        int? resumeOffset = null;
        int expandedWireLength = 1;

        for (int step = 0; step < 128 && cursor < message.Length; step++)
        {
            byte length = message[cursor];

            if ((length & 0xc0) == 0xc0)
            {
                if (cursor + 1 >= message.Length)
                {
                    name = string.Empty;
                    return false;
                }

                int pointer = ((length & 0x3f) << 8) | message[cursor + 1];
                // DNS compression pointers must target a prior occurrence.
                if (pointer >= cursor)
                {
                    name = string.Empty;
                    return false;
                }

                resumeOffset ??= cursor + 2;
                cursor = pointer;
                continue;
            }

            if ((length & 0xc0) != 0)
            {
                name = string.Empty;
                return false;
            }

            cursor++;
            if (length == 0)
            {
                offset = resumeOffset ?? cursor;
                name = string.Join('.', labels);
                return true;
            }

            if (length > 63 || cursor + length > message.Length)
            {
                name = string.Empty;
                return false;
            }

            expandedWireLength += length + 1;
            if (expandedWireLength > 255)
            {
                name = string.Empty;
                return false;
            }

            labels.Add(Encoding.ASCII.GetString(message, cursor, length));
            cursor += length;
        }

        name = string.Empty;
        return false;
    }
}
