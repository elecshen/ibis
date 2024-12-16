using Core.Alphabet;

namespace Core.Encryptor
{
    public interface IExtendedEncryptor<T> : IEncryptor<T> where T : IAlphabet
    {
        public string Oneside(string value, string key, int steps);
    }
}
