using System;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace SpectrumAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            plot = new SpectrumPlot( plot1, listBox1 );
            audioProc = new AudioProc( 0 );
            textTimer = new TextTimer();
            unnamedIndex = 0;
        }
        private void ListBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            CurrentPlot = listBox1.GetItemText( listBox1.SelectedItem );
        }

        private void Btn_close_Click( object sender, EventArgs e )
        {
            plot.RemovePlot( m_currentPlot );
        }
        private void Rec_btn_Click( object sender, EventArgs e )
        {
            stop_btn.Enabled = true;
            rec_btn.Enabled = false;
            TimerCallback tm = new TimerCallback( ( obj ) =>
            {
                textTimer.add();
                lbl_timer.Invoke( new MethodInvoker( delegate { lbl_timer.Text = textTimer.ToString(); } ) );
                plot.SetSampleRate( audioProc.getSampleRate() );
            } );
            timer = new Timer( tm, 0, 0, 1000 );
            audioProc.startRecording();
        }

        private void Stop_btn_Click( object sender, EventArgs e )
        {
            stop_btn.Enabled = false;
            rec_btn.Enabled = true;
            timer.Dispose();
            audioProc.stopRecording();
            textTimer.reset();
            var vals = audioProc.getFft();
            plot.AddPlot( vals, "unnamed" + unnamedIndex );
            unnamedIndex++;
        }

        private void Btn_save_Click( object sender, EventArgs e )
        {
            var data = plot.GetData( m_currentPlot );
            string fileName = PlotDataIO.Save( data );
            changeSavedFileName( fileName );
        }

        private void Btn_open_Click( object sender, EventArgs e )
        {
            double[] data = null;
            string filename = PlotDataIO.Restore( ref data );
            plot.AddPlot( data, filename );
        }
        private void changeSavedFileName( string newName )
        {
            if( newName == null ) return;
            var index = listBox1.Items.IndexOf( CurrentPlot );
            plot.RenamePlot( CurrentPlot, newName );
            listBox1.Items[index] = newName;
            CurrentPlot = newName;

        }

        private SpectrumPlot plot;
        private AudioProc audioProc;
        private Timer timer;
        private TextTimer textTimer;
        private string m_currentPlot;
        private int unnamedIndex;
        public string CurrentPlot
        {
            get { return m_currentPlot; }
            set
            {
                m_currentPlot = value;
                listBox1.SelectedIndex = listBox1.Items.IndexOf( m_currentPlot );
                lbl_name.Text = m_currentPlot;
                if( m_currentPlot.Contains( "unnamed" ) )
                    btn_save.Enabled = true;
                else
                    btn_save.Enabled = false;
            }
        }
    }

}
