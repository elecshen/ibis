using Core.RandomGenerator.LCG;

namespace UATests.TestSuccessor.RandomGenerator
{
    public class Test_HCLCG(LCG lcg1, LCG lcg2, LCG lcg3) : HCLCG(lcg1, lcg2, lcg3)
    {
        public new static int CountBits(int num) => HCLCG.CountBits(num);

        public new static int ComposeNums(int num1, int num2, int threshold) => HCLCG.ComposeNums(num1, num2, threshold);
    }
}
