using System;
using StegoJpeg.Util;

namespace StegoJpeg
{
    public class DCT
    {
        private const int BlockSize = 8;

        public YCrCb[,] CalculateDCT(YCrCb[,] binaryImage)
        {
            int width = binaryImage.GetLength(0);
            int height = binaryImage.GetLength(1);

            var dctSuperBlock = new YCrCb[width, height];
            for (int x = 0; x < width; x = x + BlockSize)
            {
                for (int y = 0; y < height; y = y + BlockSize)
                {
                    var block = readSubBlock(binaryImage, x, y);
                    var dctBlock = calculateDCTForBlock(block);
                    for (int i = 0; i < BlockSize; i++)
                    {
                        for (int j = 0; j < BlockSize; j++)
                        {
                            dctSuperBlock[x + i, y + j] = dctBlock[i, j];
                        }
                    }
                }
            }
            return dctSuperBlock;
        }

        public YCrCb[,] CalculateIDCT(YCrCb[,] dctMatrix)
        {
            int width = dctMatrix.GetLength(0);
            int height = dctMatrix.GetLength(1);

            var idctSuperBlock = new YCrCb[width, height];
            for (int x = 0; x < width; x = x + BlockSize)
            {
                for (int y = 0; y < height; y = y + BlockSize)
                {
                    var block = readSubBlock(dctMatrix, x, y);
                    var idctBlock = calculateIDCTForBlock(block);
                    for (int i = 0; i < BlockSize; i++)
                    {
                        for (int j = 0; j < BlockSize; j++)
                        {
                            idctSuperBlock[x + i, y + j] = idctBlock[i, j];
                        }
                    }
                }
            }
            return idctSuperBlock;
        }

        private YCrCb[,] readSubBlock(YCrCb[,] binaryImage, int x, int y)
        {
            var subBlock = new YCrCb[BlockSize, BlockSize];
            for (int i = 0; i < BlockSize; i++)
            {
                for (int j = 0; j < BlockSize; j++)
                {
                    subBlock[i, j] = binaryImage[x + i, y + j];
                }
            }
            return subBlock;
        }

        private YCrCb[,] calculateDCTForBlock(YCrCb[,] originalBlock)
        {
            var dctBlock = new YCrCb[BlockSize, BlockSize];
            for (int k = 0; k < BlockSize; k++)
            {
                for (int l = 0; l < BlockSize; l++)
                {
                    double sumY, sumCr, sumCb;
                    sumY = sumCr = sumCb = 0;
                    for (int i = 0; i < BlockSize; i++)
                    {
                        for (int j = 0; j < BlockSize; j++)
                        {
                            sumY += coefficient(k) * coefficient(l) * originalBlock[i, j].Y * calculateCosine(k, i) *
                                    calculateCosine(l, j) / 4;
                            sumCr += coefficient(k) * coefficient(l) * originalBlock[i, j].Cr * calculateCosine(k, i) *
                                     calculateCosine(l, j) / 4;
                            sumCb += coefficient(k) * coefficient(l) * originalBlock[i, j].Cb * calculateCosine(k, i) *
                                     calculateCosine(l, j) / 4;
                        }
                    }

                    dctBlock[k, l].Y = Math.Round(sumY, 2);
                    dctBlock[k, l].Cr = Math.Round(sumCr, 2);
                    dctBlock[k, l].Cb = Math.Round(sumCb, 2);
                }
            }

            return dctBlock;
        }

        private YCrCb[,] calculateIDCTForBlock(YCrCb[,] dctBlock)
        {
            var idctBlock = new YCrCb[BlockSize, BlockSize];
            int n = dctBlock.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double sumY, sumCr, sumCb;
                    sumY = sumCr = sumCb = 0;
                    for (int k = 0; k < n; k++)
                    {
                        for (int l = 0; l < n; l++)
                        {
                            sumY += coefficient(k) * coefficient(l) * dctBlock[k, l].Y * calculateCosine(k, i) *
                                    calculateCosine(l, j) / 4;
                            sumCr += coefficient(k) * coefficient(l) * dctBlock[k, l].Cr * calculateCosine(k, i) *
                                     calculateCosine(l, j) / 4;
                            sumCb += coefficient(k) * coefficient(l) * dctBlock[k, l].Cb * calculateCosine(k, i) *
                                     calculateCosine(l, j) / 4;
                        }
                    }

                    idctBlock[i, j].Y = Math.Round(sumY).ToByteBounds();
                    idctBlock[i, j].Cr = Math.Round(sumCr).ToByteBounds();
                    idctBlock[i, j].Cb = Math.Round(sumCb).ToByteBounds();
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