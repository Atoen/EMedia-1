namespace EMedia_1;
public class Crc32
{
    private const uint Generator = 0xEDB88320;

    private readonly uint[] _checksumTable;
    
    public Crc32()
    {
        _checksumTable = Enumerable.Range(0, 256).Select(i =>
        {
            var tableEntry = (uint) i;
            for (var j = 0; j < 8; j++)
            {
                tableEntry = (tableEntry & 1) != 0
                    ? Generator ^ (tableEntry >> 1) 
                    : tableEntry >> 1;
            }
            return tableEntry;
        }).ToArray();
    }

    public uint Get(IEnumerable<byte> bytes)
    {
            return ~bytes.Aggregate(0xFFFFFFFF, (checksumRegister, currentByte) => 
                      _checksumTable[(checksumRegister & 0xFF) ^ currentByte] ^ (checksumRegister >> 8));
    }
}