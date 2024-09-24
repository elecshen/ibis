namespace Core.Alphabet
{
    public interface IAlphabetModifier<T> where T :IAlphabet
    {
        public string NumsToText(IEnumerable<int> nums);
        public char SubChar(char c1, char c2);
        public string SubString(string s1, string s2);
        public char SumChar(char c1, char c2);
        public string SumString(string s1, string s2);
        public int[] TextToNums(string s);
        public int TextToBaseNum(string value, int baseNum = -1);
        public string BaseNumToText(long num, int baseNum = -1);
    }
}