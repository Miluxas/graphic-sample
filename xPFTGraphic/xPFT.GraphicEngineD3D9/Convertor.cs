using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D9;
using SharpDX;
using System.Runtime.InteropServices;

namespace xPFT.GraphicEngineD3D9
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vector2 Position;
        public SharpDX.ColorBGRA Color;
    }
    public class Shape
    {
        public static Vertex[] GetVertexArray(Vector2[] points, SharpDX.ColorBGRA color)
        {
            List<Vertex> verList = new List<Vertex>();
            verList.Add(new Vertex() { Color = color, Position = points[0] });
            verList.Add(new Vertex() { Color = color, Position = points[points.Length - 2] });
            verList.Add(new Vertex() { Color = color, Position = points[1] });
            for (int index = 1; index <= (points.Length) / 2; index++)
            {
                verList.Add(new Vertex() { Color = color, Position = points[points.Length - index-1] });
                verList.Add(new Vertex() { Color = color, Position = points[points.Length - index - 2] });

                verList.Add(new Vertex() { Color = color, Position = points[index] });
                verList.Add(new Vertex() { Color = color, Position = points[index + 1] });
            }
            return verList.ToArray();
        }
    }
}
