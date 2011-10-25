using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using StegoJpeg.Util;

namespace StegoJpeg
{
    public class ImageBinaryReader
    {
        public RGB[,] ReadRGBFromImage(Stream stream)
        {
            using (var image = new Bitmap(stream))
            {
                int width = (int)(8 * Math.Ceiling(image.Width / 8.0));
                int height = (int)(8 * Math.Ceiling(image.Height / 8.0));
                var superBlock = new RGB[width, height];
                BitmapData blockData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    var pointer = (byte*)blockData.Scan0;

                    for (int i = 0; i < blockData.Width; i++)
                    {
                        for (int j = 0; j < blockData.Height; j++)
                        {
                            superBlock[i, j].R = pointer[2];
                            superBlock[i, j].G = pointer[1];
                            superBlock[i, j].B = pointer[0];
                            pointer += 3;
                        }
                        pointer += blockData.Stride - (blockData.Width * 3);
                    }
                }
                image.UnlockBits(blockData);

                return superBlock;
            }
        }
    }
}
