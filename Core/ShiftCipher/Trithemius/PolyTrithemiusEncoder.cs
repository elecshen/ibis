using Core.Alphabet;

namespace Core.ShiftCipher.Trithemius
{
    public class PolyTrithemiusEncoder(IAlphabet alphabet) : ClassicTrithemiusEncoder(alphabet)
    {
        protected string Encode(string value, string key, int baseShift, int idleShift)
        {
            string output = "";
            var keyTable = GetKeyTable(key);
            // Холостой сдвиг (при срабатывании, зашифрованный текст не совпадает с примером)
            for (int i = 0; i < idleShift; i++)
                // Сдвигаем символ
                keyTable.ReplaceLastElement(idleShift);
            for (int i = 0; i < value.Length; i++)
            {
                // Сдвигаем символ
                keyTable.ReplaceLastElement(idleShift + i);
                // Добавляем символ, находяйся на 8 впереди (при кодировании) или позади (при расшифровке)
                output += keyTable[keyTable.IndexOf(value[i]) + baseShift];
            }
            return output;
        }

        /// <summary>
        /// Функция полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет зашифровано</param>
        /// <param name="key">Секрет используемы при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Зашиврованная строка</returns>
        public new string Encrypt(string value, string key, int idleShift = 0) => Encode(value.ToUpper(), key.ToUpper(), EncodingShift, idleShift);

        /// <summary>
        /// Функция полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет разшифровано</param>
        /// <param name="key">Секрет используемы при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Исходная строка</returns>
        public new string Decrypt(string value, string key, int idleShift = 0) => Encode(value.ToUpper(), key.ToUpper(), DecodingShift, idleShift);
    }
}
