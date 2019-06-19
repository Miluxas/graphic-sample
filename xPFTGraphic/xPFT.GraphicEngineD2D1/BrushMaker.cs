using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct2D1;

namespace xPFT.GraphicEngineD2D1
{
    internal class BrushMaker
    {
        public static Brush GetPatternBrush(RenderTarget renderTarget, System.Drawing.Color fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize = 10)
        {
            if (renderTarget != null)
            {
                BitmapRenderTarget brt = new BitmapRenderTarget(renderTarget, CompatibleRenderTargetOptions.None, new SharpDX.Size2F(patternSize, patternSize));
                Brush tempBrush = new SolidColorBrush(brt, xPFT.DrawingBase.Convertor.ColorConvertor(fillColor, opacity));
                if (fillPattern == xPFT.Charting.Base.FillPattern.Solid)
                    return tempBrush;
                brt.BeginDraw();
                brt.Clear(Color.Transparent);
                switch (fillPattern)
                {
                    case xPFT.Charting.Base.FillPattern.Hachure_Double:
                        brt.DrawLine(new SharpDX.Vector2(0, 0), new SharpDX.Vector2(patternSize, patternSize), tempBrush, 1);
                        brt.DrawLine(new SharpDX.Vector2(patternSize, 0), new SharpDX.Vector2(0, patternSize), tempBrush, 1);
                        break;
                    case xPFT.Charting.Base.FillPattern.Hachure:
                        brt.DrawLine(new SharpDX.Vector2(patternSize, 0), new SharpDX.Vector2(0, patternSize), tempBrush, 0.5f);
                        break;
                    case xPFT.Charting.Base.FillPattern.Hashure_Back:
                        brt.DrawLine(new SharpDX.Vector2(0, 0), new SharpDX.Vector2(patternSize, patternSize), tempBrush, 0.5f);
                        break;
                    case xPFT.Charting.Base.FillPattern.Dash:
                        brt.DrawLine(new SharpDX.Vector2(0, patternSize / 4), new SharpDX.Vector2(patternSize / 4, patternSize / 4), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(patternSize * 3 / 4, patternSize / 4), new SharpDX.Vector2(patternSize, patternSize / 4), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(patternSize / 4, patternSize * 3 / 4), new SharpDX.Vector2(patternSize * 3 / 4, patternSize * 3 / 4), tempBrush, 0.5f);
                        break;
                    case xPFT.Charting.Base.FillPattern.Squre:
                        brt.DrawLine(new SharpDX.Vector2(0, patternSize / 2), new SharpDX.Vector2(patternSize, patternSize / 2), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(patternSize / 2, 0), new SharpDX.Vector2(patternSize / 2, patternSize), tempBrush, 0.5f);
                        break;
                    case xPFT.Charting.Base.FillPattern.Dot:
                        brt.FillEllipse(new SharpDX.Direct2D1.Ellipse(new SharpDX.Vector2(patternSize / 4, patternSize / 4), 1, 1), tempBrush);
                        brt.FillEllipse(new SharpDX.Direct2D1.Ellipse(new SharpDX.Vector2(patternSize / 4, patternSize * 3 / 4), 1, 1), tempBrush);
                        brt.FillEllipse(new SharpDX.Direct2D1.Ellipse(new SharpDX.Vector2(patternSize * 3 / 4, patternSize / 4), 1, 1), tempBrush);
                        brt.FillEllipse(new SharpDX.Direct2D1.Ellipse(new SharpDX.Vector2(patternSize * 3 / 4, patternSize * 3 / 4), 1, 1), tempBrush);
                        break;
                    case xPFT.Charting.Base.FillPattern.Zig_Zag_Horizontal:
                        brt.DrawLine(new SharpDX.Vector2(0, patternSize / 2), new SharpDX.Vector2(patternSize / 2, 0), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(patternSize, patternSize / 2), new SharpDX.Vector2(patternSize / 2, 0), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(0, patternSize), new SharpDX.Vector2(patternSize / 2, patternSize / 2), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(patternSize, patternSize), new SharpDX.Vector2(patternSize / 2, patternSize / 2), tempBrush, 0.5f);
                        break;
                    case xPFT.Charting.Base.FillPattern.Zig_Zag_Vertical:
                        brt.DrawLine(new SharpDX.Vector2(patternSize / 2, 0), new SharpDX.Vector2(0, patternSize / 2), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(patternSize / 2, patternSize), new SharpDX.Vector2(0, patternSize / 2), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(patternSize, 0), new SharpDX.Vector2(patternSize / 2, patternSize / 2), tempBrush, 0.5f);
                        brt.DrawLine(new SharpDX.Vector2(patternSize, patternSize), new SharpDX.Vector2(patternSize / 2, patternSize / 2), tempBrush, 0.5f);
                        break;
                }
                brt.EndDraw();
                BitmapBrushProperties bmpbp = new BitmapBrushProperties();
                bmpbp.ExtendModeX = ExtendMode.Wrap;
                bmpbp.ExtendModeY = ExtendMode.Wrap;
                bmpbp.InterpolationMode = BitmapInterpolationMode.Linear;
                BitmapBrush br = new BitmapBrush(brt, brt.Bitmap, bmpbp);
                tempBrush.Dispose();
                brt.Dispose();
                return br;
            }
            else
                return null;
        }
    }
}
