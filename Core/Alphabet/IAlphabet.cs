namespace Core.Alphabet
{
    public interface IAlphabet
    {
        public int NormalizeIndex(int index);

        public int this[char ch]
        {
            get;
        }

        public char this[int index]
        {
            get;
        }

        public int Length { get; }

        public IEnumerable<char> Except(IEnumerable<char> second);
    }
}
