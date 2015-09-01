namespace WindApp2015
{
    partial class ARCH
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ARCHLoadbt = new System.Windows.Forms.Button();
            this.ARCHlb = new System.Windows.Forms.ListBox();
            this.ARCHdgv = new System.Windows.Forms.DataGridView();
            this.ARCHmodels = new System.Windows.Forms.Button();
            this.ARCHmodelsdgv = new System.Windows.Forms.DataGridView();
            this.ARCHwhitenoise = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ARCHarimadgv = new System.Windows.Forms.DataGridView();
            this.ARCHarchdgv = new System.Windows.Forms.DataGridView();
            this.ARCHsavebt = new System.Windows.Forms.Button();
            this.ARCHarimalbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHdgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHmodelsdgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHwhitenoise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHarimadgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHarchdgv)).BeginInit();
            this.SuspendLayout();
            // 
            // ARCHLoadbt
            // 
            this.ARCHLoadbt.Location = new System.Drawing.Point(33, 23);
            this.ARCHLoadbt.Name = "ARCHLoadbt";
            this.ARCHLoadbt.Size = new System.Drawing.Size(148, 45);
            this.ARCHLoadbt.TabIndex = 0;
            this.ARCHLoadbt.Text = "Load Data";
            this.ARCHLoadbt.UseVisualStyleBackColor = true;
            this.ARCHLoadbt.Click += new System.EventHandler(this.ARCHLoadbt_Click);
            // 
            // ARCHlb
            // 
            this.ARCHlb.FormattingEnabled = true;
            this.ARCHlb.Location = new System.Drawing.Point(33, 94);
            this.ARCHlb.Name = "ARCHlb";
            this.ARCHlb.Size = new System.Drawing.Size(319, 108);
            this.ARCHlb.TabIndex = 1;
            this.ARCHlb.SelectedValueChanged += new System.EventHandler(this.ARCHlb_SelectedValueChanged);
            // 
            // ARCHdgv
            // 
            this.ARCHdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ARCHdgv.Location = new System.Drawing.Point(602, 94);
            this.ARCHdgv.Name = "ARCHdgv";
            this.ARCHdgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ARCHdgv.Size = new System.Drawing.Size(551, 226);
            this.ARCHdgv.TabIndex = 2;
            //this.ARCHdgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ARCHdgv_CellClick);
            // 
            // ARCHmodels
            // 
            this.ARCHmodels.Location = new System.Drawing.Point(33, 275);
            this.ARCHmodels.Name = "ARCHmodels";
            this.ARCHmodels.Size = new System.Drawing.Size(148, 45);
            this.ARCHmodels.TabIndex = 3;
            this.ARCHmodels.Text = "Fit ARCH and GARCH";
            this.ARCHmodels.UseVisualStyleBackColor = true;
            this.ARCHmodels.Click += new System.EventHandler(this.ARCHmodels_Click);
            // 
            // ARCHmodelsdgv
            // 
            this.ARCHmodelsdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ARCHmodelsdgv.Location = new System.Drawing.Point(33, 506);
            this.ARCHmodelsdgv.Name = "ARCHmodelsdgv";
            this.ARCHmodelsdgv.Size = new System.Drawing.Size(305, 91);
            this.ARCHmodelsdgv.TabIndex = 4;
            this.ARCHmodelsdgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ARCHmodelsdgv_CellClick);
            //this.ARCHmodelsdgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ARCHmodelsdgv_CellContentClick);
            // 
            // ARCHwhitenoise
            // 
            this.ARCHwhitenoise.BackColor = System.Drawing.SystemColors.ButtonFace;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea2.AxisX.Title = "Lag";
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea2.BorderColor = System.Drawing.SystemColors.ButtonFace;
            chartArea2.Name = "ChartArea1";
            this.ARCHwhitenoise.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.ARCHwhitenoise.Legends.Add(legend2);
            this.ARCHwhitenoise.Location = new System.Drawing.Point(866, 379);
            this.ARCHwhitenoise.Name = "ARCHwhitenoise";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ARCHwhitenoise.Series.Add(series2);
            this.ARCHwhitenoise.Size = new System.Drawing.Size(398, 265);
            this.ARCHwhitenoise.TabIndex = 8;
            this.ARCHwhitenoise.Text = "chart1";
            // 
            // ARCHarimadgv
            // 
            this.ARCHarimadgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ARCHarimadgv.Location = new System.Drawing.Point(465, 451);
            this.ARCHarimadgv.Name = "ARCHarimadgv";
            this.ARCHarimadgv.Size = new System.Drawing.Size(347, 39);
            this.ARCHarimadgv.TabIndex = 10;
            // 
            // ARCHarchdgv
            // 
            this.ARCHarchdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ARCHarchdgv.Location = new System.Drawing.Point(465, 558);
            this.ARCHarchdgv.Name = "ARCHarchdgv";
            this.ARCHarchdgv.Size = new System.Drawing.Size(347, 39);
            this.ARCHarchdgv.TabIndex = 11;
            // 
            // ARCHsavebt
            // 
            this.ARCHsavebt.Location = new System.Drawing.Point(33, 364);
            this.ARCHsavebt.Name = "ARCHsavebt";
            this.ARCHsavebt.Size = new System.Drawing.Size(148, 45);
            this.ARCHsavebt.TabIndex = 12;
            this.ARCHsavebt.Text = "Save ARCH models";
            this.ARCHsavebt.UseVisualStyleBackColor = true;
            this.ARCHsavebt.Click += new System.EventHandler(this.ARCHsavebt_Click);
            // 
            // ARCHarimalbl
            // 
            this.ARCHarimalbl.AutoSize = true;
            this.ARCHarimalbl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.ARCHarimalbl.Location = new System.Drawing.Point(599, 55);
            this.ARCHarimalbl.Name = "ARCHarimalbl";
            this.ARCHarimalbl.Size = new System.Drawing.Size(140, 13);
            this.ARCHarimalbl.TabIndex = 13;
            this.ARCHarimalbl.Text = "Fitted ARIMA Models";
            //this.ARCHarimalbl.Click += new System.EventHandler(this.ARCHarimalbl_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(462, 417);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "ARIMA Parameters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(462, 526);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "ARCH/GARCH Parameters";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(30, 477);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Fitted ARCH/GARCH Models";
            // 
            // ARCH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 609);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ARCHarimalbl);
            this.Controls.Add(this.ARCHsavebt);
            this.Controls.Add(this.ARCHarchdgv);
            this.Controls.Add(this.ARCHarimadgv);
            this.Controls.Add(this.ARCHwhitenoise);
            this.Controls.Add(this.ARCHmodelsdgv);
            this.Controls.Add(this.ARCHmodels);
            this.Controls.Add(this.ARCHdgv);
            this.Controls.Add(this.ARCHlb);
            this.Controls.Add(this.ARCHLoadbt);
            this.Name = "ARCH";
            this.Text = "ARCH";
            this.Load += new System.EventHandler(this.ARCH_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ARCHdgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHmodelsdgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHwhitenoise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHarimadgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARCHarchdgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ARCHLoadbt;
        private System.Windows.Forms.ListBox ARCHlb;
        private System.Windows.Forms.DataGridView ARCHdgv;
        private System.Windows.Forms.Button ARCHmodels;
        private System.Windows.Forms.DataGridView ARCHmodelsdgv;
        private System.Windows.Forms.DataVisualization.Charting.Chart ARCHwhitenoise;
        private System.Windows.Forms.DataGridView ARCHarimadgv;
        private System.Windows.Forms.DataGridView ARCHarchdgv;
        private System.Windows.Forms.Button ARCHsavebt;
        private System.Windows.Forms.Label ARCHarimalbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}