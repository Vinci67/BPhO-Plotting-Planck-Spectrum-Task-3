using MathNet.Numerics;
using Microsoft.VisualBasic.Devices;
using ScottPlot;
using ScottPlot.AxisLimitManagers;
using ScottPlot.Plottables;
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
        public ScottPlot.Plottables.Crosshair crosshair;
        private ScottPlot.Plottables.Scatter graph1;
        private bool rbNearestXY = false; // set to false if want to make crosshair always visible, set to true if only want it visible near the line
        private List<GraphLine> graphs = new List<GraphLine>();
        private bool toggleForMouse = false;
        private Random rand = new Random();
        public Form1()
        {
            
            InitializeComponent();
            graphs.Add(generateLine(minW, maxW, 4000, ScottPlot.Colors.Blue));
            graphs.Add(generateLine(minW, maxW, 5000, ScottPlot.Colors.Orange));
            graphs.Add(generateLine(minW, maxW, 6000, ScottPlot.Colors.Red));
            graphs.Add(generateLine(minW, maxW, 8000, Colors.Green));
            formsPlot1.Plot.XLabel("Wavelength / nm");
            formsPlot1.Plot.YLabel("Irradiance / Wm^-2/nm     x10^4");
            formsPlot1.Plot.Title("Solar Irradiance vs Wavelength");
            
            formsPlot1.Refresh();
            crosshair = formsPlot1.Plot.Add.Crosshair(10, 10);
            crosshair.IsVisible = true;
            crosshair.MarkerShape = MarkerShape.OpenCircle;
            crosshair.MarkerSize = 15;
            formsPlot1.MouseMove += (s, e) =>
            {
                
                Pixel mousePixel = new(e.Location.X, e.Location.Y);
                Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
                DataPoint closestPoint = DataPoint.None;
                double closestDistance2 = double.MaxValue;
                int specificGraphI = 0;
                for (int i = 0; i < graphs.Count; i++) 
                {
                    if (toggleForMouse == false)
                    {
                        graphs[i].slider.Visible = false;
                        graphs[i].unhighlightLine();
                    }
                    DataPoint nearest = rbNearestXY
                       ? graphs[i].line.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender)
                       : graphs[i].line.Data.GetNearestX(mouseLocation, formsPlot1.Plot.LastRender);
                    double distance2 = Math.Pow(nearest.X - mouseLocation.X,2) + Math.Pow(nearest.Y - mouseLocation.Y,2);
                    if (distance2 < closestDistance2)
                    {
                        specificGraphI = i;
                        closestDistance2 = distance2;
                        closestPoint = nearest;
                    }
                }
               
                
                if (closestPoint.IsReal)
                {
                    if (toggleForMouse == false)
                    {
                        graphs[specificGraphI].highlightLine();
                        graphs[specificGraphI].slider.Visible = true;
                        crosshair.IsVisible = true;
                        crosshair.Position = closestPoint.Coordinates;
                        formsPlot1.Refresh();
                        Text = $"Wavelength={closestPoint.X:0.##}nm, Y={closestPoint.Y * 1E4:0.##}Wm^-2/nm";//Selected Index={closestPoint.Index},
                    }
                    

                }
                if (!closestPoint.IsReal && crosshair.IsVisible)
                {
                    crosshair.IsVisible = false;
                    formsPlot1.Refresh();
                    Text = $"No point selected";
                }
            };
            
            formsPlot1.MouseDown += (s, e) =>
            {
                textBox1.Visible = false;
                if (toggleForMouse)
                {
                    toggleForMouse = false;
                }
                else toggleForMouse = true;

            };

            buttonAddLine.Click += (s, e) =>
            {
                
                graphs.Add(generateLine(minW, maxW, rand.Next(1,100)*100, ScottPlot.Color.RandomHue()));
            };
            
            
        }

        public double plankSpectrumEquation(double wavelength, double temp)
        {
            //Should return spectural Irradiance 
            return 1E-13*Math.PI*(2 * h * c * c) / (Math.Pow(wavelength,5)) * (1 / (Math.Exp(h * c / (wavelength * kB * temp))-1.0)); // return in nm *E4
        }

        private GraphLine generateLine(double minWavelength, double maxWavelength, double temp, ScottPlot.Color color)
        {
            GraphLine line = new GraphLine(minWavelength, maxWavelength, temp, color, this, formsPlot1);
            
            return line;
        }
    }
}
