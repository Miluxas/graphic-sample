/*********************************************************************
 *  TextWriter.cs
 *  For Draw a text in the GDI graphics.
 *  Created on:      11-Mar-2015 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace xPFT.GraphicEngineGDI
{
    public class TextWriter
    {
        /// <summary>
        /// Draw the text on the graphics.
        /// </summary>
        public static void Draw(Graphics g,Point pos, string Text, System.Drawing.Rectangle rectangle, xPFT.Charting.Base.FontDrawFlags fontDrawFlag, System.Drawing.Color color, float opacity, float RotateAngle, System.Drawing.Font font)
        {
            //! Set the String Format.
            StringFormat sf = new StringFormat();
            if (fontDrawFlag == xPFT.Charting.Base.FontDrawFlags.Left)
                sf.Alignment = StringAlignment.Near;
            else if (fontDrawFlag == xPFT.Charting.Base.FontDrawFlags.Right)
                sf.Alignment = StringAlignment.Far;
            else if (fontDrawFlag == xPFT.Charting.Base.FontDrawFlags.Center)
                sf.Alignment = StringAlignment.Center;
            //! Create A font from parent control font.
            rectangle.Height = (int)(font.SizeInPoints * 1.5);
            //! Translate the text.
            g.TranslateTransform(rectangle.X+pos.X, rectangle.Y+pos.Y);
            //! Rotate the text.
            if (RotateAngle != 0)
            {
              //  g.TranslateTransform(-rectangle.Width / 2 - rectangle.Height / 2, 0);
                g.RotateTransform((float)(RotateAngle * 180 / Math.PI));
            }
            System.Drawing.Font newFont = new System.Drawing.Font(font.FontFamily, font.Size * 0.8f, font.Style);
            rectangle.Height = (int)(newFont.SizeInPoints * 1.5);
            //! Draw the text.
            g.DrawString(Text, newFont, new SolidBrush(BrushMaker.SetOpacity(color, opacity)), new RectangleF(0, 0, rectangle.Width, rectangle.Height), sf);
            //! Reset the transform.
            g.ResetTransform();
        }
    }
}
