using System;

namespace StegoJpeg.Util
{
    public struct YCrCb
    {
        public const double CrMax = 0.436;
        public const double CbMax = 0.615;

        public double Y { get; set; }
        public double Cr { get; set; }
        public double Cb { get; set; }

        public static YCrCb[,] Parse(RGB[,] rgb)
        {
            int m = rgb.GetLength(0);
            int n = rgb.GetLength(1);
            var matrix = new YCrCb[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = Parse(rgb[i, j]);
                }
            }
            return matrix;
        }

        public static YCrCb Parse(RGB rgb)
        {
            return new YCrCb
                       {
                           Y = calculateY(rgb),
                           Cr = calculateCr(rgb),
                           Cb = calculateCb(rgb),
                       };
        }

        private static double calculateCb(RGB rgb)
        {
            return Math.Round(128 + (-0.169 * rgb.R - 0.331 * rgb.G + 0.5 * rgb.B));

        }

        private static double calculateCr(RGB rgb)
        {
            return Math.Round(128 + (0.5 * rgb.R - 0.419 * rgb.G - 0.081 * rgb.B));
        }

        private static double calculateY(RGB rgb)
        {
            return Math.Round(rgb.R * 0.299 + rgb.G * 0.587 + rgb.B * 0.114);
        }
    }
}
