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
        /// Draw an ellipse in a rectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize)
        {
            var tmpEllipse = new SharpDX.Direct2D1.Ellipse(new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2), rectangle.Width / 2, rectangle.Height / 2);
            if (fillColor != null)
            {
                if (device.renderTarget!=prevRenderTarget || fillColor != prevFillColor || fillPattern != prevFillPattern || opacity != prevOpacity)
                {
                    fillBrush = BrushMaker.GetPatternBrush(device.renderTarget, (System.Drawing.Color)fillColor, opacity, fillPattern, patternSize);
                    prevFillColor = (System.Drawing.Color)fillColor;
                    prevFillPattern = fillPattern;
                    prevOpacity = opacity;
                    prevRenderTarget = device.renderTarget;
                }

                device.layers[layerIndex].FillEllipse(tmpEllipse, fillBrush);
            }
            if(borderColor!=null)
                device.layers[layerIndex].DrawEllipse(tmpEllipse, new SharpDX.Direct2D1.SolidColorBrush(device.renderTarget, xPFT.DrawingBase.Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity)), lineWidth);
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
                bitmapBrush = new BitmapBrush(device.renderTarget, Convertor.ConvertDrawingBmpToDirectxBmp(device.renderTarget, prevBitmap));
                bitmapBrush.InterpolationMode = BitmapInterpolationMode.Linear;
                bitmapBrush.Opacity = opacity;
                bitmapBrush.ExtendModeX = ExtendMode.Wrap;
                bitmapBrush.ExtendModeY = ExtendMode.Wrap;
            }
            var tmpEllipse = new SharpDX.Direct2D1.Ellipse(new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2), rectangle.Width / 2, rectangle.Height / 2);
            device.layers[layerIndex].FillEllipse(tmpEllipse, bitmapBrush);
            if (borderColor != null)
                device.layers[layerIndex].DrawEllipse(tmpEllipse, new SharpDX.Direct2D1.SolidColorBrush(device.renderTarget, xPFT.DrawingBase.Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity)), lineWidth);
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
