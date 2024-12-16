using Core;
using Core.Alphabet;
using Core.EncryptionMessager;
using Core.RandomGenerator.LCG;
using UATests.TestSuccessor.CombinedEncryptor;
using UATests.TestSuccessor.EncryptionMessager;
using UATests.TestSuccessor.Encryptor;
using UATests.TestSuccessor.RandomGenerator;

namespace UATests
{
    public class LB4_CCM
    {
        private static readonly RusAlphabet _alphabet;
        private static readonly AlphabetModifier<RusAlphabet> _modifier;
        private static readonly Test_ExtSBlockModPolyTrithemiusEncryptor<RusAlphabet> _encryptor;
        private static readonly Test_CHCLCGM<RusAlphabet> _generator;
        private static readonly Test_SPNetCombinedEncryptor<RusAlphabet> _combinedEncryptor;
        private static readonly DataPacketFactory<RusAlphabet> _factory;
        private static readonly Test_CCM<RusAlphabet> _CCM;
        private readonly static string[] input_data;
        private readonly static string[][] assoc_data;

        static LB4_CCM()
        {
            input_data = File.ReadLines("./TestData/inp.txt").ToArray();
            assoc_data = File.ReadLines("./TestData/ad.txt").Select(s => (s.Replace("\"", "") + " АБВГД").Split(' ')).ToArray();
            _alphabet = new();
            _modifier = new(_alphabet);
            _encryptor = new(_alphabet, _modifier);
            _generator = new(_encryptor, _modifier, LCGCoeffs.DefaultCoeffs);
            _combinedEncryptor = new(_encryptor, _modifier, _generator);
            _factory = new(_modifier);
            _CCM = new(_modifier, _generator, _combinedEncryptor);
        }

        [SetUp]
        public void Setup()
        {
        }

        private static IEnumerable<TestCaseData> TextToBinData()
        {
            return [
                new TestCaseData(input_data[0], 1840),
                new TestCaseData(input_data[1], 15395),
                new TestCaseData(input_data[2], 2595),
                new TestCaseData(input_data[3], 138),
                new TestCaseData(input_data[4], 58),
                new TestCaseData(input_data[5], 1555),
                ];
        }

        [TestCase("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ", 145)]
        [TestCase("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ0011", 149)]
        [TestCase("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ1110011011011", 158)]
        [TestCaseSource(nameof(TextToBinData))]
        public void TextToBin(string value, int expected)
        {
            var result = _modifier.TextToBin(value);
            Assert.That(result.Count(), Is.EqualTo(expected));
        }

        [TestCase("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ", "ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ")]
        [TestCase("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ00111", "ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМЖ")]
        [TestCase("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМ1110011011011", "ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМЬЫ011")]
        [TestCase("ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМЬЫ011", "ГНОЛЛЫ_ПИЛИЛИ_ПЫЛЕСОС_ЛОСОСЕМЬЫ011")]
        public void BinToText(string value, string expected)
        {
            var result = _modifier.BinToText(_modifier.TextToBin(value));
            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> PadMessageData()
        {
            return [
                new TestCaseData(input_data[0], 1840, false, 0, 0),
                new TestCaseData(input_data[1], 15440, true, 193, 45),
                new TestCaseData(input_data[2], 2640, true, 33, 45),
                new TestCaseData(input_data[3], 240, true, 3, 102),
                ];
        }

        [TestCaseSource(nameof(PadMessageData))]
        public void PadMessage(string value, int expectedLegth, bool expectedIsPadded, int expectedBlockQuantity, int expectedPadLength)
        {
            var packet = new Test_DataPacket<RusAlphabet>([], "", value, "", _modifier);
            packet.PadMessage();
            var bits = _modifier.TextToBin(packet.Message);
            var isPadded = packet.IsPadded(bits, out int blockQuantity, out int padLength);

            Assert.Multiple(() =>
            {
                Assert.That(bits.Count(), Is.EqualTo(expectedLegth));
                Assert.That(isPadded, Is.EqualTo(expectedIsPadded));
                Assert.That(blockQuantity, Is.EqualTo(expectedBlockQuantity));
                Assert.That(padLength, Is.EqualTo(expectedPadLength));
            });
        }

        private static IEnumerable<TestCaseData> UnpadMessageData()
        {
            return [
                new TestCaseData(input_data[0], 1840, false, 0, 0),
                new TestCaseData(input_data[1], 15395, false, 0, 0),
                new TestCaseData(input_data[2], 2595, false, 0, 0),
                new TestCaseData(input_data[3], 138, false, 0, 0),
                ];
        }

        [TestCaseSource(nameof(UnpadMessageData))]
        public void UnpadMessage(string value, int expectedLegth, bool expectedIsPadded, int expectedBlockQuantity, int expectedPadLength)
        {
            var packet = new Test_DataPacket<RusAlphabet>([], "", value, "", _modifier);
            packet.PadMessage();
            packet.UnpadMessage();
            var bits = _modifier.TextToBin(packet.Message);
            var isPadded = packet.IsPadded(bits, out int blockQuantity, out int padLength);

            Assert.Multiple(() =>
            {
                Assert.That(bits.Count(), Is.EqualTo(expectedLegth));
                Assert.That(isPadded, Is.EqualTo(expectedIsPadded));
                Assert.That(blockQuantity, Is.EqualTo(expectedBlockQuantity));
                Assert.That(padLength, Is.EqualTo(expectedPadLength));
            });
        }
        private static IEnumerable<TestCaseData> DoublePadMessageData()
        {
            return [
                new TestCaseData(input_data[1], 15520, true, 194, 80),
                new TestCaseData(input_data[2], 2720, true, 34, 80),
                ];
        }

        [TestCaseSource(nameof(DoublePadMessageData))]
        public void DoublePadMessage(string value, int expectedLegth, bool expectedIsPadded, int expectedBlockQuantity, int expectedPadLength)
        {
            var packet = new Test_DataPacket<RusAlphabet>([], "", value, "", _modifier);
            packet.PadMessage();
            packet.PadMessage();
            var bits = _modifier.TextToBin(packet.Message);
            var isPadded = packet.IsPadded(bits, out int blockQuantity, out int padLength);
            packet.UnpadMessage();
            packet.UnpadMessage();
            var unpadedValue = packet.Message;

            Assert.Multiple(() =>
            {
                Assert.That(bits.Count(), Is.EqualTo(expectedLegth));
                Assert.That(isPadded, Is.EqualTo(expectedIsPadded));
                Assert.That(blockQuantity, Is.EqualTo(expectedBlockQuantity));
                Assert.That(padLength, Is.EqualTo(expectedPadLength));
                Assert.That(unpadedValue, Is.EqualTo(value));
            });
        }

        private static IEnumerable<TestCaseData> TransmitRecieveData()
        {
            return [
                new TestCaseData(assoc_data[0], "КОЛЕСО", input_data[1]),
                new TestCaseData(assoc_data[1], "КОЛЕСО", input_data[1]),
                new TestCaseData(assoc_data[1], "КОЛЕСО", input_data[0]),
                ];
        }


        [TestCaseSource(nameof(TransmitRecieveData))]
        public void TransmitRecieve(string[] assocData, string initValue, string message)
        {
            var packet = _factory.MakePacket(assocData, initValue, message);
            var bits = packet.ToBits();
            var recievedPacket = _factory.FromBits(bits);
            var recievedMessage = recievedPacket.Message;
            recievedPacket.UnpadMessage();

            Assert.Multiple(() =>
            {
                Assert.That(recievedPacket.Mac, Is.EqualTo(packet.Mac));
                Assert.That(recievedPacket.InitValue, Is.EqualTo(packet.InitValue));
                Assert.That(recievedMessage, Is.EqualTo(packet.Message));
                CollectionAssert.AreEqual(recievedPacket.HeaderData, packet.HeaderData);
                Assert.That(recievedPacket.Message, Is.EqualTo(message));
            });
        }

        [TestCase("ГОЛОВКА_КРУЖИТСЯ", "МЫШКА_БЫЛА_ЛИХОЙ", "ИУФГБКВЫЖПУК_ДЭФ")]
        [TestCase("ИУФГБКВЫЖПУК_ДЭФ", "МЫШКА_БЫЛА_ЛИХОЙ", "ГОЛОВКА_КРУЖИТСЯ")]
        [TestCase("ИУФГБКВЫЖПУК_ДЭФ", "ГОЛОВКА_КРУЖИТСЯ", "МЫШКА_БЫЛА_ЛИХОЙ")]
        [TestCase("СИНЕВАТАЯ_БОРОДА", "ЗЕЛЕНЫЙ_КОТОЗМИЙ", "ЩОБ_МЩШАУОР_ШБЛК")]
        [TestCase("ЩОБ_МЩШАУОР_ШБЛК", "ЗЕЛЕНЫЙ_КОТОЗМИЙ", "СИНЕВАТАЯ_БОРОДА")]
        [TestCase("ЩОБ_МЩШАУОР_ШБЛК", "СИНЕВАТАЯ_БОРОДА", "ЗЕЛЕНЫЙ_КОТОЗМИЙ")]
        [TestCase("ЩОБ_МЩШАУОР_ШБЛК", "МЫШКА_БЫЛА_ЛИХОЙ", "ЦУЫКЛЩЫЩЧНРЛПУВА")]
        [TestCase("КОНЬ", "А__Г", "ЙОНЧ")]
        [TestCase("КОНЬ", "АБВГ", "ЙММЧ")]
        [TestCase("КОНЬ", "ЛУНЬ", "ЖЫ__")]
        [TestCase("КААА", "АБВГ", "ЙВБД")]
        public void XorBlock(string str1, string str2, string expected)
        {
            var res = _modifier.Xor(str1, str2);

            Assert.That(res, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> CTRData()
        {
            return [
                new TestCaseData(input_data[0], "АЛИСА_УМЕЕТ_ПЕТЬ", "СЕАНСОВЫЙ_КЛЮЧИК", "УЛЙЦИСУТЖМСНЛПАЯТЙРЯЬК_НН_ЦЭЩТСКАТХКМСВЩЭМСВЯХНЛБНЕЭДЬЯНРБЧЫШЬЦНКЙБГРЬ_ЩВЩВЗ_ТУКЮБЖЭУЩЙЧМЭМ_НЛФЭМЧМШЭВМЭЮХМИЕПЗЭОДВЖЫЯЮЫЮГХЮЕДЩХБГСТЛГЗХЭЛЛБШБ_ЙСЦКЗБЦФКДВЧВЧИШРЗФЛЗВЩГЫЛДЮЩЩЩГЩЧЦДЗФТЫНТВРЯСЙЬЦУ_ЮЖЩДЙПЮЫИХА_ЯКЯГЫЗИИЖЛЬЙЖФПЦСЯИИФЛЗРКОГЖАЬМВЯВЦЭЖЫСДСБРЗЛГДСХЩДГВЫДАЛЛЦЛЛЮСЖЛЩФЭФУСЕХИНЫЛЧИЧММ_ОЕЯШЫЯВЗГЛЯЮ_КМЖДАЖАЖОТЙЫЭЖФКЮ_ХЫГТСЭОРЛЦ_ТРБГБЙ_ЗГШСДННОСЬТНАОУРЫЕЕЖНСЛННМЫКЮМ"),
                new TestCaseData(input_data[0], "БОБ_НЕМНОГО_ПЬЯН", "СЕАНСОВЫЙ_КЛЮЧИК", "ЕРЛКЛ_МПСАБКМЙУБНХЬ_Ф_ЗУЛЮНЗИННМБСМЩШЗЙИЩЫУЕЛСБ_КЭДЧЖПГХСЧФШМЖИЬ_ТЯФВЖЗФЗФТЖЮВБЧКЭЗЮЮШШШЯФППИШЧРШГЭЕЭКЫЖОАГЯ__ТЦГЕЦЕП_ЬНЗНДБЫМГЦЩОМ_ЗУСКАЩЖОСИС_САЗВХЬЛЬИЛЫЦМШЙЬЯЙГДКНТВЭАЧФИЬХХВЫЗСМЫШ_ЫЬЫБТЬЦСЫЫДЕЕВВЩЕБНСППАЯНЩ_ЯКЖЦ_А_ФККВНЗПЛГЙЛЖДБЩЭИРЭЦФЗЦШЮБОСЗБУТЬПТРЦЖУУМЭАФЫЖСЯЮНХДТГЫОТИДХЖМКБЭ_ХЫДЫЙЬЯТНЬПЖЬ_ЕНГФЖЧЭ_ЦО_НИЕРЯХСЙИЮЕЦВАЛЭАВРШУХЗЧЬУГЗММТЛГСМАЭУОДЬЕХ_НЩПЯУМД_РРНХБЯД"),
                ];
        }

        [TestCaseSource(nameof(CTRData))]
        public void CTR(string message, string initValue, string key, string expected)
        {
            _CCM.Init("", "", "", "", key);
            var res = _CCM.CTR(message, initValue);

            Assert.That(res, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(CTRData))]
        public void DoubleCTR(string message, string initValue, string key, string _)
        {
            _CCM.Init("", "", "", "", key);
            var res = _CCM.CTR(message, initValue);
            var expected = _CCM.CTR(res, initValue);
            var newIV = initValue[..12] + "____";
            var expected2 = _CCM.CTR(res, newIV);

            Assert.That(message, Is.EqualTo(expected));
            Assert.That(message, Is.EqualTo(expected2));
        }

        private static IEnumerable<TestCaseData> Mac_CBCData()
        {
            return [
                new TestCaseData(input_data[0], "АЛИСА_УМЕЕТ_ПЕТЬ", "СЕАНСОВЫЙ_КЛЮЧИК", "ОФЦЫГЕОРЭЖЧШЖУГ_"),
                new TestCaseData(input_data[0], "БОБ_НЕМНОГО_ПЬЯН", "СЕАНСОВЫЙ_КЛЮЧИК", "ХГУ_ЭОИФЯТЗЖЙФЕЯ"),
                new TestCaseData(input_data[0], "БОБ_НЕМНОГО_УНЫЛ", "СЕАНСОВЫЙ_КЛЮЧИК", "ЯРДЬПЯЮРЮВМЮЙК_К"),
                ];
        }

        [TestCaseSource(nameof(Mac_CBCData))]
        public void Mac_CBC(string message, string initValue, string key, string expected)
        {
            _CCM.Init("", "", "", "", key);
            var res = _CCM.Mac_CBC(message, initValue);

            Assert.That(res, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> CCMEncodeData()
        {
            return [
                new TestCaseData(new DataPacket<RusAlphabet>(assoc_data[1], "БОБ_НЕМНОГО_ПЬЯН", input_data[0], "", _modifier), "СЕАНСОВЫЙ_КЛЮЧИК", false, "ЕРЛКЛ_МПСАБКМЙУБНХЬ_Ф_ЗУЛЮНЗИННМБСМЩШЗЙИЩЫУЕЛСБ_КЭДЧЖПГХСЧФШМЖИЬ_ТЯФВЖЗФЗФТЖЮВБЧКЭЗЮЮШШШЯФППИШЧРШГЭЕЭКЫЖОАГЯ__ТЦГЕЦЕП_ЬНЗНДБЫМГЦЩОМ_ЗУСКАЩЖОСИС_САЗВХЬЛЬИЛЫЦМШЙЬЯЙГДКНТВЭАЧФИЬХХВЫЗСМЫШ_ЫЬЫБТЬЦСЫЫДЕЕВВЩЕБНСППАЯНЩ_ЯКЖЦ_А_ФККВНЗПЛГЙЛЖДБЩЭИРЭЦФЗЦШЮБОСЗБУТЬПТРЦЖУУМЭАФЫЖСЯЮНХДТГЫОТИДХЖМКБЭ_ХЫДЫЙЬЯТНЬПЖЬ_ЕНГФЖЧЭ_ЦО_НИЕРЯХСЙИЮЕЦВАЛЭАВРШУХЗЧЬУГЗММТЛГСМАЭУОДЬЕХ_НЩПЯУМД_РРНХБЯД", "ХГ_ЫМЭРРЩЙИЧЬЙМВ"),
                ];
        }

        [TestCaseSource(nameof(CCMEncodeData))]
        public void CCMEncode(DataPacket<RusAlphabet> packet, string key, bool isMacOnly, string expectedMessage, string expectedMac)
        {
            _CCM.Init("", "", "", "", key);
            _CCM.CCMEncode(packet, isMacOnly);

            Assert.Multiple(() =>
            {
                Assert.That(packet.Message, Is.EqualTo(expectedMessage));
                Assert.That(packet.Mac, Is.EqualTo(expectedMac));
            });
        }

        private static IEnumerable<TestCaseData> CCMEncodeDecodeData()
        {
            return [
                new TestCaseData(new DataPacket<RusAlphabet>(assoc_data[1], "БОБ_НЕМНОГО_ПЬЯН", input_data[0], "", _modifier), "СЕАНСОВЫЙ_КЛЮЧИК", false),
                new TestCaseData(new DataPacket<RusAlphabet>(assoc_data[1], "БОБ_НЕМНОГО_ПЬЯН", input_data[0], "", _modifier), "СЕАНСОВЫЙ_КЛЮЧИК", true),
                ];
        }

        [TestCaseSource(nameof(CCMEncodeDecodeData))]
        public void CCMEncodeDecode(DataPacket<RusAlphabet> packet, string key, bool isMacOnly)
        {
            _CCM.Init("", "", "", "", key);
            var message = packet.Message;
            _CCM.CCMEncode(packet, isMacOnly);
            _CCM.CCMDecode(packet, isMacOnly);

            Assert.Multiple(() =>
            {
                Assert.That(packet.Message, Is.EqualTo(message));
                Assert.That(packet.Mac, Is.EqualTo("________________"));
            });
        }

        private static IEnumerable<TestCaseData> CCMSendResieveData()
        {
            assoc_data[3][4] = "ЭКЛАМПСИЯ";
            return [
                new TestCaseData(assoc_data[3], "СЕАНСОВЫЙ_КЛЮЧИК", input_data[0], "OK"),
                new TestCaseData(assoc_data[3], "СЕАНСОВЫЙ_КЛЮЧИК", input_data[1], "OK"),
                new TestCaseData(assoc_data[3], "СЕАНСОВЫЙ_КЛЮЧИК", input_data[2], "OK"),
                new TestCaseData(assoc_data[3], "СЕАНСОВЫЙ_КЛЮЧИК", input_data[4], "OK"),
                new TestCaseData(assoc_data[3], "СЕАНСОВЫЙ_КЛЮЧИК", input_data[5], "OK"),
                ];
        }

        [TestCaseSource(nameof(CCMSendResieveData))]
        public void CCMSendResieve(string[] assoc_data, string key, string message, string expectedMac)
        {
            _CCM.Init(assoc_data[0], assoc_data[1], assoc_data[2], assoc_data[3], key);
            _CCM.Send(message, out var bits);
            _CCM.Recieve(bits, out var packet);
            Assert.Multiple(() =>
            {
                Assert.That(packet!.Message, Is.EqualTo(message));
                Assert.That(packet.Mac, Is.EqualTo(expectedMac));
            });
        }
    }
}
