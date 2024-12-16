using Core.Alphabet;

namespace Core.CombinedEncryptor
{
    public interface ICombinedEncryptor<T> where T : IAlphabet
    {
        string Encrypt(string str, string key, int rounds);
        string Decrypt(string str, string key, int rounds);
    }
}