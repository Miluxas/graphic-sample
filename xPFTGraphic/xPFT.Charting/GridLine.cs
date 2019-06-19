/***************************************************************************
 *  GridLine.cs
 *  Implementation of the Class GridLine
 *  Created on:      11-Oct-2014 12:57:32 PM
 *  Original author: Teb Tasvir
 *  
 *  Update 2014/10/15 *****************************************************
 *  Add comment to this code.
 * 
 *  Update 2014/11/16 Update#3 *****************************************
 *  Change code according the new layering design.
 **************************************************************************/

using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace xPFT.Charting
{
    class GridLine
    {
        #region Constructor

        public GridLine(ChartArea chartArea,Color color)
        {
            this.chartArea = chartArea;
            line = GraphicEngine.GraphicEngine.CreateLine(chartArea.device,chartArea.Font);
            line.Width = 1;
            line.Antialias = true;
            this.color = color;
        }

        #endregion

        #region Fields
        /// <summary>
        /// Line of the grid.
        /// </summary>
        public IDrawing.ILine line;
        /// <summary>
        /// Chart area that grid show on it.
        /// </summary>
        private ChartArea chartArea;
        /// <summary>
        /// Start and End point of the line.
        /// </summary>
        private PointF[] points = new PointF[2];
        /// <summary>
        /// Color of the line.
        /// </summary>
        Color color;
        #endregion

        #region Methods
        /// <summary>
        /// Reposition the line accourding the axes label.
        /// </summary>
        /// <param name="axesLabel"></param>
        public void Reposition(AxisLabel axesLabel)
        {
            if (axesLabel.AxisSide == AxisDirection.Horizontal )
            {
                points[0].X = points[1].X = axesLabel.Position.X;
                points[0].Y = chartArea.Height- chartArea.Padding.Bottom;
                points[1].Y = chartArea.Padding.Top;
            }
            else
            {
                points[0].Y = points[1].Y = axesLabel.Position.Y;
                points[0].X = chartArea.Width - chartArea.Padding.Right;
                points[1].X = chartArea.Padding.Left;
            }
        }

        /// <summary>
        /// Draw the Grid line.
        /// </summary>
        public void Draw(int layerIndex)
        {
            try
            {
                line.Draw(layerIndex, points, color);
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        public void Draw(Graphics g,Point pos)
        {
            xPFT.GraphicEngineGDI.Line.Draw(g, pos, points, color);
        }

        /// <summary>
        /// Dispose line
        /// </summary>
        public void Dispose()
        {
            if(line!=null)
            {
                line.Dispose();
                line = null;
            }

        }

        #endregion

    }
}
