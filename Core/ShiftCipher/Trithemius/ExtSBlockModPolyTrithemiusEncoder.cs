using Core.Alphabet;

namespace Core.ShiftCipher.Trithemius
{
    public class ExtSBlockModPolyTrithemiusEncoder<T>(T alphabet, IAlphabetModifier<T> alphabetModifier) 
        : SBlockModPolyTrithemiusEncoder<T>(alphabet, alphabetModifier), IExtendedEncoder<T> where T : IAlphabet
    {
        string IExtendedEncoder<T>.GetKeyTable(string key) => string.Join("", GetKeyTable(key.ToUpper()));
    }
}
