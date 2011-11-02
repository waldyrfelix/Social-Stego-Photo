using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using StegoJpeg.Util;

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

        public List<Point> Permutation(YCrCb[,] matrix)
        {
            var points = new List<Point>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    //if (Math.Abs(matrix[i, j].Y) > 2)
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }
            return points;
        }

        private int bitsPerNonZeroDCTCoefficient(YCrCb[,] matrix)
        {
            int count = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                   // if (Math.Abs(matrix[i, j].Y) > 2)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public string ExtractMessage(YCrCb[,] matrix)
        {
            var path = Permutation(matrix);
            var pathIndex = 0;
            var bitsFromSizeField = new BitArray(32);
            for (pathIndex = 0; pathIndex < bitsFromSizeField.Length; pathIndex++)
                bitsFromSizeField.Set(pathIndex, ((int)matrix[path[pathIndex].X, path[pathIndex].Y].Y) % 2 != 0);

            var bytesFromSizeField = new byte[4];
            bitsFromSizeField.CopyTo(bytesFromSizeField, 0);

            int sizeField = BitConverter.ToInt32(bytesFromSizeField, 0);

            var bitsFromDataField = new BitArray(sizeField);
            for (int i = 0; i < sizeField; i++, pathIndex++)
                bitsFromDataField.Set(i, ((int)matrix[path[pathIndex].X, path[pathIndex].Y].Y) % 2 != 0);

            var bytesFromDataField = new byte[(int)Math.Ceiling(sizeField / 8.0)];
            bitsFromDataField.CopyTo(bytesFromDataField, 0);

            return Encoding.ASCII.GetString(bytesFromDataField);
        }

        public void HideMessage(YCrCb[,] matrix, string message)
        {
            if ((bitsPerNonZeroDCTCoefficient(matrix) / 8) - 4 < message.Length)
                throw new ArgumentException("Message too long to be embedded.");

            var path = Permutation(matrix);
            var bitsFromSizeField = new BitArray(BitConverter.GetBytes(message.Length * 8));
            var bitsFromDataField = new BitArray(Encoding.ASCII.GetBytes(message));
            int i = 0;
            foreach (var bit in bitsFromSizeField)
            {
                matrix[path[i].X, path[i].Y].Y = calculateLSB(matrix[path[i].X, path[i].Y].Y, Convert.ToInt32(bit));
                i++;
            }
            foreach (var bit in bitsFromDataField)
            {
                matrix[path[i].X, path[i].Y].Y = calculateLSB(matrix[path[i].X, path[i].Y].Y, Convert.ToInt32(bit));
                i++;
            }
        }

        private double calculateLSB(double original, int dado)
        {
            return original + dado - (int)original % 2;
        }
    }
}