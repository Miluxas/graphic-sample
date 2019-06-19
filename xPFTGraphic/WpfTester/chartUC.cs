using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xPFT.Charting;
using xPFT.Charting.Base;

namespace WpfTester
{
    public partial class chartUC : UserControl
    {
        public chartUC()
        {
            InitializeComponent();

            ChartArea chartArea1 = new ChartArea();
            chartArea1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(chartArea1);
            chartArea1.AddAxes(new xPFT.Charting.Axis("a", "For Example Vertical Axis label", xPFT.Charting.AxisDirection.Vertical, -2, 24));
            chartArea1.AddAxes(new xPFT.Charting.Axis("b", "For Example Horizontal Axis label", xPFT.Charting.AxisDirection.Horizontal, Color.Black, Color.Gray, 0, 23, true, 0, chartArea1.Axes[0]));
            //chartArea1.Axes[0].Labels = new List<object>
            //{
            //   "kalim","milim","rahim", "nadir","qadir","ali","hasan","hosine","zinal","naqi","taqi","sajad","javad","reza", "mojtaba","shahin","sorosh","sina","kamal","ahmad",
            //   "rasol","kazim","qambar ali"
            //};
            chartArea1.Axes[1].Font = new System.Drawing.Font("Consolas", 10);
            chartArea1.Axes[1].TitleFont = new System.Drawing.Font("Consolas", 28);
            chartArea1.Axes[1].LineColor=(Color.Green);
            chartArea1.Axes[1].TitleColor = Color.Red;
            chartArea1.Axes[1].LabelCount = 5;
            chartArea1.IsGridShow = false;
            chartArea1.Axes[1].PerpendicularAxis = chartArea1.Axes[0];

            var anl0 = chartArea1.AddAnnotationLine("asdf", "text1", 3f, true, Color.Red, 2, 1, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid, chartArea1.Axes[0], chartArea1.Axes[1]);
            var an = chartArea1.AddAnnotationShape("asdif", "text1", new PointF(5f, 5f), new PointF(8f, 8f), false, null, Color.Red, 2, 1, xPFT.Charting.Base.AnnotationDrawingLayer.BackOfAll, xPFT.Charting.Base.FillPattern.Zig_Zag_Horizontal, 8, chartArea1.Axes[0], chartArea1.Axes[1]);
            chartArea1.AddAnnotationShape("asdffff", "text1", new PointF(0f, 0f), new PointF(20f, 10f), false, null, Color.Blue, 1, 0.5f, xPFT.Charting.Base.AnnotationDrawingLayer.BackOfAll, xPFT.Charting.Base.FillPattern.Zig_Zag_Horizontal, 8, chartArea1.Axes[0], chartArea1.Axes[1]);
            var polyAn = chartArea1.AddAnnotationShape("asdfd ", "text1", new PointF[]{
                new PointF(1f,1f),new PointF(2f,1f),new PointF(7f,4f),new PointF(5f,8f),new PointF(1f,19f)
            }, Color.Blue, Color.Green, 0.5f, 0.5f, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid, xPFT.Charting.Base.FillPattern.Solid, 8, chartArea1.Axes[0], chartArea1.Axes[1]);
            Font f = new System.Drawing.Font("consolas", 50, FontStyle.Bold);
            var test = chartArea1.AddAnnotationText("adf", "For Example Chart Title", new PointF(Width / 2, 5), Color.Green, f, xPFT.Charting.Base.FontDrawFlags.Center, false, true, 0.3f, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid);
            chartArea1.AddAnnotationText("adef", "Masha Milad", new PointF(300, 125), Color.Blue, Font, xPFT.Charting.Base.FontDrawFlags.Center, true, true, 1, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid);
            chartArea1.AddAnnotationText("adef", "Masha Milad", new PointF(300, 150), Color.Black, Font, xPFT.Charting.Base.FontDrawFlags.Right, false, false, 1, xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfAxisAndGrid);
            chartArea1.IsRealTimeChart = false;

            seri = new xPFT.Charting.Series("adf", "salam", chartArea1.Axes[1], chartArea1.Axes[0], Color.Black, xPFT.Charting.Base.LineDrawPattern.SOLID);
            chartArea1.AddSeries(seri);

            seri2 = new xPFT.Charting.Series("adf2", "salam", chartArea1.Axes[1], chartArea1.Axes[0], Color.Red, xPFT.Charting.Base.LineDrawPattern.SOLID, 2);
            chartArea1.AddSeries(seri2);

        }
        xPFT.Charting.Series seri, seri2;
        private void button1_Click(object sender, EventArgs e)
        {
            double x = 0;
            List<PointF> points2 = new List<PointF>();
            List<PointF> points3 = new List<PointF>();
            for (int i = 0; i < 3000; i++)
            {
                points3.Add(new PointF((float)x, (float)Math.Sin(x * 5 / Math.PI) * 3 + 2.5f));
                points2.Add(new PointF((float)x, (float)Math.Sin(x * 5 / Math.PI) * 2 + 3));
                x += 23f / 3000f;
            }
            seri.AddPoints(points2.ToArray());
            seri2.AddPoints(points3.ToArray());
        }
    }
}
