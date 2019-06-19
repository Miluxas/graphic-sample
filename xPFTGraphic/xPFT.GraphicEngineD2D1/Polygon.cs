/*********************************************************************
 *  Ellipse.cs
 *  Implementation of the Class Ellipse
 *  Created on:      03-Dec-2014 17:10:56 
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
    public class Polygon :IDrawing.IPolygon
    {
        #region Constructor
        public Polygon(IDrawing.IDevice device)
        {
            this.device = (Device)device;
        }
        #endregion

        #region Fields
        private Device device;
        private BitmapBrush bitmapBrush;
        System.Drawing.Bitmap prevBitmap;
        System.Drawing.Color prevFillColor;
        float prevOpacity;
        xPFT.Charting.Base.FillPattern prevFillPattern;
        Brush fillBrush;
        RenderTarget prevRenderTarget;

        #endregion

        #region Properties
        float lineWidth = 2;
        public float LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Draw polygon on layer with layerIndex Index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="borderColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="opacity"></param>
        /// <param name="fillPattern"></param>
        /// <param name="patternSize"></param>
        public void Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize)
        {
            if (layerIndex < device.layers.Count)
            {
                var tmpGeo = new PathGeometry(device.factory);
                GeometrySink sink1 = tmpGeo.Open();
                Vector2[] tmpPointList = DrawingBase.Convertor.ToVector2(points);
                sink1.BeginFigure(tmpPointList[0], new FigureBegin());
                sink1.AddLines(tmpPointList);
                sink1.EndFigure(new FigureEnd());
                sink1.Close();

                if (fillColor != null)
                {
                    if (device.renderTarget != prevRenderTarget || fillColor != prevFillColor || fillPattern != prevFillPattern || opacity != prevOpacity)
                    {
                        fillBrush = BrushMaker.GetPatternBrush(device.renderTarget, (System.Drawing.Color)fillColor, opacity, fillPattern, patternSize);
                        prevFillColor = (System.Drawing.Color)fillColor;
                        prevFillPattern = fillPattern;
                        prevOpacity = opacity;
                        prevRenderTarget = device.renderTarget;
                    }
                    device.layers[layerIndex].FillGeometry(tmpGeo, fillBrush);

                }
                if (borderColor != null)
                    device.layers[layerIndex].DrawGeometry(tmpGeo, new SharpDX.Direct2D1.SolidColorBrush(device.renderTarget, xPFT.DrawingBase.Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity)), lineWidth);
            }
        }

        /// <summary>
        /// Draw polygon on layer with layerIndex Index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="bitmap"></param>
        /// <param name="borderColor"></param>
        /// <param name="opacity"></param>
        public void Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1)
        {

        }

        /// <summary>
        /// Dispose polygon.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
}
