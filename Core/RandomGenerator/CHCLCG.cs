using Core.Alphabet;
using Core.ShiftCipher;

namespace Core.RandomGenerator
{
    public class CHCLCG<T> where T : IAlphabet
    {
        protected HCLCG[] _hCLCGs;
        protected IAlphabetModifier<T> _modifier;

        public CHCLCG(string seed, IExtendedEncoder encoder, IAlphabetModifier<T> modifier, LCGCoeffs[] coeffs)
        {
            _hCLCGs = new HCLCG[4];
            _modifier = modifier;
            int[] seeds;
            for (int i = 0; i < 4; i++)
            {
                seeds = Utils.Make3Seeds(seed.Substring(4*i, 4), encoder, modifier);
                _hCLCGs[i] = new(new(seeds[0], coeffs[0]), new(seeds[1], coeffs[1]), new(seeds[2], coeffs[2]));
                //_hCLCGs[i] = new(new("", coeffs[0]), new(seeds[1], coeffs[1]), new(seeds[2], coeffs[2]));
            }
        }

        public string Next()
        {
            string res = "";
            int sign = 1, tmp = 0;
            for (int i = 0; i < 4; i++, sign *= -1)
            {
                tmp = (sign * _hCLCGs[i].Next() + 1048576 + tmp) % +1048576;
                res += _modifier.BaseNumToText(tmp);
            }
            return res;
        }
    }
}
