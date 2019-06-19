﻿/*********************************************************************
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
using SharpDX.Direct2D1;
using SharpDX;
using xPFT.DrawingBase;

namespace xPFT.GraphicEngineD2D1
{
    public class Line:ILine
    {
        #region Constructor
        public Line(IDrawing.IDevice device,System.Drawing.Font font)
        {
            this.device =(Device) device;
            ssp=new StrokeStyleProperties();
            ssp.DashStyle=DashStyle.Custom;
            ssp.LineJoin = LineJoin.Round;
            strokeStyle = new StrokeStyle(this.device.factory,ssp,new float[]{1,0});
            textWrt = new TextWriter(device, font);

        }
        #endregion

        #region Fields
        
        private Device device;
        private Brush brush;
        StrokeStyle strokeStyle;
        StrokeStyleProperties ssp;
        float opacity = 1;
        private System.Drawing.Color curentColor;
        private TextWriter textWrt;
       
        #endregion

        #region Properties

        float width = 1;
        /// <summary>
        /// Get or Set the line width.
        /// </summary>
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                 width = value;
            }
        }

        bool antialise = true;
        /// <summary>
        /// Get or Set line antialias.
        /// </summary>
        public bool Antialias
        {
            get
            {
                return antialise;
            }
            set
            {
                antialise = value;
            }
        }

        public bool IsDisposed
        {
            get { return true; }
        }

        Charting.Base.LineDrawPattern pattern = Charting.Base.LineDrawPattern.SOLID;
        /// <summary>
        /// Set the line pattern.
        /// </summary>
        public Charting.Base.LineDrawPattern Pattern
        {
            set
            {
                pattern = value;
                strokeStyle = new StrokeStyle(this.device.factory, ssp, Charting.Base.GetLineDrawPattern.ForD2D1(value));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draw the Line.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="color"></param>
        public System.Drawing.Point Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color color, float opacity = 1)
        {
            this.opacity = opacity;
            try
            {
                if (curentColor != color)
                    SetLineColor(color,opacity);
                Vector2[] tmpPointList = DrawingBase.Convertor.ToVector2(points);
                if (tmpPointList[0].X == tmpPointList[1].X || tmpPointList[0].Y == tmpPointList[1].Y)
                    device.layers[layerIndex].AntialiasMode = AntialiasMode.Aliased;
                if (pattern != Charting.Base.LineDrawPattern.SOLID)
                    device.layers[layerIndex].DrawLine(tmpPointList[0],tmpPointList[1],brush,Width,strokeStyle);
                else
                    device.layers[layerIndex].DrawLine(tmpPointList[0], tmpPointList[1], brush, Width);
            }
            catch { }
            return new System.Drawing.Point();
        }

        /// <summary>
        /// Draw line with the width and height scale.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="color"></param>
        /// <param name="wScale"></param>
        /// <param name="hScale"></param>
        public System.Drawing.Point Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color color, float wScale, float hScale, float opacity = 1)
        {
            this.opacity = opacity;
            if (curentColor != color)
                SetLineColor(color,opacity);
            if (device.layers[layerIndex] != null)
            {
                Vector2[] tmpPointList = DrawingBase.Convertor.ToVector2(points);
                if (tmpPointList[0].X == tmpPointList[1].X || tmpPointList[0].Y == tmpPointList[1].Y)
                    device.layers[layerIndex].AntialiasMode = AntialiasMode.Aliased;
                device.layers[layerIndex].Transform = Matrix.Translation(wScale, hScale, 0);
                if (pattern != Charting.Base.LineDrawPattern.SOLID)
                    device.layers[layerIndex].DrawLine(tmpPointList[0], tmpPointList[1], brush, Width, strokeStyle);
                else
                    device.layers[layerIndex].DrawLine(tmpPointList[0], tmpPointList[1], brush, Width);
            }
            return new System.Drawing.Point();
        }

        /// <summary>
        /// Dispose the Line.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Draw line with the data point collection.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="xAxesCenterHeight"></param>
        /// <param name="yAxesCenterWidth"></param>
        /// <param name="HeightRate"></param>
        /// <param name="WidthRate"></param>
        /// <param name="xAxesMaxValue"></param>
        /// <param name="color"></param>
        /// <param name="autoShift"></param>
        /// <param name="keepInRightOfYAxis"></param>
        public System.Drawing.Point Draw(int layerIndex, Charting.Base.DataPointCollection points, float xAxesCenterHeight, float yAxesCenterWidth, float HeightRate, float WidthRate, float xAxesMaxValue, System.Drawing.Color color, float opacity, bool autoShift = true, bool keepInRightOfYAxis = false)
        {
            this.opacity = opacity;
            device.layers[layerIndex].AntialiasMode = AntialiasMode.PerPrimitive;
            if (curentColor != color)
                SetLineColor(color,opacity);
            SharpDX.Vector2[] tmpPointArray;
            float MinX=0;
            float tmpYAxesCenterWidth = yAxesCenterWidth;
            tmpPointArray = DrawingBase.Convertor.convertDataPointToVector2Array(points, HeightRate, WidthRate, out MinX);
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

                var linePath = new PathGeometry(device.factory);
                using (var sink = linePath.Open())
                {
                    sink.BeginFigure(tmpPointArray[0], FigureBegin.Hollow);
                    sink.AddLines(tmpPointArray);
                    sink.EndFigure(FigureEnd.Open);
                    sink.Close();
                }

                if (device.layers[layerIndex] != null)
                {
                    device.layers[layerIndex].Transform = Matrix.Translation(tmpYAxesCenterWidth, xAxesCenterHeight, 0);
                    if (pattern != Charting.Base.LineDrawPattern.SOLID)
                        device.layers[layerIndex].DrawGeometry(linePath, brush, Width, strokeStyle);
                    else
                        device.layers[layerIndex].DrawGeometry(linePath, brush, Width);
                    device.layers[layerIndex].Transform = Matrix.Identity;
                }
                linePath.Dispose();
            }
            return new System.Drawing.Point((int)tmpYAxesCenterWidth, (int)xAxesCenterHeight);
        }

        /// <summary>
        /// Draw a point on the each point of the collections.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="xAxesCenterHeight"></param>
        /// <param name="yAxesCenterWidth"></param>
        /// <param name="HeightRate"></param>
        /// <param name="WidthRate"></param>
        /// <param name="xAxesMaxValue"></param>
        /// <param name="color"></param>
        /// <param name="pointType"></param>
        /// <param name="opacity"></param>
        /// <param name="autoShift"></param>
        /// <param name="keepInRightOfYAxis"></param>
        public System.Drawing.Point DrawPoints(int layerIndex, Charting.Base.DataPointCollection points, float xAxesCenterHeight, float yAxesCenterWidth, float HeightRate, float WidthRate, float xAxesMaxValue, System.Drawing.Color color, int pointType = 0, float opacity = 1, bool autoShift = true, bool keepInRightOfYAxis = false, bool isDrawPointLabel = false)
        {
            this.opacity = opacity;
            if (curentColor != color)
                SetLineColor(color, opacity);
            SharpDX.Vector2[] tmpPointArray;
            float MinX;
            float tmpYAxesCenterWidth = yAxesCenterWidth;
            tmpPointArray =DrawingBase.Convertor.convertDataPointToVector2Array(points, HeightRate, WidthRate, out MinX);
            if (tmpPointArray.Length > 0)
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

                if (device.layers[layerIndex] != null)
                {
                    device.layers[layerIndex].Transform = Matrix.Translation(tmpYAxesCenterWidth, xAxesCenterHeight, 0);
                    for (int index = 0; index < tmpPointArray.Length; index++)
                    {
                        if (pointType == 0)
                            device.layers[layerIndex].FillEllipse(new SharpDX.Direct2D1.Ellipse(tmpPointArray[index], Width / 2, Width / 2), brush);
                        else if (pointType == 1)
                            device.layers[layerIndex].FillRectangle(new RectangleF(tmpPointArray[index].X - Width / 2, tmpPointArray[index].Y - Width / 2, Width, Width), brush);
                        else if (pointType == 2)
                        {
                            List<SharpDX.Vector2> startPoints = ShapesDrawer.GetDrawStarArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5);
                            for (int i = 1; i < startPoints.Count; i++)
                                device.layers[layerIndex].DrawLine(startPoints[i], startPoints[i - 1], brush);
                        }
                        else if (pointType == 3)
                        {
                            float wid = 5;
                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointArray[index].X - wid / 2, tmpPointArray[index].Y - wid / 2),
                                new SharpDX.Vector2(tmpPointArray[index].X + wid / 2, tmpPointArray[index].Y + wid / 2), brush);
                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointArray[index].X - wid / 2, tmpPointArray[index].Y + wid / 2),
                                new SharpDX.Vector2(tmpPointArray[index].X + wid / 2, tmpPointArray[index].Y - wid / 2), brush);
                        }
                        else if (pointType == 4)
                        {
                            float wid = 5;
                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointArray[index].X, tmpPointArray[index].Y - wid / 2),
                                new SharpDX.Vector2(tmpPointArray[index].X + wid / 2, tmpPointArray[index].Y ), brush);

                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointArray[index].X + wid / 2, tmpPointArray[index].Y),
                                new SharpDX.Vector2(tmpPointArray[index].X , tmpPointArray[index].Y + wid / 2), brush);

                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointArray[index].X, tmpPointArray[index].Y + wid / 2),
                                new SharpDX.Vector2(tmpPointArray[index].X- wid / 2, tmpPointArray[index].Y ), brush);

                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointArray[index].X - wid / 2, tmpPointArray[index].Y),
                                new SharpDX.Vector2(tmpPointArray[index].X, tmpPointArray[index].Y - wid / 2), brush);
                        }

                        if (isDrawPointLabel)
                        {
                            var str=points[index].YValues[0].ToString();
                            if (points[index].Tag != null)
                            {
                                str = points[index].Tag.ToString();
                            }
                            Matrix3x2 tempMat = device.layers[layerIndex].Transform;
                            textWrt.Draw(layerIndex,str ,
                                new System.Drawing.Rectangle((int)(tmpPointArray[index].X + tmpYAxesCenterWidth)-20, (int)(tmpPointArray[index].Y + xAxesCenterHeight)-15, 40, 40), Charting.Base.FontDrawFlags.Center, color, 1, 0);
                            device.layers[layerIndex].Transform = tempMat;
                        }
                    }
                    device.layers[layerIndex].Transform = Matrix.Identity;
                }
            }
            return new System.Drawing.Point();
        }

        /// <summary>
        /// Draw a point on the each point of the collections. 
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        /// <param name="pointType"></param>
        public System.Drawing.Point DrawPoints(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color color, float opacity = 1, int pointType = 0)
        {
            this.opacity = opacity;
            if (curentColor != color)
                SetLineColor(color, opacity);
            Vector2[] tmpPointList = DrawingBase.Convertor.ToVector2(points);
            {
                if (device.layers[layerIndex] != null)
                {
                    for (int index = 0; index < tmpPointList.Length; index++)
                    {
                        if (pointType == 0)
                            device.layers[layerIndex].FillEllipse(new SharpDX.Direct2D1.Ellipse(tmpPointList[index], Width / 2, Width / 2), brush);
                        else if (pointType == 1)
                            device.layers[layerIndex].FillRectangle(new RectangleF(tmpPointList[index].X - Width / 2, tmpPointList[index].Y - Width / 2, Width, Width), brush);
                        else if (pointType == 2)
                        {
                            List<SharpDX.Vector2> startPoints = ShapesDrawer.GetDrawStarArray(tmpPointList[index].X, tmpPointList[index].Y, 5);
                            for (int i = 1; i < startPoints.Count; i++)
                                device.layers[layerIndex].DrawLine(startPoints[i], startPoints[i - 1], brush);
                        }
                        else if (pointType == 3)
                        {
                            float wid = 5;
                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointList[index].X - wid / 2, tmpPointList[index].Y - wid / 2),
                                new SharpDX.Vector2(tmpPointList[index].X + wid / 2, tmpPointList[index].Y + wid / 2), brush);
                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointList[index].X - wid / 2, tmpPointList[index].Y + wid / 2),
                                new SharpDX.Vector2(tmpPointList[index].X + wid / 2, tmpPointList[index].Y - wid / 2), brush);
                        }
                        else if (pointType == 4)
                        {
                            float wid = 5;
                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointList[index].X, tmpPointList[index].Y - wid / 2),
                                new SharpDX.Vector2(tmpPointList[index].X + wid / 2, tmpPointList[index].Y), brush);

                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointList[index].X + wid / 2, tmpPointList[index].Y),
                                new SharpDX.Vector2(tmpPointList[index].X, tmpPointList[index].Y + wid / 2), brush);

                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointList[index].X, tmpPointList[index].Y + wid / 2),
                                new SharpDX.Vector2(tmpPointList[index].X - wid / 2, tmpPointList[index].Y), brush);

                            device.layers[layerIndex].DrawLine(new SharpDX.Vector2(tmpPointList[index].X - wid / 2, tmpPointList[index].Y),
                                new SharpDX.Vector2(tmpPointList[index].X, tmpPointList[index].Y - wid / 2), brush);
                        }
                    }
                    device.layers[layerIndex].Transform = Matrix.Identity;
                }
            }
            return new System.Drawing.Point();
        }

        /// <summary>
        /// Set the color line.
        /// </summary>
        /// <param name="color"></param>
        void SetLineColor(System.Drawing.Color color,float opacity)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(device.renderTarget,xPFT.DrawingBase.Convertor.ColorConvertor(color,opacity));
            curentColor = color;
        }

        #endregion

    }
}
