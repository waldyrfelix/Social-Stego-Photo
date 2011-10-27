using System;
using System.IO;
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

        [TestMethod]
        public void Hide_message_with_steganography()
        {
            string path = Path.Combine(basePath, "teste3.jpg");
            using (var stream = new StreamReader(path))
            {
                var imageIo = new JpegImageIO();
                var bytesRGB = imageIo.ReadRGBFromImage(stream.BaseStream);

               // TestHelper.PrintMatrix("Binary image", bytesRGB);
                imageIo.WriteRGBToImage(Path.Combine(basePath, "original.jpg"), bytesRGB);

                var dct = new DCT();
                var matrix = dct.CalculateDCT(YCrCb.Parse(bytesRGB));
                TestHelper.PrintMatrix("DCT matrix", matrix);

                Quantizer q = new Quantizer();
                q.ApplyQuantization(matrix);
                TestHelper.PrintMatrix("Quantized DCT matrix", matrix);
                
                var stego = new Steganography();
                var stegoMatrix = stego.HideMessage(matrix, "waldyr Henrique felix Da Silva.");

                q.ApplyInverseQuantization(stegoMatrix);

                var coveredBytes = dct.CalculateIDCT(stegoMatrix);

                TestHelper.PrintMatrix("Coveted matrix", coveredBytes);
                imageIo.WriteRGBToImage(Path.Combine(basePath, "hided.jpg"), RGB.Parse(coveredBytes));
            }
        }


        [TestMethod]
        public void Extract_message_from_stego_image()
        {
            string path = Path.Combine(basePath, "hided.jpg");
            using (var stream = new StreamReader(path))
            {
                var reader = new JpegImageIO();
                var bytes = reader.ReadRGBFromImage(stream.BaseStream);
                //TestHelper.PrintMatrix("Stego img", bytes);

                var dct = new DCT();
                var matrix = dct.CalculateDCT(YCrCb.Parse(bytes));
                TestHelper.PrintMatrix("DCT", matrix);

                new Quantizer().ApplyQuantization(matrix);
                TestHelper.PrintMatrix("QDCT", matrix);

                var stego = new Steganography();
                var message = stego.ExtractMessage(matrix);

                Console.WriteLine(message);
            }
        }
    }
}