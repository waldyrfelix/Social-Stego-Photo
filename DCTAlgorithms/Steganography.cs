using System;
using System.Collections;
using System.Text;

namespace DCTAlgorithms
{
    public class Steganography
    {
        public string ExtractMessage(int[,] dctMatrix)
        {
            BitArray bitArray = new BitArray(6 * 8);
            int index = 0;
            for (int i = 0; i < dctMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < dctMatrix.GetLength(1) && index < bitArray.Length; j++)
                {
                    if (dctMatrix[i, j] != 0)
                    {
                        bitArray.Set(index, Convert.ToBoolean(dctMatrix[i, j] % 2));
                        index++;
                    }
                }
            }

            byte[] bytes = new byte[bitArray.Length / 8];
            bitArray.CopyTo(bytes, 0);

            return Encoding.ASCII.GetString(bytes);
        }

        public int[,] HideMessage(int[,] matrix, string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            var bitArray = new BitArray(messageBytes);

            IEnumerator enumerator = bitArray.GetEnumerator();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] > 10)
                    {
                        if (!enumerator.MoveNext()) break;

                        int bit = Convert.ToInt32(enumerator.Current);
                        if (matrix[i, j] % 2 != bit)
                        {
                            if (matrix[i, j] % 2 == 1)
                            {
                                matrix[i, j]--;
                            }
                            else
                            {
                                matrix[i, j]++;
                            }
                        }
                    }
                }
            }
            return matrix;
        }
    }
}
