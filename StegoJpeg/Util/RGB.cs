using System;

namespace StegoJpeg.Util
{
    public struct RGB
    {
        public const double Wr = 0.299;
        public const double Wg = 0.114;
        public const double Wb = 0.587;

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

        private static double calculateGreen(YCrCb yCrCb)
        {
            return yCrCb.Y - yCrCb.Cb * (Wb * (1 - Wb)) / YCrCb.CbMax * Wg - yCrCb.Cr * (Wr * (1 - Wr)) / YCrCb.CrMax * Wg;
        }

        private static double calculateBlue(YCrCb yCrCb)
        {
            return yCrCb.Y + yCrCb.Cb * (1 - Wb) / YCrCb.CbMax;
        }

        private static double calculateRed(YCrCb yCrCb)
        {
            return yCrCb.Y + yCrCb.Cr * (1 - Wr) / YCrCb.CrMax;
        }
    }
}