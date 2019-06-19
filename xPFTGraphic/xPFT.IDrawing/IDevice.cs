/*********************************************************************
*  Device.cs
*  Implementation of the Class Device
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
    public interface IDevice
    {
        /// <summary>
        /// Initialize the device for drawing on the control
        /// </summary>
        /// <param name="control"></param>
        void Initialize(System.Windows.Forms.Control control);
        /// <summary>
        /// Dispose the device.
        /// </summary>
        void Dispose();
        /// <summary>
        /// Clear the device.
        /// </summary>
        /// <param name="color"></param>
        void Clear(Color color);
        /// <summary>
        /// Set Transform of the device.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        void SetTransform(float w, float h);
        /// <summary>
        /// Get is device disposed or not
        /// </summary>
        bool IsDisposed { get; }
        /// <summary>
        /// Begin drawing on the layer with layerIndex index.
        /// </summary>
        /// <param name="layerIndex"></param>
        void BeginLayerDraw(int layerIndex);
        /// <summary>
        /// End drawing on the layer with layerIndex index.
        /// </summary>
        /// <param name="layerIndex"></param>
        void EndLayerDraw(int layerIndex);
        /// <summary>
        /// Clear the layer with layerIndex index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="color"></param>
        void ClearLayer(int layerIndex, Color color);
        /// <summary>
        /// present the device.
        /// </summary>
        void Present();
    }
}

