using Core.Alphabet;
using Core.ShiftCipher;

namespace Core.RandomGenerator
{
    public class CHCLCG<T>(IExtendedEncoder encoder, IAlphabetModifier<T> modifier) where T : IAlphabet
    {
        protected HCLCG[] _hCLCGs = new HCLCG[4];
        protected IAlphabetModifier<T> _modifier = modifier;
        protected IExtendedEncoder _encoder = encoder;

        public void Init(string seed, LCGCoeffs[] coeffs)
        {
            if (seed.Length != 16 || coeffs.Length != 3)
                return;
            int[] seeds;
            for (int i = 0; i < 4; i++)
            {
                seeds = Utils.Make3Seeds(seed.Substring(4 * i, 4), _encoder, _modifier);
                _hCLCGs[i] = new(new(seeds[0], coeffs[0]), new(seeds[1], coeffs[1]), new(seeds[2], coeffs[2]));
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
                res += _modifier.BaseNumToText(tmp);
            }
            return res;
        }
    }
}
