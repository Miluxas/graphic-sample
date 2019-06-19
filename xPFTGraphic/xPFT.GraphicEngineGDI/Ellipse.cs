using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.DrawingBase;
using System.Drawing;

namespace xPFT.GraphicEngineGDI
{
    public class Ellipse
    {

       public static void Draw(Graphics g,Point pos, System.Drawing.RectangleF rectangle, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize,float lineWidth)
        {
            g.TranslateTransform(pos.X, pos.Y);
            if (fillColor != null)
                g.FillEllipse(BrushMaker.GetPatternBrush((System.Drawing.Color)fillColor,opacity,fillPattern,(int)patternSize), rectangle);
            if (borderColor != null)
                g.DrawEllipse(new Pen(BrushMaker.SetOpacity((System.Drawing.Color)borderColor, opacity), lineWidth), rectangle);
            g.ResetTransform();
        }

       public static void Draw(Graphics g, Point pos, System.Drawing.RectangleF rectangle, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1, float lineWidth = 1)
        {
            g.TranslateTransform(pos.X, pos.Y);
            if (bitmap != null)
                g.FillEllipse(new TextureBrush(bitmap), rectangle);
            if (borderColor != null)
                g.DrawEllipse(new Pen(BrushMaker.SetOpacity((System.Drawing.Color)borderColor,opacity), lineWidth), rectangle);
            g.ResetTransform();
        } 
    }
}
