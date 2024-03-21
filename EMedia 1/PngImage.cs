using EMedia_1.Chunks;
using EMedia_1.Filters;
using EMedia_1.Pixels;

namespace EMedia_1;

public class PngImage
{
    public List<PngChunk> Chunks { get; }

    public IHDRChunk Header { get; }

    public int Width => (int) Header.Width;
    public int Height => (int) Header.Height;
    public ColorType ColorType => Header.ColorType;
    public BitDepth BitDepth => Header.BitDepth;
    
    public IPixel[,] Pixels { get; }

    public PngImage(List<PngChunk> chunks)
    {
        if (chunks[0] is not IHDRChunk header)
        {
            throw new InvalidOperationException();
        }
        
        Header = header;
        Chunks = chunks;
        
        Pixels = ReadData();
    }

    private IPixel[,] ReadData()
    {
        var pixels = new IPixel[Height, Width];

        var filter = new PngFilter(Width, ColorType, BitDepth);
        var data = Chunks
            .Where(x => x is IDATChunk)
            .SelectMany(x => x.Data)
            .Skip(2)
            .ToArray();

        var decompressed = PngChunk.Decompress(data);
        var rows = decompressed.Chunk(Width * filter.PixelWidth + 1).ToArray();

        for (var i = 0; i < rows.Length; i++)
        {
            var row = rows[i];
            var decoded = filter.Decode(row);
            
            for (int j = 0, k = 0; k < decoded.Length; j++, k += filter.PixelWidth)
            {
                pixels[i, j] = IPixel.Create(ColorType, decoded, k);
            }
        }

        return pixels;
    }

    private void SetConsoleMode()
    {
        var mode = 0u;
        var handle = NativeConsole.HandleOut;
        if (handle == (nint) (-1L) || !NativeConsole.GetConsoleMode(handle, ref mode))
        {
            throw new InvalidOperationException("Unable to set console mode");
        }

        NativeConsole.SetConsoleMode(handle, mode | 4U);
    }
}
