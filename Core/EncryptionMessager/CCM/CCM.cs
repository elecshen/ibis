using Core.Alphabet;
using Core.CombinedEncryptor.SPNet;
using Core.RandomGenerator;

namespace Core.EncryptionMessager.CCM
{
    public class CCM<T>(IAlphabetModifier<T> alphabetModifier, IRandCodeGenerator<T> generator, IPartialCombinedEncryptor<T> combinedEncryptor) : IEncodedReciever<T> where T : IAlphabet
    {
        protected readonly IAlphabetModifier<T> _alphabetModifier = alphabetModifier;
        protected readonly IRandCodeGenerator<T> _generator = generator;
        protected readonly IPartialCombinedEncryptor<T> _combinedEncryptor = combinedEncryptor;
        protected readonly DataPacketFactory<T> packerFactory = new(alphabetModifier);

        protected readonly string[] _validMTypes = ["В_", "ВА", "ВБ"];
        protected string[] _headerData = ["__", "________", "________", "_________", "_____"];
        protected string _initialValue = "________________";
        protected string[] _roundKeys = [];

        protected int _messagesTransmited = -1;

        protected int _lastRecieved = -1;

        protected string CTR(string message, string initValue)
        {
            int m = message.Length / 16;
            string iv_start = initValue[..12];
            string iv_end;
            string res = "";
            for (int i = 0; i < m; i++)
            {
                iv_end = _alphabetModifier.NumWithAlphabetBaseToText(i);
                string keyStream = _combinedEncryptor.RoundsEncrypt(iv_start + iv_end, _roundKeys, 8);
                res += _alphabetModifier.Xor(message[(i * 16)..(i * 16 + 16)], keyStream);
            }
            return res;
        }

        protected string Mac_CBC(string message, string initValue)
        {
            int m = message.Length / 16;
            string tmp;
            for (int i = 0; i < m; i++)
            {
                tmp = _alphabetModifier.Xor(initValue, message[(i * 16)..(i * 16 + 16)]);
                initValue = _combinedEncryptor.RoundsEncrypt(tmp, _roundKeys, 8);
            }
            return initValue;
        }

        protected void CCMEncode(DataPacket<T> packet, bool onlyMac)
        {
            string mac = Mac_CBC(string.Join("", packet.HeaderData) + packet.Message, packet.InitValue);
            if (onlyMac)
            {
                packet.Mac = mac;
            }
            else
            {
                string msg = CTR(packet.Message + mac, packet.InitValue);
                int l = packet.Message.Length;
                packet.Message = msg[..l];
                packet.Mac = msg[l..];
            }
        }

        protected void CCMDecode(DataPacket<T> packet, bool onlyMac)
        {
            if (!onlyMac)
            {
                string msg = CTR(packet.Message + packet.Mac, packet.InitValue);
                int l = packet.Message.Length;
                packet.Message = msg[..l];
                packet.Mac = msg[l..];
            }
            string mac = Mac_CBC(string.Join("", packet.HeaderData) + packet.Message, packet.InitValue);
            packet.Mac = _alphabetModifier.Xor(packet.Mac, mac);
        }

        public bool Init(string mType, string sender, string reciever, string session, string generatorKey, string nonce = "СЕМИХАТОВ_КВАНТЫ")
        {
            if (!_validMTypes.Contains(mType)) return false;
            _headerData[0] = mType;
            _headerData[1] = sender;
            _headerData[2] = reciever;
            _headerData[3] = session;
            _generator.Init(generatorKey);
            for (int i = new Random().Next(50, 100); i > 0; i--)
                _generator.Next();
            nonce = _generator.Next();

            string t = _alphabetModifier.SumString(_alphabetModifier.SumString(reciever + sender, mType + session + "_____"), nonce);
            _initialValue = t[0..8] + t[12..16] + t[12..16];
            _messagesTransmited = -1;
            _roundKeys = _combinedEncryptor.ProduceRoundsKeys(generatorKey, 8);
            return true;
        }

        public bool Send(string message, out IEnumerable<bool> bits)
        {
            if (_roundKeys.Length == 0)
            {
                bits = [];
                return false;
            }
            _messagesTransmited++;
            string IV = _alphabetModifier.Xor(_initialValue, "________" + _alphabetModifier.NumWithAlphabetBaseToText(_messagesTransmited) + "____");
            var packet = packerFactory.MakePacket(_headerData, IV, message);
            if (_headerData[0] != _validMTypes[0])
                CCMEncode(packet, _headerData[0] == _validMTypes[1]);
            bits = packet.ToBits();
            return true;
        }

        public bool Recieve(IEnumerable<bool> bits, out DataPacket<T>? packet)
        {
            packet = null;
            if (_roundKeys.Length == 0) return false;

            packet = packerFactory.FromBits(bits);
            if (!packet.Validate()) return false;
            var currentMessageNumber = _alphabetModifier.TextToNumWithAlphabetBase(_alphabetModifier.Xor(packet.InitValue[8..12], packet.InitValue[12..16]));
            if (currentMessageNumber <= _lastRecieved) return false;

            if (packet.HeaderData[0] != _validMTypes[0])
                CCMDecode(packet, _headerData[0] == _validMTypes[1]);
            packet.UnpadMessage();
            if (packet.Mac == "") packet.Mac = "N/A";
            if (packet.Mac == "________________") packet.Mac = "OK";
            return true;
        }
    }
}
