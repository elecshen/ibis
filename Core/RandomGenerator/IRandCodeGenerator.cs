using Core.Alphabet;

namespace Core.RandomGenerator
{
    public interface IRandCodeGenerator<T> where T : IAlphabet
    {
        public void Init(string seed, LCGCoeffs[] coeffs);
        public string Next();
    }
}