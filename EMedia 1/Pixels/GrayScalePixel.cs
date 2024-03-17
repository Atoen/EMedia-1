namespace EMedia_1.Pixels;

public class GrayScalePixel(byte[] buffer, int offset = 0) : Pixel
{
    public byte Intensity { get; } = buffer[offset];
    
    public override string Print() => $"\x1b[48;2;{Intensity};{Intensity};{Intensity}m  ";
}