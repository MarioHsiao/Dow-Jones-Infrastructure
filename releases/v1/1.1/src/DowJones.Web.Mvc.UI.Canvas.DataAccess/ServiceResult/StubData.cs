//using System;
//using System.Collections.Generic;
//using DowJones.Tools.Ajax.HeadlineList;
//using DowJones.Tools.Ajax.PortalHeadlineList;
//using DowJones.Utilities.Formatters;
//using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
//using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.CompanyOverview;
//using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
//using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page;
//using DowJones.Web.Mvc.UI.Models.Common;
//using DowJones.Web.Mvc.UI.Models.Company;
//using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;

//namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
//{
//    static internal class StubData
//    {
//        internal static void Get(CompanyOverviewNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>();
//            var companySnapShotPackage = new CompanySnapshotPackage
//            {
//                DataMonitorReports = new PortalHeadlineListDataResult
//                {
//                    HitCount = new WholeNumber(0),
//                    ResultSet = new PortalHeadlineListResultSet()
//                }
//            };

//            var executive = new Models.Executive
//            {
//                CompleteName = "Bill H. Gates III",
//                ConsolidationId = "12303144",
//                FirstName = "Bill",
//                IsEmployee = true,
//                IsOfficer = true,
//                JobTitle = "Chairman of the Board",
//                LastName = "Gates",
//                MiddleNames = "H.",
//                Suffix = "III"
//            };
//            companySnapShotPackage.Executives = new Models.ExecutiveCollection { executive };

//            companySnapShotPackage.InvestextReports = new PortalHeadlineListDataResult();
//            var portalHeadline = new PortalHeadlineInfo
//                                     {
//                                         BaseLanguage = "en",
//                                         ContentCategoryDescriptor = "publication",
//                                         ContentSubCategoryDescriptor = "article",
//                                         HasPublicationTime = false,
//                                         PublicationDateTime = DateTime.Today,
//                                         SourceCode = "xhnnjp",
//                                         SourceDescriptor = "test",
//                                         Title = "test title",
//                                         Reference = {guid = "XHNNJP0020110303e7330001a", mimetype = "text/xml", type = "accessionNo"},
//                                         WordCount = new WholeNumber(1004),
//                                         WordCountDescriptor = "1,004 ${words}"
//                                     };

//            var snippets = new SnippetCollection {"test snippet"};
//            portalHeadline.Snippets = snippets;
//            companySnapShotPackage.InvestextReports.ResultSet.Headlines.Add(portalHeadline);

//            companySnapShotPackage.Quote = new Quote {CompanyName = "Microsoft Corp", FCode = "MSFT"};

//            var exchange = new Exchange {Code = "NSQ", Descriptor = "Consolidated Issue Listed on Nasdaq Global Select Market", IsPrimary = true};
//            companySnapShotPackage.Quote.ListedExchanges = new List<Exchange> { exchange };
//            companySnapShotPackage.Quote.Ric = "MSFT.O";
//            companySnapShotPackage.Quote.DjTicker = "MSFT";
//            companySnapShotPackage.Quote.AskPrice = new DoubleNumberStock(26.17);
//            companySnapShotPackage.Quote.BidPrice = new DoubleNumberStock(26.16);
//            companySnapShotPackage.Quote.Change = new DoubleNumber(-0.0359);
//            companySnapShotPackage.Quote.CloseDate = DateTime.Today;
//            companySnapShotPackage.Quote.Currency = "USD";
//            companySnapShotPackage.Quote.FiftyTwoWeekHigh = new DoubleNumberStock(31.58);
//            companySnapShotPackage.Quote.FiftyTwoWeekLow = new DoubleNumberStock(22.73);
//            companySnapShotPackage.Quote.FiftyTwoWeekHighDate = DateTime.Today.Subtract(new TimeSpan(10, 0, 0, 0));
//            companySnapShotPackage.Quote.FiftyTwoWeekLowDate = DateTime.Today.Subtract(new TimeSpan(10, 0, 0, 0));
//            companySnapShotPackage.Quote.High = new DoubleNumberStock(26.09);
//            companySnapShotPackage.Quote.LastTradePrice = new DoubleNumberStock(26.1641);
//            companySnapShotPackage.Quote.Low = new DoubleNumberStock(26.09);
//            companySnapShotPackage.Quote.MarketCap = new DoubleMoney("233334000000") {Currency = "USD"};
//            companySnapShotPackage.Quote.Open = new DoubleNumberStock(26.22);
//            companySnapShotPackage.Quote.PercentageChange = new Percent(-0.14);
//            companySnapShotPackage.Quote.Last = new DoubleNumberStock(25.63);
//            companySnapShotPackage.Quote.LastTradeDateTime = DateTime.Today;
//            companySnapShotPackage.Quote.Volume = new WholeNumber(38037173);

//            companySnapShotPackage.ZacksReports = new PortalHeadlineListDataResult {HitCount = new WholeNumber(1)};
//            companySnapShotPackage.ZacksReports.ResultSet.Headlines.Add(portalHeadline);

//            partResult.Package = companySnapShotPackage;

//            serviceResult.PartResults = new List<CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>> { partResult };
//        }
//        internal static void Get(SummaryNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new SummaryNewsPageServicePartResult<AbstractSummaryPackage>();
//            var summaryChartPackage = new SummaryChartPackage();
//            summaryChartPackage.MarketIndexIntradayResult = new MarketIndexIntradayResult
//                                                                {
//                                                                    Code = "DJIA",
//                                                                    High = new DoubleNumberStock(),
//                                                                    Low = new DoubleNumberStock(),
//                                                                    PreviousClose = new DoubleNumberStock(),
//                                                                    Name = "Dow Jones Industrial Average"
//                                                                };
//            summaryChartPackage.MarketIndexIntradayResult.DataPoints = new BasicDataPointCollection();
//            var basicDataPoint = new BasicDataPoint
//                                                {
//                                                    Date = DateTime.Today, DateDisplay = DateTime.Today.ToLongDateString()
//                                                };
//            basicDataPoint.DataPoint.Value = 12087.54;
            
//            summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);

//            partResult.Package = summaryChartPackage;

//            serviceResult.PartResults = new List<SummaryNewsPageServicePartResult<AbstractSummaryPackage>> { partResult };

//            //var partResult1 = new SummaryNewsPageServicePartResult<AbstractSummaryPackage>();
//            //var summaryRegionalMapPackage = new SummaryRegionalMapPackage();
//            //summaryRegionalMapPackage.RegionNewsVolume= new List<NewsEntity>();
//           // NewsEntity newsEntity 
//            //summaryRegionalMapPackage.RegionNewsVolume.Add()
            
//            //summaryChartPackage.MarketIndexIntradayResult.DataPoints = new BasicDataPointCollection();
//            //var basicDataPoint = new BasicDataPoint
//            //{
//            //    Date = DateTime.Today,
//            //    DateDisplay = DateTime.Today.ToLongDateString(),
//            //    DataPoint = new Number()
//            //};
//            //summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            //summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            //summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            //summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            //summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);
//            //summaryChartPackage.MarketIndexIntradayResult.DataPoints.Add(basicDataPoint);

//            //partResult.Package = summaryChartPackage;
            

                                             

            
//        }

//        internal static void Get(CustomTopicsNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>();
//            var customTopicsPackage = new CustomTopicsPackage
//            {
//                Title = "Barak Obama"
//            };

//            var result = new PortalHeadlineListDataResult();
//            result.ResultSet.Count.Exp = 1;
//            result.ResultSet.DuplicateCount.Exp = 0;
//            result.ResultSet.First.Exp = 0;
//            result.HitCount = new WholeNumber(1);

//            var portalHeadline = new PortalHeadlineInfo
//            {
//                Authors = new AuthorCollection
//                              {
//                                  "elEconomista "
//                              },
//                BaseLanguage = "en",
//                ContentCategoryDescriptor = "publication",
//                ContentSubCategoryDescriptor = "article",
//                HasPublicationTime = false,
//                PublicationDateTime = DateTime.Today,
//                SourceCode = "xhnnjp",
//                SourceDescriptor = "test",
//                Title = "test title",
//                Reference = { guid = "XHNNJP0020110303e7330001a", mimetype = "text/xml", type = "accessionNo" }
//            };

//            var snippets = new SnippetCollection
//                               {
//                                   "test snippet"
//                               };
//            portalHeadline.Snippets = snippets;
//            result.ResultSet.Headlines.Add(portalHeadline);
//            customTopicsPackage.Result = result;
//            partResult.Package = customTopicsPackage;

//            serviceResult.PartResults = new List<CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>> { partResult };
//        }

        

//        internal static void Get(NewsstandNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>();
//            var headlinesPackage = new NewsstandHeadlinesPackage();
//            var newsStandSection = new NewsstandSection
//                                       {
//                                           SectionId = "2", 
//                                           SectionTitle = "Front Page", 
//                                           SourceCode = "J", 
//                                           SourceTitle = "The Wall Street Journal"
//                                       };

//            var result = new PortalHeadlineListDataResult
//                             {
//                                 ResultSet =
//                                     {
//                                         Count = new WholeNumber(4),
//                                         DuplicateCount =
//                                             {
//                                                 Exp = 0
//                                             },
//                                         First =
//                                             {
//                                                 Exp = 0
//                                             }
//                                     }, HitCount = new WholeNumber(7)
//                             };

//            var portalHeadline = new PortalHeadlineInfo
//            {
//                Authors = new AuthorCollection
//                              {
//                                  "By Margaret Coker in Tripoli, Charles Levinson in Benghazi, Libya, and Adam Entous in Washington "
//                              }, 
//                BaseLanguage = "en", 
//                ContentCategoryDescriptor = "publication", 
//                ContentSubCategoryDescriptor = "article", 
//                HasPublicationTime = false, 
//                PublicationDateTime = DateTime.Today, 
//                SourceCode = "xhnnjp", 
//                SourceDescriptor = "test", 
//                Title = "test title", 
//                Reference =
//                    {
//                        guid = "XHNNJP0020110303e7330001a", mimetype = "text/xml", type = "accessionNo"
//                    }, 
//                WordCount = new WholeNumber(1055), 
//                WordCountDescriptor = "1,055 ${words}"
//            };

//            var snippets = new SnippetCollection
//                               {
//                                   "test snippet"
//                               };
//            portalHeadline.Snippets = snippets;
//            result.ResultSet.Headlines.Add(portalHeadline);
//            result.ResultSet.Headlines.Add(portalHeadline);
//            result.ResultSet.Headlines.Add(portalHeadline);
//            result.ResultSet.Headlines.Add(portalHeadline);
//            newsStandSection.Result = result;

//            headlinesPackage.NewsstandSections = new List<NewsstandSection>
//                                                     {
//                                                         newsStandSection, 
//                                                         newsStandSection, 
//                                                         newsStandSection, 
//                                                         newsStandSection
//                                                     };

//            partResult.Package = headlinesPackage;
            
//            var partResult1 = new NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>();
//            var headlinesCountPackage = new NewsstandHeadlineHitCountsPackage();
            
//            var newsStandHeadlineHitCountSection = new NewsstandHeadlineHitCount
//                                                       {
//                                                           HitCount = 6,
//                                                           SectionId = "2",
//                                                           SectionTitle = "Front Page", 
//                                                           SourceCode = "J",
//                                                           SourceTitle = "The Wall Street Journal",
//                                                           Status = 0
//                                                       };

//            headlinesCountPackage.NewsstandHeadlineHitCounts = new List<NewsstandHeadlineHitCount>
//                                                                   {
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection, 
//                                                                       newsStandHeadlineHitCountSection
//                                                                   };

//            partResult1.Package = headlinesCountPackage;

//            var partResult2 = new NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>();
//            var discoveredEntiesPackage = new NewsstandDiscoveredEntitiesPackage
//                                              {
//                                                  TopNewsVolumeEntities = new List<NewsEntity>()
//                                              };

//            var newsEntity = new NewsEntity { Code = "mck", Descriptor = "McKinsey and Company Inc", CurrentTimeFrameNewsVolume = new WholeNumber(1), Type = EntityType.Organization };
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);
//            discoveredEntiesPackage.TopNewsVolumeEntities.Add(newsEntity);

//            serviceResult.PartResults = new List<NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>>
//                              {
//                                  partResult, partResult1, partResult2
//                              };
//        }

//        internal static void Get(RadarNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new RadarNewsPageServicePartResult<RadarPackage>();
//            var radarPackage = new RadarPackage
//            {
//                MaxHitCount = new WholeNumber(35572),
//                MinHitCount = new WholeNumber(0),
//                ParentNewsEntities = new ParentNewsEntities()
//            };

//            var parentNewsEntity = new ParentNewsEntity
//            {
//                Code = "BP",
//                CurrentTimeFrameNewsVolume = new WholeNumber(9043),
//                Descriptor = "BP plc",
//                Type = EntityType.Company,
//                NewsEntities = new NewsEntities()
//            };

//            var newsEntity = new NewsEntity 
//            {
//                Code = "ccat",
//                CurrentTimeFrameNewsVolume = new WholeNumber(35572),
//                Descriptor = "Corporate/Industrial News",
//                Type = EntityType.NewsSubject
//            };
//            parentNewsEntity.NewsEntities.Add(newsEntity);
//            radarPackage.ParentNewsEntities.Add(parentNewsEntity);

//            partResult.Package = radarPackage;

//            serviceResult.PartResults = new List<RadarNewsPageServicePartResult<RadarPackage>> { partResult };
//        }

//        internal static void Get(RegionalMapNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>();
//            var regionalMapPackage = new RegionalMapNewsVolumePackage();
//            var newsEntityNewsVolumeVariation = new NewsEntityNewsVolumeVariation
//            {
//                Code = "AUSTR",
//                CurrentTimeFrameNewsVolume = new WholeNumber(11262),
//                SearchContextRef = "RegionalMap::13219::22912::LastWeek::Region::AUSTR",
//                Type = EntityType.Region, NewEntrant = false, PercentVolumeChange = new Percent(-15.532888322208061),
//                PreviousTimeFrameNewsVolume = new WholeNumber(13333)
//            };

//            regionalMapPackage.RegionNewsVolume = new List<NewsEntityNewsVolumeVariation> { newsEntityNewsVolumeVariation };
//            partResult.Package = regionalMapPackage;
//            serviceResult.PartResults = new List<RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>> { partResult };
//        }

//        internal static void Get(SourcesNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new SourcesNewsPageServicePartResult<SourcePackage>();
//            var sourcePackage = new SourcePackage {SourceCode = "NYTF", SourceName = "The New York Times"};

//            var result = new PortalHeadlineListDataResult();
//            result.ResultSet.Count.Exp = 1;
//            result.ResultSet.DuplicateCount.Exp = 0;
//            result.ResultSet.First.Exp = 0;
//            result.HitCount = new WholeNumber(2);

//            var portalHeadline = new PortalHeadlineInfo
//            {
//                BaseLanguage = "en",
//                ContentCategoryDescriptor = "publication",
//                ContentSubCategoryDescriptor = "article",
//                HasPublicationTime = false,
//                PublicationDateTime = DateTime.Today,
//                SourceCode = "xhnnjp",
//                SourceDescriptor = "test",
//                Title = "test title",
//                Reference = {guid = "XHNNJP0020110303e7330001a", mimetype = "text/xml", type = "accessionNo"},
//                Authors = new AuthorCollection {"y Shelley Cook and Darrin Bauming"},
//                WordCount = new WholeNumber(200),
//                WordCountDescriptor = "200 ${words}"
//            };

//            var snippets = new SnippetCollection {"test snippet"};
//            portalHeadline.Snippets = snippets;
//            result.ResultSet.Headlines.Add(portalHeadline);
//            sourcePackage.Result = result;

//            partResult.Package = sourcePackage;
//            serviceResult.PartResults = new List<SourcesNewsPageServicePartResult<SourcePackage>> { partResult };
//        }

//        internal static void Get(SwapModuleEditorServiceResult serviceResult)
//        {
//            var telecom = new SwapModuleEditorModule { Id = "telecom", Name = "Telecommunications" };
//            var banking = new SwapModuleEditorModule { Id = "banking", Name = "Banking" };
//            var technology = new SwapModuleEditorModule { Id = "technology", Name = "Technology" };
//            var biotech = new SwapModuleEditorModule { Id = "biotech", Name = "Biotechnology" };

//            var nAmerica = new SwapModuleEditorModule { Id = "northAmerica", Name = "North America" };
//            var sAmerica = new SwapModuleEditorModule { Id = "southAmerica", Name = "South America" };
//            var cAmerica = new SwapModuleEditorModule { Id = "centralAmerica", Name = "Central America" };
//            var europe = new SwapModuleEditorModule { Id = "europe", Name = "Europe" };

//            serviceResult.Package = new SwapModuleEditorPackage
//            {
//                //SelectedCollection = SwapModuleEditorCollection.Industries,
//                Industries = new List<SwapModuleEditorModule>(new[] { telecom, banking, technology, biotech }),
//                Regions = new List<SwapModuleEditorModule>(new[] { nAmerica, sAmerica, cAmerica, europe }),
//            };
//        }

//        internal static void Get(SyndicationNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new SyndicationNewsPageServicePartResult<SyndicationPackage>();
//            var syndicationPackage = new SyndicationPackage
//            {
//                FaviconUri = "http://arch.dev.us.factiva.com/Showcase/emgajaxtoolkittests2008/emg.utility.handler.syndication.favicon.ashx?size=small&url=http%3a%2f%2fonline.wsj.com%2fxml%2frss%2f3_7087.xml",
//                FeedTitle = "WSJ.com: Politics And Policy",
//                FeedType = FeedType.RSS,
//                //FeedId = "111"
//            };

//            var result = new PortalHeadlineListDataResult();
//            result.ResultSet.Count.Exp = 1;
//            result.ResultSet.DuplicateCount.Exp = 0;
//            result.ResultSet.First.Exp = 0;
//            result.HitCount = new WholeNumber(0);

//            var portalHeadline = new PortalHeadlineInfo
//            {
//                BaseLanguage = "en",
//                ContentCategoryDescriptor = "publication",
//                ContentSubCategoryDescriptor = "article",
//                HasPublicationTime = false,
//                PublicationDateTime = DateTime.Today,
//                SourceCode = "xhnnjp",
//                SourceDescriptor = "test",
//                Title = "test title",
//                Reference = {guid = "XHNNJP0020110303e7330001a", mimetype = "text/xml", type = "accessionNo"},
//                WordCount = new WholeNumber(0)
//            };

//            var snippets = new SnippetCollection {"test snippet"};
//            portalHeadline.Snippets = snippets;
//            result.ResultSet.Headlines.Add(portalHeadline);
//            syndicationPackage.Result = result;
//            partResult.Package = syndicationPackage;

//            serviceResult.PartResults = new List<SyndicationNewsPageServicePartResult<SyndicationPackage>> { partResult };
//        }

//        internal static void Get(TopNewsNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>();
//            var topNewsEditorChoicePackage = new TopNewsEditorsChoicePackage
//                                                 {
//                                                     Title = "${TopNewsEditorsChoice}"
//                                                 };

//            var result = new PortalHeadlineListDataResult();
//            result.ResultSet.Count.Exp = 1;
//            result.ResultSet.DuplicateCount.Exp = 0;
//            result.ResultSet.First.Exp = 0;
//            result.HitCount = new WholeNumber(2);

//            var portalHeadline = new PortalHeadlineInfo
//                                     {
//                                         BaseLanguage = "en", 
//                                         ContentCategoryDescriptor = "publication", 
//                                         ContentSubCategoryDescriptor = "article", 
//                                         HasPublicationTime = false, 
//                                         PublicationDateTime = DateTime.Today,
//                                         SourceCode = "xhnnjp", 
//                                         SourceDescriptor = "test",
//                                         Title = "test title",
//                                         Reference = {guid = "XHNNJP0020110303e7330001a", mimetype = "text/xml", type = "accessionNo"}
//                                     };

//            var snippets = new SnippetCollection
//                               {
//                                   "test snippet"
//                               };
//            portalHeadline.Snippets = snippets;
//            result.ResultSet.Headlines.Add(portalHeadline);
//            topNewsEditorChoicePackage.Result = result;
//            partResult.Package = topNewsEditorChoicePackage;

//            serviceResult.PartResults = new List<TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>> { partResult };
//        }

//        internal static void Get(TrendingNewsPageModuleServiceResult serviceResult)
//        {
//            var partResult = new TrendingNewsPageServicePartResult<AbstractTrendingPackage>();

//            var trendingTopEntitiesPackage = new TrendingTopEntitiesPackage();
//            var newsEntity = new NewsEntity
//            {
//                Code = "bp",
//                CurrentTimeFrameNewsVolume = new WholeNumber(288),
//                Descriptor = "BP PLC",
//                SearchContextRef = "Trending::13219::22913::LastWeek::TopEntities::Company::bp",
//                Type = EntityType.Company
//            };
//            trendingTopEntitiesPackage.TopNewsVolumeEntities = new List<NewsEntity> { newsEntity };
//            partResult.Package = trendingTopEntitiesPackage;

//            serviceResult.PartResults = new List<TrendingNewsPageServicePartResult<AbstractTrendingPackage>> { partResult };
//        }

//        internal static void Get(AlertsNewsPageServiceResult serviceResult)
//        {
//            var partResult = new AlertsNewsPageServicePartResult<AlertsPackage>();
//            var alertsPackage = new AlertsPackage {AlertId = "200259481", AlertName = "test alert"};

//            partResult.Package = alertsPackage;
//            partResult.ReturnCode = 0;

//            var result = new PortalHeadlineListDataResult();
//            result.ResultSet.Count.Exp = 1;
//            result.ResultSet.DuplicateCount.Exp = 0;
//            result.ResultSet.First.Exp = 0;
//            result.HitCount = new WholeNumber(2);

//            var portalHeadline = new PortalHeadlineInfo
//            {
//                BaseLanguage = "en",
//                ContentCategoryDescriptor = "publication",
//                ContentSubCategoryDescriptor = "article",
//                HasPublicationTime = false,
//                PublicationDateTime = DateTime.Today,
//                SourceCode = "xhnnjp",
//                SourceDescriptor = "test",
//                Title = "test title",
//                Reference = {guid = "XHNNJP0020110303e7330001a", mimetype = "text/xml", type = "accessionNo"},
//                WordCount = new WholeNumber(1004),
//                WordCountDescriptor = "1,004 ${words}"
//            };

//            var snippets = new SnippetCollection {"test snippet"};
//            portalHeadline.Snippets = snippets;
//            result.ResultSet.Headlines.Add(portalHeadline);
//            alertsPackage.Result = result;
//            serviceResult.PartResults = new List<AlertsNewsPageServicePartResult<AlertsPackage>> { partResult };
//        }

//        public static void Get(SubscribableNewsPagesListServiceResult serviceResult)
//        {
//            var newsPage = new UI.Models.NewsPages.NewsPage
//            {
//                AccessScope = UI.Models.NewsPages.AccessScope.OwnedByUser,
//                Description = "Test page to Subscribe",
//                ID = 13653.ToString(),
//                IsActive = true,
//                LastModifiedDate = DateTime.Today,
//                Title = "test page to subscribe title",
//                Position = 0,
//                MetaData = new UI.Models.NewsPages.MetaData
//                               {
//                                   MetaDataCode = "i2569",
//                                   MetaDataDescriptor = "Biotechnology",
//                                   MetaDataType = UI.Models.NewsPages.MetaDataType.Industry
//                               },
//                CategoryInfo = new UI.Models.NewsPages.CategoryInfo
//                               {
//                                   CategoryCode = "ihea",
//                                   CategoryDescriptor = "Health Care"
//                               }
//            };

//            var newsPages = new List<UI.Models.NewsPages.NewsPage> { newsPage };
//            serviceResult.Package = new SubscribableNewsPagesListPackage
//            {
//                NewsPages = newsPages
//            };
//        }
//    }
//}
