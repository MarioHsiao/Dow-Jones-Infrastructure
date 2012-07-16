using System.Collections.Generic;

namespace DowJones.Documentation.DataAccess
{
    public interface IContentRepository
    {
        IEnumerable<ContentSection> GetCategories();
        ContentSection GetCategory(Name name);
    }
}