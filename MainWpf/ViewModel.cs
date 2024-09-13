using Core.Alphabet;
using Core.ShiftCipher.Trithemius;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using static MainWpf.MainWindow;


namespace MainWpf
{
    public class ViewModel:INotifyPropertyChanged
    {
        //переменные кодирования
        private string encodeInputText;
        private string encodeOutputText;

        //переменные декодирования
        private string decodeInputText;
        private string decodeOutputText;

        //переменные настройки
        private string key;
        private int shift;

        private readonly SBlockModPolyTrithemiusEncoder<Alphabet> encoder;

        public ViewModel()
        {
            encodeInputText = "";
            encodeOutputText = "";
            decodeInputText = "";
            decodeOutputText = "";
            key = "";
            shift = 0;
            var a = new Alphabet();
            encoder = new(a, new AlphabetModifier<Alphabet>(a));
        }

        public string EncodeInputText
        {
            get => encodeInputText;
            set
            {
                encodeInputText = value;
                OnPropertyChanged();
            }
        }

        public string EncodeOutputText
        {
            get => encodeOutputText;
            private set
            {
                encodeOutputText = value;
                OnPropertyChanged();
            }
        }

        public string DecodeInputText
        {
            get => decodeInputText;
            set
            {
                decodeInputText = value;
                OnPropertyChanged();
            }
        }

        public string DecodeOutputText
        {
            get => decodeOutputText;
            private set
            {
                decodeOutputText = value;
                OnPropertyChanged();
            }
        }

        public string Key
        {
            get => key;
            set
            {
                key = value;
                OnPropertyChanged();
            }
        }

        public int Shift
        {
            get => shift;
            set
            {
                shift = value;
                OnPropertyChanged();
            }
        }

        public ICommand EncodeCommand => new Command(obj =>
        {
            if (string.IsNullOrWhiteSpace(Key))
                EncodeOutputText = "Неверный ключ";
            else
                EncodeOutputText = encoder.Encrypt(EncodeInputText, Key, Shift);
        });
        public ICommand CopyEncodeOutputCommand => new Command(obj =>
        {
            if (!string.IsNullOrWhiteSpace(EncodeOutputText))
                Clipboard.SetText(EncodeOutputText);
        });

        public ICommand DecodeCommand => new Command(obj =>
        {
            if (string.IsNullOrWhiteSpace(Key))
                DecodeOutputText = "Неверный ключ";
            else
                DecodeOutputText = encoder.Decrypt(DecodeInputText, Key, Shift);
        });

        public ICommand CopyDecodeOutputCommand => new Command(obj =>
        {
            if (!string.IsNullOrWhiteSpace(DecodeOutputText))
                Clipboard.SetText(DecodeOutputText);
        });

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
