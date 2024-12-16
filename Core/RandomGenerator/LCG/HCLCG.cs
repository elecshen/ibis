namespace Core.RandomGenerator.LCG
{
    public class HCLCG(LCG lcg1, LCG lcg2, LCG lcg3) : IRandNumGenerator
    {
        protected LCG _lcg1 = lcg1;
        protected LCG _lcg2 = lcg2;
        protected LCG _lcg3 = lcg3;

        protected static int CountBits(int num)
        {
            int res = 0;
            while (num > 0)
            {
                res += num % 2;
                num >>= 1;
            }
            return res;
        }

        protected static int ComposeNums(int num1, int num2, int threshold)
        {
            if (threshold <= 0)
                return num1;
            if (threshold >= 20)
                return num2;
            int mask1 = (1 << 20 - threshold) - 1;
            int mask2 = 0xFFFFF ^ mask1;
            num1 &= mask2;
            num2 &= mask1;
            return num1 | num2;
        }

        public int Next()
        {
            var num1 = _lcg1.Next();
            var num2 = _lcg2.Next();
            var control = _lcg3.Next();
            var n = CountBits(control);
            int res;
            if (control % 2 == 0)
                res = ComposeNums(num1, num2, n);
            else
                res = ComposeNums(num2, num1, n);
            return res;
        }
    }
}
