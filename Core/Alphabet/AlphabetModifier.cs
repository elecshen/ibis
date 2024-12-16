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

        public long TextToNumWithAlphabetBase(string value)
        {
            int baseNum = _alphabet.Length;
            long res = 0;
            int pos = 1;
            foreach (var ch in value.Reverse())
            {
                res += pos * _alphabet[ch];
                pos *= baseNum;
            }
            return res;
        }

        public string NumWithAlphabetBaseToText(long num)
        {
            int baseNum = _alphabet.Length;
            string res = "";
            for (int i = 0; i < 4; i++)
            {
                res = _alphabet[(int)num % baseNum] + res;
                num /= baseNum;
            }
            return res;
        }

        public IEnumerable<bool> NumToBin(int num) => Utils.NumToBin(num, _alphabet.GetSignificantBitPos());

        public int BinToNum(IEnumerable<bool> bits) => Utils.BinToNum(bits, _alphabet.GetSignificantBitPos());

        public IEnumerable<bool> TextToBin(string value)
        {
            int endingStartIdx = value.IndexOfAny(['1', '0']);
            IEnumerable<bool> bits;
            if (endingStartIdx == -1)
            {
                bits = TextToNums(value).SelectMany(NumToBin);
                return bits;
            }
            else
            {
                bits = TextToNums(value[..endingStartIdx]).SelectMany(NumToBin);
                return bits.Concat(value[endingStartIdx..].Select(ch => ch == '1'));
            }
        }

        public string BinToText(IEnumerable<bool> bits)
        {
            string res = "";
            var chunks = bits.Chunk(_alphabet.GetSignificantBitPos());
            if (chunks.Last().Length != _alphabet.GetSignificantBitPos())
            {
                res = NumsToText(chunks.SkipLast(1).Select(BinToNum));
                res += string.Join("", chunks.Last().Select(b => b ? '1' : '0'));
            }
            else
                res = NumsToText(chunks.Select(BinToNum));
            return res;
        }

        public string Xor(string str1, string str2)
        {
            var nums1 = TextToNums(str1);
            var nums2 = TextToNums(str2);
            for (int i = 0; i < nums1.Length; i++)
                nums1[i] = nums1[i] ^ nums2[i];
            return NumsToText(nums1);
        }

        public string BinaryTextShift(string str, int shift)
        {
            var bits = new CircularList<bool>(TextToBin(str));
            var res = new int[str.Length];
            for (var i = 0; i < str.Length; i++)
                res[i] = BinToNum(bits.GetRange(i * Alphabet.GetSignificantBitPos() - shift, Alphabet.GetSignificantBitPos()));
            return NumsToText(res);
        }
    }
}
