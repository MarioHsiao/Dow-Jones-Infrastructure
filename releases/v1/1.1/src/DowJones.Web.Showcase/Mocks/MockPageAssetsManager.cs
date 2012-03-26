using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.PAM;
using DowJones.Tools.Session;
using DowJones.Utilities.Ajax.Canvas;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using SortBy = Factiva.Gateway.Messages.Assets.Pages.V1_0.SortBy;
using Module = Factiva.Gateway.Messages.Assets.Pages.V1_0.Module;

namespace DowJones.Web.Showcase.Mocks
{
    public class MockPageAssetsManagerFactory : IPageAssetsManagerFactory
    {
        private static MockPageAssetsManager _manager;

        public IPageAssetsManager CreateManager()
        {
            return CreateManager((SessionData)null);
        }

        public IPageAssetsManager CreateManager(Utilities.Ajax.Security.ProxyCredentials credentials)
        {
            return CreateManager((SessionData)null);
        }

        public IPageAssetsManager CreateManager(SessionData sessionData)
        {
            return _manager = _manager ?? new MockPageAssetsManager();
        }
    }

    public class MockPageAssetsManager : IPageAssetsManager
    {
        static IEnumerable<Module> _modules = new Module[] {

            new SyndicationNewspageModule
                {
                    Position = 1,
                    __id = 23332,
                    Title = "RSS",
                    Description = "Aenean lacin ia bibendum nulla sed consectetur. Nullam quisrisus eget urna mollis ornare vel eu leo. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas sed diam eget risus",
                    CustomID = "syndication-module",
                },

            new RadarNewspageModule { CustomID = "news-radar-module", __id = 2345, Title = "News Radar Industry", Position = 1 },
            
            new NewsstandNewspageModule
                {
                    CustomID = "newsstand-module", 
                    __id = 3456, 
                    Title = "Newsstand", 
                    Position = 2,
                    NewsstandListCollection =  new NewsstandListCollection() 
                    {
                        new NewsstandList()
                            {
                                NewsstandQualifier="en",
                                NewsstandQualifierType = NewsstandQualifierType.Unspecified,
                                NewsstandCollection = new NewsstandCollection(){ new Newsstand() {SectionID="2",SourceID="J"}}
                            }
                    }
                },

            new TrendingNewsPageModule { CustomID = "industry-module", __id = 5678, Title = "Industry", Position = 4 },
            
            new RegionalMapNewspageModule { CustomID = "regional-module", __id = 6789, Title = "Regional", Position = 5 },

            new CompanyOverviewNewspageModule { CustomID = "CompanyOverview", __id = 22706, Title = "Company Overview", Position = 6 },

            new SourcesNewspageModule
                {  
                            CustomID = "sources-module",
                            __id = 9012, 
                            Title = "Sources News Page Module", 
                            Position = 7,
                            SourcesListCollection = new SourceListCollection() 
                                                    {
                                                        new SourceList()
                                                        {
                                                            SourceCodes = new SourceCodes() 
                                                            {   
                                                                SourceCodeCollection = new SourceCodeCollection()
                                                                {
                                                                    "Source1","Source2","Source3","Source4","Source5"
                                                                }
                                                            },
                                                            SourceListId = "sourceListId",
                                                            SourceListType=SourceListType.SourceCodes,
                                                            }
                                                    }  
                        },



            new AlertsNewspageModule() 
                        {  
                            CustomID = "alerts-module",
                            __id = 9013, 
                            Title = "Alerts News Page module", 
                            Position = 8,
                            },
        };

        public Module GetModuleById(string pageId, string moduleId)
        {
            return _modules.OrderBy(x => x.Position).Single(x => x.Id == int.Parse(moduleId));
        }

        public void DeleteModules(string pageId, List<string> moduleIds)
        {
            _modules = _modules.Where(x => !moduleIds.Contains(x.Id.ToString())).ToArray();
        }

        public void DeleteModulesForMediaMonitor(string pageId, List<string> moduleIds)
        {
            DeleteModules(pageId, moduleIds);
        }

        public Page CreatePage(CreatePageRequest page)
        {
            throw new NotImplementedException();
        }

        public string AddModuleToPage(string pageId, int numberOfColumns, Module newModule, ValidationRoutine routine)
        {
            AddModuleToEndOfPage(pageId, newModule, routine);
            return newModule.Id.ToString();
        }

        public string AddModuleToEndOfPage(string pageId, Module newModule, ValidationRoutine routine)
        {
            newModule.Position = _modules.Max(x => x.Position) + 1;
            _modules = _modules.Union(new[] { newModule }).ToArray();
            return 3.ToString();
        }

        public void UpdateModulePositionsOnPage(string pageId, IEnumerable<IEnumerable<int>> zones)
        {
            var orderedModules =
                zones.First().Select(moduleId => _modules.Single(x => x.Id == moduleId));

            int position = 0;

            foreach (var module in orderedModules)
                module.Position = position++;
        }

        public void UpdateModuleState(string pageId, string moduleId, ModuleState state)
        {
            _modules.Single(x => x.Id == int.Parse(moduleId)).ModuleState = state;
        }

        public Page GetPage(string pageId)
        {
            if (pageId == (-1).ToString())
                return new MockPage(1, Enumerable.Empty<Module>());

            return new MockPage(int.Parse(pageId), _modules.OrderBy(x => x.Position));
        }

        public void DeletePage(string pageId)
        {
            throw new NotImplementedException();
        }

        public PageListInfoCollection GetPageListInfoCollection(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllPages(IEnumerable<PageType> pageTypes)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllPages(PageListInfoCollection pageListInfoCollection)
        {
            throw new NotImplementedException();
        }

        public void DeletePageFromSessionCache(string cacheKey, CacheScope cacheScope)
        {
            // Ok...  it's deleted!
        }

        public void UpdateModule(string pageId, string moduleId, ICollection<Property> properties)
        {
            var module = _modules.Single(x => x.Id.ToString() == moduleId);

            foreach (var property in properties)
            {
                module.SetPropertyValue(property.name, property.value);
            }
        }

        public void UpdateModulesOnPage(string pageId, Module module)
        {
            throw new NotImplementedException();
        }

        public void AddModuleIdsToPage(string pageRef, List<string> moduleIdWithPositionCollection)
        {
            throw new NotImplementedException();
        }

        public void AddModuleIdsToEndOfPage(string pageRef, List<string> moduleIdWithPositionCollection)
        {
            throw new NotImplementedException();
        }

        public void CreateModule(ModuleEx module)
        {
            throw new NotImplementedException();
        }


        protected class MockPage : Page
        {
            public MockPage(int id, IEnumerable<Module> modules)
            {
                ModuleCollection = new ModuleCollection();
                ModuleCollection.AddRange(modules);
                __id = id;
            }
        }
    }
}
