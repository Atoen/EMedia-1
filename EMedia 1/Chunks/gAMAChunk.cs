namespace EMedia_1.Chunks;

public class gAMAChunk : PngChunk
{
    public double Gamma { get; }

    public gAMAChunk(uint length, byte[] data, string type, uint crc, bool crcValid) :
        base(length, data, type, crc, crcValid)
    {
        var gammaValue = data.GetUint();
        Gamma = gammaValue / 100000.0;
    }

    public override void PrintData()
    {
        Console.WriteLine($"Type: {Type}, Gamma: {Gamma}");
    }

    protected override void EnsureValid()
    {
        if (Data.Length != 4)
        {
            throw new ArgumentException("gAMA chunk data must be exactly 4 bytes long.");
        }
    }
}