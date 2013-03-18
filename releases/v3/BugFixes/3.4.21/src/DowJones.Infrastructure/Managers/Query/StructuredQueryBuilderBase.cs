using System;
using System.Collections.Generic;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Screening.V1_1;
using System.Text;
using DowJones.Exceptions;
using Factiva.Gateway.Utils.V1_0;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Managers.QueryUtility
{
    public abstract class StructuredQueryBuilderBase
    {
        #region Constants

        protected const char SpaceChar = ' ';

        #endregion
        #region Protected variables

        private StructuredSearch _structuredSearch;
        private int _currentLength;
        //private Query _query;

        
        protected const string Space = " ";

        #endregion

        #region Public properties

        public int CurrentLength
        {
            get { return _currentLength; }
            set { _currentLength = value; }
        }

        public StructuredSearch StructuredSearch
        {
            get { return _structuredSearch; }
            set
            {
                _structuredSearch = value;

                // ensure that query is set
                if (_structuredSearch.Query == null) _structuredSearch.Query = new StructuredQuery();

                // calculate the initial query size
                CurrentLength = GeneralUtils.serialize(_structuredSearch, true, false).Length - 1;
            }
        }

        

        #endregion

        #region Virtual and abstract functions

        protected static bool HasMoreThanOneWord(string s)
        {
            return string.IsNullOrEmpty(s) ? false : (s.Trim().IndexOf(SpaceChar) > 1);
        }

        public virtual bool AddDates(DateDTO dtDTO,DateFilterTarget filterTarget)
        {
            // do SinceDays first and then look for custom date range.
            if (dtDTO != null)
            {
                Dates dt = null;

                if (dtDTO.dateDtoType.Equals(DateDtoType.DateSinceDays))
                {
                    dt = CreateDates(dtDTO.dateRange, null, null, dtDTO.dateFormat, dtDTO.SinceDays,filterTarget);
                }
                else if (dtDTO.dateDtoType.Equals(DateDtoType.DateRange))
                {
                    dt = CreateDates(dtDTO.dateRange, dtDTO.fromDate, dtDTO.toDate, dtDTO.dateFormat, null,filterTarget);
                }

                //if (ValidateQueryLength)
                //{
                //    int len = GeneralUtils.serialize(dt).Length;

                //    if (MaximumAllowedStructuredSearchLength < CurrentLength + len) return false;

                //    CurrentLength += len;
                //}

                _structuredSearch.Query.Dates = dt;
            }

            return true;
        }

        protected static Dates CreateDates(SearchDateRange dateRange, YearMonthDay fromDate, YearMonthDay toDate, DateFormat dateFormat, string SinceDays,DateFilterTarget filterTarget)
        {
            Dates dates = new Dates { Type = MapDateType(filterTarget) };
            switch (dateRange)
            {
                case SearchDateRange.Custom:
                    if (fromDate.Equals(toDate))
                        dates.EqualsDate = GetFormattedDateString(fromDate, dateFormat);
                    else
                    {
                        dates.Range = new Factiva.Gateway.Messages.Search.V2_0.DateRange
                        {
                            To = GetFormattedDateString(toDate, dateFormat),
                            From = GetFormattedDateString(fromDate, dateFormat)
                        };
                        
                    }
                    break;
                case SearchDateRange._Unspecified:
                    dates.All = true;
                    break;
                default:
                    dates.After = SinceDays;
                    dates.__typeSpecified = false;
                    break;
            }
            dates.Format = MapDateFormat(dateFormat);
            return dates;
        }

        protected static string GetFormattedDateString(YearMonthDay yearMonthDay, DateFormat dateFormat)
        {
            switch (dateFormat)
            {
                case DateFormat.CCYYMMDD:
                    return String.Format("{0}{1}{2}",
                                         (yearMonthDay.year > 0) ? yearMonthDay.year.ToString("0000") : "",
                                         (yearMonthDay.month > 0) ? yearMonthDay.month.ToString("00") : "",
                                         (yearMonthDay.day > 0) ? yearMonthDay.day.ToString("00") : "");
                case DateFormat.DDMMCCYY:
                    return String.Format("{0}/{1}/{2}",
                                         yearMonthDay.day.ToString("00"),
                                         yearMonthDay.month.ToString("00"),
                                         yearMonthDay.year.ToString("0000"));
                case DateFormat.MMDDCCYY:
                    return String.Format("{0}/{1}/{2}",
                                         yearMonthDay.month.ToString("00"),
                                         yearMonthDay.day.ToString("00"),
                                         yearMonthDay.year.ToString("0000"));
            }
            return null;
        }

        protected static Factiva.Gateway.Messages.Search.V2_0.DateFormat MapDateFormat(DateFormat dateFormat)
        {
            switch (dateFormat)
            {
                case DateFormat.DDMMCCYY:
                    return Factiva.Gateway.Messages.Search.V2_0.DateFormat.DDMMCCYY;
                default:
                    return Factiva.Gateway.Messages.Search.V2_0.DateFormat.MMDDCCYY;
            }
        }
        protected static Factiva.Gateway.Messages.Search.V2_0.DateType MapDateType(DateFilterTarget dateTargetType)
        {
            Factiva.Gateway.Messages.Search.V2_0.DateType dateType = new Factiva.Gateway.Messages.Search.V2_0.DateType();
            switch (dateTargetType)
            {
                case DateFilterTarget.PublicationDate:
                    dateType = Factiva.Gateway.Messages.Search.V2_0.DateType.Publication;
                    break;
                case DateFilterTarget.AccessionDate:
                    dateType = Factiva.Gateway.Messages.Search.V2_0.DateType.Accession;
                    break;
                default:
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_DATE_TYPE_NOT_SPECIFIED);
                    
            }
            return dateType;
        }
            

        protected static SearchString CreateFreeTextSearchString(string strData, SearchMode searchMode,string id)
        {
            if (string.IsNullOrEmpty(strData.Trim())) return null;

            SearchString searchString = new SearchString
            {
                Type = SearchType.Free,
                Value = strData.Trim(),
                Mode = searchMode

            };
            if (!string.IsNullOrEmpty(id))                
                searchString.Id = id;

            return searchString;
        }

        public abstract bool AddPerson(Operator op, PersonNewsQuery person,ProximityRule rule);

        public abstract bool AddOrganization(Operator op, companyNewsQuery company);

        public abstract bool AddGenericTraditonalString(string Scope, Operator op, string Code);

        public abstract bool AddFreeText(Operator op, string freeText);

        public abstract bool AddSourceCode(Operator op, SourceGroup sourceGroup);

        public abstract bool AddSearchAdditionalSourceFilterItem(SearchAdditionalSourceFilterItem sourceFilterItem);

        public abstract bool AddSearchExclusionFilterSubjectCodes(string Scope, List<string> codes);

        public abstract bool AddSearchFormatCategory(Operator op, SearchFormatCategory searchFormatCategory);

        public abstract bool AddSearchSectionCode(Operator op, SearchSection searchSectionCode);

        public abstract bool AddKeywordsFilterText(SearchMode mode, string freeText, string id);

        public abstract bool AddSourceEntities(SourceEntities srcEntities,ControlData controlData);

        public abstract bool AddTriggerType(Operator op, string triggerTypeCode);

        public abstract bool AddTriggerType(Operator op, List<QueryTriggerTypeValue> triggerTypeValues);

        //public abstract void CommitSearchString(Operator op);
        public abstract void MergeGroups(Group group);

        public abstract void CommitSearchString(string id);

        public abstract void CommitMergedString();

        public abstract void CheckSearchSectionForFreeText();

        public abstract void AddCloseBracketToTraditionalString();

        public abstract void AddOpenBracketToTraditionalString();

        public abstract void AddOperatorToTraditionalString(Operator op);

        public abstract void AddOperatorToMergedString(Operator op);

        public virtual void RemoveLastOpFromTraditional(StringBuilder tradtionalString,Operator op)
        {
            string str = tradtionalString.ToString().Trim();
            if (str.EndsWith(op.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                int index = str.LastIndexOf(op.ToString(),StringComparison.CurrentCultureIgnoreCase);
                str = str.Substring(0, index).Trim();
                tradtionalString.Remove(0, tradtionalString.Length - 1);
                tradtionalString.Append(str);
            }
        }

        private Operator GetLastOperator(string str)
        {
            Operator op = new Operator();
            if (str.EndsWith(Operator.Or.ToString(), StringComparison.CurrentCultureIgnoreCase)) op = Operator.Or;
            if (str.EndsWith(Operator.And.ToString(), StringComparison.CurrentCultureIgnoreCase)) op = Operator.And;
            if (str.EndsWith(Operator.Not.ToString(), StringComparison.CurrentCultureIgnoreCase)) op = Operator.Not;
            return op;
        }
        public virtual void RemoveLastOpFromTraditional(StringBuilder tradtionalString)
        {
            string str = tradtionalString.ToString().Trim();
            Operator op = GetLastOperator(str);
            int index = str.LastIndexOf(op.ToString(),StringComparison.CurrentCultureIgnoreCase);
            if (index >= 0)
            {
                str = str.Substring(0, index).Trim();
                tradtionalString.Remove(0, tradtionalString.Length - 1);
                tradtionalString.Append(str);
            }
        }


        

        #endregion
    }
}
