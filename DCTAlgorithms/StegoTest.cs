using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCTAlgorithms
{
    [TestClass]
    public class StegoTest
    {
        private const int x = 50;
        private const int y = 50;
        private const int blockSize = 8;
        private const string basePath = @"C:\Users\waldyr\Documents\Visual Studio 2010\Projects\DCTAlgorithms\DCTAlgorithms";

        private Stego stego;

        [TestInitialize]
        public void Setup()
        {
            stego = new Stego(blockSize);

            Console.WriteLine();
            Console.WriteLine("Coords= ({0},{1})", x, y);
            Console.WriteLine();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
        }

        [TestMethod]
        public void Calculate_DCT_for_block_blockSizexblockSize()
        {
            int[,] block = readBlock();
            writeBlock("dct_file.jpg", block);
            int[,] dctblock = stego.calculateDCTForBlock(block);

            int[,] invertedDctBlock = stego.calculateIDCTForBlock(dctblock);
            writeBlock("idct_file.jpg", invertedDctBlock);

            printMatrixOnConsole("Original block", block);
            printMatrixOnConsole("DCT block", dctblock);
            printMatrixOnConsole("Inverted DCT block", invertedDctBlock);

        }

        private unsafe void writeBlock(string file, int[,] block)
        {
            using (var image = new Bitmap(blockSize, blockSize, PixelFormat.Format24bppRgb))
            {
                var data = image.LockBits(new Rectangle(0, 0, blockSize, blockSize), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                var pointer = (byte*)data.Scan0;
                for (int i = 0; i < block.GetLength(0); i++)
                {
                    for (int j = 0; j < block.GetLength(1); j++)
                    {
                        pointer[0] = pointer[1] = pointer[2] = (byte)block[i, j];
                        pointer += 3;
                    }
                }
                image.UnlockBits(data);
                image.Save(Path.Combine(basePath, file));
            }
        }

        private int[,] readBlock()
        {
            int[,] block = new int[blockSize, blockSize];

            string path = Path.Combine(basePath, "teste3.jpg");
            using (var image = new Bitmap(path))
            {
                BitmapData blockData = image.LockBits(new Rectangle(x, y, blockSize, blockSize), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* pointer = (byte*)blockData.Scan0;

                    for (int i = 0; i < blockData.Width; i++)
                    {
                        for (int j = 0; j < blockData.Height; j++)
                        {
                            block[i, j] = (int)((pointer[0] + pointer[1] + pointer[2]) / 3.0);
                            pointer += 3;
                        }
                        pointer += blockData.Stride - (blockData.Width * 3);
                    }
                }
                image.UnlockBits(blockData);
            }

            return block;
        }

        private void printMatrixOnConsole(string name, int[,] matrix)
        {
            Console.WriteLine("Print matrix " + name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write((matrix[i, j]).ToString().PadLeft(4, ' '));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}