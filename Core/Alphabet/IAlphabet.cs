namespace Core.Alphabet
{
    public interface IAlphabet
    {
        /// <summary>
        /// Возвращает значения индекса в пределах диапазона значений.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int NormalizeIndex(int index);

        /// <summary>
        /// Возвращет позицию символа в алфавите.
        /// </summary>
        /// <param name="ch">Параметр является регистронезависимым</param>
        /// <returns></returns>
        public int this[char ch]
        {
            get;
        }

        /// <summary>
        /// Возвращает символ алфавита по порядковому номеру. Допускается указывать индекс выходящий за пределы диапазона значений.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Возвращает символ в верхнем регистре</returns>
        public char this[int index]
        {
            get;
        }

        public int Length { get; }

        /// <summary>
        /// Возвращает перечисление символов алфавита, иключая символы из переданного перечисления
        /// </summary>
        /// <param name="second">Перечисление символов, которые будут исключены из результата</param>
        /// <returns>Возвращает перечисление символов в верхнем регистре</returns>
        public IEnumerable<char> Except(IEnumerable<char> second);
    }
}
