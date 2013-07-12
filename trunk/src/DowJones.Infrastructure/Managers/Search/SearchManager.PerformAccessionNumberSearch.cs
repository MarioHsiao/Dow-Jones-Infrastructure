using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Exceptions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Managers.Search.Comparers;
using DowJones.Managers.Search.Requests;
using DowJones.Managers.Search.Responses;
using DowJones.Search.Core;
using DowJones.Session;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search
{
    public partial class SearchManager
    {
        public AccessionNumberSearchResponse PerformAccessionNumberSearch<TIPerformSearchRequest, TIPerformSearchResponse>(
            AccessionNumberSearchRequestDTO dto,
            IControlData controlData = null)
            where TIPerformSearchRequest : IPerformContentSearchRequest, new()
            where TIPerformSearchResponse : IPerformContentSearchResponse, new()
        {
            if (dto.IsValid())
            {
                // initialize the response
                var output = new AccessionNumberSearchResponse();

                // perform the search based on the Accession Number Search DTO [Data Transfer Object]
                var response = GetPerformContentSearchResponse<TIPerformSearchRequest, TIPerformSearchResponse>(dto, controlData);

                // run the search against 1 yr index
                if (response != null)
                {
                    // set up a dictionary object for mapping of returned headlines
                    var data = SearchUtility.GetDictionaryOfContentHeadlines(response);
                    var uniqueAccessionNumbers = dto.GetUniqueAccessionNumbers;

                    // if count matches the number of uniques accession numbers 
                    // tell user that discovery is available 
                    // Note: we only return discovery items from year archive. 
                     //output.CanReturnDiscoveryData = (data.Count == uniqueAccessionNumbers.Length);
                       
                    // generate list of items based on the mapping and sortby params
                    var items = GetAccessionNumberBasedContentItems(data, uniqueAccessionNumbers, dto.SortBy);

                    output.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection.AddRange(items);

                    // update the count value
                    output.AccessionNumberBasedContentItemSet.Count = output.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection.Count;

                    output.CollectionCountSet = response.ContentSearchResult.CollectionCountSet;
                    output.KeywordSet = response.ContentSearchResult.KeywordSet;
                    output.TimeNavigatorSet = response.ContentSearchResult.TimeNavigatorSet;
                    output.ContextualNavigatorSet = response.ContentSearchResult.ContextualNavigatorSet;
                    output.ClusterSet = response.ContentSearchResult.ClusterSet;
                    output.CodeNavigatorSet = response.ContentSearchResult.CodeNavigatorSet;

                    // generate mapping of AccesstionNumbers to respective collections ie. publications, web sites, etc.
                    output.ContentCatagorizationSet = Generate(items);

                    // return the new response and short-circuit the execution of the method.
                    return output;
                    
                }
                output.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection.AddRange(GetAccessionNumberBasedContentItems(new Dictionary<string, ContentHeadline>(), dto.AccessionNumbers, dto.SortBy));
                return output;
            }
            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.SearchManagerInvalidDto);
        }


        /// <summary>
        /// Generates the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        private static ContentCatagorizationSet Generate(ICollection<AccessionNumberBasedContentItem> items)
        {
            
            var catagorizationSet = new ContentCatagorizationSet();
            if (items != null && items.Count > 0)
            {
                var i = 0;
                foreach (var item in items)
                {
                    if (item.ContentHeadline != null && item.ContentHeadline.ContentItems != null)
                    {
                        var headlineRef = new HeadlineRef
                                              {
                                                  AccessionNo = item.AccessionNumber, 
                                                  Index = i
                                              };
                        if (item.HasBeenFound)
                        {
                            switch (item.ContentHeadline.ContentItems.ContentType.ToLower())
                            {
                                case "analyst":
                                case "pdf":
                                case "article":
                                case "file":
                                case "articlewithgraphics":
                                case "publication":
                                    catagorizationSet.PublicationRefCollection.Add(headlineRef);
                                    break;
                                case "webpage":
                                case "html":
                                    catagorizationSet.WebSiteRefCollection.Add(headlineRef);
                                    break;
                                case "picture":
                                    catagorizationSet.PictureRefCollection.Add(headlineRef);
                                    break;
                                case "multimedia":
                                    catagorizationSet.MultimediaRefCollection.Add(headlineRef);
                                    break;
                                case "board":
                                    catagorizationSet.BoardRefCollection.Add(headlineRef);
                                    break;
                                case "blog":
                                    catagorizationSet.BlogRefCollection.Add(headlineRef);
                                    break;
                                case "summary":
                                    catagorizationSet.SummaryRefCollection.Add(headlineRef);
                                    break;
                                case "customerdoc":
                                    catagorizationSet.CustomerDocRefCollection.Add(headlineRef);
                                    break;
                                case "internal":
                                    catagorizationSet.InternalRefCollection.Add(headlineRef);
                                    break;
                            }
                        }
                        else
                        {
                            catagorizationSet.UnknownRefCollection.Add(headlineRef);
                        }
                    }
                    i++;
                }   
            }
            return catagorizationSet;
        }


        /// <summary>
        /// Gets the accession number based content item.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="accessionNumbers">The accession numbers.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <returns></returns>
        private static AccessionNumberBasedContentItem[] GetAccessionNumberBasedContentItems(IDictionary<string, ContentHeadline> data, IEnumerable<string> accessionNumbers, SortBy sortBy)
        {
            var items = new List<AccessionNumberBasedContentItem>();
            foreach (var s in accessionNumbers)
            {
                // composite object representing a AccessionNumber ContentItem
                var item = new AccessionNumberBasedContentItem
                               {
                                   AccessionNumber = s
                               };

                // if found populate the object
                if (data.ContainsKey(s))
                {
                    // get the item 
                    item.HasBeenFound = true;
                    
                    // slot for content headline object 
                    item.ContentHeadline = data[s];

                    // used for sorting on date
                    item.PublicationDate = item.ContentHeadline.PublicationDate;
                    if (item.ContentHeadline.PublicationTime > DateTime.MinValue)
                    {
                        item.PublicationDate = DateTimeFormatter.Merge(item.ContentHeadline.PublicationDate, item.ContentHeadline.PublicationTime);
                    }
                }
                else
                {
                    item.HasBeenFound = false;
                    item.ContentHeadline = null;
                }
                items.Add(item);
            }
            
            var temp = items.ToArray();
            switch(sortBy)
            {
                case SortBy.LIFO:
                    Array.Reverse(temp);
                    break;
                case SortBy.PublicationDateChronological:
                    Array.Sort(temp, new AccessionNumberBasedContentItemComparer(AccessionNumberBasedContentItemComparer.SortDirections.PublicationDateChronological));
                    break;
                case SortBy.PublicationDateReverseChronological:
                    Array.Reverse(temp);
                    Array.Sort(temp, new AccessionNumberBasedContentItemComparer(AccessionNumberBasedContentItemComparer.SortDirections.PublicationDateReverseChronological));
                    break;
                default:
                    break;
            }
            return temp;
        }
    }
}
