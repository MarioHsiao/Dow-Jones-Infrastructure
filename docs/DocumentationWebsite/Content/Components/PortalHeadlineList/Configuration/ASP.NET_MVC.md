@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the Portal Headline List model:

	using DowJones.Web.Mvc.UI.Components.Common.Types;
	using DowJones.Web.Mvc.UI.Components.Models;

	var portalHeadlineListModel = new PortalHeadlineListModel
	{
		MaxNumHeadlinesToShow = 5,
		ShowAuthor = true,
		ShowSource = true,
		ShowPublicationDateTime = true,
		ShowTruncatedTitle = false,
		AuthorClickable = true,
		SourceClickable = true,
		DisplaySnippets = SnippetDisplayType.Hover,
		Layout = PortalHeadlineListLayout.HeadlineLayout,
		Data = GetData(),   // Something that returns an instance of PortalHeadlineListDataResult
	};

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@Html.DJ().Render(portalHeadlineListModel)

`GetData()` can be any method or service call that returns a valid `PortalHeadlineListDataResult`.
A sample implementation of `GetData()` using Factiva Gateway is shown below:

	using DowJones.Ajax.PortalHeadlineList;
	using DowJones.Assemblers.Headlines;
	using DowJones.Formatters.Globalization.DateTime;

	private PortalHeadlineListDataResult GetData()
	{
		// random feed url
		const string url = "http://feeds.haacked.com/haacked";

		var headlineListManager = new HeadlineListConversionManager(new DateTimeFormatter("en"));

		// process the feed a get a HeadlineListDataResult
		var feed = headlineListManager.ProcessFeed(url);

		// map relevant fields from response to portalHeadlineListDataResult
		var portalHeadlineListDataResult = PortalHeadlineConversionManager.Convert(feed.result);

		// return data
		return portalHeadlineListDataResult;
	}

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/PortalHeadlineList/data/cs")