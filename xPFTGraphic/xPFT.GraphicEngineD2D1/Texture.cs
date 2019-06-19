using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct2D1;
using xPFT.IDrawing;


namespace xPFT.GraphicEngineD2D1
{
    public class Texture : ITexture
    {
        #region Constructor
        public Texture(IDevice device, System.Drawing.Bitmap image)
        {
            this.device = ((Device)device);
            bitmap = Convertor.ConvertDrawingBmpToDirectxBmp(((Device)device).renderTarget, image);
        }
        #endregion

        #region Fields
        Device device;
        Bitmap bitmap;

        #endregion

        #region Methods
     
        /// <summary>
        /// Draw image on the location.
        /// </summary>
        /// <param name="location"></param>
        public void Draw(int layerIndex, System.Drawing.PointF location)
        {
            device.layers[layerIndex].Transform = Matrix.Translation(location.X, location.Y, 0);
            device.layers[layerIndex].DrawBitmap(bitmap, 1, BitmapInterpolationMode.Linear);
            device.layers[layerIndex].Transform = Matrix.Identity;
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
            if (rotateAngle != 0)
            {
                float tempX = bitmap.Size.Width * rotateOrigin.X;
                float tempY = bitmap.Size.Height * rotateOrigin.Y;
                device.layers[layerIndex].Transform = Matrix.Translation(new Vector3(-tempX, -tempY, 0)) *
                            Matrix.RotationZ(rotateAngle * (float)Math.PI * 4) *
                           Matrix.Translation(new Vector3(tempX, tempY, 0)) *
                           Matrix.Translation(new Vector3(location.X, location.Y, 0));
            }
            else
            {

                if (!isVerticalMirror)
                    device.layers[layerIndex].Transform = SharpDX.Matrix.Translation(new SharpDX.Vector3(location.X, location.Y, 0));
                else
                    device.layers[layerIndex].Transform = SharpDX.Matrix.Translation(-bitmap.Size.Width, 0, 0) * SharpDX.Matrix.Scaling(-1, 1, 0)
                        * SharpDX.Matrix.Translation(new SharpDX.Vector3(location.X, location.Y, 0));

            }
            device.layers[layerIndex].DrawBitmap(bitmap, 1, BitmapInterpolationMode.Linear);
            device.layers[layerIndex].Transform = Matrix.Identity;

        }

        /// <summary>
        /// dispose the texture.
        /// </summary>
        public void Dispose()
        {
            if(bitmap!=null)
            bitmap.Dispose();
        }

        /// <summary>
        /// Load image to the texture.
        /// </summary>
        /// <param name="image"></param>
        public void LoadImage(System.Drawing.Bitmap image)
        {
            bitmap = Convertor.ConvertDrawingBmpToDirectxBmp(((Device)device).renderTarget, image);
        }

        #endregion
    }
}
