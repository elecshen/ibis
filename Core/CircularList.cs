namespace Core
{
    public class CircularList<T> : List<T>
    {
        private int NormalizeIndex(int index) => index < 0 ? (index % Count) + Count : (index + Count) % Count;

        public new T this[int index] => base[NormalizeIndex(index)];

        public void ReplaceLastElement(int index)
        {
            index = NormalizeIndex(index - 1);
            Insert(index, base[^1]);
            RemoveAt(Count - 1);
        }
    }
}
