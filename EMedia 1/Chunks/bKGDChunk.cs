﻿namespace EMedia_1.Chunks;

public class bKGDChunk : PngChunk
{
    public byte[] BackgroundColorData { get; }
    
    public override bool RemoveWhenAnonymizing => true;
    
    public bKGDChunk(uint length, byte[] data, string type, uint crc, bool crcValid) : 
        base(length, data, type, crc ,crcValid)
    {
        BackgroundColorData = data;
    }
    
    public override void PrintData()
    {
        var backgroundColor = "";

        if (BackgroundColorData.Length >= 1)
        {
            int colorType = BackgroundColorData[0];

            switch (colorType)
            {
                case 0 when BackgroundColorData.Length >= 2: 
                    backgroundColor = $"grayscale {BackgroundColorData[1]}";
                    break;
                case 2 when BackgroundColorData.Length >= 6: 
                    var red = BackgroundColorData[1];
                    var green = BackgroundColorData[3];
                    var blue = BackgroundColorData[5];
                    backgroundColor = $"R={red}, G={green}, B={blue}";
                    break;
                default:
                    backgroundColor = "Unknown color type";
                    break;
            }
        }
        Console.WriteLine($"Type: {Type}, Background Color: {backgroundColor}");
    }
    
    protected override void EnsureValid()
    {
        if (Data.Length != 2)
        {
            throw new ArgumentException("bKGD chunk data must be exactly 2 bytes long.");
        }
    }
}