using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace xPFT.GraphicEngineGDI
{
    public class BrushMaker
    {
        public static Color SetOpacity(Color c, float opacity)
        {
            return Color.FromArgb((int)(opacity * 255), c);
        }
        public static Brush GetPatternBrush(System.Drawing.Color fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, int patternSize = 10)
        {
            //BitmapRenderTarget brt = new BitmapRenderTarget(renderTarget, CompatibleRenderTargetOptions.None, new SharpDX.Size2F(patternSize, patternSize));
            Bitmap bitmap = new Bitmap(patternSize, patternSize);
            Graphics g = Graphics.FromImage(bitmap);
            Pen pen = new Pen(BrushMaker.SetOpacity(fillColor,opacity), 1);
            SolidBrush solidBrush = new SolidBrush(BrushMaker.SetOpacity(fillColor,opacity));
            if (fillPattern == xPFT.Charting.Base.FillPattern.Solid)
                return solidBrush ;
            switch (fillPattern)
            {
                case xPFT.Charting.Base.FillPattern.Hachure_Double:
                    g.DrawLine(pen, new Point(0, 0), new Point(patternSize, patternSize));
                    g.DrawLine(pen,new Point(patternSize, 0), new Point(0, patternSize));
                    break;
                case xPFT.Charting.Base.FillPattern.Hachure:
                    g.DrawLine(pen,new Point(patternSize, 0), new Point(0, patternSize));
                    break;
                case xPFT.Charting.Base.FillPattern.Hashure_Back:
                    g.DrawLine(pen,new Point(0, 0), new Point(patternSize, patternSize));
                    break;
                case xPFT.Charting.Base.FillPattern.Dash:
                    g.DrawLine(pen,new Point(0, patternSize / 4), new Point(patternSize / 4, patternSize / 4));
                    g.DrawLine(pen,new Point(patternSize * 3 / 4, patternSize / 4), new Point(patternSize, patternSize / 4));
                    g.DrawLine(pen,new Point(patternSize / 4, patternSize * 3 / 4), new Point(patternSize * 3 / 4, patternSize * 3 / 4));
                    break;
                case xPFT.Charting.Base.FillPattern.Squre:
                    g.DrawLine(pen,new Point(0, patternSize / 2), new Point(patternSize, patternSize / 2));
                    g.DrawLine(pen,new Point(patternSize / 2, 0), new Point(patternSize / 2, patternSize));
                    break;
                case xPFT.Charting.Base.FillPattern.Dot:
                    g.FillEllipse( solidBrush ,patternSize / 4, patternSize / 4, 1, 1);
                    g.FillEllipse(solidBrush, patternSize / 4, patternSize * 3 / 4, 1, 1);
                    g.FillEllipse(solidBrush, patternSize * 3 / 4, patternSize / 4, 1, 1);
                    g.FillEllipse(solidBrush, patternSize * 3 / 4, patternSize * 3 / 4, 1, 1);
                    break;
                case xPFT.Charting.Base.FillPattern.Zig_Zag_Horizontal:
                    g.DrawLine(pen,new Point(0, patternSize / 2), new Point(patternSize / 2, 0));
                    g.DrawLine(pen,new Point(patternSize, patternSize / 2), new Point(patternSize / 2, 0));
                    g.DrawLine(pen,new Point(0, patternSize), new Point(patternSize / 2, patternSize / 2));
                    g.DrawLine(pen,new Point(patternSize, patternSize), new Point(patternSize / 2, patternSize / 2));
                    break;
                case xPFT.Charting.Base.FillPattern.Zig_Zag_Vertical:
                    g.DrawLine(pen,new Point( patternSize / 2,0), new Point(0,patternSize / 2));
                    g.DrawLine(pen,new Point(patternSize/2, patternSize), new Point(0,patternSize / 2));
                    g.DrawLine(pen,new Point( patternSize,0), new Point(patternSize / 2, patternSize / 2));
                    g.DrawLine(pen,new Point(patternSize, patternSize), new Point(patternSize / 2, patternSize / 2));
                    break;
            }

            TextureBrush br = new TextureBrush(bitmap);
            g.Dispose();
            pen.Dispose();
            bitmap.Dispose();
            solidBrush.Dispose();
            return br;
        }
    
    
    }
}
