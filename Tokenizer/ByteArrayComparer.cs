namespace Tokenizer;

class ByteArrayComparer : IEqualityComparer<byte[]>
{
    public bool Equals(byte[] a, byte[] b) =>
        a.SequenceEqual(b);

    public int GetHashCode(byte[] a) =>
        HashCode.Combine(a.Length, a[0], a[^1]);
}
