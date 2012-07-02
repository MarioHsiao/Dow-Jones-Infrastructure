using System.Collections.Generic;
using System.IO;

namespace JsXmlDocParser
{
    internal class BufferedTextReader
    {
        private readonly TextReader _reader;
        private readonly Queue<string> _buffer;

        public BufferedTextReader(TextReader reader)
        {
            _reader = reader;
            _buffer = new Queue<string>();
        }

        public string PeekLine()
        {
            var line = ReadLine();
            _buffer.Enqueue(line);
            return line;
        }

        public string ReadLine()
        {
            return _buffer.Count > 0 ? _buffer.Dequeue() : _reader.ReadLine();
        }
    }
}