using Core.Alphabet;
using Core.CombinedEncryptor.SPNet;
using Core.Encryptor;
using Core.RandomGenerator;

namespace UATests.TestSuccessor.CombinedEncryptor
{
    public class Test_SPNetCombinedEncryptor<T>(IEncryptor<T> encryptor, IAlphabetModifier<T> modifier, IRandCodeGenerator<T> generator) : SPNetCombinedEncryptor<T>(encryptor, modifier, generator) where T : IAlphabet
    {
        public new string PBlockEncode(string str, int shift) => base.PBlockEncode(str, shift);

        public new string PBlockDecode(string str, int shift) => base.PBlockDecode(str, shift);

        public new string RoundSPEncode(string str, string key, int shift) => base.RoundSPEncode(str, key, shift);

        public new string RoundSPDecode(string str, string key, int shift) => base.RoundSPDecode(str, key, shift);
    }
}
