/*********************************************************************
 *  Series.cs
 *  Implementation of the Class Series
 *  Created on:      28-Sep-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 * 
 * Update 2014/10/14 **************************************************
 * Edit the shifting method.
 * 
 * Update 2014/10/15 **************************************************
 * Add keep from zero option to series.
 * 
 * Update 2014/10/25 Update#2 *****************************************
 * Add code to check content should dispose before re initialize.
 * 
 * 
 **********************************************************************/


using System.Collections.Generic;
using System.Drawing;
using xPFT.Charting.Base;

namespace xPFT.Charting
{
    /// <summary>
    /// Stores data points and series attributes. 
    /// </summary>
    public class Series : ActiveElement
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Series
        /// </summary>
        public Series(string text, Axis HorizontalAxis, Axis VerticalAxis)
        {
            if (HorizontalAxis.Direction != VerticalAxis.Direction)
            {
                points = new DataPointCollection();
                points.AddNewItem += points_AddNewItem;
                XAxis = HorizontalAxis;
                YAxis = VerticalAxis;
                Text = text;
                VerticalAxis.series.Add(this);
            }
            else
            {
                Exceptions.ExceptionHandler.ShowMessage("Two Axes of a series aren't Perpendicular to each other");
            }
        }

        /// <summary>
        /// Initializes a new instance of the Series class with the specified name and maximum number of Y-values.
        /// </summary>
        /// <param name="name">The name of the Series object that will be created. This must be a unique name;
        /// otherwise, an exception will be thrown.</param>
        public Series(string name,string text, Axis HorizontalAxis, Axis VerticalAxis)
        {
            if (HorizontalAxis.Direction != VerticalAxis.Direction)
            {
                points = new DataPointCollection();
                base.Name = name;
                points.AddNewItem += points_AddNewItem;
                XAxis = HorizontalAxis;
                YAxis = VerticalAxis;
                Text = text;
                VerticalAxis.series.Add(this);
            }
            else
            {
                Exceptions.ExceptionHandler.ShowMessage("Two Axes of a series aren't Perpendicular to each other");
            }
        }

        /// <summary>
        /// Initializes a new instance of the Series
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="HorizontalAxis"></param>
        /// <param name="VerticalAxis"></param>
        /// <param name="lineColor"></param>
        /// <param name="lineDrawPattern"></param>
        /// <param name="lineThickness"></param>
        /// <param name="autoShift"></param>
        /// <param name="keepInRightOfYAxis"></param>
        public Series(string name , string text, Axis HorizontalAxis, Axis VerticalAxis, Color lineColor, LineDrawPattern lineDrawPattern = LineDrawPattern.SOLID, float lineThickness = 2, bool autoShift = true, bool keepInRightOfYAxis = false)
        {
            if (HorizontalAxis.Direction != VerticalAxis.Direction)
            {
                points = new DataPointCollection();
                base.Name = name;
                Text = text;
                points.AddNewItem += points_AddNewItem;
                XAxis = HorizontalAxis;
                YAxis = VerticalAxis;
                VerticalAxis.series.Add(this);
                LineColor=(lineColor);
                LinePattern=( lineDrawPattern);
                LineThickness=( lineThickness);
                KeepInRightOfYAxis = keepInRightOfYAxis;
                AutoShift = autoShift;
            }
            else
            {
                Exceptions.ExceptionHandler.ShowMessage("Two Axes of a series aren't Perpendicular to each other");
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Device that series is draw on it.
        /// </summary>
        private IDrawing.IDevice device;
        /// <summary>
        /// Main line that showing points.
        /// </summary>
        private IDrawing.ILine mainPolyLine;
        private IDrawing.ITextWriter labelTextWriter;
        /// <summary>
        /// The chart areas that line be showing on it.
        /// </summary>
        internal ChartAreaCollection charAreas = new ChartAreaCollection();
        /// <summary>
        /// XAxis of the Series.
        /// </summary>
        private Axis XAxis;
        /// <summary>
        /// YAxis of the Series.
        /// </summary>
        private Axis YAxis;
        /// <summary>
        /// Points of the series.
        /// </summary>
        private DataPointCollection points;
        public DataPointCollection Points
        {
            get
            {
                return points;
            }
        }
        /// <summary>
        /// The offset of the series.
        /// </summary>
        PointF ofset = new PointF(0, 0);
        Point transLate = new Point(0, 0);
        #endregion

        #region Properties
        SeriesChartType chartType = SeriesChartType.Line;
        /// <summary>
        /// Gets or sets the chart type of a series.
        /// </summary>
        public SeriesChartType ChartType
        {
            get
            {
                return chartType;
            }
            set
            {
                chartType = value;
            }
        }

        bool visible = true;
        /// <summary>
        /// Gets or sets a flag that indicates whether the series will be visible on the
        /// rendered chart.
        /// </summary>
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        Color color = Color.Black;
        /// <summary>
        /// Get or Set Color of the line.
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

        float lineThickness = 2;
        /// <summary>
        /// Get or Set  Thickness of the line.
        /// </summary>
        public float LineThickness
        {
            get
            {
                return lineThickness;
            }
            set
            {
                lineThickness = value;
            }
        }

        /// <summary>
        /// Get or Set  Is shift automatic or not.
        /// </summary>
        public bool AutoShift
        {
            get
            {
                return points.AutoShift;
            }
            set
            {
                points.AutoShift = value;
            }
        }

        bool keepInRightOfYAxis = false;
        /// <summary>
        /// Get or Set  Is Keep from x axis zero or not.
        /// </summary>
        public bool KeepInRightOfYAxis
        {
            get
            {
                return keepInRightOfYAxis;
            }
            set
            {
                keepInRightOfYAxis = value;
            }
        }

        LineDrawPattern linePattern = LineDrawPattern.SOLID;
        /// <summary>
        /// Get or Set  Line Pattern of the series in line mode.
        /// </summary>
        public LineDrawPattern LinePattern
        {
            get
            {
                return linePattern;
            }
            set
            {
                linePattern = value;
            }
        }

        float mouseHitRay = 10;
        /// <summary>
        /// Get or Set the mouse hit ray.
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

        bool isTopMost = false;
        /// <summary>
        /// Get or Set is sereis top of the other series or not.
        /// Added on 2015-04-08
        /// </summary>
        public bool IsTopMost
        {
            get
            {
                return isTopMost;
            }
            set
            {
                if (value)
                    //! Set the all series's TopMost property to false. then set this series's TopMost property to true
                    for (int chartAreaIndex = 0; chartAreaIndex < charAreas.Count; chartAreaIndex++)
                        foreach (Series s in charAreas[chartAreaIndex].Series)
                            s.isTopMost = false;
                isTopMost = value;
            }
        }

        public bool isPointOfChartLinedTogether { get; set; }

        float? textPosition = null;
        public float? TextPosition
        {
            get
            {
                return textPosition;
            }
            set
            {
                textPosition = value;
            }
        }

        bool isDrawPointLabel = false;
        public bool IsDrawPointLabel
        {
            get
            {
                return isDrawPointLabel;
            }
            set
            {
                isDrawPointLabel = value;
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Re draw the chart areas.
        /// </summary>
        private void ReDrawChartAreas(bool isInRealTimeState=true)
        {
            for (int chartAreaIndex = 0; chartAreaIndex < charAreas.Count; chartAreaIndex++)
            {
                if (charAreas[chartAreaIndex].IsRealTimeChart==isInRealTimeState)
                    charAreas[chartAreaIndex].Draw();
            }
        }

        /// <summary>
        /// Run when add new point to this series.
        /// </summary>
        /// <param name="item"></param>
        void points_AddNewItem(xPFT.Charting.Base.DataPoint item)
        {
            ReDrawChartAreas();
        }

        /// <summary>
        /// Add new point to the end of the series.
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public void AddPoint(float xValue, float yValue)
        {
            points.AddXY(xValue, yValue);
        }
        public void AddPoint(float xValue, float yValue, object tag)
        {
            var t=new DataPoint(xValue,yValue);
            t.Tag=tag;
            points.Add(t);
        }
        /// <summary>
        /// Add new point to the end of the series.
        /// Added on 2015-04-08
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public void AddPoints(PointF[] points)
        {
            this.points.AddPointFArray(points);
            //ReDrawChartAreas(false);
        }
        public void AddPoints(DataPoint[] points)
        {
            this.points.AddDataPointArray(points);
            //ReDrawChartAreas(false);
        }
        /// <summary>
        /// Shift the series on the chart area.
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void Shift(float dx, float dy)
        {
            foreach (xPFT.Charting.Base.DataPoint dp in points)
            {
                dp.XValue += dx;
                for (int i = 0; i < dp.YValues.Count; i++)
                    dp.YValues[i] += dy;
            }
            ReDrawChartAreas();
        }

        /// <summary>
        /// Draw the line.
        /// </summary>
        internal void Draw(int layerIndex)
        {
            if (Visible)
            {
                try
                {
                    if (device != null)
                        if (points.Count > 0)
                        {
                            float Y_Center = YAxis.Center + ofset.Y * YAxis.HeightRate;
                            float X_Center = XAxis.Center + ofset.X * XAxis.WidthRate;
                            if (XAxis.Labels.Count > 0)
                            {
                                X_Center = XAxis.Center + ofset.X * XAxis.WidthRate + XAxis.WidthRate / 2;
                            }
                            if(isPointOfChartLinedTogether || chartType==SeriesChartType.Line)
                                transLate= mainPolyLine.Draw(layerIndex, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 1, AutoShift, KeepInRightOfYAxis);

                            switch (chartType)
                            {
                                case SeriesChartType.Point_Ellipse:
                                    transLate = mainPolyLine.DrawPoints(layerIndex, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 0, 1, AutoShift, KeepInRightOfYAxis,isDrawPointLabel);
                                    break;
                                case SeriesChartType.Point_Rectangle:
                                    transLate = mainPolyLine.DrawPoints(layerIndex, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 1, 1, AutoShift, KeepInRightOfYAxis,isDrawPointLabel);
                                    break;
                                case SeriesChartType.Point_Star:
                                    transLate = mainPolyLine.DrawPoints(layerIndex, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 2, 1, AutoShift, KeepInRightOfYAxis, isDrawPointLabel);
                                    break;
                                case SeriesChartType.Point_X:
                                    transLate = mainPolyLine.DrawPoints(layerIndex, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 3, 1, AutoShift, KeepInRightOfYAxis, isDrawPointLabel);
                                    break;
                                case SeriesChartType.Point_Diamond:
                                    transLate = mainPolyLine.DrawPoints(layerIndex, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 4, 1, AutoShift, KeepInRightOfYAxis, isDrawPointLabel);
                                    break;
                            }
                            int textPositionY = (int)(Y_Center - points[0].YValues[0] * YAxis.HeightRate);
                            if (textPosition != null)
                                textPositionY =(int)(Y_Center- textPosition.Value*YAxis.HeightRate) ;
                            labelTextWriter.Draw(layerIndex, Text, new Rectangle(new Point((int)X_Center,textPositionY), new Size(20, 200)), FontDrawFlags.Left, LineColor, 1, 0);

                        }
                }
                catch (System.Exception ex)
                {
                    xPFT.Exceptions.ExceptionHandler.LogError(ex);
                }
            }
        }

        /// <summary>
        /// Draw series on the ghraphics.
        /// </summary>
        /// <param name="g"></param>
        internal void Draw(Graphics g,Point pos)
        {
            if (Visible)
            {
                try
                {
                    if (points.Count > 1)
                    {

                        //g.ResetTransform();
                        float Y_Center = YAxis.Center + ofset.Y * YAxis.HeightRate;
                        float X_Center = XAxis.Center + ofset.X * XAxis.WidthRate;
                        if (XAxis.Labels.Count > 0)
                        {
                            X_Center = XAxis.Center + ofset.X * XAxis.WidthRate + XAxis.WidthRate / 2;
                        }
                        if (isPointOfChartLinedTogether || chartType == SeriesChartType.Line)
                            xPFT.GraphicEngineGDI.Line.Draw(g, pos, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 1, AutoShift, KeepInRightOfYAxis, LineThickness, LinePattern);

                        switch (chartType)
                        {
                            case SeriesChartType.Point_Ellipse:
                                xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 0, 1, AutoShift, KeepInRightOfYAxis, LineThickness, isDrawPointLabel, Font);
                                break;
                            case SeriesChartType.Point_Rectangle:
                                xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 1, 1, AutoShift, KeepInRightOfYAxis, LineThickness, isDrawPointLabel, Font);
                                break;
                            case SeriesChartType.Point_Star:
                                xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 2, 1, AutoShift, KeepInRightOfYAxis, LineThickness, isDrawPointLabel, Font);
                                break;
                            case SeriesChartType.Point_X:
                                xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 3, 1, AutoShift, KeepInRightOfYAxis, LineThickness, isDrawPointLabel, Font);
                                break;
                            case SeriesChartType.Point_Diamond:
                                xPFT.GraphicEngineGDI.Line.DrawPoints(g, pos, points, Y_Center, X_Center, YAxis.HeightRate, XAxis.WidthRate, XAxis.Maximum, color, 4, 1, AutoShift, KeepInRightOfYAxis, LineThickness, isDrawPointLabel,Font);
                                break;
                        }
                        g.ResetTransform();
                        int textPositionY = (int)(Y_Center - points[0].YValues[0] * YAxis.HeightRate);
                        if (textPosition != null)
                            textPositionY = (int)(Y_Center - textPosition.Value * YAxis.HeightRate);
                        xPFT.GraphicEngineGDI.TextWriter.Draw(g,pos ,Text, new Rectangle(new Point((int)X_Center, textPositionY), new Size(20, 200)), FontDrawFlags.Left, LineColor, 1, 0,Font);

                    }
                }
                catch (System.Exception ex)
                {
                    xPFT.Exceptions.ExceptionHandler.LogError(ex);
                }
            }
        }

        /// <summary>
        /// Initialize the series component on the device.
        /// </summary>
        /// <param name="device"></param>
        internal override void Initialize(IDrawing.IDevice device)
        {
            try
            {
                this.device = device;
                mainPolyLine = GraphicEngine.GraphicEngine.CreateLine(device,Font);
                mainPolyLine.Width = LineThickness;
                mainPolyLine.Antialias = true;
                mainPolyLine.Pattern = linePattern;
                labelTextWriter = GraphicEngine.GraphicEngine.CreateTextWriter(device,Font);
                points.Width = XAxis.Maximum - XAxis.Minimum;
                points.HeightBottom = YAxis.Minimum;
                points.HeightTop = YAxis.Maximum;
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Dispose the series
        /// </summary>
        public override void Dispose()
        {
            if (mainPolyLine != null)
            {
                mainPolyLine.Dispose();
                mainPolyLine = null;
            }
            if (labelTextWriter != null)
            {
                labelTextWriter.Dispose();
                labelTextWriter = null;
            }
        }

        /// <summary>
        /// Determine is mouse on the element or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal override bool IsMouseOnTheElement(PointF point)
        {
            PointF[] tmpPoints = convertDataPointToPointFArrayWithTranslate(points, YAxis.HeightRate, XAxis.WidthRate, YAxis.Center, XAxis.Center);
            float tmpYAxesCenterWidth = XAxis.Center;

            if (tmpPoints.Length > 1)
            {
                if (AutoShift)
                {
                    if (points.GetLast().XValue > XAxis.Maximum)
                        tmpYAxesCenterWidth = -((float)points.GetLast().XValue - XAxis.Maximum) * XAxis.WidthRate + YAxis.Center;
                    else
                        tmpYAxesCenterWidth = -tmpPoints[0].X + YAxis.Center;
                }
            }

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            if (tmpPoints.Length > 2)
                gp.AddLines(tmpPoints);
            //if (XAxis.Labels.Count > 0)
               // point = new PointF(point.X - XAxis.WidthRate / 2, point.Y);
            point = new PointF(point.X - transLate.X, point.Y);
            return gp.IsOutlineVisible(point, new Pen(Color.Red, mouseHitRay));
        }

        /// <summary>
        /// Convert the data point collection to float point array.
        /// </summary>
        /// <param name="dPC"></param>
        /// <param name="HeightRate"></param>
        /// <param name="WidthRate"></param>
        /// <param name="YCenter"></param>
        /// <param name="XCenter"></param>
        /// <returns></returns>
        System.Drawing.PointF[] convertDataPointToPointFArrayWithTranslate(Charting.Base.DataPointCollection dPC, float HeightRate, float WidthRate, float YCenter, float XCenter)
        {
            List<System.Drawing.PointF> tmplist = new List<System.Drawing.PointF>();
            float x, y;
            System.Drawing.PointF tmpVecto2 = new System.Drawing.PointF();
            for (int indexOfDataPoint = 0; indexOfDataPoint < dPC.Count; indexOfDataPoint++)
            {
                x = (float)(dPC[indexOfDataPoint].XValue * WidthRate) + XCenter;
                y = -(float)(dPC[indexOfDataPoint].YValues[0]) * HeightRate + YCenter;

                if (tmplist.Count > 0)
                {
                    if (GetDistance(x, y, tmplist[tmplist.Count - 1].X, tmplist[tmplist.Count - 1].Y) > 2)
                    {
                        tmpVecto2.X = x;
                        tmpVecto2.Y = y;
                    }
                }
                else
                {
                    tmpVecto2.X = x;
                    tmpVecto2.Y = y;
                }
                tmplist.Add(tmpVecto2);
            }
            return tmplist.ToArray();
        }

        /// <summary>
        /// Get distance between two point.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        float GetDistance(float x1, float y1, float x2, float y2)
        {
            return (float)(System.Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) / (y1 - y2)));
        }
        #endregion
    }
}