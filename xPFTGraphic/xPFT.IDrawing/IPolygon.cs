using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPFT.IDrawing
{
    public interface IPolygon
    {
        /// <summary>
        /// Get or Set the sidth of the line
        /// </summary>
        float LineWidth { get; set; }
        /// <summary>
        /// Draw polygon on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="borderColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="opacity"></param>
        /// <param name="fillPattern"></param>
        /// <param name="patternSize"></param>
        void Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize);
        /// <summary>
        /// Draw polygon on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="bitmap"></param>
        /// <param name="borderColor"></param>
        /// <param name="opacity"></param>
        void Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1);
        /// <summary>
        /// Dispose the polygon.
        /// </summary>
        void Dispose();
    }
}
