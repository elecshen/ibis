namespace Core.Alphabet
{
    public interface IAlphabetModifier<T> where T :IAlphabet
    {
        public T Alphabet { get; }
        public string NumsToText(IEnumerable<int> nums);
        public char SubChar(char c1, char c2);
        public string SubString(string s1, string s2);
        public char SumChar(char c1, char c2);
        public string SumString(string s1, string s2);
        public int[] TextToNums(string s);
        public long TextToBaseNum(string value, int baseNum = -1);
        public string BaseNumToText(long num, int baseNum = -1);
        public IEnumerable<bool> NumToBin(int num, int significantBitPos = -1);
        public int BinToNum(IEnumerable<bool> bits, int significantBitPos = -1);
    }
}