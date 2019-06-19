using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xPFT.IDrawing;

namespace xPFT.GraphicEngine
{

    public static class GraphicEngine
    {
        static string GraphicEngineName = "xPFT.GraphicEngineD2D1";
        static string baseFile =System.AppDomain.CurrentDomain.BaseDirectory;
        static Assembly assembly = Assembly.LoadFrom(baseFile + GraphicEngineName+".dll");
        static GraphicEngine()
        {
            AutoDetect();
        }

        static public bool IsInRunMode
        {
            get
            {
                return System.Diagnostics.Debugger.IsAttached || System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv";
            }
        }
        /// <summary>
        /// Auto detect graphic engine. 
        /// </summary>
        public static void AutoDetect()
        {
            if ((int)OSInfo.WIN_TYPE < 61)
                GraphicEngineName = "xPFT.GraphicEngineD3D9";
            else
                GraphicEngineName = "xPFT.GraphicEngineD2D1";
           // GraphicEngineName = "xPFT.GraphicEngineGDIP";
            assembly = Assembly.LoadFrom(baseFile + GraphicEngineName + ".dll");
        }

        /// <summary>
        /// Create and return a device.
        /// </summary>
        /// <returns></returns>
        public static IDrawing.IDevice CreateDevice()
        {
            if (IsInRunMode)
            {
                System.AppDomain.CurrentDomain.Load(assembly.GetName());
                System.Type DeviceType = assembly.GetType(GraphicEngineName + ".Device");
                return (xPFT.IDrawing.IDevice)System.Activator.CreateInstance(DeviceType);
            }
            else
            return new xPFT.GraphicEngineD2D1.Device();
        }

        /// <summary>
        /// Create and return a new Texture.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static IDrawing.ITexture CreateTexture(IDevice device, System.Drawing.Bitmap image)
        {
            if (IsInRunMode)
            {
                System.AppDomain.CurrentDomain.Load(assembly.GetName());
                System.Type TextureType = assembly.GetType(GraphicEngineName + ".Texture");
                return (xPFT.IDrawing.ITexture)System.Activator.CreateInstance(TextureType, device, image);
            }
            else
            return new xPFT.GraphicEngineD2D1.Texture(device, image);
        }

        /// <summary>
        /// Create and return new TextWriter.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="FontSize"></param>
        /// <param name="FontName"></param>
        /// <returns></returns>
        public static IDrawing.ITextWriter CreateTextWriter(IDrawing.IDevice device, System.Drawing.Font font)
        {
            if (IsInRunMode)
            {
                System.AppDomain.CurrentDomain.Load(assembly.GetName());
                System.Type TextWriterType = assembly.GetType(GraphicEngineName + ".TextWriter");
                return (xPFT.IDrawing.ITextWriter)System.Activator.CreateInstance(TextWriterType, device, font);
            }
            else
                return new xPFT.GraphicEngineD2D1.TextWriter(device, font);
        }

        /// <summary>
        /// Create and return a new Line
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static IDrawing.ILine CreateLine(IDrawing.IDevice device,System.Drawing.Font font)
        {
            if (IsInRunMode)
            {
                System.AppDomain.CurrentDomain.Load(assembly.GetName());
                System.Type LineType = assembly.GetType(GraphicEngineName + ".Line");
                return (xPFT.IDrawing.ILine)System.Activator.CreateInstance(LineType, device,font);
            }
            else
                return new xPFT.GraphicEngineD2D1.Line(device,font);
        }

        /// <summary>
        /// Create and return a new Line
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static IDrawing.IRectangle CreateRectangle(IDrawing.IDevice device)
        {
            if (IsInRunMode)
            {
                System.AppDomain.CurrentDomain.Load(assembly.GetName());
                System.Type RectangleType = assembly.GetType(GraphicEngineName + ".Rectangle");
                return (xPFT.IDrawing.IRectangle)System.Activator.CreateInstance(RectangleType, device);
            }
            else
                return new xPFT.GraphicEngineD2D1.Rectangle(device);
        }

        /// <summary>
        /// Create and return a new Ellipse
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static IDrawing.IEllipse CreateEllipse(IDrawing.IDevice device)
        {
            if (IsInRunMode)
            {
                System.AppDomain.CurrentDomain.Load(assembly.GetName());
                System.Type EllipseType = assembly.GetType(GraphicEngineName + ".Ellipse");
                return (xPFT.IDrawing.IEllipse)System.Activator.CreateInstance(EllipseType, device);
            }
            else
                return new xPFT.GraphicEngineD2D1.Ellipse(device);
        }

        /// <summary>
        /// Create and return a new polygon
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static IDrawing.IPolygon CreatePolygon(IDrawing.IDevice device)
        {
            if (IsInRunMode)
            {
                System.AppDomain.CurrentDomain.Load(assembly.GetName());
                System.Type PolygonType = assembly.GetType(GraphicEngineName + ".Polygon");
                return (xPFT.IDrawing.IPolygon)System.Activator.CreateInstance(PolygonType, device);
            }
            else
                return new xPFT.GraphicEngineD2D1.Polygon(device);
        }
    }
}
