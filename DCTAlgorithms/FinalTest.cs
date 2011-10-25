using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace DCTAlgorithms
{
    [TestClass]
    public class FinalTest
    {
        private const string basePath = @"C:\Users\waldyr\Documents\Visual Studio 2010\Projects\DCTAlgorithms\DCTAlgorithms";

        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
        }


        //[TestMethod]
        //public void Calculate_DCT_for_block_blockSizexblockSize()
        //{
        //    int[,] block = readBlock();
        //    writeBlock("dct_file.jpg", block);
        //    int[,] dctblock = dct.calculateDCTForBlock(block);

        //    int[,] invertedDctBlock = dct.calculateIDCTForBlock(dctblock);
        //    writeBlock("idct_file.jpg", invertedDctBlock);

        //    TestHelper.PrintMatrix("Original block", block);
        //    TestHelper.PrintMatrix("DCT block", dctblock);
        //    TestHelper.PrintMatrix("Inverted DCT block", invertedDctBlock);

        //}

        [TestMethod]
        public void Hide_message_with_steganography()
        {
            string path = Path.Combine(basePath, "teste3.jpg");
            using (var stream = new StreamReader(path))
            {
                var reader = new ImageBinaryReader();
                var bytes = reader.ReadLuminanceFromImage(stream.BaseStream);

                TestHelper.PrintMatrix("Binary image", bytes);
                writeBlock("original.jpg", bytes);

                var dct = new DiscreteCosineTransform();
                double[,] matrix = dct.CalculateDCT(bytes);

                TestHelper.PrintMatrix("DCT matrix", matrix);

                var stego = new Steganography();
                var stegoMatrix = stego.HideMessage(matrix, "waldyr Henrique felix Da Silva.");
                var coveredBytes = dct.CalculateIDCT(stegoMatrix);
                TestHelper.PrintMatrix("Coveted matrix", coveredBytes);
                writeBlock("hided.jpg", coveredBytes);
            }
        }


        [TestMethod]
        public void Extract_message_from_stego_image()
        {
            string path = Path.Combine(basePath, "hided.jpg");
            using (var stream = new StreamReader(path))
            {
                var reader = new ImageBinaryReader();
                var bytes = reader.ReadLuminanceFromImage(stream.BaseStream);
                TestHelper.PrintMatrix("Stego img", bytes);

                var dct = new DiscreteCosineTransform();
                double[,] matrix = dct.CalculateDCT(bytes);
                TestHelper.PrintMatrix("DCT", matrix);

                var stego = new Steganography();
                var message = stego.ExtractMessage(matrix);

                Console.WriteLine(message);
            }
        }

        [TestMethod, Ignore]
        public void Hide_message_with_steganography_test_madeiro()
        {
            string path = Path.Combine(basePath, "teste3.jpg");
            using (var stream = new StreamReader(path))
            {
                var reader = new ImageBinaryReader();
                var bytes = reader.ReadLuminanceFromImage(stream.BaseStream);

                writeBlock("original.jpg", bytes);

                var dct = new DiscreteCosineTransform();
                double[,] matrix = dct.CalculateDCT(bytes);

                double[,] matrixPlus = modifyMostSignificantBits(matrix, 31);
                writeBlock("original_mais_31.jpg", dct.CalculateIDCT(matrixPlus));

                double[,] matrixMinus = modifyMostSignificantBits(matrix, -31);
                writeBlock("original_menos_31.jpg", dct.CalculateIDCT(matrixMinus));
            }
        }
        private double[,] modifyMostSignificantBits(double[,] bytes, int soma)
        {
            var matrix = new double[bytes.GetLength(0), bytes.GetLength(1)];

            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                for (int j = 0; j < bytes.GetLength(1); j++)
                {
                    matrix[i, j] = bytes[i, j];
                    if (i % 8 == 0 && j % 8 == 0)
                    {
                        matrix[i, j] += soma;
                    }
                }
            }
            return matrix;
        }

        private void printAllEdges(int[,] matrix)
        {
            List<int> edges = new List<int>();
            for (int i = 0; i < matrix.GetLength(0); i = i + 8)
            {
                for (int j = 0; j < matrix.GetLength(1); j = j + 8)
                {
                    edges.Add(matrix[i, j]);
                    //Console.WriteLine(matrix[i, j]);
                }
            }

            Console.WriteLine("Menor: " + edges.Min());
            Console.WriteLine("Maior: " + edges.Max());

        }

        private unsafe void writeBlock(string file, byte[,] block)
        {
            var width = block.GetLength(0);
            var height = block.GetLength(1);
            using (var image = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            {
                var data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                var pointer = (byte*)data.Scan0;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        pointer[0] = pointer[1] = pointer[2] = block[i, j];
                        //pointer[0] = block[i, j];
                        pointer += 3;
                    }
                }
                image.UnlockBits(data);
                image.Save(Path.Combine(basePath, file));
            }
        }

        //private int[,] readBlock()
        //{
        //    int[,] block = new int[blockSize, blockSize];

        //    string path = Path.Combine(basePath, "teste3.jpg");
        //    using (var image = new Bitmap(path))
        //    {
        //        BitmapData blockData = image.LockBits(new Rectangle(x, y, blockSize, blockSize), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        //        unsafe
        //        {
        //            byte* pointer = (byte*)blockData.Scan0;

        //            for (int i = 0; i < blockData.Width; i++)
        //            {
        //                for (int j = 0; j < blockData.Height; j++)
        //                {
        //                    block[i, j] = (int)Math.Round((pointer[0] + pointer[1] + pointer[2]) / 3.0);
        //                    pointer += 3;
        //                }
        //                pointer += blockData.Stride - (blockData.Width * 3);
        //            }
        //        }
        //        image.UnlockBits(blockData);
        //    }

        //    return block;
        //}
    }
}