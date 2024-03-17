using EMedia_1.Chunks;

namespace EMedia_1.Pixels;

public abstract class Pixel
{
    public abstract string Print();

    public static Pixel Create(ColorType colorType, byte[] buffer, int offset = 0)
    {
        return colorType switch
        {
            ColorType.Grayscale => new GrayScalePixel(buffer, offset),
            ColorType.RGB => new RGBPixel(buffer, offset),
            ColorType.RGBA => new RGBAPixel(buffer, offset),
            _ => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }
}