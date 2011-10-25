using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DCTAlgorithms
{
    public class ImageBinaryReader
    {
        public void OverrideLuminance(string path, byte[,] luminanceMatrix)
        {
            using (var image = new Bitmap(path))
            {

            }
        }

        public byte[,] ReadLuminanceFromImage(Stream stream)
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
                            superBlock[i, j] = calcY(pointer);
                            pointer += 3;
                        }
                        pointer += blockData.Stride - (blockData.Width * 3);
                    }
                }
                image.UnlockBits(blockData);

                return superBlock;
            }
        }

        private unsafe byte calcY(byte* pointer)
        {
            return calcY(pointer[2], pointer[1], pointer[0]);
        }

        private byte calcY(byte red, byte green, byte blue)
        {
            return (byte)(red * 0.299 + green * 0.587 + blue * 0.114);
        }

        //private byte calcU()
    }
}
