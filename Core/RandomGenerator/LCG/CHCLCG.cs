using Core.Alphabet;
using Core.Encryptor;

namespace Core.RandomGenerator.LCG
{
    public class CHCLCG<T>(IExtendedEncryptor<T> encryptor, IAlphabetModifier<T> modifier, LCGCoeffs[] coeffs) : IRandCodeGenerator<T> where T : IAlphabet
    {
        protected HCLCG[] _hCLCGs = new HCLCG[4];
        protected IAlphabetModifier<T> _modifier = modifier;
        protected IExtendedEncryptor<T> _encoder = encryptor;
        protected LCGCoeffs[] _coeffs = coeffs;

        protected int[] Make3Seeds(string value)
        {
            string[] keys = ["ПЕРВЫЙ_ГЕНЕРАТОР", "ВТОРОЙ_ГЕНЕРАТОР", "ТРЕТИЙ_ГЕНЕРАТОР"];
            return keys.Select(k => (int)_modifier.TextToNumWithAlphabetBase(_encoder.Oneside(value, k, 10))).ToArray();
        }

        public virtual void Init(string seed)
        {
            if (seed.Length != 16)
                throw new ArgumentException("invalid_input");
            int[] seeds;
            for (int i = 0; i < 4; i++)
            {
                seeds = Make3Seeds(seed.Substring(4 * i, 4));
                _hCLCGs[i] = new(new(seeds[0], _coeffs[0]), new(seeds[1], _coeffs[1]), new(seeds[2], _coeffs[2]));
            }
        }

        public string Next()
        {
            string res = "";
            for (int j = 0; j < 4; j++)
            {
                int sign = 1, tmp = 0;
                for (int i = 0; i < 4; i++, sign *= -1)
                    tmp = (sign * _hCLCGs[i].Next() + 1048576 + tmp) % +1048576;
                res += _modifier.NumWithAlphabetBaseToText(tmp);
            }
            return res;
        }
    }
}
