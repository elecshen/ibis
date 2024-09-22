namespace Core.Alphabet
{
    public class RusAlphabet : IAlphabet
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

        public int Length => alphabet.Length;

        static RusAlphabet()
        {
            for (int i = 0; i < alphabet.Length; i++)
                charToNumber[alphabet[i]] = i + 1;
            charToNumber[alphabet[^1]] = 0;
        }

        public int this[char ch]
        {
            get => charToNumber[char.ToUpper(ch)];
        }

        public char this[int index]
        {
            get => alphabet[Utils.NormalizeIndex(index - 1, alphabet.Length)];
        }

        public IEnumerable<char> Except(IEnumerable<char> second) => alphabet.Except(second);
    }
}
