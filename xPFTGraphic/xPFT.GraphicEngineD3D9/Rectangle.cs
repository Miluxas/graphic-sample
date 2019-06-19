using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.IDrawing;
using xPFT.DrawingBase;

namespace xPFT.GraphicEngineD3D9
{
    public class Rectangle :IDrawing.IRectangle
    {
        #region Constructor
        public Rectangle(IDevice device)
        {
            this.device = (Device)device;
            line = new SharpDX.Direct3D9.Line(this.device.device);
            line.Width = lineWidth;
            line.Antialias = true;
            filler = new SharpDX.Direct3D9.Line(this.device.device);
            filler.Width = lineWidth;
        }
        #endregion

        #region Fields
        private SharpDX.Direct3D9.Line line,filler;
        private Device device;
        float prevWidth;
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
                if(line!=null)
                line.Width = lineWidth;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draw rectangle on the layer with layer index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="rectangle"></param>
        /// <param name="borderColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="opacity"></param>
        /// <param name="fillPattern"></param>
        /// <param name="patternSize"></param>
        public void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize)
        {
            if (prevWidth != rectangle.Height)
            {
                prevWidth = rectangle.Height;
                filler.Width = prevWidth;
            }
            if (fillColor != null)
            {
                SharpDX.Vector2[] points = { new SharpDX.Vector2(rectangle.X, rectangle.Y+rectangle.Height/2),
                                         new SharpDX.Vector2(rectangle.X+rectangle.Width, rectangle.Y+rectangle.Height/2)};
                if (filler != null)
                {
                    filler.Begin();
                     filler.Draw(points, Convertor.ColorConvertor((System.Drawing.Color)fillColor, opacity));
                    filler.End();
                }
            }
            if(borderColor!=null)
            {
                SharpDX.Vector2[] points = { new SharpDX.Vector2(rectangle.X, rectangle.Y),
                                       new SharpDX.Vector2(rectangle.X+rectangle.Width, rectangle.Y),
                                       new SharpDX.Vector2(rectangle.X+rectangle.Width, rectangle.Y+rectangle.Height),
                                       new SharpDX.Vector2(rectangle.X, rectangle.Y+rectangle.Height),
                                      new SharpDX.Vector2(rectangle.X, rectangle.Y),};
                if (line != null)
                {
                    line.Begin();
                    line.Draw(points, Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity));
                    line.End();
                }
            }
        }

        /// <summary>
        /// Draw rectangle on the layer with layer index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="rectangle"></param>
        /// <param name="bitmap"></param>
        /// <param name="borderColor"></param>
        /// <param name="opacity"></param>
        public void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1)
        {
            if (borderColor != null)
            {
                SharpDX.Vector2[] points = { new SharpDX.Vector2(rectangle.X, rectangle.Y),
                                       new SharpDX.Vector2(rectangle.X+rectangle.Width, rectangle.Y),
                                       new SharpDX.Vector2(rectangle.X+rectangle.Width, rectangle.Y+rectangle.Height),
                                       new SharpDX.Vector2(rectangle.X, rectangle.Y+rectangle.Height),
                                       new SharpDX.Vector2(rectangle.X, rectangle.Y),};
                line.Begin();
                line.Draw(points, Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity));
                line.End();
            }
        }

        /// <summary>
        /// Dispose the rectangle
        /// </summary>
        public void Dispose()
        {
            if (line != null)
            {
                line.Dispose();
                line = null;
            }
            if (filler != null)
            {
                filler.Dispose();
                filler = null;
            }
        }
        #endregion
    }
}
