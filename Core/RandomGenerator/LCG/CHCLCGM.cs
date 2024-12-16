using Core.Alphabet;
using Core.Encryptor;

namespace Core.RandomGenerator.LCG
{
    public class CHCLCGM<T>(IExtendedEncryptor<T> encryptor, IAlphabetModifier<T> modifier, LCGCoeffs[] coeffs) : CHCLCG<T>(encryptor, modifier, coeffs) where T : IAlphabet
    {
        protected string CheckSeed(string value)
        {
            string key = "ОТВЕТСТВЕННЫЙ_ПОДХОД";
            var blocks = new string[4];
            for (int i = 0; i < 4; i++)
                blocks[i] = value.Substring(i * 4, 4);
            for (int j = 0; j < 4; j++)
                for (int i = j + 1; i < 4; i++)
                    if (blocks[i] == blocks[j])
                        blocks[i] = _encoder.Oneside(blocks[j], key, j + 2 * i);
            return string.Join("", blocks);
        }

        public override void Init(string seed)
        {
            base.Init(CheckSeed(seed));
            for (int i = 0; i < 4; i++)
            {
                if (i > 0)
                    for (int j = 0; j <= i; j++)
                        _hCLCGs[i].Next();
            }
        }
    }
}
