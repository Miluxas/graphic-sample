using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using xPFT.DrawingBase;

namespace xPFT.GraphicEngineGDI
{
    public class Rectangle
    {
        #region Methods
        public static void Draw(Graphics g ,Point pos, System.Drawing.Rectangle rectangle, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize, float lineWidth)
        {
            g.TranslateTransform(pos.X, pos.Y);
            if (fillColor != null)
                g.FillRectangle(BrushMaker.GetPatternBrush((System.Drawing.Color)fillColor, opacity, fillPattern, (int)patternSize), rectangle);
            if (borderColor != null)
                g.DrawRectangle(new Pen(BrushMaker.SetOpacity((Color)borderColor,opacity), lineWidth), rectangle);
            g.ResetTransform();
        }

        public static void Draw(Graphics g, Point pos, System.Drawing.Rectangle rectangle, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1, float lineWidth = 1)
        {
            g.TranslateTransform(pos.X, pos.Y);
            if (bitmap != null)
                g.FillRectangle(new TextureBrush(bitmap), rectangle);
            if (borderColor != null)
                g.DrawRectangle(new Pen(BrushMaker.SetOpacity((Color)borderColor,opacity), lineWidth), rectangle);
            g.ResetTransform();
        }
        #endregion
    }
}
