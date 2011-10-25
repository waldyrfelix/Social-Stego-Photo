using System;
using StegoJpeg.Util;

namespace StegoJpegTests
{
    public class TestHelper
    {
        public static void PrintMatrix(string name, byte[,] matrix)
        {
            Console.WriteLine(name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write((matrix[i, j]).ToString().PadLeft(4, ' '));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static void PrintMatrix(string name, int[,] matrix)
        {
            Console.WriteLine(name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write((matrix[i, j]).ToString().PadLeft(4, ' '));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static void PrintMatrix(string name, double [,] matrix)
        {
            Console.WriteLine(name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write((matrix[i, j]).ToString().PadLeft(10, ' '));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static void PrintMatrix(string name, RGB [,] matrix)
        {
            Console.WriteLine(name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write((matrix[i, j].B).ToString().PadLeft(10, ' '));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static void PrintMatrix(string name, YCrCb[,] matrix)
        {
            Console.WriteLine(name);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write((matrix[i, j].Y).ToString().PadLeft(10, ' '));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
