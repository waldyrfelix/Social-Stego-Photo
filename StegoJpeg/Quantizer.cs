using System;
using StegoJpeg.Util;

namespace StegoJpeg
{
    public class Quantizer
    {
        private double[,] Q50Lum = new double[,]
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


        private double[,] Q50Chr = new double[,]
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

        public void ApplyQuantization(YCrCb[,] matrix)
        {
            int i, j, k, l;
            for (i = 0; i < matrix.GetLength(0); i++)
            {
                k = i % 8;
                for (j = 0; j < matrix.GetLength(1); j++)
                {
                    l = j % 8;
                    matrix[i, j].Y = Math.Round(matrix[i, j].Y / Q50Lum[k, l]);
                    matrix[i, j].Cr = Math.Round(matrix[i, j].Cr / Q50Chr[k, l]);
                    matrix[i, j].Cb = Math.Round(matrix[i, j].Cb / Q50Chr[k, l]);
                }
            }
        } 
        
        public void ApplyInverseQuantization(YCrCb[,] matrix)
        {
            int i, j, k, l;
            for (i = 0; i < matrix.GetLength(0); i++)
            {
                k = i % 8;
                for (j = 0; j < matrix.GetLength(1); j++)
                {
                    l = j % 8;
                    matrix[i, j].Y = Math.Round(matrix[i, j].Y * Q50Lum[k, l]);
                    matrix[i, j].Cr = Math.Round(matrix[i, j].Cr * Q50Chr[k, l]);
                    matrix[i, j].Cb = Math.Round(matrix[i, j].Cb * Q50Chr[k, l]);
                }
            }
        }
    }
}