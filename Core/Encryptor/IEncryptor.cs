using Core.Alphabet;

namespace Core.Encryptor
{
    public interface IEncryptor<T> where T : IAlphabet
    {
        public string Encrypt(string value, string key, int idleShift = 0);

        public string Decrypt(string value, string key, int idleShift = 0);
    }
}
