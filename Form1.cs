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
        List<PlotEntity> plots;
        PlotBuilder plotBuilder;
        public Form1()
        {
            InitializeComponent();
            plotBuilder = new PlotBuilder(plot1);
            plots = new List<PlotEntity>();
            audioProc = new AudioProc();
            player = new SoundPlayer();
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
            currentIndex = 0;
        }
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_name.Text = plots[currentIndex].Name;
        }

        private void Btn_close_Click(object sender, EventArgs e)
        {
            plotBuilder.remove(currentIndex);
        }
        private void Rec_btn_Click(object sender, EventArgs e)
        {
            stop_btn.Enabled = true;
            rec_btn.Enabled = false;
            textTimer.reset();
            textTimer.start();
            audioProc.StartRecording("unnamed"+DateTime.Now.ToString());
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            var newEntity = new PlotEntity
            {
                BuildData = audioProc.GetFft(),
                AudioFilePath = audioProc.StopRecording(),
                CreationDate = DateTime.Now,
                Duration = audioProc.getDuration()
            };
            plots.Add(newEntity);
            currentIndex++;
            plotBuilder.build(newEntity);
            stop_btn.Enabled = false;
            rec_btn.Enabled = true;
            textTimer.stop();
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            plots[currentIndex] = PlotDataIO.Save(plots[currentIndex]);
        }

        private void Btn_open_Click(object sender, EventArgs e)
        {
            var newEntity = PlotDataIO.Restore();
            plots.Add(newEntity);
            plotBuilder.build(newEntity);
        }

        private readonly AudioProc audioProc;
        private SoundPlayer player;
        private AudioTimer textTimer;
        private int currentIndex;


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

                player.SoundLocation = plots[currentIndex].AudioFilePath;
  
                btn_play.Text = "Stop";
                textTimer.reset();
                textTimer.start();
                player.Play();
            }
        }

    }
}
