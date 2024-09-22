using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace UATests.TestSuccessor
{
    public class TestSBlockModPolyTrithemiusEncoder<T>(T alphabet, IAlphabetModifier<T> alphabetModifier) : SBlockModPolyTrithemiusEncoder<T>(alphabet, alphabetModifier) where T : IAlphabet
    {
        public string TestImproveBlock(string value, string key, int idleShift, bool isEncode) => ImproveBlock(value, key, idleShift, isEncode);
    }
}
