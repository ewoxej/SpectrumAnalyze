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
        FormElementsLogic formLogic;
        public Form1()
        {
            Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Audiofiles"));
            InitializeComponent();
            plotBuilder = new PlotBuilder(plot1);
            plotList = new AdvancedList(listBox1, lbl_name, lbl_frequency);
            audioProc = new AudioProc();
            player = new SoundPlayer();
            formLogic = new FormElementsLogic(rec_btn, stop_btn, btn_open, btn_save, btn_close, btn_play);
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
            if (plotList.currentIndex < 0)
            {
                formLogic.changeState(APP_STATE.NoFile);
            }
        }
        private void Rec_btn_Click(object sender, EventArgs e)
        {
            formLogic.changeState(APP_STATE.Recording);
            textTimer.reset();
            textTimer.start();
            audioProc.StartRecording("unnamed" + String.Format("{0:dd_mm_yy_hh_mm_ss}", DateTime.Now));
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            var newEntity = new PlotEntity
            {
                Name = "unnamed" + String.Format("{0:dd_mm_yy_hh_mm_ss}", DateTime.Now),
                BuildData = audioProc.GetFft(),
                AudioFilePath = audioProc.StopRecording(),
                CreationDate = DateTime.Now,
                Duration = audioProc.getDuration()
            };
            plotList.addPlot(newEntity);
            plotBuilder.build(newEntity);
            formLogic.changeState(APP_STATE.Opened);
            textTimer.stop();
            listBox1.Update();
        }

        private void onProgressChanged(object sender, PlotEventArgs e)
        {
            progressBar_saving.Invoke(new MethodInvoker(delegate { progressBar_saving.Value = e.Progress; }));
        }
        private async void Btn_save_ClickAsync(object sender, EventArgs e)
        {
            PlotDataIO saverObject = new PlotDataIO();
            saverObject.progressChanged += onProgressChanged;
            plotList.setCurrentPlot(await saverObject.Save(plotList.getCurrentPlot()));
        }

        private async void  Btn_open_Click(object sender, EventArgs e)
        {
            PlotDataIO objectRestore = new PlotDataIO();
            objectRestore.progressChanged += onProgressChanged;
            var newEntity = await objectRestore.Restore();
            if (newEntity != null)
            {
                plotList.addPlot(newEntity);
                plotBuilder.build(newEntity);
                formLogic.changeState(APP_STATE.Opened);
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
