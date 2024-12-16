using Core.RandomGenerator.LCG;

namespace UATests.TestSuccessor.RandomGenerator
{
    public class Test_LCG(int seed, LCGCoeffs coeffs) : LCG(seed, coeffs)
    {
    }
}
