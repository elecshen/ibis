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
        [TestCase("_ЯЗЬ", 32028)]
        [TestCase("ЯЯЯЯ", 1048575)]
        [TestCase("ХЗЖШ", 729337)]
        [TestCase("А___", 32768)]
        [TestCase("БЯГМ", 97421)]
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
        [TestCase(1048575, "ЯЯЯЯ")]
        [TestCase(729337, "ХЗЖШ")]
        [TestCase(32768, "А___")]
        [TestCase(97421, "БЯГМ")]
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
        [TestCase("ВАСЯ", "АААА", 2, "ХДКС")]
        [TestCase("ВАСЯ", "АААА", 3, "АГВЖ")]
        [TestCase("ВАСЯ", "АААА", 4, "ЙШРБ")]
        [TestCase("ВАСЯ", "АААА", 5, "ТЧКЭ")]
        [TestCase("ВАСЯ", "АААА", 6, "ЗЛОЖ")]
        [TestCase("ВАСЯ", "ББББ", 1, "ЗРЫЕ")]
        [TestCase("ВАСЯ", "ББББ", 2, "КОДЛ")]
        [TestCase("ВАСЯ", "ББББ", 3, "УЦЧС")]
        [TestCase("ВАСЯ", "ББББ", 4, "ЩРЛС")]
        [TestCase("ВАСЯ", "ББББ", 5, "ЦДМЗ")]
        [TestCase("____", "ЗЕЛЕНЫЙ_ШАР", 1, "ЖНФЬ")]
        [TestCase("____", "ЗЕЛЕНЫЙ_ШАР", 2, "ГГШЩ")]
        [TestCase("____", "ЗЕЛЕНЫЙ_ШАР", 3, "АЖТЫ")]
        [TestCase("___А", "ЗЕЛЕНЫЙ_ШАР", 1, "ЖНФ_")]
        [TestCase("___А", "ЗЕЛЕНЫЙ_ШАР", 2, "ЬФАС")]
        [TestCase("___А", "ЗЕЛЕНЫЙ_ШАР", 3, "ЕСМЦ")]
        [TestCase("Б___", "ЗЕЛЕНЫЙ_ШАР", 1, "ОХЭГ")]
        [TestCase("Б___", "ЗЕЛЕНЫЙ_ШАР", 2, "УЮ_Ш")]
        [TestCase("Б___", "ЗЕЛЕНЫЙ_ШАР", 3, "ХЬЗН")]
        public void Oneside(string value, string key, int steps, string expected)
        {
            var result = Utils.Oneside(value, key, steps, _extSBlockModPolyTrithemiusEncoder);
            Assert.That(result, Is.EqualTo(expected));
        }

        /*
        374003 - КЖМТ
        496486 - ОГЫЕ
        423873 - ЛЭЮА
        782097 - ЦЫЧР
        229873 - Ж_ОР
        376743 - КОЭЖ
        865031 - ЩЛЧЖ
        353452 - ЙШДЛ
        451548 - МЧЮЬ
        801643 - ЧНЫК
        */
        [TestCase(374003, 496486)]
        [TestCase(496486, 423873)]
        [TestCase(423873, 782097)]
        [TestCase(782097, 229873)]
        [TestCase(229873, 376743)]
        [TestCase(376743, 865031)]
        [TestCase(865031, 353452)]
        [TestCase(353452, 451548)]
        [TestCase(451548, 801643)]
        public void LCG_Next(int seed, int expected)
        {
            var lcg = new LCG(seed, new(723482, 8677, 983609));

            var result = lcg.Next();

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1231, 7)]
        [TestCase(723482, 8)]
        [TestCase(4321, 5)]
        [TestCase(666666, 8)]
        [TestCase(010010, 7)]
        [TestCase(845239, 13)]
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
        [TestCase(4321, 666666, 0, 4321)]
        [TestCase(4321, 666666, 20, 666666)]
        [TestCase(4321, 666666, 10, 4138)]
        [TestCase(666666, 4321, 0, 666666)]
        [TestCase(666666, 4321, 20, 4321)]
        [TestCase(666666, 4321, 10, 666849)]
        [TestCase(010010, 845239, 0, 010010)]
        [TestCase(010010, 845239, 20, 845239)]
        [TestCase(010010, 845239, 10, 9655)]
        [TestCase(845239, 010010, 0, 845239)]
        [TestCase(845239, 010010, 20, 010010)]
        [TestCase(845239, 010010, 10, 845594)]
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
        [TestCase("А___", new int[] { 200618, 279175, 66103 })]
        [TestCase("___Б", new int[] { 978500, 873570, 597862 })]
        [TestCase("ОУДК", new int[] { 411933, 518731, 526147 })]
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
        [TestCase("ОУДК", "НБЙТ", 1)]
        [TestCase("ОУДК", "УРКГ", 2)]
        [TestCase("ОУДК", "ФВДБ", 3)]
        [TestCase("ОУДК", "К_ЗМ", 4)]
        [TestCase("А___", "ЕЛИФ", 1)]
        [TestCase("А___", "ЕХЮТ", 2)]
        [TestCase("А___", "ТАЕЧ", 3)]
        [TestCase("А___", "ПСЕЗ", 4)]
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
        [TestCase("ААААББББВВВВГГГГ", "ЭОФТШУЕАЯЭЩРСЭЛВ", 1)]
        [TestCase("ААААББББВВВВГГГГ", "ЖЛМЦХООРЙЯУДХЫНЖ", 2)]
        [TestCase("ААААББББВВВВГГГГ", "РЮШНП_ЕЛЕЖУХМГЧЮ", 3)]
        [TestCase("ВВВВГГГГААААББББ", "ЭОФТШУЕАЯЭЩРСЭЛВ", 1)]
        [TestCase("ВВВВГГГГААААББББ", "ЖЛМЦХООРЙЯУДХЫНЖ", 2)]
        [TestCase("ВВВВГГГГААААББББ", "РЮШНП_ЕЛЕЖУХМГЧЮ", 3)]
        [TestCase("ААААААААББББББББ", "________________", 1)]
        [TestCase("ААААААААББББББББ", "________________", 2)]
        [TestCase("ААААААААББББББББ", "________________", 3)]
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
        [TestCase("________________", "____С_ЫДЯЦЯОЩЛСЕ")]
        public void CheckSeed(string seed, string expected)
        {
            var result = Utils.CheckSeed(seed, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ААААББББВВВВГГГГ", "ЯУЯЖРЦОДЦФЮМАЧХЭ", 1)]
        [TestCase("ААААББББВВВВГГГГ", "ЮИВПЗЩИРХЖФВЛРПУ", 2)]
        [TestCase("ААААББББВВВВГГГГ", "ЯАЛМБЧПЖЕИКЧУЙШ_", 3)]
        [TestCase("ВВВВГГГГААААББББ", "ТМТЛЦПВЗТЦШФПДОЖ", 1)]
        [TestCase("ВВВВГГГГААААББББ", "СДИБЕСТЛКПЬПУНАЛ", 2)]
        [TestCase("ВВВВГГГГААААББББ", "ЧМТЮЩЧППЫЧЕАРЮЙЖ", 3)]
        [TestCase("ААААААААББББББББ", "ИХЯМУНКТЕЭГЬФЕЦТ", 1)]
        [TestCase("ААААААААББББББББ", "ЦТЮЕОБМЖХХЦИЫХЦХ", 2)]
        [TestCase("ААААААААББББББББ", "ЗМ_ЩСФЕПДИХХСМЮД", 3)]
        public void CHCLCGM_Next(string seed, string expected, int steps)
        {
            var chclcgm = new CHCLCGM<RusAlphabet>(_extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            chclcgm.Init(seed, LCGCoeffs.DefaultCoeffs);

            string result = "";
            for (int i = 0; i < steps; i++)
            {
                result = chclcgm.Next();
            }
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
