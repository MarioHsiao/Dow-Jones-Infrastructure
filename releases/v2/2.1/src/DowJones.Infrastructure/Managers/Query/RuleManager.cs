using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Managers.QueryUtility
{
    public class RuleManager
    {
        private List<QueryManagerRule> rules;
        public RuleManager(List<QueryManagerRule> rules)
        {
            this.rules = rules;
        }

        public List<QueryManagerRule> GetAllRulesByFilterType(FilterType filterType)
        {
            var filterRules = (from r in rules where r.FilterTypes.Contains(filterType) select r).ToList();
            return filterRules;
        }
        public QueryManagerRule GetRuleByFilterType(FilterType filterType, Type ruleType)
        {
            var rule = (from r in rules where r.FilterTypes.Contains(filterType) && r.GetType() == ruleType select r).FirstOrDefault();
            return rule;
        }
    }
}
