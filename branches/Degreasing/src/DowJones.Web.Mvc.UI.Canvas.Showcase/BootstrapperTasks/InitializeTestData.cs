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

        public InitializeTestData(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        public void Execute()
        {
            _pageManager.CreatePage(new Page
                {
                    ID = "1234", 
                    Title = "Test Page 1234",
                    ModuleCollection = new List<Module>
                        {
                            new HtmlModule { Id = 1, 
                                Position = 1,
                                Title = "Simple HTML Module", 
                                Html = "<strong>It works!</strong>",
                                Script = "console.log('Simple HTML Module '+this.name+' loaded!')",
                            },
                            new HtmlModule { Id = 3, 
                                Position = 4,
                                Title = "More interesting HTML Module", 
                                Html = "<strong><span class='time'/></strong>",
                                Script = @"
                                    var timeEl = $('.time', this.element);
                                    setInterval(function () {
                                        timeEl.text(new Date().toLocaleString());
                                    }, 100);
                                ",
                            },
                            new HtmlModule { Id = 3, 
                                Position = 5,
                                Title = "External HTML Module", 
                                Html = "<script src='http://nmp.newsgator.com/NGBuzz/buzz.ashx?buzzId=81503&apiToken=AAA11919E8A84EB8986088D0B39F3E0B&trkP=&trkM=94EB6FAB-58C5-EE96-F3DB-C2444C40C1BA' type='text/javascript'></script>",
                            },
                            new HtmlModule { Id = 4, 
                                Position = 3,
                                Title = "Component HTML Module", 
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
                                            },
                                            eventHandlers: {
                                                'headlineClick.dj.PortalHeadlineList': function (data) {
                                                    window.open(data.headline.reference.guid);
                                                }
                                            }
                                        });
                                    }

                                    // Retrieve an external RSS feed  (using our custom proxy to facilitate the cross-domain call)
                                    $.ajax('../platformproxy.asmx?url=http://online.wsj.com/xml/rss/3_7011.xml')
                                        .success(populatePortalHeadlines)",
                            },
                            new EmbeddedContentModule { Id = 5, 
                                Position = 2,
                                Title = "Embedded Content Module", 
                                Url = "http://montana.dev.us.factiva.com/MVCShowCase/NewsRadar90DayAvg",
                                Height = 350,
                                Width = 600,
                            },
                       }
                });
        }
    }
}