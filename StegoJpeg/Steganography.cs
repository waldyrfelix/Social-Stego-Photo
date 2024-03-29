﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using StegoJpeg.Util;

namespace StegoJpeg
{
    public class Steganography
    {
        public List<Point> Permutation(YCrCb[,] matrix)
        {
            var points = new List<Point>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j].Y != 0 && matrix[i, j].Y != 1)
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
                    if (matrix[i, j].Y != 0 && matrix[i, j].Y != 1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public byte[] ExtractMessage(YCrCb[,] matrix)
        {
            var path = Permutation(matrix);
            var pathIndex = 0;
            var bitsFromSizeField = new BitArray(32);
            for (pathIndex = 0; pathIndex < bitsFromSizeField.Length; pathIndex++)
            {
                int lsb = ((int) matrix[path[pathIndex].X, path[pathIndex].Y].Y)%2;
                bitsFromSizeField.Set(pathIndex, lsb != 0);
            }
            var bytesFromSizeField = new byte[4];
            bitsFromSizeField.CopyTo(bytesFromSizeField, 0);

            int sizeField = BitConverter.ToInt32(bytesFromSizeField, 0);

            var bitsFromDataField = new BitArray(sizeField);
            for (int i = 0; i < sizeField; i++, pathIndex++)
                bitsFromDataField.Set(i, ((int)matrix[path[pathIndex].X, path[pathIndex].Y].Y) % 2 != 0);

            var bytesFromDataField = new byte[(int)Math.Ceiling(sizeField / 8.0)];
            bitsFromDataField.CopyTo(bytesFromDataField, 0);

            return bytesFromDataField;
        }

        public void HideMessage(YCrCb[,] matrix, byte[] message)
        {
            if (bitsPerNonZeroDCTCoefficient(matrix) - 32 < message.Length * 8)
                throw new ArgumentException("Message too long to be embedded.");

            var path = Permutation(matrix);
            var bitsFromSizeField = new BitArray(BitConverter.GetBytes(message.Length * 8));
            int i = 0;
            foreach (var bit in bitsFromSizeField)
            {
                matrix[path[i].X, path[i].Y].Y = calculateLSB(matrix[path[i].X, path[i].Y].Y, Convert.ToInt32(bit));
                i++;
            }

            var bitsFromDataField = new BitArray(message);
            i = 32;
            foreach (var bit in bitsFromDataField)
            {
                matrix[path[i].X, path[i].Y].Y = calculateLSB(matrix[path[i].X, path[i].Y].Y, Convert.ToInt32(bit));
                i++;
            }
        }

        private double calculateLSB(double original, int dado)
        {
            return original + dado - (int) Math.Abs(original % 2);
        }
    }
}