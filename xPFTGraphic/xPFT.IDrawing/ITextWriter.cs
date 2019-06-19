
/*********************************************************************
 *  ITextWriter.cs
 *  Implementation of the Class ITextWriter
 *  Created on:      15-Nov-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace xPFT.IDrawing
{
    public interface ITextWriter
    {
        /// <summary>
        /// Dispose the textwriter.
        /// </summary>
        void Dispose();
        /// <summary>
        /// Draw text on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="Text"></param>
        /// <param name="rectangle"></param>
        /// <param name="fontDrawFlag"></param>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        /// <param name="RotateAngle"></param>
        void Draw(int layerIndex, string Text, Rectangle rectangle, xPFT.Charting.Base.FontDrawFlags fontDrawFlag, Color color, float opacity, float RotateAngle);
    }
}
