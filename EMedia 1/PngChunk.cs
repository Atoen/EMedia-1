using System.Text;

namespace EMedia_1;

public class PngChunk
{
    public const int IHDRChunkLength = 13;
    
    public const string IHDR = "IHDR"; // Image header
    public const string IEND = "IEND"; // Image end
    public const string PLTE = "PLTE"; // Image palette
    public const string sRGB = "sRGB"; // Standard RGB color space
    public const string gAMA = "gAMA"; // Gamma
    public const string pHYs = "pHYs"; // Physical pixel dimensions
    public const string IDAT = "IDAT"; // Image data
    public const string cHRM = "cHRM"; // Chromaticity
    public const string iCCP = "iCCP"; // ICC color profile
    public const string tEXt = "tEXt"; // Textual data
    public const string zTXt = "zTXt"; // Compressed textual data
    public const string iTXt = "iTXt"; // International textual data
    public const string bKGD = "bKGD"; // Background color
    public const string hIST = "hIST"; // Image histogram
    public const string sBIT = "sBIT"; // Significant bits
    public const string sPLT = "sPLT"; // Suggested palette
    public const string tIME = "tIME"; // Image last-modification time
    
    public static PngChunk Create(Stream stream)
    {
        var length = stream.ReadBytes(4).GetUint();
        var typeBytes = stream.ReadBytes(4);
        var type = Encoding.ASCII.GetString(typeBytes);
        var data = stream.ReadBytes(length);
        var crc = stream.ReadBytes(4).GetUint();

        return new PngChunk
        {
            Length = length,
            Type = type,
            Data = data,
            Crc = crc,
            CrcValid = Crc32.Get([..typeBytes, ..data]) == crc
        };
    }
    
    public required uint Length { get; init; }
    public required string Type { get; init; }
    public required byte[] Data { get; init; }
    public required uint Crc { get; init; }
    public required bool CrcValid { get; init; }
}

public enum PngChunkType
{
    IHDR, // Image header
    PLTE, // Palette
    IDAT, // Image data
    IEND, // Image end
    tRNS, // Transparency
    cHRM, // Chromaticity
    sRGB, // Standard RGB color space
    gAMA, // Gamma
    iCCP, // ICC color profile
    tEXt, // Textual data
    zTXt, // Compressed textual data
    iTXt, // International textual data
    bKGD, // Background color
    hIST, // Image histogram
    pHYs, // Physical pixel dimensions
    sBIT, // Significant bits
    sPLT, // Suggested palette
    tIME  // Image last-modification time
}