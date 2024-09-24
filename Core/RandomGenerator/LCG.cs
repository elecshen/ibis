namespace Core.RandomGenerator
{
    public readonly struct LCGCoeffs(int a, int c, int m)
    {
        public readonly int A = a;
        public readonly int C = c;
        public readonly int M = m;
    }

    public class LCG(int seed, LCGCoeffs coeffs) : IRandGenerator
    {
        protected readonly LCGCoeffs _coeffs = coeffs;
        protected long state = seed;

        public int Next()
        {
            state = (_coeffs.A * state + _coeffs.C) % _coeffs.M;
            return (int)state;
        }
    }
}
