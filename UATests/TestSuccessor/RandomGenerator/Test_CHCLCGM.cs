using Core.Alphabet;
using Core.Encryptor;
using Core.RandomGenerator.LCG;

namespace UATests.TestSuccessor.RandomGenerator
{
    public class Test_CHCLCGM<T>(IExtendedEncryptor<T> encryptor, IAlphabetModifier<T> modifier, LCGCoeffs[] coeffs) : CHCLCGM<T>(encryptor, modifier, coeffs) where T : IAlphabet
    {
        public new string CheckSeed(string value) => base.CheckSeed(value);
    }
}
