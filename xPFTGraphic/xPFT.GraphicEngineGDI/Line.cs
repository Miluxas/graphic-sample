/*********************************************************************
 *  Line.cs
 *  Implementation of the Class Line
 *  Created on:      15-Nov-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.IDrawing;
using xPFT.DrawingBase;
using System.Drawing;

namespace xPFT.GraphicEngineGDI
{
    public class Line 
    {
       #region Methods
        /// <summary>
        /// Draw the Line.
        /// </summary>
        public static void Draw(Graphics g,Point pos, System.Drawing.PointF[] points, System.Drawing.Color color, float opacity = 1, float width = 1, Charting.Base.LineDrawPattern linePattern = Charting.Base.LineDrawPattern.SOLID)
        {
            try
            {
                Pen pen = new Pen(BrushMaker.SetOpacity(color,opacity), width);
                if (linePattern != Charting.Base.LineDrawPattern.SOLID)
                {
                    pen.DashPattern = Charting.Base.GetLineDrawPattern.ForD2D1(linePattern);
                    pen.DashOffset = width;
                }
                g.TranslateTransform(pos.X, pos.Y);
                g.DrawLines(pen, points);
                g.ResetTransform();
            }
            catch { }
        }

        /// <summary>
        /// Draw line with the width and height scale.
        /// </summary>
        public static void Draw(Graphics g,Point pos, System.Drawing.PointF[] points, System.Drawing.Color color, float wScale, float hScale, float opacity = 1, float width = 1, Charting.Base.LineDrawPattern linePattern = Charting.Base.LineDrawPattern.SOLID)
        {
            if (g != null)
            {
                Pen pen = new Pen(BrushMaker.SetOpacity(color,opacity), width);
                if (linePattern != Charting.Base.LineDrawPattern.SOLID)
                    pen.DashPattern = Charting.Base.GetLineDrawPattern.ForD2D1(linePattern);
                g.TranslateTransform(pos.X, pos.Y);
                g.ScaleTransform(wScale, hScale);
                g.DrawLines(pen, points);
            }
        }

        /// <summary>
        /// Draw line with the data point collection.
        /// </summary>
        public static void Draw(Graphics g,Point pos, Charting.Base.DataPointCollection points, float xAxesCenterHeight, float yAxesCenterWidth, float HeightRate, float WidthRate, float xAxesMaxValue, System.Drawing.Color color, float opacity = 1, bool autoShift = true, bool keepInRightOfYAxis = false,float width=1,Charting.Base.LineDrawPattern linePattern=Charting.Base.LineDrawPattern.SOLID)
        {
            PointF[] tmpPointArray;
            float MinX;
            float tmpYAxesCenterWidth = yAxesCenterWidth;
            tmpPointArray = DrawingBase.Convertor.convertDataPointToPointFArray(points, HeightRate, WidthRate, out MinX);
            if (tmpPointArray.Length > 1)
            {
                if (autoShift)
                {
                    if (points.GetLast().XValue > xAxesMaxValue)
                        tmpYAxesCenterWidth = -((float)points.GetLast().XValue - xAxesMaxValue) * WidthRate + yAxesCenterWidth;
                    else
                        tmpYAxesCenterWidth = -tmpPointArray[0].X + yAxesCenterWidth;
                }
                if (keepInRightOfYAxis)
                {
                    if (MinX < 0)
                        tmpYAxesCenterWidth = tmpYAxesCenterWidth - MinX;
                }
                var oldTrans = g.Transform;
                g.TranslateTransform(tmpYAxesCenterWidth+pos.X, xAxesCenterHeight+pos.Y);
                Pen pen = new Pen(BrushMaker.SetOpacity(color,opacity), width);
                if (linePattern != Charting.Base.LineDrawPattern.SOLID)
                    pen.DashPattern = Charting.Base.GetLineDrawPattern.ForD2D1(linePattern);
                g.DrawLines(pen, tmpPointArray);
                //g.TranslateTransform(-tmpYAxesCenterWidth, -xAxesCenterHeight);
                g.Transform = oldTrans;
            }
        }

        /// <summary>
        /// Draw a point on the each point of the collections.
        /// </summary>
        public static void DrawPoints(Graphics g, Point pos, Charting.Base.DataPointCollection points, float xAxesCenterHeight, float yAxesCenterWidth, float HeightRate, float WidthRate, float xAxesMaxValue, System.Drawing.Color color, int pointType = 0, float opacity = 1, bool autoShift = true, bool keepInRightOfYAxis = false, float Width = 1, bool isDrawPointLabel = false,Font font=null)
        {
            if (font == null)
                font = new Font("arial", 8);
            PointF[] tmpPointArray;
            float MinX;
            float tmpYAxesCenterWidth = yAxesCenterWidth;
            tmpPointArray = DrawingBase.Convertor.convertDataPointToPointFArray(points, HeightRate, WidthRate, out MinX);
            if (tmpPointArray.Length > 1)
            {
                if (autoShift)
                {
                    if (points.GetLast().XValue > xAxesMaxValue)
                        tmpYAxesCenterWidth = -((float)points.GetLast().XValue - xAxesMaxValue) * WidthRate + yAxesCenterWidth;
                    else
                        tmpYAxesCenterWidth = -tmpPointArray[0].X + yAxesCenterWidth;
                }
                if (keepInRightOfYAxis)
                {
                    if (MinX < 0)
                        tmpYAxesCenterWidth = tmpYAxesCenterWidth - MinX;
                }

                g.TranslateTransform(tmpYAxesCenterWidth + pos.X, xAxesCenterHeight + pos.Y);
                for (int index = 0; index < tmpPointArray.Length; index++)
                {
                    if (pointType == 0)
                        g.FillEllipse(new SolidBrush(BrushMaker.SetOpacity(color, opacity)), tmpPointArray[index].X - Width / 2, tmpPointArray[index].Y - Width / 2, Width, Width);
                    if (pointType == 1)
                        g.FillRectangle(new SolidBrush(BrushMaker.SetOpacity(color, opacity)), tmpPointArray[index].X - Width / 2, tmpPointArray[index].Y - Width / 2, Width, Width);
                    if (pointType == 2)
                        g.DrawLines(new Pen(BrushMaker.SetOpacity(color, opacity)), GetDrawStarArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray());
                    if (pointType == 3)
                        g.DrawLines(new Pen(BrushMaker.SetOpacity(color, opacity)), GetDrawXArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray());
                    if (pointType == 4)
                        g.DrawPolygon(new Pen(BrushMaker.SetOpacity(color, opacity), 1), GetDrawDiamondArrayPointF(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray());

                 //   g.ResetTransform();
                    if (isDrawPointLabel)
                    {
                        var str = tmpPointArray[index].Y.ToString();
                        if (points[index].Tag != null)
                        {
                            str = points[index].Tag.ToString();
                        }
                        System.Drawing.Drawing2D.Matrix tempMat = g.Transform;
                       // g.TranslateTransform(tmpYAxesCenterWidth , xAxesCenterHeight );
                        TextWriter.Draw(g, new Point(), str, new System.Drawing.Rectangle((int)(tmpPointArray[index].X ) - 20, (int)(tmpPointArray[index].Y ) - 15, 40, 40), Charting.Base.FontDrawFlags.Center, color, 1, 0, font);
                        g.Transform=tempMat;
                    }
                }
            }
        }

        /// <summary>
        /// Draw a point on the each point of the collections.
        /// </summary>
        public static void DrawPoints(Graphics g, Point pos, System.Drawing.PointF[] points, System.Drawing.Color color, float opacity = 1, int pointType = 0, float lineWidth = 1)
        {
            g.TranslateTransform( pos.X,  pos.Y);
            for (int index = 0; index < points.Length; index++)
            {

                if (pointType == 0)
                    g.FillEllipse(new SolidBrush(BrushMaker.SetOpacity(color, opacity)), points[index].X - lineWidth / 2, points[index].Y - lineWidth / 2, lineWidth, lineWidth);
                if (pointType == 1)
                    g.FillRectangle(new SolidBrush(BrushMaker.SetOpacity(color, opacity)), points[index].X - lineWidth / 2, points[index].Y - lineWidth / 2, lineWidth, lineWidth);
                if (pointType == 2)
                    g.DrawLines(new Pen(BrushMaker.SetOpacity(color, opacity)), GetDrawStarArray(points[index].X, points[index].Y, 5).ToArray());
                if (pointType == 3)
                    g.DrawLines(new Pen(BrushMaker.SetOpacity(color, opacity)), GetDrawXArray(points[index].X, points[index].Y, 5).ToArray());
                if (pointType == 4)
                    g.DrawPolygon(new Pen(BrushMaker.SetOpacity(color, opacity), 1), GetDrawDiamondArrayPointF(points[index].X, points[index].Y, 5).ToArray());
            }
            g.ResetTransform();
        }

        /// <summary>
        /// Get list of points that drawing them create a star shape.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<PointF> GetDrawStarArray(float x, float y, float r)
        {
            if (r < 5) r = 5;
            List<PointF> vecList = new List<PointF>();
            for (float i = 0; i <= 5; i++)
            {
                vecList.Add(new PointF((float)(x + r * Math.Sin(Math.PI + i * 4 * Math.PI / 5)),
                    (float)(y + r * Math.Cos(Math.PI + i * 4 * Math.PI / 5))));
            }
            return vecList;
        }

        /// <summary>
        /// Get list of points that drawing them create a X shape.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<PointF> GetDrawXArray(float x, float y, float r)
        {
            if (r < 5) r = 5;
            List<PointF> vecList = new List<PointF>();

            vecList.Add(new PointF(x - r / 2, y - r / 2));
            vecList.Add(new PointF(x + r / 2, y + r / 2));
            vecList.Add(new PointF(x, y));
            vecList.Add(new PointF(x - r / 2, y + r / 2));
            vecList.Add(new PointF(x + r / 2, y - r / 2));

            return vecList;
        }

        /// <summary>
        /// Get list of points that drawing them create a diamond shape.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<PointF> GetDrawDiamondArrayPointF(float x, float y, float r)
        {
            if (r < 5) r = 5;
            List<PointF> vecList = new List<PointF>();

            vecList.Add(new PointF(x, y - r / 2));
            vecList.Add(new PointF(x + r / 2, y));
            vecList.Add(new PointF(x, y + r / 2));
            vecList.Add(new PointF(x - r / 2, y));
            vecList.Add(new PointF(x, y - r / 2));

            return vecList;
        }
        #endregion
    }
}
