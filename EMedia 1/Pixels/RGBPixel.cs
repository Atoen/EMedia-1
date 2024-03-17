namespace EMedia_1.Pixels;

public class RGBPixel(byte[] buffer, int offset = 0) : Pixel
{
    public byte R { get; } = buffer[offset];
    public byte G { get; } = buffer[offset + 1];
    public byte B { get; } = buffer[offset + 2];

    public override string Print() => $"\x1b[48;2;{R};{G};{B}m  ";
}