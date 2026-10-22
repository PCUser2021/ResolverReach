# Generates a Windows ICO and preview PNG using Windows' built-in GDI+ support.
# The icon is an original Resolver Reach mark: blue R monogram + DNS circuit nodes.
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing

$repoRoot = Split-Path -Parent $PSScriptRoot
$assetDir = Join-Path $repoRoot "src\ResolverReach\Assets"
New-Item -ItemType Directory -Force -Path $assetDir | Out-Null
$icoPath = Join-Path $assetDir "ResolverReach.ico"
$pngPath = Join-Path $assetDir "ResolverReach-icon.png"

function New-ResolverReachPngBytes {
    param([Parameter(Mandatory)][int]$Size)
    $bitmap = [System.Drawing.Bitmap]::new($Size, $Size, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $graphics.ScaleTransform([single]($Size / 256.0), [single]($Size / 256.0))
    try {
        $roundRect = [System.Drawing.Drawing2D.GraphicsPath]::new()
        $roundRect.AddArc(8, 8, 56, 56, 180, 90)
        $roundRect.AddArc(192, 8, 56, 56, 270, 90)
        $roundRect.AddArc(192, 192, 56, 56, 0, 90)
        $roundRect.AddArc(8, 192, 56, 56, 90, 90)
        $roundRect.CloseFigure()
        $background = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
            [System.Drawing.Rectangle]::new(8, 8, 240, 240),
            [System.Drawing.Color]::FromArgb(8, 25, 55),
            [System.Drawing.Color]::FromArgb(2, 7, 22),
            45.0
        )
        $graphics.FillPath($background, $roundRect)
        $graphics.DrawPath([System.Drawing.Pens]::MidnightBlue, $roundRect)
        $background.Dispose()
        $roundRect.Dispose()

        $circuitPen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(0, 211, 255), 5.5)
        $circuitPen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
        $circuitPen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
        $circuitPen.LineJoin = [System.Drawing.Drawing2D.LineJoin]::Round
        $graphics.DrawLine($circuitPen, 50, 91, 77, 111)
        $graphics.DrawLine($circuitPen, 77, 111, 108, 111)
        $graphics.DrawLine($circuitPen, 50, 145, 104, 145)
        $graphics.DrawLine($circuitPen, 50, 198, 78, 177)
        $graphics.DrawLine($circuitPen, 78, 177, 109, 177)
        $circuitPen.Dispose()

        foreach ($point in @(@{X=29;Y=72}, @{X=29;Y=126}, @{X=29;Y=179})) {
            $nodeFill = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
                [System.Drawing.Rectangle]::new($point.X, $point.Y, 24, 24),
                [System.Drawing.Color]::FromArgb(0, 238, 255),
                [System.Drawing.Color]::FromArgb(0, 111, 255),
                45.0
            )
            $graphics.FillEllipse($nodeFill, $point.X, $point.Y, 24, 24)
            $nodeFill.Dispose()
            $inner = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(8, 25, 55))
            $graphics.FillEllipse($inner, $point.X + 7, $point.Y + 7, 10, 10)
            $inner.Dispose()
        }

        $glyph = [System.Drawing.Drawing2D.GraphicsPath]::new()
        $family = [System.Drawing.FontFamily]::new("Arial")
        $glyph.AddString("R", $family, [int][System.Drawing.FontStyle]::Bold, 174.0,
            [System.Drawing.PointF]::new(56, 20), [System.Drawing.StringFormat]::GenericDefault)
        $glyphFill = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
            [System.Drawing.Rectangle]::new(55, 24, 155, 208),
            [System.Drawing.Color]::FromArgb(0, 226, 255),
            [System.Drawing.Color]::FromArgb(56, 67, 255),
            28.0
        )
        $graphics.FillPath($glyphFill, $glyph)
        $outline = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(110, 49, 168, 255), 1.6)
        $graphics.DrawPath($outline, $glyph)
        $glyphFill.Dispose()
        $outline.Dispose()
        $glyph.Dispose()
        $family.Dispose()

        $stream = [System.IO.MemoryStream]::new()
        $bitmap.Save($stream, [System.Drawing.Imaging.ImageFormat]::Png)
        return ,$stream.ToArray()
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }
}

$sizes = @(16, 24, 32, 48, 64, 128, 256)
$pngImages = @()
foreach ($size in $sizes) { $pngImages += ,(New-ResolverReachPngBytes -Size $size) }
[System.IO.File]::WriteAllBytes($pngPath, $pngImages[$pngImages.Count - 1])

$stream = [System.IO.MemoryStream]::new()
$writer = [System.IO.BinaryWriter]::new($stream)
try {
    $writer.Write([UInt16]0)
    $writer.Write([UInt16]1)
    $writer.Write([UInt16]$sizes.Count)
    $offset = 6 + (16 * $sizes.Count)
    for ($i = 0; $i -lt $sizes.Count; $i++) {
        $dim = if ($sizes[$i] -eq 256) { 0 } else { [byte]$sizes[$i] }
        $writer.Write([byte]$dim); $writer.Write([byte]$dim)
        $writer.Write([byte]0); $writer.Write([byte]0)
        $writer.Write([UInt16]1); $writer.Write([UInt16]32)
        $writer.Write([UInt32]$pngImages[$i].Length)
        $writer.Write([UInt32]$offset)
        $offset += $pngImages[$i].Length
    }
    foreach ($imageBytes in $pngImages) { $writer.Write([byte[]]$imageBytes) }
    $writer.Flush()
    [System.IO.File]::WriteAllBytes($icoPath, $stream.ToArray())
}
finally {
    $writer.Dispose()
    $stream.Dispose()
}
Write-Host "Generated $icoPath"
Write-Host "Generated $pngPath"
