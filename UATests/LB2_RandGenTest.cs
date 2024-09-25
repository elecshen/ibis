using Core;
using Core.Alphabet;
using Core.RandomGenerator;
using Core.ShiftCipher.Trithemius;
using UATests.TestSuccessor;

namespace UATests
{
    public class LB2_RandGenTest
    {
        private RusAlphabet _alphabet;
        private AlphabetModifier<RusAlphabet> _alphabetModifier;
        private TestSBlockModPolyTrithemiusEncoder<RusAlphabet> _sBlockModPolyTrithemiusEncoder;
        private ExtSBlockModPolyTrithemiusEncoder<RusAlphabet> _extSBlockModPolyTrithemiusEncoder;

        [SetUp]
        public void Setup()
        {
            _alphabet = new();
            _alphabetModifier = new(_alphabet);
            _sBlockModPolyTrithemiusEncoder = new(_alphabet, _alphabetModifier);
            _extSBlockModPolyTrithemiusEncoder = new(_alphabet, _alphabetModifier);
        }

        [TestCase("АБВГ", 34916)]
        [TestCase("ЯЯЯЯ", 1048575)]
        [TestCase("ЛУЛУ", 414100)]
        [TestCase("КМЖТ", 374003)]
        [TestCase("ОГЫЕ", 496486)]
        [TestCase("ЛЭЮА", 423873)]
        [TestCase("КЯВБ", 392290)]
        public void PermutationTextToBaseNum(string value, int expected)
        {
            var result = _alphabetModifier.TextToBaseNum(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(34916, "АБВГ")]
        [TestCase(32028, "_ЯЗЬ")]
        public void PermutationBaseNumToText(int num, string expected)
        {
            var result = _alphabetModifier.BaseNumToText(num);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ВАСЯ", "____", 1, "ЖОЧБ")]
        [TestCase("ВАСЯ", "____", 2, "ЯВЗН")]
        [TestCase("ВАСЯ", "____", 3, "_АТЦ")]
        [TestCase("ВАСЯ", "____", 4, "ИЧПБ")]
        [TestCase("ВАСЯ", "АААА", 1, "ЗОЧБ")]
        public void Oneside(string value, string key, int steps, string expected)
        {
            var result = Utils.Oneside(value, key, steps, _extSBlockModPolyTrithemiusEncoder);
            Assert.That(result, Is.EqualTo(expected));
        }


        [TestCase(414100, 374003)]
        [TestCase(374003, 496486)]
        [TestCase(496486, 423873)]
        public void LCG_Next(int seed, int expected)
        {
            var lcg = new LCG(seed, new(723482, 8677, 983609));

            var result = lcg.Next();

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1231, 7)]
        [TestCase(723482, 8)]
        public void CountBits(int num, int expected)
        {
            var result = Utils.CountBits(num);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1231, 723482, 0, 1231)]
        [TestCase(1231, 723482, 20, 723482)]
        [TestCase(1231, 723482, 10, 1562)]
        [TestCase(1231, 723482, 11, 1050)]
        [TestCase(723482, 1231, 10, 723151)]
        [TestCase(723482, 1231, 11, 723663)]
        public void ComposeNums(int num1, int num2, int threshold, int expected)
        {
            var result = Utils.ComposeNums(num1, num2, threshold);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ТЖЧТ", 1)]
        [TestCase("КЯВБ", 2)]
        [TestCase("НЯПП", 3)]
        [TestCase("КХЖР", 4)]
        [TestCase("НЧ_С", 5)]
        [TestCase("ЖХПН", 6)]
        public void HCLCG_Next1(string expected, int steps)
        {
            var lcg1 = new LCG(49942, new(723482, 8677, 983609));
            var lcg2 = new LCG(786923, new(252564, 9109, 961193));
            var lcg3 = new LCG(840225, new(357630, 8971, 948209));
            var hclcg = new HCLCG(lcg1, lcg2, lcg3);
            string result = "";
            for (int i = 0; i < steps; i++)
            {
                result = _alphabetModifier.BaseNumToText(hclcg.Next());
            }

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("КОЛА", new int[] { 964101, 765457, 387227 })]
        [TestCase("ПНЛА", new int[] { 761685, 375102, 51081 })]
        [TestCase("АБВГ", new int[] { 382497, 225721, 252520 })]
        public void Make3Seeds(string value, int[] expected)
        {
            var result = Utils.Make3Seeds(value, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("КОЛА", "ВЭЕЭ", 1)]
        [TestCase("КОЛА", "СЬЭН", 2)]
        [TestCase("КОЛА", "ГДУЖ", 3)]
        [TestCase("КОЛА", "ЕАОЙ", 4)]
        [TestCase("АБВГ", "ГЯФК", 1)]
        [TestCase("АБВГ", "ГЧХЖ", 2)]
        [TestCase("АБВГ", "ВЫХД", 3)]
        [TestCase("АБВГ", "_НЙЕ", 4)]
        public void HCLCG_Next3(string seed, string expected, int steps)
        {
            var seeds = Utils.Make3Seeds(seed, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            var lcg1 = new LCG(seeds[0], new(723482, 8677, 983609));
            var lcg2 = new LCG(seeds[1], new(252564, 9109, 961193));
            var lcg3 = new LCG(seeds[2], new(357630, 8971, 948209));
            var hclcg = new HCLCG(lcg1, lcg2, lcg3);
            string result = "";
            for (int i = 0; i < steps; i++)
            {
                result = _alphabetModifier.BaseNumToText(hclcg.Next());
            }

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "ЬЕШЮШ_ЗЯЧЖ_ВЖЙЕГ", 1)]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "ГУЬЙЭЬДЫЭУ_ЮХЛДТ", 2)]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "ДИЛ_АСЩ_ВЧЯП_ИЕХ", 3)]
        public void CHCLCG_Next(string seed, string expected, int steps)
        {
            var chclcg = new CHCLCG<RusAlphabet>(_extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            chclcg.Init(seed, [new(723482, 8677, 983609), new(252564, 9109, 961193), new(357630, 8971, 948209)]);

            string result = "";
            for (int i = 0; i < steps; i++)
            {
                result = chclcg.Next();
            }
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ААААААААББББББББ", "ААААЦДЫДББББЮДЗГ")]
        [TestCase("ВВВВГГГГАААААААА", "ВВВВГГГГААААЮДЗГ")]
        [TestCase("АААААААААААААААА", "ААААЦДЫДЖЫЗЦЫЦХД")]
        public void CheckSeed(string seed, string expected)
        {
            var result = Utils.CheckSeed(seed, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "ЬЕШЮШ_ЗЯЧЖ_ВЖЙЕГ", 1)]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "ГУЬЙЭЬДЫЭУ_ЮХЛДТ", 2)]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "ДИЛ_АСЩ_ВЧЯП_ИЕХ", 3)]
        public void CHCLCGM_Next(string seed, string expected, int steps)
        {
            var chclcgm = new CHCLCGM<RusAlphabet>(_extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            chclcgm.Init(seed, [new(723482, 8677, 983609), new(252564, 9109, 961193), new(357630, 8971, 948209)]);

            string result = "";
            for (int i = 0; i < steps; i++)
            {
                result = chclcgm.Next();
            }
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
