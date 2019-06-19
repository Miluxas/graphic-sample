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
using SharpDX;
using SharpDX.Direct3D9;
using xPFT.IDrawing;
using xPFT.DrawingBase;


namespace xPFT.GraphicEngineD3D9
{
    public class TextWriter : ITextWriter
    {
        #region Constructor
        public TextWriter(IDrawing.IDevice device, System.Drawing.Font font)
        {
            FontDescription fontDescription = new FontDescription()
            {
                Height = (int)font.Size,
                Italic = false,
                CharacterSet = FontCharacterSet.Ansi,
                FaceName = font.Name,
                MipLevels = 0,
                OutputPrecision = FontPrecision.TrueType,
                PitchAndFamily = FontPitchAndFamily.Default,
                Quality = FontQuality.ClearType,
                Weight = FontWeight.Normal
            };
            
            if (font.Style == System.Drawing.FontStyle.Bold)
                fontDescription.Weight = FontWeight.Heavy;
            this.font = new Font(((Device)device).device, fontDescription);
            if (sprite != null)
            {
                sprite.Dispose();
                sprite = null;
            }
            sprite = new Sprite(((Device)device).device);
        }
        #endregion

        #region Fields
        Font font;
        Sprite sprite;
        #endregion

        #region Methods
        /// <summary>
        /// Dispose the text writer.
        /// </summary>
        public void Dispose()
        {
            font.Dispose();
            sprite.Dispose();
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
            try
            {
                sprite.Begin(SpriteFlags.AlphaBlend);
                sprite.Transform = Matrix.RotationZ(RotateAngle) * Matrix.Translation(rectangle.X, rectangle.Y, 0);
                font.DrawText(sprite, Text, new SharpDX.Rectangle(0, 0, rectangle.Width, rectangle.Height*2), (SharpDX.Direct3D9.FontDrawFlags)fontDrawFlag, Convertor.ColorConvertor(color, opacity));
                sprite.Transform = Matrix.Identity;
            }
            catch { }
            finally
            {
                sprite.End();
            }
        }
        #endregion
    }
}
