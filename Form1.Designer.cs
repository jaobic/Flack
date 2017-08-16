namespace Flack
{
    partial class formMain
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel1 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel2 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chtFrequencies = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblBitrate = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblBitDepth = new System.Windows.Forms.Label();
            this.lblChannels = new System.Windows.Forms.Label();
            this.lblSampleRate = new System.Windows.Forms.Label();
            this.imgSpectrograph = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtFrequencies)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgSpectrograph)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 255);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(834, 193);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chtFrequencies);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(834, 193);
            this.splitContainer1.SplitterDistance = 390;
            this.splitContainer1.TabIndex = 0;
            // 
            // chtFrequencies
            // 
            customLabel1.ForeColor = System.Drawing.Color.Black;
            customLabel1.Text = "0KHz";
            customLabel1.ToPosition = 200D;
            customLabel2.ForeColor = System.Drawing.Color.Black;
            customLabel2.FromPosition = 20000D;
            customLabel2.Text = "22KHz";
            customLabel2.ToPosition = 21500D;
            chartArea1.AxisX.CustomLabels.Add(customLabel1);
            chartArea1.AxisX.CustomLabels.Add(customLabel2);
            chartArea1.AxisX.LabelStyle.IsStaggered = true;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MaximumAutoSize = 100F;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea1.AxisY.Maximum = 0D;
            chartArea1.AxisY.Minimum = -100D;
            chartArea1.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated270;
            chartArea1.AxisY.Title = "dB";
            chartArea1.Name = "ChartArea1";
            this.chtFrequencies.ChartAreas.Add(chartArea1);
            this.chtFrequencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chtFrequencies.Location = new System.Drawing.Point(0, 0);
            this.chtFrequencies.Name = "chtFrequencies";
            this.chtFrequencies.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chtFrequencies.Series.Add(series1);
            this.chtFrequencies.Size = new System.Drawing.Size(390, 193);
            this.chtFrequencies.TabIndex = 0;
            this.chtFrequencies.Text = "chart1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblBitrate);
            this.groupBox1.Controls.Add(this.lblSize);
            this.groupBox1.Controls.Add(this.lblBitDepth);
            this.groupBox1.Controls.Add(this.lblChannels);
            this.groupBox1.Controls.Add(this.lblSampleRate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 193);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // lblBitrate
            // 
            this.lblBitrate.AutoSize = true;
            this.lblBitrate.Location = new System.Drawing.Point(6, 84);
            this.lblBitrate.Name = "lblBitrate";
            this.lblBitrate.Size = new System.Drawing.Size(83, 13);
            this.lblBitrate.TabIndex = 0;
            this.lblBitrate.Text = "Average Bitrate:";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(6, 68);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(49, 13);
            this.lblSize.TabIndex = 0;
            this.lblSize.Text = "File Size:";
            // 
            // lblBitDepth
            // 
            this.lblBitDepth.AutoSize = true;
            this.lblBitDepth.Location = new System.Drawing.Point(6, 52);
            this.lblBitDepth.Name = "lblBitDepth";
            this.lblBitDepth.Size = new System.Drawing.Size(54, 13);
            this.lblBitDepth.TabIndex = 0;
            this.lblBitDepth.Text = "Bit Depth:";
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new System.Drawing.Point(6, 36);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(57, 13);
            this.lblChannels.TabIndex = 0;
            this.lblChannels.Text = "Channels: ";
            // 
            // lblSampleRate
            // 
            this.lblSampleRate.AutoSize = true;
            this.lblSampleRate.Location = new System.Drawing.Point(6, 20);
            this.lblSampleRate.Name = "lblSampleRate";
            this.lblSampleRate.Size = new System.Drawing.Size(71, 13);
            this.lblSampleRate.TabIndex = 0;
            this.lblSampleRate.Text = "Sample Rate:";
            // 
            // imgSpectrograph
            // 
            this.imgSpectrograph.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.imgSpectrograph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgSpectrograph.Location = new System.Drawing.Point(0, 0);
            this.imgSpectrograph.Name = "imgSpectrograph";
            this.imgSpectrograph.Size = new System.Drawing.Size(834, 255);
            this.imgSpectrograph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgSpectrograph.TabIndex = 1;
            this.imgSpectrograph.TabStop = false;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 448);
            this.Controls.Add(this.imgSpectrograph);
            this.Controls.Add(this.panel1);
            this.Name = "formMain";
            this.Text = "Flack";
            this.Shown += new System.EventHandler(this.formMain_Shown);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtFrequencies)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgSpectrograph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox imgSpectrograph;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtFrequencies;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSampleRate;
        private System.Windows.Forms.Label lblBitrate;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblBitDepth;
        private System.Windows.Forms.Label lblChannels;
    }
}

