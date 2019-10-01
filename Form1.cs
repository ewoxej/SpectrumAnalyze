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
            audioProc = new AudioProc();
            var devices = audioProc.ScanSoundCards();
            foreach(var i in devices)
            {
                comboBox1.Items.Add( i );
            }
            if( devices.Count != 0 )
            {
                comboBox1.SelectedIndex = 0;
                audioProc.DeviceIndex = comboBox1.SelectedIndex;
                rec_btn.Enabled = true;
            }
            textTimer = new TextTimer();
            unnamedIndex = 0;
        }
        private void ListBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            CurrentPlot = listBox1.GetItemText( listBox1.SelectedItem );
        }

        private void Btn_close_Click( object sender, EventArgs e )
        {
            plot.RemovePlot( currentPlot );
        }
        private void Rec_btn_Click( object sender, EventArgs e )
        {
            stop_btn.Enabled = true;
            rec_btn.Enabled = false;
            TimerCallback tm = new TimerCallback( ( obj ) =>
            {
                textTimer.Add();
                lbl_timer.Invoke( new MethodInvoker( delegate { lbl_timer.Text = textTimer.ToString(); } ) );
                plot.SRate = audioProc.GetSampleRate();
            } );
            timer = new Timer( tm, 0, 0, 1000 );
            audioProc.StartRecording();
        }

        private void Stop_btn_Click( object sender, EventArgs e )
        {
            stop_btn.Enabled = false;
            rec_btn.Enabled = true;
            timer.Dispose();
            audioProc.StopRecording();
            textTimer.Reset();
            var vals = audioProc.GetFft();
            plot.AddPlot( vals, "unnamed" + unnamedIndex );
            unnamedIndex++;
        }

        private void Btn_save_Click( object sender, EventArgs e )
        {
            var data = plot.GetData( currentPlot );
            string fileName = PlotDataIO.Save( data );
            ChangeSavedFileName( fileName );
        }

        private void Btn_open_Click( object sender, EventArgs e )
        {
            double[] data = null;
            string filename = PlotDataIO.Restore( ref data );
            plot.AddPlot( data, filename );
        }
        private void ChangeSavedFileName( string newName )
        {
            if( newName == null ) return;
            var index = listBox1.Items.IndexOf( CurrentPlot );
            plot.RenamePlot( CurrentPlot, newName );
            listBox1.Items[index] = newName;
            CurrentPlot = newName;

        }

        private readonly SpectrumPlot plot;
        private readonly AudioProc audioProc;
        private Timer timer;
        private readonly TextTimer textTimer;
        private string currentPlot;
        private int unnamedIndex;
        public string CurrentPlot
        {
            get { return currentPlot; }
            set
            {
                currentPlot = value;
                label1.Text = "Peak frequency: " + plot.CalculatePeakFrequency( currentPlot );
                listBox1.SelectedIndex = listBox1.Items.IndexOf( currentPlot );
                lbl_name.Text = currentPlot;
                if( currentPlot.Contains( "unnamed" ) )
                    btn_save.Enabled = true;
                else
                    btn_save.Enabled = false;
            }
        }

        private void ComboBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            audioProc.DeviceIndex = comboBox1.SelectedIndex;
        }
    }

}
