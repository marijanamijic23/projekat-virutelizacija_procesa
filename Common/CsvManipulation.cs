using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CsvManipulation : IDisposable
    {
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private bool disposed = false;
        string path = "";

        public CsvManipulation(string path)
        {
            this.path = path;
        }
        ~CsvManipulation()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if (disposing)
                {
                    if(streamWriter != null)
                    {
                        streamWriter.Dispose();
                    }
                    if(streamReader != null)
                    {
                        streamReader.Dispose();
                    }
                }
                disposed = true;
            }
        }

        public void AddTextToFile(string text)
        {
            streamWriter = File.AppendText(path);
            streamWriter.WriteLine(text);
            streamWriter.Close();
        }

        public string ReadAllText()
        {
            streamReader = File.OpenText(path);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return text;
        }

        public void DeleteAllText()
        {
            File.WriteAllText(path, string.Empty);
        }
    }
}
