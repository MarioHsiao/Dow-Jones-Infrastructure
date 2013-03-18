using System;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Pages.Mappers
{
    public class PageListInfoMapper : PageMapperBase, 
                                         Mapping.ITypeMapper<PageListInfo, Page>,
                                         Mapping.ITypeMapper<PageListInfoEx, Page>
    {
        public object Map(object source)
        {
            if (source is PageListInfoEx)
                return Map((PageListInfoEx)source);

            if (source is PageListInfo)
                return Map((PageListInfo) source);

            throw new NotSupportedException();
        }

        public Page Map(PageListInfo source)
        {
            return Map(source as PageListInfoEx);
        }

        public Page Map(PageListInfoEx source)
        {
            if (source == null)
                return null;

            var newsPage = CreatePageModel<Page>(
                source.PageProperties,
                source.ShareProperties as SharePropertiesResponse,
                source.PageQualifier,
                source.Id.ToString()
            );

            return newsPage;
        }
    }
}