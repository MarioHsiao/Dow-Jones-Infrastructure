
namespace DowJones.Prod.X.Common.Exceptions
{
    public class ApplicationExceptionConstants
    {
        // The following errors are represented in this error range 219000-219999

        public const long BaseError = 219000;
        public const long GeneralError = BaseError;
        public const long InvalidArgument = BaseError + 1;
        public const long BadMultimediaResponse = BaseError + 2;
        public const long UnableToCreateNewsletter = BaseError + 3;
        public const long UnableToRetrieveNewsletter = BaseError + 4;
        public const long UnableToRetrieveWorkspace = BaseError + 5;
        public const long GeneralWebException = BaseError + 6;

        public const long NoAccessToNewsletters = 210110;
        public const string ErrInvalidSession = "-2147176633";
        public const string ErrNoSession = "-2147183530";
    }
}
