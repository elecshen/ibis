using Core.Alphabet;
using Core.ShiftCipher;
using Core.ShiftCipher.Trithemius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static MainWpf.MainWindow;


namespace MainWpf
{
    public class ViewModel:INotifyPropertyChanged
    {

        //переменные кодирования
        private string _inputText;
        private string _outputText;

        //переменные декодирования
        private string _input2Text;
        private string _output2Text;

        //переменные настройки
        public string _key;
        public int _shift;



        private SBlockModPolyTrithemiusEncoder encoder = new(new Alphabet());


       public event PropertyChangedEventHandler? PropertyChanged;

        public string InputText
        {
            get => _inputText;
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                    OnPropertyChanged(nameof(InputText));
                }
            }
        }

        public string EncryptedResult
        {
            get => _outputText;
            private set
            {
                _outputText = value;
                OnPropertyChanged(nameof(EncryptedResult));
            }
        }


        public string Input2Text
        {
            get => _input2Text;
            set
            {
                if (_input2Text != value)
                {
                    _input2Text = value;
                    OnPropertyChanged(nameof(Input2Text));
                }
            }
        }

        public string DecryptedResult
        {
            get => _output2Text;
            private set
            {
                _output2Text = value;
                OnPropertyChanged(nameof(DecryptedResult));
            }
        }



        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(Key));
            }
        }
        public int Shift
        {
            get => _shift;
            set
            {
                {
                    _shift = value;
                    OnPropertyChanged(nameof(Key));
                }
            }
        }


        public ICommand EncodeCommand => new RelayCommand(EncryptInput);

        private void EncryptInput()
        {
            if (!string.IsNullOrWhiteSpace(InputText))
            {
                string K = "ЧРОТ";
                string I = "РОЗА";
                int S = 0;

                if (Key != null) {
                    K = Key;
                }

                if (InputText != null)
                {
                    I = InputText;
                }

                if (Shift != null) 
                {
                    S = Shift;
                }

                var alf = encoder.Encrypt(I,K,S);
                EncryptedResult = alf;
            }
        }
        public ICommand DecodeCommand => new RelayCommand(DecryptInput);
        private void DecryptInput()
        {
            if (!string.IsNullOrWhiteSpace(Input2Text))
            {
                string K = "ЧРОТ";
                string I = "РОЗА";
                int S = 0;

                if (Key != null)
                {
                    K = Key;
                }

                if (Input2Text != null)
                {
                    I = Input2Text;
                }

                if (Shift != null)
                {
                    S = Shift;
                }
                

                var alf = encoder.Decrypt(I, K, S);
                DecryptedResult = alf;
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
