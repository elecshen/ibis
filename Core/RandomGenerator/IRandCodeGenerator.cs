using Core.Alphabet;

namespace Core.RandomGenerator
{
    public interface IRandCodeGenerator<T> where T : IAlphabet
    {
        /// <summary>
        /// Задать начальное состояние генератора
        /// </summary>
        /// <param name="seed"></param>
        public void Init(string seed);
        /// <summary>
        /// Сгенерировать слодующее значение
        /// </summary>
        /// <returns>16 символьная строка</returns>
        public string Next();
    }
}