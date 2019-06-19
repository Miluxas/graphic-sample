/*********************************************************************
 *  Rectangle.cs
 *  Implementation of the Class Rectangle
 *  Created on:      30-Nov-2014 17:10:56 
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.IDrawing;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace xPFT.GraphicEngineGDIP
{
    public class Rectangle:IDrawing.IRectangle
    {
        #region Constructor
        public Rectangle(IDrawing.IDevice device)
        {
            this.device =(Device) device;
        }
        #endregion

        #region Fields
        private Device device;
        private TextureBrush bitmapBrush;
        System.Drawing.Bitmap prevBitmap;
        System.Drawing.Color prevFillColor;
        Graphics prevRenderTarget;
        float prevOpacity;
        xPFT.Charting.Base.FillPattern prevFillPattern;

        Brush fillBrush;
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
                    fillBrush = xPFT.GraphicEngineGDI.BrushMaker.GetPatternBrush(fillColor.Value, opacity, fillPattern, (int)patternSize);
                    prevFillColor = (System.Drawing.Color)fillColor;
                    prevFillPattern = fillPattern;
                    prevOpacity = opacity;
                    prevRenderTarget = device.renderTarget;
                }
                device.renderTarget.FillRectangle(fillBrush, rectangle);
            }
            if (borderColor != null)
                device.renderTarget.DrawRectangle(new Pen(borderColor.Value, lineWidth), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
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
            device.renderTarget.FillRectangle(bitmapBrush, rectangle);
            if (borderColor != null)
                device.renderTarget.DrawRectangle(new Pen(borderColor.Value, (int)lineWidth), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
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
