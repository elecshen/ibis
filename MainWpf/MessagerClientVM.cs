using Core;
using Core.Alphabet;
using Core.CombinedEncryptor.SPNet;
using Core.EncryptionMessager;
using Core.EncryptionMessager.CCM;
using Core.RandomGenerator;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static MainWpf.MainWindow;

namespace MainWpf
{
    public class MessagerClientVM<T> : INotifyPropertyChanged where T : RusAlphabet
    {
        private static readonly List<(string, string)> validEncryptTypes = [ ("Простой текст", "В_" ), ( "Имитовставка", "ВА" ), ( "Шифрование с имитовставкой", "ВБ" ) ];
        public IEnumerable<string> ValidEncryptTypes { get => validEncryptTypes.Select(x => x.Item1); }
        private readonly IAlphabetModifier<T> _modifier;
        private readonly IRandCodeGenerator<T> _generator;
        private readonly IPartialCombinedEncryptor<T> _combinedEncryptor;
        private readonly CCM<T> _ccm;

        public MessagerClientVM(IAlphabetModifier<T> modifier, IRandCodeGenerator<T> generator, IPartialCombinedEncryptor<T> encryptor)
        {
            _modifier = modifier;
            _generator = generator;
            _combinedEncryptor = encryptor;
            _ccm = new(_modifier, _generator, _combinedEncryptor);
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(InterceptedBitsString))
                {
                    _ccm.Recieve(interceptedBits, out var packet);
                    RecievedPacket = packet;
                    if (packet is not null)
                    {
                        CalculatedMessageLength = modifier.TextToNumWithAlphabetBase(packet.HeaderData[4]);
                        CalculatedMessageNumber = modifier.TextToNumWithAlphabetBase(modifier.Xor(packet.InitValue[8..12], packet.InitValue[12..16]));
                    } 
                    else
                    {
                        CalculatedMessageLength = 0;
                        CalculatedMessageNumber = 0;
                    }
                }
            };
        }

        public int EncryptType
        {
            get => validEncryptTypes.FindIndex(x => x.Item2 == encryptType);
            set
            {
                var a = validEncryptTypes.ElementAtOrDefault(value);
                if (!a.Equals(default))
                    encryptType = validEncryptTypes[value].Item2;
                OnPropertyChanged();
            }
        }
        private string encryptType = "В_";
        public string Sender
        {
            get => sender;
            set
            {
                if (Utils.ValidateAndPrepareInput(ref value, 8, _modifier)) 
                    sender = value;
                OnPropertyChanged();
            }
        }
        private string sender = "________";
        public string Recipient
        {
            get => recipient;
            set
            {
                if (!Utils.ValidateAndPrepareInput(ref value, 8, _modifier)) return;
                recipient = value;
                OnPropertyChanged();
            }
        }
        private string recipient = "________";
        public string Session
        {
            get => session;
            set
            {
                if (!Utils.ValidateAndPrepareInput(ref value, 9, _modifier)) return;
                session = value;
                OnPropertyChanged();
            }
        }
        private string session = "_________";
        public string GeneratorKey
        {
            get => generatorKey;
            set
            {
                if (!Utils.ValidateAndPrepareInput(ref value, 16, _modifier)) return;
                generatorKey = value;
                OnPropertyChanged();
            }
        }
        private string generatorKey = "________________";
        public string Message
        {
            get => message;
            set
            {
                if (!Utils.ValidateAndPrepareInput(ref value, -1, _modifier)) return;
                message = value;
                OnPropertyChanged();
            }
        }
        private string message = "";

        public string InterceptedBitsString
        {
            get => string.Join(" ", interceptedBits.Select(b => b ? '1' : '0').Chunk(5).Select(x => new string(x)));
            set
            {
                if(value.All(c => c == '1' || c == '0' || c == ' '))
                    InterceptedBits = value.Replace(" ", "").Select(c => c == '1');
            }
        }
        private IEnumerable<bool> InterceptedBits
        {
            set
            {
                interceptedBits = value;
                interceptedMessage = _modifier.BinToText(interceptedBits);
                OnPropertyChanged(nameof(InterceptedMessage));
                OnPropertyChanged(nameof(InterceptedBitsString));
            }
        }
        private IEnumerable<bool> interceptedBits = [];

        public string InterceptedMessage
        {
            get => interceptedMessage;
            set
            {
                if (!Utils.ValidateAndPrepareInput(ref value, -1, _modifier)) return;
                interceptedMessage = value;
                OnPropertyChanged();
                interceptedBits = _modifier.TextToBin(interceptedMessage);
                OnPropertyChanged(nameof(InterceptedBitsString));
            }
        }
        private string interceptedMessage = "";

        public DataPacket<T>? RecievedPacket
        {
            get => recievedPacket;
            set
            {
                recievedPacket = value;
                OnPropertyChanged();
            }
        }
        private DataPacket<T>? recievedPacket;
        public long CalculatedMessageLength
        {
            get => calculatedMessageLength;
            set {
                calculatedMessageLength = value;
                OnPropertyChanged();
            }
        }
        private long calculatedMessageLength;
        public long CalculatedMessageNumber
        {
            get => calculatedMessageNumber;
            set
            {
                calculatedMessageNumber = value;
                OnPropertyChanged();
            }
        }
        private long calculatedMessageNumber;

        public ICommand InitMessager => new Command(obj => _ccm.Init(encryptType, sender, recipient, session, generatorKey));
        public ICommand SendFromSender => new Command(obj => {
            if (!string.IsNullOrEmpty(Message) && _ccm.Send(Message, out var interceptedBits))
                InterceptedBits = interceptedBits;
        });

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
