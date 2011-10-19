using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCTAlgorithms
{
    [TestClass]
    public class StegoTest
    {
        private byte [,] matrix = new byte[,]
                                       {
                                           {144, 145, 142, 146, 148, 150, 146, 146},
                                           {132, 130, 130, 126, 128, 147, 126, 126},
                                           {177, 177, 180, 181, 179, 170, 181, 181},
                                           {186, 186, 184, 182, 182, 183, 182, 182},
                                           {105, 119, 127, 131, 135, 137, 131, 131},
                                           {145, 137, 134, 139, 145, 144, 139, 139},
                                           {127, 128, 124, 124, 120, 119, 124, 124},
                                           {173, 178, 181, 179, 174, 174, 179, 179},
                                       };

        [TestMethod]
        public void hide_message_on_matrix()
        {
            TestHelper.PrintMatrix("Original", matrix);

            var dct = new DiscreteCosineTransform().CalculateDCT(matrix);

            Steganography steganography = new Steganography();
            var stego = steganography.HideMessage(dct, "Waldyr");

            TestHelper.PrintMatrix("Estego", stego);

            var message = steganography.ExtractMessage(stego);

            Console.WriteLine();
            Console.WriteLine(message);

            Assert.AreEqual("Waldyr", message);
        }
    }
}