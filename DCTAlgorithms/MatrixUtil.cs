using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCTAlgorithms
{
    public class MatrixUtil
    {
        public static int[,] Multiply(byte[,] m1, byte[,] m2)
        {
            int i, j, k;
            int row = m1.GetLength(0);
            int col = m2.GetLength(1);
            int[,] m3 = new int[row, col];
            for (i = 0; i < m1.GetLength(0); i++)
            {
                for (j = 0; j < m1.GetLength(1); j++)
                {
                    for (k = 0; k < m2.GetLength(1); k++)
                    {
                        m3[i, k] += m1[i, j] * m2[j, k];
                    }
                }
            }
            return m3;
        }
    }
}
