namespace Tokenizer;

class ByteArrayPairComparer : IEqualityComparer<(byte[] Left, byte[] Right)>
{
    private static readonly ByteArrayComparer byteComparer = new();

    public bool Equals((byte[] Left, byte[] Right) x, (byte[] Left, byte[] Right) y) =>
        byteComparer.Equals(x.Left, y.Left) && byteComparer.Equals(x.Right, y.Right);

    public int GetHashCode((byte[] Left, byte[] Right) obj)
    {
        unchecked
        {
            return (byteComparer.GetHashCode(obj.Left) * 397) ^
                   byteComparer.GetHashCode(obj.Right);
        }
    }
}

