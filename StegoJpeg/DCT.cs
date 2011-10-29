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
            var tempMatrix = new YCrCb[BlockSize, BlockSize];
            for (int k = x; k < x + BlockSize; k++)
            {
                for (int l = y; l < y + BlockSize; l++)
                {
                    double sumY, sumCr, sumCb;
                    sumY = sumCr = sumCb = 0;
                    for (int i = x; i < BlockSize; i++)
                    {
                        for (int j = y; j < BlockSize; j++)
                        {
                            sumY += coefficient(k)*coefficient(l)*matrix[i, j].Y*calculateCosine(k, i)*
                                    calculateCosine(l, j)/4;
                            sumCr += coefficient(k)*coefficient(l)*matrix[i, j].Cr*calculateCosine(k, i)*
                                     calculateCosine(l, j)/4;
                            sumCb += coefficient(k)*coefficient(l)*matrix[i, j].Cb*calculateCosine(k, i)*
                                     calculateCosine(l, j)/4;
                        }
                    }

                    tempMatrix[k, l].Y = Math.Round(sumY, 2);
                    tempMatrix[k, l].Cr = Math.Round(sumCr, 2);
                    tempMatrix[k, l].Cb = Math.Round(sumCb, 2);
                }
            }

            for (int i = x; i < x + BlockSize; i++)
            {
                for (int j = y; j < y + BlockSize; j++)
                {
                    matrix[i, j].Y = tempMatrix[i, j].Y;
                    matrix[i, j].Cb = tempMatrix[i, j].Cb;
                    matrix[i, j].Cr = tempMatrix[i, j].Cr;
                }
            }
        }

        private void calculateIDCTForBlock(YCrCb[,] matrix, int x, int y)
        {
            var tempMatrix = new YCrCb[BlockSize, BlockSize];
            for (int i = x; i < x + BlockSize; i++)
            {
                for (int j = y; j < y + BlockSize; j++)
                {
                    double sumY, sumCr, sumCb;
                    sumY = sumCr = sumCb = 0;
                    for (int k = x; k < y + BlockSize; k++)
                    {
                        for (int l = y; l < y + BlockSize; l++)
                        {
                            sumY += coefficient(k)*coefficient(l)*matrix[k, l].Y*calculateCosine(k, i)*
                                    calculateCosine(l, j)/4;
                            sumCr += coefficient(k)*coefficient(l)*matrix[k, l].Cr*calculateCosine(k, i)*
                                     calculateCosine(l, j)/4;
                            sumCb += coefficient(k)*coefficient(l)*matrix[k, l].Cb*calculateCosine(k, i)*
                                     calculateCosine(l, j)/4;
                        }
                    }

                    tempMatrix[i, j].Y = Math.Round(sumY);
                    tempMatrix[i, j].Cr = Math.Round(sumCr);
                    tempMatrix[i, j].Cb = Math.Round(sumCb);
                }
            }

            for (int i = x; i < x + BlockSize; i++)
            {
                for (int j = y; j < y + BlockSize; j++)
                {
                    matrix[i, j].Y = tempMatrix[i, j].Y;
                    matrix[i, j].Cb = tempMatrix[i, j].Cb;
                    matrix[i, j].Cr = tempMatrix[i, j].Cr;
                }
            }
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