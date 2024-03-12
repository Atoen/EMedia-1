using System.Buffers.Binary;
using System.Text;

namespace EMedia_1;

public class PngReader(Stream stream)
{
    public static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
    public static int SignatureLength => PngSignature.Length;
    public const int IHDRChunkLength = 13;
    public const string IHDR = "IHDR";
    private readonly Crc32 _crc32 = new();

    public void Read()
    {
        var isPng = CheckSignature();
        Console.WriteLine(isPng ? "File is png" : "File is not png");
        
        ReadIHDRChunk();
    }

    private bool CheckSignature()
    {
        var buffer = new byte[SignatureLength];
        var bytesRead = stream.Read(buffer);

        return buffer.Take(bytesRead)
            .SequenceEqual(PngSignature);
    }

    private void ReadIHDRChunk()
    {
        var lengthBytes = ReadBytes(4);
        var length = BinaryPrimitives.ReadUInt32BigEndian(lengthBytes);
        
        Console.WriteLine($"Length: {length}");
        if (length != IHDRChunkLength)
        {
            throw new InvalidOperationException("Invalid IHDR chunk length");
        }
        
        var typeBytes = ReadBytes(4).AsSpan();
        var type = Encoding.ASCII.GetString(typeBytes);

        Console.WriteLine($"Type: {type}");
        if (type != IHDR)
        {
            throw new InvalidOperationException("First chunk is not IHDR");
        }
        
        var data = ReadBytes(length).AsSpan();

        var width = BinaryPrimitives.ReadUInt32BigEndian(data[..4]);
        var height = BinaryPrimitives.ReadUInt32BigEndian(data[4..8]);
        var bitDepth = data[8];
        var colorType = data[9];
        var compressionMethod = data[10];
        var filterMethod = data[11];
        var interlaceMethod = data[12];

        Console.WriteLine($"Width: {width}");
        Console.WriteLine($"Height: {height}");
        Console.WriteLine($"Bit Depth: {bitDepth}");
        Console.WriteLine($"Color Type: {colorType}");
        Console.WriteLine($"Compression Method: {compressionMethod}");
        Console.WriteLine($"Filter Method: {filterMethod}");
        Console.WriteLine($"Interlace Method: {interlaceMethod}");
        
        var crcBytes = ReadBytes(4);
        var crc = BinaryPrimitives.ReadUInt32BigEndian(crcBytes);
        var crcCheck = _crc32.Get([..typeBytes, ..data]);

        Console.WriteLine($"crc {crc} == {crcCheck}: {crc == crcCheck}");
    }

    private byte[] ReadBytes(uint length)
    {
        var buffer = new byte[length];
        var bytesRead = stream.Read(buffer);
        if (bytesRead != length)
        {
            throw new InvalidOperationException();
        }

        return buffer;
    }
}