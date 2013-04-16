using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Search;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Search.UI.Components.Builders.Simple
{
    public class SimpleSearchBuilder : SearchBuilder
    {                                                 
        public IEnumerable<SelectListItem> SaveOptions { get; set; }

        public string Source { get; set; }

        public string SourceText { get; set; }

        public SourceCollection SourceCollection { get; set; }   

        public string SpellCorrection { get; set; }   

        public DidYouMean.DidYouMean DidYouMean { get; set; }

        public IEnumerable<InclusionFilter> Inclusions { get; set; }

        public bool HasRecognizedEntities
        {
            get
            {
                return DidYouMean != null && DidYouMean.Groups.Any();
            }
        }

        public string SearchText { get; set; }

        [ClientProperty("suggestServiceUrl")]
        public string SuggestServiceUrl { get; set; }   
    }
}