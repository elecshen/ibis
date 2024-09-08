using Core;

namespace ConsoleView
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TrithemusEncoder encoder = new();
            var alf = encoder.Encrypt("Головной офис", "АБВгд");
            Console.WriteLine(alf);
        }
    }
}
