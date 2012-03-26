using System;
using System.Web;
using System.Collections.Generic;
using DowJones.Ajax.TagCloud;
using DowJones.Assemblers.Assets;
using DowJones.DependencyInjection;
using DowJones.Managers.Search;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.Web.Showcase.Extensions
{
    public static class TagCloudConversionManagerExtensions
    {
        private static readonly SearchManager searchManager = ServiceLocator.Resolve<SearchManager>();

        private static SearchManager SearchManager
        {
            get { return searchManager; }
        }

        public static TagCloudModel PerformSearch(this TagConversionManager convMgr,string searchText)
        {
            var rules = GetGenerationRules();
            var tags = AddTagFromKeyWords(convMgr, rules,searchText);
            return new TagCloudModel { Tags = tags };
        }
        public static List<Tag> Process(this TagConversionManager convMgr, ContentSearchResult result)
        { 
            var rules = GetGenerationRules();
            return new List<Tag>(convMgr.Process(result.KeywordSet, rules));            
        }
        /// <summary>
        /// The get generation rules.
        /// </summary>
        /// <returns>
        /// A TagCloudGenerationRules object.
        /// </returns>
        private static TagCloudGenerationRules GetGenerationRules()
        {
            var rules = new TagCloudGenerationRules
            {
                Order = TagCloudOrder.Weight,
                TagUrlFormatString = string.Format(
                    "{0}?Tag={{0}}",
                    HttpContext.Current.Request.Url.GetComponents(UriComponents.Path | UriComponents.Host | UriComponents.Scheme, UriFormat.UriEscaped)),
                TagToolTipFormatString = "Weight: {0}",
                MaxNumberOfTags = 17
            };

            rules.Order = TagCloudOrder.Centralized;

            return rules;
        }

        /// <summary>
        /// The add tag from key words.
        /// </summary>
        private static IEnumerable<Tag> AddTagFromKeyWords(TagConversionManager convMgr, TagCloudGenerationRules rules,string searchText)
        {
            //jobs;
            var searchRequest = GetPerformContentSearchRequest(searchText);
            var searchResponse = GetPerformContentSearchResponse(searchRequest).ContentSearchResult.KeywordSet;
            return convMgr.Process(searchResponse, rules);
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
        private static PerformContentSearchRequest GetPerformContentSearchRequest(string searchText)
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
            request.MaxResults = 2000;
            request.StructuredSearch.Formatting.SnippetType = SnippetType.Contextual;
            request.StructuredSearch.Formatting.MarkupType = MarkupType.All;
            request.StructuredSearch.Formatting.DeduplicationMode = DeduplicationMode.Off;
            request.StructuredSearch.Formatting.ClusterMode = ClusterMode.On;
            request.StructuredSearch.Formatting.FreshnessDate = DateTime.Now;
            request.StructuredSearch.Formatting.SortOrder = ResultSortOrder.RelevanceHighFreshness;
            request.NavigationControl.KeywordControl.ReturnKeywords = true;
            request.NavigationControl.KeywordControl.MaxKeywords = 1000;

            return request;
        }

        /// <summary>
        /// The get perform content search response.
        /// </summary>
        /// <param name="performContentSearchRequest">
        /// The perform content search request.
        /// </param>
        /// <returns>
        /// A PerformContentSearchResponse Object.
        /// </returns>
        private static IPerformContentSearchResponse GetPerformContentSearchResponse(PerformContentSearchRequest performContentSearchRequest)
        {
            if (performContentSearchRequest != null)
            {
                var response = SearchManager.PerformContentSearch<PerformContentSearchResponse>(performContentSearchRequest);
                return response;
            }

            return null;
        }

    }
}
