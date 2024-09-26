namespace Core.RandomGenerator
{
    public class HCLCG(LCG lcg1, LCG lcg2, LCG lcg3) : IRandNumGenerator
    {
        protected LCG _lcg1 = lcg1;
        protected LCG _lcg2 = lcg2;
        protected LCG _lcg3 = lcg3;

        public int Next()
        {
            var num1 = _lcg1.Next();
            var num2 = _lcg2.Next();
            var control = _lcg3.Next();
            var n = Utils.CountBits(control);
            int res;
            if (control % 2 == 0)
                res = Utils.ComposeNums(num1, num2, n);
            else
                res = Utils.ComposeNums(num2, num1, n);
            return res;
        }
    }
}
