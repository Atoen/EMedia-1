namespace EMedia_1.Chunks;

using System;

public class sBITChunk : PngChunk
{
    public byte[] SignificantBits { get; }

    public override bool RemoveWhenAnonymizing => true;

    public sBITChunk(uint length, byte[] data, string type, uint crc, bool crcValid) :
        base(length, data, type, crc, crcValid)
    {
        SignificantBits = data;
    }
    
    public override void PrintData()
    {
        Console.WriteLine($"Type: {Type}, Significant Bits Length: {SignificantBits.Length}");
    }
    
    protected override void EnsureValid()
    {
        if (Data.Length != 1)
        {
            throw new ArgumentException("sBIT chunk data must be exactly 1 byte long.");
        }
    }
}
