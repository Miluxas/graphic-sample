/***************************************************************************
 *  Grid.cs
 *  Implementation of the Class Grid
 *  Created on:      12-Oct-2014 10:38:10 AM
 *  Original author: Teb Tasvir
 *  
 *  Update 2014/10/15 *****************************************************
 *  Add comment to this code.
 * 
***************************************************************************/

using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace xPFT.Charting
{
    class Grid
    {
        #region Costructor
        public Grid(ChartArea chartArea)
        {
            this.chartArea = chartArea;
        }

        #endregion

        #region Fields
        /// <summary>
        /// Lines of the grid.
        /// </summary>
        private List<GridLine> gridLines = new List<GridLine>();
        /// <summary>
        /// The Chart area that grid is on it.
        /// </summary>
        private ChartArea chartArea;
        #endregion

        #region Properties

        public bool enabled = true;
        /// <summary>
        /// Get or Set is grid enable or not.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reposition the Gridlines.
        /// </summary>
        public void Reposition(AxisCollection Axes)
        {
            try
            {
                foreach (GridLine tmpGridLine in gridLines)
                {
                    tmpGridLine.line.Dispose();
                }
                gridLines.Clear();
                if (enabled)
                {
                    foreach(Axis axis in Axes)
                    if(axis.IsGridShow)
                    foreach (AxisLabel al in axis.labelsCollection)
                    {
                        GridLine gl = new GridLine(chartArea,axis.GridColor);
                        gridLines.Add(gl);
                        gl.Reposition(al);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Draw the GridLines on it's position.
        /// </summary>
        public void Draw(int layerIndex)
        {
            foreach (GridLine gl in gridLines)
                gl.Draw(layerIndex);
        }

        public void Draw(Graphics g,Point pos)
        {
            foreach (GridLine gl in gridLines)
                gl.Draw(g,pos);
        }
        
             /// <summary>
        /// Draw the GridLines on it's position.
        /// </summary>
        public void Dispose()
        {
            foreach (GridLine gl in gridLines)
                gl.Dispose();
        }

        #endregion
    }
}
