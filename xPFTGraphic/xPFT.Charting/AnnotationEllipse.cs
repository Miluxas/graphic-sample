using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace xPFT.Charting
{
    public class AnnotationEllipse : AnnotationShape
    {
        public AnnotationEllipse(ChartArea chA)
        {
            CreateAnnotationShape(chA);
        }

        #region Fields
        IDrawing.IEllipse mainEllipse;
        #endregion

        #region Methods
        /// <summary>
        /// Dispose the annotation ellipse.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (mainEllipse != null)
                mainEllipse.Dispose();
        }

        /// <summary>
        /// Draw the annotation ellipse on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public override void Draw(int layerIndex)
        {
            if (mainEllipse != null)
            {
                mainEllipse.LineWidth = LineWidth;
                if (Bitmap != null)
                    mainEllipse.Draw(layerIndex, rectangle, Bitmap, LineColor, Opacity);
                else
                    mainEllipse.Draw(layerIndex, rectangle, LineColor, FillColor, Opacity, FillPattern, PatternSize);
            }
        }

        /// <summary>
        /// Draw the annotaion ellipse on the graphics.
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g,Point pos)
        {
            if (mainEllipse != null)
            {
                mainEllipse.LineWidth = LineWidth;
                if (Bitmap != null)
                    xPFT.GraphicEngineGDI.Ellipse.Draw(g,pos, rectangle, Bitmap, LineColor, Opacity, LineWidth);
                else
                    xPFT.GraphicEngineGDI.Ellipse.Draw(g,pos, rectangle, LineColor,FillColor, Opacity,FillPattern,PatternSize, LineWidth);
            }
        }

        /// <summary>
        /// Initialize the ellipse on the device.
        /// </summary>
        /// <param name="device"></param>
        internal override void Initialize(IDrawing.IDevice device)
        {
            base.Initialize(device);
            if (mainEllipse != null)
            {
                mainEllipse.Dispose();
                mainEllipse = null;
            }
            mainEllipse = GraphicEngine.GraphicEngine.CreateEllipse(device);
            Reposition();
        }

        /// <summary>
        /// Determine is the point on the annotation or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal override bool IsMouseOnTheElement(System.Drawing.PointF point)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(rectangle);
            return gp.IsVisible(point);
        }

        #endregion
    }
}

