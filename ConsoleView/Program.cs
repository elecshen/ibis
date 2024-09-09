using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace ConsoleView
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SBlockModPolyTrithemiusEncoder encoder = new(new Alphabet());
            var alf = encoder.Encrypt("ЧРОТ", "РОЗА", 0);
            Console.WriteLine(alf);
            alf = encoder.Decrypt(alf, "РОЗА", 0);
            Console.WriteLine(alf);
        }
    }
}
