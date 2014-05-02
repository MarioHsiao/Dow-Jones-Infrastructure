using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Mapping;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Requests.Mappers;

namespace DowJones.Web.Mvc.Search.Requests.Alert
{
    

    public class AlertSearchRequestMapper : TypeMapper<AlertSearchRequest, AbstractBaseSearchQuery>
    {
        private readonly SearchRequestMapper _baseMapper;

        public AlertSearchRequestMapper(SearchRequestMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override AbstractBaseSearchQuery Map(AlertSearchRequest source)
        {
            var viewType = String.IsNullOrEmpty(source.Sessionmark)? source.ViewType : AlertHeadlineViewType.Session;

            var query = new AlertSearchQuery
                            {
                                AlertId = source.AlertId,
                                Keywords = source.FreeText,
                                ViewType = viewType,
                                Sessionmark =  source.Sessionmark,
                                ResetSessionmark =  viewType == AlertHeadlineViewType.New,
                                Bookmark = source.Bookmark,
                                Sort = source.Sort.GetValueOrDefault()
            };

            _baseMapper.Map(query, source);

            //Commented it as a part of Alert Dedup enhancement
            ////No duplication logic for alert result!
            if (!source.ShowDuplicates.HasValue)
            {
                query.Duplicates = DeduplicationMode.Off;
            }

            return query;
        }
    }
}
