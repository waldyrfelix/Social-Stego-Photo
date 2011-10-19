using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DCTAlgorithms
{
    public class ImageBinaryReader
    {
        public byte[,] ReadAllBytes(Stream stream)
        {
            using (var image = new Bitmap(stream))
            {
                int width = (int)(8 * Math.Ceiling(image.Width / 8.0));
                int height = (int)(8 * Math.Ceiling(image.Height / 8.0));
                var superBlock = new byte[width, height];
                BitmapData blockData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    var pointer = (byte*)blockData.Scan0;

                    for (int i = 0; i < blockData.Width; i++)
                    {
                        for (int j = 0; j < blockData.Height; j++)
                        {
                            superBlock[i, j] = (byte)Math.Round((pointer[0] + pointer[1] + pointer[2]) / 3.0);
                            //superBlock[i, j] = pointer[0];
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
