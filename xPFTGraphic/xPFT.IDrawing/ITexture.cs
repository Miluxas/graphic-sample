using System.Drawing;

namespace xPFT.IDrawing
{
    public interface ITexture
    {
        /// <summary>
        /// Draw texture on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="location"></param>
        void Draw(int layerIndex, PointF location);
        /// <summary>
        /// Draw texture on the layer.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="location"></param>
        /// <param name="rotateAngle"></param>
        /// <param name="rotateOrigin"></param>
        /// <param name="isVerticalMirror"></param>
        void Draw(int layerIndex, PointF location, float rotateAngle, PointF rotateOrigin, bool isVerticalMirror);
        /// <summary>
        /// Dispose the texture.
        /// </summary>
        void Dispose();
        /// <summary>
        /// Load the image of the texture.
        /// </summary>
        /// <param name="image"></param>
        void LoadImage(Bitmap image);
    }
}
