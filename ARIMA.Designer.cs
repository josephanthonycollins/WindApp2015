namespace WindApp2015
{
    partial class ARIMA
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ARIMALoadbt = new System.Windows.Forms.Button();
            this.ARIMAlb = new System.Windows.Forms.ListBox();
            this.ARIMAac1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ARIMAdiff = new System.Windows.Forms.Button();
            this.ARIMAac2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ARIMAmodelbt = new System.Windows.Forms.Button();
            this.ARIMAdgv = new System.Windows.Forms.DataGridView();
            this.ARIMAresidacf = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ARIMAhist = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ARIMAparameterdgv = new System.Windows.Forms.DataGridView();
            this.ARIMAresidqqplot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ARIMAplbl = new System.Windows.Forms.Label();
            this.ARIMArlbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ARIMAroutput = new System.Windows.Forms.Label();
            this.ARIMAubt = new System.Windows.Forms.Button();
            this.ARIMAsquaredresiduals = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ARIMAsave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAac1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAac2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAdgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAresidacf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAhist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAparameterdgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAresidqqplot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAsquaredresiduals)).BeginInit();
            this.SuspendLayout();
            // 
            // ARIMALoadbt
            // 
            this.ARIMALoadbt.Location = new System.Drawing.Point(29, 36);
            this.ARIMALoadbt.Name = "ARIMALoadbt";
            this.ARIMALoadbt.Size = new System.Drawing.Size(123, 48);
            this.ARIMALoadbt.TabIndex = 0;
            this.ARIMALoadbt.Text = "Load Data";
            this.ARIMALoadbt.UseVisualStyleBackColor = true;
            this.ARIMALoadbt.Click += new System.EventHandler(this.ARIMALoadbt_Click);
            // 
            // ARIMAlb
            // 
            this.ARIMAlb.FormattingEnabled = true;
            this.ARIMAlb.Location = new System.Drawing.Point(29, 131);
            this.ARIMAlb.Name = "ARIMAlb";
            this.ARIMAlb.Size = new System.Drawing.Size(259, 121);
            this.ARIMAlb.TabIndex = 1;
            this.ARIMAlb.SelectedValueChanged += new System.EventHandler(this.ARIMAlb_SelectedValueChanged_1);
            // 
            // ARIMAac1
            // 
            this.ARIMAac1.BackColor = System.Drawing.SystemColors.ButtonFace;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.Title = "Lag";
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.ARIMAac1.ChartAreas.Add(chartArea1);
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Name = "Legend1";
            legend1.ShadowColor = System.Drawing.Color.White;
            this.ARIMAac1.Legends.Add(legend1);
            this.ARIMAac1.Location = new System.Drawing.Point(387, 1);
            this.ARIMAac1.Name = "ARIMAac1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ARIMAac1.Series.Add(series1);
            this.ARIMAac1.Size = new System.Drawing.Size(398, 265);
            this.ARIMAac1.TabIndex = 2;
            this.ARIMAac1.Text = "chart1";
            // 
            // ARIMAdiff
            // 
            this.ARIMAdiff.Location = new System.Drawing.Point(830, 36);
            this.ARIMAdiff.Name = "ARIMAdiff";
            this.ARIMAdiff.Size = new System.Drawing.Size(123, 48);
            this.ARIMAdiff.TabIndex = 3;
            this.ARIMAdiff.Text = "Differencing";
            this.ARIMAdiff.UseVisualStyleBackColor = true;
            this.ARIMAdiff.Click += new System.EventHandler(this.ARIMAdiff_Click);
            // 
            // ARIMAac2
            // 
            this.ARIMAac2.BackColor = System.Drawing.SystemColors.ButtonFace;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorTickMark.Enabled = false;
            chartArea2.AxisX.Title = "Lag";
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.Name = "ChartArea1";
            this.ARIMAac2.ChartAreas.Add(chartArea2);
            legend2.DockedToChartArea = "ChartArea1";
            legend2.Name = "Legend1";
            legend2.ShadowColor = System.Drawing.Color.White;
            this.ARIMAac2.Legends.Add(legend2);
            this.ARIMAac2.Location = new System.Drawing.Point(971, 1);
            this.ARIMAac2.Name = "ARIMAac2";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ARIMAac2.Series.Add(series2);
            this.ARIMAac2.Size = new System.Drawing.Size(398, 265);
            this.ARIMAac2.TabIndex = 4;
            this.ARIMAac2.Text = "chart1";
            // 
            // ARIMAmodelbt
            // 
            this.ARIMAmodelbt.Location = new System.Drawing.Point(830, 157);
            this.ARIMAmodelbt.Name = "ARIMAmodelbt";
            this.ARIMAmodelbt.Size = new System.Drawing.Size(123, 48);
            this.ARIMAmodelbt.TabIndex = 5;
            this.ARIMAmodelbt.Text = "Run ARIMA models";
            this.ARIMAmodelbt.UseVisualStyleBackColor = true;
            this.ARIMAmodelbt.Click += new System.EventHandler(this.ARIMAmodelbt_Click);
            // 
            // ARIMAdgv
            // 
            this.ARIMAdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ARIMAdgv.Location = new System.Drawing.Point(29, 322);
            this.ARIMAdgv.Name = "ARIMAdgv";
            this.ARIMAdgv.Size = new System.Drawing.Size(259, 180);
            this.ARIMAdgv.TabIndex = 6;
            //this.ARIMAdgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ARIMAdgv_CellContentClick);
            this.ARIMAdgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ARIMAdgv_CellDoubleClick);
            //this.ARIMAdgv.SelectionChanged += new System.EventHandler(this.ARIMAdgv_SelectionChanged);
            // 
            // ARIMAresidacf
            // 
            this.ARIMAresidacf.BackColor = System.Drawing.SystemColors.ButtonFace;
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea3.AxisX.Title = "Lag";
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea3.BorderColor = System.Drawing.SystemColors.ButtonFace;
            chartArea3.Name = "ChartArea1";
            this.ARIMAresidacf.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.ARIMAresidacf.Legends.Add(legend3);
            this.ARIMAresidacf.Location = new System.Drawing.Point(387, 272);
            this.ARIMAresidacf.Name = "ARIMAresidacf";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.ARIMAresidacf.Series.Add(series3);
            this.ARIMAresidacf.Size = new System.Drawing.Size(398, 265);
            this.ARIMAresidacf.TabIndex = 7;
            this.ARIMAresidacf.Text = "chart1";
            //this.ARIMAresidacf.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ARIMAresidacf_MouseClick);
            // 
            // ARIMAhist
            // 
            this.ARIMAhist.BackColor = System.Drawing.SystemColors.ButtonFace;
            chartArea4.AxisX.MajorGrid.Enabled = false;
            chartArea4.AxisY.MajorGrid.Enabled = false;
            chartArea4.Name = "ChartArea1";
            this.ARIMAhist.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.ARIMAhist.Legends.Add(legend4);
            this.ARIMAhist.Location = new System.Drawing.Point(971, 272);
            this.ARIMAhist.Name = "ARIMAhist";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.ARIMAhist.Series.Add(series4);
            this.ARIMAhist.Size = new System.Drawing.Size(398, 265);
            this.ARIMAhist.TabIndex = 8;
            this.ARIMAhist.Text = "chart1";
            // 
            // ARIMAparameterdgv
            // 
            this.ARIMAparameterdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ARIMAparameterdgv.Location = new System.Drawing.Point(29, 608);
            this.ARIMAparameterdgv.Name = "ARIMAparameterdgv";
            this.ARIMAparameterdgv.Size = new System.Drawing.Size(347, 39);
            this.ARIMAparameterdgv.TabIndex = 9;
            // 
            // ARIMAresidqqplot
            // 
            this.ARIMAresidqqplot.BackColor = System.Drawing.SystemColors.ButtonFace;
            chartArea5.AxisX.MajorGrid.Enabled = false;
            chartArea5.AxisY.MajorGrid.Enabled = false;
            chartArea5.Name = "ChartArea1";
            this.ARIMAresidqqplot.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.ARIMAresidqqplot.Legends.Add(legend5);
            this.ARIMAresidqqplot.Location = new System.Drawing.Point(387, 531);
            this.ARIMAresidqqplot.Name = "ARIMAresidqqplot";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.ARIMAresidqqplot.Series.Add(series5);
            this.ARIMAresidqqplot.Size = new System.Drawing.Size(398, 261);
            this.ARIMAresidqqplot.TabIndex = 10;
            this.ARIMAresidqqplot.Text = "chart1";
            // 
            // ARIMAplbl
            // 
            this.ARIMAplbl.AutoSize = true;
            this.ARIMAplbl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.ARIMAplbl.Location = new System.Drawing.Point(31, 576);
            this.ARIMAplbl.Name = "ARIMAplbl";
            this.ARIMAplbl.Size = new System.Drawing.Size(186, 13);
            this.ARIMAplbl.TabIndex = 11;
            this.ARIMAplbl.Text = "Model Parameter Estimates";
            // 
            // ARIMArlbl
            // 
            this.ARIMArlbl.AutoSize = true;
            this.ARIMArlbl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.ARIMArlbl.Location = new System.Drawing.Point(26, 697);
            this.ARIMArlbl.Name = "ARIMArlbl";
            this.ARIMArlbl.Size = new System.Drawing.Size(126, 13);
            this.ARIMArlbl.TabIndex = 12;
            this.ARIMArlbl.Text = "Residual Statistics";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 735);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "[mean, stdev, skew, kurtosis] =";
            // 
            // ARIMAroutput
            // 
            this.ARIMAroutput.AutoSize = true;
            this.ARIMAroutput.Location = new System.Drawing.Point(253, 735);
            this.ARIMAroutput.Name = "ARIMAroutput";
            this.ARIMAroutput.Size = new System.Drawing.Size(35, 13);
            this.ARIMAroutput.TabIndex = 14;
            this.ARIMAroutput.Text = "label2";
            // 
            // ARIMAubt
            // 
            this.ARIMAubt.Location = new System.Drawing.Point(830, 311);
            this.ARIMAubt.Name = "ARIMAubt";
            this.ARIMAubt.Size = new System.Drawing.Size(123, 48);
            this.ARIMAubt.TabIndex = 15;
            this.ARIMAubt.Text = "Fit Specific Model";
            this.ARIMAubt.UseVisualStyleBackColor = true;
            this.ARIMAubt.Click += new System.EventHandler(this.ARIMAubt_Click);
            // 
            // ARIMAsquaredresiduals
            // 
            this.ARIMAsquaredresiduals.BackColor = System.Drawing.SystemColors.ButtonFace;
            chartArea6.AxisX.MajorGrid.Enabled = false;
            chartArea6.AxisX.MajorTickMark.Enabled = false;
            chartArea6.AxisX.Title = "Lag";
            chartArea6.AxisY.MajorGrid.Enabled = false;
            chartArea6.Name = "ChartArea1";
            this.ARIMAsquaredresiduals.ChartAreas.Add(chartArea6);
            legend6.DockedToChartArea = "ChartArea1";
            legend6.Name = "Legend1";
            legend6.ShadowColor = System.Drawing.Color.White;
            this.ARIMAsquaredresiduals.Legends.Add(legend6);
            this.ARIMAsquaredresiduals.Location = new System.Drawing.Point(971, 543);
            this.ARIMAsquaredresiduals.Name = "ARIMAsquaredresiduals";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.ARIMAsquaredresiduals.Series.Add(series6);
            this.ARIMAsquaredresiduals.Size = new System.Drawing.Size(398, 265);
            this.ARIMAsquaredresiduals.TabIndex = 16;
            this.ARIMAsquaredresiduals.Text = "chart1";
            // 
            // ARIMAsave
            // 
            this.ARIMAsave.Location = new System.Drawing.Point(830, 432);
            this.ARIMAsave.Name = "ARIMAsave";
            this.ARIMAsave.Size = new System.Drawing.Size(123, 48);
            this.ARIMAsave.TabIndex = 17;
            this.ARIMAsave.Text = "Save ARIMA models";
            this.ARIMAsave.UseVisualStyleBackColor = true;
            this.ARIMAsave.Click += new System.EventHandler(this.ARIMAsave_Click);
            // 
            // ARIMA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1369, 796);
            this.Controls.Add(this.ARIMAsave);
            this.Controls.Add(this.ARIMAsquaredresiduals);
            this.Controls.Add(this.ARIMAubt);
            this.Controls.Add(this.ARIMAroutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ARIMArlbl);
            this.Controls.Add(this.ARIMAplbl);
            this.Controls.Add(this.ARIMAresidqqplot);
            this.Controls.Add(this.ARIMAparameterdgv);
            this.Controls.Add(this.ARIMAhist);
            this.Controls.Add(this.ARIMAresidacf);
            this.Controls.Add(this.ARIMAdgv);
            this.Controls.Add(this.ARIMAmodelbt);
            this.Controls.Add(this.ARIMAac2);
            this.Controls.Add(this.ARIMAdiff);
            this.Controls.Add(this.ARIMAac1);
            this.Controls.Add(this.ARIMAlb);
            this.Controls.Add(this.ARIMALoadbt);
            this.Name = "ARIMA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ARIMA";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ARIMA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAac1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAac2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAdgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAresidacf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAhist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAparameterdgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAresidqqplot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARIMAsquaredresiduals)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ARIMALoadbt;
        private System.Windows.Forms.ListBox ARIMAlb;
        private System.Windows.Forms.DataVisualization.Charting.Chart ARIMAac1;
        private System.Windows.Forms.Button ARIMAdiff;
        private System.Windows.Forms.DataVisualization.Charting.Chart ARIMAac2;
        private System.Windows.Forms.Button ARIMAmodelbt;
        private System.Windows.Forms.DataGridView ARIMAdgv;
        private System.Windows.Forms.DataVisualization.Charting.Chart ARIMAresidacf;
        private System.Windows.Forms.DataVisualization.Charting.Chart ARIMAhist;
        private System.Windows.Forms.DataGridView ARIMAparameterdgv;
        private System.Windows.Forms.DataVisualization.Charting.Chart ARIMAresidqqplot;
        private System.Windows.Forms.Label ARIMAplbl;
        private System.Windows.Forms.Label ARIMArlbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ARIMAroutput;
        private System.Windows.Forms.Button ARIMAubt;
        private System.Windows.Forms.DataVisualization.Charting.Chart ARIMAsquaredresiduals;
        private System.Windows.Forms.Button ARIMAsave;
    }
}