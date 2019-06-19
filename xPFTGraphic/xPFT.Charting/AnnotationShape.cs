/*********************************************************************
 *  AnnotationShape.cs
 *  Implementation of the Class AnnotationShape
 *  Created on:      3-Dec-2014 15:45:56 
 *  Original author: Teb Tasvir
 *  
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace xPFT.Charting
{
    public abstract class AnnotationShape : Annotation
    {
        #region Fields
        /// <summary>
        /// Shape rectangle start value point.
        /// </summary>
        public PointF startValuePoint = new PointF();
        /// <summary>
        /// Shape rectangle end value point.
        /// </summary>
        public PointF endValuePoint = new PointF();
        /// <summary>
        /// The rectangle that shape is draw in it.
        /// </summary>
        protected RectangleF rectangle = new RectangleF();
        /// <summary>
        /// Backgrand image.
        /// </summary>
        internal Bitmap Bitmap=null;

        #endregion

        #region Properties
        internal Color? fillColor = Color.SteelBlue;
        /// <summary>
        /// Get the shape Fill Color. If set with null the shape dont fill.
        /// </summary>
        public Color? FillColor
        {
            get
            {
                return fillColor;

            }
            set
            {
                fillColor = value;
            }
        }

        internal xPFT.Charting.Base.FillPattern fillPattern;
        /// <summary>
        /// Get the fill pattern.
        /// </summary>
        public xPFT.Charting.Base.FillPattern FillPattern
        {
            get
            {
                return fillPattern;
            }
            set
            {
                fillPattern = value;
            }
        }

        internal float patternSize = 8;
        /// <summary>
        /// Get the fill pattern size.
        /// </summary>
        public float PatternSize
        {
            get
            {
                return patternSize;
            }
            set
            {
                patternSize = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create an annotation.
        /// </summary>
        /// <param name="chA"></param>
        public void CreateAnnotationShape(ChartArea chA)
        {
            ParentControl = chA;
        }

        /// <summary>
        /// Run when mouse up on the chart area.
        /// </summary>
        private void AnnotationShape_MouseUp()
        {
            isSelected = false;
        }

        /// <summary>
        /// Reset the annotation position.
        /// </summary>
        public override void Reposition()
        {
            if (horizontalAxis != null && verticalAxis != null)
            {
                rectangle.X = startValuePoint.X * horizontalAxis.WidthRate + ParentControl.Padding.Left + horizontalAxis.Center;
                rectangle.Y = -startValuePoint.Y * verticalAxis.HeightRate + ParentControl.Padding.Top + verticalAxis.Center;
                rectangle.Height = -(endValuePoint.Y - startValuePoint.Y) * verticalAxis.HeightRate;
                rectangle.Width = (endValuePoint.X - startValuePoint.X) * horizontalAxis.WidthRate;
            }
            else
            {
                rectangle.X = startValuePoint.X * ParentControl.DrawWidth + ParentControl.Padding.Left;
                rectangle.Y = (1 - startValuePoint.Y) * ParentControl.DrawHeight + ParentControl.Padding.Top;
                rectangle.Height = -(endValuePoint.Y - startValuePoint.Y) * ParentControl.DrawHeight;
                rectangle.Width = (endValuePoint.X - startValuePoint.X) * ParentControl.DrawWidth;
            }
            if (rectangle.Width < 0)
            {
                rectangle.Width *= -1;
                rectangle.X -= rectangle.Width;
            }
            if (rectangle.Height < 0)
            {
                rectangle.Height *= -1;
                rectangle.Y -= rectangle.Height;
            }

            ParentControl.ReDrawLayer(DrawingLayer);
        }

        /// <summary>
        /// Determine is the point on the annotation or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal override bool IsMouseOnTheElement(System.Drawing.PointF point)
        {
            return rectangle.Contains(point);
        }

        public override void Draw(int layerIndex)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
