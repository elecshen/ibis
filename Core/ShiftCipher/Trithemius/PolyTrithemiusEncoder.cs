using Core.Alphabet;
using System.Diagnostics.Metrics;

namespace Core.ShiftCipher.Trithemius
{
    public class PolyTrithemiusEncoder(IAlphabet alphabet) : ClassicTrithemiusEncoder(alphabet)
    {
        protected void MakeIdleShift(ref CircularList<char> keyTable, int idleShift)
        {
            for (int i = 1; i < idleShift; i++)
                keyTable.ShiftElement(i, _alphabet.Length - 1);
        }

        public string EncryptText(string value, string key, int tableShift, int idleShift)
        {
            string output = "";
            var keyTable = GetKeyTable(key);
            MakeIdleShift(ref keyTable, idleShift);
            for (int i = 0; i < value.Length; i++)
            {
                keyTable.ShiftElement(idleShift + i, _alphabet.Length - 1);
                output += EncryptSym(value[i], keyTable, tableShift);
            }
            return output;
        }

        /// <summary>
        /// Функция полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет зашифровано</param>
        /// <param name="key">Секрет используемый при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Зашифрованная строка</returns>
        public override string Encrypt(string value, string key, int idleShift = 0) => EncryptText(value.ToUpper(), key.ToUpper(), EncodingShift, idleShift);

        /// <summary>
        /// Функция полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет расшифровано</param>
        /// <param name="key">Секрет используемый при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Исходная строка</returns>
        public override string Decrypt(string value, string key, int idleShift = 0) => EncryptText(value.ToUpper(), key.ToUpper(), DecodingShift, idleShift);
    }
}
