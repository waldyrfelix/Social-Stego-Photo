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
            var matrix = new YCrCb[m,n];

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
            var y = calculateLuminance(rgb);
            return new YCrCb
                       {
                           Y = y,
                           Cr = calculateChrominanceRed(rgb, y),
                           Cb = calculateChrominanceBlue(rgb, y),
                       };
        }

        private static double calculateChrominanceBlue(RGB rgb, double luminance)
        {
            return CbMax * (rgb.B - luminance) / (1 - RGB.Wb);
        }

        private static double calculateChrominanceRed(RGB rgb, double luminance)
        {
            return CrMax * (rgb.R - luminance) / (1 - RGB.Wr);
        }

        private static double calculateLuminance(RGB rgb)
        {
            return rgb.R * RGB.Wr + rgb.G * RGB.Wg + rgb.B * RGB.Wb;
        }
    }
}
