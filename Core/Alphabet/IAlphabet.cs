namespace Core.Alphabet
{
    public interface IAlphabet
    {
        /// <summary>
        /// Возвращет позицию символа в алфавите. Передаваемый символ может быть в любом регистре.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public int this[char ch]
        {
            get;
        }

        /// <summary>
        /// Возвращает символ алфавита в верхнем регистре по порядковому номеру. Допускается указывать индекс выходящий за пределы диапазона значений.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char this[int index]
        {
            get;
        }

        public int Length { get; }

        /// <summary>
        /// Возвращает перечисление символов алфавита в верхнем регистре, иключая символы из переданного перечисления.
        /// </summary>
        /// <param name="second">Перечисление символов, которые будут исключены из результата</param>
        /// <returns></returns>
        public IEnumerable<char> Except(IEnumerable<char> second);

        public bool IsValidString(string str);

        public int GetSignificantBitPos();
    }
}
