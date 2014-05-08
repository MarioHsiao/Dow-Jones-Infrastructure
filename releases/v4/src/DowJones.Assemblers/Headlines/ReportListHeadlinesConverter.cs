using System;
using System.Collections.Generic;
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using Factiva.Gateway.Messages.Screening.V1_1;

namespace DowJones.Assemblers.Headlines
{
    public class ReportListHeadlinesConverter : AbstractHeadlineListDataResultSetConverter
    {

        private readonly ReportsType _response;

        private readonly HeadlineListDataResult _result = new HeadlineListDataResult();



        public ReportListHeadlinesConverter(ReportsType response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _response = response;
        }

        public ReportListHeadlinesConverter(ReportsType response)
            : this(response, "en")
        {
        }

        public ReportListHeadlinesConverter(ReportsType response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            _response = response;
        }

        public override IListDataResult Process()
        {
            if (_response == null)
                return _result;

            _result.hitCount = new WholeNumber(_response.Total);
            _result.resultSet.first = new WholeNumber(_response.First);
            NumberFormatter.Format(_result.hitCount);

            if (_response.ReportCollection.Count == 0)
                return _result;

            _result.resultSet.count = new WholeNumber(_response.ReportCollection.Count);
            _result.resultSet.duplicateCount = _result.resultSet.count;
            ProcessReportHeadlines(_response.ReportCollection, _response.First);

            NumberFormatter.Format(_result.resultSet.first);
            NumberFormatter.Format(_result.resultSet.count);
            NumberFormatter.Format(_result.resultSet.duplicateCount);

            return _result;

        }

        private void ProcessReportHeadlines(IEnumerable<ReportType> responseCollection, int first)
        {
            var i = first;
            foreach (var report in responseCollection)
            {
                var headlineInfo = new HeadlineInfo();
                Convert(headlineInfo, report, ++i);
                _result.resultSet.headlines.Add(headlineInfo);
            }
        }

        private void Convert(HeadlineInfo headlineInfo, ReportType report, int index)
        {
            if (report == null || report.ReplyItem == null)
                return;

            headlineInfo.contentCategory = ContentCategory.Publication;
            headlineInfo.contentCategoryDescriptor = headlineInfo.contentCategory.ToString().ToLower();
            headlineInfo.contentSubCategory = ContentSubCategory.PDF;
            headlineInfo.contentSubCategoryDescriptor = ContentSubCategory.PDF.ToString();

            headlineInfo.reference.type = "accessionNo";
            headlineInfo.reference.guid = report.Accessionno;
            if (report.AdocTOC != null &&
                    report.AdocTOC.ItemCollection != null &&
                    report.AdocTOC.ItemCollection.Count > 0)
            {
                var item = report.AdocTOC.ItemCollection[0];
                headlineInfo.reference.@ref = item.@ref;
                headlineInfo.reference.mimetype = item.Mimetype;
                headlineInfo.reference.imageType = item.Type;
                headlineInfo.reference.subType = "Investext";
            }

            headlineInfo.baseLanguage = report.Lang;
            headlineInfo.baseLanguageDescriptor = GetLanguageToContentLanguage(report.Lang);
            headlineInfo.index = new WholeNumber(index);

            headlineInfo.sourceReference = report.ReplyItem.SrcCode.Value;
            headlineInfo.sourceDescriptor = report.ReplyItem.SrcName.Value;

            headlineInfo.publicationDateTime = PublicationDateTime(report.ReplyItem.Date.Value);
            if (headlineInfo.publicationDateTime > DateTime.MinValue)
            {
                headlineInfo.hasPublicationTime = true;
                headlineInfo.publicationTimeDescriptor = DateTimeFormatter.FormatTime(headlineInfo.publicationDateTime);
                headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDateTime(headlineInfo.publicationDateTime);
            }
            else
            {
                headlineInfo.publicationDateTimeDescriptor = DateTimeFormatter.FormatLongDate(headlineInfo.publicationDateTime);
            }

            headlineInfo.wordCount = report.ReplyItem.Num.Value;
            headlineInfo.wordCountDescriptor = string.Format(
                "{0} {1}",
                NumberFormatter.Format(report.ReplyItem.Num.Value, NumberFormatType.Whole),
                ResourceText.GetString("words"));
            headlineInfo.byline = ConvertPara(report.ReplyItem.Byline.Value);
            headlineInfo.credit = ConvertPara(report.ReplyItem.Credit.Value);
            headlineInfo.copyright = ConvertPara(report.ReplyItem.Copyright.Value);

            headlineInfo.truncationRules = GetTruncationRules(report.ReplyItem.TruncRules);
            headlineInfo.title = ConvertPara(report.ReplyItem.Title.Headline.Para.Value);
            headlineInfo.snippet = ConvertPara(report.ReplyItem.Snippet.Para.Value);
        }

        private static List<Para> ConvertPara(string para)
        {
            var output = new List<Para>();
            if (String.IsNullOrEmpty(para))
                return null;

            var item = new MarkupItem
            {
                type = EntityType.Textual.ToString(),
                value = string.IsNullOrEmpty(para) ? para : para.Trim()
            };
            var tPara = new Para { items = new List<MarkupItem> { item } };
            output.Add(tPara);


            return output;
        }

        private static DateTime PublicationDateTime(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime) || dateTime.Length < 8)
                return new DateTime();


            return new DateTime(int.Parse(dateTime.Substring(0, 4)), int.Parse(dateTime.Substring(4, 2)), int.Parse(dateTime.Substring(6, 2)));
        }


        private static Truncation GetTruncationRules(TruncRulesType truncationRules)
        {
            var truncation = new Truncation();

            if (truncationRules == null)
            {
                return null;
            }
            int val;
            if (truncationRules.XS != null && !String.IsNullOrEmpty(truncationRules.XS.Value) && Int32.TryParse(truncationRules.XS.Value, out val))
            {
                truncation.Extrasmall = val;
            }

            if (truncationRules.S != null && !String.IsNullOrEmpty(truncationRules.S.Value) && Int32.TryParse(truncationRules.S.Value, out val))
            {
                truncation.Small = val;
            }

            if (truncationRules.M != null && !String.IsNullOrEmpty(truncationRules.M.Value) && Int32.TryParse(truncationRules.L.Value, out val))
            {
                truncation.Medium = val;
            }

            if (truncationRules.L != null && !String.IsNullOrEmpty(truncationRules.L.Value) && Int32.TryParse(truncationRules.L.Value, out val))
            {
                truncation.Large = val;
            }

            return truncation;
        }
    }
}
