using Core.Alphabet;

namespace Core.Encryptor.Trithemius
{
    public class ExtSBlockModPolyTrithemiusEncryptor<T>(T alphabet, IAlphabetModifier<T> alphabetModifier)
        : SBlockModPolyTrithemiusEncryptor<T>(alphabet, alphabetModifier), IExtendedEncryptor<T> where T : IAlphabet
    {
        protected string GetKeyTableString(string key) => string.Join("", GetKeyTable(key.ToUpper()));

        string IExtendedEncryptor<T>.Oneside(string value, string key, int steps)
        {
            value = value.ToUpper();
            key = key.ToUpper() + value;
            for (int i = 0; i < steps; i++)
            {
                value = Encrypt(value, key);
                key = value + GetKeyTableString(key);
            }
            return value;
        }
    }
}
