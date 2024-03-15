namespace EMedia_1.Chunks;

public class IDATChunk : PngChunk
{
    public byte[] ImageData { get; }

    public override bool AllowMultiple => true;

    public IDATChunk(uint length, byte[] data, string type, uint crc, bool crcValid) :
        base(length, data, type, crc, crcValid)
    {
        ImageData = data;
    }

    public override void PrintData()
    {
        Console.WriteLine($"Type: {Type}, Data Length: {ImageData.Length}");
    }
}