using Core.Alphabet;

namespace Core.ShiftCipher.Trithemius
{
    public class SBlockModPolyTrithemiusEncoder<T>(T alphabet, IAlphabetModifier<T> alphabetModifier) : PolyTrithemiusEncoder(alphabet) where T : IAlphabet
    {
        private readonly IAlphabetModifier<T> _modifier = alphabetModifier;

        public string ImproveBlock(string value, string key, int idleShift, bool isEncode)
        {
            // Сдвигаем ключ на холостой сдвиг
            while (key.Length < value.Length)
                key += key;
            idleShift = idleShift % key.Length;
            key += key[..idleShift];
            key = key.Substring(idleShift, value.Length);

            var keyNums = _modifier.TextToNums(key);
            var blockNums = _modifier.TextToNums(value);
            var q = keyNums.Sum() % value.Length;
            // Перемешиваем символы
            if (isEncode)
                for (int i = 0; i < value.Length - 1; i++)
                    blockNums[(q + i + 1) % value.Length] = _alphabet.NormalizeIndex(blockNums[(q + i + 1) % value.Length] + blockNums[(q + i) % value.Length]);
            else
                for (int i = value.Length - 2; i >= 0; i--)
                    blockNums[(q + i + 1) % value.Length] = _alphabet.NormalizeIndex(blockNums[(q + i + 1) % value.Length] - blockNums[(q + i) % value.Length]);
            return _modifier.NumsToText(blockNums);
        }

        /// <summary>
        /// Функция усиленного блочного полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет зашифровано</param>
        /// <param name="key">Секрет используемы при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Зашифрованная строка</returns>
        public new string Encrypt(string value, string key, int idleShift = 0)
        {
            if (value.Length != 4)
                return "input_error";
            key = key.ToUpper();
            string output = Encode(value.ToUpper(), key, EncodingShift, idleShift);
            output = ImproveBlock(output, key, idleShift, true);
            return output; 
        }

        /// <summary>
        /// Функция усиленного блочного полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет разшифровано</param>
        /// <param name="key">Секрет используемы при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Исходная строка</returns>
        public new string Decrypt(string value, string key, int idleShift = 0)
        {
            if (value.Length != 4)
                return "input_error";
            key = key.ToUpper();
            string output = ImproveBlock(value.ToUpper(), key, idleShift, false);
            output = Encode(output, key, DecodingShift, idleShift);
            return output;
        }
    }
}
