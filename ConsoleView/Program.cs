using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace ConsoleView
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PolyTrithemiusEncoder encoder = new(new Alphabet());
            var alf = encoder.Encrypt("открытый_текст", "Аббат_тритимус", 0);
            Console.WriteLine(alf);
            alf = encoder.Decrypt(alf, "Аббат_тритимус", 0);
            Console.WriteLine(alf);
        }
    }
}
