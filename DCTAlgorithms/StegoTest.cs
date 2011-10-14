using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCTAlgorithms
{
    [TestClass]
    public class StegoTest
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

        //    printMatrixOnConsole("Original block", block);
        //    printMatrixOnConsole("DCT block", dctblock);
        //    printMatrixOnConsole("Inverted DCT block", invertedDctBlock);

        //}

        [TestMethod]
        public void Hide_message_with_steganography()
        {
            string path = Path.Combine(basePath, "teste3.jpg");
            using (var stream = new StreamReader(path))
            {
                var reader = new ImageBinaryReader();
                var bytes = reader.ReadAllBytes(stream.BaseStream);

                printMatrixOnConsole("Binary image", bytes);
                writeBlock("original.jpg", bytes);

                var dct = new DiscreteCosineTransform();
                int[,] matrix = dct.CalculateDCT(bytes);

                printMatrixOnConsole("DCT matrix", matrix);

                var stego = new Steganography();
                var stegoMatrix = stego.HideMessage(matrix, "waldyr");
                var coveredBytes = dct.CalculateIDCT(stegoMatrix);
                printMatrixOnConsole("Coveted matrix", coveredBytes);
                writeBlock("hided.jpg", coveredBytes);
            }
        }
        [TestMethod]
        public void Hide_message_with_steganography_test_madeiro()
        {
            string path = Path.Combine(basePath, "teste3.jpg");
            using (var stream = new StreamReader(path))
            {
                var reader = new ImageBinaryReader();
                var bytes = reader.ReadAllBytes(stream.BaseStream);

                writeBlock("original.jpg", bytes);
                
                var dct = new DiscreteCosineTransform();
                int[,] matrix = dct.CalculateDCT(bytes);

                int[,] matrixPlus = modifyMostSignificantBits(matrix, 31);
                writeBlock("original_mais_31.jpg", dct.CalculateIDCT(matrixPlus));

                int[,] matrixMinus = modifyMostSignificantBits(matrix, -31);
                writeBlock("original_menos_31.jpg", dct.CalculateIDCT(matrixMinus));
            }
        }

        private int[,] modifyMostSignificantBits(int[,] bytes, int soma)
        {
            int[,] matrix = new int[bytes.GetLength(0), bytes.GetLength(1)];

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

        [TestMethod]
        public void Extract_message_from_stego_image()
        {
              string path = Path.Combine(basePath, "hided.jpg");
              using (var stream = new StreamReader(path))
              {
                  var reader = new ImageBinaryReader();
                  var bytes = reader.ReadAllBytes(stream.BaseStream);

                  var dct = new DiscreteCosineTransform();
                  int[,] matrix = dct.CalculateDCT(bytes);

                  var stego = new Steganography();
                  var message = stego.ExtractMessage(matrix);

                  Console.WriteLine(message);
              }
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

        private void printMatrixOnConsole(string name, byte[,] matrix)
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