using System;

namespace DowJones.Utilities.Readers.CSV
{
    /// <summary>
    /// Provides data for the <see cref="CsvReader.ParseError"/> event.
    /// </summary>
    public class ParseErrorEventArgs
        : EventArgs
    {
        #region Fields

        /// <summary>
        /// Contains the error that occured.
        /// </summary>
        private readonly MalformedCsvException m_Error;

        /// <summary>
        /// Contains the action to take.
        /// </summary>
        private ParseErrorAction m_Action;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ParseErrorEventArgs class.
        /// </summary>
        /// <param name="error">The error that occured.</param>
        /// <param name="defaultAction">The default action to take.</param>
        public ParseErrorEventArgs(MalformedCsvException error, ParseErrorAction defaultAction)
        {
            m_Error = error;
            m_Action = defaultAction;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error that occured.
        /// </summary>
        /// <value>The error that occured.</value>
        public MalformedCsvException Error
        {
            get { return m_Error; }
        }

        /// <summary>
        /// Gets or sets the action to take.
        /// </summary>
        /// <value>The action to take.</value>
        public ParseErrorAction Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        #endregion
    }
}