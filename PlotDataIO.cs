using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SpectrumAnalyzer
{
    public class PlotEventArgs
    {
        public PlotEventArgs(int s) { Progress = s; }
        public int Progress { get; } // readonly
    }

    class PlotDataIO
    {
        public delegate void PlotIOEventHandler(object sender, PlotEventArgs e);

        public event PlotIOEventHandler progressChanged;
        public async Task<PlotEntity> Save(PlotEntity entity )
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                RestoreDirectory = true
            };
            string str = null;

            if ( saveFileDialog1.ShowDialog() == DialogResult.OK )
            {
                if( (myStream = saveFileDialog1.OpenFile()) != null )
                {
                    TextWriter tw = new StreamWriter( myStream );
                    str = saveFileDialog1.FileName;
                    entity.Name = str;
                    foreach( double s in entity.BuildData )
                        tw.WriteLine( s.ToString() );
                    tw.Close();
                    myStream.Close();
                }
            }
            progressChanged(this, new PlotEventArgs(10));
            DataContractSerializer dcs = new DataContractSerializer(typeof(PlotEntity));
            using (Stream stream = new FileStream(str + ".xml", FileMode.Create, FileAccess.Write))
            {
                using (XmlDictionaryWriter writer =
                    XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    dcs.WriteObject(writer, entity);
                }
            }
            progressChanged(this, new PlotEventArgs(30));
            var zipFile = ZipFile.Open(str+".zip", ZipArchiveMode.Create);
            progressChanged(this, new PlotEventArgs(40));
            zipFile.CreateEntryFromFile(str+".xml", "meta.xml");
            progressChanged(this, new PlotEventArgs(50));
            zipFile.CreateEntryFromFile(entity.AudioFilePath, "audio.wav");
            progressChanged(this, new PlotEventArgs(70));
            zipFile.Dispose();
            File.Delete(str);
            File.Delete(str + ".xml");
            File.Delete(entity.AudioFilePath);
            progressChanged(this, new PlotEventArgs(100));
            progressChanged(this, new PlotEventArgs(0));
            return entity;
        }

    public async Task<PlotEntity> Restore()
    {
            PlotEntity entity = new PlotEntity();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if ( openFileDialog1.ShowDialog() == DialogResult.Cancel )
            return null;
        string filename = openFileDialog1.FileName;
            using (ZipArchive zip = ZipFile.Open(filename, ZipArchiveMode.Read))
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.Name == "meta.xml")
                    {
                        entry.ExtractToFile("meta.xml",true);
                    }
                }
            progressChanged(this, new PlotEventArgs(10));
            DataContractSerializer dcs = new DataContractSerializer(typeof(PlotEntity));
            using (Stream stream = new FileStream("meta.xml", FileMode.Open,FileAccess.ReadWrite))
            {
                var xmlQuotas = new XmlDictionaryReaderQuotas();
                xmlQuotas.MaxArrayLength = 32768;//костыль
                using (XmlDictionaryReader xmlreader =
                    XmlDictionaryReader.CreateTextReader(stream, xmlQuotas))
                {
                    entity = (PlotEntity)dcs.ReadObject(xmlreader);
                }
            }
            progressChanged(this, new PlotEventArgs(50));
            using (ZipArchive zip = ZipFile.Open(filename, ZipArchiveMode.Read))
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.Name == "audio.wav")
                    {
                        entity.AudioFilePath = Path.Combine(Path.Combine(Path.GetTempPath(),"Audiofiles"), "audio" + String.Format("{0:dd_mm_yy_hh_mm_ss}", entity.CreationDate));
                        entry.ExtractToFile(entity.AudioFilePath,true);
                    }
                }
            File.Delete("meta.xml");
            progressChanged(this, new PlotEventArgs(100));
            progressChanged(this, new PlotEventArgs(0));
            return entity;
    }
}
}
