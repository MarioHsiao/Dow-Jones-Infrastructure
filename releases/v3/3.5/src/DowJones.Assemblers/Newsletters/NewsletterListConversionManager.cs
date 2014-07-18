using System;
using DowJones.Ajax;
using DowJones.Ajax.Newsletter;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Mapping;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;

namespace DowJones.Assemblers.Newsletters
{
    public class NewsletterListConversionManager: ITypeMapper<GetWorkspacesPropertiesListResponse, NewsletterListDataResult>
    {
        public DateTimeFormatter DatetimeFormatter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeFormatter"></param>
        public NewsletterListConversionManager(DateTimeFormatter dateTimeFormatter)
        {
            DatetimeFormatter = dateTimeFormatter;
        }
        public NewsletterListConversionManager(DowJones.Preferences.IPreferences preferences)
        {
            DatetimeFormatter = new DateTimeFormatter(preferences);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns>NewsletterListDataResult</returns>
        public NewsletterListDataResult Map(GetWorkspacesPropertiesListResponse source)
        {
            return Process(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns>NewsletterListDataResult</returns>
        public NewsletterListDataResult Process(GetWorkspacesPropertiesListResponse response)
        {
            var converter = new GetWorkspacesPropertiesListResponseConverter(response, DatetimeFormatter);
            return (NewsletterListDataResult)converter.Process();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public object Map(object source)
        {
            var workspacesPropertiesListResponse = source as GetWorkspacesPropertiesListResponse;
            if (workspacesPropertiesListResponse != null)
            {
                return Map(workspacesPropertiesListResponse);
            }
            throw new NotSupportedException();
        }
    }
}
