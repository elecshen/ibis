using Core.Alphabet;

namespace Core.ShiftCipher.Trithemius
{
    public class PolyTrithemiusEncoder(IAlphabet alphabet) : ClassicTrithemiusEncoder(alphabet)
    {
        protected string Encode(string value, string key, int baseShift, int idleShift)
        {
            string output = "";
            var keyTable = GetKeyTable(key);
            for (int i = 0; i < idleShift; i++)
                keyTable.ReplaceLastElement(idleShift);
            for (int i = 0; i < value.Length; i++)
            {
                keyTable.ReplaceLastElement(idleShift + i);
                output += keyTable[keyTable.IndexOf(value[i]) + baseShift];
            }
            return output;
        }

        public new string Encrypt(string value, string key, int idleShift = 0) => Encode(value.ToUpper(), key.ToUpper(), EncodingShift, idleShift);

        public new string Decrypt(string value, string key, int idleShift = 0) => Encode(value.ToUpper(), key.ToUpper(), DecodingShift, idleShift);
    }
}
