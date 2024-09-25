using Core;
using Core.Alphabet;
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

        [SetUp]
        public void Setup()
        {
            _alphabet = new();
            _alphabetModifier = new(_alphabet);
            _sBlockModPolyTrithemiusEncoder = new(_alphabet, _alphabetModifier);
            _extSBlockModPolyTrithemiusEncoder = new(_alphabet, _alphabetModifier);
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
    }
}
