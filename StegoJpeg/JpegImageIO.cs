using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using StegoJpeg.Util;

namespace StegoJpeg
{
    public class JpegImageIO
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
                            superBlock[i, j].R = pointer[0];
                            superBlock[i, j].G = pointer[1];
                            superBlock[i, j].B = pointer[2];
                            pointer += 3;
                        }
                        pointer += blockData.Stride - (blockData.Width * 3);
                    }
                }
                image.UnlockBits(blockData);

                return superBlock;
            }
        }

        public void WriteRGBToImage(string path, RGB[,] matrix)
        {
            var width = matrix.GetLength(0);
            var height = matrix.GetLength(1);
            using (var image = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            {
                var data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                                          PixelFormat.Format24bppRgb);
                unsafe
                {
                    var pointer = (byte*)data.Scan0;
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            pointer[0] = (byte)matrix[i, j].R;
                            pointer[1] = (byte)matrix[i, j].G;
                            pointer[2] = (byte)matrix[i, j].B;
                            pointer += 3;
                        }
                    }
                }
                image.UnlockBits(data);
                image.Save(path);
            }
        }
    }
}
