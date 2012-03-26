using System;
using DowJones.Tools.Ajax.HeadlineList;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Trigger.V1_1;
using MasterTrigger=Factiva.Gateway.Messages.Trigger.V2_0.MasterTrigger;


namespace DowJones.Tools.Ajax.Converters
{
    public delegate void GenerateSnippetThumbnailForHeadlineInfo(ThumbnailImage image, ContentHeadline contentHeadline);
    public delegate string GenerateExternalUrlForHeadlineInfo(ContentHeadline contentHeadline, bool isDuplicate);
    public delegate string GenerateExternalUrlForTriggerInfo(Trigger trigger);
    public delegate string GenerateExternalUrlForSearchTriggerInfo(MasterTrigger trigger);
    public delegate string GenerateExternalUrlForPropertyInfo(Property property);

    public interface IExtendedListDataResultConverter
    {
        IListDataResult Process(Delegate generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo);
    }
}
