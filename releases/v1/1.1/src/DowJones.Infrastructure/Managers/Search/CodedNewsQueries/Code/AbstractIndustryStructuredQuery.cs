using System;
using System.Collections.Generic;
using Factiva.Gateway.Messages.CodedNews.CodedNewsQueries;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.Search.V2_0;
using DateFormat = Factiva.Gateway.Messages.Search.V2_0.DateFormat;

namespace Factiva.Gateway.Messages.CodedNews
{
    public class AbstractIndustryStructuredQuery : AbstractStructuredQuery, IIndustryQuery
    {
        protected readonly List<SearchString> ListSearchString = new List<SearchString>();
        protected StructuredQuery query;

        #region IIndustryQuery Members

        public Industry Industry { get; set; }

        public override StructuredQuery ExpandQuery
        {
            get
            {
                return null;
            }
        }

        #endregion

        protected override StructuredQuery BuildQuery()
        {
            if (query != null)
            {
                return query;
            }
            ValidateAndUpdate();
            BuildSearchStringList();
            query = new StructuredQuery();

            //Apply Filter 
            AddFilters(ListSearchString);

            query.SearchCollectionCollection.Add(SearchCollection.Publications);
            query.Dates = GetDefaultDateOption();

            if (LanguagePreference != null && LanguagePreference.Length > 0)
                ListSearchString.Add(GetLanguageFilter(LanguagePreference));



            query.SearchStringCollection.AddRange(ListSearchString.ToArray());
            return query;
        }

        private static void ValidateAndUpdate()
        {
         
        }

        protected virtual void BuildSearchStringList()
        {
        }

        protected override Dates GetDefaultDateOption()
        {
            var date = new Dates
                           {
                               After = DATE_RANGE_LAST_1YEAR, 
                               Format = DateFormat.MMDDCCYY
                           };
            return date;
        }
    }

    public class IndustryQueryInfo
    {
        private readonly string restrictors;
        private readonly int wordCount;

        //Comma separated restrictor codes
        public IndustryQueryInfo(string restrictors, int wordCount)
        {
            this.restrictors = String.IsNullOrEmpty(restrictors) ? String.Empty : restrictors.Trim();
            this.wordCount = wordCount;
        }

        public int WordCount
        {
            get { return wordCount; }
        }

        public string[] Restrictors
        {
            get { return restrictors.Split(','); }
        }
    }
}