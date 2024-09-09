using Core.Alphabet;

namespace Core.ShiftCipher.Trithemius
{
    public class SBlockModPolyTrithemiusEncoder(IAlphabet alphabet) : PolyTrithemiusEncoder(alphabet)
    {
        // Результат преобразования не совпадает с примерами
        public string ImproveBlock(string value, string key, int idleShift)
        {
            // Сдвигаем ключ на холостой сдвиг
            idleShift = idleShift % value.Length;
            key += key[..idleShift];
            key = key.Substring(idleShift, value.Length);

            AlphabetModifier modifier = new(_alphabet);
            var keyNums = modifier.TextToNums(key);
            var blockNums = modifier.TextToNums(value);
            var q = keyNums.Sum() % value.Length;
            for (int i = 0; i < value.Length - 1; i++)
                blockNums[(q + i) % value.Length] = _alphabet.NormalizeIndex(blockNums[(q + i + 1) % value.Length] + blockNums[(q + i) % value.Length]);
            return modifier.NumsToText(blockNums);
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
            output = ImproveBlock(output, key, idleShift);
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
            string output = Encode(value.ToUpper(), key, DecodingShift, idleShift);
            output = ImproveBlock(output, key, idleShift);
            return output;
        }
    }
}
