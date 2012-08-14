using System.Collections.Generic;
using DowJones.Infrastructure;
using DowJones.Pages;
using DowJones.Pages.Modules;

namespace DowJones.DegreasedDashboards.Website.BootstrapperTasks
{
    using DowJones.Web.Mvc.UI.Canvas.GatewayFree.Modules;

    public class InitializeTestData : IBootstrapperTask
    {
        private readonly IPageManager _pageManager;
        private readonly IModuleTemplateManager _moduleTemplateManager;

        public InitializeTestData(IPageManager pageManager, IModuleTemplateManager moduleTemplateManager)
        {
            _pageManager = pageManager;
            _moduleTemplateManager = moduleTemplateManager;
        }

        public void Execute()
        {
            CreatePages();
            CreateModuleTemplates();
        }

        private void CreatePages()
        {
            _pageManager.CreatePage(new Page
                                        {
                                            ID = "1234",
                                            Title = "Test Page 1234",
                                            ModuleCollection = new List<Module>
                                                                   {
                                                                       new HtmlModule
                                                                           {
                                                                               Id = 1,
                                                                               Position = 5,
                                                                               Title = "Simple Module",
                                                                               Html = "<p style='text-align: center; width:160px'>Just some simple HTML (check the console for a script example)</p>",
                                                                               Script = "console.log('Simple HTML Module '+this.name+' loaded!')",
                                                                           }, /*
                            new HtmlModule { Id = 3, 
                                Position = 4,
                                Title = "More interesting HTML Module", 
                                Html = "<strong><span class='time'/></strong>",
                                Script = @"setInterval(function () { $('.time', this.element).text(new Date().toLocaleString()); }, 100);",
                            },*/
                                                                       new HtmlModule
                                                                           {
                                                                               Id = 3,
                                                                               Position = 2,
                                                                               Title = "WSJ RSS Feed Link Viewer",
                                                                               Html = @"<iframe class='external-view' border=0 width='855px' height='400px' src='about:blank'></iframe>",
                                                                               Script = @"
                                    var externalView = $('.external-view', this.$element);
                                    DJ.subscribe('headlineClick.dj.PortalHeadlineList', 
                                       function (data) {
                                          console.log('Got ' + data.headline.reference.guid);
                                          externalView.attr('src', data.headline.reference.guid);
                                       }
                                    );",
                                                                           },
                                                                       new HtmlModule
                                                                           {
                                                                               Id = 4,
                                                                               Position = 1,
                                                                               Title = "WSJ RSS Feed",
                                                                               Html = "<div class='rss-feed' style='height: 200px; overflow-y: auto'></div>",
                                                                               Script = @"
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
                                            container: $('.rss-feed', this.element).get(0),
                                            data: model,
                                            options: {
                                                maxNumHeadlinesToShow: items.length,
                                                displaySnippets: 1
                                            }
                                        });
                                    }

                                    // Retrieve an external RSS feed  (using our custom proxy to facilitate the cross-domain call)
                                    $.ajax('../platformproxy.asmx?url=http://online.wsj.com/xml/rss/3_7011.xml')
                                        .success(populatePortalHeadlines)",
                                                                           },
                                                                       new EmbeddedContentModule
                                                                           {
                                                                               Id = 5,
                                                                               Position = 3,
                                                                               Title = "Embedded Content Module",
                                                                               Url = "http://montana.dev.us.factiva.com/MVCShowCase/NewsRadar90DayAvg",
                                                                               Height = 350,
                                                                               Width = 600,
                                                                           },
                                                                       /*  new HtmlModule { Id = 6, 
                                Title = "Widget Example", 
                                Html = @"
                                  <style type='text/css'>
                                    body { font-family:Verdana; font-size:11px; color:#000000; background:url(/Developer/Content/images/examples-bg.png); margin:7px; }
                                  </style>
                                  <!-- The following script reference is required for the widgets to work -->
                                  <script type='text/javascript' src='http://widgets.dowjones.com/Widgets/2.0/common.js?sessionId=27140ZzZINHEQT2TAAAGUBAAAAAAHGBOAAAAAABSGAYTEMBYGAZTCNRTGUZTMMRX'></script>
                                  <!-- This div element is where the headlines widget will be rendered -->
                                  <div id='headlinesContainer' class='djModule'></div>
                                  <!-- Define the Headlines Widget -->
                                  <script type='text/javascript' src='http://widgets.dowjones.com/Widgets/2.0/widget.js#w=headlines&container=headlinesContainer&sourcetype=querystring&sourcevalue=solar&title=Solar Headlines'></script>
                                "
                            },*/
                                                                   }
                                        });

            if (_pageManager is RavenDbPageManager)
                ((RavenDbPageManager)_pageManager).SaveChanges();
        }

        private void CreateModuleTemplates()
        {
            _moduleTemplateManager.CreateTemplate(new ModuleTemplate {
                Title = "RSS Feed",
                Description = "Displays an RSS Feed",
                ModuleType = typeof(HtmlModule),
                MetaData = new List<ModuleTemplateMetaDatum> {
                        new ModuleTemplateMetaDatum {
                                Name = "Script", 
                                Value = @"
                                    var container = this.$contentArea.append('<div class=rss-feed>').get(0);

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
                                    $.ajax('../platformproxy.asmx?url=' + this.options.feedUrl)
                                        .success(populatePortalHeadlines)",
                            }
                    },
                Options = new List<ModuleTemplateOption> {
                        new ModuleTemplateOption("feedUrl", "Feed URL", defaultValue: "http://online.wsj.com/xml/rss/3_7011.xml")
                    }
            });
        }
    }
}