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
    public partial class Test_PointChart : Form
    {
        public Test_PointChart()
        {
            InitializeComponent();
            chartArea1.IsGridShow = true;
            chartArea1.AddAxes(new xPFT.Charting.Axis("a", "le", xPFT.Charting.AxisDirection.Vertical, -2, 24));
            chartArea1.IsRealTimeChart = false;
            chartArea1.AddAxes(new xPFT.Charting.Axis("b", "For Example", xPFT.Charting.AxisDirection.Horizontal, Color.Black, Color.Gray, 0, 23, true, 0, chartArea1.Axes[0]));
            chartArea1.Axes[1].Labels = new List<object>
            {
                new DateTime(2015,10,2,18,35,2),
                new DateTime(2015,10,2,19,35,2),
                //new DateTime(2015,10,5,2,35,2),
                //new DateTime(2015,10,12,10,35,2),
                //new DateTime(2015,10,30,6,35,2),
                //new DateTime(2015,10,5,2,35,2),
                //new DateTime(2015,10,12,10,35,2),
            };
            //chartArea1.Axes[1].LabelSize=new Size(80,20);
            //chartArea1.Axes[0].LabelSize = new Size(80, 30);
            chartArea1.Axes[0].SpaceBetweenLabel = 5;
            chartArea1.Axes[1].Font = new System.Drawing.Font("Consolas", 12);
            chartArea1.Axes[1].TitleFont = new System.Drawing.Font("Consolas", 15);
            annot=(xPFT.Charting.AnnotationLine)( chartArea1.AddAnnotationLine("asfd", "asfd", 2, false, Color.Black,5,1,xPFT.Charting.Base.AnnotationDrawingLayer.FrontOfSeriesAndAxisAndGrid,chartArea1.Axes[0],chartArea1.Axes[1]));
            annot.MouseDown+=annot_MouseDown;
            annot.MouseUp += annot_MouseUp;
            chartArea1.MouseMove += chartArea1_MouseMove;
            annot.MouseClick += annot_MouseClick;
            annot.MouseDoubleClick += annot_MouseDoubleClick;
            chartArea1.ReDraw(annot);
            chartArea1.IsGridShow = false;
            xPFT.Charting.Series ser = new xPFT.Charting.Series("df", chartArea1.Axes[1], chartArea1.Axes[0]);
            chartArea1.AddSeries(ser);
           // ser.ChartType = xPFT.Charting.SeriesChartType.Point_Star;
            ser.AddPoint(0, 1);
           // ser.AddPoint(1, 5);
            chartArea1.ReDraw(ser);
        }

        void annot_MouseDoubleClick(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
            xPFT.Charting.Series ser = new xPFT.Charting.Series("df", chartArea1.Axes[1], chartArea1.Axes[0]);
            chartArea1.AddSeries(ser);
            ser.AddPoint(0, 1);
            ser.AddPoint(1, 2);
            ser.AddPoint(2, 1);
            chartArea1.ReDraw(ser);
        }

        void annot_MouseClick(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
              annot.Value -= 0.1F;
            chartArea1.ReDraw(annot);
        }


        void chartArea1_MouseMove(object sender, MouseEventArgs e)
        {
            if (b)
            {
                annot.Value = chartArea1.GetMousePosition().X * chartArea1.Axes[1].Maximum;
                chartArea1.ReDraw(annot);
            }
        }
        xPFT.Charting.AnnotationLine annot;

        void annot_MouseUp(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
            b = false;
        }
        bool b = false;
        private void annot_MouseDown(xPFT.Charting.ActiveElement sender, MouseEventArgs e)
        {
            b = true;
        }

        private void Test_PointChart_MouseClick(object sender, MouseEventArgs e)
        {
        }
    }
}
