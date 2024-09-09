using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace ConsoleView
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PolyTrithemiusEncoder encoder = new(new Alphabet());
            var alf = encoder.Encrypt("полдень", "версаль", 0);
            Console.WriteLine(alf);
            alf = encoder.Decrypt(alf, "версаль", 0);
            Console.WriteLine(alf);
        }
    }
}
