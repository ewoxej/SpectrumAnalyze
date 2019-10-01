using System.Collections.Generic;
using System.Windows.Forms;
using ScottPlot;
using System.Drawing;
using System;
namespace SpectrumAnalyzer
{

    public class SpectrumPlot
    {
        public SpectrumPlot( FormsPlot argPlot, ListBox lbox )
        {
            plot = argPlot;
            listBox = lbox;
            SRate = defaultSampleRate;
            plots = new Dictionary<string, double[]>();
            plot.plt.Clear();
            plot.plt.YLabel( "Power (db)" );
            plot.plt.XLabel( "Frequency (kHz)" );
            plot.plt.PlotHLine( 0, color: Color.Black, lineWidth: 1 );
            plot.plt.AxisAuto();
            plot.plt.TightenLayout();
        }
        public void AddPlot( double[] vals, string plotName )
        {
            try
            {
                plots.Add( plotName, vals );
            }
            catch( ArgumentException )
            {
                MessageBox.Show( "This file already opened" );
                return;
            }
            listBox.Items.Add( plotName );
            listBox.SetSelected( listBox.Items.Count - 1, true );
            double fftSpacing = SRate / vals.Length;
            plot.plt.PlotSignal( vals, sampleRate: fftSpacing, markerSize: 0 );
            plot.Render();
        }

        public int CalculatePeakFrequency( string plotName )
        {
            if( plotName == "" ) return 0;
            double max = 0;
            foreach( var i in plots )
            {
                var a = i.Value;
            }
            var array = plots[plotName];
            int index = 0;
            foreach( var i in array )
            {
                if( i > max ) max = i;
                index++;
            }
            return index;
        }
        public void RemovePlot( string plotName )
        {
            if( plotName == null ) return;
            plot.plt.Clear();
            listBox.Items.Remove( plotName );
            if( listBox.Items.Count != 0 )
                listBox.SetSelected( listBox.Items.Count - 1, true );
            if( plotName == null ) return;
            plots.Remove( plotName );
            plot.plt.PlotHLine( 0, color: Color.Black, lineWidth: 1 );

            foreach( var i in plots )
            {
                double fftSpacing = SRate / i.Value.Length;
                plot.plt.PlotSignal( i.Value, sampleRate: fftSpacing, markerSize: 0 );
            }
            plot.Render();
        }
        public double[] GetData( string key )
        {
            return plots[key];
        }
        public void RenamePlot( string oldname, string newname )
        {
            var oldvalue = plots[oldname];
            plots.Add( newname, oldvalue );
            plots.Remove( oldname );
        }

        private readonly FormsPlot plot;
        private readonly ListBox listBox;
        private readonly Dictionary<string, double[]> plots;
        public double SRate { get; set; }
        private const double defaultSampleRate = 32000;

    }
}