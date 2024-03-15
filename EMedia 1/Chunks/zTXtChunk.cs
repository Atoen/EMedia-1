using System.IO.Compression;
using System.Text;

namespace EMedia_1.Chunks;

public class zTXtChunk : PngChunk
{
    public string Keyword { get; }
    public CompressionMethod CompressionMethod { get; }
    public string Text { get; }

    public zTXtChunk(uint length, byte[] data, string type, uint crc, bool crcValid) :
        base(length, data, type, crc, crcValid)
    {
        var span = data.AsSpan();
        var nullIndex = span.IndexOf((byte) 0); 

        Keyword = Encoding.Latin1.GetString(span[..nullIndex]);
        CompressionMethod = (CompressionMethod) span[nullIndex + 1];
        Text = Decompress(data[(nullIndex + 2)..]);
    }
    
    public static string Decompress(byte[] data)
    {
        using var decompressedStream = new MemoryStream();
        using var compressStream = new MemoryStream(data);
        using var deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress);
        
        deflateStream.CopyTo(decompressedStream);
        return Encoding.Latin1.GetString(decompressedStream.ToArray());
    }

    public override void PrintData()
    {
        Console.WriteLine($"Type: {Type}, Keyword: {Keyword}, Compression Method: {CompressionMethod}, Text: {Text}");
    }
}
