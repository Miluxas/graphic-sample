using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPFT.IDrawing
{
    public interface IEllipse
    {
        /// <summary>
        /// Get or Set the width of line.
        /// </summary>
        float LineWidth { get; set; }
        /// <summary>
        /// Draw the ellipse on the device.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="rectangle"></param>
        /// <param name="borderColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="opacity"></param>
        /// <param name="fillPattern"></param>
        /// <param name="patternSize"></param>
        void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize);
        /// <summary>
        /// Draw the ellipse on the device.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="rectangle"></param>
        /// <param name="bitmap"></param>
        /// <param name="borderColor"></param>
        /// <param name="opacity"></param>
        void Draw(int layerIndex, System.Drawing.RectangleF rectangle, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1);
        /// <summary>
        /// Dispose the Ellipse.
        /// </summary>
        void Dispose();
    }
}
