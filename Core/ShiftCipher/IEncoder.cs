using Core.Alphabet;

namespace Core.ShiftCipher
{
    public interface IEncoder<T> where T : IAlphabet
    {
        public string Encrypt(string value, string key, int idleShift = 0);

        public string Decrypt(string value, string key, int idleShift = 0);
    }
}
