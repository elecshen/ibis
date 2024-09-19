using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace ConsoleView
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*var encoder = new SBlockModPolyTrithemiusEncoder<Alphabet>(a, new AlphabetModifier<Alphabet>(a));
            var alf = encoder.Encrypt("ЧРОТ", "РОЗА", 0);
            Console.WriteLine(alf);
            alf = encoder.Decrypt(alf, "РОЗА", 0);
            Console.WriteLine(alf);
            */
            var alphabet = new Alphabet();

            #region ТЕСТ 1
            /*

            char symbol = alphabet[5];  // Ожидаемый символ 'Е'
            int value = alphabet['Е'];  // Ожидаемое значение 5

            // Проверка суммирования символов
            var modifier = new AlphabetModifier<Alphabet>(alphabet);
            char sumChar = modifier.SumChar('Б', 'З');
            char subChar = modifier.SubChar('Щ', 'З');

            Console.WriteLine($"Symbol for 5: {symbol}, Value for 'Е': {value}, Sum: {sumChar}, Sub: {subChar}");
           */
            #endregion

            #region ТЕСТ 2
            var encoder = new ClassicTrithemiusEncoder(alphabet);
            /*string encrypted = encoder.Encrypt("С", "ВЕРСАЛЬ", 0);  
            string decrypted = encoder.Decrypt(encrypted, "ВЕРСАЛЬ", 0);

            Console.WriteLine($"Encrypted: {encrypted}, Decrypted: {decrypted}");
            */
            #endregion

            #region ТЕСТ 3 (неправильный)
            /*string text1 = "ПОЛДЕНЬ";
            string text2 = "КРАМОЛА";
            string key1 = "О";
            string key2 = "ВЕРСАЛЬ";
            int j1 = 0;
            int j2 = 2;
            string encryptedText1 = encoder.Encrypt(text1, key1, j1);
            string decryptedText1 = encoder.Decrypt(encryptedText1, key1, j1);
            string encryptedText2 = encoder.Encrypt(text1, key2, j1);
            string decryptedText2 = encoder.Decrypt(encryptedText2, key2, j1);
            string encryptedText3 = encoder.Encrypt(text1, key2, j2);
            string decryptedText3 = encoder.Decrypt(encryptedText3, key2, j2);

            string encryptedText4 = encoder.Encrypt(text2, key1, j1);
            string decryptedText4 = encoder.Decrypt(encryptedText4, key1, j1);
            string encryptedText5 = encoder.Encrypt(text2, key2, j1);
            string decryptedText5 = encoder.Decrypt(encryptedText5, key2, j1);
            string encryptedText6 = encoder.Encrypt(text2, key2, j2);
            string decryptedText6 = encoder.Decrypt(encryptedText6, key2, j2);

            Console.WriteLine("============ТЕСТ 1============");
            Console.WriteLine($"Ключ: {key1}, Исходный текст: {text1}, Смещение: {j1}");
            Console.WriteLine($"Зашифрованных текст: {encryptedText1}, Расшифрованный текст: {decryptedText1}");
            Console.WriteLine();

            Console.WriteLine("============ТЕСТ 2============");
            Console.WriteLine($"Ключ: {key2}, Исходный текст: {text1}, Смещение: {j1}");
            Console.WriteLine($"Зашифрованных текст: {encryptedText2}, Расшифрованный текст: {decryptedText2}");
            Console.WriteLine();

            Console.WriteLine("============ТЕСТ 3============");
            Console.WriteLine($"Ключ: {key2}, Исходный текст: {text1}, Смещение: {j2}");
            Console.WriteLine($"Зашифрованных текст: {encryptedText3}, Расшифрованный текст: {decryptedText3}");
            Console.WriteLine();

            Console.WriteLine("============ТЕСТ 4============");
            Console.WriteLine($"Ключ: {key1}, Исходный текст: {text2}, Смещение: {j1}");
            Console.WriteLine($"Зашифрованных текст: {encryptedText4}, Расшифрованный текст: {decryptedText4}");
            Console.WriteLine();

            Console.WriteLine("============ТЕСТ 5============");
            Console.WriteLine($"Ключ: {key2}, Исходный текст: {text2}, Смещение: {j1}");
            Console.WriteLine($"Зашифрованных текст: {encryptedText5}, Расшифрованный текст: {decryptedText5}");
            Console.WriteLine();

            Console.WriteLine("============ТЕСТ 6============");
            Console.WriteLine($"Ключ: {key2}, Исходный текст: {text2}, Смещение: {j2}");
            Console.WriteLine($"Зашифрованных текст: {encryptedText6}, Расшифрованный текст: {decryptedText6}");
            Console.WriteLine();
            */

            #endregion

            #region ТЕСТ 3 (правильный)
            string text = "ГОЛОВНОЙ_ОФИС";
            string key1 = "ЧЕРНОСОТЕНЦЫ";
            string key2 = "АБВГД";
            string key3 = "А";

            string encryptedText1 = encoder.Encrypt(text, key1, 0);
            string decryptedText1 = encoder.Decrypt(encryptedText1, key1, 0);
            string encryptedText2 = encoder.Encrypt(text, key2, 0);
            string decryptedText2 = encoder.Decrypt(encryptedText2, key3, 0);

            Console.WriteLine($"Зашифрованных текст: {encryptedText1}, Расшифрованный текст: {decryptedText1}");
            Console.WriteLine();
            Console.WriteLine($"Зашифрованных текст: {encryptedText2}, Расшифрованный текст: {decryptedText2}");
            #endregion


            #region ТЕСТ 4
            /*var polyEncoder = new PolyTrithemiusEncoder(alphabet);
            int j1 = 0;
            int j2 = 1;
            int j3 = 3;
            int j4 = 5;
            string polyEncrypted1 = polyEncoder.Encrypt("ОТКРЫТЫЙ_ТЕКСТ", "АББАТ_ТРИТИМУС", j1);
            string polyDecrypted1 = polyEncoder.Decrypt(polyEncrypted1, "АББАТ_ТРИТИМУС", j1);
            string polyEncrypted2 = polyEncoder.Encrypt("ОТКРЫТЫЙ_ТЕКСТ", "АББАТ_ТРИТИМУС", j2);
            string polyDecrypted2 = polyEncoder.Decrypt(polyEncrypted2, "АББАТ_ТРИТИМУС", j2);
            string polyEncrypted3 = polyEncoder.Encrypt("ОТКРЫТЫЙ_ТЕКСТ", "АББАТ_ТРИТИМУС", j3);
            string polyDecrypted3 = polyEncoder.Decrypt(polyEncrypted3, "АББАТ_ТРИТИМУС", j3);
            string polyEncrypted4 = polyEncoder.Encrypt("ОТКРЫТЫЙ_ТЕКСТ", "АББАТ_ТРИТИМУС", j4);
            string polyDecrypted4 = polyEncoder.Decrypt(polyEncrypted4, "АББАТ_ТРИТИМУС", j4);

            Console.WriteLine($"Полиалфавитное шифрование: {polyEncrypted1}, Полиалфавитное расшифрование: {polyDecrypted1}");
            Console.WriteLine($"Полиалфавитное шифрование: {polyEncrypted2}, Полиалфавитное расшифрование: {polyDecrypted2}");
            Console.WriteLine($"Полиалфавитное шифрование: {polyEncrypted3}, Полиалфавитное расшифрование: {polyDecrypted3}");
            Console.WriteLine($"Полиалфавитное шифрование: {polyEncrypted4}, Полиалфавитное расшифрование: {polyDecrypted4}");
            */
            #endregion

            #region ТЕСТ 5
            var modifier = new AlphabetModifier<Alphabet>(alphabet);
            var sBlockEncoder = new SBlockModPolyTrithemiusEncoder<Alphabet>(alphabet, modifier);
            string key = "ЗВЕЗДНАЯ_НОЧЬ";
            string text1 = "БЛОК";
            string text2 = "БРО";
            string sBlockEncrypted1 = sBlockEncoder.Encrypt(text1, key, 11);
            string sBlockDecrypted1 = sBlockEncoder.Decrypt(sBlockEncrypted1, key, 11);

            string sBlockEncrypted2 = sBlockEncoder.Encrypt(text2, key, 11);
            string sBlockDecrypted2 = sBlockEncoder.Decrypt(sBlockEncrypted2, key, 11);

            string sBlockDecrypted3 = sBlockEncoder.Decrypt(sBlockEncrypted1, key, 3);

            Console.WriteLine($"Шифрование S-Блока: {sBlockEncrypted1}, Расшифрование S-Блока: {sBlockDecrypted1}");
            Console.WriteLine($"Шифрование S-Блока: {sBlockEncrypted2}, Расшифрование S-Блока: {sBlockDecrypted2}");
            Console.WriteLine($"Расшифрование S-Блока: {sBlockDecrypted3}");
            #endregion
        }
    }
}
