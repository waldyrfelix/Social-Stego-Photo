using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StegoJpeg;
using StegoJpeg.Util;

namespace StegoJpegTests
{
    [TestClass]
    public class FinalTest
    {
        private const string basePath =
            @"C:\Users\waldyr\Documents\Visual Studio 2010\Projects\SocialStegoPhoto\StegoJpegTests\";

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
                var bytesRGB = reader.ReadRGBFromImage(stream.BaseStream);

                TestHelper.PrintMatrix("Binary image", bytesRGB);
                writeBlock("original.jpg", bytesRGB);

                var dct = new DCT();
                var matrix = dct.CalculateDCT(YCrCb.Parse(bytesRGB));

                TestHelper.PrintMatrix("DCT matrix", matrix);

                var stego = new Steganography();
                var stegoMatrix = stego.HideMessage(matrix, "waldyr Henrique felix Da Silva.");
                var coveredBytes = dct.CalculateIDCT(stegoMatrix);
                TestHelper.PrintMatrix("Coveted matrix", coveredBytes);
                writeBlock("hided.jpg", RGB.Parse(coveredBytes));
            }
        }


        [TestMethod]
        public void Extract_message_from_stego_image()
        {
            string path = Path.Combine(basePath, "hided.jpg");
            using (var stream = new StreamReader(path))
            {
                var reader = new ImageBinaryReader();
                var bytes = reader.ReadRGBFromImage(stream.BaseStream);
                TestHelper.PrintMatrix("Stego img", bytes);

                var dct = new DCT();
                var matrix = dct.CalculateDCT(YCrCb.Parse(bytes));
                TestHelper.PrintMatrix("DCT", matrix);

                var stego = new Steganography();
                var message = stego.ExtractMessage(matrix);

                Console.WriteLine(message);
            }
        }


        private unsafe void writeBlock(string file, RGB[,] block)
        {
            var width = block.GetLength(0);
            var height = block.GetLength(1);
            using (var image = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            {
                var data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                                          PixelFormat.Format24bppRgb);
                var pointer = (byte*) data.Scan0;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        pointer[2] = (byte) block[i, j].R;
                        pointer[1] = (byte) block[i, j].G;
                        pointer[0] = (byte) block[i, j].B;
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