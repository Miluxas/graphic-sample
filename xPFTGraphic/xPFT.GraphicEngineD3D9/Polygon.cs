using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xPFT.DrawingBase;

namespace xPFT.GraphicEngineD3D9
{
    public class Polygon:IDrawing.IPolygon
    {
        #region Constructor
        public Polygon(IDrawing.IDevice device)
        {
            this.device =(Device) device;
            line = new SharpDX.Direct3D9.Line(this.device.device);
            line.Width = lineWidth;
            line.Antialias = true;
            fillerLine = new SharpDX.Direct3D9.Line(this.device.device);
            fillerLine.Width = lineWidth;
            fillerLine.Antialias = true;
            sprite = new SharpDX.Direct3D9.Sprite(this.device.device);
            tex = new SharpDX.Direct3D9.Texture(this.device.device, this.device.size.Width, this.device.size.Height, 1, SharpDX.Direct3D9.Usage.RenderTarget, SharpDX.Direct3D9.Format.A8R8G8B8, SharpDX.Direct3D9.Pool.Default);
        }
        #endregion

        #region Fields
        private SharpDX.Direct3D9.Line line,fillerLine;
        private Device device;
        SharpDX.Direct3D9.Sprite sprite;
        SharpDX.Direct3D9.Texture tex;
        #endregion

        #region Properties
        public bool IsDisposed
        {
            get { return line.IsDisposed; }
        }
        float lineWidth = 2;
        public float LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
                if (line != null)
                    line.Width = lineWidth;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Draw polygon on the layer with layer index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="borderColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="opacity"></param>
        /// <param name="fillPattern"></param>
        /// <param name="patternSize"></param>
        public void Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float opacity, xPFT.Charting.Base.FillPattern fillPattern, float patternSize)
        {

            if (borderColor != null)
                try
                {
                    if (line != null)
                    {
                        line.Begin();
                        line.Draw(DrawingBase.Convertor.ToVector2(points), Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity));
                        line.End();
                    }
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }

            if (fillColor != null)
                if (tex != null)
                {
                    var vertexElems = new[] {
                new SharpDX.Direct3D9.VertexElement(0, 0, SharpDX.Direct3D9.DeclarationType.Float2, SharpDX.Direct3D9.DeclarationMethod.Default, SharpDX.Direct3D9.DeclarationUsage.PositionTransformed, 0),
                new SharpDX.Direct3D9.VertexElement(0, 8, SharpDX.Direct3D9.DeclarationType.Color, SharpDX.Direct3D9.DeclarationMethod.Default, SharpDX.Direct3D9.DeclarationUsage.Color, 0),
                SharpDX.Direct3D9.VertexElement.VertexDeclarationEnd
            };
                    var vertexDecl = new SharpDX.Direct3D9.VertexDeclaration(device.device, vertexElems);
                    SharpDX.Direct3D9.VertexDeclaration tempVD = device.device.VertexDeclaration;
                    device.device.VertexDeclaration = vertexDecl;
                    Vertex[] verList = Shape.GetVertexArray(DrawingBase.Convertor.ToVector2(points), Convertor.ColorConvertor((System.Drawing.Color)fillColor, opacity));
                    device.device.DrawUserPrimitives<Vertex>(SharpDX.Direct3D9.PrimitiveType.TriangleStrip, verList.Length, verList);
                    device.device.VertexDeclaration = tempVD;
                    vertexDecl.Dispose();
                }

        }

        /// <summary>
        /// Draw polygon on the layer with layer index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="bitmap"></param>
        /// <param name="borderColor"></param>
        /// <param name="opacity"></param>
        public void Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Bitmap bitmap, System.Drawing.Color? borderColor, float opacity = 1)
        {
            if (borderColor != null)
            try
            {
                line.Begin();
                line.Draw(DrawingBase.Convertor.ToVector2(points), Convertor.ColorConvertor((System.Drawing.Color)borderColor, opacity));
                line.End();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Draw polygon on the layer with layer index.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="points"></param>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        public void Draw(int layerIndex, System.Drawing.PointF[] points, System.Drawing.Color color, float opacity = 1)
        {
            try
            {
                line.Begin();
                line.Draw(DrawingBase.Convertor.ToVector2(points), Convertor.ColorConvertor(color, opacity));
                line.End();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Dispose the polygon
        /// </summary>
        public void Dispose()
        {
            if (line != null)
            {
                line.Dispose();
                line = null;
            }
            if (fillerLine != null)
            {
                fillerLine.Dispose();
                fillerLine = null;
            }
            if (sprite != null)
            {
                sprite.Dispose();
                sprite = null;
            }
            if (tex != null)
            {
                tex.Dispose();
                tex = null;
            }
        }
        #endregion
    }
}
