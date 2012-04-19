using System;
using System.Linq;
using DowJones.Managers.Search;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Components.CompositeHeadline;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.Web.Mvc.UI.Components.Search.Mappers
{
    public class SearchResponseToCompositeHeadlineModelMapper : TypeMapper<SearchResponse, CompositeHeadlineModel>
    {
        public override CompositeHeadlineModel Map(SearchResponse source)
        {
            if (source == null || source.Results == null || source.Results.resultSet.count.Value == 0)
                return new CompositeHeadlineModel();

            return MapHeadlinesToModel(source);
        }

        private static CompositeHeadlineModel MapHeadlinesToModel(SearchResponse source)
        {
            var model = new CompositeHeadlineModel()
                            {
                                FirstResultIndex = Convert.ToInt32(source.Results.resultSet.first.Value),
                                LastResultIndex = Convert.ToInt32(source.Results.resultSet.count.Value + source.Results.resultSet.first.Value),
                                TotalResultCount = Convert.ToInt32(source.Results.hitCount.Value),
                            };

            var headlineListModel = new HeadlineListModel();
            headlineListModel.ShowCheckboxes = true;
            headlineListModel.ShowDuplicates = ShowDuplicates.On;
            headlineListModel.TotalRecords = Convert.ToInt32(source.Results.hitCount.Value);
            headlineListModel.Headlines = source.Results.resultSet.headlines.Select(h => new HeadlineModel(h));
            model.DuplicateCount = source.Results.resultSet.duplicateCount.Value;
            model.HeadlineList = headlineListModel;
            model.ShowHeadlineViewOptions = true;
            model.ShowPostProcessing = true;
           
            model.PostProcessing = new PostProcessing(new[] {
                                    PostProcessingOptions.Email,
                                    PostProcessingOptions.Print,
                                    PostProcessingOptions.Read,
                                    PostProcessingOptions.Save,
                                });
            model.PostProcessing.EnableDuplicateOption = true;
            return model;
        }
    }

}
