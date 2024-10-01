using Core;
using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace UATests.TestSuccessor
{
    public class TestPolyTrithemiusEncoder<T>(IAlphabet alphabet) : PolyTrithemiusEncoder<T>(alphabet) where T : IAlphabet
    {
        public void TestMakeIdleShift(ref CircularList<char> keyTable, int idleShift) => MakeIdleShift(ref keyTable, idleShift);
        public string TestEncryptText(string value, string key, int tableShift, int idleShift) => EncryptText(value, key, tableShift, idleShift);
    }
}
