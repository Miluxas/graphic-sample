using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.DrawingBase;

namespace xPFT.GraphicEngineD3D9
{
    public class Ellipse:IDrawing.IEllipse
    {
        #region Constructor
        public Ellipse(IDrawing.IDevice device)
        {
            this.device =(Device) device;
            line = new SharpDX.Direct3D9.Line(this.device.device);
            line.Width = lineWidth;
            line.Antialias = true;
            fillerLine = new SharpDX.Direct3D9.Line(this.device.device);
            fillerLine.Width = lineWidth;
            fillerLine.Antialias = true;
        }
        #endregion

        #region Fields
        private SharpDX.Direct3D9.Line line,fillerLine;
        private Device device;

        #endregion

        #region Properties
        public bool IsDisposed
        {
            get { return line.IsDisposed; }
        }
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
                if (line != null)
                    line.Width = lineWidth;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draw ellipse on the layer with layerIndex
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
            if (fillColor != null || borderColor != null)
            {
                if (line != null)
                {
                    line.Width = lineWidth;
                    List<SharpDX.Vector2> tmpPointArray = GetPointOfEllipse(rectangle.Width / 2, rectangle.Height / 2);
                    line.Begin();
                    device.device.SetTransform(0, SharpDX.Matrix.Translation(rectangle.X, rectangle.Y, 0));
                    if (fillColor != null)
                        line.Draw(ShapesFiller.GetFillEllipseArray(0, 0, rectangle.Width, rectangle.Height).ToArray(), Convertor.ColorConvertor((System.Drawing.Color)fillColor, opacity));
                    if (borderColor != null)
                        line.Draw(tmpPointArray.ToArray(), Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity));
                    line.End();
                }

            }
        }

        /// <summary>
        /// Draw ellipse on the layer with layerIndex
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="rectangle"></param>
        /// <param name="bitmap"></param>
        /// <param name="borderColor"></param>
        /// <param name="opacity"></param>
        public void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1)
        {
            List<SharpDX.Vector2> tmpPointArray = GetPointOfEllipse(rectangle.Width / 2, rectangle.Height / 2);
            line.Begin();
            device.device.SetTransform(0, SharpDX.Matrix.Translation(rectangle.X, rectangle.Y, 0));
            if (borderColor != null)
                line.Draw(tmpPointArray.ToArray(), Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity));
            line.End();
        } 

        /// <summary>
        /// Get the point of the ellipse with a B and A ray.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private List<SharpDX.Vector2> GetPointOfEllipse(float A,float B)
        {

            List<SharpDX.Vector2> tmpPointArray = new List<SharpDX.Vector2>();
            float x, y;

            for (float i = 0; i <= (2 * Math.PI); i += 0.002f)
            {
                x = A + A * (float)Math.Cos((Double)i);
                y = B + B * (float)Math.Sin((Double)i);
                tmpPointArray.Add(new SharpDX.Vector2(x, y));
            }
            tmpPointArray.Add(tmpPointArray[0]);
            return tmpPointArray;
        }

        /// <summary>
        /// Dispose tge ellipse.
        /// </summary>
        public void Dispose()
        {
            if (line != null)
            {
                line.Dispose();
                line = null;
            }
            if (fillerLine != null)
            {
                fillerLine.Dispose();
                fillerLine = null;
            }
        
        #endregion
        }
    }
}
