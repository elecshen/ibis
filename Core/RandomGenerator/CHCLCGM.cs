using Core.Alphabet;
using Core.ShiftCipher;

namespace Core.RandomGenerator
{
    public class CHCLCGM<T>(IExtendedEncoder encoder, IAlphabetModifier<T> modifier) : CHCLCG<T>(encoder, modifier) where T : IAlphabet
    {
        public override void Init(string seed, LCGCoeffs[] coeffs)
        {
            base.Init(Utils.CheckSeed(seed, _encoder, _modifier), coeffs);
        }
    }
}
