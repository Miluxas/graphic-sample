
/*********************************************************************
 *  ILine.cs
 *  Implementation of the Class ILine
 *  Created on:      15-Nov-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPFT.IDrawing
{
    public interface ILine
    {
        /// <summary>
        /// Draw line on one of the layers of device.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        System.Drawing.Point Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color color, float opacity = 1);
        /// <summary>
        /// Draw line on one of the layers of device.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="color"></param>
        /// <param name="wScale"></param>
        /// <param name="hScale"></param>
        /// <param name="opacity"></param>
        System.Drawing.Point Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color color, float wScale, float hScale, float opacity = 1);
        /// <summary>
        /// Draw line on one of the layers of device.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="xAxesCenterHeight"></param>
        /// <param name="yAxesCenterWidth"></param>
        /// <param name="HeightRate"></param>
        /// <param name="WidthRate"></param>
        /// <param name="xAxesMaxValue"></param>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        /// <param name="autoShift"></param>
        /// <param name="keepInRightOfYAxis"></param>
        System.Drawing.Point Draw(int layerIndex, xPFT.Charting.Base.DataPointCollection points, float xAxesCenterHeight, float yAxesCenterWidth, float HeightRate, float WidthRate, float xAxesMaxValue, System.Drawing.Color color, float opacity = 1, bool autoShift = true, bool keepInRightOfYAxis = false);
        /// <summary>
        /// Draw series of point on one of the layers of device.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="xAxesCenterHeight"></param>
        /// <param name="yAxesCenterWidth"></param>
        /// <param name="HeightRate"></param>
        /// <param name="WidthRate"></param>
        /// <param name="xAxesMaxValue"></param>
        /// <param name="color"></param>
        /// <param name="pointType"></param>
        /// <param name="opacity"></param>
        /// <param name="autoShift"></param>
        /// <param name="keepInRightOfYAxis"></param>
        System.Drawing.Point DrawPoints(int layerIndex, xPFT.Charting.Base.DataPointCollection points, float xAxesCenterHeight, float yAxesCenterWidth, float HeightRate, float WidthRate, float xAxesMaxValue, System.Drawing.Color color, int pointType = 0, float opacity = 1, bool autoShift = true, bool keepInRightOfYAxis = false,bool isDrawPointLabel=false);
        /// <summary>
        /// Draw series of point on one of the layers of device.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        /// <param name="pointType"></param>
        System.Drawing.Point DrawPoints(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color color, float opacity = 1, int pointType = 0);
        /// <summary>
        /// Dispose the Line.
        /// </summary>
        void Dispose();
        /// <summary>
        /// Get or Set the width of the line.
        /// </summary>
        float Width { get; set; }
        /// <summary>
        /// Set the pattern of line.
        /// </summary>
        xPFT.Charting.Base.LineDrawPattern Pattern { set; }
        /// <summary>
        /// Get or Set the line Anti alias.
        /// </summary>
        Boolean Antialias { get; set; }
        /// <summary>
        /// Get the is lien disposed or not.
        /// </summary>
        Boolean IsDisposed { get; }
    }
}
