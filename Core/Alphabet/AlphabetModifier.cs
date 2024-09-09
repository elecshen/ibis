namespace Core.Alphabet
{
    public class AlphabetModifier(IAlphabet alphabet)
    {
        private readonly IAlphabet _alphabet = alphabet;

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
    }
}
