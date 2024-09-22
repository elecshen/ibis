namespace Core
{
    public static class Utils
    {
        public static int NormalizeIndex(int index, int length, int startIndex = 0)
        {
            if (index < startIndex || index >= length)
            {
                return index < startIndex ? (index % length) + startIndex + length : (index + length) % length + startIndex;
            }
            return index;
        }
    }
}
