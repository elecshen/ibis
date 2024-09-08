namespace Core.Alphabet
{
    public class Alphabet : IAlphabet
    {
        private static readonly char[] alphabet =
        [
            'А', 'Б', 'В', 'Г', 'Д',
            'Е', 'Ж', 'З', 'И', 'Й',
            'К', 'Л', 'М', 'Н', 'О',
            'П', 'Р', 'С', 'Т', 'У',
            'Ф', 'Х', 'Ц', 'Ч', 'Ш',
            'Щ', 'Ы', 'Ь', 'Э', 'Ю',
            'Я', '_'
        ];

        private static readonly Dictionary<char, int> charToNumber = [];
        private static readonly Dictionary<int, char> numberToChar = [];

        public int Length => alphabet.Length;

        static Alphabet()
        {
            for (int i = 0; i < alphabet.Length; i++)
            {
                charToNumber[alphabet[i]] = i + 1;
                numberToChar[i + 1] = alphabet[i];
            }
        }

        public int NormalizeIndex(int index) => index < 0 ? index % alphabet.Length + alphabet.Length : (index + alphabet.Length) % alphabet.Length;

        public int this[char ch]
        {
            get => charToNumber[char.ToUpper(ch)];
        }

        public char this[int index]
        {
            get => numberToChar[NormalizeIndex(index)];
        }

        public IEnumerable<char> Except(IEnumerable<char> second) => alphabet.Except(second);
    }
}
