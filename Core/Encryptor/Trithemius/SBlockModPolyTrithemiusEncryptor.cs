using Core.Alphabet;

namespace Core.Encryptor.Trithemius
{
    public class SBlockModPolyTrithemiusEncryptor<T>(T alphabet, IAlphabetModifier<T> alphabetModifier) : PolyTrithemiusEncryptor<T>(alphabet) where T : IAlphabet
    {
        protected readonly IAlphabetModifier<T> _modifier = alphabetModifier;

        protected string? Check4Sym(string value)
        {
            if (value.Length != 4)
                return "input_error";
            return null;
        }

        protected string ImproveBlock(string value, string key, int idleShift, bool isEncode)
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
                    blockNums[(q + i + 1) % value.Length] = Utils.NormalizeIndex(blockNums[(q + i + 1) % value.Length] + blockNums[(q + i) % value.Length], _alphabet.Length);
            else
                for (int i = value.Length - 2; i >= 0; i--)
                    blockNums[(q + i + 1) % value.Length] = Utils.NormalizeIndex(blockNums[(q + i + 1) % value.Length] - blockNums[(q + i) % value.Length], _alphabet.Length);
            return _modifier.NumsToText(blockNums);
        }

        /// <summary>
        /// Функция усиленного блочного полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет зашифровано</param>
        /// <param name="key">Секрет используемый при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Зашифрованная строка</returns>
        public override string Encrypt(string value, string key, int idleShift = 0)
        {
            var res = Check4Sym(value);
            if (res is not null) return res;

            key = key.ToUpper();
            string output = EncryptString(value.ToUpper(), key, EncodingShift, idleShift);
            output = ImproveBlock(output, key, idleShift, true);
            return output;
        }

        /// <summary>
        /// Функция усиленного блочного полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет расшифровано</param>
        /// <param name="key">Секрет используемый при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Исходная строка</returns>
        public override string Decrypt(string value, string key, int idleShift = 0)
        {
            var res = Check4Sym(value);
            if (res is not null) return res;

            key = key.ToUpper();
            string output = ImproveBlock(value.ToUpper(), key, idleShift, false);
            output = EncryptString(output, key, DecodingShift, idleShift);
            return output;
        }
    }
}
