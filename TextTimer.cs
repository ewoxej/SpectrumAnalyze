namespace SpectrumAnalyzer
{
    class TextTimer
    {
        private int seconds;
        public TextTimer( int sec = 0 )
        {
            seconds = sec;
        }
        public void Add( int sec = 1 )
        {
            seconds += sec;
        }
        public void Reset()
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
