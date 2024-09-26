namespace Core.RandomGenerator
{
    public readonly struct LCGCoeffs(int a, int c, int m)
    {
        public readonly int A = a;
        public readonly int C = c;
        public readonly int M = m;

        public static readonly LCGCoeffs[] DefaultCoeffs = [new(723482, 8677, 983609), new(252564, 9109, 961193), new(357630, 8971, 948209)];
    }

    public class LCG(int seed, LCGCoeffs coeffs) : IRandNumGenerator
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
