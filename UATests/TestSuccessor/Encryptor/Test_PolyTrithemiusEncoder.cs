using Core;
using Core.Alphabet;
using Core.Encryptor.Trithemius;

namespace UATests.TestSuccessor.Encryptor
{
    public class Test_PolyTrithemiusEncoder<T>(IAlphabet alphabet) : PolyTrithemiusEncryptor<T>(alphabet) where T : IAlphabet
    {
        public new void MakeIdleShift(ref CircularList<char> keyTable, int idleShift) => base.MakeIdleShift(ref keyTable, idleShift);
        public new string EncryptString(string value, string key, int tableShift, int idleShift) => base.EncryptString(value, key, tableShift, idleShift);
    }
}
