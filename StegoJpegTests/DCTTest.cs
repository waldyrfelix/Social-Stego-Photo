using StegoJpeg;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StegoJpeg.Util;

namespace StegoJpegTests
{
    [TestClass]
    public class DCTTest
    {
        [TestMethod]
        public void Matrix_is_the_same_when_DCT_and_IDCT_are_aplied()
        {
            var matrix = convertToYCrCb(new double[,]
                                            {
                                                {26, 26, 26, 25, 25, 25, 26, 26},
                                                {26, 26, 26, 25, 25, 25, 26, 26},
                                                {26, 26, 26, 25, 25, 25, 26, 26},
                                                {26, 26, 26, 25, 25, 25, 26, 26},
                                                {26, 26, 26, 25, 25, 25, 26, 26},
                                                {26, 26, 26, 25, 25, 25, 26, 26},
                                                {26, 26, 26, 25, 25, 25, 26, 26},
                                                {26, 26, 26, 25, 25, 25, 26, 26},
                                            });

            var expectedMatrix = MatrixUtil.Copy(matrix);

            DCT dct = new DCT();
            TestHelper.PrintMatrix("DCT", matrix);

            dct.CalculateDCT(matrix);
            TestHelper.PrintMatrix("DCT", matrix);
            dct.CalculateIDCT(matrix);

            var doubleExpectedMatrix = convertToDouble(expectedMatrix);
            var doubleMatrix = convertToDouble(matrix);
            CollectionAssert.AreEquivalent(doubleExpectedMatrix, doubleMatrix);
        }

        private double[,] convertToDouble(YCrCb[,] expectedMatrix)
        {
            var convertedMatrix = new double[8,8];
            for (int m = 0; m < 8; m++)
            {
                for (int n = 0; n < 8; n++)
                {
                    convertedMatrix[m, n] = expectedMatrix[m, n].Y;
                }
            }

            return convertedMatrix;
        }

        private YCrCb[,] convertToYCrCb(double[,] matrix)
        {
            var convertedMatrix = new YCrCb[8,8];
            for (int m = 0; m < 8; m++)
            {
                for (int n = 0; n < 8; n++)
                {
                    convertedMatrix[m, n] = new YCrCb {Y = matrix[m, n]};
                }
            }

            return convertedMatrix;
        }
    }
}