namespace Core.Alphabet
{
    public interface IAlphabetModifier<T> where T :IAlphabet
    {
        public T Alphabet { get; }
        /// <summary>
        /// Складывает номера символов в алфавите и возвращает символ находящийся на полученной позиции. Позиции зациклены.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public char SumChar(char c1, char c2);
        /// <summary>
        /// Вычитает номера символов в алфавите и возвращает символ находящийся на полученной позиции. Позиции зациклены.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public char SubChar(char c1, char c2);
        /// <summary>
        /// Посимвольно складывает строки методом <see cref="SumChar(char, char)"/>. Если длины строк не равны, то символы без пары останутся без изменений.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public string SumString(string s1, string s2);
        /// <summary>
        /// Посимвольно вычитает строки методом <see cref="SubChar(char, char)"/>. Если длины строк не равны, то символы без пары останутся без изменений.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public string SubString(string s1, string s2);
        /// <summary>
        /// Переводит каждый символ в соответствующую ему позицию в алфавите.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int[] TextToNums(string s);
        /// <summary>
        /// Какждое число, как позиция символа, преобразуется в символ алфавита.
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public string NumsToText(IEnumerable<int> nums);
        /// <summary>
        /// Преобразует блок из 4 символов в число. Каждый символ воспринимается как цифра из системы счисления с основанием равным <see cref="IAlphabet.Length"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public long TextToNumWithAlphabetBase(string value);
        /// <summary>
        /// Преобразует число в блок из 4 символов. Каждый символ воспринимается как цифра из системы счисления с основанием равным <see cref="IAlphabet.Length"/>.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string NumWithAlphabetBaseToText(long num);
        /// <summary>
        /// Переводит число в двоичный вид. Двоичный вид имеет <see cref="IAlphabet.GetSignificantBitPos"/> битов.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IEnumerable<bool> NumToBin(int num);
        /// <summary>
        /// Переводит биты в десятичное число. Двоичный вид должен иметь <see cref="IAlphabet.GetSignificantBitPos"/> битов.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int BinToNum(IEnumerable<bool> bits);
        /// <summary>
        /// Последовательно использует <see cref="TextToNums(string)"/> и <see cref="NumToBin(int)"/> для преобразования строки в набор битов.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IEnumerable<bool> TextToBin(string value);
        /// <summary>
        /// Последовательно использует <see cref="NumToBin(int)"/> и <see cref="TextToNums(string)"/> для преобразования набора битов в строку.
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public string BinToText(IEnumerable<bool> bits);
        /// <summary>
        /// Преобразет строку в позиции символов и производит операцию Xor. Если длины строк не равны, то будет выдано исключение <see cref="ArgumentOutOfRangeException"/>
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public string Xor(string str1, string str2);
        /// <summary>
        /// Преобразует строку в набор битов, а затем обратно начиная на <paramref name="shift"/> битов левее. Чтение битов происходит циклично.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public string BinaryTextShift(string str, int shift);
    }
}