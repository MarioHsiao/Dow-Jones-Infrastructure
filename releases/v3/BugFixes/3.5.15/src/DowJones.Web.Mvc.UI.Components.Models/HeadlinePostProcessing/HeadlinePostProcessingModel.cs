using System.Collections.Generic;
using DowJones.Attributes;

namespace DowJones.Web.Mvc.UI.Components.HeadlinePostProcessing
{
    public class HeadlinePostProcessingModel
    {
        public IEnumerable<HeadlinePostProcessingOptions> HeadlinePostProcessingOptions { get; set; }

        public HeadlinePostProcessingModel()
        {
            HeadlinePostProcessingOptions =
                new[]
                    {
                        HeadlinePostProcessing.HeadlinePostProcessingOptions.Read,
                        HeadlinePostProcessing.HeadlinePostProcessingOptions.Email,
                        HeadlinePostProcessing.HeadlinePostProcessingOptions.Print,
                        HeadlinePostProcessing.HeadlinePostProcessingOptions.Save,
                    };
        }

        public HeadlinePostProcessingModel(IEnumerable<HeadlinePostProcessingOptions> headlinePostProcessingOptions)
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