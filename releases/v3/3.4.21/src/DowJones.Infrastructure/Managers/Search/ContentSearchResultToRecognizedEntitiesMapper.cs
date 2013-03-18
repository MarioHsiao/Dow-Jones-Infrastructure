using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DowJones.Mapping;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search
{
    public class ContentSearchResultToRecognizedEntitiesMapper
        : TypeMapper<ContentSearchResult, RecognizedEntities>
    {
        public override RecognizedEntities Map(ContentSearchResult source)
        {
            if (source == null || source.QueryTransformSet == null)
                return null;

            return GetCompanySymbolRecognition(source.QueryTransformSet);
        }

        private static RecognizedEntities GetCompanySymbolRecognition(QueryTransformSet queryTransformSet)
        {
            if (queryTransformSet == null || queryTransformSet.QueryTransformCollection == null)
            {
                return null;
            }
            
            var spellCorrectedWords = GetSpellCorrectedWords(queryTransformSet);
            var entityRecognitionSet = new RecognizedEntities { SpellCorrection = spellCorrectedWords };

            //If spell is detected then there is no need to parse other entities for DYM
            if (spellCorrectedWords == null)
            {
                QueryTransform transforms = (from transform in queryTransformSet.QueryTransformCollection
                                             where
                                                 transform.Reason == QueryTransformReason.MetadataNameRecognition ||
                                                 transform.Reason == QueryTransformReason.CompanySymbolRecognition
                                             select transform).ToList().FirstOrDefault();

                if (transforms != null && transforms.Description != null && transforms.Description.Any != null)
                {
                    FindEntities(entityRecognitionSet, transforms.Description.Any[0]);
                }
            }

            return entityRecognitionSet;
        }

        private static string GetSpellCorrectedWords(QueryTransformSet searchResult)
        {
            if (searchResult != null && searchResult.QueryTransformCollection != null)
            {
                return (from transform in searchResult.QueryTransformCollection
                        where transform.Reason == QueryTransformReason.SpellCorrection
                        select transform.SearchString.Value).FirstOrDefault();
            }
            return null;
        }

        private static void FindEntities(RecognizedEntities recognizedEntities, XmlNode xmlElement)
        {
            var xdoc = new XmlDocument();
            xdoc.LoadXml(xmlElement.InnerXml);
            var sb = new StringBuilder();
            var list = new List<RecognizedEntity>();
            if (xdoc.DocumentElement != null)
                foreach (XmlNode xNode in xdoc.DocumentElement.ChildNodes)
                {
                    switch (xNode.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xNode.Attributes != null)
                            {
                                var scope = xNode.Attributes["scope"].Value;
                                var code = xNode.Attributes["code"].Value;
                                var term = xNode.Attributes["searchTerm"].Value;
                                var name = xNode.InnerText;
                                list.Add(new RecognizedEntity { Code = code, Name = name, SearchTerm = term, Type = scope });
                            }
                            break;
                        case XmlNodeType.Text:
                            list.Add(new RecognizedEntity { SearchTerm = xNode.Value.Trim(), Type = "text" });
                            break;
                    }
                }
            recognizedEntities.Entities = list;
            if (sb.Length > 0)
            {
                recognizedEntities.SearchText = sb.Remove(sb.Length - 1, 1).ToString();
            }
        }
    }
}