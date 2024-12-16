using Core.Alphabet;
using Core.EncryptionMessager;

namespace UATests.TestSuccessor.EncryptionMessager
{
    public class Test_DataPacket<T>(string[] headerData, string initValue, string message, string mac, IAlphabetModifier<T> modifier)
    : DataPacket<T>(headerData, initValue, message, mac, modifier) where T : IAlphabet
    {
        public new bool IsPadded(IEnumerable<bool> bits, out int blockQuantity, out int padLength)
            => base.IsPadded(bits, out blockQuantity, out padLength);

        public new IEnumerable<bool> MakePad(int remainder, int blockQuantity)
            => base.MakePad(remainder, blockQuantity);
    }
}
