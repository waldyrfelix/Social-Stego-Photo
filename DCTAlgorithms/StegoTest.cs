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
        private const int x = 1000;
        private const int y = 500;
        private Stego stego;

        [TestInitialize]
        public void Setup()
        {
            stego = new Stego();

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
        public void Calculate_DCT_for_block_8x8()
        {
            byte[,] block = new byte[8, 8];

            string path = @"C:\Users\waldyr\Documents\Visual Studio 2010\Projects\DCTAlgorithms\DCTAlgorithms\ShangHai.jpg";
            using (var image = new Bitmap(path))
            {
                BitmapData blockData = image.LockBits(new Rectangle(1000, 500, 8, 8), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* imagePointer = (byte*)blockData.Scan0;

                    for (int i = 0; i < blockData.Height; i++)
                    {
                        for (int j = 0; j < blockData.Width; j++)
                        {
                            block[j, i] = (byte)((imagePointer[0] + imagePointer[1] + imagePointer[2]) / 3);
                            imagePointer += 3;
                        }
                        imagePointer += 3;
                    }
                }
                image.UnlockBits(blockData);
            }

            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            double[,] dctblock = stego.calculateDCTForBlock(block);
            stopwatch.Stop();


            printMatrixOnConsole("Original block", block);
            printMatrixOnConsole("DCT block", dctblock);
            Console.WriteLine("Spended time: "+ stopwatch.Elapsed);

        }


        private void printMatrixOnConsole(string name, double[,] matrix)
        {
            Console.WriteLine("Print matrix " + name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(( (int) matrix[i, j]).ToString().PadLeft(4, ' '));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void printMatrixOnConsole(string name, byte[,] matrix)
        {
            Console.WriteLine("Print matrix " + name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(( (int) matrix[i, j]).ToString().PadLeft(4, ' '));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}