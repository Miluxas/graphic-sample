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

    public class TextAnnotation : Annotation
    {
        public TextAnnotation(ChartArea chartArea)
        {
            ParentControl = chartArea;
        }

        /// <summary>
        /// The rectangle that string drawing in it.
        /// </summary>
        Rectangle textRectangle = new Rectangle();
        /// <summary>
        /// rectangle size.
        /// </summary>
        SizeF rSize = new SizeF(1500, 200);
        /// <summary>
        /// Relate value of the text x and y.
        /// </summary>
        float relateRateX = 1, relateRateY = 1;

        /// <summary>
        /// Get or Set the position of the text.
        /// </summary>
        public PointF Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                relateRateX = position.X / ParentControl.Width;
                relateRateY = position.Y / ParentControl.Height;
            }
        }

        string text = "";
        /// <summary>
        /// Get or Set the text.
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

        bool isRelativePositionX = false;
        /// <summary>
        /// Set or Get the boolean that determine is text position x relate to the chart area with or not.
        /// </summary>
        public bool IsRelativePositionX 
        {
            get
            {
                return isRelativePositionX;
            }
            set
            {
                isRelativePositionX = value;
            }
        }

        bool isRelativePositionY = false;
        /// <summary>
        /// Set or Get the boolean that determine is text position y relate to the chart area height or not.
        /// </summary>
        public bool IsRelativePositionY
        {
            get
            {
                return isRelativePositionY;
            }
            set
            {
                isRelativePositionY = value;
            }
        }

        xPFT.Charting.Base.FontDrawFlags align;
        /// <summary>
        /// Get or Set the align. 
        /// </summary>
        public xPFT.Charting.Base.FontDrawFlags Align
        {
            get
            {
                return align;
            }
            set
            {
                align = value;
            }
        }

        Font textFont;
        /// <summary>
        /// Get or Set the text font.
        /// </summary>
        public Font TextFont
        {
            get
            {
                return textFont;
            }
            set
            {
                textFont = value;
                if (device != null)
                {
                    if (textWriter != null)
                        textWriter.Dispose();
                    textWriter = GraphicEngine.GraphicEngine.CreateTextWriter(device, textFont);
                }
            }
        }

        /// <summary>
        /// Initialize the test annotation on the device.
        /// </summary>
        /// <param name="device"></param>
        internal override void Initialize(IDrawing.IDevice device)
        {
            try
            {
                this.device = device;
                if (device != null)
                {
                    if (textWriter != null)
                        textWriter.Dispose();
                    if (textFont == null)
                        textFont = Font;
                    textWriter = GraphicEngine.GraphicEngine.CreateTextWriter(device, textFont);
                }
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
         }

        /// <summary>
        /// Draw the annotation text annotation on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public override void Draw(int layerIndex)
        {
            if (textWriter != null)
            { 
                Rectangle rec = new Rectangle(textRectangle.Location, textRectangle.Size);
                if (align == xPFT.Charting.Base.FontDrawFlags.Left)
                {
                    rec.X -= textRectangle.Width;
                    textWriter.Draw(layerIndex, Text, rec, xPFT.Charting.Base.FontDrawFlags.Right, (Color)LineColor, Opacity, 0);
                }
                else if (align == xPFT.Charting.Base.FontDrawFlags.Right)
                {
                    textWriter.Draw(layerIndex, Text, rec, xPFT.Charting.Base.FontDrawFlags.Left, (Color)LineColor, Opacity, 0);
                }
                else
                {
                    rec.X -= textRectangle.Width / 2;
                    textWriter.Draw(layerIndex, Text, rec, xPFT.Charting.Base.FontDrawFlags.Center, (Color)LineColor, Opacity, 0);
                }
            }
        }

        /// <summary>
        /// Draw the annotaion text annotation on the graphics.
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics graphic ,Point pos)
        {
            Rectangle rec = new Rectangle(textRectangle.Location, textRectangle.Size);
            if (align == xPFT.Charting.Base.FontDrawFlags.Left)
            {
                rec.X -= textRectangle.Width;
                xPFT.GraphicEngineGDI.TextWriter.Draw(graphic, pos, Text, rec, xPFT.Charting.Base.FontDrawFlags.Right, (Color)LineColor, Opacity, 0, TextFont);
            }
            else if (align == xPFT.Charting.Base.FontDrawFlags.Right)
            {
                xPFT.GraphicEngineGDI.TextWriter.Draw(graphic, pos, Text, rec, xPFT.Charting.Base.FontDrawFlags.Left, (Color)LineColor, Opacity, 0, TextFont);
            }
            else
            {
                rec.X -= textRectangle.Width / 2;
                xPFT.GraphicEngineGDI.TextWriter.Draw(graphic, pos, Text, rec, xPFT.Charting.Base.FontDrawFlags.Center, (Color)LineColor, Opacity, 0, TextFont);
            }

        }

        /// <summary>
        /// Reset the text annotation position.
        /// </summary>
        public override void Reposition()
        {
            if (horizontalAxis != null && verticalAxis != null)
            {
                textRectangle.X = (int)(Position.X * horizontalAxis.WidthRate + ParentControl.Padding.Left + horizontalAxis.Center);
                textRectangle.Y = (int)(-Position.Y * verticalAxis.HeightRate + ParentControl.Padding.Top + verticalAxis.Center);
            }
            else
            {
                if (IsRelativePositionX)
                    textRectangle.X = (int)(relateRateX * ParentControl.Width);     // (int)position.X;
                else
                    textRectangle.X = (int)position.X;
                if (IsRelativePositionX)
                    textRectangle.Y = (int)(relateRateY * ParentControl.Height);     // (int)position.X;
                else
                    textRectangle.Y = (int)position.Y;

            }
            textRectangle.Height = (int)rSize.Height;
            textRectangle.Width = (int)rSize.Width;

            ParentControl.ReDrawLayer(DrawingLayer);
        }

        /// <summary>
        /// Get is mouse on the text annotation or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal override bool IsMouseOnTheElement(PointF point)
        {
            RectangleF tm2 = new RectangleF(position.X, position.Y, rSize.Width, rSize.Height);
            return tm2.Contains(point);
        }

        /// <summary>
        /// Dispose the polygon annotaion.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (textWriter != null)
            {
                textWriter.Dispose();
                textWriter = null;
            }
        }
    }
}
