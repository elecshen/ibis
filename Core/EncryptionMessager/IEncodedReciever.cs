using Core.Alphabet;

namespace Core.EncryptionMessager
{
    public interface IEncodedReciever<T> where T : IAlphabet
    {
        bool Init(string mType, string sender, string reciever, string session, string generatorKey, string nonce = "СЕМИХАТОВ_КВАНТЫ");
        bool Recieve(IEnumerable<bool> bits, out DataPacket<T>? packet);
        bool Send(string message, out IEnumerable<bool> bits);
    }
}