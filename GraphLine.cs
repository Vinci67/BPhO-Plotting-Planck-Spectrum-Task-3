using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BPhO__Plotting_Planck_Spectrum_Task_3
{
    internal class GraphLine : IDisposable
    {
        public ScottPlot.Plottables.Scatter line;
        public TrackBar slider;
        private double h = 6.62607015E-34; // Planck's constant  m2 kg / s
        private double kB = 1.380649E-23; // Blotzman Constant m2 kg s - 2 K-1
        private double R = 8.314; // Molar gas constant J/mol/K
        private double c = 2.998E8;
        private double tempDebye;
        
        private double maxKelvin = 400;
        private ScottPlot.WinForms.FormsPlot scottForm;
        private double[] dataTemperature;
        private double[] dataTemperaturePlotting;
        private double[] MolarHeatCapcity;
        public string id;
        
        private double highestIndex = 0;
        static private int datapoints = 1000;
        private double maxMolarHC;
        private double minMolarHC;

        private static bool useScalarForSlidervsProportion = true;
        public Button removeButton;

        public GraphLine(double tempDebye, double tempMax, Form1 form, ScottPlot.WinForms.FormsPlot scottForm, string id, List<GraphLine> graphs)
        {

            this.tempDebye = tempDebye;
            this.scottForm = scottForm;
            this.maxKelvin = tempMax;

            slider = generateSlider(form, 100, 2500);
            line = generatePlot(scottForm, maxKelvin);
            
            
            this.id = id;
            if (this.id == "generic")
            {
                removeButton = generateRemoveButton(form, graphs, scottForm);
            }
            
        }
        private ScottPlot.Plottables.Scatter generatePlot(ScottPlot.WinForms.FormsPlot scottForm,double maxTemp)
        {


            dataTemperature = MathNet.Numerics.Generate.LinearSpaced(datapoints, 0, maxTemp);
            dataTemperaturePlotting = dataTemperature; //MathNet.Numerics.Generate.LinearSpaced(datapoints, minWavelength * 1E9, maxWavelength * 1E9); // l in nm
            MolarHeatCapcity = new double[dataTemperaturePlotting.Length];
            double highestI = 0;
            for (int i = 0; i < dataTemperature.Length; i++)
            {
                MolarHeatCapcity[i] = ensteinsHeatCapacityEquation(dataTemperature[i]);

                if (MolarHeatCapcity[i] > highestI)
                {
                    highestI = MolarHeatCapcity[i];
                    highestIndex = i;
                }
                
            }
            var line = scottForm.Plot.Add.Scatter(dataTemperaturePlotting, MolarHeatCapcity);
            line.Color = setColBasedOnValue();
            line.Label = $"DT: {tempDebye}K";
            
            scottForm.Refresh();
            return line;
        }

        private System.Windows.Forms.Button generateRemoveButton(Form1 form, List<GraphLine> graphs, ScottPlot.WinForms.FormsPlot formsPlot)
        {
            removeButton = new System.Windows.Forms.Button();
            removeButton.Click += (s, e) =>
            {
                this.Dispose();
                formsPlot.Plot.Remove(this.line);
                graphs.Remove(this);
                formsPlot.Refresh();
            };
            formsPlot.Controls.Add(removeButton);
            removeButton.BringToFront();
            removeButton.Visible = false;
            removeButton.Text = "Remove Line";
            removeButton.Size = new Size(80, 30);
            removeButton.Location = new Point(200, 40);
            
            return removeButton;
        }
        private System.Windows.Forms.TrackBar generateSlider(Form1 form, int min, int max)
        {
            System.Windows.Forms.TrackBar slider = new System.Windows.Forms.TrackBar();
            slider.Name = id;
            slider.Visible = false;
            slider.Location = new Point(50, 40);
            slider.Size = new Size(100, 40);
            slider.Scroll += new EventHandler(SliderScroll);
            slider.Minimum = min;
            slider.Maximum = max;
            minMolarHC = ensteinsHeatCapacityEquation(maxKelvin,slider.Maximum);
            maxMolarHC = ensteinsHeatCapacityEquation(maxKelvin,slider.Minimum);
            int value = (int)(tempDebye);
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
                this.tempDebye = slider.Value;
                refreshLine(scottForm);
            }
        }

        private void refreshLine(ScottPlot.WinForms.FormsPlot scottForm)
        {
            dataTemperature = MathNet.Numerics.Generate.LinearSpaced(datapoints, 0, maxKelvin);
            dataTemperaturePlotting = dataTemperature; //MathNet.Numerics.Generate.LinearSpaced(1000, minWavelength * 1E9, maxWavelength * 1E9); // l in nm
            double highestI = 0;
            for (int i = 0; i < dataTemperature.Length; i++)
            {
                MolarHeatCapcity[i] = ensteinsHeatCapacityEquation(dataTemperature[i]);

                if (MolarHeatCapcity[i] > highestI)
                {
                    highestI = MolarHeatCapcity[i];
                    highestIndex = i;
                }
                

            }
            
            line.Color = setColBasedOnValue();

            line.Label = $"TD: {tempDebye}K";
            scottForm.Refresh();
        }

        private double ensteinsHeatCapacityEquation(double temp)
        {
            double tempD = tempDebye; 
            double tempE = tempD * Math.Cbrt(Math.PI / 6); // Einstein temperature
            double freqE = kB * tempE / h; // Einstein frequency
            double x = h * freqE / (kB * temp); // 
            double eX = Math.Pow(Math.E, x);
            double C = 3 * R * (x * x * eX / ((eX - 1) * (eX - 1))); // C = heat capcity of solids
            return C;
        }

        private double ensteinsHeatCapacityEquation(double temp, double tempD)
        {
            double tempE = tempD * Math.Cbrt(Math.PI / 6); // Einstein temperature
            double freqE = kB * tempE / h; // Einstein frequency
            double x = h * freqE / (kB * temp); // 
            double eX = Math.Pow(Math.E, x);
            double C = 3 * R * (x * x * eX / ((eX - 1) * (eX - 1))); // C = heat capcity of solids
            return C;
        }


        public void highlightLine()
        {
            line.Color = ScottPlot.Colors.Gray;
            showControls();
        }

        public void unhighlightLine()
        {
            line.Color = setColBasedOnValue();
            hideControls();
        }

        public void hideControls()
        {
            slider.Visible = false;
            if (id == "generic") removeButton.Visible = false;

        }
        public void showControls()
        {
            slider.Visible = true;
            if (id == "generic") removeButton.Visible = true;
        }

        public void Dispose()
        {
            if (slider != null) slider.Dispose();
            if (removeButton != null) removeButton.Dispose();


        }

        private ScottPlot.Color setColBasedOnValue()
        {
            ScottPlot.Color colour;
            double proportion = ((double)(MolarHeatCapcity[(int)highestIndex]-minMolarHC)/ (double)(maxMolarHC-minMolarHC));
            //double scalar = Math.Log10(1+proportion*10);
            if (useScalarForSlidervsProportion)
            {
                double scalar = Math.Pow(50, proportion) / 50;
                if (scalar > 1) scalar = 1;
                colour = new((byte)((1 - scalar) * 255), (byte)50, (byte)((scalar) * 255));
            }
            else
            {
                colour = new((byte)((1 - proportion) * 255), (byte)50, (byte)((proportion) * 255));
            }
               
            //Debug.WriteLine($"MaxHC {maxMolarHC} MinHC {minMolarHC}");
            //Debug.WriteLine(MolarHeatCapcity[(int)highestIndex]-minMolarHC);
            //Debug.WriteLine(maxMolarHC - minMolarHC);
            //Debug.WriteLine($"Scalar: {scalar}");
            //Debug.WriteLine(proportion);
            return colour;
        }
    }
}
