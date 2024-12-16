using Core.Alphabet;

namespace Core.CombinedEncryptor.SPNet
{
    public interface IPartialCombinedEncryptor<T> where T : IAlphabet
    {
        string[] ProduceRoundsKeys(string key, int rounds);
        string RoundsEncrypt(string str, string[] keySet, int rounds);
        string RoundsDecrypt(string str, string[] keySet, int rounds);
    }
}