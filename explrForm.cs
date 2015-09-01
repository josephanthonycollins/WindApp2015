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

namespace WindApp2015
{
    //MDI Child form, used for exploratory data analysis and for transforming the data

    public partial class explrForm : Form
    {
        //filename
        private String fn;

        public String Fn
        {
            get { return fn; }
            set { fn = value; }
        }

        //Statistics object, will be used to call various methods of the Statistics class
        private Statistics p;

        internal Statistics P
        {
            get { return p; }
            set { p = value; }
        }

        //location of all the files which will be shown in the listbox
        private List<String> flocations = new List<string>();

        public List<String> Flocations
        {
            get { return flocations; }
            set { flocations = value; }
        }

        //worksheet names of the spreadsheet that is being loaded
        private List<String> tab = new List<string>();

        public List<String> Tab
        {
            get { return tab; }
            set { tab = value; }
        }

        //spreadsheet names
        private List<String> fnames = new List<string>();

        public List<String> Fnames
        {
            get { return fnames; }
            set { fnames = value; }
        }

        //this list will contain the time series info, it is populated by the user selecting an item in the "explrlb" listbox
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

        //variable which will keep track of the iterative transformation parameter if this method is used to transform the data
        private double trnsform = 0;

        public double Trnsform
        {
            get { return trnsform; }
            set { trnsform = value; }
        }


        public explrForm()
        {
            InitializeComponent();
        }

        //this is the method that is executed when we click on the "Load Data" button in the explrForm
        //it calls the populatelistbox method
        private void explrbt_Click(object sender, EventArgs e)
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
                                //now populate the list box
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


        //this method is used to populate explrlb in explrForm
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
                    explrlb.Items.Add(f + " , " + excelSheetNames[i]);
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

        //this method is called once the user chooses a tab from the listbox
        //it (a) loads the time series data (b) calls the Hookup method which will produce a number of graphs and summary statistics c) saves a copy of the underlying data to a CSV file
        private void explrlb_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            //clear any data that may already be in the Tseries object
            Tseries.Clear();

            int position = explrlb.SelectedIndex;
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
            DataSet data = new DataSet();
            OleDbConnection con = new OleDbConnection(c);
            //var
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT * FROM [{0}]", curItem);
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
            try
            {
                con.Open();
                adapter.Fill(dataTable);
                data.Tables.Add(dataTable);
                explrdgv1.AutoGenerateColumns = true;
                explrdgv1.DataSource = data.Tables[0];
                string s = data.Tables[0].TableName;
                explrdgv1.AllowUserToAddRows = false;
                explrdgv1.MultiSelect = false;
                explrdgv1.ReadOnly = true;
                //now we populate Tseries
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
                    //call the HookUpData method which will assist with the production of charts etc
                    HookUpData();
                }
                catch (Exception x)
                {
                    explrForm_hide();
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
                //save down a CSV copy of the underlying data in case the user wants it
                curItem = curItem.Replace("'", "");
                curItem = curItem.Replace("$", "");
                string p = curItem + ".csv";
                string q = df.Replace(".xlsx", p);
                StreamWriter sw = new StreamWriter(q, false);
                sw.WriteLine("Data");
                foreach (double d in Tseries)
                    sw.WriteLine(d);
                sw.Close();
            }

        }

        //method to (a) populate a number of charts (b) call varioius methods of the Statistics class
        //calls the explrFormtbt_Click method which helps with data transformations etc
        public void HookUpData()
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            //call method to display the various objects
            explrForm_show();

            explrTS.Titles.Clear();
            explrTS.Titles.Add(new Title("Time Series (Data)", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            explrTS.ChartAreas[0].AxisY.LabelStyle.Font = explrTS.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            explrTS.ChartAreas[0].RecalculateAxesScale();
            explrhist1.Titles.Clear();
            explrhist1.Titles.Add(new Title("Histogram", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            explrhist1.ChartAreas[0].AxisY.LabelStyle.Font = explrhist1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            explrhist1.ChartAreas[0].RecalculateAxesScale();

            //plot the TSeries data in the explrTS chart
            explrTS.Series.Clear();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.explrTS.Series.Add(series1);
            int z = 0;
            foreach (double dd in Tseries)
            {
                series1.Points.AddXY(z, Tseries[z]);
                z++;
            }
            explrTS.Invalidate();

            Statistics S = new Statistics(Tseries);

            //creating the info upon which the explrhist1 chart (i.e. histogram) will be based
            var b = S.histogramBins(S.D);
            var v = S.histogramValues(S.D, b);
            int y = 0;
            S.HistBins.Clear();
            S.HistValues.Clear();
            foreach (var variable in b)
            {
                S.HistBins.Add(variable);
                S.HistValues.Add(v[y]);
                y++;
            }

            //determine an initial guess of the Weibull shape parameter
            double guess = S.weibull_initial_guess(S.D);
            //determine the maximum likelihood estimates of the weibull shape and scale parameters
            S.weibull_parameter_estimate(S.D, guess, 100);

            //using the above shape and scale weibull parameters, create a sample from the Weibull distribution and break it up into the 
            //appropriate bins for a histogram
            S.WBins.Clear();
            S.WValues.Clear();
            y = 0;
            List<double> r = S.weibull_sample(S.D.Count(), S.WShape, S.WScale);
            S.WBins = S.HistBins;
            S.WValues = S.histogramValues(r, S.WBins);

            //histogram of the original data and of the Weibull sample
            explrhist1.Series.Clear();
            var series2 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Data",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            this.explrhist1.Series.Add(series2);
            int q = 0;
            foreach (double dd in S.HistValues)
            {
                series2.Points.AddXY((int)S.HistBins[q], S.HistValues[q]);
                q++;
            }
            explrhist1.Legends[0].DockedToChartArea = "ChartArea1";

            var series3 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Weibull",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            this.explrhist1.Series.Add(series3);
            q = 0;
            foreach (double dd in S.WValues)
            {
                series3.Points.AddXY((int)S.HistBins[q], S.WValues[q]);
                q++;
            }
            explrhist1.ChartAreas[0].AxisY.Maximum = Math.Max(S.maximum(S.HistValues) * 1.05, S.maximum(S.WValues) * 1.05);
            explrhist1.Invalidate();


            //display the summary statistics
            explrForm_show();
            double average = Math.Round(S.avg(S.D), 1);
            double standarddeviation = Math.Round(S.stdev(S.D), 1);
            double maximum = Math.Round(S.maximum(S.D), 1);
            double minimum = Math.Round(S.minimum(S.D), 1);
            double skewness = Math.Round(S.skew(S.D), 2);
            double kurtosis = Math.Round(S.kurt(S.D), 2);
            double weibullshape = Math.Round(S.WShape, 2);
            double weibullscale = Math.Round(S.WScale, 1);
            explrFormlb10.Text = average.ToString();
            explrFormlb11.Text = standarddeviation.ToString();
            explrFormlb12.Text = maximum.ToString();
            explrFormlb13.Text = minimum.ToString();
            explrFormlb14.Text = skewness.ToString();
            explrFormlb15.Text = kurtosis.ToString();
            explrFormlb16.Text = weibullshape.ToString();
            explrFormlb17.Text = weibullscale.ToString();

            this.P = new Statistics(S);

            //call the explrFormtbt_Click method which will help with the data transformations etc
            explrFormtbt_Click(new object(), new EventArgs());

            this.Cursor = old;

        }

        private void explrForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            explrForm_hide();
        }

        //method to hide buttons etc
        private void explrForm_hide()
        {
            explrTS.Hide();
            explrdgv1.Hide();
            explrhist1.Hide();
            explrFormlb1.Hide();
            explrFormlb2.Hide();
            explrFormlb3.Hide();
            explrFormlb4.Hide();
            explrFormlb5.Hide();
            explrFormlb6.Hide();
            explrFormlb7.Hide();
            explrFormlb8.Hide();
            explrFormlb9.Hide();
            explrFormlb10.Hide();
            explrFormlb11.Hide();
            explrFormlb12.Hide();
            explrFormlb13.Hide();
            explrFormlb14.Hide();
            explrFormlb15.Hide();
            explrFormlb16.Hide();
            explrFormlb17.Hide();
            explrFormitrb.Hide();
            explrFormwrb.Hide();
            explrFormpl.Hide();
            explrFormlb25.Hide();
            explrFormM.Hide();
            explrFormhist2.Hide();
            explrFormQQ.Hide();
            explrFormstandardisebt.Hide();
            transformedlbl.Hide();
            transformedskewlbl.Hide();
            transformedkurtlbl.Hide();
            transformedkurtval.Hide();
            transformedskewval.Hide();
            standardisedlbl.Hide();
            standardisedskewlbl.Hide();
            standardisedkurtlbl.Hide();
            standardisedkurtval.Hide();
            standardisedskewval.Hide();
            explrFormMeans.Hide();
            explrFormSavebt.Hide();
        }

        //method to show buttons etc
        private void explrForm_show()
        {
            explrTS.Show();
            explrhist1.Show();
            explrFormlb1.Show();
            explrFormlb2.Show();
            explrFormlb3.Show();
            explrFormlb4.Show();
            explrFormlb5.Show();
            explrFormlb6.Show();
            explrFormlb7.Show();
            explrFormlb8.Show();
            explrFormlb9.Show();
            explrFormlb10.Show();
            explrFormlb11.Show();
            explrFormlb12.Show();
            explrFormlb13.Show();
            explrFormlb14.Show();
            explrFormlb15.Show();
            explrFormlb16.Show();
            explrFormlb17.Show();
            explrFormitrb.Show();
            explrFormwrb.Show();
            explrFormpl.Show();
            explrFormlb25.Show();
            explrFormstandardisebt.Show();
            transformedlbl.Show();
            transformedskewlbl.Show();
            transformedkurtlbl.Show();
            transformedskewval.Show();
            transformedkurtval.Show();
            explrFormSavebt.Show();
        }

        //if the user clicks the weibull radio button then the explrFormtbt_Click method is called
        private void explrFormwrb_CheckedChanged(object sender, EventArgs e)
        {
            explrFormtbt_Click(new object(), new EventArgs());
        }

        //if the user clicks the iterative radio button then the explrFormtbt_Click method is called
        private void explrFormitrb_CheckedChanged(object sender, EventArgs e)
        {
            explrFormtbt_Click(new object(), new EventArgs());
        }

        //this method will help the user to transform the data
        //it will also present a histogram of the transformed data and a qqplot of the transformed data
        private void explrFormtbt_Click(object sender, EventArgs e)
        {
            List<double> lst1temp = new List<double>();
            List<double> lst2temp = new List<double>();
            List<double> lst1 = new List<double>();
            List<double> lst2 = new List<double>();

            explrFormMeans.Hide();
            standardisedlbl.Hide();
            standardisedskewlbl.Hide();
            standardisedkurtlbl.Hide();
            standardisedskewval.Hide();
            standardisedkurtval.Hide();


            //if the user wants to transform the data iteratively this section of code is called
            //see chapter 3 of accompanying manual for more details
            if (explrFormitrb.Checked == true)
            {
                //calculate the iterative values using methods of the Statistics class
                lst1temp = P.gen_list(0, 2.5, 300);
                lst2temp = P.iterative_measurement(lst1temp, P.D);

                //we will do a check to see if any of the lst2 values are NaN, if they are remove them
                for (int h = 0; h < lst2temp.Count(); h++)
                    if (double.IsNaN(lst2temp[h]))
                    {
                        //do nothing
                    }
                    else
                    {
                        lst2.Add(lst2temp[h]);
                        lst1.Add(lst1temp[h]);
                    }
                ;

                //determine which transformation gives the lowest asymmetry measure, then transform the data
                double m = P.closestzero(lst2);
                int i = lst2.IndexOf(m);
                double trans = lst1[i];
                this.Trnsform = trans;
                if (P.TransformedData.Count() > 0)
                    P.TransformedData.Clear();
                foreach (double dd in P.D)
                {
                    P.TransformedData.Add(Math.Pow(dd, trans));
                }

                //clear the TBins and TValues and then populate them            
                P.TBins.Clear();
                P.TValues.Clear();
                P.TBins = P.histogramBins(P.TransformedData);
                P.TValues = P.histogramValues(P.TransformedData, P.TBins);

                //plot the histogram of the transformed data
                explrFormhist2.Show();
                explrFormhist2.Titles.Clear();
                explrFormhist2.Titles.Add(new Title("Histogram Transformed Data", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
                explrFormhist2.Series.Clear();
                explrFormhist2.ChartAreas[0].RecalculateAxesScale();
                var series5 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Transformed Data",
                    Color = System.Drawing.Color.Blue,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Column
                };
                this.explrFormhist2.Series.Add(series5);
                int q = 0;
                foreach (double dd in P.TValues)
                {
                    series5.Points.AddXY((int)P.TBins[q], P.TValues[q]);
                    q++;
                }
                explrFormhist2.ChartAreas[0].AxisY.Maximum = Math.Max(P.maximum(P.TValues) * 1.05, 0);

                //plot the asymmetry measure
                explrFormM.Show();
                explrFormM.Titles.Clear();
                explrFormM.Titles.Add(new Title("Choosing the Transformation", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
                explrFormM.ChartAreas[0].AxisY.LabelStyle.Font = explrFormM.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
                explrFormM.ChartAreas[0].RecalculateAxesScale();
                explrFormM.Series.Clear();
                var series4 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Series4",
                    Color = System.Drawing.Color.Blue,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Line
                };
                this.explrFormM.Series.Add(series4);
                int z = 0;
                explrFormM.ChartAreas[0].AxisY.Minimum = -0.5;
                foreach (double dd in lst2)
                {
                    series4.Points.AddXY(Math.Round(lst1[z], 2), dd);
                    z++;
                }
                var series7 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Series7",
                    Color = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Line
                };
                this.explrFormM.Series.Add(series7);
                z = 0;
                foreach (double dd in lst2)
                {
                    series7.Points.AddXY(Math.Round(lst1[z], 2), 0);
                    z++;
                }
                explrFormM.Invalidate();

            }

            //if the user decides to use the weibull transformation method this segment of code is executed
            if (explrFormwrb.Checked == true)
            {
                if (P.TransformedData.Count() > 0)
                    P.TransformedData.Clear();
                double guess = P.weibull_initial_guess(P.D);
                P.weibull_parameter_estimate(P.D, guess, 100);
                double t = P.WShape / 3.6;
                //transform the data
                foreach (double dd in P.D)
                {
                    P.TransformedData.Add(Math.Pow(dd, t));
                }
                P.TBins.Clear();
                P.TValues.Clear();
                P.TBins = P.histogramBins(P.TransformedData);
                P.TValues = P.histogramValues(P.TransformedData, P.TBins);

                //plot the histogram of the transformed data
                explrFormhist2.Show();
                explrFormhist2.Titles.Clear();
                explrFormhist2.Titles.Add(new Title("Histogram Transformed Data", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
                explrFormhist2.Series.Clear();
                var series5 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Transformed Data",
                    Color = System.Drawing.Color.Blue,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Column
                };
                this.explrFormhist2.Series.Add(series5);
                int q = 0;
                foreach (double dd in P.TValues)
                {
                    series5.Points.AddXY((int)P.TBins[q], P.TValues[q]);
                    q++;
                }
                explrFormhist2.ChartAreas[0].AxisY.Maximum = Math.Max(P.maximum(P.TValues) * 1.05, 0);
                //no need to plot the asymmetry measure section
                explrFormM.Hide();
                //end of the weibull transformation section
            }

            //now we want to create a normally distributed sample with mean 0 and sd 1
            List<double> normal = P.normal_sample(P.TransformedData.Count(), 0, 1);
            normal.Sort();
            List<double> sortedTD = new List<double>();
            foreach (double dd in P.TransformedData)
            {
                sortedTD.Add(dd);
            }
            sortedTD.Sort();
            //now we want to create a qqplot
            explrFormQQ.Show();
            explrFormQQ.Titles.Clear();
            explrFormQQ.Titles.Add(new Title("Q-QPlot Transformed Data", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            explrFormQQ.Series.Clear();
            explrFormQQ.ChartAreas[0].RecalculateAxesScale();
            var series6 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "QQ",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Point
            };
            this.explrFormQQ.Series.Add(series6);
            int u = 0;
            foreach (double dd in normal)
            {
                series6.Points.AddXY(Math.Round(dd, 2), sortedTD[u]);
                u++;
            }
            explrFormQQ.ChartAreas[0].AxisY.Maximum = Math.Max(P.maximum(sortedTD) * 1.05, 0);
            explrFormQQ.ChartAreas[0].AxisX.LabelStyle.Enabled = false;

            //skewness and kurtosis for transformed data
            double skewn = Math.Round(P.skew(P.TransformedData), 2);
            double kurto = Math.Round(P.kurt(P.TransformedData), 2);
            transformedskewval.Text = skewn.ToString();
            transformedkurtval.Text = kurto.ToString();
        }

        //once the time series data has been transformed under either an iterative or Weibull like transformation, the transformed
        //data can be further standardised, the standardise() method of the Statistics class is used
        //see Section 3.1 of accompanying manual
        private void explrFormstandardisebt_Click(object sender, EventArgs e)
        {
            standardiseForm f1 = new standardiseForm();
            f1.N1 = this.R;
            f1.M1 = this.C;

            //display the dialog
            if (f1.ShowDialog() == DialogResult.OK)
            {
                //need to figure out the number of rows and number of columns in the underlying spreadsheet
                int m2;
                bool m2Test = int.TryParse(f1.standardisetxtbcols.Text, out m2);
                int n2;
                bool n2Test = int.TryParse(f1.standardisetxtbrows.Text, out n2);
                if ((n2 > 0) && (m2 > 0) && (n2 <= R) && (m2 <= C))
                {
                    P.standardise(P.TransformedData, this.R, this.C, n2, m2);
                    //now plot the means
                    explrFormMeans.Show();
                    explrFormMeans.Titles.Clear();
                    explrFormMeans.Titles.Add(new Title("Transformed Data: Mean of groups", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
                    explrFormMeans.ChartAreas[0].AxisY.LabelStyle.Font = explrFormMeans.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
                    explrFormMeans.ChartAreas[0].RecalculateAxesScale();
                    explrFormMeans.Series.Clear();
                    var series10 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "Series10",
                        Color = System.Drawing.Color.Blue,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.Line
                    };
                    this.explrFormMeans.Series.Add(series10);
                    explrFormMeans.ChartAreas[0].AxisY.Minimum = Math.Round(P.minimum(P.StdMeans) * 0.95, 2);
                    explrFormMeans.ChartAreas[0].AxisY.Maximum = Math.Round(P.maximum(P.StdMeans) * 1.05, 2);
                    int u = 0;
                    foreach (double dd in P.StdMeans)
                    {
                        series10.Points.AddXY(u, Math.Round(dd, 2));
                        u++;
                    }

                    //skewness and kurtosis for transformed data
                    standardisedlbl.Show();
                    standardisedskewlbl.Show();
                    standardisedkurtlbl.Show();
                    standardisedskewval.Show();
                    standardisedkurtval.Show();
                    double sk = Math.Round(P.skew(P.StandardisedTransformedData), 2);
                    double ku = Math.Round(P.kurt(P.StandardisedTransformedData), 2);
                    standardisedskewval.Text = sk.ToString();
                    standardisedkurtval.Text = ku.ToString();

                }

            }

        }

        //this method is called once the user clicks the Save Data button, copies of the transformed and transformed + standardised data will be saved to excel
        //copies of a) original b) transformed c) transformed + standardised, will also be saved to distinct CSV files
        //this method uses the ListToExcel() method
        private void explrFormSavebt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            //save the transformed data
            if (P.TransformedData.Count() > 0)
                ListToExcel(P.TransformedData, "Transformed", this.R, this.C);

            //save the transformed and standardised data
            if (P.StandardisedTransformedData.Count() > 0)
            {
                int r = 0;
                int c = 0;
                if ((this.C % P.M2) == 0)
                    c = this.C;
                else
                    c = (int)(this.C / P.M2) * P.M2;
                if ((this.R % P.N2) == 0)
                    r = this.R;
                else
                    r = (int)(this.R / P.N2) * P.N2;
                ListToExcel(P.StandardisedTransformedData, "Standardised", r, c);
            }

            this.Cursor = old;
        }

        //method to save data to the relevant Excel workbook
        //also saves a copy of the data to a CSV file
        public void ListToExcel(List<double> vals, string type, int rows, int columns)
        {
            //start excel
            NsExcel.Application excapp = new Microsoft.Office.Interop.Excel.Application();

            //if you want to make excel visible           
            excapp.Visible = false;

            //create a blank workbook
            int position = explrlb.SelectedIndex;
            string curItem = Tab[position];
            curItem = curItem.Replace("'", "");
            curItem = curItem.Replace("$", "");
            string df = Flocations[position];

            string workbookPath = df;
            var workbook = excapp.Workbooks.Open(workbookPath,
                0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                true, false, 0, true, false, false);

            Excel.Worksheet newWorksheet;
            newWorksheet = (Excel.Worksheet)workbook.Worksheets.Add();
            string l = type + curItem;
            bool found = false;
            // Loop through all worksheets in the workbook
            foreach (Excel.Worksheet sheet in workbook.Sheets)
            {
                // Check the name of the current sheet
                if (sheet.Name == l)
                {
                    found = true;
                    break; // Exit the loop now
                }
            }
            if (found)
            {
                // Reference it by name
                excapp.DisplayAlerts = false;
                Excel.Worksheet mySheet = workbook.Sheets[l];
                mySheet.Delete();
                excapp.DisplayAlerts = true;
            }

            newWorksheet.Name = l;

            //you select now an individual cell
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Excel.Range c1 = newWorksheet.Cells[i + 1, j + 1];
                    var range = newWorksheet.get_Range(c1, c1);
                    range.Value2 = Math.Round(vals[i * columns + j], 4);

                }
            }

            workbook.Save();
            workbook.Close();

            //code to saveoutput to CSV file, the first row of the CSV file will contain information on the type of transformation that was 
            //utilised
            string p = l + ".csv";
            string o = workbookPath.Replace(".xlsx", p);
            string transform = "";
            StreamWriter sw = new StreamWriter(o, false);
            if (explrFormitrb.Checked == true)
            {
                sw.WriteLine("Iterative");
                transform = this.Trnsform.ToString();
                sw.WriteLine(transform);
            }
            if (explrFormwrb.Checked == true)
            {
                sw.WriteLine("Weibull");
                transform = this.P.WShape.ToString();
                sw.WriteLine(transform);
                transform = this.P.WScale.ToString();
                sw.WriteLine(transform);
            }
            sw.WriteLine("Data");
            foreach (double d in vals)
                sw.WriteLine(d);
            sw.Close();

            curItem = curItem.Replace("$", "");
        }


    }


}



