using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Windows.Forms.DataVisualization.Charting;
using NsExcel = Microsoft.Office.Interop.Excel;

//MDI Child form, used for ARIMA modelling, see Chapter 4 in accompanying manual

namespace WindApp2015
{
    public partial class ARIMA : Form
    {
        //filename
        private String fn;

        public String Fn
        {
            get { return fn; }
            set { fn = value; }
        }

        private Statistics p;

        internal Statistics P
        {
            get { return p; }
            set { p = value; }
        }

        private List<String> flocations = new List<string>();

        public List<String> Flocations
        {
            get { return flocations; }
            set { flocations = value; }
        }

        private List<String> tab = new List<string>();

        public List<String> Tab
        {
            get { return tab; }
            set { tab = value; }
        }

        private List<String> fnames = new List<string>();

        public List<String> Fnames
        {
            get { return fnames; }
            set { fnames = value; }
        }

        private List<double> tseries = new List<double>();

        public List<double> Tseries
        {
            get { return tseries; }
            set { tseries = value; }
        }

        //this variable will contain the number of rows in the underlying tab that we are interested in
        private int r = 0;

        public int R
        {
            get { return r; }
            set { r = value; }
        }

        //this variable will contain the number of rows in the underlying tab that we are interested in
        private int c = 0;

        public int C
        {
            get { return c; }
            set { c = value; }
        }

        //list which contains the model objects
        private BindingList<model> modellist = new BindingList<model>();

        internal BindingList<model> Modellist
        {
            get { return modellist; }
            set { modellist = value; }
        }

        //will be used to connect model objects to the datagridview
        private BindingSource bs1 = new BindingSource();

        //will be used to connect the model from thedatagrid view to chart of the acf plot for the assocatied residuals
        private BindingSource bs2 = new BindingSource();

        //parameter indicating whether the original data was transformed via Iteration or Weibull, see Section 3.1 in accompanying manual
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        //parameter indicating the transformation value, will depend on whether data was transformed via iterative or weibull approaches
        private double transform;

        public double Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        //if the original data was transformed, this variable keeps track of the transformation scale parameter
        private double transformscale;

        public double Transformscale
        {
            get { return transformscale; }
            set { transformscale = value; }
        }

        //if the user wants to centre the data (i.e. subtract the mean of the time series from each datapoint), this variable will be used to assist with the process
        private double mean;

        public double Mean
        {
            get { return mean; }
            set { mean = value; }
        }

        public ARIMA()
        {
            InitializeComponent();
        }

        //Method executed when the Load Data button is clicked
        //Calls the populatelistbox() method
        private void ARIMALoadbt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            //full file location and name
            String strfl;
            //file name and extensions
            String strfilename;
            Stream myStream = null;
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = "D:\\JC Masters\\Thesis\\Data";
                openFileDialog1.Filter = "Excel files (*.xls or .xlsx)|.xls;*.xlsx";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                bool b = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //try and use the openFileDialog1 info
                    try
                    {
                        if (b/*(myStream = openFileDialog1.OpenFile()) != null*/)
                        {
                            using (myStream)
                            {
                                strfl = openFileDialog1.FileName;
                                strfilename = openFileDialog1.SafeFileName;
                                this.Fn = strfl;
                                this.populatelistbox(strfl, strfilename);
                            }
                        }
                    }
                    //if there is an error with using the info from the openFileDialog1 button, report it
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }

                }
            }

            this.Cursor = old;
        }


        //method to populate the listbox
        public void populatelistbox(String s, String q)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            String loc = s;
            String f = q;
            OleDbConnection con = null;
            DataTable dt = null;
            string CconnectionString;
            CconnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + loc + ";Extended Properties=Excel 12.0;";
            con = new OleDbConnection(CconnectionString);

            try
            {
                con.Open();
                dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                String[] excelSheetNames = new String[dt.Rows.Count];
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {

                    excelSheetNames[i] = row["TABLE_NAME"].ToString();
                    Fnames.Add(f);
                    Flocations.Add(loc);
                    Tab.Add(row["TABLE_NAME"].ToString());
                    ARIMAlb.Items.Add(f + " , " + excelSheetNames[i]);
                    i++;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Could not read tab names. Error: " + ex.Message);
            }
            finally
            {
                con.Close();
                this.Cursor = old;
            }

        }

        //method is called if the user clicks any of the values in the listbox
        //the method a)loads the relevant time series info b)calls the HookUpData() method c)Determines if any transformation was originally applied to the data
        private void ARIMAlb_SelectedValueChanged_1(object sender, EventArgs e)
        {

            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            //next we want to remove any data that may already be in the Tseries object
            Tseries.Clear();

            int position = ARIMAlb.SelectedIndex;
            //protects in the event that the user selectes a blank line
            if (position == -1)
            {
                this.Cursor = old;
                return;
            }
            //otherwise proceed as normal
            string curItem = Tab[position];
            string df = Flocations[position];
            string f = ";Extended Properties=\"Excel 12.0;HDR=NO\"";
            string c = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + df + f;

            //we use these variables to try and determine whether or not the data was transformed
            string interim1 = Fnames[position].Replace(".xlsx", "");
            string interim2 = Tab[position].Replace("$", "");
            string interim3 = Flocations[position].Replace(fnames[position], interim1 + interim2 + ".csv");

            int counter = 0;
            string line1;
            string line2;
            string line3;
            double t = 0;
            double tscale = 0;
            try
            {
                StreamReader sr = new StreamReader(interim3);
                while (counter < 1)
                {
                    line1 = sr.ReadLine();
                    line2 = sr.ReadLine();
                    line3 = sr.ReadLine();
                    bool tf = Double.TryParse(line2, out t);
                    bool tfscale = Double.TryParse(line3, out tscale);
                    if (line1 == "Iterative")
                    {
                        this.Type = "Iterative";
                        this.Transform = t;
                        this.Transformscale = 0;
                    }
                    if (line1 == "Weibull")
                    {
                        this.Type = "Weibull";
                        this.Transform = t;
                        this.Transformscale = tscale;
                    }

                    counter++;
                }
            }
            catch
            {

            }
            DataSet data = new DataSet();
            OleDbConnection con = new OleDbConnection(c);
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT * FROM [{0}]", curItem);
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
            try
            {
                con.Open();
                adapter.Fill(dataTable);
                data.Tables.Add(dataTable);
                int m = 0, n = 0;
                m = data.Tables[0].Rows.Count;
                n = dataTable.Columns.Count;
                this.R = m;
                this.C = n;
                int q = 0, r = 0;
                try
                {
                    for (q = 0; q < m; q++)
                    {
                        for (r = 0; r < n; r++)
                        {

                            double d = (double)data.Tables[0].Rows[q].ItemArray[r];
                            Tseries.Add(d);
                        }
                    }
                    //call the HookUpData() method which will produce the acf plot of the data
                    HookUpData();
                }
                catch (Exception x)
                {
                    MessageBox.Show("No data in the underlying tab. Error:" + x.Message);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("The listbox selection did not work. Error: " + ex.Message);
            }

            finally
            {
                con.Close();
                this.Cursor = old;
            }

        }

        //method which plots the acf and pacf for the data
        public void HookUpData()
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            this.show();
            this.hidesubset();

            //if you pick an alternative tab, then you want the ACF and PACF for the previously differenced data set to be hidden
            ARIMAac2.Hide();

            this.P = new Statistics(Tseries);

            //calculate the autcorrelation and partial autocorrelation of the data
            List<double> sac = P.sampleautocorrelation(P.D, 50);
            sac.RemoveAt(0);
            List<double> spac = P.samplepartialautocorrelation(P.D, 50);
            spac.RemoveAt(0);

            //we will use the following code to create the horizontal lines in the ACF and PACF plots
            int n = this.P.D.Count();
            double l1 = 1.96 / Math.Sqrt(n);
            double l2 = -1.96 / Math.Sqrt(n);
            List<double> line1 = new List<double>();
            List<double> line2 = new List<double>();
            for (int i = 0; i < sac.Count(); i++)
            {
                line1.Add(l1);
                line2.Add(l2);
            }
            //plot of the sample acf and pacf
            ARIMAac1.Titles.Clear();
            ARIMAac1.Titles.Add(new Title("Sample ACF and PACF", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ARIMAac1.ChartAreas[0].AxisY.LabelStyle.Font = ARIMAac1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            ARIMAac1.ChartAreas[0].RecalculateAxesScale();
            ARIMAac1.Series.Clear();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ACF",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            this.ARIMAac1.Series.Add(series1);
            int z = 0;
            foreach (double dd in sac)
            {
                series1.Points.AddXY(z + 1, sac[z]);
                z++;
            }
            var series2 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "PACF",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            this.ARIMAac1.Series.Add(series2);
            z = 0;
            foreach (double dd in spac)
            {
                series2.Points.AddXY(z + 1, spac[z]);
                z++;
            }


            var series21 = new System.Windows.Forms.DataVisualization.Charting.Series
           {
               Name = "Series21",
               Color = System.Drawing.Color.Black,
               IsVisibleInLegend = false,
               IsXValueIndexed = true,
               ChartType = SeriesChartType.Line
           };
            this.ARIMAac1.Series.Add(series21);
            z = 0;
            foreach (double dd in line1)
            {
                series21.Points.AddXY(z + 1, line1[z]);
                z++;
            }

            var series22 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series22",
                Color = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ARIMAac1.Series.Add(series22);
            z = 0;
            foreach (double dd in line2)
            {
                series22.Points.AddXY(z + 1, line2[z]);
                z++;
            }
            ARIMAac1.Invalidate();
            ARIMAac1.ChartAreas[0].AxisY.Maximum = Math.Max(P.maximum(sac), P.maximum(spac));

            this.Cursor = old;
        }

        //method to hide buttons etc
        public void hide()
        {
            ARIMAac1.Hide();
            ARIMAdiff.Hide();
            ARIMAac2.Hide();
            ARIMAmodelbt.Hide();
            ARIMAdgv.Hide();
            ARIMAresidacf.Hide();
            ARIMAhist.Hide();
            ARIMAparameterdgv.Hide();
            ARIMAresidqqplot.Hide();
            ARIMAplbl.Hide();
            ARIMArlbl.Hide();
            label1.Hide();
            ARIMAroutput.Hide();
            ARIMAubt.Hide();
            ARIMAsquaredresiduals.Hide();
            ARIMAsave.Hide();
        }

        //method to show charts etc
        public void show()
        {
            ARIMAac1.Show();
            ARIMAdiff.Show();
            ARIMAmodelbt.Show();
        }

        //method to hide a subset of the carts etc
        public void hidesubset()
        {
            ARIMAac2.Hide();
            ARIMAdgv.Hide();
            ARIMAresidacf.Hide();
            ARIMAhist.Hide();
            ARIMAparameterdgv.Hide();
            ARIMAresidqqplot.Hide();
            ARIMAplbl.Hide();
            ARIMArlbl.Hide();
            label1.Hide();
            ARIMAroutput.Hide();
            ARIMAubt.Hide();
            ARIMAsquaredresiduals.Hide();
            ARIMAsave.Hide();
        }

        private void ARIMA_Load(object sender, EventArgs e)
        {
            this.hide();
        }

        //method which helps with differencing of the data
        //the method also plots the acf and pacf of the differenced data
        private void ARIMAdiff_Click(object sender, EventArgs e)
        {
            if (this.Mean > 0)
                this.Mean = 0;

            //form which will help to determine the order of differencing
            standardiseForm f1 = new standardiseForm();

            ARIMAresidacf.Hide();
            ARIMAdgv.Hide();
            ARIMAhist.Hide();
            ARIMAparameterdgv.Hide();
            ARIMAparameterdgv.Hide();
            ARIMAplbl.Hide();
            ARIMArlbl.Hide();
            ARIMAresidqqplot.Hide();
            label1.Hide();
            ARIMAroutput.Hide();
            ARIMAsquaredresiduals.Hide();
            ARIMAubt.Hide();
            ARIMAsave.Hide();
            //we don't need all the info from the "standardiseForm" hence we can hide some of objects in the form
            f1.standardisetxtbcols.Text = "1";
            f1.standardisetxtbcols.Visible = false;
            f1.standardiselblcols.Visible = false;
            f1.standardiselblh.Text = "Provide an Integer to specify the differencing:";
            f1.standardiselblrows.Text = "Integer";
            f1.N1 = 100;
            f1.M1 = 100;
            //display the dialog
            if (f1.ShowDialog() == DialogResult.OK)
            {
                int m2;
                bool m2Test = int.TryParse(f1.standardisetxtbcols.Text, out m2);
                int n2;
                bool n2Test = int.TryParse(f1.standardisetxtbrows.Text, out n2);
                if ((n2 > 0))
                {
                    //display the chart
                    ARIMAac2.Show();
                    this.P.DifferencedData.Clear();
                    this.P.Diff = n2;
                    //now difference the data
                    P.difference();
                    //calculate the autcorrelation and partial autocorrelation of the data
                    List<double> sc = P.sampleautocorrelation(P.DifferencedData, 50);
                    sc.RemoveAt(0);
                    List<double> spc = P.samplepartialautocorrelation(P.DifferencedData, 50);
                    spc.RemoveAt(0);
                    //we will use the following code to create the horizontal lines in the ACF and PACF plots
                    int n = this.P.DifferencedData.Count();
                    double l1 = 1.96 / Math.Sqrt(n);
                    double l2 = -1.96 / Math.Sqrt(n);
                    List<double> line1 = new List<double>();
                    List<double> line2 = new List<double>();
                    for (int i = 0; i < sc.Count(); i++)
                    {
                        line1.Add(l1);
                        line2.Add(l2);
                    }
                    //plot of the autcoorrelation
                    ARIMAac2.Titles.Clear();
                    ARIMAac2.Titles.Add(new Title("Differenced Data: ACF and PACF", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
                    ARIMAac2.ChartAreas[0].AxisY.LabelStyle.Font = ARIMAac2.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
                    ARIMAac2.ChartAreas[0].RecalculateAxesScale();
                    ARIMAac2.Series.Clear();
                    var series3 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "ACF",
                        Color = System.Drawing.Color.Blue,
                        IsVisibleInLegend = true,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.Column
                    };
                    this.ARIMAac2.Series.Add(series3);
                    int z = 0;
                    foreach (double dd in sc)
                    {
                        series3.Points.AddXY(z + 1, sc[z]);
                        z++;
                    }
                    var series4 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "PACF",
                        Color = System.Drawing.Color.Red,
                        IsVisibleInLegend = true,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.Column
                    };
                    this.ARIMAac2.Series.Add(series4);
                    z = 0;
                    foreach (double dd in spc)
                    {
                        series4.Points.AddXY(z + 1, spc[z]);
                        z++;
                    }

                    var series23 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "Series23",
                        Color = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.Line
                    };
                    this.ARIMAac2.Series.Add(series23);
                    z = 0;
                    foreach (double dd in line1)
                    {
                        series23.Points.AddXY(z + 1, line1[z]);
                        z++;
                    }

                    var series24 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "Series24",
                        Color = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.Line
                    };
                    this.ARIMAac2.Series.Add(series24);
                    z = 0;
                    foreach (double dd in line2)
                    {
                        series24.Points.AddXY(z + 1, line2[z]);
                        z++;
                    }

                    ARIMAac2.Invalidate();
                    ARIMAac2.ChartAreas[0].AxisY.Maximum = Math.Max(P.maximum(sc), P.maximum(spc));
                }
                else
                {
                    //ensure that the Diff is set to 0 as the user has decided not to use differencing
                    this.P.Diff = 0;
                }
            }

            //end of method
        }

        //when the user clicks on the Run ARIMA Models button, this method will be called
        //it automatically fits a number of different ARIMA models and summarises the info in a datagridview
        private void ARIMAmodelbt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            if (this.Mean > 0)
                this.Mean = 0;

            //first we need to empty the Modellist as it could contain info from previous iterations
            if (Modellist.Count() > 0)
                Modellist.Clear();

            //allows the user to centre the data if they so choose, if the data has been differenced the user is not presented with this choice
            if (P.Diff == 0)
            {
                double aver = Math.Round(P.avg(P.D), 2);
                string str = "Time series mean is " + aver.ToString() + ". ARMA models in WindApp2015 assume a zero mean (i.e.zero intercept). Do you want to subtract the mean from each data point prior to fitting ARMA models?";
                DialogResult dr = MessageBox.Show(str, "Centre the data", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes:
                        this.Mean = P.avg(P.D);
                        List<double> tempdata = new List<double>();
                        foreach (double dd in P.D)
                            tempdata.Add(dd - P.avg(P.D));
                        P.D.Clear();
                        foreach (double dd in tempdata)
                            P.D.Add(dd);
                        break;
                    case DialogResult.No:
                        this.Mean = 0;
                        break;
                }
            }

            //Fitting the Yule Walker Models
            if (P.Diff == 0)
            {
                int samplesize = P.D.Count();
                for (int i = 1; i < 10; i++)
                {
                    bool indic = false;
                    if (this.Mean != 0)
                        indic = true;
                    List<double> parameterest = P.yulewalker(P.D, i);
                    List<double> resid = P.yulewalkerresiduals(P.D, parameterest);
                    List<double> acfresid = P.sampleautocorrelation(resid, 100);
                    double ssresid = P.sumofsquares(resid);
                    double v = P.yulewalkerVariance(P.D, parameterest);
                    double AIC = Math.Log(v) * samplesize + 2 * (i + 1);
                    double BIC = Math.Log(v) * samplesize + (i + 1) * Math.Log(samplesize);
                    if (this.Type == null)
                    {
                        this.Type = "";
                        this.Transform = 0;
                        this.Transformscale = 0;

                    }
                    if ((Math.Abs(AIC) > 1000000) || (double.IsNaN(AIC)) || (double.IsNaN(ssresid)) || (double.IsInfinity(ssresid)))
                        continue;
                    else
                        Modellist.Add(new model("YW", i, 0, 0, AIC, BIC, parameterest, resid, v, acfresid, ssresid, this.Type, this.Transform, this.Transformscale, 0, 0, new List<double> { 0 }, 0, indic));

                }

            }

            if (P.Diff > 0)
            {
                int samplesize = P.DifferencedData.Count;
                for (int i = 1; i < 10; i++)
                {
                    bool indic = false;
                    if (this.Mean != 0)
                        indic = true;
                    List<double> parameterest = P.yulewalker(P.DifferencedData, i);
                    List<double> resid = P.yulewalkerresiduals(P.DifferencedData, parameterest);
                    List<double> acfresid = P.sampleautocorrelation(resid, 100);
                    double ssresid = P.sumofsquares(resid);
                    double v = P.yulewalkerVariance(P.DifferencedData, parameterest);
                    double AIC = Math.Log(v) * samplesize + 2 * (i + 1);
                    double BIC = Math.Log(v) * samplesize + (i + 1) * Math.Log(samplesize);
                    if (this.Type == null)
                    {
                        this.Type = "";
                        this.Transform = 0;
                        this.Transformscale = 0;

                    }
                    if ((Math.Abs(AIC) > 1000000) || (double.IsNaN(AIC)) || (double.IsNaN(ssresid)) || (double.IsInfinity(ssresid)))
                        continue;
                    else
                        Modellist.Add(new model("YW", i, 0, P.Diff, AIC, BIC, parameterest, resid, v, acfresid, ssresid, this.Type, this.Transform, this.Transformscale, 0, 0, new List<double> { 0 }, 0, indic));
                }

            }

            //creating a list to hold the info
            //This is really just a workaround.....could have been done in a smarter way
            List<double> data = new List<double>();
            if (P.Diff == 0)
            {
                for (int i = 0; i < P.D.Count(); i++)
                    data.Add(P.D[i]);
            }
            else
            {
                for (int i = 0; i < P.DifferencedData.Count(); i++)
                    data.Add(P.DifferencedData[i]);
            }
            int size = 0;
            if (P.Diff == 0)
                size = P.D.Count();
            else if (P.Diff > 0)
                size = P.DifferencedData.Count();


            //fiting MA models using the Innovations algorithm
            for (int i = 1; i < 10; i++)
            {
                bool indic = false;
                if (this.Mean != 0)
                    indic = true;
                List<double> paramsandvariance = P.innovations(data, i, 50);
                List<double> parameterest = new List<double>();
                for (int l = 0; l < paramsandvariance.Count() - 1; l++)
                    parameterest.Add(paramsandvariance[l]);
                List<double> resid = P.innovationsresiduals(data, parameterest);
                List<double> acfresid = P.sampleautocorrelation(resid, 100);
                double ssresid = P.sumofsquares(resid);
                double v = paramsandvariance[paramsandvariance.Count() - 1];
                double AIC = Math.Log(v) * size + 2 * (i + 0 + 1);
                double BIC = Math.Log(v) * size + (i + 0 + 1) * Math.Log(size);
                if (this.Type == null)
                {
                    this.Type = "";
                    this.Transform = 0;
                    this.Transformscale = 0;

                }
                if ((Math.Abs(AIC) > 1000000) || (double.IsNaN(AIC)) || (double.IsNaN(ssresid)) || (double.IsInfinity(ssresid)))
                    continue;
                else
                    Modellist.Add(new model("Innovations", 0, i, P.Diff, AIC, BIC, parameterest, resid, v, acfresid, ssresid, this.Type, this.Transform, this.Transformscale, 0, 0, new List<double> { 0 }, 0, indic));

            }

            //fit the ARMA models with Hannan Rissanen
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    bool indic = false;
                    if (this.Mean != 0)
                        indic = true;
                    List<double> parameterest = P.HannanRissanen(i, j, 20);
                    List<double> resid = P.ARMAresiduals(data, parameterest, i, j);
                    List<double> acfresid = P.sampleautocorrelation(resid, 100);
                    double ssresid = P.sumofsquares(resid);
                    double v = P.HannanRissanenVariance(resid, i, j);
                    double AIC = Math.Log(v) * size + 2 * (i + j + 1);
                    double BIC = Math.Log(v) * size + (i + j + 1) * Math.Log(size);
                    if (this.Type == null)
                    {
                        this.Type = "";
                        this.Transform = 0;
                        this.Transformscale = 0;

                    }
                    if ((Math.Abs(AIC) > 1000000) || (double.IsNaN(AIC)) || (double.IsNaN(ssresid)) || (double.IsInfinity(ssresid)))
                        continue;
                    else
                        Modellist.Add(new model("HR", i, j, P.Diff, AIC, BIC, parameterest, resid, v, acfresid, ssresid, this.Type, this.Transform, this.Transformscale, 0, 0, new List<double> { 0 }, 0, indic));
                }
            }

            //fit AR models using Conditional Maximum Likelihood
            for (int i = 1; i < 2; i++)
            {
                bool indic = false;
                if (this.Mean != 0)
                    indic = true;
                List<double> parameterest = P.ARML(data, i);
                List<double> resid = P.ARMLresiduals(data, parameterest);
                List<double> acfresid = P.sampleautocorrelation(resid, 100);
                double ssresid = P.sumofsquares(resid);
                double v = P.ARMLVariance(resid, i);
                double AIC = Math.Log(v) * size + 2 * (i + 0 + 1);
                double BIC = Math.Log(v) * size + (i + 0 + 1) * Math.Log(size);
                if (this.Type == null)
                {
                    this.Type = "";
                    this.Transform = 0;
                    this.Transformscale = 0;

                }
                if ((Math.Abs(AIC) > 1000000) || (double.IsNaN(AIC)) || (double.IsNaN(ssresid)) || (double.IsInfinity(ssresid)))
                    continue;
                else
                    Modellist.Add(new model("ML", i, 0, P.Diff, AIC, BIC, parameterest, resid, v, acfresid, ssresid, this.Type, this.Transform, this.Transformscale, 0, 0, new List<double> { 0 }, 0, indic));

            }

            //fit MA models using Conditional Maximum Likelihood
            //we don't fit as many of these as they take alot longer to run
            for (int i = 1; i < 2; i++)
            {
                bool indic = false;
                if (this.Mean != 0)
                    indic = true;
                List<double> paramsandvariance = P.MAMLalternative(data, i);
                List<double> parameterest = new List<double>();
                for (int l = 0; l < paramsandvariance.Count() - 1; l++)
                    parameterest.Add(paramsandvariance[l]);
                List<double> resid = P.MAMLresiduals(data, parameterest);
                List<double> acfresid = P.sampleautocorrelation(resid, 100);
                double ssresid = P.sumofsquares(resid);
                double v = paramsandvariance[paramsandvariance.Count() - 1];
                double AIC = Math.Log(v) * size + 2 * (i + 0 + 1);
                double BIC = Math.Log(v) * size + (i + 0 + 1) * Math.Log(size);
                if (this.Type == null)
                {
                    this.Type = "";
                    this.Transform = 0;
                    this.Transformscale = 0;

                }
                if ((Math.Abs(AIC) > 1000000) || (double.IsNaN(AIC)) || (double.IsNaN(ssresid)) || (double.IsInfinity(ssresid)))
                    continue;
                else
                    Modellist.Add(new model("ML", 0, i, P.Diff, AIC, BIC, parameterest, resid, v, acfresid, ssresid, this.Type, this.Transform, this.Transformscale, 0, 0, new List<double> { 0 }, 0, indic));

            }


            //fit ARMA models using Conditional Maximum Likelihood
            //given the length of time it takes to run, we'll only let the p and q go up to a maximum of order 3
            for (int i = 1; i < 2; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    bool indic = false;
                    if (this.Mean != 0)
                        indic = true;
                    List<double> paramsandvariance = P.ARMAMLalternative(data, i, j);
                    List<double> parameterest = new List<double>();
                    for (int l = 0; l < paramsandvariance.Count() - 1; l++)
                        parameterest.Add(paramsandvariance[l]);
                    List<double> resid = P.ARMAMLresiduals(data, parameterest, i, j);
                    List<double> acfresid = P.sampleautocorrelation(resid, 100);
                    double ssresid = P.sumofsquares(resid);
                    double v = paramsandvariance[paramsandvariance.Count() - 1];
                    double AIC = Math.Log(v) * size + 2 * (i + j + 1);
                    double BIC = Math.Log(v) * size + (i + j + 1) * Math.Log(size);
                    if (this.Type == null)
                    {
                        this.Type = "";
                        this.Transform = 0;
                        this.Transformscale = 0;

                    }
                    if ((Math.Abs(AIC) > 1000000) || (double.IsNaN(AIC)) || (parameterest[0] == -1000) || (double.IsNaN(ssresid)) || (double.IsInfinity(ssresid)))
                        continue;
                    else
                        Modellist.Add(new model("ML", i, j, P.Diff, AIC, BIC, parameterest, resid, v, acfresid, ssresid, this.Type, this.Transform, this.Transformscale, 0, 0, new List<double> { 0 }, 0, indic));
                }

            }

            //populating the datagridview to display the model output
            int x = ARIMAdgv.ColumnCount;
            bs1.DataSource = Modellist;
            ARIMAdgv.Show();
            ARIMAdgv.DataSource = bs1;
            ARIMAdgv.AllowUserToAddRows = false;
            ARIMAdgv.AutoGenerateColumns = false;
            //??
            if (x == 0)
            {
                DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
                col1.DataPropertyName = "Type";
                col1.HeaderText = "Alg";
                col1.Name = "Blah";
                col1.Width = 50;
                ARIMAdgv.Columns.Add(col1);
                DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
                col2.DataPropertyName = "P";
                col2.HeaderText = "AR";
                col2.Name = "AR Params";
                col2.Width = 50;
                ARIMAdgv.Columns.Add(col2);
                DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
                col3.DataPropertyName = "Q";
                col3.HeaderText = "MA";
                col3.Name = "MA Params";
                col3.Width = 50;
                ARIMAdgv.Columns.Add(col3);
                DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
                col4.DataPropertyName = "D";
                col4.HeaderText = "Diff";
                col4.Name = "Diff";
                col4.Width = 50;
                ARIMAdgv.Columns.Add(col4);
                DataGridViewColumn col5 = new DataGridViewTextBoxColumn();
                col5.DataPropertyName = "Variance";
                col5.HeaderText = "Var";
                col5.Name = "Var";
                col5.Width = 50;
                ARIMAdgv.Columns.Add(col5);
                col5.DefaultCellStyle.Format = "n4";
                DataGridViewColumn col6 = new DataGridViewTextBoxColumn();
                col6.DataPropertyName = "Sumsquaredresiduals";
                col6.HeaderText = "SS resid";
                col6.Name = "SS resid";
                col6.Width = 60;
                ARIMAdgv.Columns.Add(col6);
                col6.DefaultCellStyle.Format = "n2";
                DataGridViewColumn col7 = new DataGridViewTextBoxColumn();
                col7.DataPropertyName = "Aic";
                col7.HeaderText = "AIC";
                col7.Name = "AIC";
                col7.Width = 70;
                col7.DefaultCellStyle.Format = "n0";
                ARIMAdgv.Columns.Add(col7);
                ARIMAdgv.Columns["P"].Visible = false;
                ARIMAdgv.Columns["Q"].Visible = false;
                ARIMAdgv.Columns["Aic"].Visible = false;
                ARIMAdgv.Columns["Bic"].Visible = false;
                ARIMAdgv.Columns["Type"].Visible = false;
                ARIMAdgv.Columns["Variance"].Visible = false;
                ARIMAdgv.Columns["D"].Visible = false;
                ARIMAdgv.Columns["Sumsquaredresiduals"].Visible = false;
                ARIMAdgv.Columns["Transformtype"].Visible = false;
                ARIMAdgv.Columns["Transformshape"].Visible = false;
                ARIMAdgv.Columns["Transformscale"].Visible = false;
                ARIMAdgv.Columns["archP"].Visible = false;
                ARIMAdgv.Columns["archQ"].Visible = false;
                ARIMAdgv.Columns["archAic"].Visible = false;
                ARIMAdgv.Columns["Subtractmean"].Visible = false;
            }

            //now we will determine which of the models has the lowest residual sum of squares
            //and highlight it in the datagridview
            int msize = modellist.Count();
            int[] position = new int[msize];
            double[] residss = new double[msize];
            int minposition = 0;
            double minresidss = 100000000;
            for (int i = 0; i < msize; i++)
            {
                position[i] = i;
                residss[i] = modellist[i].Sumsquaredresiduals;
                if (residss[i] < minresidss)
                {
                    minresidss = residss[i];
                    minposition = i;
                }
            }
            ARIMAdgv.Rows[minposition].DefaultCellStyle.BackColor = Color.LightBlue;
            ARIMAdgv.FirstDisplayedScrollingRowIndex = minposition;
            ARIMAdgv.Rows[minposition].Cells[1].Selected = true;
            ARIMAdgv_CellDoubleClick(ARIMAdgv, new DataGridViewCellEventArgs(minposition, 1));
            ARIMAubt.Show();
            ARIMAsave.Show();

            this.Cursor = old;
        }


        //this method is called if the user double clicks on one of the models in the datagridview
        //it automatically produces checks on the model residuals, parameter estimates etc
        private void ARIMAdgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            ARIMAresidacf.Show();
            //place holder to determine which row in the DGV has been selected
            int selectedrowindex = 0;
            if (this.ARIMAdgv.SelectedCells.Count > 0)
                selectedrowindex = ARIMAdgv.SelectedCells[0].RowIndex;

            //ACF of the residuals for the relevant model
            List<double> residsac = new List<double>();
            foreach (double dd in Modellist[selectedrowindex].Acfresiduals)
            {
                residsac.Add(dd);
            }
            residsac.RemoveAt(0);
            //we will use the following code to create the horizontal lines in the ACF and PACF plots
            int n = Modellist[selectedrowindex].Residuals.Count();
            double l1 = 1.96 / Math.Sqrt(n);
            double l2 = -1.96 / Math.Sqrt(n);
            List<double> line1 = new List<double>();
            List<double> line2 = new List<double>();
            for (int i = 0; i < residsac.Count(); i++)
            {
                line1.Add(l1);
                line2.Add(l2);
            }
            ARIMAresidacf.Titles.Clear();
            ARIMAresidacf.Titles.Add(new Title("Residual ACF", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ARIMAresidacf.ChartAreas[0].AxisY.LabelStyle.Font = ARIMAresidacf.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            ARIMAresidacf.ChartAreas[0].RecalculateAxesScale();
            ARIMAresidacf.Series.Clear();
            var series10 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Resid ACF",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            this.ARIMAresidacf.Series.Add(series10);
            int z = 0;
            foreach (double dd in residsac)
            {
                series10.Points.AddXY(z + 1, residsac[z]);
                z++;
            }
            var series26 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series26",
                Color = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ARIMAresidacf.Series.Add(series26);
            z = 0;
            foreach (double dd in line1)
            {
                series26.Points.AddXY(z + 1, line1[z]);
                z++;
            }
            var series27 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series27",
                Color = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ARIMAresidacf.Series.Add(series27);
            z = 0;
            foreach (double dd in line2)
            {
                series27.Points.AddXY(z + 1, line2[z]);
                z++;
            }
            ARIMAresidacf.Invalidate();
            ARIMAresidacf.ChartAreas[0].AxisY.Maximum = this.P.maximum(residsac);

            //now we want to create the histogram of the residuals
            ARIMAhist.Show();
            var b = P.histogramBins(Modellist[selectedrowindex].Residuals);
            var v = P.histogramValues(Modellist[selectedrowindex].Residuals, b);
            int y = 0;
            P.HistBins.Clear();
            P.HistValues.Clear();
            foreach (var variable in b)
            {
                P.HistBins.Add(variable);
                P.HistValues.Add(v[y]);
                y++;
            }
            ARIMAhist.Titles.Clear();
            ARIMAhist.Titles.Add(new Title("Histogram Residuals", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ARIMAhist.Series.Clear();
            ARIMAhist.ChartAreas[0].RecalculateAxesScale();
            var series11 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Data",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            this.ARIMAhist.Series.Add(series11);
            int q = 0;
            foreach (double dd in P.HistValues)
            {
                series11.Points.AddXY(Math.Round(P.HistBins[q], 2), P.HistValues[q]);
                q++;
            }
            ARIMAhist.Legends[0].DockedToChartArea = "ChartArea1";
            ARIMAhist.ChartAreas[0].AxisY.Maximum = P.maximum(P.HistValues) * 1.05;
            ARIMAhist.Invalidate();

            //now we also want to publish the parameter values in the ARIMAparameterdgv datagridview
            ARIMAplbl.Show();
            ARIMAparameterdgv.Show();
            ARIMAparameterdgv.AllowUserToAddRows = false;
            ARIMAparameterdgv.AutoGenerateColumns = false;
            ARIMAparameterdgv.Rows.Clear();
            ARIMAparameterdgv.Columns.Clear();
            int nparams = Modellist[selectedrowindex].Parameters.Count();
            string[] vals = new string[nparams];
            for (int i = 0; i < nparams; i++)
            {
                vals[i] = Math.Round(Modellist[selectedrowindex].Parameters[i], 3).ToString();
            }
            int nar = Modellist[selectedrowindex].P;
            int nma = Modellist[selectedrowindex].Q;
            ARIMAparameterdgv.ColumnCount = nparams;
            ARIMAparameterdgv.RowCount = 1;
            for (int i = 0; i < nar; i++)
            {
                ARIMAparameterdgv.Columns[i].Name = "AR" + (i + 1).ToString();
            }
            for (int i = 0; i < nma; i++)
            {
                ARIMAparameterdgv.Columns[i + nar].Name = "MA" + (i + 1).ToString();
            }
            ARIMAparameterdgv.Rows[0].SetValues(vals);
            this.Cursor = old;

            //creating a qqplot of the residuals
            List<double> normal = P.normal_sample(Modellist[selectedrowindex].Residuals.Count(), 0, 1);
            normal.Sort();
            List<double> sorted = new List<double>();
            foreach (double dd in Modellist[selectedrowindex].Residuals)
            {
                sorted.Add(dd);
            }
            sorted.Sort();
            ARIMAresidqqplot.Show();
            ARIMAresidqqplot.Titles.Clear();
            ARIMAresidqqplot.Titles.Add(new Title("Q-QPlot Model Residuals", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ARIMAresidqqplot.Series.Clear();
            ARIMAresidqqplot.ChartAreas[0].RecalculateAxesScale();
            var series12 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "QQ",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Point
            };
            this.ARIMAresidqqplot.Series.Add(series12);
            int u = 0;
            foreach (double dd in normal)
            {
                series12.Points.AddXY(Math.Round(dd, 2), sorted[u]);
                u++;
            }
            ARIMAresidqqplot.ChartAreas[0].AxisY.Maximum = Math.Max(P.maximum(sorted) * 1.05, 0);
            ARIMAresidqqplot.ChartAreas[0].AxisX.LabelStyle.Enabled = false;

            //calculate summary statistics for the residuals
            ARIMArlbl.Show();
            label1.Show();
            ARIMAroutput.Show();
            double rmeans = Math.Round(P.avg(Modellist[selectedrowindex].Residuals), 2);
            double rstd = Math.Round(P.stdev(Modellist[selectedrowindex].Residuals), 2);
            double rskew = Math.Round(P.skew(Modellist[selectedrowindex].Residuals), 2);
            double rkurt = Math.Round(P.kurt(Modellist[selectedrowindex].Residuals), 2);
            string summary = "[" + rmeans.ToString() + ", " + rstd.ToString() + ", " + rskew.ToString() + ", " + rkurt.ToString() + "]";
            ARIMAroutput.Text = summary;

            //plot of the ACF for the squared residuals
            ARIMAsquaredresiduals.Show();
            List<double> resids = Modellist[selectedrowindex].Residuals;
            List<double> resids2 = new List<double>();
            for (int k = 0; k < resids.Count(); k++)
                resids2.Add(resids[k] * resids[k]);
            List<double> acfsquaredresids = P.sampleautocorrelation(resids2, 100);
            acfsquaredresids.RemoveAt(0);
            ARIMAsquaredresiduals.Titles.Clear();
            ARIMAsquaredresiduals.ChartAreas[0].RecalculateAxesScale();
            ARIMAsquaredresiduals.Titles.Add(new Title("Squared Residuals ACF", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ARIMAsquaredresiduals.ChartAreas[0].AxisY.LabelStyle.Font = ARIMAsquaredresiduals.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            ARIMAsquaredresiduals.Series.Clear();
            var series40 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Squared Residuals ACF",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            this.ARIMAsquaredresiduals.Series.Add(series40);
            z = 0;
            foreach (double dd in acfsquaredresids)
            {
                series40.Points.AddXY(z + 1, acfsquaredresids[z]);
                z++;
            }
            ARIMAsquaredresiduals.Invalidate();
            ARIMAsquaredresiduals.ChartAreas[0].AxisY.Maximum = P.maximum(acfsquaredresids);
            int no = resids.Count();
            double l4 = 1.96 / Math.Sqrt(no);
            double l5 = -1.96 / Math.Sqrt(no);
            List<double> line4 = new List<double>();
            List<double> line5 = new List<double>();
            for (int i = 0; i < acfsquaredresids.Count(); i++)
            {
                line4.Add(l4);
                line5.Add(l5);
            }
            var series41 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series41",
                Color = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ARIMAsquaredresiduals.Series.Add(series41);
            z = 0;
            foreach (double dd in line4)
            {
                series41.Points.AddXY(z + 1, line4[z]);
                z++;
            }
            var series42 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series42",
                Color = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ARIMAsquaredresiduals.Series.Add(series42);
            z = 0;
            foreach (double dd in line5)
            {
                series42.Points.AddXY(z + 1, line5[z]);
                z++;
            }

        }

        //If the user clicks on the Fit Specific Model button, this method is called
        //it allows the user to fit an ARMA model of their choosing
        private void ARIMAubt_Click(object sender, EventArgs e)
        {
            int originalsize = Modellist.Count();

            //form to allow the user to provide input
            copystandardiseForm f1 = new copystandardiseForm();

            f1.copystandardiselblh.Text = "Specify the ARMA model you want to fit";
            f1.copystandardiselblrows.Text = "AR order (integer):";
            f1.copystandardiselblcols.Text = "MA order (integer):";
            f1.N1 = 10;
            f1.M1 = 10;

            //display the dialog
            if (f1.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                //need to figure out the number of rows and number of columns in the underlying spreadsheet
                int m2;
                bool m2Test = int.TryParse(f1.copystandardisetxtbcols.Text, out m2);
                int n2;
                bool n2Test = int.TryParse(f1.copystandardisetxtbrows.Text, out n2);
                if ((n2 > 0) || (m2 > 0))
                {
                    //creating a data list to hold the info
                    //workaround.....could have been done better
                    List<double> data = new List<double>();
                    if (P.Diff == 0)
                    {
                        for (int i = 0; i < P.D.Count(); i++)
                            data.Add(P.D[i]);
                    }
                    else
                    {
                        for (int i = 0; i < P.DifferencedData.Count(); i++)
                            data.Add(P.DifferencedData[i]);
                    }

                    int size = 0;
                    if (P.Diff == 0)
                        size = P.D.Count();
                    else if (P.Diff > 0)
                        size = P.DifferencedData.Count();

                    //now fit the model and add it to the list
                    bool indic = false;
                    if (this.Mean != 0)
                        indic = true;
                    List<double> paramsandvariance = P.ARMAMLalternative(data, n2, m2);
                    List<double> parameterest = new List<double>();
                    for (int l = 0; l < paramsandvariance.Count() - 1; l++)
                        parameterest.Add(paramsandvariance[l]);
                    List<double> resid = P.ARMAMLresiduals(data, parameterest, n2, m2);
                    List<double> acfresid = P.sampleautocorrelation(resid, 100);
                    double ssresid = P.sumofsquares(resid);
                    double v = paramsandvariance[paramsandvariance.Count() - 1];
                    double AIC = Math.Log(v) * size + 2 * (n2 + m2 + 1);
                    double BIC = Math.Log(v) * size + (n2 + m2 + 1) * Math.Log(size);
                    if (this.Type == null)
                    {
                        this.Type = "";
                        this.Transform = 0;
                        this.Transformscale = 0;

                    }
                    if ((Math.Abs(AIC) > 1000000) || (double.IsNaN(AIC)) || (parameterest[0] == -1000))
                    {

                    }
                    else
                        Modellist.Add(new model("ML", n2, m2, P.Diff, AIC, BIC, parameterest, resid, v, acfresid, ssresid, this.Type, this.Transform, this.Transformscale, 0, 0, new List<double> { 0 }, 0, indic));
                }
                else
                {
                }
            }

            int finalsize = Modellist.Count();
            if (originalsize == finalsize)
            {

            }
            else
            {

                ARIMAdgv.FirstDisplayedScrollingRowIndex = finalsize - 1;
                ARIMAdgv.Rows[finalsize - 1].Cells[1].Selected = true;
                ARIMAdgv_CellDoubleClick(ARIMAdgv, new DataGridViewCellEventArgs(finalsize - 1, 1));
            }
            this.Cursor = Cursors.Default;


        }

        //if the user clicks on the Save ARIMA Models button this method is called
        //it saves the model objects to a CSV file
        private void ARIMAsave_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            int position = ARIMAlb.SelectedIndex;
            string interim1 = Fnames[position].Replace(".xlsx", "");
            string interim2 = Tab[position].Replace("$", "");
            string interim3 = Flocations[position].Replace(fnames[position], interim1 + interim2 + "Models.csv");
            if (modellist.Count() > 0)
            {
                StreamWriter sw = new StreamWriter(interim3, false);
                sw.WriteLine("Transform\tShape\tScale\tModel\tP\tQ\tD\tAIC\tSS\tVariance\tParameters\tarchQ\tarchP\tarchAIC\tarchParameters\tAdjustMean");
                for (int i = 0; i < modellist.Count(); i++)
                {
                    string temp = "";
                    int noparams = modellist[i].Parameters.Count();
                    for (int j = 0; j < noparams - 1; j++)
                        temp = temp + modellist[i].Parameters[j].ToString() + " ";
                    //don't need a whitespace for the last value
                    temp = temp + modellist[i].Parameters[noparams - 1].ToString();
                    string archtemp = "";
                    //at this stage no arch/garchmodel will have been fit to the data, hence we automatically set the arch/garch parameters to 0
                    int archnoparams = modellist[i].archParameters.Count();
                    double q = 0;
                    archtemp = q.ToString();
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}", Modellist[i].Transformtype, Modellist[i].Transformshape, Modellist[i].Transformscale, Modellist[i].Type, Modellist[i].P, Modellist[i].Q, Modellist[i].D, Modellist[i].Aic, Modellist[i].Sumsquaredresiduals, Modellist[i].Variance, temp, Modellist[i].archQ, Modellist[i].archP, Modellist[i].archAic, archtemp, Modellist[i].Subtractmean);
                }
                sw.Close();
                this.Cursor = old;
            }
        }

    }

}

