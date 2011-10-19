using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DCTAlgorithms
{
    public class DiscreteCosineTransform
    {
        const int BlockSize = 8;

        public double[,] CalculateDCT(byte[,] binaryImage)
        {
            int width = binaryImage.GetLength(0);
            int height = binaryImage.GetLength(1);
            
            var dctSuperBlock = new double[width, height];
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
        public byte[,] CalculateIDCT(double [,] dctMatrix)
        {
            int width = dctMatrix.GetLength(0);
            int height = dctMatrix.GetLength(1);
            
            var idctSuperBlock = new byte[width, height];
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

        private byte[,] readSubBlock(byte[,] binaryImage, int x, int y)
        {
            byte[,] subBlock = new byte[BlockSize, BlockSize];
            for (int i = 0; i < BlockSize; i++)
            {
                for (int j = 0; j < BlockSize; j++)
                {
                    subBlock[i, j] = binaryImage[x + i, y + j];
                }
            }
            return subBlock;
        }

        private double [,] readSubBlock(double [,] binaryImage, int x, int y)
        {
            var subBlock = new double[BlockSize, BlockSize];
            for (int i = 0; i < BlockSize; i++)
            {
                for (int j = 0; j < BlockSize; j++)
                {
                    subBlock[i, j] = binaryImage[x + i, y + j];
                }
            }
            return subBlock;
        }

        private double [,] calculateDCTForBlock(byte[,] originalBlock)
        {
            var dctBlock = new double[BlockSize, BlockSize];
            for (int k = 0; k < BlockSize; k++)
            {
                for (int l = 0; l < BlockSize; l++)
                {
                    double sum = 0;
                    for (int i = 0; i < BlockSize; i++)
                    {
                        for (int j = 0; j < BlockSize; j++)
                        {
                            sum += coefficient(k) * coefficient(l) * originalBlock[i, j] * calculateCosine(k, i) * calculateCosine(l, j) / 4;
                        }
                    }

                    dctBlock[k, l] = Math.Round(sum, 2);
                }
            }

            return dctBlock;
        }

        private byte[,] calculateIDCTForBlock(double [,] dctBlock)
        {
            var idctBlock = new byte[BlockSize, BlockSize];
            int n = dctBlock.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < n; k++)
                    {
                        for (int l = 0; l < n; l++)
                        {
                            sum += coefficient(k) * coefficient(l) * dctBlock[k, l] * calculateCosine(k, i) * calculateCosine(l, j) / 4;
                        }
                    }

                    idctBlock[i, j] = (byte)Math.Round(sum);
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