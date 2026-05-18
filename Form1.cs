using ScottPlot;
using MathNet.Numerics;
using System.Diagnostics;
using System.Numerics;
namespace BPhO__Plotting_Planck_Spectrum_Task_3
{
    
    public partial class Form1 : Form
    {
        public double h = 6.626E-34;
        public double kB = 1.381E-23;
        public double c = 2.998E8;
        public double minW = 200E-9;
        public double maxW = 2500E-9;
        public Form1()
        {
            
            InitializeComponent();
            generatePlot(minW, maxW, 4000, ScottPlot.Colors.Blue);
            generatePlot(minW, maxW, 5000, ScottPlot.Colors.Orange);
            generatePlot(minW, maxW, 6000, ScottPlot.Colors.Red);
            formsPlot1.Plot.XLabel("Wavelength / nm");
            formsPlot1.Plot.YLabel("Irradiance / Wm^-2/nm     x10^4");
            formsPlot1.Plot.Title("Solar Irradiance vs Wavelength");
            formsPlot1.Refresh();
            ScottPlot.Coordinates coords = formsPlot1.Plot.GetCoordinates(new Pixel(400, 9));
            formsPlot1.Plot.XLabel(Convert.ToString(coords));
            formsPlot1.Refresh();
            
        }

        public double plankSpectrumEquation(double wavelength, double temp)
        {
            //Should return spectural Irradiance 
            return 1E-13*Math.PI*(2 * h * c * c) / (Math.Pow(wavelength,5)) * (1 / (Math.Exp(h * c / (wavelength * kB * temp))-1.0)); // return in nm *E4
        }

        private void generatePlot(double minWavelength, double maxWavelength, double temp, ScottPlot.Color color)
        {
            double[] dataWalength = MathNet.Numerics.Generate.LinearSpaced(1000, minWavelength, maxWavelength);
            double[] dataWavelengthPlotting = MathNet.Numerics.Generate.LinearSpaced(1000, minWavelength*1E9, maxWavelength*1E9); // l in nm
            double[] irradiance = new double[dataWalength.Length];

            for (int i = 0; i < dataWalength.Length; i++)
            {
                irradiance[i] = plankSpectrumEquation(dataWalength[i], temp);
                Debug.WriteLine(irradiance[i]);
            }

            var line = formsPlot1.Plot.Add.Scatter(dataWavelengthPlotting, irradiance);
            line.Color = color;
            line.Label = Convert.ToString(temp);
            formsPlot1.Refresh();
        }
    }
}
