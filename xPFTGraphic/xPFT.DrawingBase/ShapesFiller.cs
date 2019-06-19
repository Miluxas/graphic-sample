using System;
using System.Collections.Generic;
using System.Text;

namespace xPFT.DrawingBase
{
    public class ShapesFiller
    {
        /// <summary>
        /// Get the vector2 array that if draw line on that collection drawed an ellipse.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static List<SharpDX.Vector2> GetFillEllipseArray(float x, float y, float a, float b)
        {
            a = a / 2;
            b = b / 2;
            List<SharpDX.Vector2> vecList = new List<SharpDX.Vector2>();
            for (float i = -a; i <= a; i++)
            {
                vecList.Add(new SharpDX.Vector2(a + x + i, b + y + (float)(b * Math.Cos(Math.Asin(i / a)))));
                vecList.Add(new SharpDX.Vector2(a + x + i, b + y - (float)(b * Math.Cos(Math.Asin(i / a)))));
                i++;
                vecList.Add(new SharpDX.Vector2(a + x + i, b + y + (float)(b * Math.Cos(Math.Asin(i / a)))));
                vecList.Add(new SharpDX.Vector2(a + x + i, b + y - (float)(b * Math.Cos(Math.Asin(i / a)))));
            }
            return vecList;
        }

        /// <summary>
        /// Get the vector2 array that if draw line on that collection drawed an ellipse.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static List<SharpDX.Vector2> GetFillEllipseArray(SharpDX.Vector2 position, float a, float b)
        {
            return GetFillEllipseArray(position.X, position.Y, a, b);
        }

        /// <summary>
        /// Get the vector2 array that if draw line on that collection drawed an ellipse.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static List<SharpDX.Vector2> GetFillEllipseArray(SharpDX.Rectangle rectangle)
        {
            return GetFillEllipseArray(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

    }

    public class ShapesDrawer
    {
        /// <summary>
        /// Get the vector2 array that if draw line on that collection drawed a Star.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<SharpDX.Vector2> GetDrawStarArray(float x, float y, float r)
        {
            if (r < 5) r = 5;
            List<SharpDX.Vector2> vecList = new List<SharpDX.Vector2>();
            for (float i = 0; i <= 5; i++)
            {
                vecList.Add(new SharpDX.Vector2((float)(x + r * Math.Sin(Math.PI + i * 4 * Math.PI / 5)),
                    (float)(y + r * Math.Cos(Math.PI + i * 4 * Math.PI / 5))));
            }
            return vecList;
        }

        /// <summary>
        /// Get the vector2 array that if draw line on that collection drawed a X.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<SharpDX.Vector2> GetDrawXArray(float x, float y, float r)
        {
            if (r < 5) r = 5;
            List<SharpDX.Vector2> vecList = new List<SharpDX.Vector2>();

            vecList.Add(new SharpDX.Vector2(x - r / 2, y - r / 2));
            vecList.Add(new SharpDX.Vector2(x + r / 2, y + r / 2));
            vecList.Add(new SharpDX.Vector2(x, y));
            vecList.Add(new SharpDX.Vector2(x - r / 2, y + r / 2));
            vecList.Add(new SharpDX.Vector2(x + r / 2, y - r / 2));

            return vecList;
        }

        /// <summary>
        /// Get the vector2 array that if draw line on that collection drawed a Diamond.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<SharpDX.Vector2> GetDrawDiamondArray(float x, float y, float r)
        {
            if (r < 5) r = 5;
            List<SharpDX.Vector2> vecList = new List<SharpDX.Vector2>();

            vecList.Add(new SharpDX.Vector2(x , y - r / 2));
            vecList.Add(new SharpDX.Vector2(x + r / 2, y));
            vecList.Add(new SharpDX.Vector2(x, y + r / 2));
            vecList.Add(new SharpDX.Vector2(x - r / 2, y ));
            vecList.Add(new SharpDX.Vector2(x, y - r / 2));

            return vecList;
        }

    }
    public class Convertor
    {
        /// <summary>
        /// Convert System.Drawing.Color to SharpDX.Color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static SharpDX.Color ColorConvertor(System.Drawing.Color color, float opacity = 1)
        {
            SharpDX.Color tmpColor = SharpDX.Color.FromBgra(color.ToArgb());
            if(opacity!=1)
            tmpColor.A = (byte)(opacity * 255);
            return tmpColor;
        }

        /// <summary>
        /// Convert DataPointCollection to the vector2 array.
        /// </summary>
        /// <param name="dPC"></param>
        /// <param name="HeightRate"></param>
        /// <param name="WidthRate"></param>
        /// <param name="minX"></param>
        /// <returns></returns>
        public static SharpDX.Vector2[] convertDataPointToVector2Array(Charting.Base.DataPointCollection dPC, float HeightRate, float WidthRate, out float minX)
        {
            List<SharpDX.Vector2> tmplist = new List<SharpDX.Vector2>();
            minX = 0;
            float x, y; SharpDX.Vector2 tmpVecto2 = new SharpDX.Vector2();
            for (int indexOfDataPoint = 0; indexOfDataPoint < dPC.Count; indexOfDataPoint++)
            {
                x = (float)(dPC[indexOfDataPoint].XValue * WidthRate);
                y = -(float)(dPC[indexOfDataPoint].YValues[0]) * HeightRate;
                if ((float)(dPC[indexOfDataPoint].XValue * WidthRate) < minX)
                    minX = (float)(dPC[indexOfDataPoint].XValue * WidthRate);

                if (tmplist.Count > 0)
                {
                    if (GetDistance(x, y, tmplist[tmplist.Count - 1].X, tmplist[tmplist.Count - 1].Y) >2)
                    {
                        tmpVecto2.X = x;
                        tmpVecto2.Y = y;
                    }
                }
                else
                {
                    tmpVecto2.X = x;
                    tmpVecto2.Y = y;
                }
                tmplist.Add(tmpVecto2);
            }
            return tmplist.ToArray();
        }

        public static System.Drawing.PointF[] convertDataPointToPointFArray(Charting.Base.DataPointCollection dPC, float HeightRate, float WidthRate, out float minX)
        {
            List<System.Drawing.PointF> tmplist = new List<System.Drawing.PointF>();
            minX = 0;
            float x, y;
            System.Drawing.PointF tmpVecto2 = new System.Drawing.PointF();
            for (int indexOfDataPoint = 0; indexOfDataPoint < dPC.Count; indexOfDataPoint++)
            {
                x = (float)(dPC[indexOfDataPoint].XValue * WidthRate);
                y = -(float)(dPC[indexOfDataPoint].YValues[0]) * HeightRate;
                if ((float)(dPC[indexOfDataPoint].XValue * WidthRate) < minX)
                    minX = (float)(dPC[indexOfDataPoint].XValue * WidthRate);

                if (tmplist.Count > 0)
                {
                    if (GetDistance(x, y, tmplist[tmplist.Count - 1].X, tmplist[tmplist.Count - 1].Y) > 1)
                    {
                        tmpVecto2.X = x;
                        tmpVecto2.Y = y;
                    }
                }
                else
                {
                    tmpVecto2.X = x;
                    tmpVecto2.Y = y;
                }
                tmplist.Add(tmpVecto2);
            }
            return tmplist.ToArray();
        }

       

        /// <summary>
        /// Convert PointF array to Vector2 array.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static  SharpDX.Vector2[] ToVector2(System.Drawing.PointF[] points)
        {
            SharpDX.Vector2[] vec = new SharpDX.Vector2[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                vec[i].X = points[i].X;
                vec[i].Y = points[i].Y;
            }
            return vec;
        }

        /// <summary>
        /// Get the Distance between two point.
        /// </summary>
        /// <param name="x1">First point X</param>
        /// <param name="y1">First point Y</param>
        /// <param name="x2">Second point X</param>
        /// <param name="y2">Second point Y</param>
        /// <returns></returns>
        static float GetDistance(float x1, float y1, float x2, float y2)
        {
            return (float)(System.Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) / (y1 - y2)));
        }
   
    }
}
