using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StegoJpeg;

namespace DCTAlgorithms
{
    [TestClass]
    public class DCTTest
    {
        [TestMethod]
        public void Given_a_matrix_8x8_calculate_the_DCT_and_IDCT()
        {
            byte[,] matrix = new byte[8,8]
                                 {
                                     {255, 0, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 255, 0}
                                 };

            var calcDct = new DiscreteCosineTransform();

            var dct = calcDct.CalculateDCT(matrix);
            var idct = calcDct.CalculateIDCT(dct);

            TestHelper.PrintMatrix("Original matrix", matrix);
            TestHelper.PrintMatrix("DCT matrix", dct);
            TestHelper.PrintMatrix("IDCT matrix", idct);

            CollectionAssert.AreEquivalent(matrix, idct);
        }

        [TestMethod]
        public void Given_a_matrix_8x8_calculate_the_DCT_and_IDCT_2()
        {
            byte[,] matrix = new byte[8,8]
                                 {
                                     {255, 254, 255, 0, 255, 0, 255, 0},
                                     {254, 254, 255, 0, 255, 0, 255, 0},
                                     {254, 250, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 255, 0},
                                     {255, 0, 250, 0, 225, 0, 255, 0},
                                     {255, 152, 250, 0, 255, 0, 255, 0},
                                     {255, 251, 250, 0, 215, 0, 255, 0},
                                     {255, 0, 255, 0, 255, 0, 205, 0}
                                 };

            var calcDct = new DiscreteCosineTransform();

            var dct = calcDct.CalculateDCT(matrix);
            var idct = calcDct.CalculateIDCT(dct);

            TestHelper.PrintMatrix("Original matrix", matrix);
            TestHelper.PrintMatrix("DCT matrix", dct);
            TestHelper.PrintMatrix("IDCT matrix", idct);

            CollectionAssert.AreEquivalent(matrix, idct);
        }

        [TestMethod]
        public void Given_a_matrix_8x8_calculate_the_DCT_and_IDCT_3()
        {
            byte[,] matrix = new byte[8,8]
                                 {
                                     {1, 19, 75, 132, 127, 209, 93, 110},
                                     {19, 37, 102, 139, 75, 127, 95, 110},
                                     {30, 250, 255, 199, 156, 140, 128, 128},
                                     {32, 230, 255, 250, 255, 1, 255, 0},
                                     {35, 232, 232, 225, 225, 2, 255, 0},
                                     {45, 232, 232, 19, 255, 12, 255, 0},
                                     {52, 232, 231, 17, 215, 14, 255, 0},
                                     {109, 100, 199, 15, 255, 14, 205, 0}
                                 };

            var calcDct = new DiscreteCosineTransform();

            var dct = calcDct.CalculateDCT(matrix);
            var idct = calcDct.CalculateIDCT(dct);

            TestHelper.PrintMatrix("Original matrix", matrix);
            TestHelper.PrintMatrix("DCT matrix", dct);
            TestHelper.PrintMatrix("IDCT matrix", idct);

            CollectionAssert.AreEquivalent(matrix, idct);
        }

        [TestMethod]
        public void Given_a_matrix_8x8_calculate_the_DCT_and_IDCT_4()
        {
            byte[,] matrix = new byte[8,8]
                                 {
                                     {16, 12, 14, 14, 18, 24, 49, 72},
                                     {11, 12, 13, 17, 22, 35, 64, 92},
                                     {10, 14, 16, 22, 37, 55, 78, 95},
                                     {16, 19, 24, 29, 56, 64, 87, 98},
                                     {24, 26, 40, 51, 68, 81, 103, 112},
                                     {40, 58, 57, 87, 109, 104, 121, 100},
                                     {51, 60, 69, 80, 103, 113, 120, 103},
                                     {61, 55, 56, 62, 77, 92, 101, 99}
                                 };

            var calcDct = new DiscreteCosineTransform();

            var dct = calcDct.CalculateDCT(matrix);
            var idct = calcDct.CalculateIDCT(dct);

            TestHelper.PrintMatrix("Original matrix", matrix);
            TestHelper.PrintMatrix("DCT matrix", dct);
            TestHelper.PrintMatrix("IDCT matrix", idct);

            CollectionAssert.AreEquivalent(matrix, idct);
        }

        [TestMethod]
        public void Given_a_matrix_8x8_calculate_the_DCT_and_IDCT_5()
        {
            byte[,] matrix = new byte[8,8]
                                 {
                                     {0, 0, 0, 0, 0, 0, 0, 0},
                                     {0, 0, 0, 0, 0, 0, 0, 0},
                                     {0, 0, 0, 0, 0, 0, 0, 0},
                                     {0, 0, 0, 1, 0, 0, 0, 0},
                                     {0, 0, 0, 0, 0, 0, 0, 0},
                                     {0, 0, 0, 0, 0, 0, 0, 0},
                                     {0, 0, 0, 0, 0, 0, 0, 0},
                                     {0, 0, 0, 0, 0, 0, 0, 0}
                                 };

            var calcDct = new DiscreteCosineTransform();

            var dct = calcDct.CalculateDCT(matrix);
            var idct = calcDct.CalculateIDCT(dct);

            TestHelper.PrintMatrix("Original matrix", matrix);
            TestHelper.PrintMatrix("DCT matrix", dct);
            TestHelper.PrintMatrix("IDCT matrix", idct);

            CollectionAssert.AreEquivalent(matrix, idct);
        }

        [TestMethod]
        public void Given_a_matrix_8x8_calculate_the_DCT_when_modify_any_coefficient_fail()
        {
            byte[,] matrix = new byte[8,8]
                                 {
                                     {1, 0, 255, 254, 253, 8, 255, 0},
                                     {11, 0, 10, 254, 253, 8, 255, 0},
                                     {1, 35, 255, 254, 253, 8, 255, 0},
                                     {12, 0, 255, 200, 253, 8, 255, 0},
                                     {1, 0, 255, 254, 253, 8, 255, 2},
                                     {1, 255, 255, 254, 253, 8, 255, 0},
                                     {14, 0, 50, 254, 30, 8, 255, 0},
                                     {1, 0, 255, 254, 253, 8, 255, 0}
                                 };

            var calcDct = new DiscreteCosineTransform();

            var dct = calcDct.CalculateDCT(matrix);
            dct[0, 0] = 1000;

            var idct = calcDct.CalculateIDCT(dct);

            TestHelper.PrintMatrix("Original matrix", matrix);
            TestHelper.PrintMatrix("DCT matrix", dct);
            TestHelper.PrintMatrix("IDCT matrix", idct);

            CollectionAssert.AreNotEquivalent(matrix, idct);
        }
    }
}