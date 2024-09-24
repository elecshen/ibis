using Core.Alphabet;
using Core.ShiftCipher;

namespace Core.RandomGenerator
{
    public class CHCLCGM<T>(string seed, IExtendedEncoder encoder, IAlphabetModifier<T> modifier, LCGCoeffs[] coeffs) 
        : CHCLCG<T>(Utils.CheckSeed(seed, encoder, modifier), encoder, modifier, coeffs) where T : IAlphabet
    {
    }
}
