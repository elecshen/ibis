using Core;
using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace UATests.TestSuccessor
{
    public class TestClassicTrithemiusEncoder<T>(IAlphabet alphabet) : ClassicTrithemiusEncoder<T>(alphabet) where T : IAlphabet
    {
        public CircularList<char> TestGetKeyTable(string key) => GetKeyTable(key);
        public CircularList<char> TestGetKeyTable(IEnumerable<char> key) => GetKeyTable(key);
        public char TestEncryptSym(char c, CircularList<char> keyTable, int shift) => EncryptSym(c, keyTable, shift);
        public string TestEncryptText(string value, string key, int tableShift) => EncryptText(value, key, tableShift);
    }
}
