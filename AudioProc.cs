using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpectrumAnalyzer
{
    public class AudioProc
    {
        public int DeviceIndex { get; set; }
        private NAudio.Wave.WaveInEvent wvEvent;
        private WaveFileWriter waveFile = null;
        List<Int16> dataPcm;
        private String outputFilePath;
        double[] dataFft;
        DateTime begin, end;
        private void AudioMonitorInitialize(
                int DeviceIndex, int sampleRate = 32_000,
                int bitRate = 16, int channels = 1,
                int bufferMilliseconds = 50, bool start = true
            )
        {
            if ( wvEvent == null )
            {
                wvEvent = new NAudio.Wave.WaveInEvent
                {
                    DeviceNumber = DeviceIndex,
                    WaveFormat = new NAudio.Wave.WaveFormat( sampleRate, bitRate, channels )
                };
                wvEvent.DataAvailable += OnDataAvailable;
                wvEvent.BufferMilliseconds = bufferMilliseconds;
                if( start )
                    wvEvent.StartRecording();
            }
        }

        public List<string> ScanSoundCards()
        {
            List<string> list = new List<string>();
            for( int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++ )
                list.Add( NAudio.Wave.WaveIn.GetCapabilities( i ).ProductName );
            return list;
        }
        private void OnDataAvailable( object sender, NAudio.Wave.WaveInEventArgs args )
        {
            waveFile.Write(args.Buffer, 0, args.BytesRecorded);
            int bytesPerSample = wvEvent.WaveFormat.BitsPerSample / 8;
            int samplesRecorded = args.BytesRecorded / bytesPerSample;
            if( dataPcm == null )
                dataPcm = new List<Int16>();
            for( int i = 0; i < samplesRecorded; i++ )
                dataPcm.Add( BitConverter.ToInt16( args.Buffer, i * bytesPerSample ) );
        }

        private void UpdateFFT()
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

        public void StartRecording(String fileName)
        {
            var outputFolder = Path.Combine(Path.GetTempPath(), "Audiofiles");
            Directory.CreateDirectory(outputFolder);
            outputFilePath = Path.Combine(outputFolder, fileName+".wav");
            waveFile = new WaveFileWriter(outputFilePath, new NAudio.Wave.WaveFormat(32_000, 16, 1));
            AudioMonitorInitialize( DeviceIndex );
            if ( dataPcm != null )
                dataPcm.Clear();
            dataFft = null;
            begin = DateTime.Now;
        }

        public double[] GetFft()
        {
            if( dataFft == null )
                UpdateFFT();
            return dataFft;
        }
        public double GetSampleRate()
        {
            return wvEvent.WaveFormat.SampleRate;
        }
        public double getDuration()
        {
            return (end - begin).TotalSeconds;
        }
        public String StopRecording()
        {
            end = DateTime.Now;
            if (dataPcm == null)
                return string.Empty;
            UpdateFFT();
            if( wvEvent != null )
            {
                wvEvent.StopRecording();
                wvEvent = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
            return outputFilePath;
        }
    }
}
