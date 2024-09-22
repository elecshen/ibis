namespace Core
{
    /// <summary>
    /// Расширяет возможности <see cref="List{T}"/>, позволяя передавать индекс за пределами диапазона значений.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircularList<T> : List<T>
    {
        public new T this[int index] => base[Utils.NormalizeIndex(index, Count)];

        /// <summary>
        /// Переставляет последний элемент списка на указанную позицию.
        /// </summary>
        /// <param name="index"></param>
        public void ShiftElement(int to, int from)
        {
            to = Utils.NormalizeIndex(to - 1, Count);
            Insert(to, base[from]);
            RemoveAt(from);
        }
    }
}
