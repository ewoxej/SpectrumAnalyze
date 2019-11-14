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
        public static string Save( double[] dataArray,String number,String duration )
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
                    foreach( double s in dataArray )
                        tw.WriteLine( s.ToString() );
                    tw.Close();
                    myStream.Close();
                }
            }
            MetaData info = new MetaData();
            info.DateOfCreation = DateTime.Now;
            info.originName = number.ToString();
            info.Duration = duration;
            //fill info
            DataContractSerializer dcs = new DataContractSerializer(typeof(MetaData));
            using (Stream stream = new FileStream(str + ".xml", FileMode.Create, FileAccess.Write))
            {
                using (XmlDictionaryWriter writer =
                    XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    dcs.WriteObject(writer, info);
                }
            }
                var zipFile = ZipFile.Open(str+".zip", ZipArchiveMode.Create);
            zipFile.CreateEntryFromFile(str, "plot.txt");
            zipFile.CreateEntryFromFile(str+".xml", "meta.xml");
            var outputFolder = Path.Combine(Path.GetTempPath(), "Audiofiles");
            var outputFilePath = Path.Combine(outputFolder, number + ".wav");
            zipFile.CreateEntryFromFile(outputFilePath, "audio.wav");
            zipFile.Dispose();
            File.Delete(str);
            File.Delete(str + ".xml");
            File.Delete(outputFilePath);
            return str;
        }
    public static string Restore(ref double[] data)
    {
            var outputFolder = Path.Combine(Path.GetTempPath(), "Audiofiles");
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if ( openFileDialog1.ShowDialog() == DialogResult.Cancel )
            return null;
        string filename = openFileDialog1.FileName;
        List<double> vals = new List<double>();
            using (ZipArchive zip = ZipFile.Open(filename, ZipArchiveMode.Read))
                foreach (ZipArchiveEntry entry in zip.Entries)
                    if (entry.Name == "plot.txt")
                    {
                        File.Delete(Path.Combine(outputFolder, "plot.txt"));
                        entry.ExtractToFile(Path.Combine(outputFolder, "plot.txt"));
                    }
            TextReader reader = File.OpenText(Path.Combine(outputFolder, "plot.txt"));
        string line;
        while( (line = reader.ReadLine()) != null )
        {
            string[] bits = line.Split( ' ' );
            foreach( string bit in bits )
            {
                    if( !double.TryParse( bit, out double value ) )
                    {
                        Console.WriteLine( "Bad value" );
                    }
                    vals.Add( value );
            }
        }
            reader.Dispose();
            data = vals.ToArray();
            return filename;
    }
}
}
