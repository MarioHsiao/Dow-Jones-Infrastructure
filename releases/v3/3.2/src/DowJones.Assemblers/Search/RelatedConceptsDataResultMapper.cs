// -----------------------------------------------------------------------
// <copyright file="RelatedConceptsDataResultMapper.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using DowJones.Managers.RelatedConcept;
using DowJones.Mapping;
using DowJones.Models.Search;

namespace DowJones.Assemblers.Search
{                       
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RelatedConceptsDataResultMapper : TypeMapper<ConceptSearchResult, RelatedConceptsDataResult>
    {
        public override RelatedConceptsDataResult Map(ConceptSearchResult source)
        {
            if (source != null &&
                source.conceptSearchResultInfo != null &&
                source.conceptSearchResultInfo.Length > 0)
            {
                return new RelatedConceptsDataResult()
                                {
                                    Terms = source.conceptSearchResultInfo.Select(conceptSearchResultInfo => new Term
                                                                                                       {
                                                                                                           Text = conceptSearchResultInfo.term, Weight = conceptSearchResultInfo.score,
                                                                                                       }).ToList(),
                                };
            }

            return null;
        }               
    }
}
