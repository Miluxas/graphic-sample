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

namespace xPFT.GraphicEngineD3D9
{
    public class Line : ILine
    {
        #region Constructor
        public Line(IDevice device, System.Drawing.Font font)
        {
            this.device =(Device) device;

            if (line != null)
            {
                line.Dispose();
                line = null;
            }
            line = new SharpDX.Direct3D9.Line(this.device.device);
            if (pointsLine != null)
            {
                pointsLine.Dispose();
                pointsLine = null;
            }
            pointsLine = new SharpDX.Direct3D9.Line(this.device.device);

            pointsLine.Width = 1;
            line.Width = Width;
            line.PatternScale = 1;

            pointsLine.Antialias = line.Antialias = Antialias;
            if (textWrt != null)
            {
                textWrt.Dispose();
                textWrt = null;
            }
            textWrt = new TextWriter(device, font);
        }
        #endregion

        #region Fields
        private SharpDX.Direct3D9.Line line,pointsLine;
        private Device device;
        private TextWriter textWrt;
        #endregion

        #region Properties
        public bool IsDisposed
        {
            get { return line.IsDisposed; }
        }
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
                line.Width = width * 1.5f;

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


        public Charting.Base.LineDrawPattern Pattern
        {
            set
            {
                line.Pattern = Charting.Base.GetLineDrawPattern.ForD3D9(value);
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
            try
            {
                line.Begin();
                line.Draw(DrawingBase.Convertor.ToVector2(points), Convertor.ColorConvertor(color, opacity));
                line.End();
            }
            catch (Exception e)
            {
                throw e;
            }
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
            if (device.device != null)
            {
                line.Begin();
                device.device.SetTransform(0, SharpDX.Matrix.Translation(wScale, hScale, 0));
                line.Draw(DrawingBase.Convertor.ToVector2(points), SharpDX.Color.FromBgra(color.ToArgb()));
                line.End();
            }
            return new System.Drawing.Point();
        }

        /// <summary>
        /// Dispose the Line.
        /// </summary>
        public void Dispose()
        {
            if (line != null)
            {
               
                line.Dispose();
            }
            if (pointsLine != null)
                pointsLine.Dispose();
            if (textWrt != null)
                textWrt.Dispose();

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
        public System.Drawing.Point Draw(int layerIndex, Charting.Base.DataPointCollection points, float xAxesCenterHeight, float yAxesCenterWidth, float HeightRate, float WidthRate, float xAxesMaxValue, System.Drawing.Color color, float opacity = 1, bool autoShift = true, bool keepInRightOfYAxis = false)
        {
           
            SharpDX.Vector2[] tmpPointArray;
            float MinX;
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
                line.Begin();
                device.device.SetTransform(0, SharpDX.Matrix.Translation(tmpYAxesCenterWidth, xAxesCenterHeight, 0));
                line.Draw(tmpPointArray, Convertor.ColorConvertor(color, opacity));
                line.End();

            }
            return new System.Drawing.Point((int)tmpYAxesCenterWidth,(int) xAxesCenterHeight);
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
            SharpDX.Vector2[] tmpPointArray;
            float MinX;
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
                if (tmpPointArray[0].X + tmpYAxesCenterWidth < device.padding.Left)
                    if (tmpPointArray.Length > 1)
                        tmpPointArray[0] = tmpPointArray[1];
                if (pointType == 1) 
                    pointsLine.Width = Width;
                else
                    pointsLine.Width = 1;
                pointsLine.Begin();
                device.device.SetTransform(0, SharpDX.Matrix.Translation(tmpYAxesCenterWidth, xAxesCenterHeight, 0));
                for (int index = 0; index < tmpPointArray.Length; index++)
                {
                    if (pointType == 0)
                        pointsLine.Draw(ShapesFiller.GetFillEllipseArray(tmpPointArray[index].X - Width / 2, tmpPointArray[index].Y - Width / 2, Width, Width).ToArray(), Convertor.ColorConvertor(color, opacity));
                    if (pointType == 1)
                        pointsLine.Draw(new SharpDX.Vector2[]{new SharpDX.Vector2(tmpPointArray[index].X-Width/2,tmpPointArray[index].Y),
                        new SharpDX.Vector2(tmpPointArray[index].X+Width/2,tmpPointArray[index].Y)}, Convertor.ColorConvertor(color, opacity / 2));
                    if (pointType == 2)
                        pointsLine.Draw(ShapesDrawer.GetDrawStarArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray(), Convertor.ColorConvertor(color, opacity));
                    if (pointType == 3)
                        pointsLine.Draw(ShapesDrawer.GetDrawXArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray(), Convertor.ColorConvertor(color, opacity));
                    if (pointType == 4)
                        pointsLine.Draw(ShapesDrawer.GetDrawDiamondArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray(), Convertor.ColorConvertor(color, opacity));

                }
                pointsLine.End();
            }
            return new System.Drawing.Point((int)tmpYAxesCenterWidth, (int)xAxesCenterHeight);
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
            SharpDX.Vector2[] tmpPointArray = DrawingBase.Convertor.ToVector2(points);
            if (pointType == 1)
                pointsLine.Width = Width;
            pointsLine.Begin();
            for (int index = 0; index < tmpPointArray.Length; index++)
            {
                if (pointType == 0)
                    pointsLine.Draw(ShapesFiller.GetFillEllipseArray(tmpPointArray[index].X - Width / 2, tmpPointArray[index].Y - Width / 2, Width, Width).ToArray(), Convertor.ColorConvertor(color, opacity));
                if (pointType == 1)
                    pointsLine.Draw(new SharpDX.Vector2[]{new SharpDX.Vector2(tmpPointArray[index].X-Width/2,tmpPointArray[index].Y),
                        new SharpDX.Vector2(tmpPointArray[index].X+Width/2,tmpPointArray[index].Y)}, Convertor.ColorConvertor(color, opacity / 2));
                if (pointType == 2)
                    pointsLine.Draw(ShapesDrawer.GetDrawStarArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray(), Convertor.ColorConvertor(color, opacity));
                if (pointType == 3)
                    pointsLine.Draw(ShapesDrawer.GetDrawXArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray(), Convertor.ColorConvertor(color, opacity));
                if (pointType == 4)
                    pointsLine.Draw(ShapesDrawer.GetDrawDiamondArray(tmpPointArray[index].X, tmpPointArray[index].Y, 5).ToArray(), Convertor.ColorConvertor(color, opacity));

            }
            pointsLine.End();
            return new System.Drawing.Point();
        }
        #endregion
    }
}
