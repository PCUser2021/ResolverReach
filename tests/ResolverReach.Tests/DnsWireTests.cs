using System.Buffers.Binary;
using ResolverReach;
using Xunit;

namespace ResolverReach.Tests;

public sealed class DnsWireTests
{
    [Fact]
    public void ProbeQuery_UsesIdZeroAndQueriesExampleDotComAInClassIn()
    {
        byte[] query = DnsWire.BuildExampleAQuery(out ushort id);

        Assert.Equal((ushort)0, id);
        Assert.Equal((ushort)0, BinaryPrimitives.ReadUInt16BigEndian(query.AsSpan(0, 2)));
        Assert.Equal((ushort)1, BinaryPrimitives.ReadUInt16BigEndian(query.AsSpan(4, 2)));
        Assert.Equal("example.com", ReadUncompressedName(query, 12, out int nameEnd));
        Assert.Equal((ushort)1, BinaryPrimitives.ReadUInt16BigEndian(query.AsSpan(nameEnd, 2)));
        Assert.Equal((ushort)1, BinaryPrimitives.ReadUInt16BigEndian(query.AsSpan(nameEnd + 2, 2)));
        Assert.Equal(0x01, query[2]); // RD
    }

    [Fact]
    public void AcceptsCompleteNoErrorResponseAndParsesAnswerSection()
    {
        byte[] response = BuildResponse(answerCount: 1, answerRecords: ARecord());

        bool valid = DnsWire.TryValidateResponse(response, 0, out int rcode, out int answers, out string error);

        Assert.True(valid, error);
        Assert.Equal(0, rcode);
        Assert.Equal(1, answers);
        Assert.Equal("NOERROR", DnsWire.RcodeName(rcode));
    }

    [Fact]
    public void AcceptsValidNxDomainAsDnsResponseRatherThanTransportFailure()
    {
        byte[] response = BuildResponse(flags: 0x8183);

        bool valid = DnsWire.TryValidateResponse(response, 0, out int rcode, out int answers, out string error);

        Assert.True(valid, error);
        Assert.Equal(3, rcode);
        Assert.Equal(0, answers);
        Assert.Equal("NXDOMAIN", DnsWire.RcodeName(rcode));
    }

    [Fact]
    public void ParsesExtendedRcodeFromEdnsOptRecord()
    {
        byte[] opt = new byte[]
        {
            0,             // root owner
            0, 41,         // TYPE = OPT
            4, 208,        // UDP payload size 1232
            1, 0, 0, 0,    // TTL: extended RCODE = 1, EDNS version/flags = 0
            0, 0            // RDLENGTH = 0
        };
        byte[] response = BuildResponse(additionalCount: 1, additionalRecords: opt);

        bool valid = DnsWire.TryValidateResponse(response, 0, out int rcode, out _, out string error);

        Assert.True(valid, error);
        Assert.Equal(16, rcode);
        Assert.Equal("BADVERS/BADSIG", DnsWire.RcodeName(rcode));
    }

    [Fact]
    public void ValidatesResourceRecordsInAnswerAuthorityAndAdditionalSections()
    {
        byte[] answer = ARecord();
        byte[] authority = NameRecord(type: 2); // NS, owner example.com, target root
        byte[] additional = NameRecord(type: 12); // PTR, owner example.com, target root
        byte[] response = BuildResponse(
            answerCount: 1, authorityCount: 1, additionalCount: 1,
            answerRecords: answer, authorityRecords: authority, additionalRecords: additional);

        bool valid = DnsWire.TryValidateResponse(response, 0, out int rcode, out int answers, out string error);

        Assert.True(valid, error);
        Assert.Equal(0, rcode);
        Assert.Equal(1, answers);
    }

    [Fact]
    public void RejectsMismatchedTransactionId()
    {
        byte[] response = BuildResponse(id: 42);

        Assert.False(DnsWire.TryValidateResponse(response, 0, out _, out _, out string error));
        Assert.Contains("transaction ID", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RejectsWrongQuestionNameAndType()
    {
        byte[] wrongName = BuildResponse(domain: "example.net");
        byte[] wrongType = BuildResponse(questionType: 28);

        Assert.False(DnsWire.TryValidateResponse(wrongName, 0, out _, out _, out string nameError));
        Assert.Contains("question", nameError, StringComparison.OrdinalIgnoreCase);
        Assert.False(DnsWire.TryValidateResponse(wrongType, 0, out _, out _, out string typeError));
        Assert.Contains("type A", typeError, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RejectsNonResponseAndTruncatedHeader()
    {
        byte[] notResponse = BuildResponse(flags: 0x0100);
        byte[] truncated = new byte[11];

        Assert.False(DnsWire.TryValidateResponse(notResponse, 0, out _, out _, out string flagError));
        Assert.Contains("response", flagError, StringComparison.OrdinalIgnoreCase);
        Assert.False(DnsWire.TryValidateResponse(truncated, 0, out _, out _, out string headerError));
        Assert.Contains("12-byte", headerError, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RejectsTruncatedFlagAndInvalidResourceRecordLength()
    {
        byte[] truncatedFlag = BuildResponse(flags: 0x8380);
        byte[] malformedRecord = BuildResponse(answerCount: 1, answerRecords: ARecord()[..^1]);

        Assert.False(DnsWire.TryValidateResponse(truncatedFlag, 0, out _, out _, out string flagError));
        Assert.Contains("truncated", flagError, StringComparison.OrdinalIgnoreCase);
        Assert.False(DnsWire.TryValidateResponse(malformedRecord, 0, out _, out _, out string recordError));
        Assert.Contains("RDLENGTH", recordError, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RejectsForwardOrSelfReferencingCompressionPointer()
    {
        // The question ends at byte 29; its answer owner must not point to itself.
        byte[] maliciousOwner = new byte[]
        {
            0xC0, 0x1D,     // compression pointer to the answer's own offset 29
            0, 1, 0, 1,
            0, 0, 0, 60,
            0, 4, 93, 184, 216, 34
        };
        byte[] response = BuildResponse(answerCount: 1, answerRecords: maliciousOwner);

        Assert.False(DnsWire.TryValidateResponse(response, 0, out _, out _, out string error));
        Assert.Contains("owner name", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RejectsTrailingBytesAndMalformedEdnsOptions()
    {
        byte[] trailing = BuildResponse().Concat(new byte[] { 0 }).ToArray();
        byte[] badOpt = new byte[]
        {
            0, 0, 41, 4, 208, 0, 0, 0, 0,
            0, 4,             // RDLENGTH = 4
            0, 1, 0, 8        // option header requires eight bytes but no option data follows
        };
        byte[] response = BuildResponse(additionalCount: 1, additionalRecords: badOpt);

        Assert.False(DnsWire.TryValidateResponse(trailing, 0, out _, out _, out string trailingError));
        Assert.Contains("trailing bytes", trailingError, StringComparison.OrdinalIgnoreCase);
        Assert.False(DnsWire.TryValidateResponse(response, 0, out _, out _, out string ednsError));
        Assert.Contains("EDNS option", ednsError, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RcodeNameHandlesCommonExtendedDnsErrors()
    {
        Assert.Equal("BADKEY", DnsWire.RcodeName(17));
        Assert.Equal("BADCOOKIE", DnsWire.RcodeName(23));
        Assert.Equal("RCODE 999", DnsWire.RcodeName(999));
    }

    private static byte[] BuildResponse(
        string domain = "example.com",
        ushort flags = 0x8180,
        ushort id = 0,
        ushort questionType = 1,
        ushort answerCount = 0,
        ushort authorityCount = 0,
        ushort additionalCount = 0,
        byte[]? answerRecords = null,
        byte[]? authorityRecords = null,
        byte[]? additionalRecords = null)
    {
        byte[] question = BuildQuestion(domain, questionType);
        byte[] records = (answerRecords ?? Array.Empty<byte>())
            .Concat(authorityRecords ?? Array.Empty<byte>())
            .Concat(additionalRecords ?? Array.Empty<byte>())
            .ToArray();
        byte[] packet = new byte[12 + question.Length + records.Length];

        BinaryPrimitives.WriteUInt16BigEndian(packet.AsSpan(0, 2), id);
        BinaryPrimitives.WriteUInt16BigEndian(packet.AsSpan(2, 2), flags);
        BinaryPrimitives.WriteUInt16BigEndian(packet.AsSpan(4, 2), 1);
        BinaryPrimitives.WriteUInt16BigEndian(packet.AsSpan(6, 2), answerCount);
        BinaryPrimitives.WriteUInt16BigEndian(packet.AsSpan(8, 2), authorityCount);
        BinaryPrimitives.WriteUInt16BigEndian(packet.AsSpan(10, 2), additionalCount);

        question.CopyTo(packet, 12);
        records.CopyTo(packet, 12 + question.Length);
        return packet;
    }

    private static byte[] BuildQuestion(string domain, ushort type)
    {
        using var stream = new MemoryStream();
        foreach (string label in domain.Split('.'))
        {
            byte[] labelBytes = System.Text.Encoding.ASCII.GetBytes(label);
            stream.WriteByte((byte)labelBytes.Length);
            stream.Write(labelBytes);
        }

        stream.WriteByte(0);
        Span<byte> fields = stackalloc byte[4];
        BinaryPrimitives.WriteUInt16BigEndian(fields[..2], type);
        BinaryPrimitives.WriteUInt16BigEndian(fields[2..], 1);
        stream.Write(fields);
        return stream.ToArray();
    }

    private static byte[] ARecord()
    {
        return new byte[]
        {
            0xC0, 0x0C,     // owner name points to question
            0, 1,           // TYPE A
            0, 1,           // CLASS IN
            0, 0, 0, 60,    // TTL 60 seconds
            0, 4,           // RDLENGTH 4
            93, 184, 216, 34
        };
    }

    private static byte[] NameRecord(ushort type)
    {
        return new byte[]
        {
            0xC0, 0x0C,     // owner name points to question
            (byte)(type >> 8), (byte)type,
            0, 1,
            0, 0, 0, 60,
            0, 1,           // one-byte target name (root)
            0
        };
    }

    private static string ReadUncompressedName(byte[] message, int offset, out int endOffset)
    {
        var labels = new List<string>();
        int cursor = offset;
        while (cursor < message.Length)
        {
            byte length = message[cursor++];
            if (length == 0)
                break;
            labels.Add(System.Text.Encoding.ASCII.GetString(message, cursor, length));
            cursor += length;
        }

        endOffset = cursor;
        return string.Join('.', labels);
    }
}
