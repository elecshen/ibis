using Core;
using Core.Alphabet;
using Core.RandomGenerator;
using Core.ShiftCipher.Trithemius;
using UATests.TestSuccessor;

namespace UATests
{
    public class LB3_SPweb
    {
        private RusAlphabet _alphabet;
        private AlphabetModifier<RusAlphabet> _alphabetModifier;
        private TestSBlockModPolyTrithemiusEncoder<RusAlphabet> _sBlockModPolyTrithemiusEncoder;
        private ExtSBlockModPolyTrithemiusEncoder<RusAlphabet> _extSBlockModPolyTrithemiusEncoder;
        private CHCLCGM<RusAlphabet> _generator;

        [SetUp]
        public void Setup()
        {
            _alphabet = new();
            _alphabetModifier = new(_alphabet);
            _sBlockModPolyTrithemiusEncoder = new(_alphabet, _alphabetModifier);
            _extSBlockModPolyTrithemiusEncoder = new(_alphabet, _alphabetModifier);
            _generator = new(_extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
        }

        [TestCase("АГАТ", "ТАГА", "СДДС")]
        public void XorBlockMini(string value1, string value2, string expected)
        {
            var result = Utils.XorBlockMini(value1, value2, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("АГАТ", "ТАГА", "СДДС")]
        [TestCase("КОЛЕНЬКА", "МТВ_ТЛЕН", "ЕЬОЕЭПМО")]
        public void XorBlockUltraProMax(string value1, string value2, string expected)
        {
            var result = Utils.XorBlockMini(value1, value2, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ПОЛИМАТ_ТЕХНОБОГ", 5, new string[] { "ЗБПБУОИНЯХННЯЯГН", "ЕШБКВТМЗИОАЯПТЧЭ", "ЗЯЭВФВТЫЗЧНБЖПЖЫ", "ЫКГАЖМКЮВЦЩУЭХЗФ", "ЮИ_ШЫЦЛНОТАЗТОЭЫ" })]
        public void ProduceRoundsKeys(string key, int rounds, string[] expected)
        {
            _generator.Init(key, LCGCoeffs.DefaultCoeffs);

            var result = Utils.ProduceRoundsKeys(rounds, _generator);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
