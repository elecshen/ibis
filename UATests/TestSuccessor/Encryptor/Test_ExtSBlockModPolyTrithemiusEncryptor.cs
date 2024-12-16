using Core.Alphabet;
using Core.Encryptor.Trithemius;

namespace UATests.TestSuccessor.Encryptor
{
    public class Test_ExtSBlockModPolyTrithemiusEncryptor<T>(T alphabet, IAlphabetModifier<T> alphabetModifier)
        : ExtSBlockModPolyTrithemiusEncryptor<T>(alphabet, alphabetModifier) where T : IAlphabet
    {
        public new string GetKeyTableString(string key) => base.GetKeyTableString(key);
    }
}
