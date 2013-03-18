using System;
using System.Collections.Generic;

namespace DowJones.Web.Handlers.Proxy.Core
{

    /// <summary>
    /// Summary description for PipeStreamBlock
    /// </summary>
    public class PipeStreamBlock : PipeStream
    {
        private readonly Queue<byte[]> _Buffer = new Queue<byte[]>(1000);
        private int _Length = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeStreamBlock"/> class.
        /// </summary>
        /// <param name="readWriteTimeout"></param>
        public PipeStreamBlock(int readWriteTimeout)
            : base(readWriteTimeout)
        {
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A long value representing the length of the stream in bytes.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Length
        {
            get { return _Length; }
        }

        /// <summary>
        /// Writes to buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        protected override void WriteToBuffer(byte[] buffer, int offset, int count)
        {
            byte[] bufferCopy = new byte[count];
            Buffer.BlockCopy(buffer, offset, bufferCopy, 0, count);
            _Buffer.Enqueue(bufferCopy);

            _Length += count;
        }

        /// <summary>
        /// Reads to buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        protected override int ReadToBuffer(byte[] buffer, int offset, int count)
        {
            if (0 == _Buffer.Count) return 0;

            byte[] chunk = _Buffer.Dequeue();
            // It's possible the chunk has smaller number of bytes than buffer capacity
            Buffer.BlockCopy(chunk, 0, buffer, offset, chunk.Length);

            _Length -= chunk.Length;
            return chunk.Length;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _Length = 0;
            _Buffer.Clear();
        }
    }
}