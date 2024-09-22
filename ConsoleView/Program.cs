using Core.Alphabet;
using Core.ShiftCipher.Trithemius;

namespace ConsoleView
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = new RusAlphabet();
            var encoder = new SBlockModPolyTrithemiusEncoder<RusAlphabet>(a, new AlphabetModifier<RusAlphabet>(a));
            var alf = encoder.Encrypt("ЧРОТ", "РОЗА", 0);
            Console.WriteLine(alf);
            alf = encoder.Decrypt(alf, "РОЗА", 0);
            Console.WriteLine(alf);
        }
    }
}
