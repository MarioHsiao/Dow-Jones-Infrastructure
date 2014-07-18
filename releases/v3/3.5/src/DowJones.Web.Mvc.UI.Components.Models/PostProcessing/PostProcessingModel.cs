using System.Collections.Generic;
using DowJones.Attributes;
using System.Linq;

namespace DowJones.Web.Mvc.UI.Components.PostProcessing
{
    public class PostProcessingModel
    {
        public IEnumerable<PostProcessingOptions> PostProcessingOptions { get; set; }

        public PostProcessingModel()
        {
            PostProcessingOptions = Enumerable.Empty<PostProcessingOptions>();
            ShowActionsCheckBox = true;
        }

        public PostProcessingModel(IEnumerable<PostProcessingOptions> postProcessingOptions)
        {
            PostProcessingOptions = postProcessingOptions;
            ShowActionsCheckBox = true;
        }

        public PostProcessingModel(IEnumerable<PostProcessingOptions> postProcessingOptions, bool showActionsCheckBox)
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
        [AssignedToken("printLabel")]
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
        [AssignedToken("downloadAsPDF")]
        PDF,
        [AssignedToken("downloadAsRTF")]
        RTF,
        [AssignedToken("newsletterPPS")]
        Newsletter,
        [AssignedToken("workspacePPS")]
        Workspace
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