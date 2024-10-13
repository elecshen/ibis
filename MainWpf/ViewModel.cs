using Core;
using Core.Alphabet;
using Core.RandomGenerator;
using Core.ShiftCipher.Trithemius;
using System.ComponentModel;
using System.IO;
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
        private string inputblocklab3;
 
        private string outputencodelab3;
        private string outputdecodelab3;


        //переменные декодирования
        private string decodeInputText;
        private string decodeOutputText;

        //переменные настройки
        private string key;
        private int shift;
        private int rounds;
        private string keylab3;
        

        // переменные генератора случайных чисел
        private string seed;
        private string generatedRandomOutput;
        private string generatedRandomNumericOutput;

        // Переменные управления интерфейсом
        private Visibility genValueButtonVisibility; 

        // расширенный энкодер
        private readonly ExtSBlockModPolyTrithemiusEncoder<RusAlphabet> encoder;

        // модификатор алфавита
        private AlphabetModifier<RusAlphabet> alphabetModifier;

        // генератор
        private readonly CHCLCGM<RusAlphabet> generator;

        // коэффициенты для LCG генератора
        private LCGCoeffs[] coeffs = [new(723482, 8677, 983609), new(252564, 9109, 961193), new(357630, 8971, 948209)];

        public ViewModel()
        {
            encodeInputText = "";
            encodeOutputText = "";
            decodeInputText = "";
            decodeOutputText = "";
            inputblocklab3 = "";
            outputencodelab3 = "";
            outputdecodelab3 = "";
            key = "";
            keylab3 = "";
            shift = 0;
            rounds = 0;
            var a = new RusAlphabet();
            alphabetModifier = new AlphabetModifier<RusAlphabet>(a);
            encoder = new(a, alphabetModifier);
            genValueButtonVisibility = Visibility.Hidden;
            generator = new(encoder, alphabetModifier);

        }

        public Visibility GenValueButtonVisibility
        {
            get => genValueButtonVisibility;
            set
            {
                genValueButtonVisibility = value;
                OnPropertyChanged();
            }
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
        public string Seed
        {
            get => seed;
            set
            {
                seed = value;
                OnPropertyChanged();
            }
        }
        public string GeneratedRandomOutput
        {
            get => generatedRandomOutput;
            set
            {
                generatedRandomOutput = value;
                OnPropertyChanged();
            }
        }
        public string GeneratedRandomNumericOutput
        {
            get => generatedRandomNumericOutput;
            set
            {
                generatedRandomNumericOutput = value;
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
        public string KeyLab3
        {
            get => keylab3;
            set
            {
                keylab3 = value;
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

        public string InputBlockLab3
        {
            get => inputblocklab3;
            set
            {
                inputblocklab3 = value;
                OnPropertyChanged();
            }
        }

        public string OutputEncodeLab3
        {
            get => outputencodelab3;
            set
            {
                outputencodelab3 = value;
                OnPropertyChanged();
            }
        }
        public string OutputDecodeLab3
        {
            get => outputdecodelab3;
            set
            {
                outputdecodelab3 = value;
                OnPropertyChanged();
            }
        }


        public int RoundsLab3
        {
            get => rounds;
            set
            {
                rounds = value;
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
        public ICommand CreateGeneratorCommand => new Command(obj =>
        {
            if (!string.IsNullOrWhiteSpace(Seed) 
            && Seed.Length == 16 
            && !Seed.ToUpper().Contains("Ё") 
            && !Seed.ToUpper().Contains("Ъ"))
            {
                generator.Init(Seed, coeffs);
                GenValueButtonVisibility = Visibility.Visible;

            }
            else MessageBox.Show("Неверный ввод значения инициализации! Требования:" +
                    "\n- длина - 16 символов," +
                    "\n- символы русского алфавита" + 
                    "\n- не содержит букв `ё` и `ъ`.");
        });
        public ICommand GenerateNextValueCommand => new Command(obj =>
        {
            GeneratedRandomOutput = generator.Next();
            GeneratedRandomNumericOutput = ((ulong)alphabetModifier.TextToBaseNum(generatedRandomOutput)).ToString();
        });
        //Lab3

        public ICommand EncodeCommandLab3 => new Command(obj =>
        {
            OutputEncodeLab3 = Utils.SPNetEncode(InputBlockLab3, KeyLab3, RoundsLab3, encoder, alphabetModifier, generator);


        });

        public ICommand DecodeCommandLab3 => new Command(obj =>
        {
            OutputDecodeLab3 = Utils.SPNetDecode(InputBlockLab3, KeyLab3, RoundsLab3, encoder, alphabetModifier, generator);
        });


        #region ИССЛЕДОВАНИЕ
        public ICommand Generate100ValuesForSeedCommand => new Command(obj =>
        {
            if (!string.IsNullOrWhiteSpace(Seed)
            && Seed.Length == 16
            && !Seed.ToUpper().Contains("Ё")
            && !Seed.ToUpper().Contains("Ъ"))
            {
                generator.Init(Seed, coeffs);
                var numbers = GenerateValidRandomNumbers(generator, 4000);
                SaveNumbersToFile(numbers, $"{Seed}_numbers.txt");
            }
            else
            {
                MessageBox.Show("Неверный ввод значения инициализации! Требования:" +
                                "\n- длина - 16 символов," +
                                "\n- символы русского алфавита" +
                                "\n- не содержит букв `ё` и `ъ`.");
            }
        });

        private List<ulong> GenerateValidRandomNumbers(CHCLCGM<RusAlphabet> generator, int count)
        {
            List<ulong> validNumbers = new List<ulong>();
            while (validNumbers.Count < count)
            {
                var randomText = generator.Next();
                ulong numericValue = (ulong)alphabetModifier.TextToBaseNum(randomText);

                var numericString = numericValue.ToString();

                if (numericValue > 0 /*&& numericString.Length == 16*/&& numericValue <= ulong.MaxValue)
                {
                    validNumbers.Add((ulong)numericValue);
                }
            }
            return validNumbers;
        }
        private void SaveNumbersToFile(List<ulong> numbers, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (var number in numbers)
                    {
                        writer.WriteLine(number);
                    }
                }
                MessageBox.Show($"Результаты сохранены в файл {fileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        public ICommand GenerateForThreeSeedsCommand => new Command(obj =>
        {
            var seeds = new string[] { "ААААББББВВВВГГГГ", "АБВГДЕЖЗИЙКЛМНОП", "ФЩПТУГИЕЫБЯЛАНГГ" };

            foreach (var seed in seeds)
            {
                generator.Init(seed, coeffs);
                var numbers = GenerateValidRandomNumbers(generator, 4000);
                SaveNumbersToFile(numbers, $"{seed}_numbers.txt");
            }
        });
        #endregion




        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
