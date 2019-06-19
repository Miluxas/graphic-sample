
/*********************************************************************
 *  Device.cs
 *  Implementation of the Class Device
 *  Created on:      15-Nov-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/

using System.Windows.Forms;
using xPFT.IDrawing;

namespace xPFT.GraphicEngineD3D9
{
    /// <summary>
    /// The class of the drawing layer.
    /// </summary>
    public class Layer
    {
        public Layer(SharpDX.Direct3D9.Texture texture)
        {
            this.texture = texture;
            this.surface = this.texture.GetSurfaceLevel(0);
        }

        /// <summary>
        /// The texture that contain layer.
        /// </summary>
        public SharpDX.Direct3D9.Texture texture;
        /// <summary>
        /// The surface of the texture.
        /// </summary>
        public SharpDX.Direct3D9.Surface surface;

        /// <summary>
        /// Dispose the layer.
        /// </summary>
        public void Dispose()
        {
            if (surface != null)
            {
                surface.Dispose();
                surface = null;
            }

            if (texture != null)
            {
                texture.Dispose();
                texture = null;
            }
        }
    }

    public class Device : IDevice
    {
        #region Fields
        internal SharpDX.Direct3D9.Device device;
        internal SharpDX.Direct3D9.Sprite sprite;
        internal Padding padding;
        internal System.Drawing.Size size;
        internal System.Collections.Generic.List<Layer> layers = new System.Collections.Generic.List<Layer>();
        SharpDX.Direct3D9.Surface mainSurface;
        #endregion

        #region Properties
        /// <summary>
        /// Get is device disposed or not.
        /// </summary>
        public bool IsDisposed
        {
            get { return device == null || device.IsDisposed; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Dispose the device.
        /// </summary>
        public void Dispose()
        {
            device.Dispose();
        }

        /// <summary>
        /// Clear the device with a color.
        /// </summary>
        /// <param name="color"></param>
        public void Clear(System.Drawing.Color color)
        {
            if (!device.IsDisposed)
                device.Clear(SharpDX.Direct3D9.ClearFlags.Target, SharpDX.Color.FromBgra(color.ToArgb()), 0, 0);
        }

        /// <summary>
        /// Set the device transform.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void SetTransform(float w, float h)
        {
            device.SetTransform(0, SharpDX.Matrix.Translation(w, h, 0));
        }

        /// <summary>
        /// Initialize the device on the control.
        /// </summary>
        /// <param name="control"></param>
        public void Initialize(Control control)
        {
            padding = control.Padding;
            size = control.Size;
            //! Dispose the layers.
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i] != null)
                {
                    layers[i].Dispose();
                }
            }
            //! Dispose the sprite.
            if (sprite != null)
            {
                sprite.Dispose();
                sprite = null;
            }
            //! Dispose the main surface.
            if (mainSurface != null)
            {
                mainSurface.Dispose();
                mainSurface = null;
            }
            System.GC.Collect();
            //! if device isn't null then reset the device size.
            if (device != null)
            {
                device.Reset(new SharpDX.Direct3D9.PresentParameters(control.ClientSize.Width, control.ClientSize.Height));
            }
            else
            {
                //! create a new device.
                device = new SharpDX.Direct3D9.Device(new SharpDX.Direct3D9.Direct3D(), 0, SharpDX.Direct3D9.DeviceType.Hardware, control.Handle,
                    SharpDX.Direct3D9.CreateFlags.HardwareVertexProcessing, new SharpDX.Direct3D9.PresentParameters(control.ClientSize.Width, control.ClientSize.Height));
            }
            //! create a new sprite.
            sprite = new SharpDX.Direct3D9.Sprite(device);
            //! Create layers.
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i] = new Layer(new SharpDX.Direct3D9.Texture(device, size.Width, size.Height, 1, SharpDX.Direct3D9.Usage.RenderTarget, SharpDX.Direct3D9.Format.A8R8G8B8, SharpDX.Direct3D9.Pool.Default));
            }
        }

        /// <summary>
        /// Begin drawing layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public void BeginLayerDraw(int layerIndex)
        {
            try
            {
                if (layerIndex >= layers.Count)
                {
                    SharpDX.Direct3D9.Texture tempTexture;
                    if (layerIndex == 5 && size.Width > padding.Left + padding.Right && size.Height > padding.Top + padding.Bottom)
                    {
                        tempTexture = new SharpDX.Direct3D9.Texture(device, size.Width - padding.Left - padding.Right, size.Height - padding.Top - padding.Bottom
                            , 1, SharpDX.Direct3D9.Usage.RenderTarget, SharpDX.Direct3D9.Format.A8R8G8B8, SharpDX.Direct3D9.Pool.Default);
                    }
                    else
                    {
                        tempTexture = new SharpDX.Direct3D9.Texture(device, size.Width, size.Height, 1, SharpDX.Direct3D9.Usage.RenderTarget, SharpDX.Direct3D9.Format.A8R8G8B8, SharpDX.Direct3D9.Pool.Default);
                    }
                    layers.Add(new Layer(tempTexture));
                }
                mainSurface = device.GetRenderTarget(0);
                device.SetRenderTarget(0, layers[layerIndex].surface);
                device.BeginScene();
            }
            catch (System.Exception ex)
            {
                //throw ex;
            }
        }

        /// <summary>
        /// End drawing layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        public void EndLayerDraw(int layerIndex)
        {
            try
            {
                device.EndScene();
                device.SetRenderTarget(0, mainSurface);
                mainSurface.Dispose();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Draw layers on the device and present that.
        /// </summary>
        public void Present()
        {
            if (device != null)
            {
                device.BeginScene();
                sprite.Begin(SharpDX.Direct3D9.SpriteFlags.AlphaBlend);
                for (int i = 0; i < layers.Count; i++)
                {
                    if (i == 5)
                        sprite.Transform = SharpDX.Matrix.Translation(padding.Left, padding.Top, 0);
                    sprite.Draw(layers[i].texture, SharpDX.Color.White);
                    if (i == 5)
                        sprite.Transform = SharpDX.Matrix.Identity;
                }
                sprite.End();
                device.EndScene();
                device.Present();
            }
        }

        /// <summary>
        /// Clear the layer with layerIndex index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="color"></param>
        public void ClearLayer(int layerIndex, System.Drawing.Color color)
        {
            if (!device.IsDisposed)
                device.Clear(SharpDX.Direct3D9.ClearFlags.Target, SharpDX.Color.FromBgra(color.ToArgb()), 0, 0);
        }
        #endregion
    }
}
