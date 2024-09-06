namespace Core
{
    public class AlphabetConverter
    {
        private static readonly char[] alphabet =
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
            for (int i = 0; i < alphabet.Length; i++)
            {
                charToNumber[alphabet[i]] = i + 1;
                numberToChar[i + 1] = alphabet[i];
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
    }
}
