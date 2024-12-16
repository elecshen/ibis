using Core.Alphabet;
using Core.Encryptor.Trithemius;

namespace UATests.TestSuccessor.Encryptor
{
    public class Test_SBlockModPolyTrithemiusEncoder<T>(T alphabet, IAlphabetModifier<T> alphabetModifier)
        : SBlockModPolyTrithemiusEncryptor<T>(alphabet, alphabetModifier) where T : IAlphabet
    {
        public new string ImproveBlock(string value, string key, int idleShift, bool isEncode) => base.ImproveBlock(value, key, idleShift, isEncode);
    }
}
