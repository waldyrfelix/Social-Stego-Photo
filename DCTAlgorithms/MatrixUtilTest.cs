using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StegoJpeg;

namespace DCTAlgorithms
{
    [TestClass]
    public class MatrixUtilTest
    {
        [TestMethod]
        public void Multiply_two_matrix_2x2()
        {
            byte[,] m1 = new byte[,]
                                {
                                    {1, 2}, 
                                    {1, 3}, 
                                };

            byte[,] m2 = new byte[,]
                                {
                                    {2, 2}, 
                                    {2, 0}, 
                                };

            int[,] M = new int[,]
                                {
                                    {6, 2}, 
                                    {8, 2}, 
                                };

            var result =MatrixUtil.Multiply(m1, m2);

            CollectionAssert.AreEquivalent(M, result);
        }
        
        [TestMethod]
        public void Multiply_two_matrix_3x2_by_2x3()
        {
            byte[,] m1 = new byte[,]
                                {
                                    {1, 2}, 
                                    {1, 3}, 
                                    {2, 1}, 
                                };

            byte[,] m2 = new byte[,]
                                {
                                    {2, 2, 1}, 
                                    {2, 0, 1}, 
                                };

            int[,] M = new int[,]
                                {
                                    {6, 2, 3}, 
                                    {8, 2, 4}, 
                                    {6, 4, 3}, 
                                };

            var result =MatrixUtil.Multiply(m1, m2);

            CollectionAssert.AreEquivalent(M, result);
        }

        [TestMethod]
        public void Multiply_two_matrix_3x3_by_3x1()
        {
            byte[,] m1 = new byte[,]
                                {
                                    {1, 2, 1}, 
                                    {1, 3, 0}, 
                                    {2, 1, 0}, 
                                };

            byte[,] m2 = new byte[,]
                                {
                                    {2}, 
                                    {2},
                                    {1}
                                };

            int[,] M = new int[,]
                                {
                                    {7}, 
                                    {8}, 
                                    {6}, 
                                };

            var result =MatrixUtil.Multiply(m1, m2);

            CollectionAssert.AreEquivalent(M, result);
        }

        [TestMethod]
        public void Multiply_matrix_performance_test()
        {
            byte[,] m1 = new byte[,]
                                {
                                    {1, 2,  1, 2}, 
                                    {1, 3,  0, 2}, 
                                    {2, 1,  0, 1}, 
                                    {2, 1, 10, 1}, 
                                };

            byte[,] m2 = new byte[,]
                                {
                                    {1, 0, 0, 0}, 
                                    {0, 1, 0, 0}, 
                                    {0, 0, 1, 0}, 
                                    {0, 0, 0, 1}, 
                                };

            int[,] M = new int[,]
                                {
                                    {1, 2,  1, 2}, 
                                    {1, 3,  0, 2}, 
                                    {2, 1,  0, 1}, 
                                    {2, 1, 10, 1}, 
                                };


            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 100000; i++)
            {
                var result = MatrixUtil.Multiply(m1, m2);                
            }
            watch.Stop();

            Console.WriteLine(watch.Elapsed);
        }

    }
}
