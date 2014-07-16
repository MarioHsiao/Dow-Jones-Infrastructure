using System.Collections.Generic;
using DowJones.Ajax;
using DowJones.Ajax.Newsletter;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;

namespace DowJones.Assemblers.Newsletters
{
    public class GetWorkspacesPropertiesListResponseConverter : AbstractNewsletterListDataResultConverter
    {
        private readonly GetWorkspacesPropertiesListResponse _response;
        private readonly NewsletterListDataResult _result = new NewsletterListDataResult();

        public GetWorkspacesPropertiesListResponseConverter(GetWorkspacesPropertiesListResponse response)
            : this(response, "en")
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="interfaceLanguage"></param>
        public GetWorkspacesPropertiesListResponseConverter(GetWorkspacesPropertiesListResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="dateTimeFormatter"></param>
        public GetWorkspacesPropertiesListResponseConverter(GetWorkspacesPropertiesListResponse response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            _response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IListDataResult</returns>
        public override IListDataResult Process()
        {
            if (_response == null)
            {
                return _result;
            }

            if (_response.WorkspacePropertiesItemCollection.Count == 0)
                return _result;

            _result.ResultSet.Count = new WholeNumber(_response.WorkspacePropertiesItemCollection.Count);
            ProcessNewsletterItems(_response.WorkspacePropertiesItemCollection);

            NumberFormatter.Format(_result.ResultSet.Count);
            return _result;
        }

        /// <summary>
        /// Method to process NewsletterItems
        /// </summary>
        /// <param name="newsletterItemCollection"></param>
        private void ProcessNewsletterItems(IEnumerable<WorkspacePropertiesItem> newsletterItemCollection)
        {
            foreach (var newsletterItem in newsletterItemCollection)
            {
                var newsletterInfo = new NewsletterInfo();
                Convert(newsletterInfo, newsletterItem);
                _result.ResultSet.Newsletters.Add(newsletterInfo);
            }
        }

        /// <summary>
        /// Converts WorkspacePropertiesItems
        /// </summary>
        /// <param name="newsletterInfo"></param>
        /// <param name="newsletterItem"></param>
        private void Convert(NewsletterInfo newsletterInfo, WorkspacePropertiesItem newsletterItem)
        {
            if (newsletterItem == null)
                return;

            newsletterInfo.Code = newsletterItem.Code;
            newsletterInfo.Id = newsletterItem.Id;
            newsletterInfo.Name = newsletterItem.Properties.Name;
            newsletterInfo.LastModifiedDate = newsletterItem.Properties.LastContentModifiedDate;
            newsletterInfo.LastModifiedDateDescriptor = DateTimeFormatter.FormatDate(newsletterInfo.LastModifiedDate);
        }
    }
}
