using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    { 
        System.Diagnostics.Stopwatch sv = new System.Diagnostics.Stopwatch();
        public Form1()
        {   sv.Start();
            InitializeComponent();

            serPoint = FillPoints();

           // NewMethod2();
        }
        private xPFT.Charting.ChartArea chartArea1;
        private xPFT.Charting.ChartArea chartArea3;



        private void NewMethod2()
        {
            // 
            // chartArea1
            // 
            this.chartArea1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chartArea1.AxisRelate = null;
            this.chartArea1.BackColor = System.Drawing.Color.White;
            this.chartArea1.HeightRatio = 1F;
            this.chartArea1.IsGridShow = true;
            this.chartArea1.IsReactionEnable = true;
            this.chartArea1.IsRealTimeChart = true;
            this.chartArea1.Location = new System.Drawing.Point(7, 5);
            this.chartArea1.Name = "chartArea1";
            this.chartArea1.RefreshRate = 33F;
            this.chartArea1.Size = new System.Drawing.Size(Width/2 , 360);
            this.chartArea1.TabIndex = 3;
            this.chartArea1.WidthRatio = 1F;
            // 
            // chartArea3
            // 
            this.chartArea3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartArea3.AxisRelate = null;
            this.chartArea3.BackColor = System.Drawing.Color.White;
            this.chartArea3.HeightRatio = 1F;
            this.chartArea3.IsGridShow = true;
            this.chartArea3.IsReactionEnable = true;
            this.chartArea3.IsRealTimeChart = true;
            this.chartArea3.Location = new System.Drawing.Point(Width / 2, 5);
            this.chartArea3.Name = "chartArea3";
            this.chartArea3.RefreshRate = 33F;
            this.chartArea3.Size = new System.Drawing.Size(Width / 2, 459);
            this.chartArea3.TabIndex = 2;
            this.chartArea3.Text = "chartArea3";
            this.chartArea3.WidthRatio = 1F;

            this.Controls.Add(this.chartArea3);
            this.Controls.Add(this.chartArea1);

            chartArea1.AddAxes(new xPFT.Charting.Axis("a", "For Example Vertical Axis label", xPFT.Charting.AxisDirection.Vertical, -2, 12));
            chartArea1.AddAxes(new xPFT.Charting.Axis("b", "For Example Horizontal Axis label", xPFT.Charting.AxisDirection.Horizontal, Color.Black, Color.Gray, 0, 6, true, 0, chartArea1.Axes[0]));
            chartArea1.Axes[1].PerpendicularAxis = chartArea1.Axes[0];
            
            chartArea1.IsRealTimeChart = false;
            //  Font f = new System.Drawing.Font("consolas", 50, FontStyle.Bold);
            //  var test = chartArea1.AddAnnotationText("adf", "For Example Chart Title", new PointF(Width / 2, 5), Color.Green, f, xPFT.Charting.Base.FontDrawFlags.Center, false, true, 0.3f, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid);
            //  chartArea1.AddAnnotationText("adef", " Milad", new PointF(300, 125), Color.Blue, Font, xPFT.Charting.Base.FontDrawFlags.Center, true, true, 1, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid);
            //  chartArea1.AddAnnotationText("adef", " Milad", new PointF(300, 150), Color.Black, Font, xPFT.Charting.Base.FontDrawFlags.Right, false, false, 1, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid);
            //chartArea1.AxisRelate = 5f;
            //chartArea1.IsRealTimeChart = false;

            // chartArea2.IsGridShow = false;
            chartArea3.IsGridShow = false;
            chartArea3.AddAxes(new xPFT.Charting.Axis("a", "For Example Vertical Axis label", xPFT.Charting.AxisDirection.Vertical, -2, 12));
            chartArea3.AddAxes(new xPFT.Charting.Axis("b", "For Example Horizontal Axis label", xPFT.Charting.AxisDirection.Horizontal, Color.Black, Color.Gray, 0, 6, true, 0, chartArea1.Axes[0]));
            chartArea3.Axes[1].PerpendicularAxis = chartArea3.Axes[0];
           // chartArea3.Axes["b"].Labels=new List<object> {"mosa","naghi","taghi"};
            //chartArea3.IsRealTimeChart = false;

            //  var an = chartArea1.AddAnnotationShape("asdif", "text1", new PointF(5f, 5f), new PointF(8f, 8f), false, null, Color.Red, 2, 1, xPFT.Charting.Base.AnnotationDrawingLayer.BackOfAll, xPFT.Charting.Base.FillPattern.Zig_Zag_Horizontal, 8, chartArea1.Axes[0], chartArea1.Axes[1]);
            //chartArea3.AddAnnotationShape("asdffff", "text1", new PointF(1f, 1f), new PointF(4, 5), true, Color.Red, Color.Blue, 1, 0.5f, xPFT.Charting.Base.AnnotationDrawingLayer.BackOfAll, xPFT.Charting.Base.FillPattern.Zig_Zag_Horizontal, 8, chartArea3.Axes[0], chartArea3.Axes[1]);

            //var polyAn = chartArea3.AddAnnotationShape("asdfd ", "text1", new PointF[]{
            //    new PointF(1f,1f),new PointF(2f,1f),new PointF(4f,4f),new PointF(4f,8f),new PointF(1f,19f)
            //}, Color.Blue, Color.Green, 1, 1, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid, xPFT.Charting.Base.FillPattern.Solid, 8, chartArea3.Axes[0], chartArea3.Axes[1]);
            //chartArea3.IsRealTimeChart = false;
            NewMethod();
            //  seri2 = new xPFT.Charting.Series("adf2", "salam", chartArea1.Axes[1], chartArea1.Axes[0], Color.Red, xPFT.Charting.Base.LineDrawPattern.DASH, 2);
            //   seri.ChartType = xPFT.Charting.SeriesChartType.Point_Star;
            //    chartArea1.AddSeries(seri2);
            //   seri2.MouseDown += seri_MouseDown;
            //  chartArea2.ReDraw();
            // chartArea1.ReDraw();

            // serie2 = new xPFT.Charting.Series("adf", "salam", chartArea3.Axes[1], chartArea3.Axes[0], Color.Black, xPFT.Charting.Base.LineDrawPattern.SOLID);
            // chartArea3.AddSeries(serie2);
            //timer1.Start();
        }
        int numberOfSeries = 10;
        List<xPFT.Charting.Base.DataPoint>[] serPoint;
        private void NewMethod()
        {

          

            for (int i = 0; i < numberOfSeries; i++)
            {
                var serie = new xPFT.Charting.Series("adf", "salam", chartArea1.Axes[1], chartArea1.Axes[0], Color.Black, xPFT.Charting.Base.LineDrawPattern.SOLID);
                var serie2 = new xPFT.Charting.Series("adf", "salam", chartArea3.Axes[1], chartArea3.Axes[0], Color.Black, xPFT.Charting.Base.LineDrawPattern.SOLID);
                serie.MouseEnter += serie_MouseEnter;
                //serie.ChartType = xPFT.Charting.SeriesChartType.Point_Star;

                serie2.MouseEnter += serie_MouseEnter;
                //serie2.ChartType = xPFT.Charting.SeriesChartType.Point_Star;
                serie2.isPointOfChartLinedTogether = true;
                chartArea1.AddSeries(serie);
                chartArea3.AddSeries(serie2);
                serie.AddPoints(serPoint[i].ToArray());
                serie2.AddPoints(serPoint[i].ToArray());
                //serie2.AddPoints(new PointF[]{new PointF(1,2),new PointF(2,6),new PointF(3,2),new PointF(3,5)});
              //  serie.TextPosition = 5;
                serie2.IsDrawPointLabel = true;
                serie.IsDrawPointLabel = true;
                
            }
            //chartArea2.ReDraw();
        }

        private List<xPFT.Charting.Base.DataPoint>[] FillPoints()
        {
            List<xPFT.Charting.Base.DataPoint>[] serPoint = new List<xPFT.Charting.Base.DataPoint>[numberOfSeries];
            for (int j = 0; j < numberOfSeries; j++)
                serPoint[j] = new List<xPFT.Charting.Base.DataPoint>();
            double x = 0;
            for (int i = 0; i < 2000; i++)
            {
                for (int j = 0; j < numberOfSeries; j++)
                    serPoint[j].Add(new xPFT.Charting.Base.DataPoint((float)x, (float)Math.Sin(x * 5 / Math.PI) * 3 + 2 + j * 0.5f,"a"+i.ToString()));
                x += 0.003;
            }
            return serPoint;
        }

        void serie_MouseEnter(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            ((xPFT.Charting.Series)sender).LineColor = Color.Red;
            chartArea1.ReDraw(sender);
            chartArea3.ReDraw(sender);
        }

        void serie_MouseClick(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            ((xPFT.Charting.Series)sender).Visible = false;
            chartArea1.ReDraw(sender);
        }
        void polyAn_MouseDown(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ((xPFT.Charting.AnnotationPolygon)sender).Opacity = (1);
            else
                ((xPFT.Charting.AnnotationPolygon)sender).Opacity = (0.3f);
        }

        void Form1_MouseDown(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                chartArea1.Axes[0].Maximum=(chartArea1.Axes[0].Maximum * 2);
            if (e.Button == MouseButtons.Left)
                chartArea1.Axes[0].Maximum=(chartArea1.Axes[0].Maximum / 2);
            chartArea1.ReDraw();
        }

        float x = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {

            x += 0.09f;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            sv.Stop();
           // MessageBox.Show(sv.ElapsedMilliseconds.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += pd_PrintPage;
            pd.Print();

            //Form2 frm2 = new Form2();
            //frm2.pictureBox1.Image = chartArea1.GetImage();
            //frm2.Show();
        }

        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            chartArea3.DrawOnGraphic(g,new Rectangle(150,150,500,500));
        }
        
        void seri_MouseDown(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
            ((xPFT.Charting.Series)sender).IsTopMost=( !((xPFT.Charting.Series)sender).IsTopMost);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // NewMethod();
           // chartArea1.ReDraw();
      
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var anl0 = chartArea1.AddAnnotationLine("asdf", "text1", 3f, true, Color.Red, 1, 1, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfGrid, chartArea1.Axes[0], chartArea1.Axes[1]);
            anl0.LinePattern = xPFT.Charting.Base.LineDrawPattern.DASH_DOT;
            chartArea1.ReDraw(anl0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chart3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //NewMethod2();
            //chartArea3.ReDraw();
            Text = xPFT.Charting.ChartArea.TimingResult.ToString();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var red = new System.Diagnostics.Stopwatch();
            red.Start();
            NewMethod2();
           // chartArea1.Refresh();
            Text = sv.ElapsedMilliseconds.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += pd_PrintPage;
            pd.Print();
        }
    }
}
