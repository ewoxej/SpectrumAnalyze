using System.Collections.Generic;
using System.Windows.Forms;
using ScottPlot;
using System.Drawing;

public class SpectrumPlot
{
    public SpectrumPlot( FormsPlot chart, ListBox lbox )
    {
        m_chart = chart;
        listBox = lbox;
        sRate = 32000;
        plots = new Dictionary<string, double[]>();
        m_chart.plt.Clear();
        m_chart.plt.YLabel( "Power (db)" );
        m_chart.plt.XLabel( "Frequency (kHz)" );
        m_chart.plt.PlotHLine( 0, color: Color.Black, lineWidth: 1 );
        m_chart.plt.AxisAuto();
        m_chart.plt.TightenLayout();
    }
    public void AddPlot( double[] vals, string plotName )
    {
        plots.Add( plotName, vals );
        listBox.Items.Add( plotName );
        listBox.SetSelected( listBox.Items.Count-1, true );
        double fftSpacing = sRate / vals.Length;
        m_chart.plt.PlotSignal( vals, sampleRate: fftSpacing, markerSize: 0 );
        m_chart.Render();
    }

    public void RemovePlot( string plotName )
    {
        m_chart.plt.Clear();
        listBox.Items.Remove( plotName );
        listBox.SetSelected( listBox.Items.Count - 1, true );
        plots.Remove( plotName );
        m_chart.plt.PlotHLine( 0, color: Color.Black, lineWidth: 1 );

        foreach(var i in plots)
        {
            double fftSpacing = sRate / i.Value.Length;
            m_chart.plt.PlotSignal( i.Value, sampleRate: fftSpacing, markerSize: 0 );
        }
        m_chart.Render();
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

    private FormsPlot m_chart;
    private ListBox listBox;
    private Dictionary<string, double[]> plots;
    private double sRate;
    public void SetSampleRate(double rate)
    {
        sRate = rate;
    }
}