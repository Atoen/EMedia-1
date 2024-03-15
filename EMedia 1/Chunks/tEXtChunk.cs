using System.Text;

namespace EMedia_1.Chunks;

public class tEXtChunk : PngChunk
{
    public string Keyword { get; }
    public string Text { get; }
    
    public override bool AllowMultiple => true;
    public override bool RemoveWhenAnonymizing => true;

    public tEXtChunk(uint length, byte[] data, string type, uint crc, bool crcValid) :
        base(length, data, type, crc, crcValid)
    {
        var span = data.AsSpan();
        var nullIndex = span.IndexOf((byte) 0); 

        Keyword = Encoding.Latin1.GetString(span[..nullIndex]);
        Text = Encoding.Latin1.GetString(span[(nullIndex + 1)..]);
    }

    public override void PrintData()
    {
        Console.WriteLine($"Type: {Type}, Keyword: {Keyword}, Text: {Text}");
    }
}