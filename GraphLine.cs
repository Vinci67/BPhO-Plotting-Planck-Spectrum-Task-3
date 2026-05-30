using ScottPlot;
using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BPhO__Plotting_Planck_Spectrum_Task_3
{
    internal class GraphLine : IDisposable
    {
        public ScottPlot.Plottables.Scatter line;
        private double temperature;
        public double getTemp { get => temperature; }
        public TrackBar slider;
        private double SCALE = 100;
        private double h = 6.626E-34;
        private double kB = 1.381E-23;
        private double c = 2.998E8;
        private ScottPlot.Color colour;
        private double minWavelength;
        private double maxWavelength;
        private ScottPlot.WinForms.FormsPlot scottForm;
        private double[] dataWalength;
        private double[] dataWavelengthPlotting;
        private double[] irradiance;
        public string id;
        
        private double highestIndex = 0;
        static private int datapoints = 1000;
        private double datapointsInvisibleRange = 0;
        private double minVisW = 0;
        private double maxVisW = 750E-9;

        public GraphLine(double minWavelength, double maxWavelength, double temp, Form1 form, ScottPlot.WinForms.FormsPlot scottForm, string id)
        {

            temperature = temp;
            this.minWavelength = minWavelength;
            this.scottForm = scottForm;
            this.maxWavelength = maxWavelength;
            
            line = generatePlot(scottForm, minWavelength,maxWavelength, temp);
            slider = generateSlider(form,0,120);
            this.id = id;
            
        }
        private ScottPlot.Plottables.Scatter generatePlot(ScottPlot.WinForms.FormsPlot scottForm, double minWavelength, double maxWavelength, double temp)
        {

            

            dataWalength = MathNet.Numerics.Generate.LinearSpaced(datapoints, minWavelength, maxWavelength);
            dataWavelengthPlotting = MathNet.Numerics.Generate.LinearSpaced(datapoints, minWavelength * 1E9, maxWavelength * 1E9); // l in nm
            irradiance = new double[dataWalength.Length];
            double highestI = 0;
            for (int i = 0; i < dataWalength.Length; i++)
            {
                irradiance[i] = plankSpectrumEquation(dataWalength[i], temp);

                if (dataWalength[i] > minVisW && dataWalength[i] < maxVisW)
                {
                    datapointsInvisibleRange++;
                    if (irradiance[i] > highestI)
                    {
                        highestI = irradiance[i];
                        highestIndex = datapointsInvisibleRange;
                    }
                }
                
            }

            var line = scottForm.Plot.Add.Scatter(dataWavelengthPlotting, irradiance);
            setColBasedOnValue();
            line.Color = colour;
            
            line.Label = $"T: {temp}K";
            
            scottForm.Refresh();
            return line;
        }

        private System.Windows.Forms.TrackBar generateSlider(Form1 form, int min, int max)
        {
            System.Windows.Forms.TrackBar slider = new System.Windows.Forms.TrackBar();
            slider.Name = temperature.ToString();
            slider.Visible = false;
            slider.Location = new Point(50, 40);
            slider.Size = new Size(100, 40);
            slider.Scroll += new EventHandler(SliderScroll);
            slider.Minimum = min;
            slider.Maximum = max;
            int value = (int)((temperature - 100) / SCALE);
            if (value < min) value = min;
            else if (value > max) value = max;
            slider.Value = value;
            scottForm.Controls.Add(slider);
            slider.BringToFront();
            
            return slider;
        }

        private void SliderScroll(object sender, EventArgs e)
        {
            if (slider.Visible == true)
            {
                this.temperature = slider.Value * SCALE + 100; // prevents temperature from reaching 0
                refreshLine(scottForm);
            }
        }

        private void refreshLine(ScottPlot.WinForms.FormsPlot scottForm)
        {
            dataWalength = MathNet.Numerics.Generate.LinearSpaced(1000, minWavelength, maxWavelength);
            dataWavelengthPlotting = MathNet.Numerics.Generate.LinearSpaced(1000, minWavelength * 1E9, maxWavelength * 1E9); // l in nm
            //line = scottForm.Plot.Add.Scatter(dataWavelengthPlotting, irradiance);
            double highestI = 0;
            datapointsInvisibleRange = 0;
            for (int i = 0; i < dataWalength.Length; i++)
            {
                irradiance[i] = plankSpectrumEquation(dataWalength[i], temperature);

                if (dataWalength[i] > minVisW && dataWalength[i] < maxVisW)
                {
                    datapointsInvisibleRange++;
                    if (irradiance[i] > highestI)
                    {
                        highestI = irradiance[i];
                        highestIndex = datapointsInvisibleRange;
                    }
                }

            }
            Debug.WriteLine("Hello");
            setColBasedOnValue();
            line.Color = colour;
            //line.Data.GetScatterPoints().
            
            line.Label = $"T: {temperature}K";
            scottForm.Refresh();
        }
        private double plankSpectrumEquation(double wavelength, double temp)
        {
            //Should return spectural Irradiance 
            return 1E-13 * Math.PI * (2 * h * c * c) / (Math.Pow(wavelength, 5)) * (1 / (Math.Exp(h * c / (wavelength * kB * temp)) - 1.0)); // return in nm *E4
        }

        public void highlightLine()
        {
            line.Color = ScottPlot.Colors.Gray;
        }

        public void unhighlightLine()
        {
            line.Color = colour;
        }

        public void Dispose()
        {
            slider.Dispose();

        }

        private void setColBasedOnValue()
        {
            double proportion = ((double)highestIndex / (double)datapointsInvisibleRange);
            double scalar = Math.Log10(proportion);
            colour = new((byte)((proportion)*255), (byte)50, (byte)((1 - proportion) * 255));
            
            Debug.WriteLine(proportion);
            Debug.WriteLine(highestIndex);
            Debug.WriteLine((proportion) * 255);
            
            //new ScottPlot.Color = new()
        }
    }
}
