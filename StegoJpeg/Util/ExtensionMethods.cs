using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StegoJpeg.Util
{
    public static class ExtensionMethods
    {
        public static double ToByteBounds(this double number)
        {
            return Math.Max(0, Math.Min(255, number));
        }
    }
}
