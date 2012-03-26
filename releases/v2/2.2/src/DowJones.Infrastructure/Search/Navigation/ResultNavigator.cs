using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Search.Navigation
{
    public class ResultNavigator
    {
        private readonly CompositeNavigatorGroup _compositeNavigatorGroup;

        public Navigator Author
        {
            get { return GetNavigator(Navigator.Codes.Author); }
        }

        public Navigator Company
        {
            get { return GetNavigator(Navigator.Codes.Company); }
        }

        public Navigator Executive
        {
            get { return GetNavigator(Navigator.Codes.Executive); }
        }

        public Navigator Industry
        {
            get { return GetNavigator(Navigator.Codes.Industry); }
        }

        public IEnumerable<Keyword> Keywords { get; private set; }

        public IEnumerable<Navigator> Navigators { get; private set; }

        public SourceGroups SourceGroups { get; private set; }

        public Navigator Region
        {
            get { return GetNavigator(Navigator.Codes.Region); }
        }

        public Navigator Source
        {
            get { return GetNavigator(Navigator.Codes.Source); }
        }

        public Navigator Subject
        {
            get { return GetNavigator(Navigator.Codes.Subject); }
        }

        public CompositeNavigatorGroup CompositeNavigatorGroup
        {
            get { return _compositeNavigatorGroup; }
        }


        public ResultNavigator()
            : this(null, null, null, null)
        {
        }
        
        public ResultNavigator(SourceGroups groups, IEnumerable<Navigator> navigators, IEnumerable<Keyword> keywords, CompositeNavigatorGroup compositeNavigatorGroup)
        {
            SourceGroups = groups ?? new SourceGroups();
            Keywords = keywords ?? Enumerable.Empty<Keyword>();
            Navigators = navigators ?? Enumerable.Empty<Navigator>();
            _compositeNavigatorGroup = compositeNavigatorGroup ?? new CompositeNavigatorGroup();
        }


        private Navigator GetNavigator(string code)
        {
            return Navigators.FirstOrDefault(x => string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase));
        }
    }
}