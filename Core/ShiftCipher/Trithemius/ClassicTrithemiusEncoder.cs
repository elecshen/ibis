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

        public CircularList<char> GetKeyTable(string key)
        {
            var keyTable = new CircularList<char>();
            // Вставляем уникальные символы из ключа
            foreach (char letter in key.ToCharArray())
            {
                if (!keyTable.Contains(letter))
                {
                    keyTable.Add(letter);
                }
            }
            // Вставляем остальные символы алфавита
            var exp = _alphabet.Except(keyTable);
            keyTable.AddRange(exp);
            return keyTable;
        }

        protected string Encode(string value, string key, int shift)
        {
            string output = "";
            var keyTable = GetKeyTable(key);
            foreach (var letter in value)
            {
                // Добавляем символ, находяйся на 8 впереди (при кодировании) или позади (при расшифровке)
                output += keyTable[keyTable.IndexOf(letter) + shift];
            }
            return output;
        }

        /// <summary>
        /// Функция простого шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет зашифровано</param>
        /// <param name="key">Секрет используемы при шифровании</param>
        /// <param name="idleShift">Игнорируется</param>
        /// <returns>Зашиврованная строка</returns>
        public string Encrypt(string value, string key, int idleShift = 0) => Encode(value.ToUpper(), key.ToUpper(), EncodingShift);

        /// <summary>
        /// Функция простого шифра Тритемиуса.
        /// </summary>
        /// <param name="value">Значение, которое будет разшифровано</param>
        /// <param name="key">Секрет используемы при шифровании</param>
        /// <param name="idleShift">Холостой сдвиг</param>
        /// <returns>Исходная строка</returns>
        public string Decrypt(string value, string key, int idleShift = 0) => Encode(value.ToUpper(), key.ToUpper(), DecodingShift);
    }
}
