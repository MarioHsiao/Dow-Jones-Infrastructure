using DowJones.Mapping;
using DowJones.Search;

namespace DowJones.Web.Mvc.Search.Requests.Mappers
{
    public class EntityTypeMapper : TypeMapper<NewsFilterCategory, EntityType>
    {
        public override EntityType Map(NewsFilterCategory category)
        {
            switch (category)
            {
                case NewsFilterCategory.Author:
                    return EntityType.Author;
                case NewsFilterCategory.Company:
                    return EntityType.Company;
                case NewsFilterCategory.Executive:
                    return EntityType.Executive;
                case NewsFilterCategory.Industry:
                    return EntityType.Industry;
                case NewsFilterCategory.Region:
                    return EntityType.Region;
                case NewsFilterCategory.Subject:
                    return EntityType.Subject;
                default:
                    return EntityType.Unknown;
            }
        }
    }
}