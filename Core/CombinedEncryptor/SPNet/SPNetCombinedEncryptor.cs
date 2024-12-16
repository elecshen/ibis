using Core.Alphabet;
using Core.Encryptor;
using Core.RandomGenerator;

namespace Core.CombinedEncryptor.SPNet
{
    public class SPNetCombinedEncryptor<T>(IEncryptor<T> encryptor, IAlphabetModifier<T> modifier, IRandCodeGenerator<T> generator) : ICombinedEncryptor<T>, IPartialCombinedEncryptor<T> where T : IAlphabet
    {
        protected readonly IEncryptor<T> _encoder = encryptor;
        protected readonly IAlphabetModifier<T> _modifier = modifier;
        protected readonly IRandCodeGenerator<T> _generator = generator;

        protected string PBlockEncode(string str, int shift)
        {
            var tmp = MagicSquare.Encode(str, shift);
            return _modifier.BinaryTextShift(tmp, 4 * (shift % 4) + 2);
        }

        protected string PBlockDecode(string str, int shift)
        {
            var tmp = _modifier.BinaryTextShift(str, -(4 * (shift % 4) + 2));
            return MagicSquare.Decode(tmp, shift);
        }

        protected string RoundSPEncode(string str, string key, int shift)
        {
            var tmp = "";
            for (int i = 0; i < 4; i++)
                tmp += _encoder.Encrypt(str.Substring(4 * i, 4), key, i * 4);
            tmp = PBlockEncode(tmp, shift);
            return _modifier.Xor(tmp, key);
        }

        protected string RoundSPDecode(string str, string key, int shift)
        {
            str = _modifier.Xor(str, key);
            str = PBlockDecode(str, shift);
            var tmp = "";
            for (int i = 0; i < 4; i++)
                tmp += _encoder.Decrypt(str.Substring(4 * i, 4), key, i * 4);
            return tmp;
        }

        public string[] ProduceRoundsKeys(string key, int rounds)
        {
            _generator.Init(key);
            string[] keys = new string[rounds];
            for (int i = 0; i < rounds; i++)
                keys[i] = _generator.Next();
            return keys;
        }

        public string RoundsEncrypt(string str, string[] keySet, int rounds)
        {
            for (int i = 0; i < rounds; i++)
                str = RoundSPEncode(str, keySet[i], i);
            return str;
        }

        public string RoundsDecrypt(string str, string[] keySet, int rounds)
        {
            for (int i = rounds - 1; i >= 0; i--)
                str = RoundSPDecode(str, keySet[i], i);
            return str;
        }

        public string Encrypt(string str, string key, int rounds)
        {
            var keySet = ProduceRoundsKeys(key, rounds);
            for (int i = 0; i < rounds; i++)
                str = RoundSPEncode(str, keySet[i], i);
            return str;
        }

        public string Decrypt(string str, string key, int rounds)
        {
            var keySet = ProduceRoundsKeys(key, rounds);
            for (int i = rounds - 1; i >= 0; i--)
                str = RoundSPDecode(str, keySet[i], i);
            return str;
        }
    }
}
