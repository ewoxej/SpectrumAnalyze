using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumAnalyzer
{
    class PlotBuilder
    {
        public PlotBuilder(FormsPlot argPlot)
        {
            plot = argPlot;
            plots = new List<double[]>();
            plot.plt.Clear();
            plot.plt.YLabel("Power (db)");
            plot.plt.XLabel("Frequency (kHz)");
            plot.plt.PlotHLine(0, color: Color.Black, lineWidth: 1);
            plot.plt.AxisAuto();
            plot.plt.TightenLayout();
        }
        public void build(PlotEntity entity)
        {
           plots.Add(entity.BuildData);
           double fftSpacing = 32000 / entity.BuildData.Length;
           plot.plt.PlotSignal(entity.BuildData, sampleRate: fftSpacing, markerSize: 0);
           plot.Render();
        }
        public void remove(int entityIndex)
        {
            plot.plt.Clear();
            plots.RemoveAt(entityIndex);
            plot.plt.PlotHLine(0, color: Color.Black, lineWidth: 1);
            foreach (var i in plots)
            {
                double fftSpacing = 32000 / i.Length;
                plot.plt.PlotSignal(i, sampleRate: fftSpacing, markerSize: 0);
            }
            plot.Render();
        }
        private List<double[]> plots;
        private FormsPlot plot;
    }
}
