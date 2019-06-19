/*********************************************************************
 *  VerticalAnnotationLine.cs
 *  Implementation of the Class VerticalAnnotationLine
 *  Created on:      21-Oct-2014 10:45:56 
 *  Original author: Teb Tasvir
 *  
 * 
 **********************************************************************/

using System;
using System.Drawing;
using System.Collections.Generic;

namespace xPFT.Charting
{
    public class VerticalAnnotationLine:AnnotationLine
    {
        public VerticalAnnotationLine(ChartArea parentChartArea)
        {
            base.ParentControl = parentChartArea;
        }

        #region Methods

        /// <summary>
        /// Determine is the point on the line or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal override bool IsMouseOnTheElement(PointF point)
        {
            return (point.Y >= points[0].Y && point.Y <= points[1].Y) && (Math.Abs(point.X - points[0].X) < MouseHitRay+LineWidth/2);
        }

        /// <summary>
        /// Reset the position of the line. 
        /// </summary>
        public override void Reposition()
        {
            points[0].Y = ParentControl.Padding.Top;
            points[1].Y = ParentControl.Height - ParentControl.Padding.Bottom;
            float x;

            if (horizontalAxis != null)
                x = ParentControl.Padding.Left + horizontalAxis.Center + Value * horizontalAxis.WidthRate;
            else
                x = ParentControl.Padding.Left + Value * ParentControl.DrawWidth;

            points[0].X = points[1].X = x;
            textRectangle = new Rectangle((int)points[0].X + 5, (int)points[0].Y, TEXT_RECTANGLE_WIDTH, TEXT_RECTANGLE_HEIGHT);
        }
        
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion
    }
}
