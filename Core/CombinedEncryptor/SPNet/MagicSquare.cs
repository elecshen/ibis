namespace Core.CombinedEncryptor.SPNet
{
    public static class MagicSquare
    {
        public static readonly int[][] DefaultMatrix =
            [
                [
                    16, 3, 2, 13,
                    5, 10, 11, 8,
                    9, 6, 7, 12,
                    4, 15, 14, 1
                ],
                [
                    7, 14, 4, 9,
                    12, 1, 15, 6,
                    13, 8, 10, 3,
                    2, 11, 5, 16
                ],
                [
                    4, 14, 15, 1,
                    9, 7, 6, 12,
                    5, 11, 10, 8,
                    16, 2, 3, 13
                ]
            ];

        public static int[] GetMatrix(int num) => DefaultMatrix[num % DefaultMatrix.Length];

        public static string Encode(string str, int matrixNum)
        {
            var matrix = GetMatrix(matrixNum);
            string res = "";
            foreach (var item in matrix)
            {
                res += str[item - 1];
            }
            return res;
        }

        public static string Decode(string str, int matrixNum)
        {
            var matrix = GetMatrix(matrixNum);
            var res = new char[str.Length];
            for (var i = 0; i < matrix.Length; i++)
            {
                res[matrix[i] - 1] = str[i];
            }
            return string.Join("", res);
        }
    }
}
