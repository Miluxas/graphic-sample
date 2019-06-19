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
using SharpDX.Direct2D1;
using SharpDX;

namespace xPFT.GraphicEngineD2D1
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
        private BitmapBrush bitmapBrush;
        System.Drawing.Bitmap prevBitmap;
        System.Drawing.Color prevFillColor;
        RenderTarget prevRenderTarget;
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
        /// Draw the rectangle on layer with layerIndex
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize)
        {
            if (device.renderTarget != null)
            {
                var tmpRectangle = new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

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
                    device.layers[layerIndex].FillRectangle(tmpRectangle, fillBrush);
                }
                if (borderColor != null)
                    device.layers[layerIndex].DrawRectangle(tmpRectangle, new SharpDX.Direct2D1.SolidColorBrush(device.renderTarget, xPFT.DrawingBase.Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity)), lineWidth);
            }
        }

        /// <summary>
        /// Draw the rectangle on layer with layerIndex
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="rectangle"></param>
        /// <param name="bitmap"></param>
        /// <param name="borderColor"></param>
        /// <param name="opacity"></param>
        public void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1)
        {
            if (bitmap != prevBitmap)
            {
                prevBitmap=bitmap;
                bitmapBrush=new BitmapBrush(device.renderTarget, Convertor.ConvertDrawingBmpToDirectxBmp(device.renderTarget,prevBitmap));
                bitmapBrush.InterpolationMode = BitmapInterpolationMode.Linear;
                bitmapBrush.Opacity = opacity;
                bitmapBrush.ExtendModeX = ExtendMode.Wrap;
                bitmapBrush.ExtendModeY = ExtendMode.Wrap;
                
            }
            device.layers[layerIndex].FillRectangle(new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), bitmapBrush);
            if (borderColor != null)
                device.layers[layerIndex].DrawRectangle(new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), new SharpDX.Direct2D1.SolidColorBrush(device.renderTarget, xPFT.DrawingBase.Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity)), lineWidth);
        }


        public void Dispose()
        {
        }
        #endregion
    }
}
