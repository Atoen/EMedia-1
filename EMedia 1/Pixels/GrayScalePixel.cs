namespace EMedia_1.Pixels;

public readonly struct GrayScalePixel(byte[] buffer, int offset = 0) : IPixel
{
    public byte Intensity { get; } = buffer[offset];
    
    public string Print() => $"\x1b[48;2;{Intensity};{Intensity};{Intensity}m  ";

    public double AverageValue => Intensity;
}