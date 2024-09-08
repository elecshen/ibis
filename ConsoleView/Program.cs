using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace ConsoleView
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClassicTrithemiusEncoder encoder = new(new Alphabet());
            var alf = encoder.Encrypt("Головной офис", "АБВгд");
            Console.WriteLine(alf);
            alf = encoder.Decrypt(alf, "АБВгд");
            Console.WriteLine(alf);
        }
    }
}
