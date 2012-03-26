namespace DowJones.Utilities.Readers.CSV
{
    public class ExceptionMessage
    {
        public const string BufferSizeTooSmall = "Buffer size must be 1 or more.";
        
        public const string CannotMovePreviousRecordInForwardOnly = "Cannot move to a previous record in forward-only mode.";
        
        public const string CannotReadRecordAtIndex = "Cannot read record at index '{0}'.";
        
        public const string EnumerationFinishedOrNotStarted = "Enumeration has either not started or has already finished.";
        
        public const string EnumerationVersionCheckFailed = "Collection was modified; enumeration operation may not execute.";

        public const string FieldHeaderNotFound = "'{0}' field header not found.";

        public const string FieldIndexOutOfRange = "Field index must be included in [0, FieldCount[. Specified field index was : '{0}'.";

        public const string MalformedCsvException = "The CSV appears to be corrupt near record '{0}' field '{1} at position '{2}'. Current raw data : '{3}'.";

        public const string MissingFieldActionNotSupported = "'{0}' is not a supported missing field action.";

        public const string NoCurrentRecord = "No current record.";

        public const string NoHeaders = "The CSV does not have headers (CsvReader.HasHeaders property is false).";

        public const string NotEnoughSpaceInArray = "The number of fields in the record is greater than the available space from index to the end of the destination array.";

        public const string ParseErrorActionInvalidInsideParseErrorEvent = "'{0}' is not a valid ParseErrorAction while inside a ParseError event.";

        public const string ParseErrorActionNotSupported = "'{0}' is not a supported ParseErrorAction.";

        public const string ReaderClosed = "This operation is invalid when the reader is closed.";

        public const string RecordIndexLessThanZero = "Record index must be 0 or more.";
    }
}