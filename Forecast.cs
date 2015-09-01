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
    //MDI Child form, used for forecasting and simulations
    //See Chapter 6 in accompanying pdf for more details
    
    public partial class Forecast : Form
    {
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

        //this list will contain the data i.e. depending on the ARIMA/ARCH/GARCH model, it may be the plain data, plain data - mean, differenced data
        private List<double> data = new List<double>();

        public List<double> Data
        {
            get { return data; }
            set { data = value; }
        }

        private int r = 0;

        public int R
        {
            get { return r; }
            set { r = value; }
        }

        private int c = 0;

        public int C
        {
            get { return c; }
            set { c = value; }
        }

        private BindingList<model> modellist = new BindingList<model>();

        internal BindingList<model> Modellist
        {
            get { return modellist; }
            set { modellist = value; }
        }

        private BindingSource bs1 = new BindingSource();

        private BindingSource bs2 = new BindingSource();

        private BindingSource bs3 = new BindingSource();

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private double transform;

        public double Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        private double transformscale;

        public double Transformscale
        {
            get { return transformscale; }
            set { transformscale = value; }
        }

        public Forecast()
        {
            InitializeComponent();
        }
        
        //this method is called when the user clicks on the Load Data button
        //it calls the populatelistbox() method
        private void ForecastLoadbt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            hide();

            String strfl;
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }

            this.Cursor = old;
        }

        //this method is used to populate ListBox
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
                    Forecastlb.Items.Add(f + " , " + excelSheetNames[i]);
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

        //this method is called once the user selects a value from the listbox
        //it a) loads the underlying data into the Tseries list b) calls the loadmodels() method c) calls the populatedatagridview method
        private void Forecastlb_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            Tseries.Clear();

            int position = Forecastlb.SelectedIndex;
            if (position == -1)
            {
                this.Cursor = old;
                return;
            }

            string curItem = Tab[position];
            string df = Flocations[position];
            string f = ";Extended Properties=\"Excel 12.0;HDR=NO\"";
            string c = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + df + f;
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
                Modellist.Clear();
                this.loadmodels(position);
                if (Modellist.Count() == 0)
                {
                    hide();
                    string message = "No ARIMA/ARCH/GARCH models relating to this data set have been found. Please go back to ARIMA section, run the models and save output.";
                    string caption = "ARIMA/ARCH/GARCH Models";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;
                    result = MessageBox.Show(message, caption, buttons);
                }
                else
                {
                    populatedatagridview();
                    this.P = new Statistics(Tseries);
                }
                this.Cursor = old;
            }
        }

        //method which loads the details of previously calibrated ARIMA/ARCH/GARCH models into modellist
        private void loadmodels(int position)
        {
            string interim1 = Fnames[position].Replace(".xlsx", "");
            string interim2 = Tab[position].Replace("$", "");
            string interim3 = Flocations[position].Replace(fnames[position], interim1 + interim2 + "Models.csv");
            if (!File.Exists(interim3))
                return;
            StreamReader sr = new StreamReader(interim3);
            string line;
            sr.ReadLine();
            Modellist.Clear();
            while ((line = sr.ReadLine()) != null)
            {
                bool subtractmean = false;
                char[] delimiters = new char[] { '\t', ' ' };
                string[] details = line.Split(delimiters);
                string Transform = details[0];
                double Shape = double.Parse(details[1]);
                double Scale = double.Parse(details[2]);
                string Model = details[3];
                int P = int.Parse(details[4]);
                int Q = int.Parse(details[5]);
                int D = int.Parse(details[6]);
                double A = double.Parse(details[7]);
                double SS = double.Parse(details[8]);
                double Variance = double.Parse(details[9]);
                List<double> Parameters = new List<double>();
                for (int i = 0; i < P + Q; i++)
                {
                    Parameters.Add(double.Parse(details[10 + i]));
                }
                int archQ = int.Parse(details[10 + P + Q]);
                int archP = int.Parse(details[11 + P + Q]);
                double archAIC = double.Parse(details[12 + P + Q]);
                List<double> archParameters = new List<double>();
                if (archQ == 0)
                {
                    archParameters.Add(0);
                }
                else
                {
                    //if we have an ARCH(Q) model then there are Q+1 parameters
                    for (int i = 0; i < archP + (archQ + 1); i++)
                    {
                        archParameters.Add(double.Parse(details[13 + P + Q + i]));
                    }
                }
                subtractmean = bool.Parse(details[13 + P + Q + archP + (archQ + 1)]);
                Modellist.Add(new model(Model, P, Q, D, A, 0, Parameters, new List<double> { 0.0 }, Variance, new List<double> { 0.0 }, SS, Transform, Shape, Scale, archQ, archP, archParameters, archAIC, subtractmean));
            }
        }

        //populates the Fitted ARIMA/ARCH/GARCH datagridview
        private void populatedatagridview()
        {
            bs1.DataSource = Modellist;
            Mdgv.Show();
            ForecastMlbl.Show();
            Mdgv.DataSource = bs1;
            Mdgv.AllowUserToAddRows = false;
            Mdgv.AutoGenerateColumns = false;
            DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
            col1.DataPropertyName = "Type";
            col1.HeaderText = "Alg";
            col1.Name = "Blah";
            col1.Width = 50;
            Mdgv.Columns.Add(col1);
            DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
            col2.DataPropertyName = "P";
            col2.HeaderText = "AR";
            col2.Name = "AR Params";
            col2.Width = 50;
            Mdgv.Columns.Add(col2);
            DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
            col3.DataPropertyName = "Q";
            col3.HeaderText = "MA";
            col3.Name = "MA Params";
            col3.Width = 50;
            Mdgv.Columns.Add(col3);
            DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
            col4.DataPropertyName = "D";
            col4.HeaderText = "Diff";
            col4.Name = "Diff";
            col4.Width = 50;
            Mdgv.Columns.Add(col4);
            DataGridViewColumn col5 = new DataGridViewTextBoxColumn();
            col5.DataPropertyName = "Variance";
            col5.HeaderText = "Var";
            col5.Name = "Var";
            col5.Width = 50;
            Mdgv.Columns.Add(col5);
            col5.DefaultCellStyle.Format = "n4";
            DataGridViewColumn col6 = new DataGridViewTextBoxColumn();
            col6.DataPropertyName = "Sumsquaredresiduals";
            col6.HeaderText = "SS resid";
            col6.Name = "SS resid";
            col6.Width = 60;
            Mdgv.Columns.Add(col6);
            col6.DefaultCellStyle.Format = "n2";
            DataGridViewColumn col7 = new DataGridViewTextBoxColumn();
            col7.DataPropertyName = "Transformtype";
            col7.HeaderText = "Transform";
            col7.Name = "Transform";
            col7.Width = 60;
            Mdgv.Columns.Add(col7);
            DataGridViewColumn col8 = new DataGridViewTextBoxColumn();
            col8.DataPropertyName = "Transformshape";
            col8.HeaderText = "Shape";
            col8.Name = "Shape";
            col8.Width = 60;
            col8.DefaultCellStyle.Format = "n2";
            Mdgv.Columns.Add(col8);
            DataGridViewColumn col9 = new DataGridViewTextBoxColumn();
            col9.DataPropertyName = "Transformscale";
            col9.HeaderText = "Scale";
            col9.Name = "Scale";
            col9.Width = 60;
            DataGridViewColumn col10 = new DataGridViewTextBoxColumn();
            col10.DataPropertyName = "archP";
            col10.HeaderText = "Garch(p)";
            col10.Name = "Garch(p)";
            col10.Width = 60;
            //col9.DefaultCellStyle.Format = "n2";
            Mdgv.Columns.Add(col10);
            DataGridViewColumn col11 = new DataGridViewTextBoxColumn();
            col11.DataPropertyName = "archQ";
            col11.HeaderText = "Garch(Q)";
            col11.Name = "Garch(Q)";
            col11.Width = 60;
            //col9.DefaultCellStyle.Format = "n2";
            Mdgv.Columns.Add(col11);
            Mdgv.Columns["P"].Visible = false;
            Mdgv.Columns["Q"].Visible = false;
            Mdgv.Columns["Aic"].Visible = false;
            Mdgv.Columns["Bic"].Visible = false;
            Mdgv.Columns["Type"].Visible = false;
            Mdgv.Columns["Variance"].Visible = false;
            Mdgv.Columns["D"].Visible = false;
            Mdgv.Columns["Sumsquaredresiduals"].Visible = false;
            Mdgv.Columns["Transformtype"].Visible = false;
            Mdgv.Columns["Transformshape"].Visible = false;
            Mdgv.Columns["Transformscale"].Visible = false;
            Mdgv.Columns["archP"].Visible = false;
            Mdgv.Columns["archQ"].Visible = false;
            Mdgv.Columns["archAic"].Visible = false;
            Mdgv.Columns["Subtractmean"].Visible = false;

            //now we will determine which of the models has the lowest residual sum of squares
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
            Mdgv.Rows[minposition].DefaultCellStyle.BackColor = Color.LightBlue;
            Mdgv.FirstDisplayedScrollingRowIndex = minposition;
            Mdgv.Rows[minposition].Cells[1].Selected = true;
        }

        //method to hide buttons, graphs etc.
        private void hide()
        {
            Mdgv.Hide();
            ForecastMlbl.Hide();
            numericUpDown1.Hide();
            numericUpDown2.Hide();
            ForecastRun.Hide();
            ForecastP1.Hide();
            label1.Hide();
            label2.Hide();
            ForecastP2.Hide();
            ForecastP3.Hide();
        }

        private void Forecast_Load(object sender, EventArgs e)
        {
            hide();
        }

        //this method is called once the user selects a value from the listbox
        //it a) loads the underlying data into the Tseries list b) calls the loadmodels() method c) calls the populatedatagridview method
        private void Forecastlb_SelectedValueChanged_1(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            Tseries.Clear();
            int position = Forecastlb.SelectedIndex;
            if (position == -1)
            {
                this.Cursor = old;
                return;
            }
            string curItem = Tab[position];
            string df = Flocations[position];
            string f = ";Extended Properties=\"Excel 12.0;HDR=NO\"";
            string c = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + df + f;
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
                Modellist.Clear();
                this.loadmodels(position);
                if (Modellist.Count() == 0)
                {
                    hide();
                    string message = "No ARIMA/ARCH/GARCH models relating to this data set have been found. Please go back to ARIMA section, run the models and save output.";
                    string caption = "ARIMA/ARCH/GARCH Models";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;
                    result = MessageBox.Show(message, caption, buttons);
                }
                else
                {
                    populatedatagridview();
                    this.P = new Statistics(Tseries);
                    int msize = modellist.Count();
                    int[] pos = new int[msize];
                    double[] residss = new double[msize];
                    int minposition = 0;
                    double minresidss = 100000000;
                    for (int i = 0; i < msize; i++)
                    {
                        pos[i] = i;
                        residss[i] = modellist[i].Sumsquaredresiduals;
                        if (residss[i] < minresidss)
                        {
                            minresidss = residss[i];
                            minposition = i;
                        }
                    }
                    Mdgv_CellClick(Mdgv, new DataGridViewCellEventArgs(minposition, 0));
                }
                this.Cursor = old;
            }
        }

        //this method helps setup the numeric counters
        private void Mdgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            ForecastP1.Hide();
            ForecastP2.Hide();
            ForecastP3.Hide();
            numericUpDown1.Show();
            numericUpDown2.Show();
            ForecastRun.Show();
            label1.Show();
            label2.Show();
            //depending on the model that the user chooses, setup the Data list object which will be used elsewhere
            //the numeric counters will be based off of the Data list
            int mposition = 0;
            mposition = Mdgv.SelectedRows[0].Index;
            int nd = Modellist[mposition].D;
            bool subtmean = false;
            subtmean = Modellist[mposition].Subtractmean;
            this.Data.Clear();
            if (nd > 0)
            {
                P.DifferencedData.Clear();
                this.P.Diff = nd;
                P.difference();
                foreach (double dd in P.DifferencedData)
                    this.Data.Add(dd);
            }
            else
            {
                List<double> tempdata = new List<double>();
                if (subtmean == true)
                {
                    double aver = P.avg(P.D);
                    foreach (double dd in P.D)
                        tempdata.Add(dd - P.avg(P.D));
                }
                else
                {
                    foreach (double dd in P.D)
                        tempdata.Add(dd);
                }
                foreach (double dd in tempdata)
                    this.Data.Add(dd);
            }
            //now setup the numeric objects
            int count = Data.Count();
            numericUpDown1.Value = 1;
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 200;
            numericUpDown1.Increment = 1;
            int nar = 0;
            int nma = 0;
            int narchp = 0;
            int narchq = 0;
            nar = Modellist[mposition].P;
            nma = Modellist[mposition].Q;
            narchp = Modellist[mposition].archP;
            narchq = Modellist[mposition].archQ;
            int inc = (int)Math.Floor((double)this.Data.Count() / 500.0);
            List<double> lst = new List<double> { (double)nar, (double)nma, (double)narchp, (double)narchq };
            numericUpDown2.Minimum = (int)P.maximum(lst) + 2;
            numericUpDown2.Maximum = Data.Count();
            numericUpDown2.Value = Data.Count();
            numericUpDown2.Increment = inc;
            this.Cursor = old;
        }

        //this method is called when the user clicks the Forecast Run button
        //depending on the user input it a) creates the forecasts b) creates the simulations c) calls the plot1() plot2() and plot(3) methods
        private void ForecastRun_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            //getting all the information we need about the model
            int mposition = 0;
            mposition = Mdgv.SelectedRows[0].Index;
            int nar = 0;
            int nma = 0;
            int nd = 0;
            int narchp = 0;
            int narchq = 0;
            bool subtmean = false;
            string transform;
            double shape;
            List<double> armaestimates = new List<double>();
            List<double> garchestimates = new List<double>();
            double v = 0;
            nar = Modellist[mposition].P;
            nma = Modellist[mposition].Q;
            nd = Modellist[mposition].D;
            subtmean = Modellist[mposition].Subtractmean;
            v = Modellist[mposition].Variance;
            narchp = Modellist[mposition].archP;
            narchq = Modellist[mposition].archQ;
            for(int i =0;i<nar+nma;i++)
                armaestimates.Add( Modellist[mposition].Parameters[i]);
            for (int i = 0; i < narchp + narchq+1; i++)
                garchestimates.Add(Modellist[mposition].archParameters[i]);
            transform = Modellist[mposition].Transformtype;
            shape = Modellist[mposition].Transformshape;
            //temporary lists used in plot2
            List<double> p2listf = new List<double>();
            List<double> p2lista = new List<double>();
            List<List<double>> p3simulations = new List<List<double>>();
            List<List<double>> p3lists = new List<List<double>>();
            int simulations = 100;

            //Remember: 
            //TSeries will contain the data - if the model transform is either Iterative or Weibull it will be transformed, otherwise it will be the unadjusted data
            //Data will contain the info for the first graph i.e. the object on which the models were calibrated. This could be the original data, transformed data, transformed data - mean, differenced data

            //setup the variables for the forecast
            int nsteps = (int)this.numericUpDown1.Value;
            int point = (int)this.numericUpDown2.Value;

            //now do the forecast for the first graph
            List<double> forecast = P.predictnstepsahead(data, armaestimates, nar, nma, nsteps, point);
            List<double> actual = new List<double>();
            int counter =0;
            while ((point - 1 + counter < data.Count()) && (counter <= nsteps))
            {
                actual.Add(this.Data[point - 1 + counter]);
                counter++;
            }

            //now draw the first plot 
            plot1(forecast, actual, nsteps, point);

            //if the time series is differenced then we need to calculate the forecast and actual values for the original series
            //and then undo the transform
            List<double> forecastD = new List<double>();
            List<double> actualD = new List<double>();
            if (nd > 0)
            {
                //Note if we assume d = 1, then we need to start with "point+1" for the forecasts in plot 1 and plot 2 to commence at the exact
                //same time point. This is due to differencing and the inclusion of a 0 in the predictnstepsaheadDD function for convention.
                //Hence, if d = 2 or larger, then the time points on the two graphs may not coincide exactly.
                
                //creating the forecast if we have an ARIMA model
                List<double> temp = P.predictnstepsaheadDD(Tseries, armaestimates, nar, nma, nsteps, point+1, nd);
                foreach(double dd in temp)
                    forecastD.Add(dd);

                //creating a list to hold the actual data if we have an ARIMA model
                counter = 0;
                while ((point  + counter < Tseries.Count()) && (counter <= nsteps))
                {
                    actualD.Add(this.Tseries[point  + counter]);
                    counter++;
                }
            }

            if ((transform == "Iterative") || (transform == "Weibull") || (subtmean == true))
            {
                //mean
                double m = 0;
                if (subtmean == true)
                    m = P.avg(Tseries);

                //if i in the ARIMA model is >0 then we want to work with forecastD and actualD lists
                if (nd > 0)
                {
                    List<double> tempA = P.unwind(forecastD, transform, shape, m);
                    List<double> tempB = P.unwind(actualD, transform, shape, m);
                    foreach (double dd in tempA)
                        p2listf.Add(dd);
                    foreach (double dd in tempB)
                        p2lista.Add(dd);
                }
                else
                {
                    List<double> tempA = P.unwind(forecast, transform, shape, m);
                    List<double> tempB = P.unwind(actual, transform, shape, m);
                    foreach (double dd in tempA)
                        p2listf.Add(dd);
                    foreach (double dd in tempB)
                        p2lista.Add(dd);
                }

                plot2(p2listf, p2lista, nsteps, point);
            }

            //ARMAsimulations with nd = 0
            if ((nd == 0) && (narchq == 0))
            {
                for (int i = 0; i < simulations; i++)
                {
                    List<double> tempC = P.ARMAsimulatenstepsahead(data, armaestimates, nar, nma, nsteps, point, v);
                    p3simulations.Add(tempC);
                }

            }

            //ARCHsimulations with nd = 0
            if ((nd == 0) && (narchq > 0)&&(narchp==0))
            {
                for (int i = 0; i < simulations; i++)
                {
                    List<double> tempC = P.ARCHsimulatenstepsahead(data, armaestimates, nar, nma, nsteps, point, v, narchq, garchestimates);
                    p3simulations.Add(tempC);
                }

            }

            //GARCHsimulations with nd = 0
            if ((nd == 0) && (narchq > 0) && (narchp > 0))
            {
                for (int i = 0; i < simulations; i++)
                {
                    List<double> tempC = P.GARCHsimulatenstepsahead(data, armaestimates, nar, nma, nsteps, point, v, narchq, narchp, garchestimates);
                    p3simulations.Add(tempC);
                }

            }


            //ARMAsimulations with nd > 0
            if ((nd > 0) && (narchq == 0))
            {
                for (int i = 0; i < simulations; i++)
                {
                    List<double> tempC = P.ARMAsimulatenstepsaheadDD(Tseries, armaestimates, nar, nma, nsteps, point, v,nd);
                    p3simulations.Add(tempC);
                }

            }

            //ARCHsimulations with nd > 0
            if ((nd > 0) && (narchq > 0) && (narchp == 0))
            {
                for (int i = 0; i < simulations; i++)
                {
                    List<double> tempC = P.ARCHsimulatenstepsaheadDD(Tseries, armaestimates, nar, nma, nsteps, point, v,nd, narchq, garchestimates);
                    p3simulations.Add(tempC);
                }

            }

            //GARCHsimulations with nd > 0
            if ((nd > 0) && (narchq > 0) && (narchp > 0))
            {
                for (int i = 0; i < simulations; i++)
                {
                    List<double> tempC = P.GARCHsimulatenstepsaheadDD(Tseries, armaestimates, nar, nma, nsteps, point, v, nd,narchq, narchp, garchestimates);
                    p3simulations.Add(tempC);
                }

            }

            //now we need to check if we need to do any unwinding
            if ((transform == "Iterative") || (transform == "Weibull") || (subtmean == true))
            {
                //mean
                double m = 0;
                if (subtmean == true)
                    m = P.avg(Tseries);

                for (int i = 0; i < simulations; i++)
                {
                        List<double> tempD = P.unwind(p3simulations[i], transform, shape, m);
                            p3lists.Add(tempD);
                }
                plot3(p3lists, nsteps, point);
            }

            this.Cursor = old;
        }

        //method to produce plot titled Forecast vs Actual [A]
        private void plot1(List<double> forecast, List<double> actual, int nsteps, int point)
        {
            ForecastP1.Show();
            ForecastP1.Titles.Clear();
            ForecastP1.Titles.Add(new Title("Forecast vs Actual [A]", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ForecastP1.ChartAreas[0].AxisY.LabelStyle.Font = ForecastP1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            ForecastP1.Legends[0].DockedToChartArea = "ChartArea1";
            ForecastP1.Series.Clear();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "forecast",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ForecastP1.Series.Add(series1);
            ForecastP1.Legends[0].DockedToChartArea = "ChartArea1";
            int z = 0;
            foreach (double dd in forecast)
            {
                series1.Points.AddXY(point+z, dd);
                z++;
            }
            var series2 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "actual",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ForecastP1.Series.Add(series2);
            z = 0;
            foreach (double dd in actual)
            {
                series2.Points.AddXY(point+z, dd);
                z++;
            }
            ForecastP1.ChartAreas[0].RecalculateAxesScale();
            ForecastP1.Invalidate();
        }

        //method to produce plot titled Forecast vs Actual [B]
        private void plot2(List<double> forecast, List<double> actual, int nsteps, int point)
        {
            ForecastP2.Show();
            ForecastP2.Titles.Clear();
            ForecastP2.Titles.Add(new Title("Forecast vs Actual [B]", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ForecastP2.ChartAreas[0].AxisY.LabelStyle.Font = ForecastP2.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            ForecastP2.Legends[0].DockedToChartArea = "ChartArea1";
            ForecastP2.Series.Clear();
            var series3 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "forecast",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ForecastP2.Series.Add(series3);
            ForecastP2.Legends[0].DockedToChartArea = "ChartArea1";
            int z = 0;
            foreach (double dd in forecast)
            {
                series3.Points.AddXY(point + z, dd);
                z++;
            }
            var series4 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "actual",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = true,
                //IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            this.ForecastP2.Series.Add(series4);
            z = 0;
            foreach (double dd in actual)
            {
                series4.Points.AddXY(point + z, dd);
                z++;
            }
            ForecastP2.ChartAreas[0].RecalculateAxesScale();
            ForecastP2.Invalidate();
        }

        //method to produce plot titled Simulation: 100 Paths
        private void plot3(List<List<double>> simulations, int nsteps, int point)
        {
            int i = 0;
            i = simulations.Count();
            ForecastP3.Show();
            ForecastP3.Titles.Clear();
            ForecastP3.Titles.Add(new Title("Simulation: 100 Paths", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ForecastP3.ChartAreas[0].AxisY.LabelStyle.Font = ForecastP3.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            ForecastP3.Legends[0].DockedToChartArea = "ChartArea1";
            ForecastP3.Series.Clear();
            for (int j = 0; j < i; j++)
            {
                int q = 1 + j;
                string nam="Simulation "+ q.ToString();
                var series = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = nam,
                    Color = System.Drawing.Color.Blue,
                    IsVisibleInLegend = false,
                    //IsXValueIndexed = true,
                    ChartType = SeriesChartType.Line
                };
                this.ForecastP3.Series.Add(series);
                ForecastP3.Legends[0].DockedToChartArea = "ChartArea1";
                int z = 0;
                foreach (double dd in simulations[j])
                {
                    series.Points.AddXY(point + z, dd);
                    z++;
                }
                ForecastP3.ChartAreas[0].RecalculateAxesScale();
                ForecastP3.Invalidate();
            }
        }

        //end of class
    }

}

