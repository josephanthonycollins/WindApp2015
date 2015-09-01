using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

//Joseph Collins
//Student ID: 98718584
//Course: M.Sc. Mathematical Modelling and Scientific Computing
//University College Cork
//Submission Date: 2015

namespace WindApp2015
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exploratoryAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //used for the exploratory data analysis, see Chapter 3 in accompanying manual
            explrForm exploratory = new explrForm();
            exploratory.MdiParent = this;
            exploratory.WindowState = FormWindowState.Maximized;
            exploratory.Show();
        }

        private void aRIMAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //used for ARIMA modelling, see Chapter 4 in accompanying manual
            ARIMA arima = new ARIMA();
            arima.MdiParent = this;
            arima.WindowState = FormWindowState.Maximized;
            arima.Show();
        }

        private void aRCHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //used for ARCH/GARCH modelling, see Chapter 5 in accompanying manual
            ARCH arch = new ARCH();
            arch.MdiParent = this;
            arch.WindowState = FormWindowState.Maximized;
            arch.Show();
        }

        private void forecastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //used for forecasting/simulating time series, see Chapter 6 in accompanying manual
            Forecast forecast = new Forecast();
            forecast.MdiParent = this;
            forecast.WindowState = FormWindowState.Maximized;
            forecast.Show();
        }


    }


    //end of namespace
}
