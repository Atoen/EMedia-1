using EMedia_1.Chunks;

namespace EMedia_1;

public class PngReader(Stream stream)
{
    public static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
    public static int SignatureLength => PngSignature.Length;

    public ImageData Read()
    {
        var isPng = CheckSignature();
        Console.WriteLine(isPng ? "File is png" : "File is not png");

        var chunks = new List<PngChunk>();

        PngChunk chunk;
        do
        {
            chunk = PngChunk.Create(stream);
            chunk.PrintData();
            chunks.Add(chunk);
        } while (chunk is not IENDChunk);

        Console.WriteLine($"Chunks total: {chunks.Count}");

        return new ImageData(chunks);
    }

    public static void Anonymize(ImageData imageData, string resultFilename)
    {
        try
        {
            using var stream = new FileStream(resultFilename, FileMode.CreateNew);
            stream.Write(PngSignature);
            
            foreach (var chunk in imageData.Chunks.Where(x => !x.RemoveWhenAnonymizing))
            {
                chunk.AppendToStream(stream);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

private bool CheckSignature()
    {
        var buffer = new byte[SignatureLength];
        var bytesRead = stream.Read(buffer);

        return buffer.Take(bytesRead)
            .SequenceEqual(PngSignature);
    }
}