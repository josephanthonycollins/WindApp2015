using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindApp2015
{
    //we will use delegates to implement functions
    //func1 will handle the 1 dimensional functions i.e. f(x) = 2+x
    public delegate double func1(double val);

    //we will use delegates to implement the functions
    //funcN will handle the multi-dimensional functions i.e. f(x,y) = x+y;
    public delegate double funcN(double[] arr);

    class Statistics
    {
        //this variable will contain data upon which the calculations, transformations etc will be called
        private List<Double> d = new List<double>();

        public List<Double> D
        {
            get { return d; }
            set { d = value; }
        }

        //attribute to hold the bins for creating a histogram
        private List<Double> histBins = new List<double>();

        public List<Double> HistBins
        {
            get { return histBins; }
            set { histBins = value; }
        }

        //attribute to hold the values for creating a histogram
        private List<Double> histValues = new List<double>();

        public List<Double> HistValues
        {
            get { return histValues; }
            set { histValues = value; }
        }

        //attribute to hold the bins for creating a histogram, used for the Weibull distribution
        private List<Double> wBins = new List<double>();

        public List<Double> WBins
        {
            get { return wBins; }
            set { wBins = value; }
        }

        //attribute to hold the values for creating a histogram, used for the Weibull distribution
        private List<Double> wValues = new List<double>();

        public List<Double> WValues
        {
            get { return wValues; }
            set { wValues = value; }
        }

        //this attribute will be populated by the histogramBins() method, it specifies the number of bins in the histogram
        private int noBins = 0;

        public int NoBins
        {
            get { return noBins; }
            set { noBins = value; }
        }

        //attribute to hold the bins for the transformed data, used in the creation of a histogram
        private List<Double> tBins = new List<double>();

        public List<Double> TBins
        {
            get { return tBins; }
            set { tBins = value; }
        }

        //attribute to hold the values for the transformed data, used in the creation of a histogram
        private List<Double> tValues = new List<double>();

        public List<Double> TValues
        {
            get { return tValues; }
            set { tValues = value; }
        }

        //weibull scale parameter, associated with the weibull_parameter_estimate() method
        private double wScale = 0;

        public double WScale
        {
            get { return wScale; }
            set { wScale = value; }
        }

        //weibull shape parameter, associated with the weibull_parameter_estimate() method
        private double wShape = 0;

        public double WShape
        {
            get { return wShape; }
            set { wShape = value; }
        }

        //this variable will contain the transformed data once the initial exploratory analysis has been performed
        private List<Double> transformedData = new List<double>();

        public List<Double> TransformedData
        {
            get { return transformedData; }
            set { transformedData = value; }
        }

        //this variable will contain the transformed and standarised data, see Section 3.1 in the accompanying manual
        private List<Double> standardisedtransformedData = new List<double>();

        public List<Double> StandardisedTransformedData
        {
            get { return standardisedtransformedData; }
            set { standardisedtransformedData = value; }
        }

        //variable indicating the order of differencing to be applied to the time series data
        private int diff = 0;

        public int Diff
        {
            get { return diff; }
            set { diff = value; }
        }

        //this variable will contain the differenced time series
        private List<Double> differencedData = new List<double>();

        public List<Double> DifferencedData
        {
            get { return differencedData; }
            set { differencedData = value; }
        }

        //if the data is transformed and then standarised, this variable will contain the means of each grouping
        private List<Double> stdMeans = new List<double>();

        public List<Double> StdMeans
        {
            get { return stdMeans; }
            set { stdMeans = value; }
        }

        //if the data is transformed and then standarised, this variable will contain the standard deviation of each grouping
        private List<Double> stdStdev = new List<double>();

        public List<Double> StdStdev
        {
            get { return stdStdev; }
            set { stdStdev = value; }
        }

        //number of rows in the transformed and standardised data
        private int n2 = 0;

        public int N2
        {
            get { return n2; }
            set { n2 = value; }
        }

        //number of columns in the transformed and standardised data
        private int m2 = 0;

        public int M2
        {
            get { return m2; }
            set { m2 = value; }
        }

        //null constructor
        public Statistics()
        {
        }

        //constructor
        public Statistics(List<double> data)
        {
            this.D = data;
        }

        //copy constructor
        public Statistics(Statistics stats)
        {
            this.D = stats.D;
        }

        //method to calculate the average
        public double avg(List<double> lst)
        {
            double a = 0;
            int s = 0;
            int counter = 0;
            double total = 0;
            s = lst.Count;
            for (counter = 0; counter < s; counter++)
            {
                total = total + lst[counter];
            }
            a = total / s;
            return a;
        }

        //method to calculate the median
        public double median(List<double> lst)
        {
            double a = 0;
            int s = 0;
            s = lst.Count;
            lst.Sort();
            if (s % 2 == 1)
                a = lst[((s + 1) / 2) - 1];
            else
                a = lst[(s / 2) - 1];
            return a;
        }


        //method to calculate the standard deviation
        public double stdev(List<double> lst)
        {
            double a = 0;
            double b = 0;
            int s = 0;
            int counter = 0;
            double total = 0;
            s = lst.Count;
            b = avg(lst);
            for (counter = 0; counter < s; counter++)
            {
                total = total + (lst[counter] - b) * (lst[counter] - b);
            }
            a = Math.Sqrt(total / (s - 1));
            return a;
        }

        //method to calculate the skewness
        public double skew(List<double> lst)
        {
            double a = 0;
            double b = 0;
            double c = 0;
            int s = 0;
            int counter = 0;
            double total = 0;
            s = lst.Count;
            b = avg(lst);
            c = stdev(lst);
            for (counter = 0; counter < s; counter++)
            {
                total = total + (lst[counter] - b) * (lst[counter] - b) * (lst[counter] - b);
            }
            a = total / (s * c * c * c);
            return a;
        }

        //method to calculate the kurtosis
        public double kurt(List<double> lst)
        {
            double a = 0;
            double b = 0;
            double c = 0;
            int s = 0;
            int counter = 0;
            double total = 0;
            s = lst.Count;
            b = avg(lst);
            c = stdev(lst);
            for (counter = 0; counter < s; counter++)
            {
                total = total + (lst[counter] - b) * (lst[counter] - b) * (lst[counter] - b) * (lst[counter] - b);
            }
            a = total / (s * c * c * c * c);
            return a;
        }

        //method to calculate the maximum
        public double maximum(List<double> lst)
        {
            double a = 0;
            int s = 0;
            int counter = 0;
            s = lst.Count;
            a = lst[0];
            for (counter = 1; counter < s; counter++)
            {
                if (a < lst[counter])
                    a = lst[counter];
            }
            return a;
        }

        //method to calculate the maximum, works with lists which have NaN values
        public double maximumExcludingNaN(List<double> lst)
        {
            double a = 0;
            int s = 0;
            int counter = 0;
            s = lst.Count;
            a = lst[0];
            for (counter = 1; counter < s; counter++)
            {
                if ((a < lst[counter]) || (double.IsNaN(a)))
                    a = lst[counter];
            }
            return a;
        }


        //method to calculate the minimum
        public double minimum(List<double> lst)
        {
            double a = 0;
            int s = 0;
            int counter = 0;
            s = lst.Count;
            a = lst[0];
            for (counter = 1; counter < s; counter++)
            {
                if (a > lst[counter])
                    a = lst[counter];
            }
            return a;
        }

        //method to determine the point which is closest to zero
        public double closestzero(List<double> lst)
        {
            double a = 0;
            int s = 0;
            int counter = 0;
            s = lst.Count;
            a = lst[0];
            for (counter = 1; counter < s; counter++)
            {
                if (Math.Abs(a) > Math.Abs(lst[counter]))
                    a = lst[counter];
            }
            return a;
        }

        //method to calculate the sum of the squared values of a list
        public double sumofsquares(List<double> lst)
        {
            double total = 0;
            int counter = 0;
            int s = lst.Count;
            for (counter = 0; counter < s; counter++)
                total = total + lst[counter] * lst[counter];
            return total;
        }

        //method to calculate the bins for any histogram
        //sets the attribute NoBins to 50
        public List<double> histogramBins(List<double> lst)
        {
            List<double> bins = new List<double>();
            //assuming that the number of bins is 40
            NoBins = 50;
            int counter = 0;
            for (counter = 0; counter < NoBins; counter++)
            {
                bins.Add(0);
            }
            //now we populate the HistValues list
            double ml = minimum(lst);
            double mu = maximum(lst);
            double bucketSize = (mu - ml) / NoBins;
            //now we populate the HistBins list
            for (counter = 0; counter < NoBins; counter++)
            {
                /******************************************I think this should be +ml to start the binning at the minimum value*****/
                bins[counter] = bucketSize * counter + ml;
            }
            return bins;
            //end of method
        }

        //method to calculate the frequency of the bins for any histogram
        public List<double> histogramValues(List<double> lst, List<double> bins)
        {
            List<double> values = new List<double>();
            int counter = 0;
            for (counter = 0; counter < NoBins; counter++)
            {
                values.Add(0);
            }
            //now we populate the HistValues list
            double ml = minimum(lst);
            double mu = maximum(lst);
            double bucketSize = (mu - ml) / NoBins;
            foreach (double v in lst)
            {
                int bucketIndex = 0;
                if (bucketSize > 0.0)
                {
                    bucketIndex = (int)((v - ml) / bucketSize);
                    if (bucketIndex == NoBins)
                    {
                        bucketIndex--;
                    }
                }
                values[bucketIndex]++;
            }
            return values;
            //end of method
        }

        //this method will estimate the optimum scale and shape parameters for a Weibull distribution
        //See Section 3.2 in the accompanying manual
        //init is the initial guess for the shape parameter, iterations is the number of iterations used in the Newton Raphson method to find the estimate of the shape and scale parameters
        public void weibull_parameter_estimate(List<double> lst, double init, int iterations)
        {
            int n = lst.Count();
            double[] matA = new double[n];
            double[] matB = new double[n];
            double[] matC = new double[n];
            double[] matD = new double[n];
            int i = 0, k = 0;
            n = lst.Count();
            for (i = 0; i < iterations; i++)
            {
                double s = 0, sp = 0, lobs = 0, f = 0, dfx = 0, dsp = 0, update = 0;
                int z = 0;
                foreach (double d in lst)
                {
                    matA[z] = Math.Pow(d, init);
                    s = s + matA[z];
                    z++;
                }
                z = 0;
                foreach (double e in lst)
                {
                    matB[z] = Math.Log(e);
                    lobs = lobs + matB[z];
                    z++;
                }
                z = 0;
                foreach (double a in matA)
                {
                    matC[z] = a * matB[z];
                    sp = sp + matC[z];
                    z++;
                }
                z = 0;
                foreach (double b in matB)
                {
                    matD[z] = b * b * matA[z];
                    dsp = dsp + matD[z];
                    z++;
                }

                f = (sp / s) - (1 / init) - (1 / (double)n) * lobs;
                dfx = 1 / (init * init) - (sp * sp) / (s * s) + dsp / s;
                update = init - f / dfx;
                init = update;
            }
            this.WShape = init;

            //we have the shape parameter, now to estimate the scale parameter
            double[] matE = new double[n];
            double tot = 0;
            k = 0;
            foreach (double a in lst)
            {
                matE[k] = Math.Pow(a, this.WShape);
                tot = tot + matE[k];
                k++;
            }
            double scale = 0;
            scale = tot / n;
            scale = Math.Pow(scale, 1 / this.WShape);
            this.WScale = scale;
        }

        //method is designed to come with a good initial approximation for the shape parameter of a weibull distribution
        //once a starting value is provided, the weibull_parameter_estimate() method can come up with a mroe refined estimate
        //See section 3.2 in the accompanying manual
        public double weibull_initial_guess(List<double> lst)
        {
            int n = lst.Count();
            double s = this.stdev(lst);
            double m = this.avg(lst);
            double val = s / m;
            val = Math.Pow(val, -1.086);
            return val;
            //end of weibull_initial_guess
        }

        //method to generate a weibull sample
        //See Section 3.3 in the accompanying manual
        public List<double> weibull_sample(double size, double shape, double scale)
        {
            List<double> wsample = new List<double>();
            Random r = new Random();
            double y = 0, x = 0, w = 0, val = 0;
            int counter = 0;
            for (counter = 0; counter < (int)size; counter++)
            {
                y = r.NextDouble();
                x = -Math.Log(y);
                w = Math.Pow(x, 1 / shape);
                val = w * scale;
                wsample.Add(val);
            }
            return wsample;
        }

        //method to generate a normal sample
        //See Section 3.3 in the accompanying manual       
        public List<double> normal_sample(double size, double m, double s)
        {
            List<double> nsample = new List<double>();
            int seed = (int)DateTime.Now.Ticks;
            Random ran = new Random(seed);
            double r = 0, u1 = 0, u2 = 0, theta = 0, val = 0;
            int counter = 0;
            for (counter = 0; counter < (int)size; counter++)
            {
                u1 = ran.NextDouble();
                u2 = ran.NextDouble();
                r = Math.Sqrt(-2 * Math.Log(u1));
                theta = 2 * Math.PI * u2;
                val = m + s * r * Math.Sin(theta);
                nsample.Add(val);
            }
            return nsample;
        }

        //method to determine via an iterative procedure what is the optimum transformation
        //for the underlying data, see Section 3.1 in the accompanying manual
        public List<double> iterative_measurement(List<double> steps, List<double> lst)
        {
            List<double> tempA = new List<double>();
            List<double> tempB = new List<double>();
            double mean = 0, med = 0, sd = 0, d = 0, val = 0;
            int counter = 0, size = 0;
            size = steps.Count();
            for (counter = 0; counter < size; counter++)
            {
                d = steps[counter];
                if (tempA.Count() > 0)
                    tempA.Clear();
                foreach (double b in lst)
                {
                    tempA.Add(Math.Pow(b, d));
                }
                mean = this.avg(tempA);
                med = this.median(tempA);
                sd = this.stdev(tempA);
                val = (mean - med) / sd;
                tempB.Add(val);
            }
            return tempB;
        }

        //this generates a list from min to max in steps of (max-min)/intervals, note that 0 is dropped from the list
        public List<double> gen_list(double min, double max, double no_intervals)
        {
            List<double> tempA = new List<double>();
            int counter = 0;
            double d = (max - min) / no_intervals;
            for (counter = 0; counter <= no_intervals; counter++)
            {
                tempA.Add(d * counter + min);
            }
            //if the list contains the value of 0, then drop it
            tempA.RemoveAll(item => item == 0.0);
            return tempA;
        }

        //method to generate the pdf of a normal distribution given the mean, standard deviation and x vals
        public List<double> normal_pdf(double mean, double sd, List<double> xvalues)
        {
            List<double> probs = new List<double>();
            double r = 1 / (sd * Math.Sqrt(2.0 * Math.PI));
            double theta = 0;
            double val = 0;
            foreach (double inc in xvalues)
            {
                theta = -(inc - mean) * (inc - mean) / (2 * sd * sd);
                val = r * Math.Exp(theta);
                probs.Add(val);
            }
            return probs;
            //end of normal_pdf () method
        }

        //method to generate the pdf of a weibull distribution given the shape, scale and x vals
        public List<double> weibull_pdf(double shape, double scale, List<double> xvalues)
        {
            List<double> probs = new List<double>();
            double r = 0;
            double theta = 0;
            double val = 0;
            foreach (double inc in xvalues)
            {
                r = (shape / scale) * (Math.Pow((inc / scale), (shape - 1)));
                theta = Math.Exp(-Math.Pow((inc / scale), shape));
                val = r * theta;
                probs.Add(val);
            }
            return probs;
            //end of weibull_pdf () method
        }

        //pdf given a set of data and x vals
        public List<double> pdf(List<double> sample, List<double> xvalues)
        {
            List<double> probs = histogramValues(sample, xvalues);
            double obs = sample.Count();
            int z = 0;
            for (z = 0; z < xvalues.Count(); z++)
            {
                probs[z] = probs[z] / obs;
            }
            return probs;
            //end of pdf () method
        }

        //method to standarise data
        //See Section 3.1 in the accompanying manual
        //(n1,m1) are the dimensions of the dataset, (n2,m2) is how the data is to be standardised. Again, see Section 3.1 in the manual 
        public void standardise(List<double> sample, double n1, double m1, double n2, double m2)
        {
            this.M2 = (int)m2;
            this.N2 = (int)n2;
            //clear any values that may be in the StandardisedTransformedData list
            if (StandardisedTransformedData.Count() > 0)
                StandardisedTransformedData.Clear();

            List<List<double>> blocks = new List<List<double>>();
            List<double> standardiseddata = new List<double>();
            int i = 0, k = 0, j = 0, l = 0;

            //The following helps to create subblocks on which we calculate the means and standarddeviations
            double index;
            for (k = 0; k < (int)(n1 - (n1 % n2)) / n2; k++)
            {

                for (j = 1; j <= (int)m1 / m2; j++)
                {
                    List<double> subblock = new List<double>();
                    for (i = 1; i <= n2; i++)
                    {
                        //we do this to replicate the Range method in Mathematica
                        for (l = 1; l <= m2; l++)
                        {
                            index = ((j - 1) * m2 + l) + (i - 1) * (m1);
                            index = index + k * n2 * m1;
                            subblock.Add(sample[(int)(index - 1)]);
                            //end of l loop
                        }
                        //end of i loop
                    }
                    blocks.Add(subblock);
                    //end of j loop
                }
                //end of k loop
            }

            //at this stage we have the data split into blocks, now we need to calculate the mean of and standard deviation of each block
            int size = blocks.Count();
            List<double> means = new List<double>();
            List<double> stdeviations = new List<double>();
            foreach (List<double> dd in blocks)
            {
                double a = avg(dd);
                double s = stdev(dd);
                means.Add(a);
                stdeviations.Add(s);
            }
            //now we populate the list so that the info will be available elsewhere
            if (StdMeans.Count() > 0)
            {
                StdMeans.Clear();
                StdStdev.Clear();
            }
            foreach (double qr in means)
                StdMeans.Add(qr);
            foreach (double qs in stdeviations)
                StdStdev.Add(qs);


            //now we want to take each observation and subtract the appropriate mean and divide the result by the appropriate standard deviation
            blocks.Clear();
            List<double> temp = new List<double>();
            foreach (double dd in sample)
                temp.Add(dd);
            for (k = 0; k < (int)(n1 - (n1 % n2)) / n2; k++)
            {
                for (j = 1; j <= (int)m1 / m2; j++)
                {
                    List<double> subblock = new List<double>();
                    for (i = 1; i <= n2; i++)
                    {
                        for (l = 1; l <= m2; l++)
                        {
                            index = ((j - 1) * m2 + l) + (i - 1) * (m1);
                            index = index + k * n2 * m1;
                            subblock.Add(sample[(int)(index - 1)]);
                            //end of l loop
                        }
                        //end of i loop
                    }
                    blocks.Add(subblock);
                    double me = avg(subblock);
                    double sd = stdev(subblock);
                    for (i = 1; i <= n2; i++)
                    {
                        for (l = 1; l <= m2; l++)
                        {
                            index = ((j - 1) * m2 + l) + (i - 1) * (m1);
                            index = index + k * n2 * m1;
                            temp[(int)(index - 1)] = (sample[(int)(index - 1)] - me) / sd;
                            //end of l loop
                        }
                        //end of i loop
                    }

                }
                //end of k loop
            }

            //now add the transformed data to the StandardisedTransformedData list which will store the results
            int counter = (int)StdMeans.Count() * (int)n2 * (int)m2;
            for (int g = 0; g < counter; g++)
                StandardisedTransformedData.Add(temp[g]);
        }

        //method to calculate the sample autocovariance out to lag n
        public List<double> sampleautocovariance(List<double> sample, int lag)
        {
            List<double> sc = new List<double>();
            int size = sample.Count();
            double total = 0;
            double mean = avg(sample);

            //get autocovariance at lag 0
            for (int i = 0; i < size; i++)
            {
                total = total + (sample[i] - mean) * (sample[i] - mean);

            }
            double ac0 = total / size;
            sc.Add(ac0);

            //get autocovariance at lags>0
            for (int i = 1; i <= lag; i++)
            {
                total = 0;
                //get autocovariance at lag 0
                for (int j = 0; j < size - i; j++)
                {
                    total = total + (sample[j + i] - mean) * (sample[j] - mean);
                }
                double ac = total / size;
                sc.Add(ac);
            }

            return sc;

            //end of sampleautocovariance() method
        }

        //method to calculate the sample autocovariance out to lag n
        public List<double> sampleautocorrelation(List<double> sample, int lag)
        {
            List<double> sa = new List<double>();
            List<double> sc = sampleautocovariance(sample, lag);
            double autoc = 0;
            //get autocovariance at lag 0
            for (int i = 0; i <= lag; i++)
            {
                autoc = sc[i] / sc[0];
                sa.Add(autoc);

            }
            return sa;
        }

        //method to calculate the sample partialautocorrelation out to lag n
        //this is based on Durbin relations, see Equation 4.6 in the accompanying manual
        public List<double> samplepartialautocorrelation(List<double> sample, int lag)
        {
            List<double> sac = sampleautocorrelation(sample, lag + 1);
            List<double> spac = new List<double>();
            double[,] mat = new double[lag + 1, lag + 1];
            //PACF1=ACF1
            mat[1, 1] = sac[1];

            //PACF0=1
            spac.Add(1);

            for (int i = 2; i < lag + 1; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    double total = 0;
                    if (j == i)
                    {
                        for (int k = 1; k <= i - 1; k++)
                            total = total + mat[i - 1, k] * sac[i - k];
                        mat[i, i] = (sac[i] - total) / (1 - total);
                    }
                    else
                        mat[i, j] = mat[i - 1, j] - mat[i, i] * mat[i - 1, i - j];
                }
            }

            for (int i = 1; i <= lag; i++)
                spac.Add(mat[i, i]);
            return spac;
        }

        //method to estimate the AR coefficients via Yule Walker
        //See Section 4.2.1 in the accompanying manual
        public List<double> yulewalker(List<double> sample, int lag)
        {
            List<double> coeff = new List<double>();
            List<double> scov = sampleautocorrelation(sample, lag + 1);
            //We need to initialise the matrix A and the vector b which will be passed to the GElim object
            double[,] A = new double[lag, lag];
            double[] b = new double[lag];
            for (int i = 0; i < lag; i++)
            {
                b[i] = scov[i + 1];
                for (int j = 0; j < lag; j++)
                {
                    A[i, j] = scov[Math.Abs(i - j)];
                }
            }
            //Gaussian Elimination, Ax=b, we want to solve for x
            GElim equation = new GElim(A, b, 2);
            equation.solve();
            double[] sol = new double[lag];
            for (int i = 0; i < lag; i++)
                coeff.Add(equation.Solution[i]);
            return coeff;
        }

        //method to estimate the residuals associated with the yulewalker() method
        //Note: if the model is AR(p), then first p residuals are assumed to be 0
        //here yule are the parameter estimates and sample is the time series
        public List<double> yulewalkerresiduals(List<double> sample, List<double> yule)
        {
            List<double> residuals = new List<double>();
            int arsize = yule.Count();
            int samplesize = sample.Count();
            int i = 0, k = 0;

            //for the first p observations, the residuals is assumed to be 0
            for (k = 0; k <= arsize - 1; k++)
                residuals.Add(0);

            for (i = arsize; i <= samplesize - 1; i++)
            {
                double sum = 0;
                for (k = 0; k <= arsize - 1; k++)
                {
                    sum = sum + yule[k] * sample[i - 1 - k];

                }
                double r = sample[i] - sum;
                residuals.Add(r);
            }

            return residuals;
        }

        //method to estimate the sigma hat squared value using the Yule Walker equations
        //See Equation 6.2 in Mathematica Time Series
        //here yule are the parameter estimates and sample is the time series
        public double yulewalkerVariance(List<double> sample, List<double> yule)
        {
            int lag = yule.Count;
            double variance = 0;
            double total = 0;
            int q = 0;
            List<double> scov = sampleautocovariance(sample, lag + 1);
            for (q = 0; q < lag; q++)
            {
                total = yule[q] * scov[q + 1];

            }
            variance = scov[0] - total;
            return variance;
            //end of method
        }

        //method to difference the time series
        public void difference()
        {
            List<List<double>> blocks = new List<List<double>>();
            List<double> subblock = new List<double>();

            //the first block is just the original time series data
            for (int i = 0; i < D.Count(); i++)
                subblock.Add(D[i]);
            blocks.Add(subblock);

            //calculate the differenced data
            for (int i = 1; i <= Diff; i++)
            {
                List<double> t = new List<double>();
                for (int j = 1; j < blocks[i - 1].Count(); j++)
                {
                    t.Add(blocks[i - 1][j] - blocks[i - 1][j - 1]);
                }
                blocks.Add(t);
            }

            //now add the differenced data to the differencedData object
            for (int i = 0; i < blocks[Diff].Count(); i++)
                differencedData.Add(blocks[Diff][i]);
        }

        //least squares method
        // returns betahat = (Xtranspose*X)^-1 * Xtranspose*y
        public double[] leastsquares(double[,] explanatoryvariables, double[] observations)
        {
            system_solver s = new system_solver();
            Vector Vy = new Vector(observations);
            Matrix MX = new Matrix(explanatoryvariables);
            Matrix MXtranspose = MX.TransposeMatrix();
            double[,] XtransposeX = (double[,])(MXtranspose * MX);
            double[,] XtranposeXinv = s.find_inverse(XtransposeX, XtransposeX.GetLength(0));
            Vector VXtransposey = MXtranspose * Vy;
            Matrix MXtranposeXinv = new Matrix(XtranposeXinv);
            double[] betahat = (double[])(MXtranposeXinv * VXtransposey);
            return betahat;
        }

        //method to implement the Hannan Rissanen algorithm
        //p and q are the values in the ARMA(p,q) model, m is the order of the regression
        //See Section 4.2.3 in the accompanying manual for details on m
        public List<double> HannanRissanen(int p, int q, int m)
        {
            //variable to hold the ARMA parameter estimates
            List<double> parameters = new List<double>();

            //variable to hold the data to which we wish to fit the ARMA model
            List<double> data = new List<double>();
            if (Diff == 0)
            {
                for (int i = 0; i < D.Count(); i++)
                    data.Add(D[i]);
            }
            else
            {
                for (int i = 0; i < differencedData.Count(); i++)
                    data.Add(differencedData[i]);
            }

            //now the first step is to fit an AR model of order m to the data
            List<double> arstep1 = yulewalker(data, m);
            List<double> arstep1residuals = yulewalkerresiduals(data, arstep1);

            //next we need to create the matrix comprised of X(t) and Z(t) values upon which we will perform least squares regression
            int cols = p + q;
            int rows = data.Count() - (m + Math.Max(p, q));
            double[,] step2data = new double[rows, cols];
            //first lets populate the X(t) data
            for (int i = m + Math.Max(p, q); i < data.Count(); i++)
            {
                for (int j = 1; j <= p; j++)
                {
                    step2data[i - (m + Math.Max(p, q)), j - 1] = data[i - j];
                }
            }
            //next lets populate the Z(t) data
            for (int i = m + Math.Max(p, q); i < data.Count(); i++)
            {
                for (int j = 1; j <= q; j++)
                {
                    step2data[i - (m + Math.Max(p, q)), p + j - 1] = arstep1residuals[i - j];
                }
            }
            double[] obs = new double[rows];
            //now lets populate the obs array i.e. we will use this in the least squares regression as the dependent variable
            for (int i = 0; i < rows; i++)
            {
                obs[i] = data[m + i];
                //??should the m here be m+Math.Max(p,q)

            }

            //now we have y = Xb hence do least squares regression
            //Here X is "step2data", y is "obs"
            double[] estimates = leastsquares(step2data, obs);
            for (int i = 0; i < estimates.GetLength(0); i++)
                parameters.Add(estimates[i]);
            return parameters;
        }

        //method to estimate the residuals from an ARMA(p,q) model
        //Note: if the model is ARMA(p,q), then first Max(p,q) residuals are assumed to be 0
        //sample is the timeseries, hrestimates are the parameter estimates, p and q are the AR and MA orders
        public List<double> ARMAresiduals(List<double> sample, List<double> hrestimates, int p, int q)
        {
            List<double> residuals = new List<double>();
            int arsize = p;
            int masize = q;
            int samplesize = sample.Count();
            int i = 0, k = 0;

            //for the first Max(p,q) observations, the residual is assumed to be 0
            for (k = 0; k <= Math.Max(arsize, masize) - 1; k++)
                residuals.Add(0);
            for (i = Math.Max(arsize, masize); i <= samplesize - 1; i++)
            {
                //calculate the AR components
                double sumAR = 0;
                for (k = 0; k <= arsize - 1; k++)
                {
                    sumAR = sumAR + hrestimates[k] * sample[i - 1 - k];
                }
                //calculate the MA components
                double sumMA = 0;
                for (k = 0; k <= masize - 1; k++)
                {
                    sumMA = sumMA + hrestimates[arsize + k] * residuals[i - 1 - k];
                }
                double r = sample[i] - (sumAR + sumMA);
                residuals.Add(r);
            }

            return residuals;
        }

        //method to calculate the noise variance associated with the Hannan Rissanen algorithm
        //residuals are the residuals from the Hannan Rissanen algorithm
        //Equation 6.4 in Mathematica time series documentation
        public double HannanRissanenVariance(List<double> residuals, int p, int q)
        {
            int arsize = p;
            int masize = q;
            int samplesize = residuals.Count();
            int t = Math.Max(p, q);
            double variance = 0;
            double total = 0;
            for (int i = t; i < samplesize; i++)
                total = total + residuals[i] * residuals[i];
            variance = total / (samplesize - t);
            return variance;
            //end of method
        }

        //method to calculate the MA coefficients and estimate of sigma hat squared using the Innovations Algorithm
        //See section 4.2.2 in the accompanying manual, this method is an implementation of Equation 4.10
        public List<double> innovations(List<double> sample, int q, int iter)
        {
            List<double> sacv = sampleautocovariance(sample, iter);
            List<double> results = new List<double>();
            double[,] theta = new double[iter + 1, iter + 1];
            double[,] P = new double[iter + 1, iter + 1];
            //P[0,0]=Auctocovariance at lag 0
            P[0, 0] = sacv[0];

            //populate the diagonals of the theta matrix
            for (int n = 1; n < iter; n++)
            {
                theta[n, n] = sacv[n] / P[0, 0];
            }

            //theta[1,1] has been calculated so now calculate P[1,1]
            P[1, 1] = sacv[0] - theta[1, 1] * theta[1, 1] * P[0, 0];

            //populate the remainder of the theta and the P matrices
            for (int n = 2; n < iter; n++)
            {
                double sum = 0;
                //calculate the remaining theta's, we don't need to caclulate
                //the diagonal theta as this has already beendone
                for (int i = 1; i < n; i++)
                {
                    sum = 0;
                    for (int j = 0; j <= i - 1; j++)
                    {
                        sum = sum + theta[i, i - j] * theta[n, n - j] * P[j, j];
                    }
                    theta[n, n - i] = (sacv[n - i] - sum) / P[i, i];
                }
                //theta[1,1] has been calculated so now calculate P[1,1]
                sum = 0;
                for (int i = 0; i <= n - 1; i++)
                {
                    sum = sum + theta[n, n - i] * theta[n, n - i] * P[i, i];
                }
                P[n, n] = sacv[0] - sum;
            }

            //now the theta Matrix has been determined, we need to check where it stabilises and then return the relevant row
            double[] currentrow = new double[q + 1];
            double[] previousrow = new double[q + 1];
            double[] ratio = new double[q];
            for (int i = q + 2; i < iter; i++)
            {
                double sum = 0;
                for (int j = 0; j < q; j++)
                {
                    currentrow[j] = theta[i, j + 1];
                    previousrow[j] = theta[i - 1, j + 1];
                    ratio[j] = Math.Abs(1 - theta[i - 1, j + 1] / theta[i, j + 1]);
                    sum = sum + ratio[j];
                }
                currentrow[q] = P[i, i];
                previousrow[q] = P[i - 1, i - 1];
                if (sum < q * 0.1)
                {
                    List<double> ans = new List<double>();
                    for (int u = 0; u <= q; u++)
                        ans.Add(previousrow[u]);
                    return ans;
                }
            }

            //it hasn't converged by this stage, hence we return dummy values
            List<double> dummy = new List<double>();
            for (int k = 0; k <= q; k++)
                dummy.Add(-1000.0);
            return dummy;
            //end of method
        }

        //method to estimate the residuals from the innovations algorithm
        //Note: if the model is  MA(q), then first q residuals are assumed to be 0
        //this method is simply a copy of the MAMLresiduals() method
        public List<double> innovationsresiduals(List<double> sample, List<double> estimates)
        {
            List<double> residuals = new List<double>();
            int masize = estimates.Count();
            int samplesize = sample.Count();
            int i = 0, k = 0;

            //for the first p observations, the residuals is assumed to be 0
            for (k = 0; k <= masize - 1; k++)
                residuals.Add(0);
            for (i = masize; i <= samplesize - 1; i++)
            {
                double sum = 0;
                for (k = 0; k <= masize - 1; k++)
                {
                    sum = sum + estimates[k] * residuals[i - 1 - k];
                }
                double r = sample[i] - sum;
                residuals.Add(r);
            }
            return residuals;
        }

        //Method to find the AR parameter estimates using Maximum Likelihood and Newton Raphson
        //See section 4.2.4 in the accompanying manual
        public List<double> ARML(List<double> sample, int lag)
        {
            double[] ans = new double[3];
            double[] initial = new double[lag];
            double[] NRoot = new double[lag];
            FunctionMatrix F = new FunctionMatrix(lag, 1);

            //apply yulewalker() to get a good starting guess for the parameters
            List<double> startguess = this.yulewalker(sample, lag);
            for (int i = 0; i < lag; i++)
                initial[i] = startguess[i];

            //now we need to create a list of the functions which need to be solved
            List<Likelihood> lList = new List<Likelihood>();
            for (int i = 0; i < lag; i++)
            {
                if (this.Diff == 0)
                    lList.Add(new Likelihood(this.D, (int)i + 1));
                else if (this.Diff > 0)
                    lList.Add(new Likelihood(this.DifferencedData, (int)i + 1));
            }
            //next initial the function list
            for (int i = 0; i < lag; i++)
                F[i, 0] = lList[i].ARScorefunction;

            //create an instance of the DR class, this will be used to find the roots of the system
            DR drobj = new DR();
            double[] NRroot = drobj.newtonraphson(F, initial, 0.01);
            int l = NRoot.Length;
            List<double> result = new List<double>();
            for (int i = 0; i < l; i++)
                result.Add(-NRroot[i]);
            return result;
            //end of method
        }

        //method to estimate the residuals from the AR Conditional Maximum Likelihood
        //Note: if the model is AR(p), then first p residuals are assumed to be 0
        public List<double> ARMLresiduals(List<double> sample, List<double> ARML)
        {
            List<double> residuals = new List<double>();
            int arsize = ARML.Count();
            int samplesize = sample.Count();
            int i = 0, k = 0;
            //for the first p observations, the residuals is assumed to be 0
            for (k = 0; k <= arsize - 1; k++)
                residuals.Add(0);
            for (i = arsize; i <= samplesize - 1; i++)
            {
                double sum = 0;
                for (k = 0; k <= arsize - 1; k++)
                {
                    sum = sum + ARML[k] * sample[i - 1 - k];

                }
                double r = sample[i] - sum;
                residuals.Add(r);
            }
            return residuals;
        }

        //method to estimate the variance from the AR Conditional Maximum Likelihood
        //Page 122/123 of J Hamilton "Time Series Analysis"
        public double ARMLVariance(List<double> residuals, int p)
        {
            int t = p;
            int samplesize = residuals.Count();
            double variance = 0;
            double total = 0;
            for (int i = t; i < samplesize; i++)
                total = total + residuals[i] * residuals[i];
            variance = total / (samplesize - t);
            return variance;
            //end of method
        }

        //recursive method to help generate a grid which will be used by the ARMAMLAlternative and MAMLAlternative methods
        //See Section 4.2.5 in the accompanying manual
        public Matrix grid(int n)
        {
            if (n == 1)
            {
                Matrix mat = new Matrix(3, 1);
                mat[0, 0] = -1;
                mat[1, 0] = 0;
                mat[2, 0] = 1;
                return mat;
            }

            //for n = 2 just populate the matrix
            if (n == 2)
            {
                Matrix mat = new Matrix(9, 2);
                mat[0, 0] = -1;
                mat[1, 0] = -1;
                mat[2, 0] = -1;
                mat[3, 0] = 0;
                mat[4, 0] = 0;
                mat[5, 0] = 0;
                mat[6, 0] = 1;
                mat[7, 0] = 1;
                mat[8, 0] = 1;

                mat[0, 1] = -1;
                mat[1, 1] = 0;
                mat[2, 1] = 1;
                mat[3, 1] = -1;
                mat[4, 1] = 0;
                mat[5, 1] = 1;
                mat[6, 1] = -1;
                mat[7, 1] = 0;
                mat[8, 1] = 1;

                return mat;
            }
            else
            {
                //what is the gird from the previous step?
                Matrix temp = grid(n - 1);
                //we want to take the grid from the previous step and add in an extra column
                Matrix mat = new Matrix(temp.Rows * 3, temp.Cols + 1);
                //first block
                for (int i = 0; i < temp.Rows; i++)
                    for (int j = 0; j < temp.Cols; j++)
                        mat[i, j + 1] = temp[i, j];
                //second block
                for (int i = 0; i < temp.Rows; i++)
                    for (int j = 0; j < temp.Cols; j++)
                        mat[i + temp.Rows, j + 1] = temp[i, j];
                //third block
                for (int i = 0; i < temp.Rows; i++)
                    for (int j = 0; j < temp.Cols; j++)
                        mat[i + 2 * temp.Rows, j + 1] = temp[i, j];
                //now populate the first column
                for (double i = 0; i < temp.Rows * 3; i++)
                {
                    int a = (int)Math.Floor(i / temp.Rows);
                    if (a == 0)
                        mat[(int)i, 0] = -1;
                    if (a == 1)
                        mat[(int)i, 0] = 0;
                    if (a == 2)
                        mat[(int)i, 0] = 1;
                }
                return mat;
            }
        }

        //Method to find the MA parameter estimates using Maximum Likelihood
        //See Section 4.2.5 in the accompanying manual
        public List<double> MAML(List<double> sample, int lag)
        {
            double[] initial = new double[lag];
            double error = 1000;
            double stepsize = 0.005;
            int counter = 0;
            double oldll = 0;

            //fit HR to get a good starting guess for the model
            List<double> startguess = this.HannanRissanen(0, lag, 40);
            for (int i = 0; i < lag; i++)
                initial[i] = startguess[i];

            oldll = -this.sumofsquares((this.ARMAresiduals(sample, startguess, 0, lag)));
            //we will use the following to help iterate around the grid
            Matrix g = this.grid(lag);
            Matrix step = stepsize * g;

            while ((error > 0.01) && (counter < 500))
            {
                //create a list of the likelihood values
                List<double> likelihood = new List<double>();
                //create a list of the theta grid values
                List<List<double>> theta = new List<List<double>>();

                Double[] next = new double[lag];

                for (int i = 0; i < step.Rows; i++)
                {
                    for (int j = 0; j < step.Cols; j++)
                    {
                        next[j] = step[i, j] + initial[j];
                    }

                    //we only do this step as the HannanRissanresiduals function needs a List<double> as input
                    List<double> hrestimates = new List<double>();
                    for (int k = 0; k < next.Length; k++)
                        hrestimates.Add(next[k]);

                    //calculate the log likelihood associated with this value
                    double ll = -this.sumofsquares(this.ARMAresiduals(sample, hrestimates, 0, lag));
                    //add it to the list
                    likelihood.Add(ll);
                    theta.Add(hrestimates);
                }
                //what is the maximum likelihood value
                double max = this.maximum(likelihood);
                //what is its index
                int ind = likelihood.FindIndex(item => item == max);

                //how does this compare to the old loglike value
                error = Math.Abs(oldll - max);
                counter++;
                //reassign variables before the next iteration
                oldll = max;
                for (int j = 0; j < step.Cols; j++)
                {
                    initial[j] = theta[ind][j];
                }
                //end of while loop
            }

            List<double> final = new List<double>();

            //if the error > 0.01 or it has not converged i.e. number of steps > 500, return -1000 for each answer
            if ((error > 0.01) || (counter > 500))
            {
                for (int i = 0; i < lag; i++)
                    final.Add(-1000);
            }
            //otherwise return the last estimate
            else
            {
                for (int i = 0; i < lag; i++)
                    final.Add(initial[i]);
            }

            return final;
            //end of method
        }

        //method to estimate the residuals from the MA Conditional Maximum Likelihood
        //Note: if the model is  MA(q), then first q residuals are assumed to be 0
        public List<double> MAMLresiduals(List<double> sample, List<double> MAML)
        {
            List<double> residuals = new List<double>();
            int masize = MAML.Count();
            int samplesize = sample.Count();
            int i = 0, k = 0;

            //for the first p observations, the residuals is assumed to be 0
            for (k = 0; k <= masize - 1; k++)
                residuals.Add(0);

            for (i = masize; i <= samplesize - 1; i++)
            {
                double sum = 0;
                for (k = 0; k <= masize - 1; k++)
                {
                    sum = sum + MAML[k] * residuals[i - 1 - k];
                }
                double r = sample[i] - sum;
                residuals.Add(r);
            }

            return residuals;
        }

        //method to estimate the variance from the MA Conditional Maximum Likelihood
        public double MAMLVariance(List<double> residuals, int q)
        {
            int t = q;
            int samplesize = residuals.Count();
            double variance = 0;
            double total = 0;
            for (int i = t; i < samplesize; i++)
                total = total + residuals[i] * residuals[i];
            variance = total / (samplesize - t);
            return variance;
            //end of method
        }

        //Method to find the MA parameter estimates using Maximum Likelihood
        //unlike the other method, we'll also endeavour to estimate sigma
        //See Section 4.2.5 in the accompanying manual
        public List<double> MAMLalternative(List<double> sample, int lag)
        {
            double[] initial = new double[lag + 1];
            double error = 1000;
            double stepsize = 0.01;
            int counter = 0;
            double oldll = 0;
            double sigmaguess = 0;
            List<double> startguess = this.innovations(sample, lag, 50);
            for (int i = 0; i < lag; i++)
                initial[i] = startguess[i];
            List<double> initresiduals = this.ARMAresiduals(sample, startguess, 0, lag);
            sigmaguess = startguess[lag];
            initial[lag] = sigmaguess;
            List<double> parameters = new List<double>();
            for (int i = 0; i < lag; i++)
                parameters.Add(initial[i]);
            oldll = -(sample.Count() / 2) * Math.Log(2 * Math.PI) - (sample.Count() / 2) * Math.Log(initial[lag]) - this.sumofsquares((this.ARMAresiduals(sample, parameters, 0, lag))) / (2 * initial[lag]);
            //we will use the following to help iterate around the grid
            Matrix g = this.grid(lag + 1);
            Matrix step = stepsize * g;

            while ((error > 0.01) && (counter < 1000))
            {
                //create a list of the likelihood values
                List<double> likelihood = new List<double>();
                //create a list of the theta grid values
                List<List<double>> theta = new List<List<double>>();
                Double[] next = new double[lag + 1];
                for (int i = 0; i < step.Rows; i++)
                {
                    for (int j = 0; j < step.Cols; j++)
                    {
                        next[j] = step[i, j] + initial[j];
                    }

                    //we only do this step as the ARMAresiduals() method needs a List<double> as input
                    List<double> hrestimates = new List<double>();
                    for (int k = 0; k < next.Length; k++)
                        hrestimates.Add(next[k]);

                    List<double> temp = new List<double>();
                    for (int p = 0; p < lag; p++)
                        temp.Add(hrestimates[p]);

                    //calculate the log likelihood associated with this value
                    double ll = -(sample.Count() / 2) * Math.Log(2 * Math.PI) - (sample.Count() / 2) * Math.Log(hrestimates[lag]) - this.sumofsquares((this.ARMAresiduals(sample, temp, 0, lag))) / (2 * hrestimates[lag]);

                    //add it to the list, but ensure sigma > 0
                    if (hrestimates[lag] < 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        likelihood.Add(ll);
                        theta.Add(hrestimates);
                    }

                }
                //what is the maximum likelihood value
                double max = this.maximumExcludingNaN(likelihood);
                //what is its index
                int ind = likelihood.FindIndex(item => item == max);
                if (ind == -1)
                {
                    counter = 1000;
                    error = 1000;
                    break;
                }
                //how does this compare to the old loglike value
                error = Math.Abs(oldll - max);
                counter++;
                //reassign variables before the next iteration
                oldll = max;
                for (int j = 0; j < step.Cols; j++)
                {
                    initial[j] = theta[ind][j];
                }
                //end of while loop
            }

            List<double> final = new List<double>();

            //if the error > 0.01 or it has not converged i.e. number of steps > 500, return -1000 for each answer
            if ((error > 0.01) || (counter > 1000))
            {
                for (int i = 0; i < lag + 1; i++)
                    final.Add(-1000);
            }
            //otherwise return the last estimate
            else
            {
                for (int i = 0; i < lag + 1; i++)
                    final.Add(initial[i]);
            }

            return final;
        }

        //Method to find the ARMA parameter estimates using Maximum Likelihood
        //See Section 4.2.5 in the accompanying manual
        public List<double> ARMAML(List<double> sample, int p, int q)
        {
            double[] initial = new double[p + q];
            double error = 1000;
            double stepsize = 0.005;
            int counter = 0;
            double oldll = 0;

            //fit HannanRissanen to get a good starting guess for the model
            List<double> startguess = this.HannanRissanen(p, q, 40);
            for (int i = 0; i < (p + q); i++)
                initial[i] = startguess[i];

            oldll = -this.sumofsquares((this.ARMAresiduals(sample, startguess, p, q)));
            //we will use the following to help iterate around the grid
            Matrix g = this.grid(p + q);
            Matrix step = stepsize * g;

            while ((error > 0.01) && (counter < 500))
            {
                //create a list of the likelihood values
                List<double> likelihood = new List<double>();
                //create a list of the rho and theta grid values
                List<List<double>> v = new List<List<double>>();

                Double[] next = new double[p + q];

                for (int i = 0; i < step.Rows; i++)
                {
                    for (int j = 0; j < step.Cols; j++)
                    {
                        next[j] = step[i, j] + initial[j];
                    }
                    //we only do this step as the ARMAresiduals function needs a List<double> as input
                    List<double> hrestimates = new List<double>();
                    for (int k = 0; k < next.Length; k++)
                        hrestimates.Add(next[k]);
                    //calculate the log likelihood associated with this value
                    double ll = -this.sumofsquares(this.ARMAresiduals(sample, hrestimates, p, q));
                    //add it to the list
                    likelihood.Add(ll);
                    v.Add(hrestimates);
                }
                //what is the maximum likelihood value
                double max = this.maximum(likelihood);
                //what is its index
                int ind = likelihood.FindIndex(item => item == max);

                if (ind == -1)
                {
                    counter = 501;
                    error = 1000;
                    break;

                }
                //how does this compare to the old loglike value
                error = Math.Abs(oldll - max);
                counter++;
                //reassign variables before the next iteration
                oldll = max;
                for (int j = 0; j < step.Cols; j++)
                {
                    initial[j] = v[ind][j];
                }
                //end of while loop
            }

            List<double> final = new List<double>();

            //if the error > 0.01 or it has not converged i.e. number of steps > 500, return -1000 for each answer
            if ((error > 0.01) || (counter > 500))
            {
                for (int i = 0; i < (p + q); i++)
                    final.Add(-1000);
            }
            //otherwise return the last estimate
            else
            {
                for (int i = 0; i < (p + q); i++)
                    final.Add(initial[i]);
            }

            return final;
        }

        //method to estimate the residuals from the ARMAML() method
        //sample is the data, ARMAML is the list of ARMA(p,q) parameter estimates (does not include sigma hat squared)
        public List<double> ARMAMLresiduals(List<double> sample, List<double> ARMAML, int p, int q)
        {
            List<double> residuals = new List<double>();
            int arsize = p;
            int masize = q;
            int samplesize = sample.Count();
            int i = 0, k = 0;

            //for the first Max(p,q) observations, the residual is assumed to be 0
            for (k = 0; k <= Math.Max(arsize, masize) - 1; k++)
                residuals.Add(0);

            for (i = Math.Max(arsize, masize); i <= samplesize - 1; i++)
            {
                //calculate the AR components
                double sumAR = 0;
                for (k = 0; k <= arsize - 1; k++)
                {
                    sumAR = sumAR + ARMAML[k] * sample[i - 1 - k];
                }
                //calculate the MA components
                double sumMA = 0;
                for (k = 0; k <= masize - 1; k++)
                {
                    sumMA = sumMA + ARMAML[arsize + k] * residuals[i - 1 - k];

                }
                double r = sample[i] - (sumAR + sumMA);
                residuals.Add(r);
            }

            return residuals;
        }

        //Method to find the ARMA parameter estimates using conditional maximum likelihood, sigma is also estimated
        //See Section 4.2.5 in the accompanying manual
        public List<double> ARMAMLalternative(List<double> sample, int p, int q)
        {
            double[] initial = new double[p + q + 1];
            double error = 1000;
            double stepsize = 0.005;
            int counter = 0;
            double oldll = 0;

            //fit HannanRissanen to get a good starting guess for the model
            List<double> startguess = this.HannanRissanen(p, q, 40);
            for (int i = 0; i < (p + q); i++)
                initial[i] = startguess[i];
            double sigmaguess = this.HannanRissanenVariance(this.ARMAresiduals(sample, startguess, p, q), p, q);
            initial[p + q] = sigmaguess;

            //we only do this as we want to pass a subset of the estimates to one of the functions
            List<double> parameters = new List<double>();
            for (int i = 0; i < (p + q); i++)
                parameters.Add(initial[i]);

            int m = Math.Max(p, q);
            oldll = -((sample.Count() - m) / 2) * Math.Log(2 * Math.PI) - ((sample.Count() - m) / 2) * Math.Log(initial[p + q]) - this.sumofsquares((this.ARMAresiduals(sample, parameters, p, q))) / (2 * initial[p + q]);

            //we will use the following to help iterate around the grid
            Matrix g = this.grid(p + q + 1);
            Matrix step = stepsize * g;

            while ((error > 0.01) && (counter < 750))
            {
                //create a list of the likelihood values
                List<double> likelihood = new List<double>();
                //create a list of the rho and theta grid values
                List<List<double>> v = new List<List<double>>();

                Double[] next = new double[p + q + 1];

                for (int i = 0; i < step.Rows; i++)
                {
                    for (int j = 0; j < step.Cols; j++)
                    {
                        next[j] = step[i, j] + initial[j];
                    }

                    //we only do this step as the ARMAresiduals function needs a List<double> as input
                    List<double> hrestimates = new List<double>();
                    for (int k = 0; k < next.Length; k++)
                        hrestimates.Add(next[k]);

                    List<double> temp = new List<double>();
                    for (int z = 0; z < p + q; z++)
                        temp.Add(hrestimates[z]);

                    //calculate the log likelihood associated with this value
                    double ll = -((sample.Count() - m) / 2) * Math.Log(2 * Math.PI) - ((sample.Count() - m) / 2) * Math.Log(hrestimates[p + q]) - this.sumofsquares((this.ARMAresiduals(sample, temp, p, q))) / (2 * hrestimates[p + q]);

                    //add it to the list, but ensure that sigma is >0
                    if (hrestimates[p + q] < 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        likelihood.Add(ll);
                        v.Add(hrestimates);
                    }
                }
                //what is the maximum likelihood value
                double max = this.maximumExcludingNaN(likelihood);
                //what is its index
                int ind = likelihood.FindIndex(item => item == max);

                if (ind == -1)
                {
                    counter = 751;
                    error = 1000;
                    break;

                }

                //how does this compare to the old loglike value
                error = Math.Abs(oldll - max);
                counter++;
                //reassign variables before the next iteration
                oldll = max;
                for (int j = 0; j < step.Cols; j++)
                {
                    initial[j] = v[ind][j];
                }
                //end of while loop
            }

            List<double> final = new List<double>();

            //if the error > 0.01 or it has not converged i.e. number of steps > 500, return -1000 for each answer
            if ((error > 0.01) || (counter > 750))
            {
                for (int i = 0; i < (p + q + 1); i++)
                    final.Add(-1000);
            }
            //otherwise return the last estimate
            else
            {
                for (int i = 0; i < (p + q + 1); i++)
                    final.Add(initial[i]);
            }

            return final;
        }

        //Method to find the ARCH parameter estimates using conditional maximum likelihood
        //The method will also return the AIC
        //it is important that the list aramestimates only contains the AR and MA estimates and does not contain sigma hat squared 
        //See Section 5.2 in the accompanying manual
        //Also see Equation 10.5 in http://media.wolfram.com/documents/TimeSeriesDocumentation.pdf
        public List<double> archML(List<double> sample, List<double> armaestimates, int p, int q, int archq)
        {
            double[] initial = new double[archq + 1];
            double aic = 0;
            double error = 1000;
            double stepsize = 0.005;
            int counter = 0;
            double oldll = 0;

            //initial guesses for the alpha1,...alphaq parameters
            for (int i = 0; i < archq; i++)
            {
                initial[i + 1] = (1 - 0.05) / archq;
            }

            //inital guess for the arch0 parameter
            initial[0] = 0.5;

            //need to calculate the residuals from the ARIMA model, we then creat a list of the squared residuals
            List<double> residuals = this.ARMAresiduals(sample, armaestimates, p, q);
            List<double> residualssquared = new List<double>();
            foreach (double resid in residuals)
                residualssquared.Add(resid * resid);

            //now we need to calculate the ht
            //first we need to get the initial vector into a list format
            List<double> init = new List<double>();
            for (int i = 0; i < archq + 1; i++)
                init.Add(initial[i]);
            List<double> ht = this.archht(residualssquared, init);

            //next we need to estimate the log likelihhood
            for (int i = 0; i < sample.Count(); i++)
            {
                if (ht[i] == 0)
                    continue;
                else
                    oldll = oldll - 0.5 * Math.Log(ht[i]) - residualssquared[i] / (2 * ht[i]);
            }
            oldll = oldll - (sample.Count() / 2) * Math.Log(2.0 * Math.PI);

            //we will use the following to help iterate around the grid
            Matrix g = this.grid(archq + 1);
            Matrix step = stepsize * g;

            while ((error > 0.01) && (counter < 750))
            {
                //create a list of the likelihood values
                List<double> likelihood = new List<double>();
                //create a list of the arch parameter grid values
                List<List<double>> v = new List<List<double>>();

                Double[] next = new double[archq + 1];

                //run through a particular scenario
                for (int i = 0; i < step.Rows; i++)
                {
                    for (int j = 0; j < step.Cols; j++)
                    {
                        //we want to ensure that we are only searching a positive grid space i.e. all the alphas >=0
                        if (step[i, j] + initial[j] < 0)
                            next[j] = initial[j];
                        else
                            next[j] = step[i, j] + initial[j];
                    }

                    //we only do this step as the archht function needs a List<double> as input
                    List<double> archestimates = new List<double>();
                    for (int k = 0; k < next.Length; k++)
                        archestimates.Add(next[k]);

                    //calculate the log likelihood associated with this scenario
                    double ll = 0;
                    List<double> tempht = this.archht(residualssquared, archestimates);
                    for (int k = 0; k < sample.Count(); k++)
                    {
                        if (tempht[k] == 0)
                            continue;
                        else
                            ll = ll - 0.5 * Math.Log(tempht[k]) - residualssquared[k] / (2 * tempht[k]);
                    }
                    ll = ll - (sample.Count() / 2) * Math.Log(2.0 * Math.PI);
                    likelihood.Add(ll);
                    v.Add(archestimates);
                }

                //what is the maximum likelihood value
                double max = this.maximumExcludingNaN(likelihood);
                //what is its index
                int ind = likelihood.FindIndex(item => item == max);

                if (ind == -1)
                {
                    counter = 751;
                    error = 1000;
                    break;

                }

                //how does this compare to the old loglike value
                error = Math.Abs(oldll - max);
                counter++;
                //reassign variables before the next iteration
                oldll = max;
                for (int j = 0; j < step.Cols; j++)
                {
                    initial[j] = v[ind][j];
                }
                //formula taken from equation 16 in "Comprehensive evaluation of ARMA-GARCH(-M) approaches for modelling the mean and volatility of wind speed"
                aic = 2 * (archq + 1) - 2 * max;
                //end of while loop
            }

            List<double> final = new List<double>();

            //if the error > 0.01 or it has not converged i.e. number of steps > 500, return -1000 for each answer
            if ((error > 0.01) || (counter > 750))
            {
                for (int i = 0; i < (archq + 1); i++)
                    final.Add(-1000);
                //add an additional -1000 to correspond to the AIC placeholder
                final.Add(-1000);
            }
            //otherwise return the last estimate
            else
            {
                for (int i = 0; i < (archq + 1); i++)
                    final.Add(initial[i]);
            }
            //add in the AIC value for the model
            final.Add(aic);

            return final;
        }

        //Method to find the GARCH parameter estimates using conditional maximum likelihood
        //The method will also return the AIC
        //it is important that the list aramestimates only contains the AR and MA estimates and does not contain sigma hat squared
        //See Section 5.2 in the accompanying manual
        //Also see Equation 10.5 in http://media.wolfram.com/documents/TimeSeriesDocumentation.pdf
        public List<double> garchML(List<double> sample, List<double> armaestimates, int p, int q, int archq, int archp)
        {
            double[] initial = new double[archq + archp + 1];
            double error = 1000;
            double stepsize = 0.005;
            int counter = 0;
            double oldll = 0;
            double aic = 0;

            //initial guesses for the alpha1,...alphaq, beta1,....,betap parameters
            for (int i = 0; i < (archq + archp); i++)
            {
                initial[i + 1] = (1 - 0.05) / (archq + archp);
            }

            //inital guess for the alpha0 parameter
            initial[0] = 0.1;

            //need to calculate the residuals from the ARIMA model, we then create a list of the squared residuals
            List<double> residuals = this.ARMAresiduals(sample, armaestimates, p, q);
            List<double> residualssquared = new List<double>();
            foreach (double resid in residuals)
                residualssquared.Add(resid * resid);

            //now we need to calculate the ht
            //first we need to get the initial vector into a list format
            List<double> init = new List<double>();
            for (int i = 0; i < archq + archp + 1; i++)
                init.Add(initial[i]);
            List<double> ht = this.garchht(residualssquared, init, archq, archp);

            //next we need to estimate the log likelihhood
            for (int i = 0; i < sample.Count(); i++)
            {
                if (ht[i] == 0)
                    continue;
                else
                    oldll = oldll - 0.5 * Math.Log(ht[i]) - residualssquared[i] / (2 * ht[i]);
            }
            oldll = oldll - (sample.Count() / 2) * Math.Log(2.0 * Math.PI);

            //we will use the following to help iterate around the grid
            Matrix g = this.grid(archq + archp + 1);
            Matrix step = stepsize * g;

            while ((error > 0.01) && (counter < 750))
            {
                //create a list of the likelihood values
                List<double> likelihood = new List<double>();
                //create a list of the arch parameter grid values
                List<List<double>> v = new List<List<double>>();

                Double[] next = new double[archq + archp + 1];

                //run through a particular scenario
                for (int i = 0; i < step.Rows; i++)
                {
                    for (int j = 0; j < step.Cols; j++)
                    {
                        //we want to ensure that we are only searching a positive grid space i.e. all the alphas and betas >=0
                        if (step[i, j] + initial[j] < 0)
                            next[j] = initial[j];
                        else
                            next[j] = step[i, j] + initial[j];
                    }

                    //we only do this step as the archht function needs a List<double> as input
                    List<double> garchestimates = new List<double>();
                    for (int k = 0; k < next.Length; k++)
                        garchestimates.Add(next[k]);

                    //calculate the log likelihood associated with this scenario
                    double ll = 0;
                    List<double> tempht = this.garchht(residualssquared, garchestimates, archq, archp);
                    for (int k = 0; k < sample.Count(); k++)
                    {
                        if (tempht[k] == 0)
                            continue;
                        else
                            ll = ll - 0.5 * Math.Log(tempht[k]) - residualssquared[k] / (2 * tempht[k]);
                    }
                    ll = ll - (sample.Count() / 2) * Math.Log(2.0 * Math.PI);
                    likelihood.Add(ll);
                    v.Add(garchestimates);
                }

                //what is the maximum likelihood value
                double max = this.maximumExcludingNaN(likelihood);
                //what is its index
                int ind = likelihood.FindIndex(item => item == max);

                if (ind == -1)
                {
                    counter = 751;
                    error = 1000;
                    break;

                }

                //how does this compare to the old loglike value
                error = Math.Abs(oldll - max);
                counter++;
                //reassign variables before the next iteration
                oldll = max;
                for (int j = 0; j < step.Cols; j++)
                {
                    initial[j] = v[ind][j];
                }
                //formula taken from equation 16 in "Comprehensive evaluation of ARMA-GARCH(-M) approaches for modelling the mean and volatility of wind speed"
                aic = 2 * (archp + archq + 1) - 2 * max;
                //end of while loop
            }

            List<double> final = new List<double>();

            //if the error > 0.01 or it has not converged i.e. number of steps > 500, return -1000 for each answer
            if ((error > 0.01) || (counter > 750))
            {
                for (int i = 0; i < (archq + 1); i++)
                    final.Add(-1000);
                //add in a placehoder for the aic
                final.Add(-1000);
            }
            //otherwise return the last estimate
            else
            {
                for (int i = 0; i < (archq + archp + 1); i++)
                    final.Add(initial[i]);
                //add in the aic 
                final.Add(aic);
            }

            return final;
        }

        //method to calculate the ht values given by
        // ht = alpha0 + alpha1 * e(t-1)*e(t-1)+....+alphaq*e(t-q)*e(t-q)
        //where the et's are the residuals for a particular ARIMA model
        //See Section 5.2 in the accompanying manual
        public List<double> archht(List<double> squaredresiduals, List<double> archestimates)
        {
            List<double> ht = new List<double>();
            int numparams = archestimates.Count();
            int samplesize = squaredresiduals.Count();
            int i = 0, k = 0;

            //for the first archq observations, the ht values are assumed to be 0
            for (k = 0; k < numparams - 1; k++)
                ht.Add(0);

            for (i = numparams - 1; i <= samplesize - 1; i++)
            {
                double sum = 0;
                for (k = 1; k <= numparams - 1; k++)
                {
                    sum = sum + archestimates[k] * squaredresiduals[i - k];
                }
                double r = sum + archestimates[0];
                ht.Add(r);
            }

            return ht;
        }

        //method to calculate the ht values given by
        // h(t) = alpha0 + alpha1 * e(t-1)*e(t-1)+....+alphaq*e(t-q)*e(t-q)+beta1*h(t-1)+.....+betap*h(t-p)
        //where the et's are the residuals for a particular ARIMA model
        //See Section 5.2 in the accompanying manual
        public List<double> garchht(List<double> squaredresiduals, List<double> garchestimates, int archq, int archp)
        {
            List<double> ht = new List<double>();
            int numparams = garchestimates.Count();
            int samplesize = squaredresiduals.Count();
            int i = 0, k = 0;

            //for the first max(archq, archp) observations set h(t) to 0
            for (k = 0; k < Math.Max(archq, archp); k++)
                ht.Add(0);

            for (i = Math.Max(archq, archp); i <= samplesize - 1; i++)
            {
                //calculate the MA components
                double sumMA = 0;
                for (k = 0; k <= archq - 1; k++)
                {
                    sumMA = sumMA + garchestimates[k + 1] * squaredresiduals[i - k - 1];

                }
                //calculate the AR components
                double sumAR = 0;
                for (k = 0; k <= archp - 1; k++)
                {
                    sumAR = sumAR + garchestimates[k + archq + 1] * ht[i - 1 - k];
                }

                double r = garchestimates[0] + sumAR + sumMA;
                ht.Add(r);
            }

            return ht;
        }

        //method to calculate the v(t) from an ARCH model
        //need to ensure that the "archestimates" only contains alpha0,....,alphaq and does not contain the AIC value
        //See Equation 5.2 in the accompanying manual
        public List<double> archresiduals(List<double> sample, List<double> armaestimates, int p, int q, int archq, List<double> archestimates)
        {
            List<double> archresiduals = new List<double>();

            //need to calculate the residuals from the ARIMA model, we then created a list of the squared residuals
            List<double> residuals = this.ARMAresiduals(sample, armaestimates, p, q);
            List<double> residualssquared = new List<double>();
            foreach (double resid in residuals)
                residualssquared.Add(resid * resid);

            //now we need to calculate the ht
            List<double> ht = this.archht(residualssquared, archestimates);

            //now calculate the standardised residuals
            for (int i = 0; i < residuals.Count(); i++)
            {
                if (ht[i] == 0)
                    archresiduals.Add(0);
                else
                    archresiduals.Add(residuals[i] / Math.Sqrt(ht[i]));
            }

            return archresiduals;
        }

        //method to calculate the v(t) from an ARCH model
        //need to ensure that the "garchestimates" only contains alpha0,....,alphaq,beta1,.....,betap and does not contain the AIC value
        //See Equation 5.2 in the accompanying manual
        public List<double> garchresiduals(List<double> sample, List<double> armaestimates, int p, int q, int archq, int archp, List<double> garchestimates)
        {
            List<double> garchresiduals = new List<double>();

            //need to calculate the residuals from the ARIMA model, we then created a list of the squared residuals
            List<double> residuals = this.ARMAresiduals(sample, armaestimates, p, q);
            List<double> residualssquared = new List<double>();
            foreach (double resid in residuals)
                residualssquared.Add(resid * resid);

            //now we need to calculate the ht
            List<double> ht = this.garchht(residualssquared, garchestimates, archq, archp);

            //now calculate the standardised residuals
            for (int i = 0; i < residuals.Count(); i++)
            {
                if (ht[i] == 0)
                    garchresiduals.Add(0);
                else
                    garchresiduals.Add(residuals[i] / Math.Sqrt(ht[i]));
            }

            return garchresiduals;
        }

        //method to predict n steps ahead from a particular point in a dataset
        //the last observation, plus the next n point forecasts will be returned
        //assumes that the parameters are an ARIMA model
        //note only the param estimates are passed and not the valued of sigma hat squared
        //See Section 6.1 in the accompanying manual
        public List<double> predictnstepsahead(List<double> data, List<double> armaestimates, int p, int q, int nsteps, int point)
        {
            //variable which indicates the correct starting point of the forecast
            int startpoint = 0;

            //list which will contain the output
            List<double> forecast = new List<double>();

            //temporary lists which will be used in the forecasting process
            List<double> tdata = new List<double>();
            List<double> tresid = new List<double>();

            //first we need to calculate the residuals
            List<double> resid = this.ARMAresiduals(data, armaestimates, p, q);

            startpoint = point;

            //now add the very first point to the forecast list
            forecast.Add(data[startpoint - 1]);

            //setup the tdata list
            for (int i = 0; i < (startpoint - 1) - p; i++)
                tdata.Add(0);
            for (int i = p; i > 0; i--)
                tdata.Add(data[(startpoint - 1) - i]);

            //setup the tresid list
            for (int i = 0; i < (startpoint - 1) - q; i++)
                tresid.Add(0);
            for (int i = q; i > 0; i--)
                tresid.Add(resid[(startpoint - 1) - i]);

            //now we can start the forecasting process

            //we want to do the loopo n times
            for (int i = 0; i < nsteps; i++)
            {
                //variable to help with the indexing
                int vsize = tresid.Count();

                //variables which will hold the sum of the AR and MA components respectively
                double sumAR = 0;
                double sumMA = 0;

                for (int k = 0; k <= p - 1; k++)
                {
                    sumAR = sumAR + armaestimates[k] * tdata[vsize - 1 - k];
                }
                //calculate the MA components
                for (int k = 0; k <= q - 1; k++)
                {
                    sumMA = sumMA + armaestimates[p + k] * tresid[vsize - 1 - k];

                }
                double total = sumAR + sumMA;

                //add the point forecast
                forecast.Add(total);

                //update the tdata and tresid lists
                tresid.Add(0);
                tdata.Add(total);
            }

            return forecast;
        }

        //method to predict n steps ahead from a particular point in a dataset
        //Same as the predictnstepsahead() method, except this method will work with differencing
        //data is the original data i.e. undifferenced
        //the last observation, plus the next n point forecasts will be returned
        //assumes that the parameters are an ARIMA model
        //note only the param estimates are passed and not the valued of sigma squared
        //See Section 6.1 in the accompanying manual
        //Also see the discussion below
        //http://stats.stackexchange.com/questions/126525/time-series-forecast-convert-differenced-forecast-back-to-before-difference-lev?rq=1.
        public List<double> predictnstepsaheadDD(List<double> data, List<double> armaestimates, int p, int q, int nsteps, int point, int diff)
        {
            List<double> ans = new List<double>();

            //first we need to set up the Lists i.e. (1) data (2) data differenced order 1 (3) data differenced order 2 etc..........
            List<List<double>> blocks = new List<List<double>>();
            List<double> subblock = new List<double>();
            //the first block is just the original time series data
            for (int i = 0; i < data.Count(); i++)
                subblock.Add(data[i]);
            blocks.Add(subblock);
            //calculate the differenced data
            for (int i = 1; i <= diff; i++)
            {
                List<double> t = new List<double>();
                for (int k = 1; k <= i; k++)
                    t.Add(0);
                for (int j = i; j < blocks[i - 1].Count(); j++)
                {
                    t.Add(blocks[i - 1][j] - blocks[i - 1][j - 1]);
                }
                blocks.Add(t);
            }

            int countA = blocks.Count();

            //create the n step ahead forecast using the last list
            List<double> forecast = predictnstepsahead(blocks[diff], armaestimates, p, q, nsteps, point);
            //we don't need the first point as that is the actual observation and not a forecast
            forecast.RemoveAt(0);
            //now we'll need to start working backwards up the list
            List<List<double>> forecastblocks = new List<List<double>>();
            forecastblocks.Add(forecast);
            for (int i = 0; i < diff; i++)
            {
                List<double> t = new List<double>();
                for (int j = 0; j < forecastblocks[i].Count(); j++)
                {
                    double sum = 0;
                    for (int l = 0; l <= j; l++)
                    {
                        sum = sum + forecastblocks[i][l];
                    }
                    double total = blocks[countA - i - 2][point - 1] + sum;
                    t.Add(total);
                }
                forecastblocks.Add(t);
            }

            int countB = forecastblocks.Count();
            //we insert the last actual observation, this is just convention as it is how the predictnstepsahead() function also works
            forecastblocks[countB - 1].Insert(0, blocks[0][point - 1]);
            return forecastblocks[countB - 1];
        }

        //given a forecast or a set of data, this method will return the forecast/data to the original units depending on how the data was transformed
        //it will also take account of whether the data was mean adjusted
        //values will be capped at 3,500 and a floor 0, reflecting Irish Electricity System
        public List<double> unwind(List<double> data, string method, double shape, double mean)
        {
            List<double> ans = new List<double>();
            List<double> forecast = new List<double>();

            //adjust for the mean
            if (mean != 0)
            {
                foreach (double dd in data)
                    forecast.Add(dd + mean);
            }
            else
            {
                foreach (double dd in data)
                    forecast.Add(dd);
            }

            switch (method)
            {
                case "Iterative":
                    foreach (double dd in forecast)
                        ans.Add(Math.Pow(dd, (1.0 / shape)));
                    break;
                case "Weibull":
                    foreach (double dd in forecast)
                        ans.Add(Math.Pow(dd, (3.6 / shape)));
                    break;
                default:
                    foreach (double dd in forecast)
                        ans.Add(dd);
                    break;
            }

            int z=0;
            for(z=0;z<ans.Count();z++)
            {
                if (ans[z] > 3500)
                {
                    ans[z] = 3500.0;
                }
                if (ans[z] < 0)
                {
                    ans[z] = 0;
                }
            }
            return ans;
        }

        //method to simulate n steps ahead from a particular point in a dataset
        //the last observation, plus the next n point forecasts will be returned
        //assumes that the parameters are an ARMA model
        //See Section 6.1 in the accompanying manual
        public List<double> ARMAsimulatenstepsahead(List<double> data, List<double> armaestimates, int p, int q, int nsteps, int point, double variance)
        {
            //variable which indicates the correct starting point of the forecast
            int startpoint = 0;

            //list which will contain the output
            List<double> forecast = new List<double>();

            //temporary lists which will be used in the forecasting process
            List<double> tdata = new List<double>();
            List<double> tresid = new List<double>();

            //List of random draws from a normal distribution
            List<double> nsample = this.normal_sample(nsteps, 0, Math.Sqrt(variance));

            //first we need to calculate the residuals
            List<double> resid = this.ARMAresiduals(data, armaestimates, p, q);

            startpoint = point;

            //now add the very first point to the forecast list
            forecast.Add(data[startpoint - 1]);

            //setup the tdata list
            for (int i = 0; i < (startpoint - 1) - p; i++)
                tdata.Add(0);
            for (int i = p; i > 0; i--)
                tdata.Add(data[(startpoint - 1) - i]);

            //setup the tresid list
            for (int i = 0; i < (startpoint - 1) - q; i++)
                tresid.Add(0);
            for (int i = q; i > 0; i--)
                tresid.Add(resid[(startpoint - 1) - i]);

            //now we can start the forecasting process

            //we want to do the loopo n times
            for (int i = 0; i < nsteps; i++)
            {
                //variable to help with the indexing
                int vsize = tresid.Count();

                //variables which will hold the sum of the AR and MA components respectively
                double sumAR = 0;
                double sumMA = 0;

                for (int k = 0; k <= p - 1; k++)
                {
                    sumAR = sumAR + armaestimates[k] * tdata[vsize - 1 - k];
                }
                //calculate the MA components
                for (int k = 0; k <= q - 1; k++)
                {
                    sumMA = sumMA + armaestimates[p + k] * tresid[vsize - 1 - k];

                }
                //here we add in the random draw
                double total = sumAR + sumMA + nsample[i];

                //add the point forecast
                forecast.Add(total);

                //update the tdata and tresid lists
                tresid.Add(nsample[i]);
                tdata.Add(total);
            }

            return forecast;
        }

        //method to simulate n steps ahead from a particular point in a dataset
        //the last observation, plus the next n point forecasts will be returned
        //assumes that the parameters are an ARMA model and ARCH model
        //See Section 6.1 in the accompanying manual
        public List<double> ARCHsimulatenstepsahead(List<double> data, List<double> armaestimates, int p, int q, int nsteps, int point, double variance, int archq, List<double> archestimates)
        {
            //variable which indicates the correct starting point of the forecast
            int startpoint = 0;

            //list which will contain the output
            List<double> forecast = new List<double>();

            //temporary lists which will be used in the forecasting process
            List<double> tdata = new List<double>();
            List<double> tresid = new List<double>();
            List<double> tresid2 = new List<double>();
            //List<double> tarchHT = new List<double>();

            //List of random draws from a normal distribution
            List<double> nsample = this.normal_sample(nsteps, 0, 1);

            //first we need to calculate the residuals
            List<double> resid = this.ARMAresiduals(data, armaestimates, p, q);
            List<double> resid2 = new List<double>();
            foreach (double dd in resid)
                resid2.Add(dd * dd);

            startpoint = point;

            //now add the very first point to the forecast list
            forecast.Add(data[startpoint - 1]);

            //setup the tdata list
            for (int i = 0; i < (startpoint - 1) - p; i++)
                tdata.Add(0);
            for (int i = p; i > 0; i--)
                tdata.Add(data[(startpoint - 1) - i]);

            //setup the tresid list
            for (int i = 0; i < (startpoint - 1) - q; i++)
                tresid.Add(0);
            for (int i = q; i > 0; i--)
                tresid.Add(resid[(startpoint - 1) - i]);

            //setup the tresid2 list
            for (int i = 0; i < (startpoint - 1) - archq; i++)
                tresid2.Add(0);
            for (int i = archq; i > 0; i--)
                tresid2.Add(resid2[(startpoint - 1) - i]);

            //now we can start the simulating

            //we want to do the loopo n times
            for (int i = 0; i < nsteps; i++)
            {
                //variable to help with the indexing
                int vsize = tresid.Count();

                //variables which will hold the sum of the AR and MA components respectively
                double sumAR = 0;
                double sumMA = 0;

                for (int k = 0; k <= p - 1; k++)
                {
                    sumAR = sumAR + armaestimates[k] * tdata[vsize - 1 - k];
                }
                //calculate the MA components
                for (int k = 0; k <= q - 1; k++)
                {
                    sumMA = sumMA + armaestimates[p + k] * tresid[vsize - 1 - k];

                }

                //calculate the ht
                double ht = 0;
                for (int k = 1; k <= archq; k++)
                {
                    ht = ht + archestimates[k] * tresid2[vsize - k];
                }
                ht = ht + archestimates[0];

                //calulcate the et
                double et = Math.Sqrt(ht) * nsample[i];

                //here we add in the random draw
                double total = sumAR + sumMA + et;

                //add the point forecast
                forecast.Add(total);

                //update the tdata and tresid lists
                tresid.Add(et);
                tdata.Add(total);
                tresid2.Add(et * et);
            }

            return forecast;
        }

        //method to simulate n steps ahead from a particular point in a dataset
        //the last observation, plus the next n point forecasts will be returned
        //assumes that the parameters are an ARMA model and GARCH model
        //See Section 6.1 in the accompanying manual
        public List<double> GARCHsimulatenstepsahead(List<double> data, List<double> armaestimates, int p, int q, int nsteps, int point, double variance, int archq, int archp,List<double> archestimates)
        {
            //variable which indicates the correct starting point of the forecast
            int startpoint = 0;

            //list which will contain the output
            List<double> forecast = new List<double>();

            //temporary lists which will be used in the forecasting process
            List<double> tdata = new List<double>();
            List<double> tresid = new List<double>();
            List<double> tresid2 = new List<double>();
            List<double> tarchHT = new List<double>();

            //List of random draws from a normal distribution
            List<double> nsample = this.normal_sample(nsteps, 0, 1);

            //first we need to calculate the residuals
            List<double> resid = this.ARMAresiduals(data, armaestimates, p, q);
            List<double> resid2 = new List<double>();
            foreach (double dd in resid)
                resid2.Add(dd * dd);

            List<double> archHT = garchht(resid2, archestimates, archq, archp);

            startpoint = point;

            //now add the very first point to the forecast list
            forecast.Add(data[startpoint - 1]);

            //setup the tdata list
            for (int i = 0; i < (startpoint - 1) - p; i++)
                tdata.Add(0);
            for (int i = p; i > 0; i--)
                tdata.Add(data[(startpoint - 1) - i]);

            //setup the tresid list
            for (int i = 0; i < (startpoint - 1) - q; i++)
                tresid.Add(0);
            for (int i = q; i > 0; i--)
                tresid.Add(resid[(startpoint - 1) - i]);

            //setup the tresid2 list
            for (int i = 0; i < (startpoint - 1) - archq; i++)
                tresid2.Add(0);
            for (int i = archq; i > 0; i--)
                tresid2.Add(resid2[(startpoint - 1) - i]);
            
            //setup the tarchHT list
            for (int i = 0; i < (startpoint - 1) - archp; i++)
                tarchHT.Add(0);
            for (int i = archp; i > 0; i--)
                tarchHT.Add(archHT[(startpoint - 1) - i]);

            //now we can start the simulating

            //we want to do the loopo n times
            for (int i = 0; i < nsteps; i++)
            {
                //variable to help with the indexing
                int vsize = tresid.Count();

                //variables which will hold the sum of the AR and MA components respectively
                double sumAR = 0;
                double sumMA = 0;

                for (int k = 0; k <= p - 1; k++)
                {
                    sumAR = sumAR + armaestimates[k] * tdata[vsize - 1 - k];
                }
                //calculate the MA components
                for (int k = 0; k <= q - 1; k++)
                {
                    sumMA = sumMA + armaestimates[p + k] * tresid[vsize - 1 - k];

                }

                //calculate the sum of the et^2 + alpha0
                double sumet = 0;
                for (int k = 1; k <= archq; k++)
                {
                    sumet = sumet + archestimates[k] * tresid2[vsize - k];
                }
                sumet = sumet + archestimates[0];

                //calculate the sum of the ht
                double sumht = 0;
                for (int k = 1; k <= archp; k++)
                {
                    sumht = sumht + archestimates[archq+k] * tarchHT[vsize - k];
                }

                double ht = 0;
                ht = sumet + sumht;

                //calulcate the et
                double et = Math.Sqrt(ht) * nsample[i];

                //here we add in the random draw
                double total = sumAR + sumMA + et;

                //add the point forecast
                forecast.Add(total);

                //update the tdata and tresid lists
                tresid.Add(et);
                tdata.Add(total);
                tresid2.Add(et * et);
                tarchHT.Add(ht);
            }

            return forecast;
        }

        //simulate n steps ahead for an ARMA model which has differencing
        //the last observation plus the next n observations will be returned
        //See Section 6.1 in the accompanying manual
        //Also see discussion in
        //http://stats.stackexchange.com/questions/126525/time-series-forecast-convert-differenced-forecast-back-to-before-difference-lev?rq=1.
        public List<double> ARMAsimulatenstepsaheadDD(List<double> data, List<double> armaestimates, int p, int q, int nsteps, int point, double variance, int diff)
        {
            List<double> ans = new List<double>();

            //first we need to set up the Lists i.e. (1) data (2) data differenced order 1 (3) data differenced order 2 etc..........
            List<List<double>> blocks = new List<List<double>>();
            List<double> subblock = new List<double>();
            //the first block is just the original time series data
            for (int i = 0; i < data.Count(); i++)
                subblock.Add(data[i]);
            blocks.Add(subblock);
            //calculate the differenced data
            for (int i = 1; i <= diff; i++)
            {
                List<double> t = new List<double>();
                for (int k = 1; k <= i; k++)
                    t.Add(0);
                for (int j = i; j < blocks[i - 1].Count(); j++)
                {
                    t.Add(blocks[i - 1][j] - blocks[i - 1][j - 1]);
                }
                blocks.Add(t);
            }

            int countA = blocks.Count();

            //create the n step ahead forecast using the last list
            //List<double> forecast = predictnstepsahead(blocks[diff], armaestimates, p, q, nsteps, point);
            List<double> forecast = ARMAsimulatenstepsahead(blocks[diff], armaestimates, p, q, nsteps, point,variance);
            //we don't need the first point as that is the actual observation and not a forecast
            forecast.RemoveAt(0);
            //now we'll need to start working backwards up the list
            List<List<double>> forecastblocks = new List<List<double>>();
            forecastblocks.Add(forecast);
            for (int i = 0; i < diff; i++)
            {
                List<double> t = new List<double>();
                for (int j = 0; j < forecastblocks[i].Count(); j++)
                {
                    double sum = 0;
                    for (int l = 0; l <= j; l++)
                    {
                        sum = sum + forecastblocks[i][l];
                    }
                    double total = blocks[countA - i - 2][point - 1] + sum;
                    t.Add(total);
                }
                forecastblocks.Add(t);
            }

            int countB = forecastblocks.Count();
            //we insert the last actual observation, this is just convention as it is how the predictnstepsahead() function also works
            forecastblocks[countB - 1].Insert(0, blocks[0][point - 1]);
            return forecastblocks[countB - 1];
        }

        //simulate n steps ahead for an ARMA + ARCH model which has differencing
        //the last observation plus the next n observations will be returned
        //See Section 6.1 in the accompanying manual
        //Also see discussion in
        //http://stats.stackexchange.com/questions/126525/time-series-forecast-convert-differenced-forecast-back-to-before-difference-lev?rq=1.
        public List<double> ARCHsimulatenstepsaheadDD(List<double> data, List<double> armaestimates, int p, int q, int nsteps, int point, double variance,int diff, int archq,List<double> archestimates)
        {
            List<double> ans = new List<double>();

            //first we need to set up the Lists i.e. (1) data (2) data differenced order 1 (3) data differenced order 2 etc..........
            List<List<double>> blocks = new List<List<double>>();
            List<double> subblock = new List<double>();
            //the first block is just the original time series data
            for (int i = 0; i < data.Count(); i++)
                subblock.Add(data[i]);
            blocks.Add(subblock);
            //calculate the differenced data
            for (int i = 1; i <= diff; i++)
            {
                List<double> t = new List<double>();
                for (int k = 1; k <= i; k++)
                    t.Add(0);
                for (int j = i; j < blocks[i - 1].Count(); j++)
                {
                    t.Add(blocks[i - 1][j] - blocks[i - 1][j - 1]);
                }
                blocks.Add(t);
            }

            int countA = blocks.Count();


            //http://stats.stackexchange.com/questions/126525/time-series-forecast-convert-differenced-forecast-back-to-before-difference-lev?rq=1.

            //create the n step ahead forecast using the last list
            //List<double> forecast = predictnstepsahead(blocks[diff], armaestimates, p, q, nsteps, point);
            List<double> forecast = ARCHsimulatenstepsahead(blocks[diff], armaestimates, p, q, nsteps, point, variance, archq, archestimates);
            //we don't need the first point as that is the actual observation and not a forecast
            forecast.RemoveAt(0);
            //now we'll need to start working backwards up the list
            List<List<double>> forecastblocks = new List<List<double>>();
            forecastblocks.Add(forecast);
            for (int i = 0; i < diff; i++)
            {
                List<double> t = new List<double>();
                for (int j = 0; j < forecastblocks[i].Count(); j++)
                {
                    double sum = 0;
                    for (int l = 0; l <= j; l++)
                    {
                        sum = sum + forecastblocks[i][l];
                    }
                    double total = blocks[countA - i - 2][point - 1] + sum;
                    t.Add(total);
                }
                forecastblocks.Add(t);
            }

            int countB = forecastblocks.Count();
            //we insert the last actual observation, this is just convention as it is how the predictnstepsahead() function also works
            forecastblocks[countB - 1].Insert(0, blocks[0][point - 1]);
            return forecastblocks[countB - 1];
        }

        //simulate n steps ahead for an ARMA + GARCH model which has differencing
        //the last observation plus the next n observations will be returned
        //See Section 6.1 in the accompanying manual
        public List<double> GARCHsimulatenstepsaheadDD(List<double> data, List<double> armaestimates, int p, int q, int nsteps, int point, double variance,int diff,int archq, int archp, List<double> archestimates)
        {
            List<double> ans = new List<double>();

            //first we need to set up the Lists i.e. (1) data (2) data differenced order 1 (3) data differenced order 2 etc..........
            List<List<double>> blocks = new List<List<double>>();
            List<double> subblock = new List<double>();
            //the first block is just the original time series data
            for (int i = 0; i < data.Count(); i++)
                subblock.Add(data[i]);
            blocks.Add(subblock);
            //calculate the differenced data
            for (int i = 1; i <= diff; i++)
            {
                List<double> t = new List<double>();
                for (int k = 1; k <= i; k++)
                    t.Add(0);
                for (int j = i; j < blocks[i - 1].Count(); j++)
                {
                    t.Add(blocks[i - 1][j] - blocks[i - 1][j - 1]);
                }
                blocks.Add(t);
            }

            int countA = blocks.Count();

            //create the n step ahead forecast using the last list
            List<double> forecast = GARCHsimulatenstepsahead(blocks[diff], armaestimates, p, q, nsteps, point, variance, archq, archp, archestimates);
            //we don't need the first point as that is the actual observation and not a forecast
            forecast.RemoveAt(0);
            //now we'll need to start working backwards up the list
            List<List<double>> forecastblocks = new List<List<double>>();
            forecastblocks.Add(forecast);
            for (int i = 0; i < diff; i++)
            {
                List<double> t = new List<double>();
                for (int j = 0; j < forecastblocks[i].Count(); j++)
                {
                    double sum = 0;
                    for (int l = 0; l <= j; l++)
                    {
                        sum = sum + forecastblocks[i][l];
                    }
                    double total = blocks[countA - i - 2][point - 1] + sum;
                    t.Add(total);
                }
                forecastblocks.Add(t);
            }

            int countB = forecastblocks.Count();
            //we insert the last actual observation, this is just convention as it is how the predictnstepsahead() function also works
            forecastblocks[countB - 1].Insert(0, blocks[0][point - 1]);
            return forecastblocks[countB - 1];
        }

        //end of statistics class
    }





    //end of namespace
}



