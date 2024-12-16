using Core.Alphabet;

namespace Core.EncryptionMessager
{
    public class DataPacketFactory<T>(IAlphabetModifier<T> alphabetModifier) where T : IAlphabet
    {
        protected readonly IAlphabetModifier<T> _alphabetModifier = alphabetModifier;

        public DataPacket<T> MakePacket(string[] headerData, string initValue, string message)
        {
            initValue = _alphabetModifier.SumString("________________", initValue);
            var packet = new DataPacket<T>(headerData, initValue, message, "", _alphabetModifier);
            packet.PadMessage();
            long l = packet.Message.Length * 5;
            string a = "";
            for (int i = 0; i < 4; i++)
            {
                a = _alphabetModifier.Alphabet[(int)l] + a;
                l /= _alphabetModifier.Alphabet.Length;
            }
            packet.HeaderData[4] = a;
            return packet;
        }

        public DataPacket<T> FromBits(IEnumerable<bool> bits)
        {
            string str = _alphabetModifier.BinToText(bits);
            int l = 0;
            for (int i = 0; i < 4; i++)
            {
                l *= _alphabetModifier.Alphabet.Length;
                l += _alphabetModifier.Alphabet[str[27 + i]];
            }
            l /= 5;
            return new([str[0..2], str[2..10], str[10..18], str[18..27], str[27..31]], str[31..47], str[47..(47 + l)], str[(47 + l)..], _alphabetModifier);
        }
    }
}
