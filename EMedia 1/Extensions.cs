using System.Buffers.Binary;

namespace EMedia_1;

public static class Extensions
{
    public static uint GetUint(this Span<byte> span)
    {
        return BinaryPrimitives.ReadUInt32BigEndian(span);
    }

    public static double GetFixedPoint(this Span<byte> span)
    {
        var num = BinaryPrimitives.ReadUInt32BigEndian(span);
        return num / 100000.0;
    }
    
    public static uint GetUint(this byte[] array)
    {
        return BinaryPrimitives.ReadUInt32BigEndian(array);
    }

    public static byte[] ReadBytes(this Stream stream, uint length)
    {
        var buffer = new byte[length];
        var bytesRead = stream.Read(buffer);
        if (bytesRead != length)
        {
            throw new InvalidOperationException("Failed to read requested amount");
        }

        return buffer;
    }
}