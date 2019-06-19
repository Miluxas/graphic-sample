/***************************************************************************
 *  Axes.cs
 *  Implementation of the Class Axes
 *  Created on:      07-Oct-2014 12:57:32 PM
 *  Original author: Teb Tasvir
 *  
 *  Update 2014/10/14 *****************************************************
 *  Add title.
 *  Add comment to this code.
 *  
 *  Update 2014/11/16 Update#3 *****************************************
 *  Change code according the new layering design.
 *  
 *  Update 2015/02/08 Update#6 *****************************************
 *  Change the griding system. before we have one grid and it drawing accourding
 *  main axes. but now we can drawing grid line for each axis with different line color.
 * 
***************************************************************************/

using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using xPFT.Charting.Base;

namespace xPFT.Charting
{
    public class Axis:ActiveElement
    {

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"> Name of the Axis. </param>
        /// <param name="axisDirection"> Direction of the axis.(vertical or horizontal)</param>
        /// <param name="Min"> Axis Minimum value. </param>
        /// <param name="Max"> Axis Maximum value</param>
        /// <param name="labelInTrueSide"> Is axis label show in true side or not.(true side is left or bottom). </param>
        /// <param name="locationPercent"> The percent that Determine where axis is show in chart area. </param>
        public Axis(string name, string title, AxisDirection axisDirection, Color lineColor, Color gridColor, float Min, float Max, bool labelInTrueSide, float locationPercent, Axis perpendicularAxis)
        {
            CreateAxis(name, title, axisDirection, lineColor, gridColor, Min, Max, labelInTrueSide, locationPercent,perpendicularAxis);
        }

        public Axis(string name, string title, AxisDirection axisDirection, Color lineColor, Color gridColor, float Min, float Max, bool labelInTrueSide)
        {
            CreateAxis(name, title, axisDirection, lineColor, gridColor, Min, Max, labelInTrueSide, 0.0f,null);
        }

        public Axis(string name, string title, AxisDirection axisDirection, Color lineColor, Color gridColor, float Min, float Max)
        {
            CreateAxis(name, title, axisDirection, lineColor, gridColor, Min, Max, true, 0.0f, null);
        }

        public Axis(string name, string title, AxisDirection axisDirection, float Min, float Max)
        {
            CreateAxis(name, title, axisDirection, LineColor, GridColor, Min, Max, true, 0.0f, null);
        }

        public Axis(string name, string title, AxisDirection axisDirection, Color lineColor, Color gridColor)
        {
            CreateAxis(name, title, axisDirection, lineColor, gridColor, Minimum, Maximum, true, 0.0f, null);
        }

        public Axis(string name, AxisDirection axisDirection)
        {
            CreateAxis(name, "", axisDirection, LineColor, GridColor, Minimum, Maximum, true, 0,null);
        }




        private void CreateAxis(string name, string title, AxisDirection axisDirection, Color lineColor, Color gridColor, float Min, float Max, bool labelInTrueSide, float locationPercent,Axis perpendicularAxis )
        {
            try
            {
                Name = name;
                this.Direction = axisDirection;
                this.minimum = Min;
                this.maximum = Max;
                axisTitle = new AxisLabel(this);
                this.perpendicularAxis = perpendicularAxis;
                axisTitle.IsAxisTitle = true;
                axisTitle.AxisSide = axisDirection;
                LabelInTrueSide = (labelInTrueSide);
                this.LocationPercent = locationPercent;
                Title = (title);
                GridColor = (gridColor);
                LineColor = (lineColor);
                axisTitle.LineColor = LineColor;
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }
       

        #endregion

        /// <summary>
        /// Get is point on the element or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal override bool IsMouseOnTheElement(PointF point)
        {

            if (type == AxisDirection.Vertical)
            {
                if (LabelInTrueSide)
                    return (point.X <= points[0].X && point.X >= points[0].X-MouseHitRay) && (point.Y >= points[1].Y &&  point.Y <= points[0].Y );
                else
                    return (point.X >= points[0].X && point.X <= points[0].X+MouseHitRay) && (point.Y >= points[1].Y &&  point.Y <= points[0].Y );
            }
            else
            {
                if (LabelInTrueSide)
                    return (point.X >= points[0].X && point.X <= points[1].X) && (point.Y >= points[0].Y && point.Y <= points[0].Y+MouseHitRay);
                else
                    return (point.X >= points[0].X && point.X <= points[1].X) && (point.Y >= points[0].Y-MouseHitRay && point.Y <= points[0].Y);
            }
        }


        #region Fields
        /// <summary>
        /// List of values that the axis is devided to them. there are standard of .NET chart element.
        /// </summary>
        private float[] spaceLists = { 500, 200, 100, 50, 20, 10, 5, 2, 1, 0.5f, 0.2f, 0.1f, 0.05f, 0.02f, 0.01f };//};
        /// <summary>
        /// Type of the axis
        /// </summary>
        private AxisDirection type = AxisDirection.Vertical;
        /// <summary>
        /// List of the labels that shown in side of the Axis.
        /// </summary>
        internal AxisLabelCollection labelsCollection = new AxisLabelCollection();
        /// <summary>
        /// start and end point of the axis.
        /// </summary>
        private PointF[] points = new PointF[2];
        /// <summary>
        /// The axis line.
        /// </summary>
        private IDrawing.ILine line;
        /// <summary>
        /// The series that bind to this axis.
        /// </summary>
        internal SeriesCollection series = new SeriesCollection();
        /// <summary>
        /// labels of the axis.
        /// </summary>
        List<object> labels = new List<object>();
        public List<object> Labels
        {
            get
            {
                return labels;
            }
            set
            {
                labels = value;
                minimum = 0;
                maximum = value.Count-1;
            }
        }

        #endregion

        #region Properties


        Axis perpendicularAxis;
        /// <summary>
        /// Get or Set the perpendicular axis to this axis. this property can set to null.
        /// </summary>
        public Axis PerpendicularAxis
        {
            get
            {
                return perpendicularAxis;
            }
            set
            {
                perpendicularAxis = value;
            }
        }
        float center = 0.0f;
        /// <summary>
        /// Get or Set the center of the axis.
        /// </summary>
        public float Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }
        float locationPercent = 0.0f;
        /// <summary>
        /// Get or Set the percent of the chart area width location in percent.
        /// </summary>
        public float LocationPercent
        {
            get
            {
                return locationPercent;
            }
            set
            {
                locationPercent = value;
            }
        }
        /// <summary>
        /// Set the startPoint of the Axis.
        /// </summary>
        internal PointF startPoint
        {
            set
            {
                points[0] = value;
            }
            get
            {
                return points[0];
            }
        }
        /// <summary>
        /// Set the endPoint of the Axis.
        /// </summary>
        internal PointF endPoint
        {
            set
            {
                points[1] = value;
                axisTitle.Position=( value);
            }
            get
            {
                return points[1];
            }
        }
        /// <summary>
        /// Get or Set the type of the Axis. X or Y Axis.
        /// </summary>
        public AxisDirection Direction
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                if(axisTitle!=null)
                axisTitle.AxisSide = type;
            }
        }
        float minimum=-1;
        /// <summary>
        /// Minimum Value of the axis.
        /// </summary>
        public float Minimum
        {
            get
            {
                if (Labels.Count > 0)
                    return 0;
                else
                    return minimum;
            }
            set
            {
                if (Labels.Count > 0)
                    minimum = 0;
                else
                    minimum = value;
            }
        }
        float maximum=1;
        /// <summary>
        /// Maximum Value of the axis.
        /// </summary>
        public float Maximum
        {
            get
            {
                if (Labels.Count > 0)
                    return Labels.Count;
                else
                    return maximum;
            }
            set
            {
                if (Labels.Count > 0)
                    maximum = Labels.Count;
                else
                    maximum = value;
            }
        }
        Color color=Color.Black;
        /// <summary>
        /// Get or Set the color of the axis. Defult is black.
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
        /// Get or Set the line width of the axis. Defult is 1 pixel.
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
        AxisLabel axisTitle ;//= new AxisLabel();
        /// <summary>
        /// Get or Set the title of the Axis.
        /// </summary>
        public string Title
        {
            get
            {
                return axisTitle.Text[0];
            }
            set
            {
                axisTitle.Text[0] = value;
            }
        }
        /// <summary>
        /// Get drawing height rate. 
        /// </summary>
        internal float HeightRate
        {
            get
            {
                return ParentControl.DrawHeight / ((Maximum - Minimum));
            }
        }
        /// <summary>
        /// Get drawing width rate.
        /// </summary>
        internal float WidthRate
        {
            get
            {
                return ParentControl.DrawWidth / ((Maximum - Minimum));
            }
        }
        bool labelInTrueSide = true;
        /// <summary>
        /// Get is label shown in true side. 
        /// True side is left for vertical axis and bottom for horizontal axis.
        /// </summary>
        public bool LabelInTrueSide
        {
            get
            {
                return labelInTrueSide;
            }
            set
            {
                labelInTrueSide = value;
            }
        }
        bool isGridShow = true;
        /// <summary>
        /// Get is Grid shown for this axis label or not.
        /// </summary>
        public bool IsGridShow
        {
            get
            {
                return isGridShow;
            }
            set
            {
                isGridShow = value;
            }
        }
        internal System.Drawing.Color gridColor = System.Drawing.Color.LightGray;
        /// <summary>
        /// Get the grid line color.
        /// </summary>
        public System.Drawing.Color GridColor
        {
            get
            {
                return gridColor;
            }
            set
            {
                gridColor = value;
            }
        }
        //float labelWriteAngle = 0;
        ///// <summary>
        ///// Get the axis label write angle in degree.
        ///// </summary>
        //public float LabelWriteAngle
        //{
        //    get
        //    {
        //        return labelWriteAngle;
        //    }
        //    set
        //    {
        //        labelWriteAngle = value;
        //    }
        //}
        ///// <summary>
        ///// Get the label write angle in radian.
        ///// </summary>
        //internal float LabelWriteAngleInRadian
        //{
        //    get
        //    {
        //        return (float)( labelWriteAngle * System.Math.PI / 180);
        //    }
        //}
        int? labelCount = null;
        /// <summary>
        /// Get or Set the count of the axis labels.
        /// </summary>
        public int LabelCount
        {
            get
            {
                return (int)labelCount;
            }
            set
            {
                //if (value > Maximum - Minimum)
                //    labelCount = null;
                //else
                    labelCount = value;
            }
        }
        //bool isAxisLabelManageAutomatically = true;
        ///// <summary>
        ///// Get or Set the option to determine is axis label managment automatically or not.
        ///// </summary>
        //public bool IsAxisLabelManageAutomatically
        //{
        //    get
        //    {
        //        return isAxisLabelManageAutomatically;
        //    }
        //    set
        //    {
        //        isAxisLabelManageAutomatically = value;
        //    }
        //}
        Color titleColor = Color.Black;
        /// <summary>
        /// Get or Set the Axis title color.
        /// </summary>
        public Color TitleColor
        {
            get
            {
                return titleColor;
            }
            set
            {
                titleColor = value;
            }
        }
        Font titleFont = null;
        /// <summary>
        /// Get or Set the title font.
        /// </summary>
        public Font TitleFont
        {
            get
            {
                return titleFont;
            }
            set
            {
                titleFont = value;
            }
        }
        Size labelSize = new Size(AxisLabel.LABEL_TEXTBOX_WIDTH, AxisLabel.LABEL_TEXTBOX_HEIGHT);
        /// <summary>
        /// Size of the each label text area size.
        /// </summary>
        internal Size LabelSize
        {
            get
            {
                return labelSize;
            }
        }
        void SetLabelsSize()
        {
            string str=" ";
            foreach(AxisLabel aL in labelsCollection)
            {
                if(aL.Text[0].Length>str.Length)
                    str=aL.Text[0];
            }
            SizeF sf= ParentControl.CreateGraphics().MeasureString(str, Font);

            labelSize = new Size((int)sf.Width, (int)sf.Height);
        }
        float mouseHitRay = 30;
        /// <summary>
        /// Get or Set the float that determine mouse event active in how Axis line distance in from axis label side.
        /// </summary>
        public float MouseHitRay
        {
            get
            {
                return mouseHitRay;
            }
            set
            {
                mouseHitRay = value;
            }
        }
        /// <summary>
        /// Space that should be between to label in both type of axis.
        /// </summary>
        float spaceBetweenLabel = 15;
        public float SpaceBetweenLabel
        {
            get
            {
                return spaceBetweenLabel;
            }
            set
            {
                spaceBetweenLabel = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Clear label and recalculate the position and set position.
        /// </summary>
        internal void RePositionLabel()
        {
            try
            {
                if (type == AxisDirection.Vertical)
                {
                    //! Reposition labels if axis is Y type.
                    float tmpSpace = spaceBetweenLabel / HeightRate;
                    tmpSpace = SetPos(tmpSpace);
                }
                else if (type == AxisDirection.Horizontal)
                {
                    //! Reposition labels if axis is X type.
                    float tmpSpace = spaceBetweenLabel / WidthRate;
                    tmpSpace = SetPos(tmpSpace);
                }
                axisTitle.Position = points[1];
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Set posation of the labels
        /// </summary>
        /// <param name="tmpSpace"></param>
        /// <returns></returns>
        private float SetPos(float tmpSpace)
        {
            if (Labels.Count > 0)
            {
                //! Label text should set with the Labels collection.
                tmpSpace = 1;
            }
            else if (labelCount != null && labelCount > 0)
            {
                //! Label Count is enter with user.
                tmpSpace = (int)((Maximum - Minimum) / labelCount);
                if (tmpSpace == 0)
                    tmpSpace = 1;
            }
            else
            {
                //! Label text is generate automatically
                for (int i = 0; i < spaceLists.Length; i++)
                {
                    if (tmpSpace > spaceLists[i])
                    {
                        if (i - 1 >= 0 && i - 1 < spaceLists.Length)
                        {
                            tmpSpace = spaceLists[i - 1];
                            break;
                        }
                    }
                    else
                    {
                        if (i == spaceLists.Length - 1)
                            tmpSpace = spaceLists[spaceLists.Length - 1];
                    }
                }
            }

            float tmpValue = 0;
            labelsCollection.Dispose();
            if (minimum > 0)
                tmpValue = minimum;
            while (tmpValue <= maximum)
            {
                CreateAndAddAxisLabel(tmpValue, type);
                tmpValue += tmpSpace;
                tmpValue = (float)System.Math.Round((double)tmpValue, 2);
            }
            tmpValue = 0;
            while (tmpValue > minimum)
            {
                CreateAndAddAxisLabel(tmpValue, type);
                tmpValue -= tmpSpace;
                tmpValue = (float)System.Math.Round((double)tmpValue, 2);
            }
            SetLabelsSize();
            if (labelsCollection.Count > 1)
            {
                if (type == AxisDirection.Horizontal)
                {
                    int dis = (int)System.Math.Abs(labelsCollection[0].Position.X - labelsCollection[1].Position.X);
                    drawLabelStringEvrey = (int)System.Math.Ceiling((double)labelSize.Width / (double)dis);
                }
                else
                {
                    int dis = (int)System.Math.Abs(labelsCollection[0].Position.Y - labelsCollection[1].Position.Y);
                    drawLabelStringEvrey = (int)System.Math.Ceiling((double)labelSize.Height / (double)dis);
                }
            }
            
            return (float)System.Math.Round((double)tmpSpace, 2);
        }

        internal int drawLabelStringEvrey = 1;

        /// <summary>
        /// Create an axis label and add it to the X axis labels list. the positon of it is detemine from tempValue.
        /// </summary>
        /// <param name="tmpValue"></param>
        private void CreateAndAddAxisLabel(float tmpValue, AxisDirection axisSide)
        {
            try
            {
                AxisLabel al = new AxisLabel(this);
                al.LineColor = LineColor;
                al.AxisSide = type;
                labelsCollection.Add(al);

                if (axisSide == AxisDirection.Vertical)
                    al.Position = new PointF(startPoint.X, Center + ParentControl.Padding.Top - tmpValue * HeightRate);
                else
                    al.Position = new PointF(Center + ParentControl.Padding.Left + tmpValue * WidthRate, endPoint.Y);

                if (Labels.Count > 0)
                {
                    if (Labels[0].GetType() == typeof(System.DateTime))
                    {
                        al.Text[0] = ((System.DateTime)(Labels[(int)tmpValue])).Date.ToShortDateString();
                        System.DateTime tempDateTime = ((System.DateTime)(Labels[(int)tmpValue]));
                        al.Text[1] = string.Format("{0}:{1}", tempDateTime.Hour, tempDateTime.Minute);
                    }
                    else
                        al.Text[0] = Labels[(int)tmpValue].ToString();
                    al.Position = (new PointF(Center + ParentControl.Padding.Left + (tmpValue + 0.5f) * WidthRate, endPoint.Y));
                }
                else
                    al.Text[0] = tmpValue.ToString();
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Initilize the axis on the device.
        /// </summary>
        /// <param name="device"></param>
        internal override void Initialize(IDrawing.IDevice device)
        {
            try
            {
                if (line!=null && !line.IsDisposed)
                {
                    line.Dispose();
                }

                line = GraphicEngine.GraphicEngine.CreateLine(device,Font);
                line.Width = lineWidth;
                line.Antialias = true;
                foreach (AxisLabel al in labelsCollection)
                {
                    al.Initialize(device, Font);
                }
                axisTitle.AxisSide = type;
                axisTitle.Initialize(device, Font);
               
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Draw Axis.
        /// </summary>
        internal void Draw(int layerIndex)
        {
            if (line != null)
                try
                {
                    //! Draw line.
                    line.Draw(layerIndex, points, color);
                    //! Draw labels.
                    for (int index = 0; index < labelsCollection.Count; index++)
                    {
                        labelsCollection[index].Draw(layerIndex, ParentControl, type, (index % drawLabelStringEvrey == 0));
                    }
                    axisTitle.Draw(layerIndex, ParentControl, type);
                }
                catch (System.Exception ex)
                {
                    xPFT.Exceptions.ExceptionHandler.LogError(ex);
                }
        }

        /// <summary>
        /// Draw axis on the graphics.
        /// </summary>
        /// <param name="g"></param>
        internal void Draw(Graphics g,Point pos)
        {
            xPFT.GraphicEngineGDI.Line.Draw(g, pos, points, color);
            for (int index = 0; index < labelsCollection.Count; index++)
            {
                labelsCollection[index].Draw(g,pos, ParentControl, type, (index % drawLabelStringEvrey == 0));
            }
            axisTitle.Draw(g,pos, ParentControl, type);

        }

        /// <summary>
        /// Dispose the axis.
        /// </summary>
        public override void Dispose()
        {
            if (line != null)
            {
                line.Dispose();
                line = null;
            }
            axisTitle.Dispose();
        }

        /// <summary>
        /// Set the Axis center height or width.
        /// </summary>
        internal void SetCenter()
        {
            if (ChartArea.IsInRunMode)
                try
                {
                    if (Direction == AxisDirection.Vertical)
                    {
                        float tmpHeight = ParentControl.Height - ParentControl.Padding.Bottom - ParentControl.Padding.Top;
                        tmpHeight = tmpHeight * (Maximum) / (Maximum - Minimum);
                        Center = tmpHeight;
                    }
                    else
                    {
                        if (Maximum - Minimum > Maximum)
                        {
                            float tmpWidth = ParentControl.Width - ParentControl.Padding.Left - ParentControl.Padding.Right;
                            tmpWidth = tmpWidth * (-Minimum) / (Maximum - Minimum);
                            Center = tmpWidth;
                        }
                        else
                        {
                            Center = 0;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    xPFT.Exceptions.ExceptionHandler.LogError(ex);
                }
        }

        /// <summary>
        /// Initialize the axis.
        /// </summary>
        internal void InitializeAxes()
        {
            try
            {
                if (Direction == AxisDirection.Vertical)
                {
                    if (perpendicularAxis == null)
                    {
                        endPoint = new PointF(ParentControl.Padding.Left + ParentControl.DrawWidth * LocationPercent, ParentControl.Padding.Top);
                        startPoint = new PointF(ParentControl.Padding.Left + ParentControl.DrawWidth * LocationPercent, ParentControl.Height - ParentControl.Padding.Bottom);
                    }
                    else
                    {
                        float tempX = ParentControl.Padding.Left + perpendicularAxis.Center;
                        endPoint = new PointF(tempX, ParentControl.Padding.Top);
                        startPoint = new PointF(tempX, ParentControl.Height - ParentControl.Padding.Bottom);
                    }
                }
                else
                {
                    if (perpendicularAxis == null)
                    {
                        endPoint = new PointF(ParentControl.Width - ParentControl.Padding.Right, ParentControl.Padding.Top + ParentControl.DrawHeight * (1 - LocationPercent));
                        startPoint = new PointF(ParentControl.Padding.Left, ParentControl.Padding.Top + ParentControl.DrawHeight * (1 - LocationPercent));
                    }
                    else
                    {
                        float tempY = ParentControl.Padding.Top +  perpendicularAxis.Center;
                        endPoint = new PointF(ParentControl.Width - ParentControl.Padding.Right,tempY);
                        startPoint = new PointF(ParentControl.Padding.Left, tempY);
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
