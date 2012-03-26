using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Ajax.TagCloud;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.TopNewsCommunicator;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using AuthorCollection = DowJones.Tools.Ajax.PortalHeadlineList.AuthorCollection;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using TagCollection = DowJones.Web.Mvc.Models.News.TagCollection;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "topNewsCommunicatorServiceResult", Namespace = "")]
    public class TopNewsCommunicatorServiceResult :
        Generic.AbstractServiceResult<TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>, AbstractTopNewsCommunicatorPackage>, IPopulate<TopNewsCommunicatorDataRequest>
    {
        public void Populate(ControlData controlData, TopNewsCommunicatorDataRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        // First, validate the request
                        if (request == null || !request.IsValid())
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                        }

                        // Populate preferences if it is null
                        if (preferences == null)
                        {
                            preferences = GetPreferences(controlData);
                        }

                        // Get the module
                        ModuleEx module = null;
                        // var module = GetModule<ModuleTypeGoesHere>(request, controlData, preferences);
                        // Validate the module, throw DowJonesUtilitiesException if the module is invalid

                        // Now that we have a valid module, go retrieve data for that

                        PartResults = GetParts(module, request, controlData, preferences);
                        MaxPartsAvailable = 3;
                    },
                preferences);
        }

        private IEnumerable<TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>> GetParts(ModuleEx module, TopNewsCommunicatorDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var uniqueParts = request.Parts.GetUniques();

            // if only one part keep it on the same thread
            if (uniqueParts.Count == 1)
            {
                switch (uniqueParts.First())
                {
                    case TopNewsCommunicatorPart.RecentHeadlines:
                        return new List<TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>>
                                   {
                                       GetRecentHeadlinesServicePartResult(preferences)
                                   };
                    case TopNewsCommunicatorPart.BreakingNews:
                        return new List<TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>>
                                   {
                                       GetBreakingNewsServicePartResult(preferences)
                                   };
                    case TopNewsCommunicatorPart.Trending:
                        return new List<TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>>
                                   {
                                       GetTrendingServicePartResult(preferences)
                                   };
                }
            }
            else
            {
                var tasks = new List<Task<TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>>>();
                foreach (var part in uniqueParts)
                {
                    switch (part)
                    {
                        case TopNewsCommunicatorPart.RecentHeadlines:
                            tasks.Add(TaskFactory.StartNew(() => GetRecentHeadlinesServicePartResult(preferences)));
                            break;
                        case TopNewsCommunicatorPart.BreakingNews:
                            tasks.Add(TaskFactory.StartNew(() => GetBreakingNewsServicePartResult(preferences)));
                            break;
                        case TopNewsCommunicatorPart.Trending:
                            tasks.Add(TaskFactory.StartNew(() => GetTrendingServicePartResult(preferences)));
                            break;
                    }
                }

                Task.WaitAll(tasks.ToArray());
                return tasks.Select(task => task.Result).ToList();
            }
            return null;
        }

        // TODO: Add more parameters to make the proper calls once stubs are removed
        private TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage> GetTrendingServicePartResult(IPreferences preferences)
        {
            var partResult = new TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>();
            var package = new TopNewsCommunicatorTrendingPackage();
            partResult.Package = package;
            ProcessServicePartResult<TopNewsCommunicatorTrendingPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                        // Get data here
                        // TODO: STUBBED, TO BE REPLACED
                        package.Result = GetStubbedTrending();
                    },
                preferences);

            return partResult;
        }

        private static TagCollection GetStubbedTrending()
        {
            return new TagCollection
                       {
                           new Tag
                               {
                                   DistributionIndex = 4,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(5.00),
                                   Text = "ca technologies",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 5,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(3.96),
                                   Text = "capital partners",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 4,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(4.88),
                                   Text = "cloud services",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 4,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(4.76),
                                   Text = "hill capital",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 2,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(17.16),
                                   Text = "investment weekly",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 3,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(7.63),
                                   Text = "marketing weekly",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 4,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(5.00),
                                   Text = "oak hill",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 4,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(4.74),
                                   Text = "vice president",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 4,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(4.15),
                                   Text = "wall street",
                                   Type = 0
                               },
                           new Tag
                               {
                                   DistributionIndex = 2,
                                   NavigateUrl = "",
                                   Snippet = "",
                                   TagWeight = new DoubleNumber(23.82),
                                   Text = "weekly news",
                                   Type = 0
                               }
                       };
        }

        // TODO: Add more parameters to make the proper calls once stubs are removed
        private TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage> GetBreakingNewsServicePartResult(IPreferences preferences)
        {
            var partResult = new TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>();
            var package = new TopNewsCommunicatorBreakingNewsPackage();
            partResult.Package = package;
            ProcessServicePartResult<TopNewsCommunicatorBreakingNewsPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                        // Get data here
                        // TODO: STUBBED, TO BE REPLACED
                        package.Result = GetStubbedBreakingNews();
                    },
                preferences);

            return partResult;
        }

        // TODO: Add more parameters to make the proper calls once stubs are removed
        private TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage> GetRecentHeadlinesServicePartResult(IPreferences preferences)
        {
            var partResult = new TopNewsCommunicatorServicePartResult<AbstractTopNewsCommunicatorPackage>();
            var package = new TopNewsCommunicatorRecentHeadlinesPackage();
            partResult.Package = package;
            ProcessServicePartResult<TopNewsCommunicatorRecentHeadlinesPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                        // Get data here
                        // TODO: STUBBED, TO BE REPLACED
                        package.Result = GetStubbedRecentHeadlines();
                    },
                preferences);

            return partResult;
        }

        private static PortalHeadlineListDataResult GetStubbedRecentHeadlines()
        {
            return new PortalHeadlineListDataResult
                       {
                           HitCount = new WholeNumber(462),
                           ResultSet = new PortalHeadlineListResultSet
                                           {
                                               Count = new WholeNumber(6),
                                               DuplicateCount = new WholeNumber(0),
                                               First = new WholeNumber(0),
                                               Headlines = new List<PortalHeadlineInfo>
                                                               {
                                                                   new PortalHeadlineInfo
                                                                       {
                                                                           BaseLanguage = "en",
                                                                           BaseLanguageDescriptor = "English",
                                                                           ContentCategoryDescriptor = "publication",
                                                                           ContentSubCategoryDescriptor = "article",
                                                                           HasPublicationTime = true,
                                                                           ModificationDateDescriptor = "26 May 2011",
                                                                           ModificationDateTime = new DateTime(1306414896000),
                                                                           ModificationDateTimeDescriptor = "26 May 2011 1:01 PM GMT",
                                                                           ModificationTimeDescriptor = "1:01 PM GMT",
                                                                           PublicationDateDescriptor = "26 May 2011",
                                                                           PublicationDateTime = new DateTime(1306414790000),
                                                                           PublicationDateTimeDescriptor = "26 May 2011 12:59 PM GMT",
                                                                           PublicationTimeDescriptor = "12:59 PM GMT",
                                                                           Reference = new Reference
                                                                                           {
                                                                                               guid = "LBA0000020110526e75q00113",
                                                                                               mimetype = "text/xml",
                                                                                               type = "accessionNo"
                                                                                           },
                                                                           Snippets = new SnippetCollection
                                                                                          {
                                                                                              "...Rentrak Corp at BMO Cap Markets Marketing Services Conf 02 Jun 17:35 Microsoft Corp at Cowen & Co Tech Conf 02 Jun 17:55 The...Canterbury Park Holding Corp S/holders Meeting 02 Jun 21:00 Google S/holders Meeting ... "
                                                                                          },
                                                                           SourceCode = "lba",
                                                                           SourceDescriptor = "Reuters News",
                                                                           Title = "DIARY-U.S. MEETINGS/WEEK AHEAD ",
                                                                           TruncatedTitle = "DIARY-U.S. MEETINGS/WEEK AHEAD",
                                                                           WordCount = new WholeNumber(4875),
                                                                           WordCountDescriptor = "4,875 words"
                                                                       },
                                                                   new PortalHeadlineInfo
                                                                       {
                                                                           BaseLanguage = "en",
                                                                           BaseLanguageDescriptor = "English",
                                                                           ContentCategoryDescriptor = "publication",
                                                                           ContentSubCategoryDescriptor = "article",
                                                                           HasPublicationTime = true,
                                                                           ModificationDateDescriptor = "26 May 2011",
                                                                           ModificationDateTime = new DateTime(1306405672000),
                                                                           ModificationDateTimeDescriptor = "26 May 2011 10:27 AM GMT",
                                                                           ModificationTimeDescriptor = "10:27 AM GMT",
                                                                           PublicationDateDescriptor = "26 May 2011",
                                                                           PublicationDateTime = new DateTime(1306405670062),
                                                                           PublicationDateTimeDescriptor = "26 May 2011 10:27 AM GMT",
                                                                           PublicationTimeDescriptor = "10:27 AM GMT",
                                                                           Reference = new Reference
                                                                                           {
                                                                                               guid = "DJGPRW0020110526e75q0lgwh",
                                                                                               mimetype = "text/xml",
                                                                                               type = "accessionNo"
                                                                                           },
                                                                           Snippets = new SnippetCollection
                                                                                          {
                                                                                              "...Cls A Ordinary 258,684 --------------------- ---------- --------------------- -------------------- Microsoft Corp 39,800 NPV Common Stock 650,405... "
                                                                                          },
                                                                           SourceCode = "djgprw",
                                                                           SourceDescriptor = "Dow Jones Global Press Release Wire",
                                                                           Title = "DJ Albany Investment Trust PLC Final Results ",
                                                                           TruncatedTitle = "DJ Albany Investment Trust PLC Final Results",
                                                                           WordCount = new WholeNumber(10550),
                                                                           WordCountDescriptor = "10,550 words"
                                                                       },
                                                                   new PortalHeadlineInfo
                                                                       {
                                                                           BaseLanguage = "en",
                                                                           BaseLanguageDescriptor = "English",
                                                                           ContentCategoryDescriptor = "publication",
                                                                           ContentSubCategoryDescriptor = "article",
                                                                           HasPublicationTime = true,
                                                                           ModificationDateDescriptor = "26 May 2011",
                                                                           ModificationDateTime = new DateTime(1306405671000),
                                                                           ModificationDateTimeDescriptor = "26 May 2011 10:27 AM GMT",
                                                                           ModificationTimeDescriptor = "10:27 AM GMT",
                                                                           PublicationDateDescriptor = "26 May 2011",
                                                                           PublicationDateTime = new DateTime(1306405620000),
                                                                           PublicationDateTimeDescriptor = "26 May 2011 10:27 AM GMT",
                                                                           PublicationTimeDescriptor = "10:27 AM GMT",
                                                                           Reference = new Reference
                                                                                           {
                                                                                               guid = "RNS0000020110526e75q000b9",
                                                                                               mimetype = "text/xml",
                                                                                               type = "accessionNo"
                                                                                           },
                                                                           Snippets = new SnippetCollection
                                                                                          {
                                                                                              "...Cls A Ordinary 258,684 --------------------- ---------- --------------------- -------------------- Microsoft Corp 39,800 NPV Common Stock 650,405... "
                                                                                          },
                                                                           SourceCode = "rns",
                                                                           SourceDescriptor = "Regulatory News Service TEST",
                                                                           Title = "Albany Investment Trust PLC Final Results ",
                                                                           TruncatedTitle = "Albany Investment Trust PLC Final Results",
                                                                           WordCount = new WholeNumber(10450),
                                                                           WordCountDescriptor = "10,450 words"
                                                                       },
                                                                   new PortalHeadlineInfo
                                                                       {
                                                                           BaseLanguage = "en",
                                                                           BaseLanguageDescriptor = "English",
                                                                           ContentCategoryDescriptor = "publication",
                                                                           ContentSubCategoryDescriptor = "article",
                                                                           HasPublicationTime = true,
                                                                           ModificationDateDescriptor = "25 May 2011",
                                                                           ModificationDateTime = new DateTime(1306346129000),
                                                                           ModificationDateTimeDescriptor = "25 May 2011 5:55 PM GMT",
                                                                           ModificationTimeDescriptor = "5:55 PM GMT",
                                                                           PublicationDateDescriptor = "25 May 2011",
                                                                           PublicationDateTime = new DateTime(1306346127608),
                                                                           PublicationDateTimeDescriptor = "25 May 2011 5:55 PM GMT",
                                                                           PublicationTimeDescriptor = "5:55 PM GMT",
                                                                           Reference = new Reference
                                                                                           {
                                                                                               guid = "DJGPRW0020110525e75p0ljvv",
                                                                                               mimetype = "text/xml",
                                                                                               type = "accessionNo"
                                                                                           },
                                                                           Snippets = new SnippetCollection
                                                                                          {
                                                                                              "...attributed to their willingness to compete in many different markets and implement new innovations. Companies like Google , Microsoft , Amazon, Facebook, Apple, Twitter, and so on are continually looking for other opportunities from developments in... "
                                                                                          },
                                                                           SourceCode = "djgprw",
                                                                           SourceDescriptor = "Dow Jones Global Press Release Wire",
                                                                           Title = "DJ Reportlinker Adds Global Digital Media - Online Sector Entertains the World ",
                                                                           TruncatedTitle = "DJ Reportlinker Adds Global Digital Media - Online Sector...",
                                                                           WordCount = new WholeNumber(2813),
                                                                           WordCountDescriptor = "2,813 words"
                                                                       },
                                                                   new PortalHeadlineInfo
                                                                       {
                                                                           BaseLanguage = "en",
                                                                           BaseLanguageDescriptor = "English",
                                                                           ContentCategoryDescriptor = "publication",
                                                                           ContentSubCategoryDescriptor = "article",
                                                                           HasPublicationTime = true,
                                                                           ModificationDateDescriptor = "25 May 2011",
                                                                           ModificationDateTime = new DateTime(1306327328000),
                                                                           ModificationDateTimeDescriptor = "25 May 2011 12:42 PM GMT",
                                                                           ModificationTimeDescriptor = "12:42 PM GMT",
                                                                           PublicationDateDescriptor = "25 May 2011",
                                                                           PublicationDateTime = new DateTime(1306327241000),
                                                                           PublicationDateTimeDescriptor = "25 May 2011 12:40 PM GMT",
                                                                           PublicationTimeDescriptor = "12:40 PM GMT",
                                                                           Reference = new Reference
                                                                                           {
                                                                                               guid = "LBA0000020110525e75p000py",
                                                                                               mimetype = "text/xml",
                                                                                               type = "accessionNo"
                                                                                           },
                                                                           Snippets = new SnippetCollection
                                                                                          {
                                                                                              "...Markets Advertising & Marketing Conf 02 Jun 17:35 Microsoft Corp at Cowen & Co Tech Conf 02 Jun 17:55 The...Canterbury Park Holding Corp S/holders Meeting 02 Jun 21:00 Google S/holders Meeting ... "
                                                                                          },
                                                                           SourceCode = "lba",
                                                                           SourceDescriptor = "Reuters News",
                                                                           Title = "DIARY-U.S. MEETINGS/WEEK AHEAD ",
                                                                           TruncatedTitle = "DIARY-U.S. MEETINGS/WEEK AHEAD",
                                                                           WordCount = new WholeNumber(7239),
                                                                           WordCountDescriptor = "7,239 words"
                                                                       },
                                                                   new PortalHeadlineInfo
                                                                       {
                                                                           BaseLanguage = "en",
                                                                           BaseLanguageDescriptor = "English",
                                                                           ContentCategoryDescriptor = "publication",
                                                                           ContentSubCategoryDescriptor = "article",
                                                                           HasPublicationTime = true,
                                                                           ModificationDateDescriptor = "25 May 2011",
                                                                           ModificationDateTime = new DateTime(1306303318000),
                                                                           ModificationDateTimeDescriptor = "25 May 2011 6:01 AM GMT",
                                                                           ModificationTimeDescriptor = "6:01 AM GMT",
                                                                           PublicationDateDescriptor = "25 May 2011",
                                                                           PublicationDateTime = new DateTime(1306303200000),
                                                                           PublicationDateTimeDescriptor = "25 May 2011 6:00 AM GMT",
                                                                           PublicationTimeDescriptor = "6:00 AM GMT",
                                                                           Reference = new Reference
                                                                                           {
                                                                                               guid = "PRNDIS0020110525e75p0002v",
                                                                                               mimetype = "text/xml",
                                                                                               type = "accessionNo"
                                                                                           },
                                                                           Snippets = new SnippetCollection
                                                                                          {
                                                                                              "...Super Platform, our proprietary technology platform upon which all our travel-booking applications sit, is built on the Microsoft BizTalk platform and supports communication with internal and third-party systems via the internet. We are developing mobile... "
                                                                                          },
                                                                           SourceCode = "prndis",
                                                                           SourceDescriptor = "PR Newswire Disclose",
                                                                           Title = "HOGG ROBINSON GROUP PLC - Final Results ",
                                                                           TruncatedTitle = "HOGG ROBINSON GROUP PLC - Final Results",
                                                                           WordCount = new WholeNumber(12808),
                                                                           WordCountDescriptor = "12,808 words"
                                                                       },
                                                               }
                                           }
                       };
        }

        private static PortalHeadlineListDataResult GetStubbedBreakingNews()
        {
            return new PortalHeadlineListDataResult
                       {
                           HitCount = new WholeNumber(127),
                           ResultSet =
                               {
                                   Count = new WholeNumber(6),
                                   DuplicateCount = new WholeNumber(0),
                                   First = new WholeNumber(0),
                                   Headlines = new List<PortalHeadlineInfo>
                                                   {
                                                       new PortalHeadlineInfo
                                                           {
                                                               Authors = new AuthorCollection
                                                                             {
                                                                                 "By Shannon Bond in New York "
                                                                             },
                                                               BaseLanguage = "en",
                                                               BaseLanguageDescriptor = "English",
                                                               ContentCategoryDescriptor = "publication",
                                                               ContentSubCategoryDescriptor = "article",
                                                               HasPublicationTime = false,
                                                               ModificationDateDescriptor = "26 May 2011",
                                                               ModificationDateTime = new DateTime(1306420323000),
                                                               ModificationDateTimeDescriptor = "26 May 2011 2:32 PM GMT",
                                                               ModificationTimeDescriptor = "2:32 PM GMT",
                                                               PublicationDateDescriptor = null,
                                                               PublicationDateTime = new DateTime(1306368000000),
                                                               PublicationDateTimeDescriptor = "26 May 2011",
                                                               Reference = new Reference
                                                                               {
                                                                                   guid = "FTCMA00020110526e75q004mu",
                                                                                   mimetype = "text/xml",
                                                                                   type = "accessionNo"
                                                                               },
                                                               Snippets = new SnippetCollection
                                                                              {
                                                                                  "The US economy grew at an unrevised 1.8 per cent in the first quarter, disappointing expectations that growth would be faster than the rate initially reported last month. "
                                                                              },
                                                               SourceCode = "ftcma",
                                                               SourceDescriptor = "Financial Times (FT.Com)",
                                                               Title = "US GDP revision disappoints ",
                                                               TruncatedTitle = "US GDP revision disappoints",
                                                               WordCount = new WholeNumber(482),
                                                               WordCountDescriptor = "482 words"
                                                           },
                                                       new PortalHeadlineInfo
                                                           {
                                                               Authors = new AuthorCollection
                                                                             {
                                                                                 "Pat Sweet "
                                                                             },
                                                               BaseLanguage = "en",
                                                               BaseLanguageDescriptor = "English",
                                                               ContentCategoryDescriptor = "website",
                                                               ContentSubCategoryDescriptor = "webpage",
                                                               HasPublicationTime = true,
                                                               ModificationDateDescriptor = "26 May 2011",
                                                               ModificationDateTime = new DateTime(1306418598000),
                                                               ModificationDateTimeDescriptor = "26 May 2011 2:03 PM GMT",
                                                               ModificationTimeDescriptor = "2:03 PM GMT",
                                                               PublicationDateDescriptor = "26 May 2011",
                                                               PublicationDateTime = new DateTime(1306418640000),
                                                               PublicationDateTimeDescriptor = "26 May 2011 2:04 PM GMT",
                                                               PublicationTimeDescriptor = "2:04 PM GMT",
                                                               Reference = new Reference
                                                                               {
                                                                                   guid = "WCACCUK020110526e75q00006",
                                                                                   mimetype = "text/xml",
                                                                                   type = "accessionNo"
                                                                               },
                                                               Snippets = new SnippetCollection
                                                                              {
                                                                                  "The Big Four accountants are opposing government plans to reform corporate insolvency which are designed to provide greater protection for unsecured creditors. "
                                                                              },
                                                               SourceCode = "wcaccuk",
                                                               SourceDescriptor = "Accountancy",
                                                               Title = "Big Four critical of insolvency reforms ",
                                                               TruncatedTitle = "Big Four critical of insolvency reforms",
                                                               WordCount = new WholeNumber(284),
                                                               WordCountDescriptor = "284 words"
                                                           },
                                                       new PortalHeadlineInfo
                                                           {
                                                               Authors = new AuthorCollection
                                                                             {
                                                                                 "Pat Sweet "
                                                                             },
                                                               BaseLanguage = "en",
                                                               BaseLanguageDescriptor = "English",
                                                               ContentCategoryDescriptor = "website",
                                                               ContentSubCategoryDescriptor = "webpage",
                                                               HasPublicationTime = true,
                                                               ModificationDateDescriptor = "26 May 2011",
                                                               ModificationDateTime = new DateTime(1306418598000),
                                                               ModificationDateTimeDescriptor = "26 May 2011 2:03 PM GMT",
                                                               ModificationTimeDescriptor = "2:03 PM GMT",
                                                               PublicationDateDescriptor = "26 May 2011",
                                                               PublicationDateTime = new DateTime(1306418640000),
                                                               PublicationDateTimeDescriptor = "26 May 2011 2:04 PM GMT",
                                                               PublicationTimeDescriptor = "2:04 PM GMT",
                                                               Reference = new Reference
                                                                               {
                                                                                   guid = "WCACCUK020110526e75q00005",
                                                                                   mimetype = "text/xml",
                                                                                   type = "accessionNo"
                                                                               },
                                                               Snippets = new SnippetCollection
                                                                              {
                                                                                  "Ernst & Young, administrators of failed DIY chain Focus, have confirmed that about 3,000 jobs will be lost after they failed to find a buyer for the loss-making chain. "
                                                                              },
                                                               SourceCode = "wcaccuk",
                                                               SourceDescriptor = "Accountancy",
                                                               Title = "E&Y can’t fix it for Focus ",
                                                               TruncatedTitle = "E&Y can’t fix it for Focus",
                                                               WordCount = new WholeNumber(228),
                                                               WordCountDescriptor = "228 words"
                                                           },
                                                       new PortalHeadlineInfo
                                                           {
                                                               Authors = new AuthorCollection
                                                                             {
                                                                                 "Pat Sweet "
                                                                             },
                                                               BaseLanguage = "en",
                                                               BaseLanguageDescriptor = "English",
                                                               ContentCategoryDescriptor = "website",
                                                               ContentSubCategoryDescriptor = "webpage",
                                                               HasPublicationTime = true,
                                                               ModificationDateDescriptor = "26 May 2011",
                                                               ModificationDateTime = new DateTime(1306418598000),
                                                               ModificationDateTimeDescriptor = "26 May 2011 2:03 PM GMT",
                                                               ModificationTimeDescriptor = "2:03 PM GMT",
                                                               PublicationDateDescriptor = "26 May 2011",
                                                               PublicationDateTime = new DateTime(1306418640000),
                                                               PublicationDateTimeDescriptor = "26 May 2011 2:04 PM GMT",
                                                               PublicationTimeDescriptor = "2:04 PM GMT",
                                                               Reference = new Reference
                                                                               {
                                                                                   guid = "WCACCUK020110526e75q00002",
                                                                                   mimetype = "text/xml",
                                                                                   type = "accessionNo"
                                                                               },
                                                               Snippets = new SnippetCollection
                                                                              {
                                                                                  "Almost a third of CEOs and CFOs of UK mid-market firms think it will be the second half of 2012 before financing improves, according to research by Grant Thornton. "
                                                                              },
                                                               SourceCode = "wcaccuk",
                                                               SourceDescriptor = "Accountancy",
                                                               Title = "CEOs and CFOs see financing improving ",
                                                               TruncatedTitle = "CEOs and CFOs see financing improving",
                                                               WordCount = new WholeNumber(306),
                                                               WordCountDescriptor = "306 words"
                                                           },
                                                       new PortalHeadlineInfo
                                                           {
                                                               Authors = new AuthorCollection
                                                                             {
                                                                                 "Pat Sweet "
                                                                             },
                                                               BaseLanguage = "en",
                                                               BaseLanguageDescriptor = "English",
                                                               ContentCategoryDescriptor = "website",
                                                               ContentSubCategoryDescriptor = "webpage",
                                                               HasPublicationTime = true,
                                                               ModificationDateDescriptor = "26 May 2011",
                                                               ModificationDateTime = new DateTime(1306418598000),
                                                               ModificationDateTimeDescriptor = "26 May 2011 2:03 PM GMT",
                                                               ModificationTimeDescriptor = "2:03 PM GMT",
                                                               PublicationDateDescriptor = "26 May 2011",
                                                               PublicationDateTime = new DateTime(1306418640000),
                                                               PublicationDateTimeDescriptor = "26 May 2011 2:04 PM GMT",
                                                               PublicationTimeDescriptor = "2:04 PM GMT",
                                                               Reference = new Reference
                                                                               {
                                                                                   guid = "WCACCUK020110526e75q00001",
                                                                                   mimetype = "text/xml",
                                                                                   type = "accessionNo"
                                                                               },
                                                               Snippets = new SnippetCollection
                                                                              {
                                                                                  "The Financial Reporting Council’s Auditing Practices Board (APB) has issued revised guidance on the audit of credit unions in the UK. Practice Note 27 The audit of credit unions in the United Kingdom has been updated to reflect the issuance ... "
                                                                              },
                                                               SourceCode = "wcaccuk",
                                                               SourceDescriptor = "Accountancy",
                                                               Title = "Guidance on the audit of UK credit unions ",
                                                               TruncatedTitle = "Guidance on the audit of UK credit unions",
                                                               WordCount = new WholeNumber(157),
                                                               WordCountDescriptor = "157 words"
                                                           },
                                                       new PortalHeadlineInfo
                                                           {
                                                               Authors = new AuthorCollection
                                                                             {
                                                                                 ""
                                                                             },
                                                               BaseLanguage = "en",
                                                               BaseLanguageDescriptor = "English",
                                                               ContentCategoryDescriptor = "publication",
                                                               ContentSubCategoryDescriptor = "article",
                                                               HasPublicationTime = true,
                                                               ModificationDateDescriptor = "26 May 2011",
                                                               ModificationDateTime = new DateTime(1306417917000),
                                                               ModificationDateTimeDescriptor = "26 May 2011 1:51 PM GMT",
                                                               ModificationTimeDescriptor = "1:51 PM GMT",
                                                               PublicationDateDescriptor = "26 May 2011",
                                                               PublicationDateTime = new DateTime(1306408753000),
                                                               PublicationDateTimeDescriptor = "26 May 2011 11:19 AM GMT",
                                                               PublicationTimeDescriptor = "11:19 AM GMT",
                                                               Reference = new Reference
                                                                               {
                                                                                   guid = "LBA0000020110526e75q000um",
                                                                                   mimetype = "text/xml",
                                                                                   type = "accessionNo"
                                                                               },
                                                               Snippets = new SnippetCollection
                                                                              {
                                                                                  "* Q4 net profit 922 mln rupees, vs 2.27 bln rupees * Pricing outlook stable, will trend higher - executive * See stable revenue from largest client BT - executive "
                                                                              },
                                                               SourceCode = "lba",
                                                               SourceDescriptor = "Reuters News",
                                                               Title = "UPDATE 2 -Tech Mahindra Q4 net profit halves on Satyam ",
                                                               TruncatedTitle = "UPDATE 2 -Tech Mahindra Q4 net profit halves on Satyam",
                                                               WordCount = new WholeNumber(527),
                                                               WordCountDescriptor = "527 words"
                                                           }
                                                   }
                               }
                       };
        }
    }
}