using System;
using System.Linq;
using System.Web.Mvc;
using DowJones.Managers.Search;
using DowJones.Models.Search;
using DowJones.Search;
using DowJones.Search.Attributes;
using DowJones.Web.Mvc.UI.Components.Models.Discovery;
using Factiva.Gateway.Messages.Search.V2_0;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;
using DeduplicationMode = Factiva.Gateway.Messages.Search.V2_0.DeduplicationMode;   

namespace DowJones.Web.Showcase.Controllers
{
    public class DateHistogramController : ControllerBase
    {
        public ActionResult Index()
        {
            return View("Index", GetBasicModel());
        }

        /// <summary>
        /// The get perform content search request.
        /// </summary>
        /// <param name="searchText">
        /// The search text.
        /// </param>
        /// <returns>
        /// A PerformContentSearchRequest Object. 
        /// </returns>
        private PerformContentSearchRequest GetPerformContentSearchRequest(string searchText)
        {
            var request = new PerformContentSearchRequest();

            var searchstring = new SearchString
            {
                Value = searchText,
                Type = SearchType.Free,
                Mode = SearchMode.Simple,
                Combine = true,
                Filter = false,
                Scope = string.Empty,
                Validate = true
            };
            request.StructuredSearch.Query.SearchStringCollection.Add(searchstring);
            request.DescriptorControl.Mode = DescriptorControlMode.All;
            request.FirstResult = 0;
            request.MaxResults = 0;
            request.StructuredSearch.Formatting.SnippetType = SnippetType.Contextual;
            request.StructuredSearch.Formatting.MarkupType = MarkupType.All;
            request.StructuredSearch.Formatting.DeduplicationMode = DeduplicationMode.Off;
            request.StructuredSearch.Formatting.ClusterMode = ClusterMode.On;
            request.StructuredSearch.Formatting.FreshnessDate = DateTime.Now;
            request.StructuredSearch.Formatting.SortOrder = ResultSortOrder.RelevanceHighFreshness;
            request.StructuredSearch.Query.Dates = this.ProcessDates(SearchDateRange.LastThreeMonths);
            request.NavigationControl.TimeNavigatorMode = TimeNavigatorMode.AutoDetect;

            return request;
        }

        public string GetTimeSlice<T>( T value )
        {
            Type enumType = typeof( T );
            string fieldName = value.ToString();
            var attribute = ( TimeSlice ) Attribute.GetCustomAttribute( enumType.GetField( fieldName ), typeof( TimeSlice ) );
            return attribute != null ? attribute.Slice.ToString() : null;
        }

        private Dates ProcessDates( SearchDateRange dateRange )
        {
            var dates = new Dates
            {
                Type = DateType.Publication
            };
            switch( dateRange )
            {
                case SearchDateRange.All:
                    dates.All = true;
                    break;
                case SearchDateRange.Custom:
                    break;
                default:
                    string timeSlice = GetTimeSlice( dateRange );
                    if( timeSlice != null )
                    {
                        dates.After = timeSlice;
                    }
                    break;
            }
            return dates;
        }

        private DateHistogramModel GetBasicModel()
        {
            var manager = new SearchManager(this.ControlData, this.Preferences);   
            var request = new PerformContentSearchRequest();
            var result  = manager.PerformContentSearch<PerformContentSearchResponse>(GetPerformContentSearchRequest("obama"));

            if (result.ContentSearchResult != null &&
                result.ContentSearchResult.TimeNavigatorSet != null &&
                result.ContentSearchResult.TimeNavigatorSet.Count > 0)
            {
                var nav = result.ContentSearchResult.TimeNavigatorSet.NavigatorCollection.First();
                  var temp = new DateHistogramModel
                    {
                        BarColor = "#FF0000",
                        Histogram = Mapper.Map<Histogram>(nav),
                    };         
             
                    return temp;
            }

             throw new NullReferenceException();
        }

    }
}
