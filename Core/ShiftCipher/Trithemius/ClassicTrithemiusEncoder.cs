using Core.Alphabet;

namespace Core.ShiftCipher.Trithemius
{
    public class ClassicTrithemiusEncoder : IEncoder
    {
        protected readonly IAlphabet _alphabet;

        protected readonly int EncodingShift;
        protected readonly int DecodingShift;

        /// <param name="alphabet">Алфавит на основе, которого будет производиться шифрование.</param>
        public ClassicTrithemiusEncoder(IAlphabet alphabet)
        {
            _alphabet = alphabet;
            EncodingShift = 8;
            DecodingShift = _alphabet.Length - 8;
        }

        protected CircularList<char> GetKeyTable(string key) => GetKeyTable(key.ToCharArray());

        protected CircularList<char> GetKeyTable(IEnumerable<char> key)
        {
            var keyTable = new CircularList<char>();
            // Вставляем уникальные символы из ключа
            foreach (char letter in key)
            {
                if (!keyTable.Contains(letter))
                    keyTable.Add(letter);
            }
            // Вставляем остальные символы алфавита
            var exp = _alphabet.Except(keyTable);
            keyTable.AddRange(exp);
            return keyTable;
        }

        protected char EncryptSym(char c, CircularList<char> keyTable, int shift)
        {
            return keyTable[keyTable.IndexOf(c) + shift];
        }

        protected string EncryptText(string value, string key, int tableShift)
        {
            string output = "";
            var keyTable = GetKeyTable(key);
            foreach (var letter in value)
            {
                // Добавляем символ, находяйся на 8 впереди (при шифровании) или позади (при расшифровании)
                output += EncryptSym(letter, keyTable, tableShift);
            }
            return output;
        }

        /// <summary>
        /// Функция простого шифра Тритемиуса. Значение idleShift игнорируется.
        /// </summary>
        /// <param name="value">Значение, которое будет зашифровано</param>
        /// <param name="key">Секрет используемый при шифровании</param>
        /// <param name="idleShift">Игнорируется</param>
        /// <returns>Зашифрованная строка</returns>
        public virtual string Encrypt(string value, string key, int idleShift = 0) => EncryptText(value.ToUpper(), key.ToUpper(), EncodingShift);

        /// <summary>
        /// Функция простого шифра Тритемиуса. Значение idleShift игнорируется.
        /// </summary>
        /// <param name="value">Значение, которое будет расшифровано</param>
        /// <param name="key">Секрет используемый при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Исходная строка</returns>
        public virtual string Decrypt(string value, string key, int idleShift = 0) => EncryptText(value.ToUpper(), key.ToUpper(), DecodingShift);
    }
}
