namespace Core
{
    /// <summary>
    /// Расширяет возможности <see cref="List{T}"/>, позволяя передавать индекс за пределами диапазона значений.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircularList<T> : List<T>
    {
        /// <summary>
        /// Возвращает значения индекса в пределах диапазона значений.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int NormalizeIndex(int index) => index < 0 ? (index % Count) + Count : (index + Count) % Count;

        public new T this[int index] => base[NormalizeIndex(index)];

        /// <summary>
        /// Переставляет последний элемент списка на указанную позицию.
        /// </summary>
        /// <param name="index"></param>
        public void ReplaceLastElement(int index)
        {
            index = NormalizeIndex(index - 1);
            Insert(index, base[^1]);
            RemoveAt(Count - 1);
        }
    }
}
