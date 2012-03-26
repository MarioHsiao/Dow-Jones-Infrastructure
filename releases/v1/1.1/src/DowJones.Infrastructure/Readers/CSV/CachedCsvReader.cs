using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace DowJones.Utilities.Readers.CSV
{
    /// <summary>
    /// Represents a reader that provides fast, cached, dynamic access to CSV data.
    /// </summary>
    /// <remarks>The number of records is limited to <see cref="System.Int32.MaxValue"/> - 1.</remarks>
    public partial class CachedCsvReader : IListSource
    {
        #region Fields

        /// <summary>
        /// Contains the binding list linked to this reader.
        /// </summary>
        private CsvBindingList m_BindingList;

        /// <summary>
        /// Contains the current record index (inside the cached records array).
        /// </summary>
        private long m_CurrentRecordIndex;

        /// <summary>
        /// Indicates if a new record is being read from the CSV stream.
        /// </summary>
        private bool m_ReadingStream;

        /// <summary>
        /// Contains the cached records.
        /// </summary>
        private readonly List<string[]> _records;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders)
            : this(reader, hasHeaders, DefaultBufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders, int bufferSize)
            : this(reader, hasHeaders, DefaultDelimiter, DefaultQuote, DefaultEscape, DefaultComment, true, bufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <exception cref="ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter)
            : this(reader, hasHeaders, delimiter, DefaultQuote, DefaultEscape, DefaultComment, true, DefaultBufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter, int bufferSize)
            : this(reader, hasHeaders, delimiter, DefaultQuote, DefaultEscape, DefaultComment, true, bufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="quote">The quotation character wrapping every field (default is ''').</param>
        /// <param name="escape">
        /// The escape character letting insert quotation characters inside a quoted field (default is '\').
        /// If no escape character, set to '\0' to gain some performance.
        /// </param>
        /// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
        /// <param name="trimSpaces"><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>. Default is <see langword="true"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///		Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces)
            : this(reader, hasHeaders, delimiter, quote, escape, comment, trimSpaces, DefaultBufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CsvReader class.
        /// </summary>
        /// <param name="reader">A <see cref="TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="quote">The quotation character wrapping every field (default is ''').</param>
        /// <param name="escape">
        /// The escape character letting insert quotation characters inside a quoted field (default is '\').
        /// If no escape character, set to '\0' to gain some performance.
        /// </param>
        /// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
        /// <param name="trimSpaces"><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>. Default is <see langword="true"/>.</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="ArgumentNullException">
        ///		<paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///		<paramref name="bufferSize"/> must be 1 or more.
        /// </exception>
        public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces, int bufferSize)
            : base(reader, hasHeaders, delimiter, quote, escape, comment, trimSpaces, bufferSize)
        {
            _records = new List<string[]>();
            m_CurrentRecordIndex = -1;
        }

        #endregion

        #region Properties

        #region State

        /// <summary>
        /// Gets the current record index in the CSV file.
        /// </summary>
        /// <value>The current record index in the CSV file.</value>
        public override long CurrentRecordIndex
        {
            get { return m_CurrentRecordIndex; }
        }

        /// <summary>
        /// Gets a value that indicates whether the current stream position is at the end of the stream.
        /// </summary>
        /// <value><see langword="true"/> if the current stream position is at the end of the stream; otherwise <see langword="false"/>.</value>
        public override bool EndOfStream
        {
            get
            {
                if (m_CurrentRecordIndex < base.CurrentRecordIndex)
                    return false;
                else
                    return base.EndOfStream;
            }
        }

        #endregion

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the field at the specified index.
        /// </summary>
        /// <value>The field at the specified index.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///		<paramref name="field"/> must be included in [0, <see cref="CsvReader.FieldCount"/>[.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///		No record read yet. Call ReadLine() first.
        /// </exception>
        /// <exception cref="MissingFieldCsvException">
        ///		The CSV data appears to be missing a field.
        /// </exception>
        /// <exception cref="MalformedCsvException">
        ///		The CSV appears to be corrupt at the current position.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///		The instance has been disposed of.
        /// </exception>
        public override String this[int field]
        {
            get
            {
                if (m_ReadingStream)
                    return base[field];
                else if (m_CurrentRecordIndex > -1)
                {
                    if (field > -1 && field < FieldCount)
                        return _records[(int) m_CurrentRecordIndex][field];
                    else
                        throw new ArgumentOutOfRangeException("field", field, string.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldIndexOutOfRange, field));
                }
                else
                    throw new InvalidOperationException(ExceptionMessage.NoCurrentRecord);
            }
        }

        #endregion

        #region Methods

        #region Read

        /// <summary>
        /// Reads the CSV stream from the current position to the end of the stream.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        ///	The instance has been disposed of.
        /// </exception>
        public virtual void ReadToEnd()
        {
            m_CurrentRecordIndex = base.CurrentRecordIndex;

            while (ReadNextRecord()) {;}
        }

        /// <summary>
        /// Reads the next record.
        /// </summary>
        /// <param name="onlyReadHeaders">
        /// Indicates if the reader will proceed to the next record after having read headers.
        /// <see langword="true"/> if it stops after having read headers; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="skipToNextLine">
        /// Indicates if the reader will skip directly to the next line without parsing the current one. 
        /// To be used when an error occurs.
        /// </param>
        /// <returns><see langword="true"/> if a record has been successfully reads; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ObjectDisposedException">
        ///	The instance has been disposed of.
        /// </exception>
        protected override bool ReadNextRecord(bool onlyReadHeaders, bool skipToNextLine)
        {
            if (m_CurrentRecordIndex < base.CurrentRecordIndex)
            {
                m_CurrentRecordIndex++;
                return true;
            }
            else
            {
                m_ReadingStream = true;

                try
                {
                    bool canRead = base.ReadNextRecord(onlyReadHeaders, skipToNextLine);

                    if (canRead)
                    {
                        string[] record = new string[FieldCount];

                        if (base.CurrentRecordIndex > -1)
                        {
                            CopyCurrentRecordTo(record);
                            _records.Add(record);
                        }
                        else
                        {
                            MoveTo(0);
                            CopyCurrentRecordTo(record);
                            MoveTo(-1);
                        }

                        if (!onlyReadHeaders)
                            m_CurrentRecordIndex++;
                    }
                    else
                    {
                        // No more records to read, so set array size to only what is needed
                        _records.Capacity = _records.Count;
                    }

                    return canRead;
                }
                finally
                {
                    m_ReadingStream = false;
                }
            }
        }

        #endregion

        #region Move

        /// <summary>
        /// Moves before the first record.
        /// </summary>
        public void MoveToStart()
        {
            m_CurrentRecordIndex = -1;
        }

        /// <summary>
        /// Moves to the last record read so far.
        /// </summary>
        public void MoveToLastCachedRecord()
        {
            m_CurrentRecordIndex = base.CurrentRecordIndex;
        }

        /// <summary>
        /// Moves to the specified record index.
        /// </summary>
        /// <param name="record">The record index.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///		Record index must be > 0.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///		The instance has been disposed of.
        /// </exception>
        public override void MoveTo(long record)
        {
            if (record < -1)
                throw new ArgumentOutOfRangeException("record", record, ExceptionMessage.RecordIndexLessThanZero);

            if (record <= base.CurrentRecordIndex)
                m_CurrentRecordIndex = record;
            else
            {
                m_CurrentRecordIndex = base.CurrentRecordIndex;

                long offset = record - m_CurrentRecordIndex;

                // read to the last record before the one we want
                while (offset-- > 0 && ReadNextRecord()) {;}
            }
        }

        #endregion

        #endregion

        #region IListSource Members

        bool IListSource.ContainsListCollection
        {
            get { return false; }
        }

        IList IListSource.GetList()
        {
            if (m_BindingList == null)
                m_BindingList = new CsvBindingList(this);

            return m_BindingList;
        }

        #endregion
    }
}