namespace Core.Alphabet
{
    public class AlphabetModifier<T>(T alphabet) : IAlphabetModifier<T> where T : IAlphabet
    {
        private readonly T _alphabet = alphabet;
        public T Alphabet { get { return _alphabet; } }

        public char SumChar(char c1, char c2) => _alphabet[_alphabet[c1] + _alphabet[c2]];

        public char SubChar(char c1, char c2) => _alphabet[_alphabet[c1] - _alphabet[c2]];

        public string SumString(string s1, string s2)
        {
            if (s1.Length < s2.Length)
                (s1, s2) = (s2, s1);
            string output = "";
            foreach ((var i, var item) in s1.Select((v, i) => (i, v)))
            {
                if (i >= s2.Length)
                    output += item;
                else
                    output += SumChar(item, s2[i]);
            }
            return output;
        }

        public string SubString(string s1, string s2)
        {
            if (s1.Length < s2.Length)
                (s1, s2) = (s2, s1);
            string output = "";
            foreach ((var i, var item) in s1.Select((v, i) => (i, v)))
            {
                if (i >= s2.Length)
                    output += item;
                else
                    output += SubChar(item, s2[i]);
            }
            return output;
        }

        public int[] TextToNums(string s) => s.Select(c => _alphabet[c]).ToArray();

        public string NumsToText(IEnumerable<int> nums) => string.Join("", nums.Select(c => _alphabet[c]));

        public long TextToBaseNum(string value, int baseNum = -1)
        {
            if(baseNum == -1) 
                baseNum = _alphabet.Length;
            long res = 0;
            int pos = 1;
            foreach (var ch in value.Reverse())
            {
                res += pos * _alphabet[ch];
                pos *= baseNum;
            }
            return res;
        }

        public string BaseNumToText(long num, int baseNum = -1)
        {
            if (baseNum == -1) 
                baseNum = _alphabet.Length;
            string res = "";
            for (int i = 0; i < 4; i++)
            {
                res = _alphabet[(int)num % baseNum] + res;
                num /= baseNum;
            }
            return res;
        }

        public IEnumerable<bool> NumToBin(int num, int significantBitPos = -1)
        {
            if (significantBitPos == -1)
                significantBitPos = _alphabet.GetSignificantBitPos();
            for (var i = significantBitPos - 1; i >= 0; i--)
                yield return (num & (1 << i)) != 0;
        }

        public int BinToNum(IEnumerable<bool> bits, int significantBitPos = -1)
        {
            if (significantBitPos == -1)
                significantBitPos = _alphabet.GetSignificantBitPos();
            int res = 0;
            for (var i = 0; i < significantBitPos; i++)
                if (bits.ElementAt(i))
                    res |= 1 << (significantBitPos - i - 1);
            return res;
        }
    }
}
