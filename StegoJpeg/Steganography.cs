using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace StegoJpeg
{
    public class Steganography
    {

        //public List<Point> Permutation(int[,] matrix, int count)
        //{
        //    int width = matrix.GetLength(0);
        //    int height = matrix.GetLength(1);

        //    var points = new List<Point>();
        //    for (int i = 0; i < count; i++)
        //    {
        //        var point = new Point(random.Next(width), random.Next(height));
        //        if (points.Contains(point) || matrix[point.X, point.Y] == 0)
        //        {
        //            i--;
        //        }
        //        else
        //        {
        //            points.Add(point);
        //        }
        //    }
        //    return points;
        //}

        //public List<Point> Permutation(double[,] matrix, int count)
        //{
        //    int xy = 0;
        //    var points = new List<Point>();
        //    for (int i = 0; i < count; i++)
        //    {
        //        points.Add(new Point(xy, xy));
        //        xy += 8;
        //    }
        //    return points;
        //}

        //public List<Point> Permutation(double[,] matrix, int count)
        //{
        //    int width = matrix.GetLength(0);
        //    int height = matrix.GetLength(1);
        //    var random = new Random(615718);

        //    var points = new List<Point>();
        //    for (int i = 0; i < count; i++)
        //    {
        //        var point = new Point(random.Next(width), random.Next(height));
        //        if (points.Contains(point) || matrix[point.X, point.Y] == 0 || Math.Abs(matrix[point.X, point.Y]) == 1.0)
        //        {
        //            i--;
        //        }
        //        else
        //        {
        //            points.Add(point);
        //        }
        //    }
        //    return points;
        //} 

        public List<Point> Permutation(double[,] matrix)
        {
            var points = new List<Point>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != 0 && Math.Abs(matrix[i, j]) != 1)
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }
            return points;
        }

        private int bitsPerNonZeroDCTCoefficient(double[,] matrix)
        {
            int count = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != 0 && Math.Abs(matrix[i, j]) != 1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public string ExtractMessage(double[,] matrix)
        {
            var path = Permutation(matrix);
            var bitArray = new BitArray((int)Math.Ceiling(path.Count / 8.0) * 8);

            for (int i = 0; i < path.Count; i++)
            {
                bitArray.Set(i, ((int)matrix[path[i].X, path[i].Y]) % 2 == 1);
            }

            var bytes = new byte[bitArray.Length / 8];
            bitArray.CopyTo(bytes, 0);

            return Encoding.ASCII.GetString(bytes).Split('\0')[0];
        }

        public double[,] HideMessage(double[,] matrix, string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message + '\0');
            var bitArray = new BitArray(messageBytes);
            var path = Permutation(matrix);
            int i;

            IEnumerator enumerator = bitArray.GetEnumerator();
            for (i = 0; i < path.Count && enumerator.MoveNext(); i++)
            {
                matrix[path[i].X, path[i].Y] = calculateLSB(matrix[path[i].X, path[i].Y], Convert.ToInt32(enumerator.Current));
            }

            return matrix;
        }

        private double calculateLSB(double original, int dado)
        {
            return Math.Round(original + dado - (int)original % 2, 2);
        }
    }
}