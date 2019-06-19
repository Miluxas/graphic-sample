using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace xPFT.GraphicEngineGDIP
{
    public class Convertor
    {
        /// <summary>
        /// Convert System.Drawing.Bitmap to SharpDX.Direct2D1.Bitmap.
        /// </summary>
        /// <param name="renderTarget"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        //public static Bitmap ConvertDrawingBmpToDirectxBmp(RenderTarget renderTarget, System.Drawing.Bitmap bitmap)
        //{
        //    var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
        //    var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
        //    var size = new Size2(bitmap.Width, bitmap.Height);
        //    /// Transform pixels from BGRA to RGBA
        //    int stride = bitmap.Width * sizeof(int);
        //    using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
        //    {
        //        /// Lock System.Drawing.Bitmap
        //        var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
        //        /// Convert all pixels 
        //        for (int y = 0; y < bitmap.Height; y++)
        //        {
        //            int offset = bitmapData.Stride * y;
        //            for (int x = 0; x < bitmap.Width; x++)
        //            {
        //                byte B = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
        //                byte G = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
        //                byte R = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
        //                byte A = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
        //                int rgba = R | (G << 8) | (B << 16) | (A << 24);
        //                tempStream.Write(rgba);
        //            }
        //        }
        //        bitmap.UnlockBits(bitmapData);
        //        tempStream.Position = 0;
        //        return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
        //    }
        //}
    }
}
