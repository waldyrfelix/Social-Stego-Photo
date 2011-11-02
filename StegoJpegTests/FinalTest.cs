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
            const int qf = 90;

            string path = Path.Combine(basePath, "teste3.jpg");
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
                q.ApplyQuantization(matrix, qf);
                TestHelper.PrintMatrix("Quantized DCT", matrix);
                
                var stego = new Steganography();
                stego.HideMessage(matrix, @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas euismod sagittis ullamcorper. Pellentesque nec metus eget nunc pretium pretium porta eget felis. Maecenas fermentum hendrerit malesuada. Donec fermentum varius massa a elementum. Suspendisse at dapibus enim. Cras non enim tortor. Mauris enim nunc, cursus et tempus quis, molestie nec ligula. Fusce tellus tortor, eleifend eget consectetur eu, ornare a magna. Mauris scelerisque ornare convallis. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vitae leo non dui blandit tristique in vel magna. Nulla sed tristique quam. Nulla sit amet risus nec elit faucibus consectetur. Fusce dapibus velit et lacus volutpat convallis.

Vestibulum condimentum libero quis neque dignissim porta. Aliquam sapien justo, semper vitae ornare vel, fringilla id ante. Cras eleifend, risus ut feugiat adipiscing, augue turpis sagittis dolor, sed hendrerit elit sem vel turpis. Curabitur ac metus in turpis ultricies egestas vestibulum nec nisl. Quisque et odio metus. Aenean at tortor ac mi viverra luctus. Vivamus odio metus, vestibulum et tristique in, fermentum eu nibh.

Fusce auctor mollis leo, eget convallis nulla laoreet imperdiet. Sed eget felis ut est gravida pulvinar. Fusce elit turpis, pulvinar pellentesque posuere nec, ultrices eget purus. Praesent sit amet faucibus arcu. Nulla ornare urna vitae dui porta vestibulum. Pellentesque id ligula orci, eget porttitor justo. Suspendisse in eros in neque sagittis feugiat eu id ante. Ut commodo semper neque et luctus. Ut a tellus eget nulla egestas malesuada nec placerat odio. Duis dignissim eros leo. Aliquam erat volutpat.

Nam eget purus at libero fermentum consectetur non in nibh. Curabitur placerat mi eu nisi tristique et aliquam est tristique. Duis leo neque, sagittis id suscipit ac, blandit convallis purus. Fusce tincidunt ultricies leo quis malesuada. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Suspendisse laoreet, nisl eu suscipit aliquet, enim felis pharetra ante, ut faucibus turpis ante at arcu. Mauris in turpis vitae enim placerat iaculis nec non nunc. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Suspendisse ac lectus condimentum sem laoreet sodales. Integer aliquet tortor nec sem porttitor congue.

Donec iaculis est eu elit pellentesque laoreet. Mauris accumsan viverra quam, sit amet lobortis libero venenatis feugiat. Mauris et ipsum orci, sed sagittis erat. Quisque vel tellus dui. Mauris tempus urna quis augue sollicitudin dapibus commodo risus pulvinar. Integer condimentum odio quis turpis ultricies a luctus mauris iaculis. Fusce non fringilla orci. Pellentesque vitae aliquet erat. Mauris a volutpat elit. Ut commodo consectetur est a adipiscing. Aliquam erat volutpat. Proin mattis ante eu ligula tincidunt feugiat.");
                TestHelper.PrintMatrix("Stego", matrix);

                q.ApplyInverseQuantization(matrix, qf);
                //TestHelper.PrintMatrix("Unquantized DCT", matrix);

                dct.CalculateIDCT(matrix);
                dct.Supersample(matrix);
                //TestHelper.PrintMatrix("Inversed DCT", matrix);
                
                imageIo.WriteRGBToImage(Path.Combine(basePath, "hided.jpg"), RGB.Parse(matrix));
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

                var matrix = YCrCb.Parse(bytes);
                //TestHelper.PrintMatrix("Luminance Coeff", matrix);

                var dct = new DCT();
                dct.Subsample(matrix);
                dct.CalculateDCT(matrix);
                //TestHelper.PrintMatrix("DCT", matrix);

                new Quantizer().ApplyQuantization(matrix, 90);
                TestHelper.PrintMatrix("QDCT", matrix);

                var stego = new Steganography();
                var message = stego.ExtractMessage(matrix);

                Console.WriteLine(message);
            }
        }
    }
}