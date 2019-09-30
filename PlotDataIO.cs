using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumAnalyzer
{
    class PlotDataIO
    {
        public static string Save( double[] dataArray )
        {
            System.IO.Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.RestoreDirectory = true;
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
            return str;
        }
    public static string Restore(ref double[] data)
    {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        if( openFileDialog1.ShowDialog() == DialogResult.Cancel )
            return null;
        string filename = openFileDialog1.FileName;
        List<double> vals = new List<double>();
        TextReader reader = File.OpenText( filename );
        string line;
        while( (line = reader.ReadLine()) != null )
        {
            string[] bits = line.Split( ' ' );
            foreach( string bit in bits )
            {
                double value;
                if( !double.TryParse( bit, out value ) )
                {
                    Console.WriteLine( "Bad value" );
                }
                vals.Add( value );
            }
        }
            data = vals.ToArray();
            return filename;
    }
}
}
