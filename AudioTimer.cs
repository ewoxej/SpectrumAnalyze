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
        public delegate void TimerHandler();
        public event TimerHandler stopped;
        int totalDuration = -1;
        public AudioTimer(Label timerLabel)
        {
            timerLbl = timerLabel;
            timerLbl.Text = "00:00";
        }
        public void start(int duration=-1)
        {
            totalDuration = duration;
            TimerCallback tm = new TimerCallback((obj) =>
            {
                seconds++;
                timerLbl.Invoke(new MethodInvoker(delegate { timerLbl.Text = ToString(); }));
                if (totalDuration!=-1 && seconds > totalDuration) stop();
            });
            timer = new System.Threading.Timer(tm, 0, 0, 1000);
        }
        public void stop()
        {
            timer.Dispose();
            stopped?.Invoke();
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
