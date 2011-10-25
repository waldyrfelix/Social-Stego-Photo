using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StegoJpeg;

namespace StegoJpegTests
{
    [TestClass]
    public class StegoTest
    {
        //private double[,] matrix = new double[,]
        //                               {
        //                                   {144, 145, 142, 146, 148, 150, 146, 146},
        //                                   {132, 130, 130, 126, 128, 147, 126, 126},
        //                                   {177, 177, 180, 181, 179, 170, 181, 181},
        //                                   {186, 186, 184, 182, 182, 183, 182, 182},
        //                                   {105, 119, 127, 131, 0, 0, 0, 0},
        //                                   {145, 137, 134, 139, 0, 0, 0, 0},
        //                                   {127, 128, 124, 124, 0, 0, 0, 0},
        //                                   {173, 178, 181, 179, 0, 0, 0, 0},
        //                               };

        //[TestMethod]
        //public void hide_message_on_matrix()
        //{
        //    const string message = "xua";
        //    TestHelper.PrintMatrix("Original", matrix);

        //    var steganography = new Steganography();
        //    var stego = steganography.HideMessage(matrix, message);

        //    TestHelper.PrintMatrix("Estego", stego);

        //    var extractedMessage = steganography.ExtractMessage(stego);

        //    Console.WriteLine();
        //    Console.WriteLine(extractedMessage);

        //    Assert.AreEqual(message, extractedMessage);
        //}
    }
}