using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.Search;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Web.Showcase.Mocks
{
    public class MockSearchQueryResourceManager : ISearchQueryResourceManager
    {
        #region ISearchQueryResourceManager Members

        public IEnumerable<string> PrimarySourceTypesByProductId(string productId)
        {
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> PrimarySourceTypesByProductDefineCode(string productId, string sourceGoupId)
        {
            return Enumerable.Empty<string>();
        }

        public IEnumerable<Group> SourceList(string id)
        {
            return Enumerable.Empty<Group>();
        }

        #endregion
    }
}