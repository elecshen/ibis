using Core;
using Core.Alphabet;
using Core.Encryptor.Trithemius;

namespace UATests.TestSuccessor.Encryptor
{
    public class Test_ClassicTrithemiusEncoder<T>(IAlphabet alphabet) : ClassicTrithemiusEncryptor<T>(alphabet) where T : IAlphabet
    {
        public new CircularList<char> GetKeyTable(string key) => base.GetKeyTable(key);
        public new CircularList<char> GetKeyTable(IEnumerable<char> key) => base.GetKeyTable(key);
        public new char EncryptChar(char c, CircularList<char> keyTable, int shift) => base.EncryptChar(c, keyTable, shift);
        public new string EncryptString(string value, string key, int tableShift) => base.EncryptString(value, key, tableShift);
    }
}
