using Core.Alphabet;

namespace Core.EncryptionMessager
{
    public class DataPacket<T>(string[] headerData, string initValue, string message, string mac, IAlphabetModifier<T> alphabetModifier) where T : IAlphabet
    {
        protected readonly IAlphabetModifier<T> _alphabetModifier = alphabetModifier;

        public string[] HeaderData = headerData;
        public string InitValue = initValue;
        public string Message = message;
        public string Mac = mac;

        public IEnumerable<bool> ToBits()
        {
            return _alphabetModifier.TextToBin(string.Join("", HeaderData[0], HeaderData[1], HeaderData[2], HeaderData[3], HeaderData[4], InitValue, Message, Mac));
        }

        protected bool IsPadded(IEnumerable<bool> bits, out int blockQuantity, out int padLength)
        {
            blockQuantity = 0;
            padLength = 0;
            var bitArray = bits.ToArray();
            if (bitArray.Length % 80 != 0 // сообщение выравнено до 80 бит
                || bitArray[^3] || bitArray[^2] || !bitArray[^1]) return false; // проверка что конец 001
            blockQuantity = Utils.BinToNum(bitArray[^13..^3], 10);
            padLength = Utils.BinToNum(bitArray[^20..^13], 7);
            if (blockQuantity != bitArray.Length / 80 // количество блоков в записанных подложке совпадает с фактическим
                || padLength < 23 || padLength >= 103 // длина подложки корректна
                || !bitArray[^padLength] // бит начала подложки 1
                || bitArray[^(padLength + 1)..^20].All(x => !x)) return false; // остальные биты должны быть 0
            return true;
        }

        protected IEnumerable<bool> MakePad(int remainder, int blockQuantity)
        {
            int padLength;
            if (remainder == 80)
            {
                blockQuantity++;
                padLength = 80;
            }
            else if (remainder <= 57) // 80 - 23 где 23 минимальная длина подложки
            {
                blockQuantity++;
                padLength = 80 - remainder;
            }
            else
            {
                blockQuantity += 2;
                padLength = 160 - remainder;
            }
            var pad = new bool[padLength];
            pad[0] = true;
            pad[^1] = true;
            int i = 0;
            foreach (var item in Utils.NumToBin(blockQuantity, 10))
                pad[^(13 - i++)] = item;
            i = 0;
            foreach (var item in Utils.NumToBin(padLength, 7))
                pad[^(20 - i++)] = item;
            return pad;
        }

        public void PadMessage()
        {
            var bits = _alphabetModifier.TextToBin(Message);
            int remainder = bits.Count() % 80;
            int blockQuantity = bits.Count() / 80;
            if (remainder != 0 || IsPadded(bits, out _, out _))
                Message = _alphabetModifier.BinToText(bits.Concat(MakePad(remainder, blockQuantity)));
        }

        public void UnpadMessage()
        {
            var bits = _alphabetModifier.TextToBin(Message);
            if (IsPadded(bits, out _, out int padLength))
                Message = _alphabetModifier.BinToText(bits.SkipLast(padLength));
        }
    }
}
