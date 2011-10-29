using System;
using StegoJpeg.Util;

namespace StegoJpeg
{
    public class DCT
    {
        private const int BlockSize = 8;

        public void CalculateDCT(YCrCb[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            for (int x = 0; x < width; x = x + BlockSize)
            {
                for (int y = 0; y < height; y = y + BlockSize)
                {
                    calculateDCTForBlock(matrix, x, y);
                }
            }
        }

        public void CalculateIDCT(YCrCb[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            for (int x = 0; x < width; x = x + BlockSize)
            {
                for (int y = 0; y < height; y = y + BlockSize)
                {
                    calculateIDCTForBlock(matrix, x, y);
                }
            }
        }

        private void calculateDCTForBlock(YCrCb[,] matrix, int x, int y)
        {
            var tempMatrix = new YCrCb[BlockSize,BlockSize];
            for (int k = 0; k < BlockSize; k++)
            {
                for (int l = 0; l < BlockSize; l++)
                {
                    double sumY = 0, sumCr = 0, sumCb = 0;
                    for (int i = 0; i < BlockSize; i++)
                    {
                        for (int j = 0; j < BlockSize; j++)
                        {
                            sumY += coefficient(k)*coefficient(l)*matrix[i + x, j + y].Y*calculateCosine(k, i)*
                                    calculateCosine(l, j)/4;
                            sumCr += coefficient(k)*coefficient(l)*matrix[i + x, j + y].Cr*calculateCosine(k, i)*
                                     calculateCosine(l, j)/4;
                            sumCb += coefficient(k)*coefficient(l)*matrix[i + x, j + y].Cb*calculateCosine(k, i)*
                                     calculateCosine(l, j)/4;
                        }
                    }

                    tempMatrix[k, l].Y = Math.Round(sumY, 2);
                    tempMatrix[k, l].Cr = Math.Round(sumCr, 2);
                    tempMatrix[k, l].Cb = Math.Round(sumCb, 2);
                }
            }

            copyTempMatrixToRealMatrix(x, y, matrix, tempMatrix);
        }

        private void copyTempMatrixToRealMatrix(int x, int y, YCrCb[,] matrix, YCrCb[,] tempMatrix)
        {
            for (int i = 0; i < BlockSize; i++)
            {
                for (int j = 0; j < BlockSize; j++)
                {
                    matrix[i + x, j + y].Y = tempMatrix[i, j].Y;
                    matrix[i + x, j + y].Cb = tempMatrix[i, j].Cb;
                    matrix[i + x, j + y].Cr = tempMatrix[i, j].Cr;
                }
            }
        }

        private void calculateIDCTForBlock(YCrCb[,] matrix, int x, int y)
        {
            var tempMatrix = new YCrCb[BlockSize,BlockSize];
            for (int i = 0; i < BlockSize; i++)
            {
                for (int j = 0; j < BlockSize; j++)
                {
                    double sumY, sumCr, sumCb;
                    sumY = sumCr = sumCb = 0;
                    for (int k = 0; k < BlockSize; k++)
                    {
                        for (int l = 0; l < BlockSize; l++)
                        {
                            sumY += coefficient(k)*coefficient(l)*matrix[k + x, l + y].Y*calculateCosine(k, i)*
                                    calculateCosine(l, j)/4;
                            sumCr += coefficient(k)*coefficient(l)*matrix[k + x, l + y].Cr*calculateCosine(k, i)*
                                     calculateCosine(l, j)/4;
                            sumCb += coefficient(k)*coefficient(l)*matrix[k + x, l + y].Cb*calculateCosine(k, i)*
                                     calculateCosine(l, j)/4;
                        }
                    }

                    tempMatrix[i, j].Y = Math.Round(sumY);
                    tempMatrix[i, j].Cr = Math.Round(sumCr);
                    tempMatrix[i, j].Cb = Math.Round(sumCb);
                }
            }

            copyTempMatrixToRealMatrix(x, y, matrix, tempMatrix);
        }

        private double calculateCosine(double k, int i)
        {
            return Math.Cos(Math.PI/16*k*(2*i + 1));
        }

        private double coefficient(int coord)
        {
            if (coord == 0)
            {
                return 1/Math.Sqrt(2);
            }
            return 1;
        }
    }
}