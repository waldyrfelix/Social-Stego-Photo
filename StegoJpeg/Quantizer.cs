using System;
using StegoJpeg.Util;

namespace StegoJpeg
{
    public class Quantizer
    {
        private int[,] Q50Lum = new int[,]
                                       {
                                           {16, 11, 10, 16, 24, 40, 51, 61},
                                           {12, 12, 14, 19, 26, 58, 60, 55},
                                           {14, 13, 16, 24, 40, 57, 69, 56},
                                           {14, 17, 22, 29, 51, 87, 80, 62},
                                           {18, 22, 37, 56, 68, 109, 103, 77},
                                           {24, 35, 55, 64, 81, 104, 113, 92},
                                           {49, 64, 78, 87, 103, 121, 120, 101},
                                           {72, 92, 95, 98, 112, 100, 103, 99}
                                       };


        private int[,] Q50Chr = new int[,]
                                       {
                                           {17, 18, 24, 47, 99, 99, 99, 99},
                                           {18, 21, 26, 66, 99, 99, 99, 99},
                                           {24, 26, 56, 99, 99, 99, 99, 99},
                                           {47, 66, 99, 99, 99, 99, 99, 99},
                                           {99, 99, 99, 99, 99, 99, 99, 99},
                                           {99, 99, 99, 99, 99, 99, 99, 99},
                                           {99, 99, 99, 99, 99, 99, 99, 99},
                                           {99, 99, 99, 99, 99, 99, 99, 99}
                                       };

        public void ApplyQuantization(YCrCb[,] matrix, int qualityFactor)
        {
            int i, j, k, l;
            int[,] qualityLuminanceMatrix = generateQualityFactorLuminanceMatrix(qualityFactor);
            int[,] qualityChrominanceMatrix = generateQualityFactorChrominanceMatrix(qualityFactor);

            for (i = 0; i < matrix.GetLength(0); i++)
            {
                k = i % 8;
                for (j = 0; j < matrix.GetLength(1); j++)
                {
                    l = j % 8;
                    matrix[i, j].Y = Math.Round(matrix[i, j].Y / qualityLuminanceMatrix[k, l]);
                    matrix[i, j].Cr = Math.Round(matrix[i, j].Cr / qualityChrominanceMatrix[k, l]);
                    matrix[i, j].Cb = Math.Round(matrix[i, j].Cb / qualityChrominanceMatrix[k, l]);
                }
            }
        }

        public void ApplyInverseQuantization(YCrCb[,] matrix, int qualityFactor)
        {
            int i, j, k, l;
            int[,] qualityLuminanceMatrix = generateQualityFactorLuminanceMatrix(qualityFactor);
            int[,] qualityChrominanceMatrix = generateQualityFactorChrominanceMatrix(qualityFactor);

            for (i = 0; i < matrix.GetLength(0); i++)
            {
                k = i % 8;
                for (j = 0; j < matrix.GetLength(1); j++)
                {
                    l = j % 8;
                    matrix[i, j].Y = Math.Round(matrix[i, j].Y * qualityLuminanceMatrix[k, l]);
                    matrix[i, j].Cr = Math.Round(matrix[i, j].Cr * qualityChrominanceMatrix[k, l]);
                    matrix[i, j].Cb = Math.Round(matrix[i, j].Cb * qualityChrominanceMatrix[k, l]);
                }
            }
        }

        private int[,] generateQualityFactorLuminanceMatrix(int qualityFactor)
        {
            if (qualityFactor == 50)
                return Q50Lum;

            return calculateFactorForMatrix(Q50Lum, qualityFactor);
        }

        private int[,] generateQualityFactorChrominanceMatrix(int qualityFactor)
        {
            if (qualityFactor == 50)
                return Q50Chr;

            return calculateFactorForMatrix(Q50Chr, qualityFactor);
        }

        private int[,] calculateFactorForMatrix(int[,] matrix50, int qualityFactor)
        {
            int qf = (qualityFactor < 50) ? 5000 / qualityFactor : 200 - qualityFactor * 2;
            int[,] matrix = new int[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    matrix[i, j] = (int)Math.Round((qf * matrix50[i, j] + 50) / 100.0);
                }
            }
            return matrix;
        }

    }
}