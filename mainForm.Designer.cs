namespace WindApp2015
{
    partial class mainForm
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
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exploratoryAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aRIMAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aRCHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forecastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.exploratoryAnalysisToolStripMenuItem,
            this.aRIMAToolStripMenuItem,
            this.aRCHToolStripMenuItem,
            this.forecastToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1056, 24);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeyDisplayString = "_C";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exploratoryAnalysisToolStripMenuItem
            // 
            this.exploratoryAnalysisToolStripMenuItem.Name = "exploratoryAnalysisToolStripMenuItem";
            this.exploratoryAnalysisToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.exploratoryAnalysisToolStripMenuItem.Text = "Exploratory Analysis";
            this.exploratoryAnalysisToolStripMenuItem.Click += new System.EventHandler(this.exploratoryAnalysisToolStripMenuItem_Click);
            // 
            // aRIMAToolStripMenuItem
            // 
            this.aRIMAToolStripMenuItem.Name = "aRIMAToolStripMenuItem";
            this.aRIMAToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.aRIMAToolStripMenuItem.Text = "ARIMA";
            this.aRIMAToolStripMenuItem.Click += new System.EventHandler(this.aRIMAToolStripMenuItem_Click);
            // 
            // aRCHToolStripMenuItem
            // 
            this.aRCHToolStripMenuItem.Name = "aRCHToolStripMenuItem";
            this.aRCHToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.aRCHToolStripMenuItem.Text = "ARCH";
            this.aRCHToolStripMenuItem.Click += new System.EventHandler(this.aRCHToolStripMenuItem_Click);
            // 
            // forecastToolStripMenuItem
            // 
            this.forecastToolStripMenuItem.Name = "forecastToolStripMenuItem";
            this.forecastToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.forecastToolStripMenuItem.Text = "Forecast";
            this.forecastToolStripMenuItem.Click += new System.EventHandler(this.forecastToolStripMenuItem_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1056, 455);
            this.Controls.Add(this.mainMenuStrip);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "mainForm";
            this.Text = "Wind App 2015";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exploratoryAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aRIMAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aRCHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forecastToolStripMenuItem;
    }
}

