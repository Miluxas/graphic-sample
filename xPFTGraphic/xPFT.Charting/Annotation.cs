/*********************************************************************
 *  Annonation.cs
 *  Implementation of the Class Annonation
 *  Created on:      19-Oct-2014 15:45:56 
 *  Original author: Teb Tasvir
 *  
 * Update 2014/10/25 Update#2 *****************************************
 * Add code to check content should dispose before re initialize.
 *
 * Update 2014/11/16 Update#3 *****************************************
 * Change code according the new layering design.
 **********************************************************************/

using System.Drawing;
using System.Collections.Generic;

namespace xPFT.Charting
{
    
    public abstract class Annotation : ActiveElement
    {
        public Annotation()
        {
            MouseLeave += Annotation_MouseLeave;
        }

        /// <summary>
        /// Run this method when mouse leave the annotation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Annotation_MouseLeave(ActiveElement sender, System.Windows.Forms.MouseEventArgs e)
        {
            bool isAnySelected = false;
            foreach (Annotation ann in ParentControl.annotations)
                if (ann.isSelected)
                    isAnySelected = true;
            if (!isAnySelected)
                ParentControl.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        #region Fields
        /// <summary>
        /// Position of the annotation.
        /// </summary>
        protected PointF position = new PointF(0, 0);
        /// <summary>
        /// Determine is this annotation selected or not.
        /// </summary>
        internal bool isSelected;
        /// <summary>
        /// The device that annotation is drawing on it.
        /// </summary>
        protected IDrawing.IDevice device;
        /// <summary>
        /// Font that write the label text.
        /// </summary>
        protected IDrawing.ITextWriter textWriter;
        #endregion

        #region Properties
        internal float opacity = 1;
        /// <summary>
        /// Get the opacity.
        /// </summary>
        public float Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;
            }
        }
        internal float lineWidth = 1;
        /// <summary>
        /// Get the line width. In shapes the boarder line width.
        /// </summary>
        public float LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
            }
        }
        
        internal Color? color = Color.Black;
        /// <summary>
        /// Get the color of the line. Default is black. If set with null the line don't draw.
        /// </summary>
        public Color? LineColor
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        internal xPFT.Charting.Base.LineDrawPattern linePattern = xPFT.Charting.Base.LineDrawPattern.SOLID;
        /// <summary>
        /// Get the pattern of the line.
        /// </summary>
        public xPFT.Charting.Base.LineDrawPattern LinePattern
        {
            get
            {
                return linePattern;
            }
            set
            {
                linePattern = value;
            }
        }
        

        internal Axis horizontalAxis;
        /// <summary>
        /// Get the horizontal Axis.
        /// </summary>
        public Axis HorizontalAxis
        {
            get
            {
                return horizontalAxis;
            }
            set
            {
                horizontalAxis = value;
            }
        }

        internal Axis verticalAxis;
        /// <summary>
        /// Get the vertical Axis.
        /// </summary>
        public Axis VerticalAxis
        {
            get
            {
                return verticalAxis;
            }
            set
            {
                verticalAxis = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draw the Annotation.
        /// </summary>
        abstract public void Draw(int layerIndex);
        abstract public void Draw(Graphics graphic,Point pos);
        /// <summary>
        /// Reset the Annotation position on the parent.
        /// </summary>
        abstract public void Reposition();
        
        /// <summary>
        /// Initialize the annotation drawer object. 
        /// </summary>
        /// <param name="device"></param>
        internal override void Initialize(IDrawing.IDevice device)
        {
            #region Update#2
            if (textWriter != null)
            {
                textWriter.Dispose();
                textWriter = null;
            }
            #endregion

            textWriter = GraphicEngine.GraphicEngine.CreateTextWriter(device,Font);
        }

        /// <summary>
        /// Dispose the annotation object.
        /// </summary>
        
        override public void Dispose()
        {
            if (textWriter != null)
            {
                textWriter.Dispose();
                textWriter = null;
            }
        }

        /// <summary>
        /// Re draw the parent chart area.
        /// </summary>
        protected void ReDraw()
        {
            if (ParentControl != null)
            {
                ParentControl.Draw();
            }
        }

        #endregion

    }
}
