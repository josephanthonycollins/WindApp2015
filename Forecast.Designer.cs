namespace WindApp2015
{
    partial class Forecast
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.ForecastLoadbt = new System.Windows.Forms.Button();
            this.Forecastlb = new System.Windows.Forms.ListBox();
            this.Mdgv = new System.Windows.Forms.DataGridView();
            this.ForecastMlbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ForecastRun = new System.Windows.Forms.Button();
            this.ForecastP1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ForecastP2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ForecastP3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mdgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForecastP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForecastP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForecastP3)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(173, 352);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(97, 20);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ForecastLoadbt
            // 
            this.ForecastLoadbt.Location = new System.Drawing.Point(28, 12);
            this.ForecastLoadbt.Name = "ForecastLoadbt";
            this.ForecastLoadbt.Size = new System.Drawing.Size(148, 45);
            this.ForecastLoadbt.TabIndex = 1;
            this.ForecastLoadbt.Text = "Load Data";
            this.ForecastLoadbt.UseVisualStyleBackColor = true;
            this.ForecastLoadbt.Click += new System.EventHandler(this.ForecastLoadbt_Click);
            // 
            // Forecastlb
            // 
            this.Forecastlb.FormattingEnabled = true;
            this.Forecastlb.Location = new System.Drawing.Point(28, 90);
            this.Forecastlb.Name = "Forecastlb";
            this.Forecastlb.Size = new System.Drawing.Size(319, 108);
            this.Forecastlb.TabIndex = 2;
            this.Forecastlb.SelectedValueChanged += new System.EventHandler(this.Forecastlb_SelectedValueChanged_1);
            // 
            // Mdgv
            // 
            this.Mdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Mdgv.Location = new System.Drawing.Point(466, 53);
            this.Mdgv.Name = "Mdgv";
            this.Mdgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Mdgv.Size = new System.Drawing.Size(603, 226);
            this.Mdgv.TabIndex = 3;
            this.Mdgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Mdgv_CellClick);
            // 
            // ForecastMlbl
            // 
            this.ForecastMlbl.AutoSize = true;
            this.ForecastMlbl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.ForecastMlbl.Location = new System.Drawing.Point(463, 12);
            this.ForecastMlbl.Name = "ForecastMlbl";
            this.ForecastMlbl.Size = new System.Drawing.Size(233, 13);
            this.ForecastMlbl.TabIndex = 14;
            this.ForecastMlbl.Text = "Fitted ARIMA/ARCH/GARCH Models";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(42, 352);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "n steps ahead:";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(173, 397);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(97, 20);
            this.numericUpDown2.TabIndex = 16;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(45, 397);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "starting point:";
            // 
            // ForecastRun
            // 
            this.ForecastRun.Location = new System.Drawing.Point(45, 461);
            this.ForecastRun.Name = "ForecastRun";
            this.ForecastRun.Size = new System.Drawing.Size(148, 45);
            this.ForecastRun.TabIndex = 18;
            this.ForecastRun.Text = "Run";
            this.ForecastRun.UseVisualStyleBackColor = true;
            this.ForecastRun.Click += new System.EventHandler(this.ForecastRun_Click);
            // 
            // ForecastP1
            // 
            this.ForecastP1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ForecastP1.BorderlineColor = System.Drawing.SystemColors.ButtonFace;
            chartArea1.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Title = "Time Period";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.Title = "Value";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.ForecastP1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ForecastP1.Legends.Add(legend1);
            this.ForecastP1.Location = new System.Drawing.Point(409, 296);
            this.ForecastP1.Name = "ForecastP1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ForecastP1.Series.Add(series1);
            this.ForecastP1.Size = new System.Drawing.Size(450, 250);
            this.ForecastP1.TabIndex = 27;
            this.ForecastP1.Text = "Data";
            // 
            // ForecastP2
            // 
            this.ForecastP2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ForecastP2.BorderlineColor = System.Drawing.SystemColors.ButtonFace;
            chartArea2.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.Title = "Time Period";
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.AxisY.Title = "Value";
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea2.BorderColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.ForecastP2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.ForecastP2.Legends.Add(legend2);
            this.ForecastP2.Location = new System.Drawing.Point(843, 296);
            this.ForecastP2.Name = "ForecastP2";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.IsVisibleInLegend = false;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ForecastP2.Series.Add(series2);
            this.ForecastP2.Size = new System.Drawing.Size(450, 250);
            this.ForecastP2.TabIndex = 28;
            this.ForecastP2.Text = "Data";
            // 
            // ForecastP3
            // 
            this.ForecastP3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ForecastP3.BorderlineColor = System.Drawing.SystemColors.ButtonFace;
            chartArea3.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea3.AxisX.MajorGrid.Enabled = false;
            chartArea3.AxisX.Title = "Time Period";
            chartArea3.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.AxisY.MajorGrid.Enabled = false;
            chartArea3.AxisY.Title = "Value";
            chartArea3.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea3.BorderColor = System.Drawing.Color.Transparent;
            chartArea3.Name = "ChartArea1";
            this.ForecastP3.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.ForecastP3.Legends.Add(legend3);
            this.ForecastP3.Location = new System.Drawing.Point(607, 397);
            this.ForecastP3.Name = "ForecastP3";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.IsVisibleInLegend = false;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.ForecastP3.Series.Add(series3);
            this.ForecastP3.Size = new System.Drawing.Size(450, 250);
            this.ForecastP3.TabIndex = 29;
            this.ForecastP3.Text = "Data";
            // 
            // Forecast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 719);
            this.Controls.Add(this.ForecastP3);
            this.Controls.Add(this.ForecastP2);
            this.Controls.Add(this.ForecastP1);
            this.Controls.Add(this.ForecastRun);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ForecastMlbl);
            this.Controls.Add(this.Mdgv);
            this.Controls.Add(this.Forecastlb);
            this.Controls.Add(this.ForecastLoadbt);
            this.Controls.Add(this.numericUpDown1);
            this.Name = "Forecast";
            this.Text = "Forecast";
            this.Load += new System.EventHandler(this.Forecast_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mdgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForecastP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForecastP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForecastP3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button ForecastLoadbt;
        private System.Windows.Forms.ListBox Forecastlb;
        private System.Windows.Forms.DataGridView Mdgv;
        private System.Windows.Forms.Label ForecastMlbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ForecastRun;
        private System.Windows.Forms.DataVisualization.Charting.Chart ForecastP1;
        private System.Windows.Forms.DataVisualization.Charting.Chart ForecastP2;
        private System.Windows.Forms.DataVisualization.Charting.Chart ForecastP3;

    }
}