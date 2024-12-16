using Core.Alphabet;
using Core.CombinedEncryptor.SPNet;
using Core.EncryptionMessager.CCM;
using Core.EncryptionMessager;
using Core.RandomGenerator;

namespace UATests.TestSuccessor.EncryptionMessager
{
    public class Test_CCM<T>(IAlphabetModifier<T> modifier, IRandCodeGenerator<T> generator, IPartialCombinedEncryptor<T> combinedEncryptor)
    : CCM<T>(modifier, generator, combinedEncryptor) where T : IAlphabet
    {
        public new string CTR(string message, string initValue) => base.CTR(message, initValue);

        public new string Mac_CBC(string message, string initValue) => base.Mac_CBC(message, initValue);

        public new void CCMEncode(DataPacket<T> packet, bool onlyMac) => base.CCMEncode(packet, onlyMac);

        public new void CCMDecode(DataPacket<T> packet, bool onlyMac) => base.CCMDecode(packet, onlyMac);
    }
}
