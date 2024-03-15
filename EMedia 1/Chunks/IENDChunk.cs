namespace EMedia_1.Chunks;

public class IENDChunk : PngChunk
{
    public IENDChunk(uint length, byte[] data, string type, uint crc, bool crcValid) :
        base(length, data, type, crc, crcValid)
    {
    }

    public override void PrintData()
    {
        Console.WriteLine($"Type: {Type}");
    }
}