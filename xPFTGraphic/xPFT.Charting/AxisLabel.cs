/**************************************************************************
 *  AxesLabel.cs
 *  Implementation of the Class AxesLabel
 *  Created on:      08-Oct-2014 12:57:32 PM
 *  Original author: Teb Tasvir
 *
 *  Update 2014/10/15 *****************************************************
 *  Add comment to this code.
 *  
 * Update 2014/10/25 Update#2 *****************************************
 * Add code to check content should dispose before re initialize.
 *
 *  Update 2014/11/16 Update#3 *****************************************
 *  Change code according the new layering design.
 **************************************************************************/
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using xPFT.Charting.Base;

namespace xPFT.Charting
{
    internal class AxisLabel : ChartNamedElement
    {
        #region Consts
        /// <summary>
        /// Label text rectangle width.
        /// </summary>
        public const int LABEL_TEXTBOX_WIDTH = 300;
        /// <summary>
        /// Label text rectangle height.
        /// </summary>
        public const int LABEL_TEXTBOX_HEIGHT = 15;
        #endregion

        #region Constructor
        public AxisLabel(Axis ax)
        {
            axis = ax;
        }

        #endregion

        #region Fields
        /// <summary>
        /// Label text rectangle.
        /// </summary>
        private Rectangle textRectangle = new Rectangle(0, 0, LABEL_TEXTBOX_WIDTH, LABEL_TEXTBOX_HEIGHT);
        /// <summary>
        /// Label line start and end points.
        /// </summary>
        private PointF[] points = new PointF[2];
        /// <summary>
        /// Is this label is the axis title label or not.
        /// </summary>
        internal bool IsAxisTitle = false;
        /// <summary>
        /// Line of the label.
        /// </summary>
        private IDrawing.ILine line;
        /// <summary>
        /// Font that write the label text.
        /// </summary>
        private IDrawing.ITextWriter textWriter;

        #endregion

        #region Properties
        PointF position = new PointF(0, 0);
        /// <summary>
        /// Get or Set the position of the label
        /// </summary>
        public PointF Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                SetPosition();
            }
        }

        private AxisDirection axesType;
        /// <summary>
        /// Get or Set the type of axis that this label belong to it.
        /// </summary>
        public AxisDirection AxisSide
        {
            get
            {
                return axesType;
            }
            set
            {
                axesType = value;
            }
        }

        Color color = Color.Black;
        /// <summary>
        /// Get or Set the color of the line.
        /// </summary>
        public Color LineColor
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        float lineWidth = 1;
        /// <summary>
        /// Get or Set the line width.
        /// </summary>
        public float LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
            }
        }

        string[] text=new string[2];
        /// <summary>
        /// Get or Set the label text.
        /// </summary>
        public string[] Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        Axis axis;
        /// <summary>
        /// Get the Font af the axis.
        /// </summary>
        public Font Font
        {
            get
            {
                if (IsAxisTitle && axis.TitleFont != null)
                    return axis.TitleFont;
                else
                    return axis.Font;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the position of the label.
        /// </summary>
        private void SetPosition()
        {
            try
            {
                if (IsAxisTitle)
                    textRectangle.Width = 2 * LABEL_TEXTBOX_WIDTH;
                points[1] = position;
                if (axesType == AxisDirection.Vertical && axis.LabelInTrueSide)
                {
                    if (IsAxisTitle)
                    {
                        textRectangle.X = (int)position.X - 5-2* LABEL_TEXTBOX_HEIGHT;
                        textRectangle.Y = (int)position.Y;// -LABEL_TEXTBOX_HEIGHT;
                    }
                    else
                    {
                        textRectangle.X = (int)position.X - LABEL_TEXTBOX_WIDTH - 7;
                        textRectangle.Y = (int)position.Y - LABEL_TEXTBOX_HEIGHT / 2;
                        points[0] = new PointF(position.X-5, position.Y);
                    }
                }
                else if (axesType == AxisDirection.Vertical && !axis.LabelInTrueSide)
                {
                    if (IsAxisTitle)
                    {
                        textRectangle.X = (int)position.X + 5+3 * LABEL_TEXTBOX_HEIGHT;
                        textRectangle.Y = (int)position.Y;// -LABEL_TEXTBOX_HEIGHT;
                    }
                    else
                    {
                        textRectangle.X = (int)position.X +7;
                        textRectangle.Y = (int)position.Y - LABEL_TEXTBOX_HEIGHT / 2;
                        points[0] = new PointF(position.X + 5, position.Y);
                    }
                }
                else if (axesType == AxisDirection.Horizontal && axis.LabelInTrueSide)
                {
                    if (IsAxisTitle)
                    {
                        textRectangle.X = (int)position.X - textRectangle.Width;
                        textRectangle.Y = (int)position.Y + 2* LABEL_TEXTBOX_HEIGHT;
                    }
                    else
                    {
                        textRectangle.X = (int)position.X - textRectangle.Width / 2;
                        textRectangle.Y = (int)position.Y + 5;
                        points[0] = new PointF(position.X,position.Y+ 5);
                    }
                }
                else if (axesType == AxisDirection.Horizontal && !axis.LabelInTrueSide)
                {
                    if (IsAxisTitle)
                    {
                        textRectangle.X = (int)position.X - textRectangle.Width;
                        textRectangle.Y = (int)position.Y - 2* LABEL_TEXTBOX_HEIGHT;
                    }
                    else
                    {
                        textRectangle.X = (int)position.X - LABEL_TEXTBOX_WIDTH / 2;
                        textRectangle.Y = (int)position.Y - LABEL_TEXTBOX_HEIGHT - 5;
                        points[0] = new PointF(position.X, position.Y - 5);
                    }
                }
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Initialize the label content on the device.
        /// </summary>
        /// <param name="device"></param>
        public void Initialize(IDrawing.IDevice device,Font font)
        {
            try
            {
                #region Update#2
                if (line != null)
                {
                    line.Dispose();
                    line = null;
                }
                
                if (textWriter != null)
                {
                    textWriter.Dispose();
                    textWriter = null;
                }
                #endregion

                line = GraphicEngine.GraphicEngine.CreateLine(device,font);
                line.Width = lineWidth;
                line.Antialias = true;
                textWriter = GraphicEngine.GraphicEngine.CreateTextWriter(device,Font);// 12, "Mitra");

            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Dispose the axis label.
        /// </summary>
        public new void Dispose()
        {
            if(textWriter!=null)  textWriter.Dispose();
            if (line != null) line.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Draw the label on the device.
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="axesType"></param>
        public void Draw(int layerIndex,ChartArea parentControl, AxisDirection axesType,bool isDrawString=true)
        {
            try
            {
                if (axesType == AxisDirection.Vertical)
                {
                    if (IsAxisTitle)
                    {
                        if (axis.series.Count == 0)
                            textWriter.Draw(layerIndex, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Left, axis.TitleColor, 1, (float)System.Math.PI / 2);
                        else
                        {
                            textWriter.Draw(layerIndex, Text[0], new Rectangle(textRectangle.X, textRectangle.Y + 15, textRectangle.Width, textRectangle.Height),
                                xPFT.Charting.Base.FontDrawFlags.Left, axis.TitleColor, 1, (float)System.Math.PI / 2);
                            points[0] = new PointF(textRectangle.X - textRectangle.Height / 2, textRectangle.Location.Y);
                            points[1] = new PointF(textRectangle.X - textRectangle.Height / 2, textRectangle.Location.Y + 10);
                            line.Width = axis.series[0].LineThickness;

                            if (axis.series[0].ChartType == SeriesChartType.Line)
                            {
                                line.Pattern = axis.series[0].LinePattern;
                                line.Draw(layerIndex, points, axis.series[0].LineColor);
                            }
                            else
                            {
                                switch (axis.series[0].ChartType)
                                {
                                    case SeriesChartType.Point_Ellipse:
                                        line.DrawPoints(layerIndex, points, axis.series[0].LineColor, 1, 0);
                                        break;
                                    case SeriesChartType.Point_Rectangle:
                                        line.DrawPoints(layerIndex, points, axis.series[0].LineColor, 1, 1);
                                        break;
                                    case SeriesChartType.Point_Star:
                                        line.DrawPoints(layerIndex, points, axis.series[0].LineColor, 1, 2);
                                        break;
                                    case SeriesChartType.Point_X:
                                        line.DrawPoints(layerIndex, points, axis.series[0].LineColor, 1, 3);
                                        break;
                                    case SeriesChartType.Point_Diamond:
                                        line.DrawPoints(layerIndex, points, axis.series[0].LineColor, 1, 4);
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (isDrawString)
                        {
                            if (axis.LabelInTrueSide)
                                textWriter.Draw(layerIndex, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Right, color, 1, 0);
                            else
                                textWriter.Draw(layerIndex, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Left, color, 1, 0);
                        }
                        else
                        {
                            if (axis.LabelInTrueSide)
                                points[0].X += 1;
                            else
                                points[0].X -= 1;
                        }
                        line.Draw(layerIndex, points, color);
                    }
                }
                else
                {
                    if (IsAxisTitle)
                    {
                        textWriter.Draw(layerIndex, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Right, axis.TitleColor, 1, 0);
                    }
                    else
                    {
                        if (isDrawString)
                        {
                            textWriter.Draw(layerIndex, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Center, color, 1, 0);
                            textWriter.Draw(layerIndex, Text[1], new Rectangle(textRectangle.X, textRectangle.Y + (int)(Font.Height * 0.8), textRectangle.Width, textRectangle.Height), xPFT.Charting.Base.FontDrawFlags.Center, color, 1,0);
                        }
                        else
                        {
                            if (axis.LabelInTrueSide)
                                points[0].Y -= 1;
                            else
                                points[0].Y += 1;
                        }
                        line.Draw(layerIndex, points, color);
                    }
                }
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Draw axis label on the graphics.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="parentControl"></param>
        /// <param name="axesType"></param>
        /// <param name="isDrawString"></param>
        public void Draw(Graphics g,Point pos, ChartArea parentControl, AxisDirection axesType, bool isDrawString = true)
        {
            try
            {
                if (axesType == AxisDirection.Vertical)
                {
                    if (IsAxisTitle)
                    {
                        if (axis.series.Count == 0)
                            xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Left, axis.TitleColor, 1, (float)System.Math.PI / 2, Font);
                        else
                        {
                            xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos, Text[0], new Rectangle(textRectangle.X, textRectangle.Y + 15, textRectangle.Width, textRectangle.Height),
                                xPFT.Charting.Base.FontDrawFlags.Left, axis.TitleColor, 1, (float)System.Math.PI / 2, axis.Font);
                            points[0] = new PointF(textRectangle.X - textRectangle.Height / 2, textRectangle.Location.Y);
                            points[1] = new PointF(textRectangle.X - textRectangle.Height / 2, textRectangle.Location.Y + 10);
                            line.Width = axis.series[0].LineThickness;

                            if (axis.series[0].ChartType == SeriesChartType.Line)
                            {
                                xPFT.GraphicEngineGDI.Line.Draw(g, pos, points, axis.series[0].LineColor, 1, axis.series[0].LineThickness, axis.series[0].LinePattern);
                            }
                            else
                            {
                                switch (axis.series[0].ChartType)
                                {
                                    case SeriesChartType.Point_Ellipse:
                                        xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, axis.series[0].LineColor, 1, 0, axis.series[0].LineThickness);
                                        break;
                                    case SeriesChartType.Point_Rectangle:
                                        xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, axis.series[0].LineColor, 1, 1, axis.series[0].LineThickness);
                                        break;
                                    case SeriesChartType.Point_Star:
                                        xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, axis.series[0].LineColor, 1, 2, axis.series[0].LineThickness);
                                        break;
                                    case SeriesChartType.Point_X:
                                        xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, axis.series[0].LineColor, 1, 3, axis.series[0].LineThickness);
                                        break;
                                    case SeriesChartType.Point_Diamond:
                                        xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, axis.series[0].LineColor, 1, 4, axis.series[0].LineThickness);
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        xPFT.GraphicEngineGDI.Line.Draw(g, pos, points, color);
                        if (axis.LabelInTrueSide)
                            xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Right, color, 1, 0, axis.Font);
                        else
                            xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Left, color, 1, 0, axis.Font);
                    }
                }
                else
                {
                    if (IsAxisTitle)
                    {
                        xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Right, axis.TitleColor, 1, 0, Font);
                    }
                    else
                    {
                        xPFT.GraphicEngineGDI.Line.Draw(g, pos, points, color);
                        if (isDrawString)
                        {
                            xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos, Text[0], textRectangle, xPFT.Charting.Base.FontDrawFlags.Center, color, 1, 0, axis.Font);
                            xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos, Text[1], new Rectangle(textRectangle.X, textRectangle.Y + (int)(Font.Height * 0.8), textRectangle.Width, textRectangle.Height), xPFT.Charting.Base.FontDrawFlags.Center, color, 1, 0, axis.Font);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }
        #endregion 
    }
}
