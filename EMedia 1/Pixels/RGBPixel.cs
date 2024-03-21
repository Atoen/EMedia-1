namespace EMedia_1.Pixels;

public readonly struct RGBPixel(byte[] buffer, int offset = 0) : IPixel
{
    public byte R { get; } = buffer[offset];
    public byte G { get; } = buffer[offset + 1];
    public byte B { get; } = buffer[offset + 2];

    public string Print() => $"\x1b[48;2;{R};{G};{B}m  ";
    
    public double AverageValue => 0.299 * R + 0.587 * G + 0.114 * B;
}