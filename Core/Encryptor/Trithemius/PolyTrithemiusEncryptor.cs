using Core.Alphabet;

namespace Core.Encryptor.Trithemius
{
    public class PolyTrithemiusEncryptor<T>(IAlphabet alphabet) : ClassicTrithemiusEncryptor<T>(alphabet) where T : IAlphabet
    {
        protected void MakeIdleShift(ref CircularList<char> keyTable, int idleShift)
        {
            for (int i = 1; i < idleShift; i++)
                keyTable.ShiftElement(i, _alphabet.Length - 1);
        }

        protected string EncryptString(string value, string key, int tableShift, int idleShift)
        {
            string output = "";
            var keyTable = GetKeyTable(key);
            MakeIdleShift(ref keyTable, idleShift);
            for (int i = 0; i < value.Length; i++)
            {
                keyTable.ShiftElement(idleShift + i, _alphabet.Length - 1);
                output += EncryptChar(value[i], keyTable, tableShift);
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
        public override string Encrypt(string value, string key, int idleShift = 0) => EncryptString(value.ToUpper(), key.ToUpper(), EncodingShift, idleShift);

        /// <summary>
        /// Функция полиалфавитного шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет расшифровано</param>
        /// <param name="key">Секрет используемый при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Исходная строка</returns>
        public override string Decrypt(string value, string key, int idleShift = 0) => EncryptString(value.ToUpper(), key.ToUpper(), DecodingShift, idleShift);
    }
}
