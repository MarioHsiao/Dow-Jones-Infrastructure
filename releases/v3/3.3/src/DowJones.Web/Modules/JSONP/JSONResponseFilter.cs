// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JSONResponseFilter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved
// </copyright>
// <summary>
//   The JSON response filter.
// </summary>
// <author>
//   David Da Costa
// </author>
// <lastModified>
//  <entry><date>5/10/2008</date><user>dacostad</user></entry>
// </lastModified>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Text;
using System.Web;

namespace DowJones.Web.Modules.JSONP
{
    /// <summary>
    /// The json response filter.
    /// </summary>
    public class JSONResponseFilter : Stream
    {
        /// <summary>
        /// The _context.
        /// </summary>
        private readonly HttpContext _context;

        /// <summary>
        /// The _response stream.
        /// </summary>
        private readonly Stream _responseStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONResponseFilter"/> class.
        /// </summary>
        /// <param name="responseStream">The response stream.</param>
        /// <param name="context">The context.</param>
        public JSONResponseFilter(Stream responseStream, HttpContext context)
        {
            _responseStream = responseStream;
            _context = context;
        }

        /// <summary>
        /// Gets a value indicating whether CanRead.
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether CanSeek.
        /// </summary>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether CanWrite.
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// Gets Length.
        /// </summary>
        public override long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets or sets Position.
        /// </summary>
        public override long Position { get; set; }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> or <paramref name="count"/> is negative.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support writing.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            var b1 = Encoding.UTF8.GetBytes(_context.Request.Params["callback"] + "(");
            _responseStream.Write(b1, 0, b1.Length);
            _responseStream.Write(buffer, offset, count);
            var b2 = Encoding.UTF8.GetBytes(");");
            _responseStream.Write(b2, 0, b2.Length);
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.
        /// </summary>
        public override void Close()
        {
            _responseStream.Close();
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        public override void Flush()
        {
            _responseStream.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support seeking, such as if the stream is constructed from a pipe or console output.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _responseStream.Seek(offset, origin);
        }

        /// <summary>
        /// Sets the length.
        /// </summary>
        /// <param name="length">The length.</param>
        public override void SetLength(long length)
        {
            _responseStream.SetLength(length);
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> or <paramref name="count"/> is negative.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support reading.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        /// </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _responseStream.Read(buffer, offset, count);
        }
    }
}