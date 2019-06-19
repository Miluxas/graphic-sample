using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.IDrawing;
using System.Drawing;

namespace xPFT.GraphicEngineD3D9
{
    public class Texture:ITexture
    {
        #region Constructor
        public Texture(IDevice device, System.Drawing.Bitmap image)
        {
            this.device = (Device)device;
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            texture = SharpDX.Direct3D9.Texture.FromMemory(((Device)device).device, (byte[])converter.ConvertTo(image, typeof(byte[])),
            (int)(image.Width), (int)(image.Height), 1, SharpDX.Direct3D9.Usage.None, SharpDX.Direct3D9.Format.A8R8G8B8, SharpDX.Direct3D9.Pool.Default, SharpDX.Direct3D9.Filter.Linear, SharpDX.Direct3D9.Filter.Linear, 0);
            imageSize = image.Size;
            sprite = new SharpDX.Direct3D9.Sprite(this.device.device);
        }
        #endregion

        #region Fields

        internal SharpDX.Direct3D9.Texture texture;
        internal SharpDX.Direct3D9.Sprite sprite;
        internal Device device;
        
        Size imageSize = new Size();
        #endregion

        #region Methods
        /// <summary>
        /// dispose the texture.
        /// </summary>
        public void Dispose()
        {
            if(sprite!=null)
                sprite.Dispose();
            if(texture!=null)
                texture.Dispose();
        }

        /// <summary>
        /// Load image to the texture.
        /// </summary>
        /// <param name="image"></param>
        public void LoadImage(System.Drawing.Bitmap image)
        {
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            texture = SharpDX.Direct3D9.Texture.FromMemory(device.device, (byte[])converter.ConvertTo(image, typeof(byte[])),
            (int)(image.Width), (int)(image.Height), 1, SharpDX.Direct3D9.Usage.None, SharpDX.Direct3D9.Format.A8R8G8B8, SharpDX.Direct3D9.Pool.Default, SharpDX.Direct3D9.Filter.Linear, SharpDX.Direct3D9.Filter.Linear, 0);
            imageSize = image.Size;
        }

        /// <summary>
        /// Draw image on the location.
        /// </summary>
        /// <param name="location"></param>
        public void Draw(int layerIndex, System.Drawing.PointF location)
        {
            sprite.Begin(SharpDX.Direct3D9.SpriteFlags.AlphaBlend);
            sprite.Transform = SharpDX.Matrix.Translation(new SharpDX.Vector3(location.X, location.Y, 0));
            sprite.Draw(texture, SharpDX.Color.White);
            sprite.End();
        }

        /// <summary>
        /// Draw image on the location with the rotation and vertical mirror.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="rotateAngle"></param>
        /// <param name="rotateOrigin"></param>
        /// <param name="isVerticalMirror"></param>
        public void Draw(int layerIndex, System.Drawing.PointF location, float rotateAngle, System.Drawing.PointF rotateOrigin, bool isVerticalMirror)
        {
            sprite.Begin(SharpDX.Direct3D9.SpriteFlags.AlphaBlend);
            if (rotateAngle != 0)
            {
                float tempX = imageSize.Width * rotateOrigin.X;
                float tempY = imageSize.Height * rotateOrigin.Y;
                sprite.Transform = SharpDX.Matrix.Translation(new SharpDX.Vector3(-tempX, -tempY, 0)) *
                            SharpDX.Matrix.RotationZ(rotateAngle * (float)Math.PI * 4) *
                           SharpDX.Matrix.Translation(new SharpDX.Vector3(tempX, tempY, 0)) *
                           SharpDX.Matrix.Translation(new SharpDX.Vector3( location.X,location.Y,0));
            }
            else
            {
                  if (!isVerticalMirror)
                sprite.Transform = SharpDX.Matrix.Translation(new SharpDX.Vector3(location.X, location.Y, 0));
                else
                      sprite.Transform = SharpDX.Matrix.Translation(-imageSize.Width, 0, 0) * SharpDX.Matrix.Scaling(-1, 1, 0)
                          * SharpDX.Matrix.Translation(new SharpDX.Vector3(location.X, location.Y, 0));
            }
            sprite.Draw(texture, SharpDX.Color.White);
            sprite.End();
        }
        #endregion
    }
}
