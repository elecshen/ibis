using Core.Alphabet;
using Core.RandomGenerator;
using Core.ShiftCipher;

namespace Core
{
    public static class Utils
    {
        #region LB1
        public static int NormalizeIndex(int index, int length, int startIndex = 0)
        {
            if (index < startIndex || index >= length)
            {
                return index < startIndex ? (index % length) + startIndex + length : (index + length) % length + startIndex;
            }
            return index;
        }
        #endregion LB1

        #region LB2
        public static string Oneside<T>(string value, string key, int steps, IExtendedEncoder<T> encoder) where T : IAlphabet
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

        public static int[] Make3Seeds<T>(string value, IExtendedEncoder<T> encoder, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            string[] keys = ["ПЕРВЫЙ_ГЕНЕРАТОР", "ВТОРОЙ_ГЕНЕРАТОР", "ТРЕТИЙ_ГЕНЕРАТОР"];
            return keys.Select(k => (int)modifier.TextToBaseNum(Oneside(value, k, 10, encoder))).ToArray();
        }

        public static string CheckSeed<T>(string value, IExtendedEncoder<T> encoder, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            string key = "ОТВЕТСТВЕННЫЙ_ПОДХОД";
            var blocks = new string[4];
            for (int i = 0; i < 4; i++)
                blocks[i] = value.Substring(i * 4, 4);
            for (int j = 0; j < 4; j++)
                for (int i = j + 1; i < 4; i++)
                    if (blocks[i] == blocks[j])
                        blocks[i] = Oneside(blocks[j], key, j + 2 * i, encoder);
            return string.Join("", blocks);
        }
        #endregion LB2

        #region LB3
        public static string XorBlockMini<T>(string str1, string str2, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            var nums1 = modifier.TextToNums(str1);
            var nums2 = modifier.TextToNums(str2);
            for (int i = 0; i < nums1.Length; i++)
                nums1[i] = nums1[i] ^ nums2[i];
            return modifier.NumsToText(nums1);
        }

        public static string XorBlockUltraProMax<T>(string str1, string str2, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            string res = "";
            for (int i = 0; i < str1.Length / 4; i++)
                res += XorBlockMini(str1.Substring(i * 4, 4), str2.Substring(i * 4, 4), modifier);
            return res;
        }

        public static string[] ProduceRoundsKeys<T>(string key, int rounds, IRandCodeGenerator<T> generator) where T : IAlphabet
        {
            generator.Init(key, LCGCoeffs.DefaultCoeffs);
            string[] keys = new string[rounds];
            for (int i = 0; i < rounds; i++)
                keys[i] = generator.Next();
            return keys;
        }

        private static int[][] matrix = 
            [
                [
                    16, 3, 2, 13,
                    5, 10, 11, 8,
                    9, 6, 7, 12,
                    4, 15, 14, 1
                ],
                [
                    7, 14, 4, 9,
                    12, 1, 15, 6,
                    13, 8, 10, 3,
                    2, 11, 5, 16
                ],
                [
                    4, 14, 15, 1,
                    9, 7, 6, 12,
                    5, 11, 10, 8,
                    16, 2, 3, 13
                ]
            ];

        public static int[] GetMatrix(int num) => matrix[num % matrix.Length];
            

        public static string MagicSquareEncode(string str, int[] matrix)
        {
            string res = "";
            foreach (var item in matrix)
            {
                res += str[item - 1];
            }
            return res;
        }

        public static string MagicSquareDecode(string str, int[] matrix)
        {
            var res = new char[str.Length];
            for (var i = 0; i < matrix.Length; i++)
            {
                res[matrix[i] -1] = str[i];
            }
            return string.Join("", res);
        }

        public static string BinaryTextShift<T>(string str, int shift, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            CircularList<bool> bits = [];
            foreach (var item in modifier.TextToNums(str))
                bits.AddRange(modifier.NumToBin(item));
            var res = new int[str.Length];
            for (var i = 0; i < str.Length; i++)
                res[i] = modifier.BinToNum(bits.GetRange(i * modifier.Alphabet.GetSignificantBitPos() - shift, modifier.Alphabet.GetSignificantBitPos()));
            return modifier.NumsToText(res);
        }

        public static string PBlockEncode<T>(string str, int shift, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            var tmp = MagicSquareEncode(str, GetMatrix(shift));
            return BinaryTextShift(tmp, 4 * (shift % 4) + 2, modifier);
        }

        public static string PBlockDecode<T>(string str, int shift, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            var tmp = BinaryTextShift(str, -(4 * (shift % 4) + 2), modifier);
            return MagicSquareDecode(tmp, GetMatrix(shift));
        }

        public static string RoundSPEncode<T>(string str, string key, int shift, IEncoder<T> encoder, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            var tmp = "";
            for (int i = 0; i < 4; i++)
                tmp += encoder.Encrypt(str.Substring(4 * i, 4), key, i * 4);
            tmp = PBlockEncode(tmp, shift, modifier);
            return XorBlockUltraProMax(tmp, key, modifier);
        }

        public static string RoundSPDecode<T>(string str, string key, int shift, IEncoder<T> encoder, IAlphabetModifier<T> modifier) where T : IAlphabet
        {
            str = XorBlockUltraProMax(str, key, modifier);
            str = PBlockDecode(str, shift, modifier);
            var tmp = "";
            for (int i = 0; i < 4; i++)
                tmp += encoder.Decrypt(str.Substring(4 * i, 4), key, i * 4);
            return tmp;
        }

        public static string SPNetEncode<T>(string str, string key, int rounds, IEncoder<T> encoder, IAlphabetModifier<T> modifier, IRandCodeGenerator<T> generator) where T : IAlphabet
        {
            var keySet = ProduceRoundsKeys(key, rounds, generator);
            for (int i = 0; i < rounds; i++)
                str = RoundSPEncode(str, keySet[i], i, encoder, modifier);
            return str;
        }

        public static string SPNetDecode<T>(string str, string key, int rounds, IEncoder<T> encoder, IAlphabetModifier<T> modifier, IRandCodeGenerator<T> generator) where T : IAlphabet
        {
            var keySet = ProduceRoundsKeys(key, rounds, generator);
            for (int i = rounds - 1; i >= 0; i--)
                str = RoundSPDecode(str, keySet[i], i, encoder, modifier);
            return str;
        }
        #endregion LB3
    }
}
