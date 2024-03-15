namespace EMedia_1.Chunks;

public class ImageData
{
    public List<PngChunk> Chunks { get; }

    public ImageData(List<PngChunk> chunks)
    {
        Chunks = chunks;
    }
}