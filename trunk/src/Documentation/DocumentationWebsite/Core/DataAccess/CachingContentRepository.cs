using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Documentation.DataAccess
{
    public class CachingContentRepository : IContentRepository
    {
        private readonly IContentRepository _repository;

        protected IDictionary<string, ContentSection> Categories
        {
            get { return _categories.Value; }
        }
        private readonly Lazy<IDictionary<string, ContentSection>> _categories;

        protected IDictionary<string, ContentSection> Pages { get; private set; }


        public CachingContentRepository(IContentRepository repository)
        {
            _repository = repository;
            _categories = new Lazy<IDictionary<string, ContentSection>>(
                () => _repository.GetCategories().ToDictionary(x => x.Name.Value, y => y));

            Pages = new Dictionary<string, ContentSection>();
        }


        public IEnumerable<ContentSection> GetCategories()
        {
            return Categories.Values;
        }

        public ContentSection GetCategory(Name name)
        {
            ContentSection category;

            var key = Categories.Keys.FirstOrDefault(x => x == name);

            // If we didn't have a cached value, try to get the non-cached value
            if (key == null || !Categories.TryGetValue(key, out category))
            {
                category = _repository.GetCategory(name);

                if(category != null)
                    Categories.Add(category.Name.Value, category);
            }

            return category;
        }

        public ContentSection GetPage(Name name, Name categoryName)
        {
            ContentSection page;

            string key = string.Format("{0}_{1}", categoryName.Value, name.Value);

            if(!Pages.TryGetValue(key, out page))
            {
                page = _repository.GetPage(name, categoryName);

                if(page != null)
                    Pages.Add(key, page);
            }

            return page;
        }
    }
}