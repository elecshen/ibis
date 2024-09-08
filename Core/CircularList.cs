namespace Core
{
    public class CircularList<T> : List<T>
    {
        public new T this[int index] => base[index < 0 ? (index % Count) + Count : (index + Count) % Count];
    }
}
