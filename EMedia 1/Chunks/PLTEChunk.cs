namespace EMedia_1.Chunks;

public class PLTEChunk : PngChunk
{
    public byte[] Palette { get; }

    public PLTEChunk(uint length, byte[] data, string type, uint crc, bool crcValid) :
        base(length, data, type, crc, crcValid)
    {
        Palette = data;
    }

    public override void PrintData()
    {
        Console.WriteLine($"Type: {Type}, Palette Length: {Palette.Length / 3} colors");
    }

    protected override void EnsureValid()
    {
        if (Length % 3 != 0)
        {
            throw new ArgumentException("Invalid PLTE chunk data length.");
        }
    }
}