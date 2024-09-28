using Core.Alphabet;

namespace Core.ShiftCipher
{
    public interface IExtendedEncoder<T> : IEncoder<T> where T : IAlphabet
    {
        public string GetKeyTable(string key);
    }
}
