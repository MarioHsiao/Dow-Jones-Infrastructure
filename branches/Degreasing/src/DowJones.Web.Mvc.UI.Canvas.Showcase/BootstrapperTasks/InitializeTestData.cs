using System.Collections.Generic;
using DowJones.Infrastructure;
using DowJones.Pages;
using DowJones.Pages.Modules.Templates;

namespace DowJones.DegreasedDashboards.Website.BootstrapperTasks
{
    public class InitializeTestData : IBootstrapperTask
    {
        private readonly IPageRepository _pageRepository;
        private readonly IScriptModuleTemplateManager _scriptModuleTemplateManager;

        public InitializeTestData(IPageRepository pageRepository, IScriptModuleTemplateManager scriptModuleTemplateManager)
        {
            _pageRepository = pageRepository;
            _scriptModuleTemplateManager = scriptModuleTemplateManager;
        }

        public void Execute()
        {
            CreatePages();
            CreateModuleTemplates();
        }

        private void CreatePages()
        {
        }

        private void CreateModuleTemplates()
        {
            var rssFeed = _scriptModuleTemplateManager.GetTemplate("1");

            if (rssFeed != null)
                return;

            _scriptModuleTemplateManager.CreateTemplate(new ScriptModuleTemplate {
                Id = 1.ToString(),
                Title = "RSS Feed",
                Description = "Displays an RSS Feed",
                Scripts = new List<string> {
                        @"
                            var container = this.container;

                            // The function that maps an RSS feed to a Portal Headlines component
                            function populatePortalHeadlines(rss) {
                                var items = $('item', rss);

                                var model = {
                                    resultSet: {
                                        count: { value: items.length },
                                        headlines: _.map(items, function (item) {
                                            return {
                                                'title': $('title', item).text(),
                                                'reference': { 'guid': $('link', item).text() },
                                                'snippets': [$('description', item).text()]
                                            };
                                        })
                                    }
                                };

                                DJ.add('PortalHeadlineList', {
                                    container: container,
                                    data: model,
                                    options: {
                                        maxNumHeadlinesToShow: items.length,
                                        displaySnippets: 1
                                    }
                                });
                            }

                            // Retrieve an external RSS feed  (using our custom proxy to facilitate the cross-domain call)
                            $.ajax('platformproxy.asmx?url=' + this.options.feedUrl)
                                .success(populatePortalHeadlines)",
                },
                Options = new List<ScriptModuleTemplateOption> {
                        new ScriptModuleTemplateOption("feedUrl", "Feed URL", defaultValue: "http://online.wsj.com/xml/rss/3_7011.xml")
                    }
            });
        }
    }
}