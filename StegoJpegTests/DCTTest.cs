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
                                 {212, 217, 211, 109, 180, 220, 218, 221},
                                 {215, 217, 215, 107, 182, 222, 228, 217},
                                 {222, 216, 207, 116, 204, 216, 223, 223},
                                 {218, 220, 198, 134, 222, 217, 218, 223},
                                 {215, 211, 177, 134, 189, 202, 225, 218},
                                 {216, 164, 139, 136, 148, 164, 208, 221},
                                 {193, 117, 99, 115, 118, 126, 165, 219},
                                 {140, 113, 125, 136, 157, 154, 143, 193},
                             });

            var expectedMatrix = MatrixUtil.Copy(matrix);

            DCT dct = new DCT();
            dct.CalculateDCT(matrix);
            dct.CalculateIDCT(matrix);

            var doubleExpectedMatrix = convertToDouble(expectedMatrix);
            var doubleMatrix = convertToDouble(matrix);
            CollectionAssert.AreEquivalent(doubleExpectedMatrix, doubleMatrix);
        }

        private double[,] convertToDouble(YCrCb[,] expectedMatrix)
        {
            var convertedMatrix = new double[8, 8];
            for (int m = 0; m < 8; m++)
            {
                for (int n = 0; n < 8; n++)
                {
                    convertedMatrix[m, n] = expectedMatrix[m,n].Y;
                }
            }

            return convertedMatrix;
        }

        private YCrCb[,] convertToYCrCb(double[,] matrix)
        {
            var convertedMatrix = new YCrCb[8, 8];
            for (int m = 0; m < 8; m++)
            {
                for (int n = 0; n < 8; n++)
                {
                    convertedMatrix[m, n] = new YCrCb { Y = matrix[m, n] };
                }
            }

            return convertedMatrix;
        }
    }
}