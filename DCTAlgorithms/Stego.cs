using System;

namespace DCTAlgorithms
{
    public class Stego
    {
        internal double[,] calculateDCTForBlock(byte[,] originalBlock)
        {
            double[,] dctBlock = new double[8,8];

            for (int u = 0; u < originalBlock.GetLength(0); u++)
            {
                for (int v = 0; v < originalBlock.GetLength(1); v++)
                {
                    dctBlock[u, v] = calculateDCTForPixel(originalBlock, u, v);
                }
            }

            return dctBlock;
        }

        internal double calculateCosine(int stegoCoord, int coord)
        {
            return Math.Cos((2*coord + 1)*stegoCoord*Math.PI/16.0);
        }

        internal double calculateIteration(byte[,] originalBlock, int x, int y, int u, int v)
        {
            return originalBlock[x, y]*calculateCosine(u, x)*calculateCosine(v, y);
        }

        internal double calculateSum(byte[,] originalBlock, int u, int v)
        {
            double sum = 0;
            for (int x = 0; x < originalBlock.GetLength(0); x++)
            {
                for (int y = 0; y < originalBlock.GetLength(1); y++)
                {
                    sum += calculateIteration(originalBlock, x, y, u, v);
                }
            }

            return sum;
        }

        internal double calculateDCTForPixel(byte[,] originalBlock, int u, int v)
        {
            return calculateWeight(u)*calculateWeight(v)/4*calculateSum(originalBlock, u, v);
        }

        internal double calculateWeight(int coord)
        {
            if (coord == 0)
            {
                return 1/Math.Sqrt(2);
            }
            return 1;
        }
    }
}