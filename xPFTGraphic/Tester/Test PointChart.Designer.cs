namespace Tester
{
    partial class Test_PointChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chartArea1 = new xPFT.Charting.ChartArea();
            this.SuspendLayout();
            // 
            // chartArea1
            // 
            this.chartArea1.AxisRelate = null;
            this.chartArea1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartArea1.HeightRatio = 1F;
            this.chartArea1.IsGridShow = true;
            this.chartArea1.IsRealTimeChart = true;
            this.chartArea1.IsReactionEnable = true;
            this.chartArea1.Location = new System.Drawing.Point(0, 0);
            this.chartArea1.Name = "chartArea1";
            this.chartArea1.RefreshRate = 33F;
            this.chartArea1.Size = new System.Drawing.Size(990, 448);
            this.chartArea1.TabIndex = 0;
            this.chartArea1.Text = "chartArea1";
            this.chartArea1.WidthRatio = 1F;
            // 
            // Test_PointChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 473);
            this.Controls.Add(this.chartArea1);
            this.Name = "Test_PointChart";
            this.Text = "Test_PointChart";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Test_PointChart_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion

        private xPFT.Charting.ChartArea chartArea1;
    }
}