using Core.Alphabet;

namespace Core.ShiftCipher.Trithemius
{
    public class ClassicTrithemiusEncoder : IEncoder
    {
        protected readonly IAlphabet _alphabet;

        protected readonly int EncodingShift;
        protected readonly int DecodingShift;

        public ClassicTrithemiusEncoder(IAlphabet alphabet)
        {
            _alphabet = alphabet;
            EncodingShift = 8;
            DecodingShift = _alphabet.Length - 8;
        }

        public CircularList<char> GetKeyTable(string key)
        {
            var keyTable = new CircularList<char>();
            foreach (char letter in key.ToCharArray())
            {
                if (!keyTable.Contains(letter))
                {
                    keyTable.Add(letter);
                }
            }
            var exp = _alphabet.Except(keyTable);
            keyTable.AddRange(exp);
            return keyTable;
        }

        protected string Encode(string value, string key, int shift)
        {
            string output = "";
            var keyTable = GetKeyTable(key);
            foreach (var letter in value)
            {
                output += keyTable[keyTable.IndexOf(letter) + shift];
            }
            return output;
        }

        public string Encrypt(string value, string key, int idleShift = 0) => Encode(value.ToUpper(), key.ToUpper(), EncodingShift);

        public string Decrypt(string value, string key, int idleShift = 0) => Encode(value.ToUpper(), key.ToUpper(), DecodingShift);
    }
}
