using System.Collections.Generic;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.DataAccess
{
    public interface IContentRepository
    {
        IEnumerable<ContentSection> GetCategories();
        ContentSection GetCategory(Name name);
    }
}