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
            string path = Path.Combine(basePath, "lenna.png");
            using (var stream = new StreamReader(path))
            {
                var imageIo = new JpegImageIO();
                var bytesRGB = imageIo.ReadRGBFromImage(stream.BaseStream);

                //TestHelper.PrintMatrix("Binary image", bytesRGB);
                //imageIo.WriteRGBToImage(Path.Combine(basePath, "original.jpg"), bytesRGB);

                var matrix = YCrCb.Parse(bytesRGB);
                //TestHelper.PrintMatrix("Luminance Coeff", matrix);

                var dct = new DCT();
                dct.Subsample(matrix);
                dct.CalculateDCT(matrix);
                //TestHelper.PrintMatrix("DCT", matrix);

                Quantizer q = new Quantizer();
                q.ApplyQuantization(matrix, 90);
                //TestHelper.PrintMatrix("Quantized DCT", matrix);
                
                var stego = new Steganography();
                stego.HideMessage(matrix, "di");
                //TestHelper.PrintMatrix("Stego", matrix);

                q.ApplyInverseQuantization(matrix, 90);
                //TestHelper.PrintMatrix("Unquantized DCT", matrix);

                dct.CalculateIDCT(matrix);
                dct.Supersample(matrix);
                //TestHelper.PrintMatrix("Inversed DCT", matrix);
                
                imageIo.WriteRGBToImage(Path.Combine(basePath, "hided-90.jpg"), RGB.Parse(matrix));
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
                var matrix = YCrCb.Parse(bytes);
                dct.CalculateDCT(matrix);
                TestHelper.PrintMatrix("DCT", matrix);

                new Quantizer().ApplyQuantization(matrix, 100);
                TestHelper.PrintMatrix("QDCT", matrix);

                var stego = new Steganography();
                var message = stego.ExtractMessage(matrix);

                Console.WriteLine(message);
            }
        }
    }
}