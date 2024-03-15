namespace EMedia_1;

public record ImageData(
    uint Width,
    uint Height,
    byte BitDepth,
    byte ColorType,
    byte CompressionMethod,
    byte FilterMethod,
    byte InterlaceMethod);