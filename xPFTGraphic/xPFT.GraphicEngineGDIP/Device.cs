
/*********************************************************************
 *  Device.cs
 *  Implementation of the Class Device
 *  Created on:      15-Nov-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using xPFT.IDrawing;

namespace xPFT.GraphicEngineGDIP
{
    public class Device: IDevice
    {
        public Device()
        {
        }

        #region Fields
        internal System.Drawing.Graphics renderTarget;
        //Graphics tempGraphics;
        Control contr;
        internal System.Collections.Generic.List<Image> layers = new System.Collections.Generic.List<Image>();
        internal Padding padding;
        System.Drawing.Size size;
        #endregion

        #region Properties
        /// <summary>
        /// Get is device disposed or not.
        /// </summary>
        public bool IsDisposed
        {
            get { return (renderTarget == null); }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Dispose the device.
        /// </summary>
        public void Dispose()
        {
            renderTarget.Dispose();
            renderTarget = null;
        }

        /// <summary>
        /// Clear the device with a color.
        /// </summary>
        /// <param name="color"></param>
        public void Clear(System.Drawing.Color color)
        {
            renderTarget.Clear(color);
        }

        /// <summary>
        /// Set the device transform.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void SetTransform(float w, float h)
        {
            renderTarget.TranslateTransform(w, h);
        }
      
        /// <summary>
        /// Initialize the device on the control.
        /// </summary>
        /// <param name="control"></param>
        public void Initialize(Control control)
        {
            
            padding = control.Padding;
            size = control.Size;
            contr = control;
            //var renderTargetProperties = new RenderTargetProperties()
            //{
            //    DpiX = 96,  
            //    DpiY = 96,
            //    MinLevel = FeatureLevel.Level_10,
            //    Type = RenderTargetType.Hardware,
            //    Usage = RenderTargetUsage.None
            //};

            //var windowsRenderTargetPropertiesPointer = new HwndRenderTargetProperties()
            //{
            //    Hwnd = control.Handle,
            //    PixelSize = new SharpDX.Size2(control.ClientSize.Width, control.ClientSize.Height),
            //    PresentOptions = PresentOptions.Immediately
            //};

            if (renderTarget != null)
            {
                //renderTarget.re
                //((WindowRenderTarget)renderTarget).Resize(new SharpDX.Size2(control.ClientSize.Width, control.ClientSize.Height));
                //for (int i = 0; i < layers.Count; i++)
                //    if (i == 5)
                //    {
                //        float w = control.ClientSize.Width - control.Padding.Left - control.Padding.Right;
                //        float h = control.ClientSize.Height - control.Padding.Top - control.Padding.Bottom;
                //        if (w < 1) w = 50;
                //        if (h < 1) h = 50;
                //        layers[i] = new BitmapRenderTarget(renderTarget, CompatibleRenderTargetOptions.None,
                //            new SharpDX.Size2F(w,h));
                //    }
                //    else
                //    {
                //        layers[i] = new BitmapRenderTarget(renderTarget, CompatibleRenderTargetOptions.None);
                //    }
                renderTarget.Dispose();
                renderTarget = control.CreateGraphics();
            }
            else
            {
                renderTarget = control.CreateGraphics();
            }
            renderTarget.SmoothingMode = SmoothingMode.AntiAlias;
            renderTarget.Clear(contr.BackColor);
        }

        /// <summary>
        /// Begin drawing layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public void BeginLayerDraw(int layerIndex)
        {
            //if (layerIndex >= layers.Count)
            //{
            //renderTarget.Clear(contr.BackColor);
            if (layerIndex == 5)
            {
                renderTarget.Clip = new Region(new System.Drawing.Rectangle(padding.Left, padding.Top, size.Width - 2 * padding.Left, size.Height - 2 * padding.Top));
                renderTarget.TranslateTransform(padding.Left, 0);
            }
            
            //    {
            //        int w = size.Width - padding.Left - padding.Right;
            //        int h = size.Height - padding.Top - padding.Bottom;
            //        if (w < 1) w = 50;
            //        if (h < 1) h = 50;
                   
            //        layers.Add(new Bitmap(w, h,renderTarget));
            //    }
            //    else
            //        layers.Add(new Bitmap(size.Width, size.Height));
            //}
            ////if (layerIndex < layers.Count)
            ////{
            ////    //    layers[layerIndex].BeginDraw();
            ////    //    layers[layerIndex].Clear(SharpDX.Color.Transparent);
            ////    //}
            //if (layerIndex < layers.Count)
            //{
            //    renderTarget = Graphics.FromImage(layers[layerIndex]);
            //    renderTarget.SmoothingMode = SmoothingMode.AntiAlias;
            //    renderTarget.Clear(Color.Transparent);
            //    renderTarget.InterpolationMode = InterpolationMode.High;
            //}
        }

        /// <summary>
        /// End Drawing Layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public void EndLayerDraw(int layerIndex)
        {
                renderTarget.ResetTransform();
            //if (layerIndex< layers.Count)
            //layers[layerIndex].EndDraw();
        }

        /// <summary>
        /// Present the device.
        /// </summary>
        public void Present()
        {
          //  renderTarget.Clear(contr.BackColor);
           // Image im = layers[0] + layers[1];

           // renderTarget = contr.CreateGraphics();
           //// renderTarget.InterpolationMode = InterpolationMode.Invalid;
           // renderTarget.Clear(contr.BackColor);
           // for (int i = 0; i < layers.Count; i++)
           // {
           //     if (i == 5)
           //         renderTarget.DrawImageUnscaled(layers[i], new Point(padding.Left, padding.Top));
           //     else
           //         renderTarget.DrawImageUnscaled(layers[i], new Point());
           // }
        }

        /// <summary>
        /// Clear Layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="color"></param>
        public void ClearLayer(int layerIndex, System.Drawing.Color color)
        {
              // renderTarget.Clear(contr.BackColor);
        }

        #endregion

    }
}
