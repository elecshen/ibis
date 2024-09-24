using Core.Alphabet;
using Core.ShiftCipher;

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

        public static string Oneside(string value, string key, int steps, IExtendedEncoder encoder)
        {
            value = value.ToUpper();
            key = key.ToUpper() + value;
            for (int i = 0; i < steps; i++)
            {
                value = encoder.Encrypt(value, key);
                key = value + encoder.GetKeyTable(key);
            }
            return value;
        }

        public static int CountBits(int num)
        {
            int res = 0;
            while (num > 0)
            {
                res += num % 2;
                num >>= 1;
            }
            return res;
        }

        public static int ComposeNums(int num1, int num2, int threshold)
        {
            if (threshold <= 0)
                return num1;
            if (threshold >= 20)
                return num2;
            int mask1 = (1 << 20 - threshold) - 1;
            int mask2 = 0xFFFFF ^ mask1;
            num1 &= mask2;
            num2 &= mask1;
            return num1 | num2;
        }

        public static int[] Make3Seeds<T>(string value, IExtendedEncoder encoder, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            string[] keys = ["ПЕРВЫЙ_ГЕНЕРАТОР", "ВТОРОЙ_ГЕНЕРАТОР", "ТРЕТИЙ_ГЕНЕРАТОР"];
            return keys.Select(k => modifier.TextToBaseNum(Oneside(value, k, 10, encoder))).ToArray();
        }
    }
}
