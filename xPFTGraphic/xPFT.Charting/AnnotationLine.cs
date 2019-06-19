/*********************************************************************
 *  AnnotationLine.cs
 *  Implementation of the Class AnnotationLine
 *  Created on:      19-Oct-2014 15:45:56 
 *  Original author: Teb Tasvir
 *  
 * Update 2014/10/25 Update#2 *****************************************
 * Add code to check content should dispose before re initialize.
 *
 * Update 2014/11/16 Update#3 *****************************************
 * Change code according the new layering design.
 **********************************************************************/

using System;
using System.Drawing;
using System.Collections.Generic;

namespace xPFT.Charting
{
    public abstract class AnnotationLine : Annotation
    {
        #region Fields
        protected const int TEXT_RECTANGLE_WIDTH = 100;
        protected const int TEXT_RECTANGLE_HEIGHT = 20;
       
        IDrawing.ILine line;
        /// <summary>
        /// start and end point of the line.
        /// </summary>
        protected PointF[] points = new PointF[2];

        protected Rectangle textRectangle;
        #endregion

        float mouseHitRay = 10;
        /// <summary>
        /// Get or Set the float that determine mouse event active in how annotation line distance.
        /// </summary>
        public float MouseHitRay
        {
            get
            {
                return mouseHitRay;
            }
            set
            {
                mouseHitRay = value;
            }
        }

        internal float value = 0;
        /// <summary>
        /// Get the position of the line in Chart area or relate value in Axes.
        /// </summary>
        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        #region Methods
        /// <summary>
        /// Dispose the annotaion line.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (line != null) line.Dispose();
        }

        /// <summary>
        /// Draw the annotation line on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public override void Draw(int layerIndex)
        {
            if (line != null)
            {
                try
                {
                    line.Pattern = LinePattern;
                    line.Width = LineWidth;
                    line.Draw(layerIndex, points, (Color)LineColor, Opacity);
                }
                catch { }
                try
                {
                    textWriter.Draw(layerIndex, Text, textRectangle, xPFT.Charting.Base.FontDrawFlags.Left, (Color)LineColor, Opacity, 0);
                }
                catch { }
            }
        }

        /// <summary>
        /// Draw the annotaion line on the graphics.
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g ,Point pos)
        {
            xPFT.GraphicEngineGDI.Line.Draw(g,pos, points, (Color)LineColor, Opacity, LineWidth,LinePattern);
            xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos, Text, textRectangle, xPFT.Charting.Base.FontDrawFlags.Left, (Color)LineColor, Opacity, 0, Font);
        }

        /// <summary>
        /// Initialize the line on the device.
        /// </summary>
        /// <param name="device"></param>
        internal override void Initialize(IDrawing.IDevice device)
        {
            base.Initialize(device);
            try
            {
                this.device = device;

                #region Update#2
                if (line != null)
                {
                    line.Dispose();
                    line = null;
                }
                #endregion

                line = GraphicEngine.GraphicEngine.CreateLine(device,Font);
                line.Width = LineWidth;
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }


        internal override bool IsMouseOnTheElement(PointF point)
        {
            return false;
        }

        public override void Reposition()
        {
           // throw new NotImplementedException();
        } 
        #endregion
    }
}
