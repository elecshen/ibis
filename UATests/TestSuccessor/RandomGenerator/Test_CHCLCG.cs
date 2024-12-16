using Core.Alphabet;
using Core.Encryptor;
using Core.RandomGenerator.LCG;

namespace UATests.TestSuccessor.RandomGenerator
{
    public class Test_CHCLCG<T>(IExtendedEncryptor<T> encryptor, IAlphabetModifier<T> modifier, LCGCoeffs[] coeffs) : CHCLCG<T>(encryptor, modifier, coeffs) where T : IAlphabet
    {
        public new int[] Make3Seeds(string value) => base.Make3Seeds(value);
    }
}
