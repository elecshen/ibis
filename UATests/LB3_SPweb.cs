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
        // Проверка работоспособности операции побитного XOR, проверка ее обратности
        [TestCase("АГАТ", "ТАГА", "СДДС")]
        [TestCase("____", "АААА", "АААА")]
        [TestCase("АБВГ", "АААА", "_ВБД")]
        [TestCase("ТАГА", "АГАТ", "СДДС")]
        public void XorBlockMini(string value1, string value2, string expected)
        {
            var result = Utils.XorBlockMini(value1, value2, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }
        // Проверка работоспособности операции побитного XOR, проверка ее обратности
        [TestCase("АГАТ", "ТАГА", "СДДС")]
        [TestCase("____", "АААА", "АААА")]
        [TestCase("АБВГ", "АААА", "_ВБД")]
        [TestCase("ТАГА", "АГАТ", "СДДС")]
        [TestCase("КОЛЕНЬКА", "МТВ_ТЛЕН", "ЕЬОЕЭПМО")]
        [TestCase("ТОРТ_ХОЧЕТ_ГОРКУ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", "ЮЬСТВГИЧ_ИЕГЬЭМЩ")]
        [TestCase("ЭТ_ЕЩЕ_ОДИН_ТЕСТ", "А_А_А_А_А_А_А_А_", "ЬТАЕЫЕАОГИО_СЕТТ")]
        [TestCase("ТОРТ_ХОЧЕТ_ГОРКУ", "ЮЬСТВГИЧ_ИЕГЬЭМЩ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН")]
        [TestCase("АГАТ", "СДДС", "ТАГА")]
        [TestCase("МТВ_ТЛЕН", "ЕЬОЕЭПМО", "КОЛЕНЬКА")]
        public void XorBlockUltraProMax(string value1, string value2, string expected)
        {
            var result = Utils.XorBlockMini(value1, value2, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }
        // Проверка работоспособности генерации раундовых ключей
        [TestCase("ПОЛИМАТ_ТЕХНОБОГ", 5, new string[] { "ЗБПБУОИНЯХННЯЯГН", "ЕШБКВТМЗИОАЯПТЧЭ", "ЗЯЭВФВТЫЗЧНБЖПЖЫ", "ЫКГАЖМКЮВЦЩУЭХЗФ", "ЮИ_ШЫЦЛНОТАЗТОЭЫ" })]
        [TestCase("КРАТНЫЙ__ЧЕТЫРЕМ", 5, new string[] { "ХЕЯРЩЛАЧ_ЫВСЗ_ЗК", "ЧФЗАЯТПДФШПОГОСЙ", "ВЦЗИЭЫРСФКЕМФПЮЗ", "ТЗЫПГМБШЕЗОНВШ_З", "ВАМ__ЬКЗАТГ_ЦЮБЗ" })]
        public void ProduceRoundsKeys(string key, int rounds, string[] expected)
        {
            var result = Utils.ProduceRoundsKeys(key, rounds, _generator);
            Assert.That(result, Is.EqualTo(expected));
        }
        // Проверка работоспособности шифрования при помощи шифра перестановки с магическими квадратами
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 0, "ПВБМДЙКЗИЕЖЛГОНА")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 1, "ЖНГИЛАОЕМЗЙВБКДП")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 2, "ГНОАИЖЕЛДКЙЗПБВМ")]
        [TestCase("НТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "НВТТВЩЕ_ЕСЕ__ЕЛН")]
        [TestCase("НТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "ЕЛ_Е_НЕСТ_ЩВТЕВН")]
        [TestCase("НТВ_ВСЕ_ЕЩЕ_ТЛЕН", 2, "_ЛЕНЕЕС_ВЕЩ_НТВТ")]
        public void MagicSquareEncode(string str, int matrixNum, string expected)
        {
            var result = Utils.MagicSquareEncode(str, Utils.GetMatrix(matrixNum));
            Assert.That(result, Is.EqualTo(expected));
        }

        // Проверка работоспособности расшифрования при помощи шифра перестановки с магическими квадратами
        [TestCase("ПВБМДЙКЗИЕЖЛГОНА", 0, "АБВГДЕЖЗИЙКЛМНОП")]
        [TestCase("ЖНГИЛАОЕМЗЙВБКДП", 1, "АБВГДЕЖЗИЙКЛМНОП")]
        [TestCase("ГНОАИЖЕЛДКЙЗПБВМ", 2, "АБВГДЕЖЗИЙКЛМНОП")]
        [TestCase("НВТТВЩЕ_ЕСЕ__ЕЛН", 0, "НТВ_ВСЕ_ЕЩЕ_ТЛЕН")]
        [TestCase("ЕЛ_Е_НЕСТ_ЩВТЕВН", 1, "НТВ_ВСЕ_ЕЩЕ_ТЛЕН")]
        [TestCase("_ЛЕНЕЕС_ВЕЩ_НТВТ", 2, "НТВ_ВСЕ_ЕЩЕ_ТЛЕН")]
        public void MagicSquareDecode(string str, int matrixNum, string expected)
        {
            var result = Utils.MagicSquareDecode(str, Utils.GetMatrix(matrixNum));
            Assert.That(result, Is.EqualTo(expected));
        }

        // 
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(12)]
        [TestCase(31)]
        [TestCase(00000)]
        public void NumToBinToNum(int num)
        {
            var tmp = _alphabetModifier.NumToBin(num);
            var result = _alphabetModifier.BinToNum(tmp);
            Assert.That(result, Is.EqualTo(num));
        }

        // Проверка работоспособности циклического сдвига двоичного слова
        [TestCase("ГОЛД", 1, "СЖХБ")]
        [TestCase("ГОЛД", 2, "ИВЫА")]
        [TestCase("ГОЛД", 3, "УРЭП")]
        [TestCase("ЯРУС", 1, "ОЧЩИ")]
        [TestCase("ЯРУС", 2, "ЦЬМГ")]
        [TestCase("ЯРУС", 3, "КЮЕС")]
        [TestCase("СКИЙ", 1, "ИДУФ")]
        [TestCase("СКИЙ", 2, "УСЩЙ")]
        [TestCase("СКИЙ", 3, "ЙИМД")]
        public void BinaryTextShift(string value, int shift, string expected)
        {
            var result = Utils.BinaryTextShift(value, shift, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }
        // Проверка работоспособности прямого P-блока
        [TestCase("ЗОЛОТАЯ_СЕРЕДИНА", 1, "ПЯУЦШВГЖ_СПВЕЖЧШ")]
        [TestCase("ЗОЛОТАЯ_СЕРЕДИНА", 2, "ЛДОИНЗСЯАЕТРЕ_АО")]
        [TestCase("ЗОЛОТАЯ_СЕРЕДИНА", 3, "ЬСПБЧЮКЕМБАГВЮЛЮ")]
        [TestCase("ЗОЛОТАЯ_СЕРЕДИНА", 4, "ОЩКЬРСВПИЗАТВЬЛЧ")]
        [TestCase("ПЛАНОВАЯПРОВЕРКА", 1, "ППЧЦЗАЧДРТОЧПХЖЦ")]
        [TestCase("ПЛАНОВАЯПРОВЕРКА", 2, "АЕНРКППАВВООРЯАЛ")]
        [TestCase("ПЛАНОВАЯПРОВЕРКА", 3, "ЦВ_ББЧЛЯБЯЯ_ЕБЕЬ")]
        [TestCase("ПЛАНОВАЯПРОВЕРКА", 4, "ЗЛКУ_ЬБЧШЦЬЗКВЫЧ")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 1, "ЧВЦБГХ_ЦТЕУДАРДС")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 2, "ВМГНОАИЖЕЛДКЙЗПБ")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 3, "ЮЬВ_ЕГЩЙУХПСЛНЧЗ")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", 4, "АЫРБК_КШТЙБПЧСШЛ")]
        public void PBlockEncode(string value, int shift, string expected)
        {
            var result = Utils.PBlockEncode(value, shift, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }
        // Проверка работоспособности обратного P-блока
        [TestCase("ПЯУЦШВГЖ_СПВЕЖЧШ", 1, "ЗОЛОТАЯ_СЕРЕДИНА")]
        [TestCase("ЛДОИНЗСЯАЕТРЕ_АО", 2, "ЗОЛОТАЯ_СЕРЕДИНА")]
        [TestCase("ЬСПБЧЮКЕМБАГВЮЛЮ", 3, "ЗОЛОТАЯ_СЕРЕДИНА")]
        [TestCase("ОЩКЬРСВПИЗАТВЬЛЧ", 4, "ЗОЛОТАЯ_СЕРЕДИНА")]
        [TestCase("ППЧЦЗАЧДРТОЧПХЖЦ", 1, "ПЛАНОВАЯПРОВЕРКА")]
        [TestCase("АЕНРКППАВВООРЯАЛ", 2, "ПЛАНОВАЯПРОВЕРКА")]
        [TestCase("ЦВ_ББЧЛЯБЯЯ_ЕБЕЬ", 3, "ПЛАНОВАЯПРОВЕРКА")]
        [TestCase("ЗЛКУ_ЬБЧШЦЬЗКВЫЧ", 4, "ПЛАНОВАЯПРОВЕРКА")]
        [TestCase("ЧВЦБГХ_ЦТЕУДАРДС", 1, "АБВГДЕЖЗИЙКЛМНОП")]
        [TestCase("ВМГНОАИЖЕЛДКЙЗПБ", 2, "АБВГДЕЖЗИЙКЛМНОП")]
        [TestCase("ЮЬВ_ЕГЩЙУХПСЛНЧЗ", 3, "АБВГДЕЖЗИЙКЛМНОП")]
        [TestCase("АЫРБК_КШТЙБПЧСШЛ", 4, "АБВГДЕЖЗИЙКЛМНОП")]
        public void PBlockDecode(string value, int shift, string expected)
        {
            var result = Utils.PBlockDecode(value, shift, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }


        // Проверка работоспособности прямого раундового преобразования P-блока
        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "ИЗООРЮЧШГЗСАЫИХЧ")]
        [TestCase("КОРЫСТЬ_СЛОН__ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "ИЗОМШЮЧШГЗСАЫОЧП")]
        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "УКБ_ЕПЩКВФГИДУЫЩ")]
        [TestCase("КОРЫСТЬ_СЛОН__ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "УКНПЕПЩЖТРУИДУЫЩ")]
        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 2, "_МВБФЙЛПЛЯОЫ_ЗФЯ")]
        [TestCase("КОРЫСТЬ_СЛОН__ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 2, "_ГВЫЛЙЛПЛЯОЫ_ЗФЯ")]
        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 3, "АХСЕЧРЩТАЛНУЕЛЛО")]
        [TestCase("КОРЫСТЬ_СЛОН__ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 3, "СГСЕЧРЗТАЛНУЕЛЛН")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "У_АФБГИДЗАНИЯНРЧ")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "НТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0, "ЦГДЖЩУШМАШАИИЦЕЛ")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "НТЬЗН_ЫФ__ЛЬДИЗМ")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "НТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "МЭТДЭРТХ_ФЬММ_ЗЬ")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 2, "АЖТЮИИЭАКЯ_ЬЙШ_Г")]
        [TestCase("АБВГДЕЖЗИЙКЛМНОП", "НТВ_ВСЕ_ЕЩЕ_ТЛЕН", 2, "СЛЗ_ОШЩЭЙЭБЭЗЧ_Х")]
        public void RoundSPEncode(string value, string key, int shift, string expected)
        {
            var result = Utils.RoundSPEncode(value, key, shift, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        // Проверка работоспособности обратного раундового преобразования P-блока
        [TestCase("ИЗООРЮЧШГЗСАЫИХЧ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0,"КОРЫСТЬ_СЛОНА_ЭХ" )]
        [TestCase("ИЗОМШЮЧШГЗСАЫОЧП", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0,"КОРЫСТЬ_СЛОН__ЭХ" )]
        [TestCase("УКБ_ЕПЩКВФГИДУЫЩ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1,"КОРЫСТЬ_СЛОНА_ЭХ" )]
        [TestCase("УКНПЕПЩЖТРУИДУЫЩ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1,"КОРЫСТЬ_СЛОН__ЭХ" )]
        [TestCase("_МВБФЙЛПЛЯОЫ_ЗФЯ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 2,"КОРЫСТЬ_СЛОНА_ЭХ" )]
        [TestCase("_ГВЫЛЙЛПЛЯОЫ_ЗФЯ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 2,"КОРЫСТЬ_СЛОН__ЭХ" )]
        [TestCase("АХСЕЧРЩТАЛНУЕЛЛО", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 3,"КОРЫСТЬ_СЛОНА_ЭХ" )]
        [TestCase("СГСЕЧРЗТАЛНУЕЛЛН", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 3,"КОРЫСТЬ_СЛОН__ЭХ" )]
        [TestCase("У_АФБГИДЗАНИЯНРЧ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 0,"АБВГДЕЖЗИЙКЛМНОП" )]
        [TestCase("СЛЗ_ОШЩЭЙЭБЭЗЧ_Х", "НТВ_ВСЕ_ЕЩЕ_ТЛЕН", 2,"АБВГДЕЖЗИЙКЛМНОП" )]
        public void RoundSPDecode(string value, string key, int shift, string expected)
        {
            var result = Utils.RoundSPDecode(value, key, shift, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "ЦЙПЮЮФЖЦЫЦПОДММО")]
        [TestCase("КОРЫСТЬ_СЛОНА_ЭХ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 8, "ННЧЦМШЙВГУЬБЕЕИМ")]
        [TestCase("ЛЕРА_КЛОНКА_КОНЯ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "С_ЩПОКЛАЖИЩТЙЗЦВ")]
        [TestCase("ЛЕРА_КЛОНКА_КОНЯ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 8, "ЩЙ_ЖЕПБФЭРЩЛАЯНЭ")]
        [TestCase("АААААААААААААААА", "АААААААААААААААА", 1, "ГВЩЛЫОАТЙОЛБДИЦ_")]
        [TestCase("АААААААААААААААА", "АААААААААААААААА", 8, "ДЬШБОЙПРАУИФПСЛР")]
        [TestCase("ААААААААААААААА_", "________________", 1, "ЧЧИХЬЩСЯ_НРАЗПСР")]
        [TestCase("ААААААААААААААА_", "________________", 8, "ХЬ_ЩКЬФЬГЬДЮШЦФЩ")]
        public void SPNetEncode(string value, string key, int rounds, string expected)
        {
            var result = Utils.SPNetEncode(value, key, rounds, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier, _generator);
            Assert.That(result, Is.EqualTo(expected));
        }


        [TestCase("ЦЙПЮЮФЖЦЫЦПОДММО", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "КОРЫСТЬ_СЛОНА_ЭХ")]
        [TestCase("ННЧЦМШЙВГУЬБЕЕИМ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 8, "КОРЫСТЬ_СЛОНА_ЭХ")]
        [TestCase("С_ЩПОКЛАЖИЩТЙЗЦВ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 1, "ЛЕРА_КЛОНКА_КОНЯ")]
        [TestCase("ЩЙ_ЖЕПБФЭРЩЛАЯНЭ", "МТВ_ВСЕ_ЕЩЕ_ТЛЕН", 8, "ЛЕРА_КЛОНКА_КОНЯ")]
        [TestCase("ГВЩЛЫОАТЙОЛБДИЦ_", "АААААААААААААААА", 1, "АААААААААААААААА")]
        [TestCase("ДЬШБОЙПРАУИФПСЛР", "АААААААААААААААА", 8, "АААААААААААААААА")]
        [TestCase("ЧЧИХЬЩСЯ_НРАЗПСР", "________________", 1, "ААААААААААААААА_")]
        [TestCase("ХЬ_ЩКЬФЬГЬДЮШЦФЩ", "________________", 8, "ААААААААААААААА_")]
        public void SPNetDecode(string value, string key, int rounds, string expected)
        {
            var result = Utils.SPNetDecode(value, key, rounds, _extSBlockModPolyTrithemiusEncoder, _alphabetModifier, _generator);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
