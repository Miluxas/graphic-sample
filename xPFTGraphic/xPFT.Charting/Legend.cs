using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xPFT.Charting
{
    public partial class Legend : UserControl
    {
        public Legend()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the legend. this legend note the series of the series collection.
        /// </summary>
        /// <param name="series"></param>
        public void Initialize(SeriesCollection series)
        {
            int y=0;
            foreach (Series ser in series)
            {
                Label tmpLabel=new Label();
                Panel tmpPanel = new Panel();
                tmpPanel.Height = 15;
                tmpPanel.Width = 45;
                tmpPanel.Top = y;
                tmpPanel.Left = 2;
                tmpLabel.Text=ser.Name;
                tmpPanel.Paint += tmpPanel_Paint;
                tmpPanel.ForeColor = ser.LineColor;
                tmpPanel.Tag = ser;
                this.Controls.Add(tmpPanel);
                this.Controls.Add(tmpLabel);
                tmpLabel.Top=y;
                tmpLabel.Left=4+tmpPanel.Width;
                tmpLabel.Font = new System.Drawing.Font(tmpLabel.Font, FontStyle.Bold);
                y += tmpLabel.Height;
            }
            Height = y;
        }

        /// <summary>
        /// This method run when panel is painting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmpPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel pnlSender=((Panel)sender);
            switch (((Series)pnlSender.Tag).ChartType)
            {
                case SeriesChartType.Line:
                    Pen pen=new Pen(pnlSender.ForeColor,2);
                    pen.DashPattern = Charting.Base.GetLineDrawPattern.ForD2D1(((Series)pnlSender.Tag).LinePattern);
                    e.Graphics.DrawLine(pen,
                        new Point(0, pnlSender.Height / 2), new Point(pnlSender.Width, pnlSender.Height / 2));
                    break;
                case SeriesChartType.Point_X:
                    for (int i = 0; i < 4; i++)
                    {
                        e.Graphics.DrawLine(new Pen(pnlSender.ForeColor, 1),
                            new PointF(i * 10, 5), new PointF(i * 10 + 5, 10));
                        e.Graphics.DrawLine(new Pen(pnlSender.ForeColor, 1),
                            new PointF(i * 10, 10), new PointF(i * 10 + 5, 5));
                    }
                    break;
                case SeriesChartType.Point_Star:
                    for (int i = 0; i < 4; i++)
                    {
                        e.Graphics.DrawLines(new Pen(pnlSender.ForeColor, 1), GetDrawStarArray(3.5f+i * 10, pnlSender.Height / 2, 5).ToArray());
                    }
                    break;
                case SeriesChartType.Point_Diamond:
                    for (int i = 0; i < 4; i++)
                    {
                        e.Graphics.DrawLines(new Pen(pnlSender.ForeColor, 1), GetDrawDiamondArray(3.5f + i * 10, pnlSender.Height / 2, 5).ToArray());
                    }
                    break;
                case SeriesChartType.Point_Ellipse:
                    for (int i = 0; i < 4; i++)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(pnlSender.ForeColor), i * 10, 5, 5, 5);
                    }
                    break;
                case SeriesChartType.Point_Rectangle:
                    for (int i = 0; i < 4; i++)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(pnlSender.ForeColor), i * 10, 5, 5, 5);
                    }
                    break;
            }
        }

        /// <summary>
        /// Get the star point array.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<PointF> GetDrawStarArray(float x, float y, float r)
        {
            if (r < 2.5f) r = 2.5f;
            List<PointF> vecList = new List<PointF>();
            for (float i = 0; i <= 5; i++)
            {
                vecList.Add(new PointF((float)(x + r * Math.Sin(Math.PI + i * 4 * Math.PI / 5)),
                    (float)(y + r * Math.Cos(Math.PI + i * 4 * Math.PI / 5))));
            }
            return vecList;
        }

        /// <summary>
        /// Get the diamond point array.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<PointF> GetDrawDiamondArray(float x, float y, float r)
        {
            if (r < 5) r = 5;
            List<PointF> vecList = new List<PointF>();

            vecList.Add(new PointF(x, y - r / 2));
            vecList.Add(new PointF(x + r / 2, y));
            vecList.Add(new PointF(x, y + r / 2));
            vecList.Add(new PointF(x - r / 2, y));
            vecList.Add(new PointF(x, y - r / 2));

            return vecList;
        }
        private void Legend_Load(object sender, EventArgs e)
        {

        }
    }
}
