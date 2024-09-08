namespace Core.ShiftCipher
{
    public interface IEncoder
    {
        public string Encrypt(string value, string key);

        public string Decrypt(string value, string key);
    }
}
