using System;
using System.Collections;
using System.IO;
using System.Text;

namespace DowJones.Readers.CSV
{
    public class BasicCSVReader : IDisposable
    {
        private readonly StreamReader m_StreamReader;
        private bool m_Disposed;
        private readonly string m_DisposedError = "This object has been disposed.";


        //add name space System.IO.Stream       
        public BasicCSVReader(Stream filestream) : this(filestream, null)
        {
            m_Disposed = false;

        }

        public BasicCSVReader(Stream filestream, Encoding enc)
        {
            //check the Pass Stream whether it is readable or not           
            if (!filestream.CanRead)
            {
                return;
            }
            m_StreamReader = (enc != null) ? new StreamReader(filestream, enc) : new StreamReader(filestream);
        }

        public BasicCSVReader(string dataStr, Encoding enc)
        {
            if (string.IsNullOrEmpty(dataStr) || string.IsNullOrEmpty(dataStr.Trim()))
            {
                return;
            }

            Encoding encoding = (enc) ??  Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(dataStr);
            MemoryStream m = new MemoryStream(bytes);
            m_StreamReader = new StreamReader(m);
       }

        //parse the Line       
        public string[] GetCSVLine()
        {
            if (m_Disposed)
            {
                throw new ObjectDisposedException(m_DisposedError);
            }

            string data = m_StreamReader.ReadLine();
            if (data == null) return null;
            if (data.Length == 0) return new string[0];
            //System.Collection.Generic           
            ArrayList result = new ArrayList();
            //parsing CSV Data           
            ParseCSVData(result, data);
            return (string[]) result.ToArray(typeof (string));
        }

        private void ParseCSVData(ArrayList result, string data)
        {
            int position = -1;
            while (position < data.Length)
            {
                result.Add(ParseCSVField(ref data, ref position));
            }
        }

        private string ParseCSVField(ref string data, ref int StartSeperatorPos)
        {
            if (StartSeperatorPos == data.Length - 1)
            {
                StartSeperatorPos++;
                return "";
            }
            int fromPos = StartSeperatorPos + 1;
            if (data[fromPos] == '"')
            {
                int nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                int lines = 1;
                while (nextSingleQuote == -1)
                {
                    data = data + "\n" + m_StreamReader.ReadLine();
                    nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                    lines++;
                    if (lines > 20)
                        throw new Exception("lines overflow: " + data);
                }
                StartSeperatorPos = nextSingleQuote + 1;
                string tempString = data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
                tempString = tempString.Replace("'", "''");
                return tempString.Replace("\"\"", "\"");
            }
            int nextComma = data.IndexOf(',', fromPos);
            if (nextComma == -1)
            {
                StartSeperatorPos = data.Length;
                return data.Substring(fromPos);
            }
            else
            {
                StartSeperatorPos = nextComma;
                return data.Substring(fromPos, nextComma - fromPos);
            }
        }

        private static int GetSingleQuote(string data, int SFrom)
        {
            int i = SFrom - 1;
            while (++i < data.Length)
                if (data[i] == '"')
                {
                    if (i < data.Length - 1 && data[i + 1] == '"')
                    {
                        i++;
                        continue;
                    }
                    else
                        return i;
                }
            return -1;
        }

        #region IDisposable Members

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    // Code to dispose managed resources held by the class
                    if (m_StreamReader != null)
                    {
                        m_StreamReader.Dispose();
                    }
                }
            }
            // Code to dispose unmanaged resources held by the class
            m_Disposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="BasicCSVReader"/> is reclaimed by garbage collection.
        /// </summary>
        ~BasicCSVReader()
        {
            Dispose(false);
        }
        #endregion
    }
}