// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalHeadlineListDataResultAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Attributes;
using DowJones.Utilities.Core;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Formatters.Numerical;
using DowJones.Utilities.Managers.Core;
using Factiva.Gateway.Messages.Screening.V1_1;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common
{
    public class PortalHeadlineListDataResultAssembler : IAssembler<PortalHeadlineListDataResult, List<ReportType>>
    {
        private readonly DateTimeFormatter dateTimeFormatter;
        private readonly NumberFormatter numberFormatter;

        public enum ReportTypeEx
        {
            Investext,
            Zacks,
        }

        public PortalHeadlineListDataResultAssembler(DateTimeFormatter formatter)
        {
            dateTimeFormatter = formatter;
            numberFormatter = new NumberFormatter();
        }

        #region Implementation of IAssembler<out PortalHeadlineListDataResult,in List<ReportType>>

        public PortalHeadlineListDataResult Convert(List<ReportType> source, ReportTypeEx rType = ReportTypeEx.Investext)
        {
            if (source == null)
            {
                return new PortalHeadlineListDataResult
                           {
                               HitCount = new WholeNumber(0),
                               ResultSet = new PortalHeadlineListResultSet
                                               {
                                                   Count = new WholeNumber(0),
                                                   First = new WholeNumber(0),
                                                   DuplicateCount = new WholeNumber(0),
                                                   Headlines = new List<PortalHeadlineInfo>(),
                                               },
                           };
            }

            var response = new PortalHeadlineListDataResult
                               {
                                   HitCount = new WholeNumber(source.Count),
                                   ResultSet = new PortalHeadlineListResultSet
                                                   {
                                                       Count = new WholeNumber(source.Count), 
                                                       First = new WholeNumber(0),
                                                       DuplicateCount = new WholeNumber(0),
                                                       Headlines = new List<PortalHeadlineInfo>(),
                                                   },
                               };

            foreach (var reportType in source)
            {
                DateTime curDate;
                var tempStr = reportType.ReplyItem.Date.Value;
                if (reportType.ReplyItem.Date.Fid != null && reportType.ReplyItem.Date.Fid.Equals("pd", StringComparison.InvariantCultureIgnoreCase))
                {
                    curDate = new DateTime(
                        int.Parse(tempStr.Substring(0, 4)), 
                        int.Parse(tempStr.Substring(4, 2)), 
                        int.Parse(tempStr.Substring(6, 2)));
                }
                else if (DateTime.TryParse(tempStr, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeUniversal, out curDate))
                {
                }
                else
                {
                    curDate = new DateTime(
                        int.Parse(tempStr.Substring(0, 4)), 
                        int.Parse(tempStr.Substring(5, 2)), 
                        int.Parse(tempStr.Substring(8, 2)));
                }

                var temp = new PortalHeadlineInfo
                               {
                                   Authors = null,
                                   HeadlineUrl = null,
                                   Title = reportType.ReplyItem.Title.Headline.Para.Value,
                                   PublicationDateTime = DateTimeFormatter.ConvertToUtc(curDate),
                                   PublicationDateTimeDescriptor = dateTimeFormatter.FormatDate(curDate),
                                   PublicationDateDescriptor = dateTimeFormatter.FormatDate(curDate),
                                   PublicationTimeDescriptor = string.Empty,
                                   HasPublicationTime = false,
                                   SourceCode = reportType.ReplyItem.SrcCode.Value,
                                   SourceDescriptor = reportType.ReplyItem.SrcName.Value,
                                   //// ContentCategory = ContentCategory.Publication,
                                   ContentCategoryDescriptor = ContentCategory.Publication.ToString(),
                                   //// ContentSubCategory = ContentSubCategory.PDF,
                                   ContentSubCategoryDescriptor = ContentSubCategory.PDF.ToString(),
                                   Reference = new Reference
                                                   {
                                                       guid = reportType.ReplyItem.AccessionNo.Value,
                                                       type = "accessionNo",
                                                       subType = rType.ToString(),
                                                   },

                                   BaseLanguage = reportType.ReplyItem.BaseLang.Value.ToLowerInvariant(),
                                   BaseLanguageDescriptor = GetLanguageToContentLanguage(reportType.ReplyItem.BaseLang.Value.ToLowerInvariant()),
                                   WordCount = new WholeNumber(0),
                                   WordCountDescriptor = string.Format(
                                       "{0} {1}",
                                       numberFormatter.Format(0, NumberFormatType.Whole),
                                       ResourceTextManager.Instance.GetString("words")),
                               };
                if (reportType.AdocTOC != null &&
                    reportType.AdocTOC.ItemCollection != null &&
                    reportType.AdocTOC.ItemCollection.Count > 0)
                {
                    var item = reportType.AdocTOC.ItemCollection[0];
                    temp.Reference.@ref = item.@ref;
                    temp.Reference.mimetype = item.Mimetype;
                    temp.Reference.imageType = item.Type;
                }
                
                response.ResultSet.Headlines.Add(temp);
            }

            return response;
        }

        #endregion

        /// <summary>
        /// Maps the language to content language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>A string descriptor of the language code.</returns>
        protected string GetLanguageToContentLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(language.Trim()))
            {
                var fieldInfo = typeof(ContentLanguage).GetField(language.ToLower());
                if (fieldInfo != null)
                {
                    var assignedToken = (AssignedToken)Attribute.GetCustomAttribute(fieldInfo, typeof(AssignedToken));
                    if (assignedToken != null)
                    {
                        return ResourceTextManager.Instance.GetString(assignedToken.Token);
                    }
                }
            }

            return language;
        }

        #region Implementation of IAssembler<out PortalHeadlineListDataResult,in List<ReportType>>

        public PortalHeadlineListDataResult Convert(List<ReportType> source)
        {
            return Convert(source, ReportTypeEx.Investext);
        }

        #endregion
    }
}
