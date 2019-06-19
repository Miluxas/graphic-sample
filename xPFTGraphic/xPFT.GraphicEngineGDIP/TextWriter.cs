/*********************************************************************
 *  TextWriter.cs
 *  Implementation of the Class TextWriter
 *  Created on:      15-Nov-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using xPFT.IDrawing;


namespace xPFT.GraphicEngineGDIP
{
    public class TextWriter:ITextWriter
    {
        #region Constructor
        public TextWriter(IDrawing.IDevice device, System.Drawing.Font font)// float FontSize = 10, String FontName = "Arial")
        {
            this.font = font;
            renderTarget = ((Device)device).renderTarget;
            textFormat = new StringFormat();
           // this.device = ((Device)device);
           // this.FontName = font.Name;
           // this.FontSize = font.Size;
           
           // renderTarget =this.device.renderTarget;
           // SharpDX.DirectWrite.Factory factory = new SharpDX.DirectWrite.Factory();
           // SharpDX.DirectWrite.FontWeight fontW = SharpDX.DirectWrite.FontWeight.Normal;
           // if(font.Style==System.Drawing.FontStyle.Bold)
           //     fontW = SharpDX.DirectWrite.FontWeight.Heavy;
           // textFormat = new SharpDX.DirectWrite.TextFormat(factory, FontName,fontW, SharpDX.DirectWrite.FontStyle.Normal, FontSize); 
           //// this.device.ChangeRenderTarget += TextWriter_ChangeRenderTarget;
            
        }

      //  void TextWriter_ChangeRenderTarget(RenderTarget newRenderTarget)
      //  {
            ////throw new NotImplementedException();
            //renderTarget = device.renderTarget;
            //brush = new SharpDX.Direct2D1.SolidColorBrush(renderTarget, xPFT.DrawingBase.Convertor.ColorConvertor(curentColor, opacity));
            //SharpDX.DirectWrite.Factory factory = new SharpDX.DirectWrite.Factory();
            //textFormat = new SharpDX.DirectWrite.TextFormat(factory, FontName, FontSize);
   //     }
        #endregion

        #region Fields
        //Font font;
        Graphics renderTarget;
        Brush brush;
        Device device;
        Font font;
        System.Drawing.Color curentColor;
        StringFormat textFormat;
        RectangleF drawRec = new RectangleF();
        float opacity = 1;
        #endregion

        #region Methods
        /// <summary>
        /// Dispose the text writer.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Draw the text on the rectangle.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="rectangle"></param>
        /// <param name="fontDrawFlag"></param>
        /// <param name="color"></param>
        public void Draw(int layerIndex, string Text, System.Drawing.Rectangle rectangle, xPFT.Charting.Base.FontDrawFlags fontDrawFlag, System.Drawing.Color color, float opacity, float RotateAngle)
        {
            if(Text!=null)
             //   try
                {
                    //this.opacity = opacity;
                    if (fontDrawFlag == xPFT.Charting.Base.FontDrawFlags.Left)
                        textFormat.Alignment = StringAlignment.Near;
                    else if (fontDrawFlag == xPFT.Charting.Base.FontDrawFlags.Right)
                        textFormat.Alignment = StringAlignment.Far;
                    else if (fontDrawFlag == xPFT.Charting.Base.FontDrawFlags.Center)
                        textFormat.Alignment =StringAlignment.Center;

                    //drawRec.Width = rectangle.Width;
                    //drawRec.Height = textFormat.FontSize * 1.5f;
                   // renderTarget.DrawString(Text, font, brush, new PointF());
                    try
                    {
                        if (curentColor != color)
                            SetLineColor(color, opacity);

                      //  renderTarget.TranslateTransform();
                       // renderTarget.RotateTransform(RotateAngle);
                      //  Graphics.FromImage(device.layers[layerIndex]
                        renderTarget.DrawString(Text, font, brush, new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, font.Size * 1.5f), textFormat);

                    }
                    catch { }
                }
                //catch (System.Exception ex)
                //{
                //    throw ex;
                //}
        }

        /// <summary>
        /// Set line color.
        /// </summary>
        /// <param name="color"></param>
        void SetLineColor(System.Drawing.Color color,float opacity)
        {
            brush = new SolidBrush(color);// new SharpDX.Direct2D1.SolidColorBrush(renderTarget, xPFT.DrawingBase.Convertor.ColorConvertor(color, opacity));
            curentColor = color;
        }
        #endregion

    }
}
