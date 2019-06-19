using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace xPFT.GraphicEngineGDI
{
    public class Polygon
    {

        public static void Draw(Graphics g,Point pos, System.Drawing.PointF[] points, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize, float lineWidth)
        {
            g.TranslateTransform(pos.X, pos.Y);
            if (fillColor != null)
                g.FillPolygon(BrushMaker.GetPatternBrush((System.Drawing.Color)fillColor, opacity, fillPattern, (int)patternSize), points);
            if (borderColor != null)
                g.DrawPolygon(new Pen(BrushMaker.SetOpacity((Color)borderColor,opacity), lineWidth), points);
            g.ResetTransform();
        }

        public static void Draw(Graphics g, Point pos, System.Drawing.PointF[] points, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1, float lineWidth = 1)
        {
            g.TranslateTransform(pos.X, pos.Y);
            if (bitmap != null)
                g.FillPolygon(new TextureBrush(bitmap), points);
            if (borderColor != null)
                g.DrawPolygon(new Pen(BrushMaker.SetOpacity((Color)borderColor,opacity), lineWidth), points);
            g.ResetTransform();
        }

        public static void Draw(Graphics g, Point pos, System.Drawing.PointF[] points, System.Drawing.Color color, float opacity = 1, float lineWidth = 1)
        {
            g.TranslateTransform(pos.X, pos.Y);
            if (color != null)
                g.DrawPolygon(new Pen(BrushMaker.SetOpacity((Color)color,opacity), lineWidth), points);
            g.ResetTransform();
        }

    }
}
