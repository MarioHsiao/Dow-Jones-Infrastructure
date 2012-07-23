using EMG.Tools.Ajax.HeadlineList;
using Factiva.Gateway.Messages.Search.V2_0;

namespace EMG.Tools.Ajax.Converters.HeadlineList
{
    interface IHeadlineListDataResultConverter
    {
        HeadlineListDataResult Process();
    }

    interface IExtendedHeadlineListDataResultConverter
    {
        HeadlineListDataResult Process(GenerateExternalUrl generateExternalUrl, GenerateSnippetThumbnail generateSnippetThumbnail);
    }
}
