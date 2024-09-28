using Core;
using Core.Alphabet;
using Core.RandomGenerator;
using Core.ShiftCipher.Trithemius;

namespace UATests
{
    public class LB3_SPweb
    {
        private RusAlphabet _alphabet;
        private AlphabetModifier<RusAlphabet> _alphabetModifier;
        private ExtSBlockModPolyTrithemiusEncoder<RusAlphabet> _extSBlockModPolyTrithemiusEncoder;
        private CHCLCGM<RusAlphabet> _generator;

        [SetUp]
        public void Setup()
        {
            _alphabet = new();
            _alphabetModifier = new(_alphabet);
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
            var result = Utils.ProduceRoundsKeys(key, rounds, _generator);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 0, "ПВБМДЙКЗИЕЖЛГОНА")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 1, "ЖНГИЛАОЕМЗЙВБКДП")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 2, "ГНОАИЖЕЛДКЙЗПБВМ")]
        public void MagicSquareEncode(string str, int matrixNum, string expected)
        {
            var result = Utils.MagicSquareEncode(str, Utils.GetMatrix(matrixNum));
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ПВБМДЙКЗИЕЖЛГОНА", 0, "АБВГДЕЖЗИЙКЛМНОП")]
        [TestCase("ЖНГИЛАОЕМЗЙВБКДП", 1, "АБВГДЕЖЗИЙКЛМНОП")]
        [TestCase("ГНОАИЖЕЛДКЙЗПБВМ", 2, "АБВГДЕЖЗИЙКЛМНОП")]
        public void MagicSquareDecode(string str, int matrixNum, string expected)
        {
            var result = Utils.MagicSquareDecode(str, Utils.GetMatrix(matrixNum));
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(2)]
        public void NumToBinToNum(int num)
        {
            var tmp = _alphabetModifier.NumToBin(num);
            var result = _alphabetModifier.BinToNum(tmp);
            Assert.That(result, Is.EqualTo(num));
        }

        [TestCase("ГОЛД", 1, "СЖХБ")]
        [TestCase("ЯРУС", 1, "ОЧЩИ")]
        public void BinaryTextShift(string value, int shift, string expected)
        {
            var result = Utils.BinaryTextShift(value, shift, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ЗОЛОТАЯ_СЕРЕДИНА", 1, "ПЯУЦШВГЖ_СПВЕЖЧШ")]
        [TestCase("ЗОЛОТАЯ_СЕРЕДИНА", 2, "ЛДОИНЗСЯАЕТРЕ_АО")]
        [TestCase("ЗОЛОТАЯ_СЕРЕДИНА", 4, "ОЩКЬРСВПИЗАТВЬЛЧ")]
        public void PBlockEncode(string value, int shift, string expected)
        {
            var result = Utils.PBlockEncode(value, shift, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ПЯУЦШВГЖ_СПВЕЖЧШ", 1, "ЗОЛОТАЯ_СЕРЕДИНА")]
        [TestCase("ЛДОИНЗСЯАЕТРЕ_АО", 2, "ЗОЛОТАЯ_СЕРЕДИНА")]
        [TestCase("ОЩКЬРСВПИЗАТВЬЛЧ", 4, "ЗОЛОТАЯ_СЕРЕДИНА")]
        public void PBlockDecode(string value, int shift, string expected)
        {
            var result = Utils.PBlockDecode(value, shift, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "ИЗООРЮЧШГЗСАЫИХЧ")]
        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 5, "СТБКОЦНДГНЫШАДЮЧ")]
        public void RoundSPEncode(string value, string key, int shift, string expected)
        {
            var result = Utils.RoundSPEncode(value, key, shift, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ИЗООРЮЧШГЗСАЫИХЧ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "КОРЫСТЬ_СЛОНА_ЭХ")]
        [TestCase("СТБКОЦНДГНЫШАДЮЧ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 5, "КОРЫСТЬ_СЛОНА_ЭХ")]
        public void RoundSPDecode(string value, string key, int shift, string expected)
        {
            var result = Utils.RoundSPDecode(value, key, shift, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 8, "ННЧЦМШЙВГУЬБЕЕИМ")]
        [TestCase("АААААААААААААААА", "АААААААААААААААА", 8, "ДЬШБОЙПРАУИФПСЛР")]
        [TestCase("ААААААААААААААА_", "________________", 8, "ХЬ_ЩКЬФЬГЬДЮШЦФЩ")]
        public void SPNetEncode(string value, string key, int rounds, string expected)
        {
            var result = Utils.SPNetEncode(value, key, rounds, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier, _generator);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("ННЧЦМШЙВГУЬБЕЕИМ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 8, "КОРЫСТЬ_СЛОНА_ЭХ")]
        public void SPNetDecode(string value, string key, int rounds, string expected)
        {
            var result = Utils.SPNetDecode(value, key, rounds, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier, _generator);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
