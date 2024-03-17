using System.Text;
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

    public PngImage(List<PngChunk> chunks)
    {
        if (chunks[0] is not IHDRChunk header)
        {
            throw new InvalidOperationException();
        }
        
        Header = header;
        Chunks = chunks;

        SetConsoleMode();

        try
        {
            Read();
        }
        finally
        {
            Console.ResetColor();
        }
    }

    private void Read()
    {
        var builder = new StringBuilder();
        var filter = new PngFilter(Width, ColorType, BitDepth);
        var data = Chunks.Where(x => x is IDATChunk)
            .SelectMany(x => x.Data)
            .Skip(2);

        var decompressed = PngChunk.Decompress(data.ToArray());
        
        var rows = decompressed.Chunk(Width * filter.PixelWidth + 1);
        foreach (var row in rows)
        {
            var decoded = filter.Decode(row);
            
            for (var i = 0; i < decoded.Length; i += filter.PixelWidth)
            {
                var pixel = Pixel.Create(ColorType, decoded, i);
                builder.Append(pixel.Print());
            }

            Console.WriteLine(builder);
            builder.Clear();
        }
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
