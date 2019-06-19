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
using System.Drawing;
using System.Drawing.Imaging;
using xPFT.DrawingBase;

namespace xPFT.GraphicEngineGDIP
{
    public class Ellipse : IDrawing.IEllipse
    {
        #region Constructor
        public Ellipse(IDrawing.IDevice device)
        {
            this.device = (Device)device;
        }
        #endregion

        #region Fields
        private Device device;
        System.Drawing.Bitmap prevBitmap;
         System.Drawing.Color prevFillColor;
         float prevOpacity;
        xPFT.Charting.Base.FillPattern prevFillPattern;
        Brush bitmapBrush;
        Brush fillBrush;
        Graphics prevRenderTarget;
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
        /// Draw an ellipse in a rectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize)
        {
            //var tmpEllipse = new SharpDX.Direct2D1.Ellipse(new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2), rectangle.Width / 2, rectangle.Height / 2);
            if (fillColor != null)
            {
                if (fillColor != prevFillColor || fillPattern != prevFillPattern || opacity != prevOpacity)
                {
                    fillBrush = xPFT.GraphicEngineGDI.BrushMaker.GetPatternBrush(fillColor.Value, opacity, fillPattern,(int) patternSize);
                    prevFillColor = (System.Drawing.Color)fillColor;
                    prevFillPattern = fillPattern;
                    prevOpacity = opacity;
                    prevRenderTarget = device.renderTarget;
                }
                device.renderTarget.FillEllipse(fillBrush,rectangle);
            }
            if(borderColor!=null)
                device.renderTarget.DrawEllipse(new Pen(borderColor.Value,lineWidth),rectangle);
        }
        /// <summary>
        /// Draw an ellipse in a rectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1)
        {
            if (bitmap != prevBitmap)
            {
                prevBitmap = bitmap;
                bitmapBrush = new TextureBrush(bitmap);
            }
            device.renderTarget.FillEllipse( bitmapBrush,rectangle);
            if (borderColor != null)
                device.renderTarget.DrawEllipse(new Pen(borderColor.Value, (int)lineWidth),rectangle);
        }
        
        /// <summary>
        /// Dispose the ellipse.
        /// </summary>
        public void Dispose()
        {
        }
        
        #endregion
    }
}
