using System;

namespace DCTAlgorithms
{
    public class Stego
    {
        private readonly int _blockSize;

        public Stego(int blockSize)
        {
            _blockSize = blockSize;
        }

        internal int[,] calculateDCTForBlock(int[,] originalBlock)
        {
            int [,] dctBlock = new int[_blockSize, _blockSize];
            int n = originalBlock.GetLength(1);

            for (int k = 0; k < n; k++)
            {
                for (int l = 0; l < n; l++)
                {
                    double sum = 0;
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            sum += coefficient(k) * coefficient(l) * originalBlock[i, j] * calculateCosine(k, i) * calculateCosine(l, j) / 4;
                        }
                    }

                    dctBlock[k, l] = (int) Math.Round(sum);
                }
            }

            return dctBlock;
        }

        internal int[,] calculateIDCTForBlock(int[,] originalBlock)
        {
            int[,] idctBlock = new int[_blockSize, _blockSize];
            int n = originalBlock.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < n; k++)
                    {
                        for (int l = 0; l < n; l++)
                        {
                            sum += coefficient(k) * coefficient(l) * originalBlock[k, l] * calculateCosine( k, i) * calculateCosine(l, j) / 4;
                        }
                    }

                    idctBlock[i, j] = (int) Math.Round(sum);
                }
            }

            return idctBlock;
        }

        internal double calculateCosine(double k, int i)
        {
            return Math.Cos(Math.PI / 16 * k * (2 * i + 1));
        }

        internal double coefficient(int coord)
        {
            if (coord == 0)
            {
                return 1 / Math.Sqrt(2);
            }
            return 1;
        }
    }
}