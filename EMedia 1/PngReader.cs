namespace EMedia_1;

public class PngReader(Stream stream)
{
    public static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
    public static int SignatureLength => PngSignature.Length;

    public async Task ReadAsync()
    {
        var isPng = await CheckSignatureAsync();
        Console.WriteLine(isPng ? "File is png" : "File is not png");
    }

    private async Task<bool> CheckSignatureAsync()
    {
        var buffer = new byte[SignatureLength];
        var bytesRead = await stream.ReadAsync(buffer.AsMemory(0, SignatureLength));

        return buffer.Take(bytesRead)
            .SequenceEqual(PngSignature);
    }
}