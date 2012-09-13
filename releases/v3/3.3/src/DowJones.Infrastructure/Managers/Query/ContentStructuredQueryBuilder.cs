using System;
using System.Collections.Generic;
using DowJones.Exceptions;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using System.Linq;
using System.Text;
using DowJones.Managers.Search;
using Factiva.Gateway.Utils.V1_0;
using DowJones.Session;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Managers.QueryUtility
{
    internal class ContentStructuredQueryBuilder : StructuredQueryBuilderBase
    {
        public ContentSearchStringContext _searchStringContext = new ContentSearchStringContext();
        private SearchStringCollection _tradtionalSearchStrings = new SearchStringCollection();
        private SearchStringCollection _simpleSearchStrings = new SearchStringCollection();
        public StringBuilder mergedGroupTradtionalString = new StringBuilder();

        private bool containsSearchSectionCode = false;

        private const char DoubleQuote = '"';
        private const string ScopePE = "pe=";
        private const string ScopeFDS = "fds:";
        private const string ScopeNS = "ns=";
        private const string ScopeIN = "in=";
        private const string ScopeLA = "la=";
        private const string ScopeRE = "re=";
        private const string ScopeAU = "au=";
        private const string ScopeSN = "sn=";
        private const string ScopeFMT = "fmt=";
        private const string ScopeRST = "rst=";
        private const char OpenBraket = '(';
        private const char CloseBraket = ')';
        private const char Colon = ':';
        private const char Equal = '=';
        private const string PaddedOrString = " or ";
        private const string PaddedAndString = " and ";        
        private const string PaddedWithin3String = " w/3 ";

       /* private bool CheckElementLengthAndUpdate(bool checkBaseValue, int baseElementLength, int additionalElementLength)
        {
            // validate
            if (MaximumAllowedStructuredSearchLength <
                    CurrentLength + _currentElementLength
                    + (checkBaseValue ? baseElementLength : 0)
                    + additionalElementLength) return false;

            // increment current element length
            _currentElementLength += additionalElementLength + (checkBaseValue ? baseElementLength : 0);

            return true;
        }
        */
        private void AppendPersonAsPerScope(PersonNewsQuery person,ProximityRule rule)
        {
            bool flag2 = false;
            if (person.scope.Equals(NewsQueryScope.About)
                    || person.scope.Equals(NewsQueryScope.Occur))
            {
                flag2 = _searchStringContext.TraditionalSearchStringText.Length > 0;

                _searchStringContext.TraditionalSearchStringText
                    .Append(ScopePE)
                    .Append(person.djPersonCode);
           }
            else if (person.scope.Equals(NewsQueryScope.None))
            {
                #region Building the query

                bool hasAliases = !(person.aliasCollection == null || person.aliasCollection.Count == 0);
                bool hasLastName = !string.IsNullOrEmpty(person.lastName);

                if (hasAliases)
                {
                    // get the combined alias collection string
                    string aliasString = string.Join(PaddedOrString,
                        person.aliasCollection.Select(c => DoubleQuote + c + DoubleQuote).ToArray());

                    if (hasLastName)
                    {
                        _searchStringContext.TraditionalSearchStringText
                            .Append(OpenBraket)
                            .Append(OpenBraket);
                    }

                    _searchStringContext.TraditionalSearchStringText
                        .Append(aliasString);

                    if (hasLastName)
                    {
                        _searchStringContext.TraditionalSearchStringText
                            .Append(CloseBraket);

                        if (rule!=null)
                        {
                            _searchStringContext.TraditionalSearchStringText
                                .Append(Space)
                                .Append(GeneralUtils.GetXmlEnumName(typeof(RuleOperator),rule.RuleOperator))
                                .Append("/")
                                .Append(rule.Value.ToString())
                                .Append(Space);
                        }
                        else
                            _searchStringContext.TraditionalSearchStringText.Append(PaddedWithin3String);

                       _searchStringContext.TraditionalSearchStringText.Append(DoubleQuote)
                            .Append(person.lastName)
                            .Append(DoubleQuote)
                            .Append(CloseBraket);
                    }

                }
                else if (hasLastName)
                {
                    #region Has last name

                    _searchStringContext.TraditionalSearchStringText
                        .Append(DoubleQuote)
                        .Append(person.lastName)
                        .Append(DoubleQuote);
                    #endregion
                }
                else if (!string.IsNullOrEmpty(person.MixedValue))
                {
                    #region has mixed value like full name
                    
                    _searchStringContext.TraditionalSearchStringText
                        .Append(DoubleQuote)
                        .Append(person.MixedValue)
                        .Append(DoubleQuote);

                    #endregion
                }

                    #endregion
            }
        }

        public override bool AddPerson(Operator op, PersonNewsQuery person,ProximityRule rule)
        {
            if (person == null || person.status != 0) return true;
            if (op == Operator.Not)
            {
                //if (_searchStringContext.TraditionalSearchStringText.Length == 0) throw new Exception("Not cannot be used as a Unary Operator");

                _searchStringContext.TraditionalSearchStringText
                    .Append(Space)
                    .Append(op.ToString())
                    .Append(Space);

                AppendPersonAsPerScope(person,rule);
            }
            else if (op == Operator.And || op == Operator.Or)
            {
                AppendPersonAsPerScope(person,rule);
                _searchStringContext.TraditionalSearchStringText
                                              .Append(Space)
                                              .Append(op.ToString())
                                              .Append(Space);
            }
            

            //if (Logger.IsDebugEnabled) IncrementCurrentPersonCount();
            
            return true;
        }

        public override bool AddSourceEntities(SourceEntities srcEntities,Factiva.Gateway.Utils.V1_0.ControlData controlData)
        {
            if (srcEntities.SourceEntityCollection.Count == 0) return true;

            ProductSourceGroupConfigurationManager prodSrcConfigManager = new ProductSourceGroupConfigurationManager(ControlDataManager.Convert(controlData),null);
            _searchStringContext.TraditionalSearchStringText
                .Append(OpenBraket);

            foreach (SourceEntity srcEntity in srcEntities.SourceEntityCollection)
            {
                if (srcEntity.Type == SourceEntityType.PDF)
                {
                    _searchStringContext.TraditionalSearchStringText
                        .Append("pst=(")
                        .Append(string.Join(PaddedOrString, prodSrcConfigManager.PrimarySourceTypes("communicator", srcEntity.Value).ToArray()))
                        .Append(CloseBraket)
                        .Append(PaddedAndString);
                }
                else
                    _searchStringContext.TraditionalSearchStringText
                                                  .Append(srcEntity.Type.ToString().ToLower())
                                                  .Append(Equal)
                                                  .Append(srcEntity.Value)
                                                  .Append(PaddedAndString);
            
            }
            RemoveLastOpFromTraditional(_searchStringContext.TraditionalSearchStringText);
            _searchStringContext.TraditionalSearchStringText
               .Append(CloseBraket);

            //if (Logger.IsDebugEnabled) IncrementCurrentPersonCount();

            return true;
        }

        public override bool AddGenericTraditonalString(string Scope, Operator op, string Code)
        {
            if (Code == null) return true;

            if (Code.Length == 0) return true;

            if (op == Operator.Not)
            {
                //if (_searchStringContext.TraditionalSearchStringText.Length == 0) throw new Exception("Not cannot be used as a Unary Operator");

                _searchStringContext.TraditionalSearchStringText
                    .Append(Space)
                    .Append(op.ToString())
                    .Append(Space)
                    .Append(Scope)
                    .Append(Code);

            }
            else if (op == Operator.And || op == Operator.Or)
            {
                _searchStringContext.TraditionalSearchStringText
                    .Append(Scope)
                    .Append(Code);


                _searchStringContext.TraditionalSearchStringText
                                       .Append(Space)
                                       .Append(op.ToString());

            }
            else
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_FILTER_OPERATOR_NOT_SUPPORTED);

            _searchStringContext.TraditionalSearchStringText.Append(Space);

            return true;
        }
        

        public string MapSourceGroupCategory(SearchFormatCategory searchFormatCategory)
        {
            switch (searchFormatCategory)
            {
                case SearchFormatCategory.Publications:
                    return "(article or report or file)";
                case SearchFormatCategory.WebNews:
                    return "webpage ";
                case SearchFormatCategory.Blogs:
                    return "blog";
                case SearchFormatCategory.Boards:
                    return "board";
                case SearchFormatCategory.Internal:
                    return "customerdoc";
                case SearchFormatCategory.Multimedia:
                    return "multimedia";
                case SearchFormatCategory.Pictures:
                    return "picture";
                default:
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.QM_SEARCHFORMATCATEGORY_NOT_SUPPORTED);
            }
        }
        public override bool AddSearchFormatCategory(Operator op, SearchFormatCategory searchFormatCategory)
        {
            _searchStringContext.TraditionalSearchStringText
                .Append(OpenBraket)
                .Append(ScopeFMT)
                .Append(MapSourceGroupCategory(searchFormatCategory))
                .Append(CloseBraket)
                .Append(Space)
                .Append(op.ToString())
                .Append(Space);

            return true;
        }
        public override bool AddSourceCode(Operator op, SourceGroup sourceGroup) //old without sourceNewsQuery
        {
            if (sourceGroup == null) return true;

            if (sourceGroup.SourceCodeCollection.Count == 0) return true;

            string sourceGroupCategory = MapSourceGroupCategory(sourceGroup.SearchFormatCategory);

            _searchStringContext.TraditionalSearchStringText
               .Append(OpenBraket)
               .Append(ScopeFMT)
               .Append(sourceGroupCategory);

            if (op == Operator.Not)
                _searchStringContext.TraditionalSearchStringText
                    .Append(Space)
                    .Append(op.ToString())
                    .Append(Space);
            else
            {
                _searchStringContext.TraditionalSearchStringText
                    .Append(PaddedAndString);
            }

            _searchStringContext.TraditionalSearchStringText
                .Append(ScopeRST);


            if (sourceGroup.SourceCodeCollection.Count > 1)
                _searchStringContext.TraditionalSearchStringText.Append(OpenBraket);

            string sourceCodeStr = string.Empty;
            foreach (string sourceCode in sourceGroup.SourceCodeCollection)
            {
                sourceCodeStr += sourceCode;
                sourceCodeStr += PaddedOrString;
            }

            int index = sourceCodeStr.LastIndexOf(PaddedOrString);
            sourceCodeStr = sourceCodeStr.Substring(0, index).Trim();

            _searchStringContext.TraditionalSearchStringText.Append(sourceCodeStr);

            if (sourceGroup.SourceCodeCollection.Count > 1)
                _searchStringContext.TraditionalSearchStringText.Append(CloseBraket);

            _searchStringContext.TraditionalSearchStringText.Append(CloseBraket);

            if (!(op == Operator.Not))
            {
                _searchStringContext.TraditionalSearchStringText.Append(Space);
                _searchStringContext.TraditionalSearchStringText.Append(op.ToString());
                _searchStringContext.TraditionalSearchStringText.Append(Space);
            }
            else if (op == Operator.Not)
            {
                _searchStringContext.TraditionalSearchStringText.Append(PaddedOrString);
            }

            return true;
        }

        public override bool AddSearchAdditionalSourceFilterItem(SearchAdditionalSourceFilterItem sourceFilterItem) 
        {
            //if (sourceGroup.SourceCodeCollection.Count == 0) return true;

            string sourceGroupCategory = MapSourceGroupCategory(sourceFilterItem.SearchFormatCategory);

            _searchStringContext.TraditionalSearchStringText
               .Append(OpenBraket)
               .Append(ScopeFMT)
               .Append(sourceGroupCategory);
            
                _searchStringContext.TraditionalSearchStringText
                    .Append(PaddedAndString);
            
            _searchStringContext.TraditionalSearchStringText
                .Append(ScopeRST)
                .Append(sourceFilterItem.Code)
                .Append(CloseBraket);

            return true;
        }

        public override bool AddSearchExclusionFilterSubjectCodes(string Scope, List<string> codes)
        {
            //if (sourceGroup.SourceCodeCollection.Count == 0) return true;

            if (codes.Count == 0) return true;

            _searchStringContext.TraditionalSearchStringText
                .Append(Space)
                .Append(Operator.Not.ToString().ToLower())
               .Append(OpenBraket);

            string subjectCodeStr = string.Empty;
            foreach (string subjectCode in codes)
            {
                subjectCodeStr += ScopeNS;
                subjectCodeStr += subjectCode;
                subjectCodeStr += PaddedOrString;
            }

            int index = subjectCodeStr.LastIndexOf(PaddedOrString);
            subjectCodeStr = subjectCodeStr.Substring(0, index).Trim();

            _searchStringContext.TraditionalSearchStringText
                .Append(subjectCodeStr)
                .Append(CloseBraket);
            
            return true;
        }

       /* public void AddSourceIdToTraditional(sourceNewsQuery sourceNewsQuery,Operator op)
        {
            string sourceGroupCategory = MapSourceGroupCategory(sourceNewsQuery.searchFormatCategory);

            _searchStringContext.TraditionalSearchStringText
               .Append(OpenBraket)
               .Append(ScopeFMT)
               .Append(sourceGroupCategory);

            if (op == Operator.Not)
                _searchStringContext.TraditionalSearchStringText
                    .Append(Space)
                    .Append(op.ToString())
                    .Append(Space);
            else
            {
                _searchStringContext.TraditionalSearchStringText
                    .Append(PaddedAndString);
            }

            _searchStringContext.TraditionalSearchStringText
                .Append(ScopeRST);


            if (sourceNewsQuery.sourceId.Count > 1)
                _searchStringContext.TraditionalSearchStringText.Append(OpenBraket);

            string sourceCodeStr = string.Empty;
            foreach (string sourceCode in sourceNewsQuery.sourceId)
            {
                sourceCodeStr += sourceCode;
                sourceCodeStr += PaddedOrString;
            }

            int index = sourceCodeStr.LastIndexOf(PaddedOrString);
            sourceCodeStr = sourceCodeStr.Substring(0, index).Trim();

            _searchStringContext.TraditionalSearchStringText.Append(sourceCodeStr);

            if (sourceNewsQuery.sourceId.Count > 1)
                _searchStringContext.TraditionalSearchStringText.Append(CloseBraket);

            _searchStringContext.TraditionalSearchStringText.Append(CloseBraket);

            if (!(op == Operator.Not))
            {
                _searchStringContext.TraditionalSearchStringText.Append(Space);
                _searchStringContext.TraditionalSearchStringText.Append(op.ToString());
                _searchStringContext.TraditionalSearchStringText.Append(Space);
            }
            else if (op == Operator.Not)
            {
                _searchStringContext.TraditionalSearchStringText.Append(PaddedOrString);
            }

        }

        public void AddSourceNameToTraditional(sourceNewsQuery sourceNewsQuery,Operator op)
        {
            
            foreach (string sourceName in sourceNewsQuery.sourceName)
            {
                _searchStringContext.TraditionalSearchStringText
                   .Append(ScopeSN)
                   .Append(sourceName);

                if (op == Operator.Not)
                    _searchStringContext.TraditionalSearchStringText
                        .Append(Space)
                        .Append(op.ToString())
                        .Append(Space);
                else
                    _searchStringContext.TraditionalSearchStringText.Append(PaddedOrString);                                                      
            }
            RemoveLastOpFromTraditional(_searchStringContext.TraditionalSearchStringText,Operator.Or);           
        }

        public override bool AddSourceCode(Operator op, SourceGroup sourceGroup)
        {
            if (sourceGroup == null) return true;

            if (sourceGroup.SourceNewsQueries.Count == 0) return true;

            foreach (var sourceNewsQuery in sourceGroup.SourceNewsQueries)
            {
                if (sourceNewsQuery.sourceId.Count > 0)
                    AddSourceIdToTraditional(sourceNewsQuery, op);
                else if (sourceNewsQuery.sourceName.Count > 0)
                {
                    if (op == Operator.Not)
                    {
                        RemoveLastOpFromTraditional(_searchStringContext.TraditionalSearchStringText);
                        AddOperatorToTraditionalString(op);
                    }
                    AddSourceNameToTraditional(sourceNewsQuery, op);
                }
                                    
            }
            return true;
        }
        */
        public override void AddCloseBracketToTraditionalString()
        {
            _searchStringContext.TraditionalSearchStringText.Append(CloseBraket);
        }

        public override void AddOpenBracketToTraditionalString()
        {
            _searchStringContext.TraditionalSearchStringText.Append(OpenBraket);
        }


        public override void AddOperatorToTraditionalString(Operator op)
        {
            if (_searchStringContext.TraditionalSearchStringText.ToString().Trim().Length >0) 
                _searchStringContext.TraditionalSearchStringText
                    .Append(Space)
                    .Append(op.ToString())
                    .Append(Space);
        }

        public override void AddOperatorToMergedString(Operator op)
        {
            if (mergedGroupTradtionalString.ToString().Trim().Length > 0)
                mergedGroupTradtionalString
                    .Append(Space)
                    .Append(op.ToString())
                    .Append(Space);
        }
        public override bool AddSearchSectionCode(Operator op, SearchSection searchSectionCode)
        {
            if (searchSectionCode == SearchSection.FullArticle) return true;

            containsSearchSectionCode = true;

            string searchSectionCodeStr = GeneralUtils.GetXmlEnumName<SearchSection>(searchSectionCode);
            
            bool flag2 = _searchStringContext.TraditionalSearchStringText.Length > 0;

            if (flag2) _searchStringContext.TraditionalSearchStringText.Append(Space);


            _searchStringContext.TraditionalSearchStringText
                .Append(searchSectionCodeStr)
                .Append(Equal)
                .Append(OpenBraket);


            return true;
        }

        private void AppendAboutOccurNoneString(NewsQueryScope scope, string djOrgCode,string orgNameSearch)
        { 
            
            bool flag1;
            if (scope.Equals(NewsQueryScope.About)
                        || scope.Equals(NewsQueryScope.Occur))
                {
                    _searchStringContext.TraditionalSearchStringText
                        .Append(ScopeFDS)
                        .Append(scope.ToString().ToLower())
                        .Append(Equal)
                        .Append(djOrgCode);
            }
             else if (scope.Equals(NewsQueryScope.None))
                {
                    // trim organization name
                    string orgName = orgNameSearch.Trim();

                    // person last name may have more than one word
                    flag1 = HasMoreThanOneWord(orgName);
                    if (flag1)
                    {
                        _searchStringContext.TraditionalSearchStringText
                            .Append(DoubleQuote)
                            .Append(orgName)
                            .Append(DoubleQuote);
                    }
                    else
                    {
                        _searchStringContext.TraditionalSearchStringText.Append(orgName);
                    }
                }

        }
        public override bool AddOrganization(Operator op, companyNewsQuery company)
        {
            if (company == null || company.status != 0) return true;
        
            if (op == Operator.Not)
            {
                //if (_searchStringContext.TraditionalSearchStringText.Length == 0) throw new Exception("Not cannot be used as a Unary Operator");

                _searchStringContext.TraditionalSearchStringText
                    .Append(Space)
                    .Append(op.ToString())
                    .Append(Space);
                     
                AppendAboutOccurNoneString(company.scope,company.djOrgCode,company.orgNameSearch);
            }
            else if (op == Operator.And || op == Operator.Or)
            {
                AppendAboutOccurNoneString(company.scope, company.djOrgCode, company.orgNameSearch);
                _searchStringContext.TraditionalSearchStringText
                                              .Append(Space)
                                              .Append(op.ToString())
                                              .Append(Space);
            }
            //if (Logger.IsDebugEnabled) IncrementCurrentOrganizationCount();
            return true;
        }

        public override bool AddFreeText(Operator op, string freeText)
        {
            if (freeText == null) return true;

            string freeTextTrimmed = freeText.Trim();

            if (freeTextTrimmed.Length == 0) return true;
            if (op == Operator.Not)
            {
                //if (_searchStringContext.TraditionalSearchStringText.Length == 0) throw new Exception("Not cannot be used as a Unary Operator");

                _searchStringContext.TraditionalSearchStringText
                    .Append(Space)
                    .Append(op.ToString())
                    .Append(Space)
                    .Append(freeTextTrimmed);
                    
           }
            else if (op == Operator.And || op == Operator.Or)
            {

                _searchStringContext.TraditionalSearchStringText
                    .Append(freeTextTrimmed);

                _searchStringContext.TraditionalSearchStringText
                        .Append(Space)
                        .Append(op.ToString())
                        .Append(Space);
            }
            
           // if (Logger.IsDebugEnabled) IncrementCurrentFreeTextCount();

            return true;
        }

        
        public override bool AddTriggerType(Operator op, string triggerTypeCode)
        {
            // do nothing
            return true;
        }

        public override bool AddKeywordsFilterText(SearchMode mode, string freeText, string id)
        {
            if (freeText == null) return true;
            string freeTextTrimmed = freeText.Trim();

            if (freeTextTrimmed.Length == 0) return true;

            _simpleSearchStrings.Add(CreateFreeTextSearchString(freeText, mode, id));

            return true;
        }

        public override bool AddTriggerType(Operator op, List<QueryTriggerTypeValue> triggerTypeValues)
        {
            // do nothing
            return true;
        }
        public override void CheckSearchSectionForFreeText()
        {
            if (containsSearchSectionCode)
                _searchStringContext.TraditionalSearchStringText.Append(CloseBraket);
        }

        private Operator GetConnectingOperatorFromGroup(Group group)
        {
            Operator connectingOp = new Operator();

            if (group.FilterGroup is AndFilterGroup) connectingOp = Operator.And;
            else if (group.FilterGroup is OrFilterGroup) connectingOp = Operator.Or;
            else if (group.FilterGroup is NotFilterGroup) connectingOp = Operator.Not;

            return connectingOp;
        }
        public void MergeWithOperator(Operator connectingOp,GroupOperator groupOperator)
        {
            try
            {
                if (_tradtionalSearchStrings.Count > 0) mergedGroupTradtionalString.Append(OpenBraket);

                foreach (SearchString searchString in _tradtionalSearchStrings)
                {
                    if (!searchString.Value.StartsWith("not", StringComparison.CurrentCultureIgnoreCase))
                    {
                        mergedGroupTradtionalString.Append(OpenBraket);
                    }
                    else
                    {
                        RemoveLastOpFromTraditional(mergedGroupTradtionalString);
                        mergedGroupTradtionalString.Append(Space);
                    }

                    mergedGroupTradtionalString.Append(searchString.Value);

                    if (!searchString.Value.StartsWith("not", StringComparison.CurrentCultureIgnoreCase))
                    {
                        mergedGroupTradtionalString.Append(CloseBraket);
                    }


                    mergedGroupTradtionalString.Append(Space)
                        .Append(connectingOp.ToString())
                        .Append(Space);
                }

                RemoveLastOpFromTraditional(mergedGroupTradtionalString, connectingOp);

                if (_tradtionalSearchStrings.Count > 0) mergedGroupTradtionalString.Append(CloseBraket);

                mergedGroupTradtionalString.Append(Space)
                    .Append(groupOperator.ToString())
                    .Append(Space);

            }

            finally
            {
                _tradtionalSearchStrings.Clear();
            }
        }
        public override void MergeGroups(Group group)
        {
            Operator connectingOp = GetConnectingOperatorFromGroup(group);

            MergeWithOperator(connectingOp,group.Operator);
         
        }
        
        public override void CommitSearchString(string id)
        {
            try{
             if (_searchStringContext.TraditionalSearchStringText.Length > 0)
                {
                    SearchString ssTemp = CreateFreeTextSearchString(_searchStringContext.TraditionalSearchStringText.ToString(), SearchMode.Traditional,id);
                    if (ssTemp != null) _tradtionalSearchStrings.Add(ssTemp);
                }

                if (_searchStringContext.AdditionalSearchStrings.Count > 0)
                {
                    StructuredSearch.Query.SearchStringCollection.AddRange(_searchStringContext.AdditionalSearchStrings);
                }
            }
            finally{
            //CurrentLength += _currentElementLength;
                //if (Logger.IsDebugEnabled) CommitCurrentCounts();
                _searchStringContext = new ContentSearchStringContext();
                //_currentElementLength = 0;
            }
        }

        public override void CommitMergedString()
        {
            try
            {
                /*if (noMergeRequired)
                {
                    StructuredSearch.Query.SearchStringCollection.AddRange(_tradtionalSearchStrings.ToList());
                }
                else
                {*/
                    if (mergedGroupTradtionalString.Length > 0)
                    {
                        SearchString ssTemp = CreateFreeTextSearchString(mergedGroupTradtionalString.ToString(), SearchMode.Traditional,"");
                        if (ssTemp != null) StructuredSearch.Query.SearchStringCollection.Add(ssTemp);
                    }
                //}
                if (_searchStringContext.AdditionalSearchStrings.Count > 0)
                {
                    StructuredSearch.Query.SearchStringCollection.AddRange(_searchStringContext.AdditionalSearchStrings);
                }
                if (_simpleSearchStrings.Count > 0)
                {
                    StructuredSearch.Query.SearchStringCollection.AddRange(_simpleSearchStrings);
                }
            }
            finally
            {
                //CurrentLength += _currentElementLength;
                //if (Logger.IsDebugEnabled) CommitCurrentCounts();
                mergedGroupTradtionalString = new StringBuilder();
                //_currentElementLength = 0;
            }
        }
        /*public override void CommitSearchString(Operator op)
        {
            try
            {
                SearchMode searchMode = SearchMode.All;
                if (op.Equals(Operator.Or))
                {
                    searchMode = SearchMode.Any;
                }
                else if (op.Equals(Operator.And))
                {
                    searchMode = SearchMode.All;
                }
                else if (op.Equals(Operator.Not))
                {
                    searchMode = SearchMode.None;
                }
                else
                {
                    throw new NotSupportedException(op.ToString() + " filter operator is not supported.");
                }

                if (_searchStringContext.SearchStringText.Length > 0)
                {
                    SearchString ssTemp = CreateFreeTextSearchString(_searchStringContext.SearchStringText.ToString(), searchMode);
                    if (ssTemp != null) StructuredSearch.Query.SearchStringCollection.Add(ssTemp);
                }

                if (_searchStringContext.TraditionalSearchStringText.Length > 0)
                {
                    SearchString ssTemp = CreateFreeTextSearchString(_searchStringContext.TraditionalSearchStringText.ToString(), SearchMode.Traditional);
                    if (ssTemp != null) StructuredSearch.Query.SearchStringCollection.Add(ssTemp);
                }

                if (_searchStringContext.AdditionalSearchStrings.Count > 0)
                {
                    StructuredSearch.Query.SearchStringCollection.AddRange(_searchStringContext.AdditionalSearchStrings);
                }
            }
            finally
            {
                //CurrentLength += _currentElementLength;
                //if (Logger.IsDebugEnabled) CommitCurrentCounts();
                _searchStringContext = new ContentSearchStringContext();
                //_currentElementLength = 0;
            }
        }
        */


    }
}
