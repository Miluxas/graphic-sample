
/*********************************************************************
 *  Device.cs
 *  Implementation of the Class Device
 *  Created on:      15-Nov-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/
using SharpDX.Direct2D1;
using System.Windows.Forms;
using xPFT.IDrawing;

namespace xPFT.GraphicEngineD2D1
{
    public class Device: IDevice
    {
        public Device()
        {
            factory = new Factory();
        }

        #region Fields
        internal Factory factory;
        internal SharpDX.Direct2D1.RenderTarget renderTarget;
        internal System.Collections.Generic.List<BitmapRenderTarget> layers = new System.Collections.Generic.List<BitmapRenderTarget>();
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
            if (!renderTarget.IsDisposed)
            renderTarget.Clear(new  SharpDX.Color4(color.ToArgb()));
        }

        /// <summary>
        /// Set the device transform.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void SetTransform(float w, float h)
        {
            renderTarget.Transform=SharpDX.Matrix.Translation(w, h, 0);
        }
      
        /// <summary>
        /// Initialize the device on the control.
        /// </summary>
        /// <param name="control"></param>
        public void Initialize(Control control)
        {
            
            padding = control.Padding;
            size = control.Size;
            var renderTargetProperties = new RenderTargetProperties()
            {
                DpiX = 96,  
                DpiY = 96,
                MinLevel = FeatureLevel.Level_9,
                Type = RenderTargetType.Hardware,
                Usage = RenderTargetUsage.None
            };

            var windowsRenderTargetPropertiesPointer = new HwndRenderTargetProperties()
            {
                Hwnd = control.Handle,
                PixelSize = new SharpDX.Size2(control.ClientSize.Width, control.ClientSize.Height),
                PresentOptions = PresentOptions.Immediately
            };

            if (renderTarget != null)
            {
                ((WindowRenderTarget)renderTarget).Resize(new SharpDX.Size2(control.ClientSize.Width, control.ClientSize.Height));
                for (int i = 0; i < layers.Count; i++)
                {
                    if (layers[i] != null)
                        layers[i].Dispose();
                    if (i == 5)
                    {
                        float w = control.ClientSize.Width - control.Padding.Left - control.Padding.Right;
                        float h = control.ClientSize.Height - control.Padding.Top - control.Padding.Bottom;
                        if (w < 1) w = 50;
                        if (h < 1) h = 50;
                        layers[i] = new BitmapRenderTarget(renderTarget, CompatibleRenderTargetOptions.None,
                            new SharpDX.Size2F(w, h));
                    }
                    else
                    {
                        layers[i] = new BitmapRenderTarget(renderTarget, CompatibleRenderTargetOptions.None);
                    }
                }
            }
            else
            {
                windowsRenderTargetPropertiesPointer.PixelSize = new SharpDX.Size2(control.ClientSize.Width, control.ClientSize.Height);
                renderTarget = new WindowRenderTarget(factory, renderTargetProperties, windowsRenderTargetPropertiesPointer);
            }
        }

        /// <summary>
        /// Begin drawing layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public void BeginLayerDraw(int layerIndex)
        {
            if(renderTarget!=null)
            if (layerIndex >= layers.Count)
            {
                if (layerIndex == 5)
                {
                    float w = size.Width - padding.Left - padding.Right;
                    float h = size.Height - padding.Top - padding.Bottom;
                    if (w < 1) w = 50;
                    if (h < 1) h = 50;
                    layers.Add(new BitmapRenderTarget(renderTarget, CompatibleRenderTargetOptions.None,
                        new SharpDX.Size2F(w,h)));
                }
                else
                    layers.Add(new BitmapRenderTarget(renderTarget, CompatibleRenderTargetOptions.None));
            }
            if (layerIndex < layers.Count)
            {
                layers[layerIndex].BeginDraw();
                layers[layerIndex].Clear(SharpDX.Color.Transparent);
            }
        }

        /// <summary>
        /// End Drawing Layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public void EndLayerDraw(int layerIndex)
        {
            if (layerIndex< layers.Count)
            layers[layerIndex].EndDraw();
        }

        /// <summary>
        /// Present the device.
        /// </summary>
        public void Present()
        {
            if (renderTarget != null)
            {
                renderTarget.BeginDraw();
                for (int i = 0; i < layers.Count; i++)
                {
                    if (i == 5)
                        renderTarget.Transform = SharpDX.Matrix.Translation(padding.Left, padding.Top, 0);
                    renderTarget.DrawBitmap(layers[i].Bitmap, 1, BitmapInterpolationMode.Linear);
                    renderTarget.Transform = SharpDX.Matrix.Identity;
                }
                renderTarget.EndDraw();
            }
        }

        /// <summary>
        /// Clear Layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="color"></param>
        public void ClearLayer(int layerIndex, System.Drawing.Color color)
        {
            if (layerIndex < layers.Count)
                layers[layerIndex].Clear(DrawingBase.Convertor.ColorConvertor(color));
        }

        #endregion

    }
}
