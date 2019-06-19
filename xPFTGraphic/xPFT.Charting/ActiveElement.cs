/*********************************************************************
 *  ActiveElement.cs
 *  Implementation of the Class ActiveElement
 *  Created on:      19-Oct-2014 15:45:56 
 *  Original author: Teb Tasvir
 *  
 * Update 2014/11/16 Update#3 *****************************************
 * Change code according the new layering design.
 * 
 **********************************************************************/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;

namespace xPFT.Charting
{
    public abstract class ActiveElement
    {
        public delegate void EventHandler(ActiveElement sender, System.Windows.Forms.MouseEventArgs e);
        
        /// <summary>
        /// Mouse down event handler.
        /// </summary>
        public event EventHandler MouseDown;
        /// <summary>
        /// Mouse Up event handler
        /// </summary>
        public event EventHandler MouseUp;
        /// <summary>
        /// Mouse Move event handler.
        /// </summary>
        public event EventHandler MouseMove;
        /// <summary>
        /// Mouse Enter event handler.
        /// </summary>
        public event EventHandler MouseEnter;
        /// <summary>
        /// Mouse Leave event handler.
        /// </summary>
        public event EventHandler MouseLeave;
        /// <summary>
        /// Mouse Click event handler.
        /// </summary>
        public event EventHandler MouseClick;
        /// <summary>
        /// Mouse Double Click event handler.
        /// </summary>
        public event EventHandler MouseDoubleClick;
        /// <summary>
        /// get last mouse state.
        /// </summary>
        private bool lastMouseState = false;
        /// <summary>
        /// Parent chart area that axis belong to it.
        /// </summary>
        internal ChartArea ParentControl;

        bool isReactionEnable = true;
        /// <summary>
        /// Get or Set is this element re act to mouse action or not.
        /// </summary>
        public bool IsReactionEnable
        {
            get
            {
                return isReactionEnable;
            }
            set
            {
                isReactionEnable = value;
            }
        }

        Font font=null;
        /// <summary>
        /// Get or Set the font of the element.
        /// </summary>
        public Font Font
        {
            get
            {
                if (font == null)
                {
                    if (ParentControl != null)
                        return ParentControl.Font;
                    else
                        return new Font("arial", 10);
                }
                else
                    return font;
            }
            set
            {
                font = value;
            }
        }

        string name = "NoName";
        /// <summary>
        /// Get or Set the name of element.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        string text = " ";
        /// <summary>
        /// Get or Set the text of the element.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        Charting.Base.AnnotationDrawingLayer drawingLayer;
        /// <summary>
        /// Get or Set the layer of drawing.
        /// </summary>
        public Charting.Base.AnnotationDrawingLayer DrawingLayer
        {
            get
            {
                return drawingLayer;
            }
            set
            {
                drawingLayer = value;
            }
        }

        /// <summary>
        /// Get is point on the element or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal abstract bool IsMouseOnTheElement(PointF point);
        
        /// <summary>
        /// Dispose the Active element.
        /// </summary>
        public abstract void Dispose();
        
        /// <summary>
        /// Initialize a active element.
        /// </summary>
        /// <param name="device"></param>
        internal abstract void Initialize(IDrawing.IDevice device);

        /// <summary>
        /// Called this methods when the mouse down on the element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        internal void OnMouseDown(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsMouseOnTheElement(point))
                if (MouseDown != null)
                    MouseDown(this,e);
        }

        /// <summary>
        /// Called this methods when the mouse up on the element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        internal void OnMouseUp(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
                if (MouseUp != null)
                    MouseUp(this,e);
        }

        /// <summary>
        /// Called this methods when the mouse move on the element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        internal void OnMouseMove(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsMouseOnTheElement(point))
            {
                //! If mouse last state is false then
                if (!lastMouseState)
                    if (MouseEnter != null)
                        MouseEnter(this,e);
                if (MouseMove != null)
                    MouseMove(this,e);
            }
                //! If mouse last state is true then
            else
                if (lastMouseState)
                    if (MouseLeave != null)
                        MouseLeave(this,e);
            lastMouseState = IsMouseOnTheElement(point);
        }

        /// <summary>
        /// Called this methods when the mouse click on the element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        internal void OnMouseClick(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsMouseOnTheElement(point))
            if (MouseClick != null)
                MouseClick(this, e);
        }

        /// <summary>
        /// Called this methods when the mouse double click on the element.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        internal void OnMouseDoubleClick(PointF point, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsMouseOnTheElement(point))
                if (MouseDoubleClick != null)
                    MouseDoubleClick(this, e);
        }
    }
}
