using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace SpectrumAnalyzer
{
    
    public partial class Form1 : Form
    {
        AdvancedList plotList;
        PlotBuilder plotBuilder;
        public Form1()
        {
            InitializeComponent();
            plotBuilder = new PlotBuilder(plot1);
            plotList = new AdvancedList(listBox1, lbl_name);
            audioProc = new AudioProc();
            player = new SoundPlayer();
            FormClosing += onClosing;
            var devices = audioProc.ScanSoundCards();
            foreach (var i in devices)
            {
                comboBox1.Items.Add(i);
            }
            if (devices.Count != 0)
            {
                comboBox1.SelectedIndex = 0;
                audioProc.DeviceIndex = comboBox1.SelectedIndex;
                rec_btn.Enabled = true;
            }
            textTimer = new AudioTimer(lbl_timer);
        }

        private void Btn_close_Click(object sender, EventArgs e)
        {
            plotBuilder.remove(plotList.currentIndex);
            plotList.removePlot();
            if(plotList.currentIndex<0)
            {
                btn_play.Enabled = false;
                btn_save.Enabled = false;
                btn_close.Enabled = false;
            }
        }
        private void Rec_btn_Click(object sender, EventArgs e)
        {
            stop_btn.Enabled = true;
            rec_btn.Enabled = false;
            textTimer.reset();
            textTimer.start();
            audioProc.StartRecording("unnamed"+ String.Format("{0:dd_mm_yy_hh_mm_ss}",DateTime.Now));
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            var newEntity = new PlotEntity
            {
                Name = "unnamed"+ String.Format("{0:dd_mm_yy_hh_mm_ss}", DateTime.Now),
                BuildData = audioProc.GetFft(),
                AudioFilePath = audioProc.StopRecording(),
                CreationDate = DateTime.Now,
                Duration = audioProc.getDuration()
            };
            plotList.addPlot(newEntity);
            plotBuilder.build(newEntity);
            stop_btn.Enabled = false;
            rec_btn.Enabled = true;
            btn_play.Enabled = true;
            btn_save.Enabled = true;
            btn_close.Enabled = true;
            textTimer.stop();
            listBox1.Update();
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            plotList.setCurrentPlot(PlotDataIO.Save(plotList.getCurrentPlot()));
        }

        private void Btn_open_Click(object sender, EventArgs e)
        {
            var newEntity = PlotDataIO.Restore();
            if (newEntity != null)
            {
                plotList.addPlot(newEntity);
                plotBuilder.build(newEntity);
                btn_play.Enabled = true;
                btn_save.Enabled = true;
            }
        }

        private readonly AudioProc audioProc;
        private SoundPlayer player;
        private AudioTimer textTimer;


        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            audioProc.DeviceIndex = comboBox1.SelectedIndex;
        }

        private void Btn_play_Click(object sender, EventArgs e)
        {
            if (btn_play.Text == "Stop")
            {
                textTimer.stop();
                player.Stop();
                btn_play.Text = "Play";
            }
            else
            {
                var outputFolder = Path.Combine(Path.GetTempPath(), "Audiofiles");

                player.SoundLocation = plotList.getCurrentPlot().AudioFilePath;
  
                btn_play.Text = "Stop";
                textTimer.reset();
                textTimer.stopped += (() => btn_play.Invoke(new MethodInvoker(delegate { btn_play.Text = "Play"; })));
                textTimer.start(Convert.ToInt32(plotList.getCurrentPlot().Duration));
                player.Play();
            }
        }
        public void onClosing(object sender, EventArgs e)
        {
            var path = Path.Combine(Path.GetTempPath(), "Audiofiles");
            if(Directory.Exists(path))
            Directory.Delete(path,true);
        }

    }
}
