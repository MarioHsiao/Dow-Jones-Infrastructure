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
                                Position = 5,
                                Title = "Simple Module", 
                                Html = "<p style='text-align: center; width:160px'>Just some simple HTML (check the console for a script example)</p>",
                                Script = "console.log('Simple HTML Module '+this.name+' loaded!')",
                            },
                            new HtmlModule { Id = 3, 
                                Position = 4,
                                Title = "More interesting HTML Module", 
                                Html = "<strong><span class='time'/></strong>",
                                Script = @"setInterval(function () { $('.time', this.element).text(new Date().toLocaleString()); }, 100);",
                            },
                            /*new EmbeddedContentModule { Id = 3, 
                                Position = 2,
                                Title = "Dow Jones SBK",
                                Width = 215,
                                Height = 350,
                                Url = "https://maps.google.com/maps?f=q&amp;source=s_q&amp;hl=en&amp;geocode=&amp;q=820+Ridge+Rd,+Monmouth+Junction,+NJ&amp;aq=1&amp;oq=8&amp;sll=37.6,-95.665&amp;sspn=75.424868,56.513672&amp;t=h&amp;ie=UTF8&amp;hq=&amp;hnear=820+Ridge+Rd,+South+Brunswick+Township,+Middlesex,+New+Jersey+08540&amp;z=14&amp;ll=40.367372,-74.585825&amp;output=embed&amp;iwloc=near",
                            },*/
                            new HtmlModule { Id = 4, 
                                Position = 1,
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
        }
    }
}