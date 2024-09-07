namespace Core
{
    public static class AlphabetConverter
    {
        public static readonly char[] Alphabet =
        [
            'А', 'Б', 'В', 'Г', 'Д',
            'Е', 'Ж', 'З', 'И', 'Й',
            'К', 'Л', 'М', 'Н', 'О',
            'П', 'Р', 'С', 'Т', 'У',
            'Ф', 'Х', 'Ц', 'Ч', 'Ш',
            'Щ', 'Ы', 'Ь', 'Э', 'Ю',
            'Я', ' '
        ];

        private static readonly Dictionary<char, int> charToNumber = [];
        private static readonly Dictionary<int, char> numberToChar = [];

        static AlphabetConverter()
        {
            for (int i = 0; i < Alphabet.Length; i++)
            {
                charToNumber[Alphabet[i]] = i + 1;
                numberToChar[i + 1] = Alphabet[i];
            }
        }

        public static int ConvertCharToNumber(char c)
        {
            c = char.ToUpper(c);
            return charToNumber.TryGetValue(c, out int number) ? number : -1; // Возвращаем -1, если символ не найден
        }

        public static char ConvertNumberToChar(int number)
        {
            return numberToChar.TryGetValue(number, out char c) ? c : '\0'; // Возвращаем '\0', если число не найдено
        }

        public static char SumSym(char c1, char c2)
        {
            int sum = ConvertCharToNumber(c1) + ConvertCharToNumber(c2);
            return ConvertNumberToChar(sum % Alphabet.Length);
        }

        public static char SubSym(char c1, char c2)
        {
            int sub = ConvertCharToNumber(c1) - ConvertCharToNumber(c2);
            return ConvertNumberToChar((sub + Alphabet.Length) % Alphabet.Length);
        }
    }
}
