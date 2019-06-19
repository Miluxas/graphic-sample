/*********************************************************************
 *  HorizontalAnnotationLine.cs
 *  Implementation of the Class HorizontalAnnotationLine
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
    internal class HorizontalAnnotationLine : AnnotationLine
    {
        public HorizontalAnnotationLine(ChartArea parentChartArea)
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
            return (point.X >= points[0].X && point.X <= points[1].X) && (Math.Abs(point.Y - points[0].Y) < MouseHitRay + LineWidth / 2);
        }

        /// <summary>
        /// Reset the position of the line. 
        /// </summary>
        public override void Reposition()
        {
            points[0].X = ParentControl.Padding.Left;
            points[1].X = ParentControl.Width - ParentControl.Padding.Right;
            float y;
            if (verticalAxis != null)
                y = ParentControl.Padding.Top - Value * verticalAxis.HeightRate + verticalAxis.Center;
            else
                y = ParentControl.Padding.Top + (1 - Value) * ParentControl.DrawHeight;

            points[0].Y = points[1].Y = y;
            textRectangle = new Rectangle((int)points[0].X + 5, (int)points[0].Y, TEXT_RECTANGLE_WIDTH, TEXT_RECTANGLE_HEIGHT);
        }

        #endregion

    }
}
