using System;
using System.Collections.Generic;

namespace SpectrumAnalyzer
{
    public partial class AudioProc
    {
        private int index;
        private NAudio.Wave.WaveInEvent wvin;
        List<Int16> dataPcm;
        double[] dataFft;
        public AudioProc(int deviceIndex)
        {
            index = deviceIndex;
        }

        private void AudioMonitorInitialize(
                int DeviceIndex, int sampleRate = 32_000,
                int bitRate = 16, int channels = 1,
                int bufferMilliseconds = 50, bool start = true
            )
        {
            if( wvin == null )
            {
                wvin = new NAudio.Wave.WaveInEvent();
                wvin.DeviceNumber = DeviceIndex;
                wvin.WaveFormat = new NAudio.Wave.WaveFormat( sampleRate, bitRate, channels );
                wvin.DataAvailable += OnDataAvailable;
                wvin.BufferMilliseconds = bufferMilliseconds;
                if( start )
                    wvin.StartRecording();
            }
        }


        private void OnDataAvailable( object sender, NAudio.Wave.WaveInEventArgs args )
        {
            int bytesPerSample = wvin.WaveFormat.BitsPerSample / 8;
            int samplesRecorded = args.BytesRecorded / bytesPerSample;
            if( dataPcm == null )
                dataPcm = new List<Int16>();
            for( int i = 0; i < samplesRecorded; i++ )
                dataPcm.Add( BitConverter.ToInt16( args.Buffer, i * bytesPerSample ) );
        }

        private void updateFFT()
        {
            // the PCM size to be analyzed with FFT must be a power of 2
            int fftPoints = 2;
            while( fftPoints * 2 <= dataPcm.Count )
                fftPoints *= 2;

            // apply a Hamming window function as we load the FFT array then calculate the FFT
            NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
            for( int i = 0; i < fftPoints; i++ )
                fftFull[i].X = (float)(dataPcm[i] * NAudio.Dsp.FastFourierTransform.HammingWindow( i, fftPoints ));
            NAudio.Dsp.FastFourierTransform.FFT( true, (int)Math.Log( fftPoints, 2.0 ), fftFull );

            // copy the complex values into the double array that will be plotted
            if( dataFft == null )
                dataFft = new double[fftPoints / 2];
            for( int i = 0; i < fftPoints / 2; i++ )
            {
                double fftLeft = Math.Abs( fftFull[i].X + fftFull[i].Y );
                double fftRight = Math.Abs( fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y );
                dataFft[i] = fftLeft + fftRight;
            }
        }

        public void startRecording()
        {
            AudioMonitorInitialize( index );
            if( dataPcm != null )
                dataPcm.Clear();
            dataFft = null;
        }

        public double[] getFft()
        {
            if( dataFft == null )
                updateFFT();
            return dataFft;
        }
        public double getSampleRate()
        {
            return wvin.WaveFormat.SampleRate;
        }
        public void stopRecording()
        {
            if( dataPcm == null )
                return;
            updateFFT();
            if( wvin != null )
            {
                wvin.StopRecording();
                wvin = null;
            }
        }
    }
}
