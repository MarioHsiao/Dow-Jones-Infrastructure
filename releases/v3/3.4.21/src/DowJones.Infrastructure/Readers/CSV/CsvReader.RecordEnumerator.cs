using System;
using System.Collections;
using System.Collections.Generic;
using DowJones.Readers.CSV.Exceptions;

namespace DowJones.Readers.CSV
{
    public partial class CsvReader
    {
        #region Nested type: RecordEnumerator

        /// <summary>
        /// Supports a simple iteration over the records of a <see cref="CsvReader"/>.
        /// </summary>
        public struct RecordEnumerator
            : IEnumerator<string[]>
        {
            #region Fields

            /// <summary>
            /// Contains the current record.
            /// </summary>
            private string[] m_Current;

            /// <summary>
            /// Contains the current record index.
            /// </summary>
            private long m_CurrentRecordIndex;

            /// <summary>
            /// Contains the enumerated <see cref="CsvReader"/>.
            /// </summary>
            private CsvReader m_Reader;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="RecordEnumerator"/> class.
            /// </summary>
            /// <param name="reader">The <see cref="CsvReader"/> to iterate over.</param>
            /// <exception cref="ArgumentNullException">
            ///		<paramref name="reader"/> is a <see langword="null"/>.
            /// </exception>
            public RecordEnumerator(CsvReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                m_Reader = reader;
                m_Current = null;

                m_CurrentRecordIndex = reader._currentRecordIndex;
            }

            #endregion

            #region IEnumerator<string[]> Members

            /// <summary>
            /// Gets the current record.
            /// </summary>
            public string[] Current
            {
                get { return m_Current; }
            }

            /// <summary>
            /// Advances the enumerator to the next record of the CSV.
            /// </summary>
            /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next record, <see langword="false"/> if the enumerator has passed the end of the CSV.</returns>
            public bool MoveNext()
            {
                if (m_Reader._currentRecordIndex != m_CurrentRecordIndex)
                    throw new InvalidOperationException(ExceptionMessage.EnumerationVersionCheckFailed);

                if (m_Reader.ReadNextRecord())
                {
                    m_Current = new string[m_Reader._fieldCount];

                    m_Reader.CopyCurrentRecordTo(m_Current);
                    m_CurrentRecordIndex = m_Reader._currentRecordIndex;

                    return true;
                }
                else
                {
                    m_Current = null;
                    m_CurrentRecordIndex = m_Reader._currentRecordIndex;

                    return false;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first record in the CSV.
            /// </summary>
            public void Reset()
            {
                if (m_Reader._currentRecordIndex != m_CurrentRecordIndex)
                    throw new InvalidOperationException(ExceptionMessage.EnumerationVersionCheckFailed);

                m_Reader.MoveTo(-1);

                m_Current = null;
                m_CurrentRecordIndex = m_Reader._currentRecordIndex;
            }

            /// <summary>
            /// Gets the current record.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    if (m_Reader._currentRecordIndex != m_CurrentRecordIndex)
                        throw new InvalidOperationException(ExceptionMessage.EnumerationVersionCheckFailed);

                    return Current;
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                m_Reader = null;
                m_Current = null;
            }

            #endregion
        }

        #endregion
    }
}