using System;
namespace StegoJpeg
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

        public static T[,] Copy<T>(T[,] matrix) where T: ICloneable
        {
            var copy = new T[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    copy[i, j] = (T) matrix[i, j].Clone();
                }
            }
            return copy;
        }
    }
}
