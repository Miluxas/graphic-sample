
/*********************************************************************
 *  ChartArea.cs
 *  Implementation of the Class ChartArea
 *  Created on:      28-Sep-2014 11:45:56 AM
 *  Original author: Teb Tasvir
 *  
 * 
 * Update 2014/10/14 **************************************************
 * Add comment to this code.
 * 
 * Update 2014/10/25 Update#2 *****************************************
 * Add code to check device should dispose before re initialize.
 * 
 * Update 2014/10/26 Update#3 *****************************************
 * Add lock object to syncronize draw and initialize action in multi thread
 * state.
 * 
 * Update 2014/10/29 Update#4 *****************************************
 * Add other device config that used for a system that don't have an installed 
 * graphic card.
 * 
 *  Update 2014/11/16 Update#5 *****************************************
 *  Change code according the new layering design.
 *  
 *  Update 2015/02/08 Update#6 *****************************************
 *  Change the design of the axes. 
 *  before each chart area have two main axis and all series is drawing
 *  accourding it. but now when we create a series we should determein
 *  to axes in constructor. in this state series drawing accourding to 
 *  the two axes that determine in create time.
 *  
***********************************************************************/

using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
namespace xPFT.Charting
{
    /// <summary>
    /// Represents a chart area on the chart image. 
    /// </summary>
    public class ChartArea : Control
    {

        public static string TimingResult = "";

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ChartArea class with the specified chart area
        /// name.
        /// </summary>
        /// <param name="name">The name for the new ChartArea object.</param>
        public ChartArea(string name = " ")
        {
            Name = name;
            CreateChartArea();
        }
        public ChartArea()
        {
            Name = " ";
            CreateChartArea();
        }

        private void CreateChartArea()
        {
            if (IsInRunMode)
            {
                device = GraphicEngine.GraphicEngine.CreateDevice();
                series = new SeriesCollection();
                Axes = new AxisCollection();
                annotations = new AnnotationCollection();
                Padding = new Padding(50);
              //  
              //  Initialize();
                //OnPaint(null);
            }
        }

        #endregion

        #region Fields
        /// <summary>
        /// Axis Collection
        /// </summary>
        public AxisCollection Axes=new AxisCollection();
        /// <summary>
        /// device that showing the chart area.
        /// </summary>
        public IDrawing.IDevice device;//=new Device();
        Grid grid;

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        object objLock = new object();
        #endregion

        #region Properties
        
        SeriesCollection series=new SeriesCollection();
        /// <summary>
        /// The series that shown in this chart area
        /// </summary>
        public SeriesCollection Series
        {
            get
            {
                return series;
            }
            set
            {
                series = value;
            }
        }

        /// <summary>
        /// Get is In run mode or in design mode?
        /// </summary>
        static public bool IsInRunMode
        {
            get
            {
                return System.Diagnostics.Debugger.IsAttached || System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv";
            }
        }

        float refreshRate = 33;
        /// <summary>
        /// Get or Set the automatically refresh rate.
        /// </summary>
        public float RefreshRate
        {
            get
            {
                return refreshRate;
            }
            set
            {
                refreshRate = value;
            }
        }

        
        bool isRealTimeChart = true;
        /// <summary>
        /// Get or Set is Refreshing automaticaly optimaize or not.
        /// </summary>
        public bool IsRealTimeChart
        {
            get
            {
                return isRealTimeChart;
            }
            set
            {
                isRealTimeChart = value;
            }
        }

        float drawHeight;
        /// <summary>
        /// Get the height of the drawable region.
        /// </summary>
        internal float DrawHeight
        {
            get
            {
                return drawHeight;
            }
        }

        float drawWidth;
        /// <summary>
        /// Get the width of the drawable region.
        /// </summary>
        internal float DrawWidth
        {
            get
            {
                return drawWidth;
            }
        }


        float widthRatio = 1;
        /// <summary>
        /// Ratio of the width in chart control.
        /// </summary>
        public float WidthRatio
        {
            get
            {
                return widthRatio;
            }
            set
            {
                widthRatio = value;
            }
        }

        float heightRatio = 1;
        /// <summary>
        /// Ratio of the height in chart control.
        /// </summary>
        public float HeightRatio
        {
            get
            {
                return heightRatio;
            }
            set
            {
                heightRatio = value;
            }
        }

        /// <summary>
        /// Get the mouse point
        /// </summary>
        internal System.Drawing.Point MousePos
        {
            get
            {
                return PointToClient(MousePosition);
            }
        }

        bool isGridShow = true;
        /// <summary>
        /// Get or Set is grid show or not.
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

        float? axisRelate = null;
        /// <summary>
        /// Get or Set the axis relate.
        /// </summary>
        public float? AxisRelate
        {
            get
            {
                return axisRelate;
            }
            set
            {
                axisRelate = value;
            }
        }

        bool isReactionEnable = true;
        /// <summary>
        /// Get or Set is series reaction enable or not.
        /// </summary>
        public bool IsReactionEnable
        {
            get
            {
                return isReactionEnable;
            }
            set
            {
                isReactionEnable = value;
            }
        }

        internal AnnotationCollection annotations=new AnnotationCollection();
        /// <summary>
        /// The annotations that shown in chart area.
        /// </summary>
        public AnnotationCollection Annotations
        {
            get
            {
                return annotations;
            }
            set
            {
                annotations = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Recalculate the chart area content.
        /// </summary>
        internal void RecalculateContent(Size? newSize = null)
        {
           
            sv.Start();
            System.Drawing.Size s= Size;;
            if (newSize != null)
            {
                Size = ((System.Drawing.Size)newSize);
            }
            if (IsInRunMode)
                try
                {
                    lock (objLock)                      //# Update#3
                    {
                        //! Set center of all axis of this chartarea.
                        foreach (Axis ax in Axes)
                            ax.SetCenter();
                        //! If have two axis and axis Relate isn't null then set the axis height or width with together accourding to axis Relate value.
                        if (Axes.Count > 1 && axisRelate != null)
                        {
                            if (axisRelate > 0)
                            {
                                if (Axes[0].Direction == AxisDirection.Vertical)
                                {
                                    float r = (Height - Padding.Top - Padding.Bottom) / (Axes[0].Maximum - Axes[0].Minimum);
                                    Padding = new Padding(Padding.Left, Padding.Top, (int)(Width - Padding.Left - r * axisRelate * (Axes[1].Maximum - Axes[1].Minimum)), Padding.Bottom);
                                }
                                else
                                {
                                    float r = (Height - Padding.Top - Padding.Bottom) / (Axes[1].Maximum - Axes[1].Minimum);
                                    Padding = new Padding(Padding.Left, Padding.Top, (int)(Width - Padding.Left - r * axisRelate * (Axes[0].Maximum - Axes[0].Minimum)), Padding.Bottom);
                                }
                            }
                        }
                        drawHeight = Height - Padding.Bottom - Padding.Top;
                        drawWidth = Width - Padding.Left - Padding.Right;
                        //! Set the startPoint and EndPoint of the axes.
                        foreach (Axis ax in Axes)
                            ax.InitializeAxes();

                        //! Reset the position and count of the each axis labels.
                        foreach (Axis ax in Axes)
                            ax.RePositionLabel();

                        //! Initialize the chart area 
                        Initialize();
                        
                        //! Reposition all annotations.
                        for (int index = 0; index < annotations.Count; index++)
                        {
                            annotations[index].Reposition();
                        }

                        //! Reset the grid's line position and count.
                        if (grid != null)
                            grid.Reposition(Axes);

                        DrawBack();
                        //! Draw the backGround.

                    }
                }
                catch (System.Exception ex)
                {
                    xPFT.Exceptions.ExceptionHandler.LogError(ex);
                }
            if (newSize != null)
            {
                Size = s;
            }
            WriteToFile("Recalcuted the device :");
        }
        System.Diagnostics.Stopwatch sv = new System.Diagnostics.Stopwatch();
        /// <summary>
        /// Initialize the control to drawing on it
        /// </summary>
        internal void Initialize()
        {
           
                sv.Start();
                if (IsInRunMode)
                try
                {
                    //! Dispose all element in the chart area. 
                    //! because if don't dispose elements device resize don't worked properly.
                    if (grid != null)
                        grid.Dispose();
                    Axes.DisposeAllItems();
                    series.DisposeAllItems();
                    annotations.DisposeAllItems();
                    WriteToFile("Disposed items :");
                    //sv.Start();
                    #region Update#4
                    device.Initialize(this);
                    WriteToFile("Created Device :");
                    #endregion
                    //! After Inititalize new device or resized device Inititalize all element.
                    grid = new Grid(this);
                    Axes.InitializeAllItems(device);
                    series.InitializeAllItems(device);
                    annotations.InitializeAllItems(device);
                    WriteToFile("Initialized items :");
                }
                catch (System.Exception ex)
                {
                    xPFT.Exceptions.ExceptionHandler.LogError(ex);
                }
             //   WriteToFile("Initialize the device :");
        }

        private void WriteToFile(string str)
        {
           // sv.Stop();

            TimingResult +=str  + sv.ElapsedMilliseconds.ToString()+"      ";
           // sv.Start();
        }

        /// <summary>
        /// Drawing the series axis annotation and ...
        /// </summary>
        internal void Draw()
        {   
            if (IsInRunMode)
            {
                if (!stopWatch.IsRunning)
                    stopWatch.Start();
                //! If the chart area dont in real time mode redraw series layer and present device.
                //! or if last series draw method isn't called in last 1000/refreshRate mili secconde.
                if (!isRealTimeChart || stopWatch.ElapsedMilliseconds > 1000 / refreshRate)
                {
                    lock (objLock)                      
                    {
                        DrawSeries();
                        device.Present();
                    }
                    stopWatch.Reset();
                    stopWatch.Start();
                }
                if (isRealTimeChart)
                    device.Present();
            }
        }

        public void ReDraw(ActiveElement Element)
        {
            //RecalculateContent();
            ReDrawLayer(Element.DrawingLayer);
            Draw();
        }
        public void ReDraw()
        {
            //RecalculateContent();
            //DrawBack();
            Draw();
        }
        /// <summary>
        /// Draw all layer.
        /// </summary>
        internal void DrawBack()
        {
            DrawBackGround(); 
            DrawGrid();
            DrawFrontOfGridLayer();
            DrawAxesLayer();
            DrawFrontOfAxisAndGridLayer();
            DrawSeries();
            DrawFrontOfAllLayer();
        }

        /// <summary>
        /// Drawing annotation that insert the background layer.
        /// </summary>
        void DrawBackGround()
        {
            if (device != null)
            {
                device.BeginLayerDraw(0);
                device.ClearLayer(0, BackColor);
                for (int index = 0; index < annotations.Count; index++)
                {
                    if (annotations[index].DrawingLayer == Base.AnnotationDrawingLayer.BackOfAll)
                        annotations[index].Draw(0);
                }
                device.EndLayerDraw(0);
            }
        }

        /// <summary>
        /// Drawing grid's layer.
        /// </summary>
        void DrawGrid()
        {
            if (device != null)
            {
                device.BeginLayerDraw(1);
                device.ClearLayer(1, Color.Transparent);
                if (IsGridShow && grid != null)
                    grid.Draw(1);
                device.EndLayerDraw(1);
            }
        }

        /// <summary>
        /// Drawing annotation that insert the layer that drawing before the grid layer.
        /// </summary>
        void DrawFrontOfGridLayer()
        {
            if (device != null)
            {
                device.BeginLayerDraw(2);
                device.ClearLayer(2, Color.Transparent);
                for (int index = 0; index < annotations.Count; index++)
                {
                    if (annotations[index].DrawingLayer == Base.AnnotationDrawingLayer.FrontOfGrid)
                        annotations[index].Draw(2);
                }
                device.EndLayerDraw(2);
            }
        }

        /// <summary>
        /// Drawing axis layer
        /// </summary>
        internal void DrawAxesLayer()
        {
            if (device != null)
            {
                device.BeginLayerDraw(3);
                device.ClearLayer(3, Color.Transparent);
                for (int index = 0; index < Axes.Count; index++)
                {
                    Axes[index].Draw(3);
                }
                device.EndLayerDraw(3);
            }
        }

        /// <summary>
        /// Drawing annotation that insert the layer that drawing before the axis layer.
        /// </summary>
        void DrawFrontOfAxisAndGridLayer()
        {
            if (device != null)
            {
                device.BeginLayerDraw(4);
                device.ClearLayer(4, Color.Transparent);
                for (int index = 0; index < annotations.Count; index++)
                {
                    if (annotations[index].DrawingLayer == Base.AnnotationDrawingLayer.FrontOfAxisAndGrid)
                        annotations[index].Draw(4);
                }
                device.EndLayerDraw(4);
            }
        }

        /// <summary>
        /// Drawing series layer.
        /// </summary>
        internal void DrawSeries()
        {
            if (device != null)
            {
                device.BeginLayerDraw(5);
                device.ClearLayer(5, Color.Transparent);
                for (int index = 0; index < series.Count; index++)
                {
                    if (!series[index].IsTopMost)
                        series[index].Draw(5);
                }
                for (int index = 0; index < series.Count; index++)
                {
                    if (series[index].IsTopMost)
                        series[index].Draw(5);
                }
                device.EndLayerDraw(5);
            }
        }

        /// <summary>
        /// Drawing annotation that insert the front layer 
        /// </summary>
        void DrawFrontOfAllLayer()
        {
            if (device != null)
            {
                device.BeginLayerDraw(6);
                device.ClearLayer(6, Color.Transparent);
                for (int index = 0; index < annotations.Count; index++)
                {
                    if (annotations[index].DrawingLayer == Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid)
                        annotations[index].Draw(6);
                }
                device.EndLayerDraw(6);
            }
        }

        /// <summary>
        /// Redraw the layer 
        /// </summary>
        /// <param name="layer"></param>
        internal void ReDrawLayer(Base.AnnotationDrawingLayer layer)
        {
            switch (layer)
            {
                case Base.AnnotationDrawingLayer.BackOfAll:
                    DrawBackGround();
                    break;
                case Base.AnnotationDrawingLayer.FrontOfAxisAndGrid:
                    DrawFrontOfAxisAndGridLayer();
                    break;
                case Base.AnnotationDrawingLayer.FrontOfGrid:
                    DrawFrontOfGridLayer();
                    break;
                case Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid:
                    DrawFrontOfAllLayer();
                    break;
            }
        }

        /// <summary>
        /// Redraw layer
        /// </summary>
        /// <param name="layerIndex"></param>
        internal void ReDrawLayer(int layerIndex)
        {
            switch (layerIndex)
            {
                case 0:
                    DrawBackGround();
                    break;
                case 1:
                    DrawGrid();
                    break;
                case 2:
                    DrawFrontOfGridLayer();
                    break;
                case 3:
                    DrawAxesLayer();
                    break;
                case 4:
                    DrawFrontOfAxisAndGridLayer();
                    break;
                case 5:
                    DrawSeries();
                    break;
                case 6:
                    DrawFrontOfAllLayer();
                    break;
            }
        }

        /// <summary>
        /// Add a new series to series collection.
        /// </summary>
        /// <param name="newSeries"></param>
        public virtual void AddSeries(Series newSeries)
        {
            try
            {
                series.Add(newSeries);
                newSeries.charAreas.Add(this);
            }
            catch (System.Exception ex)
            {
                xPFT.Exceptions.ExceptionHandler.LogError(ex);
            }
        }

        /// <summary>
        /// Remove Series from series collection.
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        public virtual bool RemoveSeries(Series series)
        {
            var result= this.series.Remove(series);
            OnPaint(null);
            return result;
        }

        /// <summary>
        /// Get the series according name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Series GetSeries(string name)
        {
            foreach (Series s in series)
            {
                if (s.Name == name)
                    return s;
            }
            return null;
        }

        /// <summary>
        /// Create an annotation line and add it to the chart area. and return annotation.
        /// </summary>
        Annotation AddAnnotationLine(string name,string text, float value, bool isHorizontalAnnotationLine, Charting.Base.AnnotationDrawingLayer drawingLayer = Charting.Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid)
        {
            AnnotationLine annot;
            if (isHorizontalAnnotationLine)
                annot = new HorizontalAnnotationLine(this);
            else
                annot = new VerticalAnnotationLine(this);
            annot.Name = name;
            annot.DrawingLayer = drawingLayer;
            annot.value = value;
            annot.Text = text;
            annotations.Add(annot);
            annot.Initialize(device);
            ReDrawLayer(drawingLayer);
            return annot;
        }

        /// <summary>
        /// Create an annotation line and add it to the chart area. and return annotation.
        /// </summary>
        public Annotation AddAnnotationLine(string name,string text, float value, bool isHorizontalAnnotationLine, System.Drawing.Color color, float lineWidth = 2, float opacity = 1, Charting.Base.AnnotationDrawingLayer drawingLayer = Charting.Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid, Axis Vertical_axis = null, Axis Horizontal_axis = null)
        {
            Annotation annot = AddAnnotationLine(name,text, value, isHorizontalAnnotationLine);
            annot.opacity = opacity;
            annot.lineWidth = lineWidth;
            annot.DrawingLayer = drawingLayer;
            if (annot.GetType().BaseType == typeof(AnnotationLine))
            {
                ((AnnotationLine)annot).color = color;
            }
            annot.horizontalAxis = Horizontal_axis;
            annot.verticalAxis = Vertical_axis;
            ReDrawLayer(drawingLayer);
            return annot;
        }

        /// <summary>
        /// Create an annotation shape and add it to the chart area. and return annotation.
        /// </summary>
        public Annotation AddAnnotationShape(string name,string text, PointF topLeftPoint, PointF bottomRight, bool isRectangleAnnotation, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float lineWidth = 2, float opacity = 1, Charting.Base.AnnotationDrawingLayer drawingLayer = Charting.Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid, Charting.Base.FillPattern fillPattern = Charting.Base.FillPattern.Solid, float patternSize = 8, Axis Vertical_axis = null, Axis Horizontal_axis = null)
        {
            AnnotationShape annot;
            if (isRectangleAnnotation)
                annot = new AnnotationRectangle(this);
            else
                annot = new AnnotationEllipse(this);
            annotations.Add(annot);
            annot.startValuePoint = topLeftPoint;
            annot.endValuePoint = bottomRight;
            annot.opacity = opacity;
            annot.lineWidth = lineWidth;
            annot.fillColor = fillColor;
            annot.color = borderColor;
            annot.DrawingLayer = drawingLayer;
            annot.fillPattern = fillPattern;
            annot.patternSize = patternSize;
            annot.Initialize(device);
            annot.horizontalAxis = Horizontal_axis;
            annot.verticalAxis = Vertical_axis;
            ReDrawLayer(drawingLayer);
            annot.Name = name;
            annot.Text = text;
            return annot;
        }

        /// <summary>
        /// Create an annotation shape and add it to the chart area. and return annotation.
        /// </summary>
        public Annotation AddAnnotationShape(string name,string text, PointF[] points, System.Drawing.Color? borderColor, System.Drawing.Color? fillColor, float lineWidth = 2, float opacity = 1, Charting.Base.AnnotationDrawingLayer drawingLayer = Charting.Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid, Charting.Base.FillPattern fillPattern = Charting.Base.FillPattern.Solid, float patternSize = 8, Axis Vertical_axis = null, Axis Horizontal_axis = null)
        {
            AnnotationPolygon annot;
            annot = new AnnotationPolygon(this,points);
            annotations.Add(annot);
            annot.opacity = opacity;
            annot.lineWidth = lineWidth;
            annot.fillColor = fillColor;
            annot.color = borderColor;
            annot.DrawingLayer = drawingLayer;
            annot.fillPattern = fillPattern;
            annot.patternSize = patternSize;
            annot.Initialize(device);
            annot.horizontalAxis = Horizontal_axis;
            annot.verticalAxis = Vertical_axis;
            ReDrawLayer(drawingLayer);
            annot.Name = name;
            annot.Text = text;
            return annot;
        }

        /// <summary>
        /// Create an annotation shape and add it to the chart area.  and return annotation.
        /// </summary>
        public Annotation AddAnnotationShape(string name,string text, PointF topLeftPoint, PointF bottomRight, bool isRectangleAnnotation, System.Drawing.Color? borderColor, Bitmap bitmap, float lineWidth = 2, float opacity = 1, Charting.Base.AnnotationDrawingLayer drawingLayer = Charting.Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid, Axis Vertical_axis = null, Axis Horizontal_axis = null)
        {
            AnnotationShape annot;
            if ( isRectangleAnnotation)
                annot = new AnnotationRectangle(this);
            else
                annot = new AnnotationEllipse(this);
            annotations.Add(annot);
            annot.startValuePoint = topLeftPoint;
            annot.endValuePoint = bottomRight;
            annot.opacity = opacity;
            annot.Bitmap = bitmap;
            annot.color = borderColor;
            annot.lineWidth = lineWidth;
            annot.DrawingLayer = drawingLayer;
            annot.Initialize(device);
            annot.horizontalAxis = Horizontal_axis;
            annot.verticalAxis = Vertical_axis;
            ReDrawLayer(drawingLayer);
            annot.Name = name;
            Text = text;
            return annot;
        }

        /// <summary>
        /// Create an annotation text and add it to the chart area. and return annotation.
        /// </summary>
        public TextAnnotation AddAnnotationText(string name, string text, PointF position, System.Drawing.Color? borderColor, Font font, Base.FontDrawFlags TextAlign, bool isStaticPositionX = true, bool isStaticPositionY = true, float opacity = 1, Charting.Base.AnnotationDrawingLayer drawingLayer = Charting.Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid, Axis Vertical_axis = null, Axis Horizontal_axis = null)
        {
            TextAnnotation annot = new TextAnnotation(this);
            annotations.Add(annot);  
            annot.IsRelativePositionX = !isStaticPositionX;
            annot.IsRelativePositionY = !isStaticPositionY;
            annot.Position = position;
            annot.opacity = opacity;
            annot.color = borderColor;
            annot.DrawingLayer = drawingLayer;
            annot.Text = text;
            annot.Align = TextAlign;
            annot.Initialize(device);
            annot.TextFont = font;
            annot.Name = name;
            annot.horizontalAxis = Horizontal_axis;
            annot.verticalAxis = Vertical_axis;
            ReDrawLayer(drawingLayer);
            return annot;
        }

        /// <summary>
        /// Remove annotation from annotation collection.
        /// </summary>
        /// <param name="annot"></param>
        /// <returns></returns>
        public bool RemoveAnnotation(Annotation annot)
        {
            var result = this.annotations.Remove(annot);
            OnPaint(null);
            return result;
        }

        /// <summary>
        /// Find and get annotation accourding name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Annotation GetAnnotation(string name)
        {
            return annotations[name];
        }

        /// <summary>
        /// Add a new axes to this chart area.
        /// </summary>
        /// <param name="axes"></param>
        public void AddAxes(Axis axes)
        {
            axes.ParentControl = this;
            Axes.Add(axes);
        }

        /// <summary>
        /// Get the mouse location point. its a float 0-1.
        /// </summary>
        /// <param name="e"> Mouse event args </param>
        /// <returns></returns>
        private PointF GetMouseValue(MouseEventArgs e)
        {
            PointF tmpPoint = new PointF
            ((e.X - Padding.Left) / DrawWidth, 1 - (e.Y - Padding.Top) / DrawHeight);
            if (tmpPoint.X > 1) tmpPoint.X = 1;
            if (tmpPoint.X < 0) tmpPoint.X = 0;

            if (tmpPoint.Y > 1) tmpPoint.Y = 1;
            if (tmpPoint.Y < 0) tmpPoint.Y = 0;
            return tmpPoint;
        }

        /// <summary>
        /// Get mouse relative point.
        /// </summary>
        /// <returns></returns>
        public PointF GetMousePosition()
        {
            PointF mousep =PointToClient(MousePosition);
            PointF tmpPoint = new PointF
            ((mousep.X - Padding.Left) / DrawWidth, 1 - (mousep.Y - Padding.Top) / DrawHeight);
            if (tmpPoint.X > 1) tmpPoint.X = 1;
            if (tmpPoint.X < 0) tmpPoint.X = 0;

            if (tmpPoint.Y > 1) tmpPoint.Y = 1;
            if (tmpPoint.Y < 0) tmpPoint.Y = 0;
            return tmpPoint;

        }

        public void DrawOnGraphic(Graphics g, Rectangle? rec=null)
        {
            var pos = new Point(0, 0);
            if (rec != null)
            {
                RecalculateContent(rec.Value.Size);
              //  g.Clip = new System.Drawing.Region(new Rectangle(0, 0, ((Size)graphicsSize).Width, ((Size)graphicsSize).Height));//.Height,((System.Drawing.Size)graphicsSize).Width);
                pos = rec.Value.Location;
            }
            
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.ResetTransform();
           // g.TranslateTransform(x, y);
            for (int index = 0; index < annotations.Count; index++)
            {
                if (annotations[index].DrawingLayer == Base.AnnotationDrawingLayer.BackOfAll)
                    annotations[index].Draw(g,pos);
            }
            g.ResetTransform();
           // g.TranslateTransform(x, y);
            if (IsGridShow)
                grid.Draw(g,pos);
            g.ResetTransform();
           // g.TranslateTransform(x, y);
            for (int index = 0; index < annotations.Count; index++)
            {
                if (annotations[index].DrawingLayer == Base.AnnotationDrawingLayer.FrontOfGrid)
                    annotations[index].Draw(g,pos);
            }
            g.ResetTransform();
           // g.TranslateTransform(x, y);
            for (int index = 0; index < Axes.Count; index++)
            {
                Axes[index].Draw(g,pos);
            }
            g.ResetTransform();
          //  g.TranslateTransform(x, y);
            for (int index = 0; index < annotations.Count; index++)
            {
                if (annotations[index].DrawingLayer == Base.AnnotationDrawingLayer.FrontOfAxisAndGrid)
                    annotations[index].Draw(g,pos);
            }
            g.ResetTransform();
         //   g.TranslateTransform(x, y);
           // g.Clip = new Region(new Rectangle(Padding.Left+pos.X, Padding.Top+pos.Y, Width, Height));
            //g.TranslateTransform(Padding.Left, Padding.Top);
            for (int index = 0; index < series.Count; index++)
            {
                if (!series[index].IsTopMost)
                    series[index].Draw(g, new Point(pos.X+Padding.Left,pos.Y+Padding.Top));
            }
            for (int index = 0; index < series.Count; index++)
            {
                if (series[index].IsTopMost)
                    series[index].Draw(g, new Point(pos.X + Padding.Left, pos.Y + Padding.Top));
            }
            g.ResetTransform();
           // g.TranslateTransform(x, y);
            for (int index = 0; index < annotations.Count; index++)
            {
                if (annotations[index].DrawingLayer == Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid)
                    annotations[index].Draw(g,pos);
            }
            RecalculateContent();
        }
       
        #endregion

        #region Overriding
        protected override void OnPaddingChanged(System.EventArgs e)
        {
            base.OnPaddingChanged(e);
            MinimumSize = new System.Drawing.Size(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RecalculateContent();
            //DrawBack();
            Draw();  //base.OnPaint(e);
        }
       
        protected override void Dispose(bool disposing)
        {
            if (IsInRunMode)
                try
                {
                    if (device != null)
                        device.Dispose();
                }
                catch (System.Exception ex)
                {
                    xPFT.Exceptions.ExceptionHandler.LogError(ex);
                }
                finally
                {
                    base.Dispose(false);
                    System.GC.Collect();
                }
        }

        //protected override void OnParentChanged(System.EventArgs e)
        //{
        //    if (Parent.GetType() == typeof(Chart))
        //    {
        //        base.OnParentChanged(e);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            Parent.Controls.Remove(this);
        //        }
        //        catch { }
        //    }
        //}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (isReactionEnable)
            {
                if (annotations != null) annotations.MouseDown(new PointF(e.X, e.Y), e);
                if (series != null) series.MouseDown(new PointF(e.X - Padding.Left, e.Y - Padding.Top), e);
                if (Axes != null) Axes.MouseDown(new PointF(e.X, e.Y), e);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isReactionEnable)
            {
                if (annotations != null) annotations.MouseUp(new PointF(e.X, e.Y), e);
                if (series != null) series.MouseUp(new PointF(e.X - Padding.Left, e.Y - Padding.Top), e);
                if (Axes != null) Axes.MouseUp(new PointF(e.X, e.Y), e);
            }
        }


        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (isReactionEnable)
            {
                if (annotations != null) annotations.MouseClick(new PointF(e.X, e.Y), e);
                if (series != null) series.MouseClick(new PointF(e.X - Padding.Left, e.Y - Padding.Top), e);
                if (Axes != null) Axes.MouseClick(new PointF(e.X, e.Y), e);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
              if (isReactionEnable)
            {
                if (annotations != null) annotations.MouseDoubleClick(new PointF(e.X, e.Y), e);
                if (series != null) series.MouseDoubleClick(new PointF(e.X - Padding.Left, e.Y - Padding.Top), e);
                if (Axes != null) Axes.MouseDoubleClick(new PointF(e.X, e.Y), e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isReactionEnable)
            {
                if(annotations!=null) annotations.MouseMove(new PointF(e.X, e.Y), e);
                if (series != null) series.MouseMove(new PointF(e.X - Padding.Left, e.Y - Padding.Top), e);
                if (Axes != null) Axes.MouseMove(new PointF(e.X, e.Y), e);
            }
        }
        #endregion
    }
}