using System;

namespace StegoJpeg.Util
{
    public struct RGB
    {
        public const double Wr = 0.299;
        public const double Wg = 0.587;
        public const double Wb = 0.114;

        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public static RGB[,] Parse(YCrCb[,] yCrCb)
        {
            int m = yCrCb.GetLength(0);
            int n = yCrCb.GetLength(1);
            var matrix = new RGB[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = Parse(yCrCb[i, j]);
                }
            }

            return matrix;
        }

        public static RGB Parse(YCrCb yCrCb)
        {
            return new RGB
                       {
                           R = calculateRed(yCrCb),
                           B = calculateBlue(yCrCb),
                           G = calculateGreen(yCrCb)
                       };
        }

        private static double calculateRed(YCrCb yCrCb)
        {
            return  Math.Round(yCrCb.Y + 1.402 * (yCrCb.Cr - 128)).ToByteBounds() ;
        }

        private static double calculateGreen(YCrCb yCrCb)
        {
            return  Math.Round(yCrCb.Y - 0.34414*(yCrCb.Cb - 128) - 0.71414*(yCrCb.Cr - 128)).ToByteBounds();
        }

        private static double calculateBlue(YCrCb yCrCb)
        {
            return Math.Round(yCrCb.Y + 1.772*(yCrCb.Cb - 128)).ToByteBounds();
        }
    }
}