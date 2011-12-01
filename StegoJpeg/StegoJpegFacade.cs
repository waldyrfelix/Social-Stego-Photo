using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StegoJpeg.Util;

namespace StegoJpeg
{
   public class StegoJpegFacade
   {
       private const int Qf=90;
       public void EmbedData(string path, byte[] message)
       {
           using (var stream = new StreamReader(path))
           {
               var imageIo = new JpegImageIO();
               var bytesRGB = imageIo.ReadRGBFromImage(stream.BaseStream);

               var matrix = YCrCb.Parse(bytesRGB);

               var dct = new DCT();
               dct.CalculateDCT(matrix);

               Quantizer q = new Quantizer();

               q.ApplyQuantization(matrix, Qf);

               var stego = new Steganography();
               stego.HideMessage(matrix, message);

               q.ApplyInverseQuantization(matrix, Qf);

               dct.CalculateIDCT(matrix);


               string pathToSave = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + "_stego.jpg");

               imageIo.WriteRGBToImage(pathToSave, RGB.Parse(matrix));
           }
       }

       public byte[] ExtractData(string path)
       {
           using (var stream = new StreamReader(path))
           {
               var reader = new JpegImageIO();
               var bytes = reader.ReadRGBFromImage(stream.BaseStream);

               var matrix = YCrCb.Parse(bytes);

               var dct = new DCT();
               dct.CalculateDCT(matrix);

               new Quantizer().ApplyQuantization(matrix, Qf);

               var stego = new Steganography();
               
               return stego.ExtractMessage(matrix);
           }
       }
    }
}
