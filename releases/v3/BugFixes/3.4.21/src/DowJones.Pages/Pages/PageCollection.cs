using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Pages
{
    public class PageCollection : IEnumerable<Page>
    {
        private readonly IList<Page> _pages;

        public PageCollection()
        {
        }

        public PageCollection(IEnumerable<Page> pages)
        {
            _pages = new List<Page>(pages ?? Enumerable.Empty<Page>());
        }

        public Page DefaultPage
        {
            get
            {
                var defaultPage = _pages.FirstOrDefault(x => x.IsDefault);

                if (defaultPage == null)
                    defaultPage = _pages.OrderBy(x => x.Position).FirstOrDefault();

                return defaultPage;
            }
        }

        public IEnumerator<Page> GetEnumerator()
        {
            return _pages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
