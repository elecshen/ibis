using Core.Alphabet;

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
        public static IEnumerable<bool> NumToBin(int num, int significantBitPos)
        {
            for (var i = significantBitPos - 1; i >= 0; i--)
                yield return (num & (1 << i)) != 0;
        }

        public static int BinToNum(IEnumerable<bool> bits, int significantBitPos)
        {
            int res = 0;
            for (var i = 0; i < significantBitPos; i++)
                if (bits.ElementAt(i))
                    res |= 1 << (significantBitPos - i - 1);
            return res;
        }

        public static bool ValidateAndPrepareInput<T>(ref string input, int requiredLength, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            if (input.Any(l => !modifier.Alphabet.Contains(l))) return false;
            input = modifier.SumString(input, new string('_', requiredLength));
            input = input[..requiredLength];
            return true;
        }
    }
}
