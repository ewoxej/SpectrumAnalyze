using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SpectrumAnalyzer
{
    class PlotDataIO
    {
        public static PlotEntity Save(PlotEntity entity )
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                RestoreDirectory = true
            };
            string str = null;
            if( saveFileDialog1.ShowDialog() == DialogResult.OK )
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
            var zipFile = ZipFile.Open(str+".zip", ZipArchiveMode.Create);
            zipFile.CreateEntryFromFile(str+".xml", "meta.xml");
            zipFile.CreateEntryFromFile(entity.AudioFilePath, "audio.wav");
            zipFile.Dispose();
            File.Delete(str);
            File.Delete(str + ".xml");
            File.Delete(entity.AudioFilePath);
            return entity;
        }

    public static PlotEntity Restore()
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
                        File.Delete("meta.xml");
                        entry.ExtractToFile("meta.xml");
                    }
                }
            DataContractSerializer dcs = new DataContractSerializer(typeof(PlotEntity));
            using (Stream stream = new FileStream("meta.xml", FileMode.Open,FileAccess.ReadWrite))
            {
                using (XmlDictionaryReader xmlreader =
                    XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()))
                {
                    entity = (PlotEntity)dcs.ReadObject(xmlreader);
                }
            }
            using (ZipArchive zip = ZipFile.Open(filename, ZipArchiveMode.Read))
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.Name == "audio.wav")
                    {
                        File.Delete(entity.AudioFilePath);
                        entry.ExtractToFile(entity.AudioFilePath);
                    }
                }
            File.Delete("meta.xml");
            return entity;
    }
}
}
