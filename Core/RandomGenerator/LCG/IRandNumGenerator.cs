namespace Core.RandomGenerator.LCG
{
    public interface IRandNumGenerator
    {
        /// <summary>
        /// Сгенерировать следующее состояние
        /// </summary>
        /// <returns></returns>
        public int Next();
    }
}