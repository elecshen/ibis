using Core.Alphabet;
using UATests.TestSuccessor.Encryptor;

namespace UATests
{
    public class LB1_EncoderTest
    {
        private RusAlphabet _alphabet;
        private AlphabetModifier<RusAlphabet> _alphabetModifier;
        private Test_ClassicTrithemiusEncoder<RusAlphabet> _classicTrithemiusEncoder;
        private Test_PolyTrithemiusEncoder<RusAlphabet> _polyTrithemiusEncoder;
        private Test_SBlockModPolyTrithemiusEncoder<RusAlphabet> _sBlockModPolyTrithemiusEncoder;

        [SetUp]
        public void Setup()
        {
            _alphabet = new();
            _classicTrithemiusEncoder = new(_alphabet);
            _polyTrithemiusEncoder = new(_alphabet);
            _alphabetModifier = new(_alphabet);
            _sBlockModPolyTrithemiusEncoder = new(_alphabet, _alphabetModifier);
        }

        [TestCase('�', 15)]
        [TestCase('�', 15)]
        [TestCase('�', 7)]
        [TestCase('�', 6)]
        [TestCase('�', 1)]
        [TestCase('_', 0)]
        public void PermutationCharToNum_Success(char ch, int expected)
        {
            var result = _alphabet[ch];
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase('�', typeof(KeyNotFoundException))]
        [TestCase('Y', typeof(KeyNotFoundException))]
        public void PermutationCharToNum_Failure(char ch, Type expectedException)
        {
            Assert.Throws(expectedException, () => { _ = _alphabet[ch]; });
        }

        [TestCase(15, '�')]
        [TestCase(7, '�')]
        [TestCase(6, '�')]
        [TestCase(1, '�')]
        [TestCase(0, '_')]
        [TestCase(-32, '_')]
        [TestCase(32, '_')]
        public void PermutationNumToChar_Success(int num, char expected)
        {
            var result = _alphabet[num];
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("��������_����", "������������", "����������_��")]
        [TestCase("��������_����", "�����", "�������������")]
        public void ClassicEncrypt(string value, string key, string expected)
        {
            var result = _classicTrithemiusEncoder.Encrypt(value, key, 0);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("��������_����", "������������", "����������_��")]
        [TestCase("��������_����", "�����", "�������������")]
        public void shiftTable(string value, string key, string expected)
        {
            var result = _classicTrithemiusEncoder.Encrypt(value, key, 0);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("��������_�����", "�����_��������", 0, "����_���������")]
        [TestCase("��������_�����", "�����_��������", 1, "��������������")]
        public void PolyEncrypt(string value, string key, int idleShift, string expected)
        {
            var result = _polyTrithemiusEncoder.Encrypt(value, key, idleShift);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("����", "����", 0, "����")]
        public void SBlockEncrypt(string value, string key, int idleShift, string expected)
        {
            var result = _sBlockModPolyTrithemiusEncoder.Encrypt(value, key, idleShift);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}