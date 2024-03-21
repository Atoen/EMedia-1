using EMedia_1.Chunks;

namespace EMedia_1.Pixels;

public interface IPixel
{
    static IPixel Create(ColorType colorType, byte[] buffer, int offset = 0)
    {
        return colorType switch
        {
            ColorType.Grayscale => new GrayScalePixel(buffer, offset),
            ColorType.RGB => new RGBPixel(buffer, offset),
            ColorType.RGBA => new RGBAPixel(buffer, offset),
            _ => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }

    string Print();
    
    double AverageValue { get; }
}
