
namespace Core.Alphabet
{
    public interface IAlphabetModifier<T> where T :IAlphabet
    {
        string NumsToText(IEnumerable<int> nums);
        char SubChar(char c1, char c2);
        string SubString(string s1, string s2);
        char SumChar(char c1, char c2);
        string SumString(string s1, string s2);
        int[] TextToNums(string s);
    }
}