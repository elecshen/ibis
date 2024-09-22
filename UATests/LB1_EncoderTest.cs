using Core.Alphabet;
using UATests.TestSuccessor;

namespace UATests
{
    public class LB1_EncoderTest
    {
        private RusAlphabet _alphabet;
        private TestClassicTrithemiusEncoder _classicTrithemiusEncoder;
        private TestPolyTrithemiusEncoder _polyTrithemiusEncoder;
        private AlphabetModifier<RusAlphabet> _alphabetModifier;
        private TestSBlockModPolyTrithemiusEncoder<RusAlphabet> _sBlockModPolyTrithemiusEncoder;

        [SetUp]
        public void Setup()
        {
            _alphabet = new();
            _classicTrithemiusEncoder = new(_alphabet);
            _polyTrithemiusEncoder = new(_alphabet);
            _alphabetModifier = new(_alphabet);
            _sBlockModPolyTrithemiusEncoder = new(_alphabet, _alphabetModifier);
        }

        [TestCase('Î', 15)]
        [TestCase('î', 15)]
        [TestCase('Æ', 7)]
        [TestCase('Å', 6)]
        [TestCase('À', 1)]
        [TestCase('_', 0)]
        public void PermutationCharToNum_Success(char ch, int expected)
        {
            var result = _alphabet[ch];
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase('¨', typeof(KeyNotFoundException))]
        [TestCase('Y', typeof(KeyNotFoundException))]
        public void PermutationCharToNum_Failure(char ch, Type expectedException)
        {
            Assert.Throws(expectedException, () => { _ = _alphabet[ch]; });
        }

        [TestCase(15, 'Î')]
        [TestCase(7, 'Æ')]
        [TestCase(6, 'Å')]
        [TestCase(1, 'À')]
        [TestCase(0, '_')]
        [TestCase(-32, '_')]
        [TestCase(32, '_')]
        public void PermutationNumToChar_Success(int num, char expected)
        {
            var result = _alphabet[num];
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}