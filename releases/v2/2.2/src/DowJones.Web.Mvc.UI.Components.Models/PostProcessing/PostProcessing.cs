using System.Collections.Generic;
using DowJones.Attributes;
using System.Linq;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class PostProcessing
    {
        public IEnumerable<PostProcessingOptions> PostProcessingOptions { get; set; }

        public PostProcessing()
        {
            PostProcessingOptions = Enumerable.Empty<PostProcessingOptions>();
            ShowActionsCheckBox = true;
        }

        public PostProcessing(IEnumerable<PostProcessingOptions> postProcessingOptions)
        {
            PostProcessingOptions = postProcessingOptions;
            ShowActionsCheckBox = true;
        }

        public PostProcessing(IEnumerable<PostProcessingOptions> postProcessingOptions, bool showActionsCheckBox)
        {
            PostProcessingOptions = postProcessingOptions;
            ShowActionsCheckBox = showActionsCheckBox;
        }

        public bool ShowActionsCheckBox { get; set; }

        public bool EnableDuplicateOption { get; set; }
    }

    public enum PostProcessingOptions
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
        Email,
        [AssignedToken("pressClips")]
        PressClips,
        [AssignedToken("export")]
        Export,
        [AssignedToken("share")]
        Share,
        [AssignedToken("listen")]
        Listen,
        [AssignedToken("translate")]
        Translate,
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