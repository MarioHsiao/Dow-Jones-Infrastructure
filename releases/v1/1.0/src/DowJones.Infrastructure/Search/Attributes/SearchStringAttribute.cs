using System;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Search.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal class SearchStringInfoAttribute : Attribute
    {
        private SearchMode m_SearchMode = SearchMode.Simple;
        private SearchType m_SearchType = SearchType.Free;

        public SearchMode SearchMode
        {
            get { return m_SearchMode; }
            set { m_SearchMode = value; }
        }
        
        public SearchType SearchType
        {
            get { return m_SearchType; }
            set { m_SearchType = value; }
        }

        
        public bool Validate { get; set; }

        public bool Filter { get; set; }
        
        public string Scope { get; set; }

        public string Id { get; set; }


        public SearchStringInfoAttribute(SearchMode SearchMode, bool Validate, bool Filter, SearchType SearchType, string Scope)
        {
            m_SearchMode = SearchMode;
            this.Validate = Validate;
            this.Filter = Filter;
            m_SearchType = SearchType;
            this.Scope = Scope;
        }
    }
}