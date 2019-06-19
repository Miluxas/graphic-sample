using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace xPFT.Charting
{
    public class AnnotationPolygon : AnnotationShape
    {

        public AnnotationPolygon(ChartArea chA,PointF[] points)
        {
            CreateAnnotationShape(chA);
            this.points = points;
            DrawPoints = new PointF[points.Length+1];
        }


        #region Fields
        /// <summary>
        /// polygon.
        /// </summary>
        IDrawing.IPolygon mainPolygon;
        /// <summary>
        /// polygon points.
        /// </summary>
        PointF[] points,DrawPoints;
        #endregion

        #region Methods
        /// <summary>
        /// Dispose the polygon annotaion.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (mainPolygon != null)
                mainPolygon.Dispose();
        }

        /// <summary>
        /// Draw the annotation polygon on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public override void Draw(int layerIndex)
        {
            if (mainPolygon != null)
                if (DrawPoints.Length > 2)
                {
                    mainPolygon.LineWidth = LineWidth;
                    if (Bitmap != null)
                        mainPolygon.Draw(layerIndex, DrawPoints, Bitmap, LineColor, Opacity);
                    else
                       mainPolygon.Draw(layerIndex, DrawPoints, LineColor, FillColor, Opacity, FillPattern, PatternSize);
                }
        }

        /// <summary>
        /// Draw the annotaion polygon on the graphics.
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g,Point pos)
        {
            if (DrawPoints.Length > 2)
                if (Bitmap != null)
                    xPFT.GraphicEngineGDI.Polygon.Draw(g,pos, DrawPoints, Bitmap, LineColor, Opacity, LineWidth);
                else
                    xPFT.GraphicEngineGDI.Polygon.Draw(g,pos, DrawPoints, LineColor, FillColor, Opacity, FillPattern, PatternSize, LineWidth);
        }

        /// <summary>
        /// Reset the annotation position.
        /// </summary>
        public override void Reposition()
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (horizontalAxis != null && verticalAxis != null)

                    DrawPoints[i] = new PointF(points[i].X * horizontalAxis.WidthRate + ParentControl.Padding.Left + horizontalAxis.Center
                    , 1 - points[i].Y * verticalAxis.HeightRate + ParentControl.Padding.Top + verticalAxis.Center);
                else
                    DrawPoints[i] = new PointF(points[i].X * ParentControl.DrawWidth + ParentControl.Padding.Left
                    , (1 - points[i].Y) * ParentControl.DrawHeight + ParentControl.Padding.Top);
            }
            DrawPoints[points.Length] = DrawPoints[0];
        }

        /// <summary>
        /// Initialize the polygon on the device.
        /// </summary>
        /// <param name="device"></param>
        internal override void Initialize(IDrawing.IDevice device)
        {
            base.Initialize(device);
            if (mainPolygon != null)
            {
                mainPolygon.Dispose();
                mainPolygon = null;
            }
            mainPolygon = GraphicEngine.GraphicEngine.CreatePolygon(device);
            Reposition();
        }

        /// <summary>
        /// Get is mouse on the annotation polygon or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal override bool IsMouseOnTheElement(PointF point)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            if (DrawPoints.Length > 5)
                gp.AddLines(DrawPoints);
            return gp.IsVisible(point);
        }

        #endregion
    }
}

