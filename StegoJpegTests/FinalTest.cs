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
            @"C:\Users\waldyr\Documents\Git\SocialStegoPhoto\StegoJpegTests\";
        const int qf = 90;

        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
        }

//        [TestMethod]
//        public void Hide_message_with_steganography()
//        {

//            string path = Path.Combine(basePath, "lenna.png");
//            using (var stream = new StreamReader(path))
//            {
//                var imageIo = new JpegImageIO();
//                var bytesRGB = imageIo.ReadRGBFromImage(stream.BaseStream);

//                TestHelper.PrintMatrix("Binary image", bytesRGB);
//                //imageIo.WriteRGBToImage(Path.Combine(basePath, "original.jpg"), bytesRGB);

//                var matrix = YCrCb.Parse(bytesRGB);
//           //     TestHelper.PrintMatrix("Luminance Coeff", matrix);

//                var dct = new DCT();
//               // dct.Subsample(matrix);
//                dct.CalculateDCT(matrix);
//             //   TestHelper.PrintMatrix("DCT", matrix);

//                Quantizer q = new Quantizer();
//                q.ApplyQuantization(matrix, qf);
//                //TestHelper.PrintMatrix("Quantized DCT", matrix);

//                var stego = new Steganography();
//                stego.HideMessage(matrix, @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce elit turpis, pulvinar pellentesque posuere nec, ultrices eget purus. Praesent sit amet faucibus arcu. Nulla ornare urna vitae dui porta vestibulum. Pellentesque id ligula orci, eget porttitor justo. Suspendisse in eros in neque sagittis feugiat eu id ante. Ut commodo semper neque et luctus. Ut a tellus eget nulla egestas malesuada nec placerat odio. Duis dignissim eros leo. Aliquam erat volutpat.
//
//Donec iaculis est eu elit pellentesque laoreet. Mauris accumsan viverra quam, sit amet lobortis libero venenatis feugiat. Mauris et ipsum orci, sed sagittis erat. Quisque vel tellus dui. Mauris tempus urna quis augue sollicitudin dapibus commodo risus pulvinar. Integer condimentum odio quis turpis ultricies a luctus mauris iaculis. Fusce non fringilla orci. Pellentesque vitae aliquet erat. Mauris a volutpat elit. Ut commodo consectetur est a adipiscing. Aliquam erat volutpat. Proin mattis ante eu ligula tincidunt feugiat.");
//                TestHelper.PrintMatrix("Stego", matrix);
                
//                q.ApplyInverseQuantization(matrix, qf);
//           //     TestHelper.PrintMatrix("Unquantized DCT", matrix);

//                dct.CalculateIDCT(matrix);
//               // dct.Supersample(matrix);
//             //   TestHelper.PrintMatrix("Inversed DCT", matrix);
                
//                imageIo.WriteRGBToImage(Path.Combine(basePath, "hided.jpg"), RGB.Parse(matrix));
//            }
//        }


        [TestMethod]
        public void Extract_message_from_stego_image()
        {
            string path = Path.Combine(basePath, "hided.jpg");
            using (var stream = new StreamReader(path))
            {
                var reader = new JpegImageIO();
                var bytes = reader.ReadRGBFromImage(stream.BaseStream);
                //TestHelper.PrintMatrix("Stego img", bytes);

                var matrix = YCrCb.Parse(bytes);
                //TestHelper.PrintMatrix("Luminance Coeff", matrix);

                var dct = new DCT();
                //dct.Subsample(matrix);
                dct.CalculateDCT(matrix);
                //TestHelper.PrintMatrix("DCT", matrix);

                new Quantizer().ApplyQuantization(matrix, qf);
               // TestHelper.PrintMatrix("QDCT", matrix);

                var stego = new Steganography();
                var message = stego.ExtractMessage(matrix);

                Console.WriteLine(message);
            }
        }
    }
}