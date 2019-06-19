using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace xPFT.Charting
{
    public class AnnotationRectangle : AnnotationShape
    {
        public AnnotationRectangle(ChartArea chA)
        {
            CreateAnnotationShape(chA);
        }

        #region Fields
        /// <summary>
        /// rectangle that be drawn.
        /// </summary>
        IDrawing.IRectangle mainRec;
        #endregion

        #region Methods
        /// <summary>
        /// Dispose the rectangle annotaion.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (mainRec != null)
                mainRec.Dispose();
        }

        /// <summary>
        /// Draw the annotation rectangle on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public override void Draw(int layerIndex)
        {
            if (mainRec != null)
            {
                mainRec.LineWidth = LineWidth;
                if (Bitmap != null)
                    mainRec.Draw(layerIndex, rectangle, Bitmap, LineColor, Opacity);
                else
                    mainRec.Draw(layerIndex, rectangle, LineColor, FillColor, Opacity, FillPattern, PatternSize);
            }
        }

        /// <summary>
        /// Draw the annotaion rectangle on the graphics.
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g,Point pos)
        {
            if (mainRec != null)
            {
                mainRec.LineWidth = LineWidth;
                if (Bitmap != null)
                    xPFT.GraphicEngineGDI.Rectangle.Draw(g,pos, new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height), Bitmap, LineColor, Opacity);
                else
                    xPFT.GraphicEngineGDI.Rectangle.Draw(g,pos, new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height), LineColor, FillColor, Opacity, FillPattern, PatternSize, LineWidth);
            }
        }

        /// <summary>
        /// Initialize the rectangle on the device.
        /// </summary>
        /// <param name="device"></param>
        internal override void Initialize(IDrawing.IDevice device)
        {
            base.Initialize(device);
            if (mainRec != null)
            {
                mainRec.Dispose();
                mainRec = null;
            }
            mainRec = GraphicEngine.GraphicEngine.CreateRectangle(device);
            Reposition();
        }
        #endregion
    }
}
