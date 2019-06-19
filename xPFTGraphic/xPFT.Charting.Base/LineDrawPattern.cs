using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPFT.Charting.Base
{
    public enum LineDrawPattern
    {
        SOLID = 0,              // 11111111111111111111111111111111
        DASH = 1,               // 11110000111100001111000011110000
        DASH_DOT = 2,           // 11111010111110101111101011111010
        SMALL_DASH = 3,         // 11101110111011101110111011101110
        DOT = 4,                // 10101010101010101010101010101010
        DASH_DOT_DOT=5          // 11111100110011001111110011001100

    }
    public class GetLineDrawPattern
    {
        public static int ForD3D9(LineDrawPattern pattern)
        {
            if (pattern == LineDrawPattern.SOLID)
                return Convert.ToInt32("11111111111111111111111111111111", 2);
            if (pattern == LineDrawPattern.DASH)
                return Convert.ToInt32("11111111000111111110001111111100", 2);
            if (pattern == LineDrawPattern.DASH_DOT)
                return Convert.ToInt32("11111111110011001111111111001100", 2);
            if (pattern == LineDrawPattern.SMALL_DASH)
                return Convert.ToInt32("11111100111111001111110011111100", 2);
            if (pattern == LineDrawPattern.DOT)
                return Convert.ToInt32("11001100110011001100110011001100", 2);
            if (pattern == LineDrawPattern.DASH_DOT_DOT)
                return Convert.ToInt32("11111100110011001111110011001100", 2);
            return 0;
        }

        public static float[] ForD2D1(LineDrawPattern pattern)
        {
            if (pattern == LineDrawPattern.SOLID)
                return new float[] { 1, 0 };
            if (pattern == LineDrawPattern.DASH)
                return new float[] { 4, 4 };
            if (pattern == LineDrawPattern.DASH_DOT)
                return new float[] { 5, 1, 1, 1 };
            if (pattern == LineDrawPattern.SMALL_DASH)
                return new float[] { 3, 1 };
            if (pattern == LineDrawPattern.DOT)
                return new float[] { 1, 1 };
            if (pattern == LineDrawPattern.DASH_DOT_DOT)
                return new float[] { 3, 1, 1, 1, 1, 1 };
            return new float[] { 1, 0 };
        }
    }

}
