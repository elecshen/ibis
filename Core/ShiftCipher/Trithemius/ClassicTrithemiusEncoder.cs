using Core.Alphabet;

namespace Core.ShiftCipher.Trithemius
{
    public class ClassicTrithemiusEncoder : IEncoder
    {
        protected readonly IAlphabet _alphabet;

        public readonly int EncodingShift;
        public readonly int DecodingShift;

        public ClassicTrithemiusEncoder(IAlphabet alphabet)
        {
            _alphabet = alphabet;
            EncodingShift = 8;
            DecodingShift = _alphabet.Length - 8;
        }

        protected CircularList<char> GetShiftTable(string key)
        {
            var shiftTable = new CircularList<char>();
            foreach (char letter in key.ToCharArray())
            {
                if (!shiftTable.Contains(letter))
                {
                    shiftTable.Add(letter);
                }
            }
            var exp = _alphabet.Except(shiftTable);
            shiftTable.AddRange(exp);
            return shiftTable;
        }

        protected string Encode(string value, string key, int shift)
        {
            string output = "";
            var shiftTable = GetShiftTable(key);
            foreach (var letter in value)
            {
                output += shiftTable[shiftTable.IndexOf(letter) + shift];
            }
            return output;
        }

        public string Encrypt(string value, string key) => Encode(value.ToUpper(), key.ToUpper(), EncodingShift);

        public string Decrypt(string value, string key) => Encode(value.ToUpper(), key.ToUpper(), DecodingShift);
    }
}
