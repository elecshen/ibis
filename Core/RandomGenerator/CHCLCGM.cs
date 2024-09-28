using Core.Alphabet;
using Core.ShiftCipher;

namespace Core.RandomGenerator
{
    public class CHCLCGM<T>(IExtendedEncoder encoder, IAlphabetModifier<T> modifier) : CHCLCG<T>(encoder, modifier) where T : IAlphabet
    {
        public override void Init(string seed, LCGCoeffs[] coeffs)
        {
            base.Init(Utils.CheckSeed(seed, _encoder, _modifier), coeffs);
            for (int i = 0; i < 4; i++)
            {
                if (i > 0)
                    for (int j = 0; j <= i; j++)
                        _hCLCGs[i].Next();
            }
        }
    }
}
