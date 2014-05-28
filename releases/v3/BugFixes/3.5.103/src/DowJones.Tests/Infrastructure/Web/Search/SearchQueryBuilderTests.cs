using System;
using System.Linq;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Preference;
using DowJones.Managers.SearchContext;
using DowJones.Preferences;
using DowJones.Search;
using DowJones.Search.Filters;
using DowJones.Session;
using DowJones.Token;
using Factiva.Gateway.Messages.Search.V2_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DeduplicationMode = DowJones.Search.DeduplicationMode;

namespace DowJones.Infrastructure.Web.Search
{
    [TestClass]
    public class SearchQueryBuilderTests : UnitTestFixtureBase<SearchQueryBuilder>
    {
        private IControlData _controlData = new ControlData { UserID = "joyful", UserPassword = "joyful", ProductID = "16" };
        private ITokenRegistry _tokenRegistry = new Mock<ITokenRegistry>().Object;
        private IPreferences _preferences = new DowJones.Preferences.Preferences("en");

        protected SearchQueryBuilder Provider
        {
            get { return UnitUnderTest; }
        }

        public void BuildSimpleSearchQuery()
        {
            var query = new SimpleSearchQuery();
            query.Keywords = "Obama";
            query.Source = "5973";
            query.DateRange = SearchDateRange.LastSixMonths;
            query.Duplicates = DeduplicationMode.NearExact;
            query.PrimarySourceGroupId = "SG_SMEDIA";
            query.SecondarySourceGroupId = "SG_NEWSBLOGS";
            query.Filters = new SearchQueryFilters
                                {
                                    new KeywordQueryFilter(new[] { "economy" }),
                                    new EntitiesQueryFilter(EntityType.Region) { "CO_NEWS_Filter1", "CO_NEWS_Filter2" }
                                };
            query.ProductId = "communicator";
            query.Filters.Add(new DateRangeQueryFilter(new DateRange(DateTime.Parse("20110808"), DateTime.Parse("20110825"))));

            // TODO:  Fix this - executing the request has nothing to do with building the query...
            var searchQuery = Provider.GetRequest<PerformContentSearchRequest>(query);
            Assert.IsNotNull(searchQuery);
            System.Diagnostics.Debug.WriteLine(Serialize(searchQuery));
            Assert.IsTrue(searchQuery.StructuredSearch.Query.SearchStringCollection.Count() == 4);
        }

        //[TestMethod]
        public void BuildAdvancedSearchQuery()
        {
            var query = new FreeTextSearchQuery();
            query.FreeText = "wc>100";
            query.DateRange = SearchDateRange.Custom;
            query.CustomDateRange = new DateRange(DateTime.Parse("20110808"), DateTime.Parse("20110825"));
            query.PrimarySourceGroupId = "SG_SMEDIA";
            query.SecondarySourceGroupId = "SG_NEWSBLOGS";
            query.Filters = new SearchQueryFilters { new KeywordQueryFilter(new[] { "economy" }), };
            query.ProductId = "communicator";
            query.Inclusions = new[] { InclusionFilter.SocialMedia };
            query.Company = new CompoundQueryFilter
                                {
                                    Include = new[]
                                                  {
                                                      new EntitiesQueryFilter(EntityType.Company)
                                                          {
                                                              "Microsoft",
                                                              "Apple Computer",
                                                              "Wal-Mart",
                                                              "Yahoo",
                                                          }
                                                  },
                                    Exclude = new[]
                                                {
                                                    new EntitiesQueryFilter(EntityType.Company)
                                                        {
                                                            "Microsoft",
                                                            "Apple Computer",
                                                            "Wal-Mart",
                                                            "Yahoo",
                                                        }
                                                }
                                };

            query.Exclusions = new[] {
                                            ExclusionFilter.Crime,
                                            ExclusionFilter.Sports,
                                            ExclusionFilter.RepublishedNews
                                        };
            query.ContentLanguages = new[] { "en", "it", "es", "de", "fr" };

            query.FreeTextIn = SearchFreeTextArea.HeadlineAndLeadParagraph; //"BY", "HL"

            // TODO:  Fix this - executing the request has nothing to do with building the query
            var searchQuery = Provider.GetRequest<PerformContentSearchRequest>(query);
            Assert.IsNotNull(searchQuery);
            System.Diagnostics.Debug.WriteLine(Serialize(searchQuery));
            Assert.IsTrue(searchQuery.StructuredSearch.Query.SearchStringCollection.Count() == 6);
        }


        //[TestMethod]
        public void BuildSearchFormSearchQuery()
        {
            var query = new SearchFormSearchQuery();

            query.Keywords = "wc:>1";
            query.NotWords = "Bush";
            query.AnyWords = "The Economy";
            query.ExactPhrase = "Obama";

            query.DateRange = SearchDateRange.Custom;
            query.CustomDateRange = new DateRange(DateTime.Parse("20110808"), DateTime.Parse("20110825"));
            query.PrimarySourceGroupId = "SG_SMEDIA";
            query.SecondarySourceGroupId = "SG_NEWSBLOGS";
            query.Filters = new SearchQueryFilters { new KeywordQueryFilter(new[] { "economy" }), };
            query.ProductId = "communicator";


            query.Inclusions = new[] { InclusionFilter.SocialMedia };

            query.Company = new CompoundQueryFilter
                                {
                                    Include = new[]
                                                  {
                                                      new EntitiesQueryFilter(EntityType.Company)
                                                          {
                                                              "Microsoft",
                                                              "Apple Computer",
                                                              "Wal-Mart",
                                                              "Yahoo",
                                                          }
                                                  },
                                    Exclude = new[]
                                                  {
                                                      new EntitiesQueryFilter(EntityType.Company)
                                                          {
                                                              "Microsoft",
                                                              "Apple Computer",
                                                              "Wal-Mart",
                                                              "Yahoo",
                                                          }
                                                  }
                                };

            query.Exclusions = new [] {
                                            ExclusionFilter.Crime,
                                            ExclusionFilter.Sports,
                                            ExclusionFilter.RepublishedNews
                                        };
            query.ContentLanguages = new[] { "en", "it", "es", "de", "fr" };

            // TODO:  Fix this - executing the request has nothing to do with building the query
            var searchQuery = Provider.GetRequest<PerformContentSearchRequest>(query);
            Assert.IsNotNull(searchQuery);
            System.Diagnostics.Debug.WriteLine(Serialize(searchQuery));
            Assert.IsTrue(searchQuery.StructuredSearch.Query.SearchStringCollection.Count() == 9);
        }


        protected override SearchQueryBuilder CreateUnitUnderTest()
        {
            var sourceGroupConfigurationManager = new ProductSourceGroupConfigurationManager(_controlData, _tokenRegistry);
            var searchQueryResourceManager = new SearchQueryResourceManager(_controlData, sourceGroupConfigurationManager);

            return new SearchQueryBuilder(_preferences, searchQueryResourceManager, new SearchPreferenceService(), new SearchContextManager(_controlData, _preferences));
        }
    }
}
