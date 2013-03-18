using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace DowJones.Readers.CSV.Exceptions
{
    /// <summary>
    /// Represents the exception that is thrown when a CSV file is malformed.
    /// </summary>
    [Serializable]
    public class MalformedCsvException : ApplicationException
    {
        #region Fields

        /// <summary>
        /// Contains the current field index.
        /// </summary>
        private readonly int m_CurrentFieldIndex;

        /// <summary>
        /// Contains the current position in the raw data.
        /// </summary>
        private readonly int m_CurrentPosition;

        /// <summary>
        /// Contains the current record index.
        /// </summary>
        private readonly long m_CurrentRecordIndex;

        /// <summary>
        /// Contains the message that describes the error.
        /// </summary>
        private readonly string m_Message;

        /// <summary>
        /// Contains the raw data when the error occured.
        /// </summary>
        private readonly string m_RawData;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MalformedCsvException class.
        /// </summary>
        public MalformedCsvException()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MalformedCsvException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MalformedCsvException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MalformedCsvException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MalformedCsvException(string message, Exception innerException)
            : base(String.Empty, innerException)
        {
            m_Message = (message ?? string.Empty);

            m_RawData = string.Empty;
            m_CurrentPosition = -1;
            m_CurrentRecordIndex = -1;
            m_CurrentFieldIndex = -1;
        }

        /// <summary>
        /// Initializes a new instance of the MalformedCsvException class.
        /// </summary>
        /// <param name="rawData">The raw data when the error occured.</param>
        /// <param name="currentPosition">The current position in the raw data.</param>
        /// <param name="currentRecordIndex">The current record index.</param>
        /// <param name="currentFieldIndex">The current field index.</param>
        public MalformedCsvException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex)
            : this(rawData, currentPosition, currentRecordIndex, currentFieldIndex, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MalformedCsvException class.
        /// </summary>
        /// <param name="rawData">The raw data when the error occured.</param>
        /// <param name="currentPosition">The current position in the raw data.</param>
        /// <param name="currentRecordIndex">The current record index.</param>
        /// <param name="currentFieldIndex">The current field index.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MalformedCsvException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex, Exception innerException)
            : base(String.Empty, innerException)
        {
            m_RawData = (rawData ?? string.Empty);
            m_CurrentPosition = currentPosition;
            m_CurrentRecordIndex = currentRecordIndex;
            m_CurrentFieldIndex = currentFieldIndex;

            m_Message = String.Format(CultureInfo.InvariantCulture, ExceptionMessage.MalformedCsvException, m_CurrentRecordIndex, m_CurrentFieldIndex, m_CurrentPosition, m_RawData);
        }

        /// <summary>
        /// Initializes a new instance of the MalformedCsvException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected MalformedCsvException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_Message = info.GetString("MyMessage");

            m_RawData = info.GetString("RawData");
            m_CurrentPosition = info.GetInt32("CurrentPosition");
            m_CurrentRecordIndex = info.GetInt64("CurrentRecordIndex");
            m_CurrentFieldIndex = info.GetInt32("CurrentFieldIndex");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the raw data when the error occured.
        /// </summary>
        /// <value>The raw data when the error occured.</value>
        public string RawData
        {
            get { return m_RawData; }
        }

        /// <summary>
        /// Gets the current position in the raw data.
        /// </summary>
        /// <value>The current position in the raw data.</value>
        public int CurrentPosition
        {
            get { return m_CurrentPosition; }
        }

        /// <summary>
        /// Gets the current record index.
        /// </summary>
        /// <value>The current record index.</value>
        public long CurrentRecordIndex
        {
            get { return m_CurrentRecordIndex; }
        }

        /// <summary>
        /// Gets the current field index.
        /// </summary>
        /// <value>The current record index.</value>
        public int CurrentFieldIndex
        {
            get { return m_CurrentFieldIndex; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value>A message that describes the current exception.</value>
        public override string Message
        {
            get { return m_Message; }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("MyMessage", m_Message);

            info.AddValue("RawData", m_RawData);
            info.AddValue("CurrentPosition", m_CurrentPosition);
            info.AddValue("CurrentRecordIndex", m_CurrentRecordIndex);
            info.AddValue("CurrentFieldIndex", m_CurrentFieldIndex);
        }

        #endregion
    }
}