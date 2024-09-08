namespace Core
{
    public class TrithemusEncoder : IEncoder
    {
        public readonly int EncodingShift = 8;
        public readonly int DecodingShift = AlphabetConverter.Alphabet.Length - 8;

        private List<char> GetShiftTable(string key)
        {
            var shiftTable = new List<char>();
            foreach (char letter in key.ToCharArray())
            {
                if (!shiftTable.Contains(letter))
                {
                    shiftTable.Add(letter);
                }
            }
            var exp = AlphabetConverter.Alphabet.Except(shiftTable);
            shiftTable.AddRange(exp);
            return shiftTable;
        }

        private string Encode(string value, string key, int shift)
        {
            string output = "";
            var shiftTable = GetShiftTable(key);
            int pos;
            foreach (var letter in value)
            {
                pos = (shiftTable.IndexOf(letter) + shift) % AlphabetConverter.Alphabet.Length;
                output += shiftTable[pos];
            }
            return output;
        }

        public string Encrypt(string value, string key) => Encode(value.ToUpper(), key.ToUpper(), EncodingShift);

        public string Decrypt(string value, string key) => Encode(value.ToUpper(), key.ToUpper(), DecodingShift);
    }
}
