namespace Core.ShiftCipher
{
    public interface IEncoder
    {
        public string Encrypt(string value, string key, int idleShift = 0);

        public string Decrypt(string value, string key, int idleShift = 0);
    }
}
