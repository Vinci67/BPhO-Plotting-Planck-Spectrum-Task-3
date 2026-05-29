using ScottPlot;
using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BPhO__Plotting_Planck_Spectrum_Task_3
{
    internal class GraphLine
    {
        public ScottPlot.Plottables.Scatter line;
        private double temperature;
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

        public GraphLine(double minWavelength, double maxWavelength, double temp, ScottPlot.Color color, Form1 form, ScottPlot.WinForms.FormsPlot scottForm)
        {
            colour = color;
            temperature = temp;
            this.minWavelength = minWavelength;
            this.scottForm = scottForm;
            this.maxWavelength = maxWavelength;
            line = generatePlot(scottForm, minWavelength,maxWavelength, temp, color);
            slider = generateSlider(form,0,100);
        }
        private ScottPlot.Plottables.Scatter generatePlot(ScottPlot.WinForms.FormsPlot scottForm, double minWavelength, double maxWavelength, double temp, ScottPlot.Color color)
        {

            

            dataWalength = MathNet.Numerics.Generate.LinearSpaced(1000, minWavelength, maxWavelength);
            dataWavelengthPlotting = MathNet.Numerics.Generate.LinearSpaced(1000, minWavelength * 1E9, maxWavelength * 1E9); // l in nm
            irradiance = new double[dataWalength.Length];

            for (int i = 0; i < dataWalength.Length; i++)
            {
                irradiance[i] = plankSpectrumEquation(dataWalength[i], temp);
            }

            var line = scottForm.Plot.Add.Scatter(dataWavelengthPlotting, irradiance);
            line.Color = color;
            line.Label = Convert.ToString(temp);
            
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
            for (int i = 0; i < dataWalength.Length; i++)
            {
                irradiance[i] = plankSpectrumEquation(dataWalength[i], temperature);
                
            }
            //line.Data.GetScatterPoints().
            
            line.Label = temperature.ToString();
            scottForm.Refresh();
        }
        private double plankSpectrumEquation(double wavelength, double temp)
        {
            //Should return spectural Irradiance 
            return 1E-13 * Math.PI * (2 * h * c * c) / (Math.Pow(wavelength, 5)) * (1 / (Math.Exp(h * c / (wavelength * kB * temp)) - 1.0)); // return in nm *E4
        }
    }
}
