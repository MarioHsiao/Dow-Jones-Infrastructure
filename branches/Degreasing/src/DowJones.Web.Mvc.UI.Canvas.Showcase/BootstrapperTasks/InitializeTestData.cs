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

                HtmlLayout = @"<div class='actions clear-fix pull-right'>
	<span class='action prevPage'> <i class='icon-chevron-left'></i></span>
	<span class='action'><span class='currentPage'>1</span> of <span class='maxPage'>5</span></span>
	<span class='action nextPage'> <i class='icon-chevron-right'></i></span>
</div>
<div class='rss-feed'></div>",

                Script = @"var container = $(this.container);
var sectionId = container.parents('.dj_module').attr('id');

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
	        container: $('.rss-feed', container),
	        options: {
	            displaySnippets: 0,
	            allowPagination: true,
	            circularPaging: true,
	            pageSize: 10,
	            pageDirection: 'horizontal',
	            pageSpeed: 500,
	            pagePrevSelector: '#' + sectionId + ' .actions .prevPage',
	            pageNextSelector: '#' + sectionId + ' .actions .nextPage',
	            maxNumHeadlinesToShow: items.length
	        },
	        data: model
	    })
	    .done(function(comp) {
		    function refreshPaging() {
		        container.find('.actions .currentPage').html(comp.currentPageIndex + 1);
		        container.find('.actions .maxPage').html(comp.pagesCount);          
		    }

	    	comp.on('headlineClick.dj.PortalHeadlineList', function(args) {
	            window.open(args.headline.reference.guid);
	        });
	        
	        comp.on('pageIndexChanged.dj.PortalHeadlineList', refreshPaging);

	    	refreshPaging();
	    });
}

// Retrieve an external RSS feed  (using our custom proxy to facilitate the cross-domain call)
$.ajax('platformproxy.asmx?url=' + this.options.feedUrl).success(populatePortalHeadlines)",
                                                                                          
    Styles = @".script-module .rss-feed .article-list {
  list-style: none;
  margin: 5px 10px; }
.script-module .rss-feed .article-group .article-wrap {
  margin-bottom: 5px; }
.script-module .rss-feed .article-headline {
  font-size: 93%;
  font-weight: bold;
  line-height: 16px; }
  .script-module .rss-feed .article-headline a {
    color: #000; }
.script-module .rss-feed .article-meta {
  color: #999; }
  .script-module .rss-feed .article-meta .date-stamp {
    text-transform: none; }
  .script-module .rss-feed .article-meta .article-source,
  .script-module .rss-feed .article-meta .time-stamp,
  .script-module .rss-feed .article-meta .date-stamp,
  .script-module .rss-feed .article-meta .media-length,
  .script-module .rss-feed .article-meta .fi,
  .script-module .rss-feed .article-meta .byline,
  .script-module .rss-feed .article-meta .author {
    display: -moz-inline-stack;
    display: inline-block;
    vertical-align: top;
    color: #999;
    font-size: 85%;
    line-height: normal;
    text-transform: uppercase; }
",
                Options = new List<ScriptModuleTemplateOption> {
                        new ScriptModuleTemplateOption("feedUrl", "Feed URL", defaultValue: "http://online.wsj.com/xml/rss/3_7011.xml")
                    }
            });
        }
    }
}