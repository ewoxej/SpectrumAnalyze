using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumAnalyzer
{
    class AudioTimer
    {
        private Label timerLbl;
        private System.Threading.Timer timer;
        int seconds;
        public AudioTimer(Label timerLabel)
        {
            timerLbl = timerLabel;
            timerLbl.Text = "00:00";
        }
        public void start()
        {
            TimerCallback tm = new TimerCallback((obj) =>
            {
                seconds++;
                timerLbl.Invoke(new MethodInvoker(delegate { timerLbl.Text = ToString(); }));
            });
            timer = new System.Threading.Timer(tm, 0, 0, 1000);
        }
        public void stop()
        {
            timer.Dispose();
        }
        public void reset()
        {
            seconds = 0;
        }
        public override string ToString()
        {
            int secs = seconds % 60;
            int mins = seconds / 60;
            string strMins = mins.ToString();
            string strSecs = secs.ToString();
            strMins = (strMins.Length < 2) ? "0" + strMins : strMins;
            strSecs = (strSecs.Length < 2) ? "0" + strSecs : strSecs;
            return strMins + ":" + strSecs;
        }
    }
}
