using System.Collections.Generic;
using DowJones.Attributes;

namespace DowJones.Web.Mvc.UI.Components.Search
{
    public class HeadlinePostProcessing
    {
        public IEnumerable<HeadlinePostProcessingOptions> HeadlinePostProcessingOptions { get; set; }

        public HeadlinePostProcessing()
        {
            HeadlinePostProcessingOptions =
                new[]
                    {
                        Search.HeadlinePostProcessingOptions.Read,
                        Search.HeadlinePostProcessingOptions.Email,
                        Search.HeadlinePostProcessingOptions.Print,
                        Search.HeadlinePostProcessingOptions.Save,
                    };
        }

        public HeadlinePostProcessing(IEnumerable<HeadlinePostProcessingOptions> headlinePostProcessingOptions)
        {
            HeadlinePostProcessingOptions = headlinePostProcessingOptions;
        }
    }

    public enum HeadlinePostProcessingOptions
    {
        [AssignedToken("")]
        Unspecified,
        [AssignedToken("read")]
        Read,
        [AssignedToken("save")]
        Save,
        [AssignedToken("print")]
        Print,
        [AssignedToken("email")]
        Email
    }

    public enum HeadlineSaveOptions
    {
        [AssignedToken("rtf")]
        Rtf,
        [AssignedToken("pdf")]
        Pdf,
        [AssignedToken("xml")]
        Xml,
    }

    public enum FormatOptions
    {
        [AssignedToken("headlineFormat")]
        HeadlineFormat,
        [AssignedToken("articleFormat")]
        ArticleFormat,
        [AssignedToken("articleWithTOC")]
        ArticleWithToc,
    }
    
}