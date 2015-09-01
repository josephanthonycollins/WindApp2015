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
    //MDI Child form, used for fitting ARCH and GARCH models
    //See Chapter 5 in accompanying pdf for more details

    public partial class ARCH : Form
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

        //temporary list to hold the output of the ARCH models which have been run
        private BindingList<model> archmodellist = new BindingList<model>();

        internal BindingList<model> archModellist
        {
            get { return archmodellist; }
            set { archmodellist = value; }
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

        public ARCH()
        {
            InitializeComponent();
        }


        //this method is called once the user clicks on the Load Data button
        //the method calls the populatelistbox() method
        private void ARCHLoadbt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            hide();

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

        //this method populates the Listbox
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
                    ARCHlb.Items.Add(f + " , " + excelSheetNames[i]);
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

        //on selecting one of the values from the listbox, this method is called.
        //It a)populates the Tseries list b)calls the loadmodels() method c) calls the populatedatagridview() method
        private void ARCHlb_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            Tseries.Clear();

            int position = ARCHlb.SelectedIndex;
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
                    string message = "No ARIMA models relating to this data set have been found.\nPlease go back to ARIMA section, run the models and save output.";
                    string caption = "ARIMA Models";
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

        //method to read in the details of the previously calibrated ARIMA models
        //a list of model objects will be populated as a result of this method
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
                bool subtractmean = bool.Parse(details[13 + P + Q + archP + (archQ + 1)]);
                Modellist.Add(new model(Model, P, Q, D, A, 0, Parameters, new List<double> { 0.0 }, Variance, new List<double> { 0.0 }, SS, Transform, Shape, Scale, archQ, archP, archParameters, archAIC, subtractmean));
            }

        }

        //method to populate the Fitted ARIMA Models datagridview
        private void populatedatagridview()
        {
            bs1.DataSource = Modellist;
            ARCHdgv.Show();
            ARCHarimalbl.Show();
            ARCHmodels.Show();
            ARCHdgv.DataSource = bs1;
            ARCHdgv.AllowUserToAddRows = false;
            ARCHdgv.AutoGenerateColumns = false;
            DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
            col1.DataPropertyName = "Type";
            col1.HeaderText = "Alg";
            col1.Name = "Blah";
            col1.Width = 50;
            ARCHdgv.Columns.Add(col1);
            DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
            col2.DataPropertyName = "P";
            col2.HeaderText = "AR";
            col2.Name = "AR Params";
            col2.Width = 50;
            ARCHdgv.Columns.Add(col2);
            DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
            col3.DataPropertyName = "Q";
            col3.HeaderText = "MA";
            col3.Name = "MA Params";
            col3.Width = 50;
            ARCHdgv.Columns.Add(col3);
            DataGridViewColumn col4 = new DataGridViewTextBoxColumn();
            col4.DataPropertyName = "D";
            col4.HeaderText = "Diff";
            col4.Name = "Diff";
            col4.Width = 50;
            ARCHdgv.Columns.Add(col4);
            DataGridViewColumn col5 = new DataGridViewTextBoxColumn();
            col5.DataPropertyName = "Variance";
            col5.HeaderText = "Var";
            col5.Name = "Var";
            col5.Width = 50;
            ARCHdgv.Columns.Add(col5);
            col5.DefaultCellStyle.Format = "n4";
            DataGridViewColumn col6 = new DataGridViewTextBoxColumn();
            col6.DataPropertyName = "Sumsquaredresiduals";
            col6.HeaderText = "SS resid";
            col6.Name = "SS resid";
            col6.Width = 60;
            ARCHdgv.Columns.Add(col6);
            col6.DefaultCellStyle.Format = "n2";
            DataGridViewColumn col7 = new DataGridViewTextBoxColumn();
            col7.DataPropertyName = "Transformtype";
            col7.HeaderText = "Transform";
            col7.Name = "Transform";
            col7.Width = 60;
            ARCHdgv.Columns.Add(col7);
            DataGridViewColumn col8 = new DataGridViewTextBoxColumn();
            col8.DataPropertyName = "Transformshape";
            col8.HeaderText = "Shape";
            col8.Name = "Shape";
            col8.Width = 60;
            col8.DefaultCellStyle.Format = "n2";
            ARCHdgv.Columns.Add(col8);
            DataGridViewColumn col9 = new DataGridViewTextBoxColumn();
            col9.DataPropertyName = "Transformscale";
            col9.HeaderText = "Scale";
            col9.Name = "Scale";
            col9.Width = 60;
            col9.DefaultCellStyle.Format = "n2";
            ARCHdgv.Columns.Add(col9);
            ARCHdgv.Columns["P"].Visible = false;
            ARCHdgv.Columns["Q"].Visible = false;
            ARCHdgv.Columns["Aic"].Visible = false;
            ARCHdgv.Columns["Bic"].Visible = false;
            ARCHdgv.Columns["Type"].Visible = false;
            ARCHdgv.Columns["Variance"].Visible = false;
            ARCHdgv.Columns["D"].Visible = false;
            ARCHdgv.Columns["Sumsquaredresiduals"].Visible = false;
            ARCHdgv.Columns["Transformtype"].Visible = false;
            ARCHdgv.Columns["Transformshape"].Visible = false;
            ARCHdgv.Columns["Transformscale"].Visible = false;
            ARCHdgv.Columns["archP"].Visible = false;
            ARCHdgv.Columns["archQ"].Visible = false;
            ARCHdgv.Columns["archAic"].Visible = false;
            ARCHdgv.Columns["Subtractmean"].Visible = false;

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
            ARCHdgv.Rows[minposition].DefaultCellStyle.BackColor = Color.LightBlue;
            ARCHdgv.FirstDisplayedScrollingRowIndex = minposition;
            ARCHdgv.Rows[minposition].Cells[1].Selected = true;

        }

        //On clicking the Fit ARCH and GARCH button this method is called
        //it will automatically fit a number of ARCH and GARCH models to the data
        private void ARCHmodels_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            archModellist.Clear();
            int mposition = 0;
            mposition = ARCHdgv.SelectedRows[0].Index;
            int nar = 0;
            int nma = 0;
            int nd = 0;
            bool subtmean = false;
            List<double> arimaestimates = new List<double>();
            nar = Modellist[mposition].P;
            nma = Modellist[mposition].Q;
            nd = Modellist[mposition].D;
            subtmean = Modellist[mposition].Subtractmean;
            List<double> data = new List<double>();

            //setting up the data.......could be done in a smarter way
            if (nd > 0)
            {
                P.DifferencedData.Clear();
                this.P.Diff = nd;
                P.difference();
                foreach (double dd in P.DifferencedData)
                    data.Add(dd);
            }
            else
            {
                if (subtmean == true)
                {
                    double aver = P.avg(P.D);
                    List<double> tempdata = new List<double>();
                    foreach (double dd in P.D)
                        tempdata.Add(dd - P.avg(P.D));
                    P.D.Clear();
                    foreach (double dd in tempdata)
                        P.D.Add(dd);
                }
                foreach (double dd in P.D)
                    data.Add(dd);

            }

            //fit the ARCH models
            for (int i = 1; i < 6; i++)
            {
                List<double> archestandaic = P.archML(data, Modellist[mposition].Parameters, nar, nma, i);
                List<double> archparamest = new List<double>();
                for (int l = 0; l < i + 1; l++)
                    archparamest.Add(archestandaic[l]);
                List<double> resid = P.archresiduals(data, Modellist[mposition].Parameters, nar, nma, i, archparamest);
                List<double> acfresid = P.sampleautocorrelation(resid, 100);
                if ((Math.Abs(archestandaic[i + 1]) > 1000000) || (double.IsNaN(archestandaic[i + 1])))
                    continue;
                else
                    archModellist.Add(new model("ML", nar, nma, nd, Modellist[mposition].Aic, Modellist[mposition].Bic, Modellist[mposition].Parameters, new List<double> { 0.0 }, Modellist[mposition].Variance, new List<double> { 0.0 }, Modellist[mposition].Sumsquaredresiduals, Modellist[mposition].Transformtype, Modellist[mposition].Transformshape, Modellist[mposition].Transformscale, i, 0, archparamest, archestandaic[i + 1], resid, acfresid, Modellist[mposition].Subtractmean));

            }

            //fit the GARCH models
            for (int i = 1; i < 3; i++)
                for (int j = 1; j < 3; j++)
                {
                    List<double> garchestandaic = P.garchML(data, Modellist[mposition].Parameters, nar, nma, i, j);
                    List<double> garchparamest = new List<double>();
                    for (int l = 0; l < i + j + 1; l++)
                        garchparamest.Add(garchestandaic[l]);
                    List<double> resid = P.garchresiduals(data, Modellist[mposition].Parameters, nar, nma, i, j, garchparamest);
                    List<double> acfresid = P.sampleautocorrelation(resid, 100);
                    if ((Math.Abs(garchestandaic[i + j + 1]) > 1000000) || (double.IsNaN(garchestandaic[i + j + 1])))
                        continue;
                    else
                        archModellist.Add(new model("ML", nar, nma, nd, Modellist[mposition].Aic, Modellist[mposition].Bic, Modellist[mposition].Parameters, new List<double> { 0.0 }, Modellist[mposition].Variance, new List<double> { 0.0 }, Modellist[mposition].Sumsquaredresiduals, Modellist[mposition].Transformtype, Modellist[mposition].Transformshape, Modellist[mposition].Transformscale, i, j, garchparamest, garchestandaic[i + j + 1], resid, acfresid, Modellist[mposition].Subtractmean));
                }

            ARCHmodelsdgv.Show();
            label3.Show();

            //populating the datagridview
            int x = ARCHmodelsdgv.ColumnCount;
            bs3.DataSource = archModellist;
            ARCHmodelsdgv.DataSource = bs3;
            ARCHmodelsdgv.AllowUserToAddRows = false;
            ARCHmodelsdgv.AutoGenerateColumns = false;
            if (x == 0)
            {
                DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
                col1.DataPropertyName = "archQ";
                col1.HeaderText = "GARCH (Q)";
                col1.Name = "Blah";
                col1.Width = 90;
                ARCHmodelsdgv.Columns.Add(col1);
                DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
                col2.DataPropertyName = "archP";
                col2.HeaderText = "GARCH (P)";
                col2.Name = "Blah";
                col2.Width = 90;
                ARCHmodelsdgv.Columns.Add(col2);
                DataGridViewColumn col3 = new DataGridViewTextBoxColumn();
                col3.DataPropertyName = "archAic";
                col3.HeaderText = "Arch AIC";
                col3.Name = "Blah";
                col3.Width = 90;
                col3.DefaultCellStyle.Format = "n2";
                ARCHmodelsdgv.Columns.Add(col3);
                ARCHmodelsdgv.Columns["P"].Visible = false;
                ARCHmodelsdgv.Columns["Q"].Visible = false;
                ARCHmodelsdgv.Columns["Aic"].Visible = false;
                ARCHmodelsdgv.Columns["Bic"].Visible = false;
                ARCHmodelsdgv.Columns["Type"].Visible = false;
                ARCHmodelsdgv.Columns["Variance"].Visible = false;
                ARCHmodelsdgv.Columns["D"].Visible = false;
                ARCHmodelsdgv.Columns["Sumsquaredresiduals"].Visible = false;
                ARCHmodelsdgv.Columns["Transformtype"].Visible = false;
                ARCHmodelsdgv.Columns["Transformshape"].Visible = false;
                ARCHmodelsdgv.Columns["Transformscale"].Visible = false;
                ARCHmodelsdgv.Columns["archP"].Visible = false;
                ARCHmodelsdgv.Columns["archQ"].Visible = false;
                ARCHmodelsdgv.Columns["archAic"].Visible = false;
                ARCHmodelsdgv.Columns["Subtractmean"].Visible = false;
            }
            this.Cursor = old;

        }

        //if the user clicks on one of the ARCH/GARCH models in the Fitted ARCH/GARCH models datagridview, this method is called
        //it will display the parameters from the ARCH/GARCH model and it will also give a diagnostic plot of the residuals
        private void ARCHmodelsdgv_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            ARCHarimadgv.Show();
            ARCHarchdgv.Show();
            label1.Show();
            label2.Show();
            ARCHwhitenoise.Show();
            ARCHsavebt.Show();

            //place holder to determine which row in the DGV has been selected
            int selectedrowindex = 0;
            if (this.ARCHmodelsdgv.SelectedCells.Count > 0)
                selectedrowindex = ARCHmodelsdgv.SelectedCells[0].RowIndex;

            //look at the acf of the theoretical white noise
            List<double> residsac = new List<double>();
            foreach (double dd in archModellist[selectedrowindex].archacfWhitenoise)
            {
                residsac.Add(dd);
            }
            residsac.RemoveAt(0);
            //we will use the following code to create the horizontal lines in the ACF and PACF plots
            int n = archModellist[selectedrowindex].archWhitenoise.Count();
            double l1 = 1.96 / Math.Sqrt(n);
            double l2 = -1.96 / Math.Sqrt(n);
            List<double> line1 = new List<double>();
            List<double> line2 = new List<double>();
            for (int i = 0; i < residsac.Count(); i++)
            {
                line1.Add(l1);
                line2.Add(l2);
            }
            ARCHwhitenoise.Titles.Clear();
            ARCHwhitenoise.Titles.Add(new Title("ACF of v(t) from ARCH/GARCH model", Docking.Top, new Font("Verdana", 8f, FontStyle.Bold), Color.Black));
            ARCHwhitenoise.ChartAreas[0].AxisY.LabelStyle.Font = ARCHwhitenoise.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);
            ARCHwhitenoise.ChartAreas[0].RecalculateAxesScale();
            ARCHwhitenoise.Series.Clear();
            var series10 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ACF Arch white noise",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            this.ARCHwhitenoise.Series.Add(series10);
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
            this.ARCHwhitenoise.Series.Add(series26);
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
            this.ARCHwhitenoise.Series.Add(series27);
            z = 0;
            foreach (double dd in line2)
            {
                series27.Points.AddXY(z + 1, line2[z]);
                z++;
            }
            ARCHwhitenoise.Invalidate();
            ARCHwhitenoise.ChartAreas[0].AxisY.Maximum = this.P.maximum(residsac);
            //populating the ARCGarimadgv with the parameter estimates
            int mposition = 0;
            mposition = ARCHdgv.SelectedRows[0].Index;
            ARCHarimadgv.Show();
            ARCHarimadgv.AllowUserToAddRows = false;
            ARCHarimadgv.AutoGenerateColumns = false;
            ARCHarimadgv.Rows.Clear();
            ARCHarimadgv.Columns.Clear();
            int nparams = Modellist[mposition].Parameters.Count();
            string[] vals = new string[nparams];
            for (int i = 0; i < nparams; i++)
            {
                vals[i] = Math.Round(Modellist[mposition].Parameters[i], 3).ToString();
            }
            int nar = Modellist[mposition].P;
            int nma = Modellist[mposition].Q;
            ARCHarimadgv.ColumnCount = nparams;
            ARCHarimadgv.RowCount = 1;
            for (int i = 0; i < nar; i++)
            {
                ARCHarimadgv.Columns[i].Name = "AR" + (i + 1).ToString();
            }
            for (int i = 0; i < nma; i++)
            {
                ARCHarimadgv.Columns[i + nar].Name = "MA" + (i + 1).ToString();
            }
            ARCHarimadgv.Rows[0].SetValues(vals);
            //populating the ARCGarchdgv with the parameter estimates
            ARCHarchdgv.Show();
            ARCHarchdgv.AllowUserToAddRows = false;
            ARCHarchdgv.AutoGenerateColumns = false;
            ARCHarchdgv.Rows.Clear();
            ARCHarchdgv.Columns.Clear();
            int narchparams = archModellist[selectedrowindex].archParameters.Count();
            string[] archvals = new string[narchparams];
            for (int i = 0; i < narchparams; i++)
            {
                archvals[i] = Math.Round(archModellist[selectedrowindex].archParameters[i], 3).ToString();
            }
            int archnar = archModellist[selectedrowindex].archP;
            int archnma = archModellist[selectedrowindex].archQ + 1;
            ARCHarchdgv.ColumnCount = narchparams;
            ARCHarchdgv.RowCount = 1;
            for (int i = 0; i < archnma; i++)
            {
                ARCHarchdgv.Columns[i].Name = "alpha" + (i).ToString();
            }

            for (int i = 0; i < archnar; i++)
            {
                ARCHarchdgv.Columns[i + archnma].Name = "beta" + (i + 1).ToString();
            }
            ARCHarchdgv.Rows[0].SetValues(archvals);

        }

        //this method is called if the user clicks on the Save ARCH models button
        private void ARCHsavebt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor old = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            int position = ARCHlb.SelectedIndex;
            string interim1 = Fnames[position].Replace(".xlsx", "");
            string interim2 = Tab[position].Replace("$", "");
            string interim3 = Flocations[position].Replace(fnames[position], interim1 + interim2 + "Models.csv");
            if (modellist.Count() > 0)
            {
                StreamWriter sw = new StreamWriter(interim3, false);
                sw.WriteLine("Transform\tShape\tScale\tModel\tP\tQ\tD\tAIC\tSS\tVariance\tParameters\tarchQ\tarchP\tarchAIC\tarchParameters\tSubtractmean");
                for (int i = 0; i < modellist.Count(); i++)
                {
                    string temp = "";
                    int noparams = modellist[i].Parameters.Count();
                    for (int j = 0; j < noparams - 1; j++)
                        temp = temp + modellist[i].Parameters[j].ToString() + " ";
                    //don't need a whitespace for the last value
                    temp = temp + modellist[i].Parameters[noparams - 1].ToString();
                    string archtemp = "";
                    int archnoparams = modellist[i].archParameters.Count();
                    for (int j = 0; j < archnoparams - 1; j++)
                        archtemp = archtemp + modellist[i].archParameters[j].ToString() + " ";
                    //no need for a white space for the last arch parameter
                    archtemp = archtemp + modellist[i].archParameters[archnoparams - 1].ToString();
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}", Modellist[i].Transformtype, Modellist[i].Transformshape, Modellist[i].Transformscale, Modellist[i].Type, Modellist[i].P, Modellist[i].Q, Modellist[i].D, Modellist[i].Aic, Modellist[i].Sumsquaredresiduals, Modellist[i].Variance, temp, Modellist[i].archQ, Modellist[i].archP, Modellist[i].archAic, archtemp, Modellist[i].Subtractmean);
                }
                for (int i = 0; i < archModellist.Count(); i++)
                {
                    string temp = "";
                    int noparams = archmodellist[i].Parameters.Count();
                    for (int j = 0; j < noparams - 1; j++)
                        temp = temp + archmodellist[i].Parameters[j].ToString() + " ";
                    //don't need a whitespace for the last value
                    temp = temp + archmodellist[i].Parameters[noparams - 1].ToString();
                    string archtemp = "";
                    int archnoparams = archmodellist[i].archParameters.Count();
                    for (int j = 0; j < archnoparams - 1; j++)
                        archtemp = archtemp + archmodellist[i].archParameters[j].ToString() + " ";
                    //no need for a white space for the last arch parameter
                    archtemp = archtemp + archmodellist[i].archParameters[archnoparams - 1].ToString();
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}", archModellist[i].Transformtype, archModellist[i].Transformshape, archModellist[i].Transformscale, archModellist[i].Type, archModellist[i].P, archModellist[i].Q, archModellist[i].D, archModellist[i].Aic, archModellist[i].Sumsquaredresiduals, archModellist[i].Variance, temp, archModellist[i].archQ, archModellist[i].archP, archModellist[i].archAic, archtemp, archModellist[i].Subtractmean);
                }
                sw.Close();

                this.Cursor = old;
            }
        }

        //this method hides buttons, plots etc.
        private void hide()
        {
            ARCHdgv.Hide();
            ARCHmodels.Hide();
            ARCHmodelsdgv.Hide();
            ARCHwhitenoise.Hide();
            ARCHarimadgv.Hide();
            ARCHarchdgv.Hide();
            ARCHsavebt.Hide();
            ARCHarimalbl.Hide();
            label1.Hide();
            label2.Hide();
            label3.Hide();

        }

        private void ARCH_Load(object sender, EventArgs e)
        {
            hide();
        }

    }
}




