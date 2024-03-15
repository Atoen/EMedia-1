using System.Text;
using EMedia_1.Chunks;

namespace EMedia_1;

public class PngChunk
{
    public uint Length { get; }
    public byte[] Data { get; }
    public string Type { get; }
    public uint Crc { get; }
    public bool CrcValid { get; }

    public virtual bool AllowMultiple => false;
    
    public virtual void PrintData() => Console.WriteLine($"Type {Type}, Length: {Length}, CRC: {CrcValid}");

    protected virtual void EnsureValid() { }
    
    public static PngChunk Create(Stream stream)
    {
        var length = stream.ReadBytes(4).GetUint();
        var typeBytes = stream.ReadBytes(4);
        
        var data = stream.ReadBytes(length);
        var crc = stream.ReadBytes(4).GetUint();

        var chunk = CreateTyped(length, typeBytes, data, crc);
        chunk.EnsureValid();

        return chunk;
    }

    private static PngChunk CreateTyped(uint length, byte[] typeBytes, byte[] data, uint crc)
    {
        var typeName = Encoding.ASCII.GetString(typeBytes);
        if (!Enum.TryParse<PngChunkType>(typeName, out var type))
        {
            Console.WriteLine($"Unknown chunk type: {typeName}");
        }

        var expectedCrc = Crc32.Get([..typeBytes, ..data]);
        var crcValid = expectedCrc == crc;
        if (!crcValid)
        {
            Console.WriteLine($"Crc value is invalid. Expected: {expectedCrc}, actual: {crc}");
        }

        return type switch
        {
            PngChunkType.IHDR => new IHDRChunk(length, data, typeName, crc, crcValid),
            PngChunkType.tEXt => new tEXtChunk(length, data, typeName, crc, crcValid),
            PngChunkType.IDAT => new IDATChunk(length, data, typeName, crc, crcValid),
            PngChunkType.IEND => new IENDChunk(length, data, typeName, crc, crcValid),
            PngChunkType.gAMA => new gAMAChunk(length, data, typeName, crc, crcValid),
            PngChunkType.sRGB => new sRGBChunk(length, data, typeName, crc, crcValid),
            PngChunkType.PLTE => new PLTEChunk(length, data, typeName, crc, crcValid),
            PngChunkType.cHRM => new cHRMChunk(length, data, typeName, crc, crcValid),
            PngChunkType.tRNS => new tRNSChunk(length, data, typeName, crc, crcValid),
            PngChunkType.tIME => new tIMEChunk(length, data, typeName, crc, crcValid),
            PngChunkType.zTXt => new zTXtChunk(length, data, typeName, crc, crcValid),
            _ => new PngChunk(length, data, typeName, crc, crcValid)
        };
    }
    
    protected PngChunk(uint length, byte[] data, string type, uint crc, bool crcValid)
    {
        Length = length;
        Data = data;
        Type = type;
        Crc = crc;
        CrcValid = crcValid;
    }
    
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