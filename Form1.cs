using System;
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
        public Form1()
        {
            InitializeComponent();
            plot = new SpectrumPlot(plot1, listBox1);
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
            textTimer = new TextTimer();
            unnamedIndex = 0;
        }
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPlot = listBox1.GetItemText(listBox1.SelectedItem);
        }

        private void Btn_close_Click(object sender, EventArgs e)
        {
            plot.RemovePlot(currentPlot);
        }
        private void Rec_btn_Click(object sender, EventArgs e)
        {
            stop_btn.Enabled = true;
            rec_btn.Enabled = false;
            TimerCallback tm = new TimerCallback((obj) =>
           {
               textTimer.Add();
               lbl_timer.Invoke(new MethodInvoker(delegate { lbl_timer.Text = textTimer.ToString(); }));
               plot.SRate = audioProc.GetSampleRate();
           });
            timer = new Timer(tm, 0, 0, 1000);
            audioProc.StartRecording(unnamedIndex);
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            stop_btn.Enabled = false;
            rec_btn.Enabled = true;
            timer.Dispose();
            audioProc.StopRecording();
            textTimer.Reset();
            var vals = audioProc.GetFft();
            plot.AddPlot(vals, "unnamed" + unnamedIndex);
            unnamedIndex++;
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            var data = plot.GetData(currentPlot);
            string fileName = PlotDataIO.Save(data, currentPlot);
            ChangeSavedFileName(fileName);
        }

        private void Btn_open_Click(object sender, EventArgs e)
        {
            double[] data = null;
            string filename = PlotDataIO.Restore(ref data);
            plot.AddPlot(data, filename);
        }
        private void ChangeSavedFileName(string newName)
        {
            if (newName == null) return;
            var index = listBox1.Items.IndexOf(CurrentPlot);
            plot.RenamePlot(CurrentPlot, newName);
            listBox1.Items[index] = newName;
            CurrentPlot = newName;

        }

        private readonly SpectrumPlot plot;
        private readonly AudioProc audioProc;
        private SoundPlayer player;
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
                label1.Text = "Peak frequency: " + plot.CalculatePeakFrequency(currentPlot);
                listBox1.SelectedIndex = listBox1.Items.IndexOf(currentPlot);
                lbl_name.Text = currentPlot;
                btn_play.Enabled = true;
                if (currentPlot.Contains("unnamed"))
                    btn_save.Enabled = true;
                else
                    btn_save.Enabled = false;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            audioProc.DeviceIndex = comboBox1.SelectedIndex;
        }

        private void Btn_play_Click(object sender, EventArgs e)
        {
            if (btn_play.Text == "Stop")
            {
                timer.Dispose();
                player.Stop();
                btn_play.Text = "Play";
            }
            else
            {
                var outputFolder = Path.Combine(Path.GetTempPath(), "Audiofiles");
                if (!currentPlot.Contains("unnamed"))
                {
                    var filename = currentPlot;
                    using (ZipArchive zip = ZipFile.Open(filename, ZipArchiveMode.Read))
                        foreach (ZipArchiveEntry entry in zip.Entries)
                            if (entry.Name == "audio.wav")
                            {
                                entry.ExtractToFile(Path.Combine(outputFolder, "audio.wav"), true);
                            }
                    player.SoundLocation = Path.Combine(outputFolder, "audio.wav");
                }
                else
                {
                    player.SoundLocation = Path.Combine(outputFolder, currentPlot + ".wav");
                }
                btn_play.Text = "Stop";
                textTimer.Reset();
                TimerCallback tm = new TimerCallback((obj) =>
                {
                    textTimer.Add();
                    lbl_timer.Invoke(new MethodInvoker(delegate { lbl_timer.Text = textTimer.ToString(); }));
                });
                timer = new Timer(tm, 0, 0, 1000);
                player.Play();
            }
        }

        //void playAudio()
        //{
        //    player.PlaySync();
        //    btn_play.Invoke(new Action(() => { btn_play.Text = "Play"; }));
        //}
    }
}
