namespace Core.ShiftCipher
{
    public interface IExtendedEncoder : IEncoder
    {
        public string GetKeyTable(string key);
    }
}
