@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the Portal Headline List model (either in your controller or model):

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

	@@model DowJones.Web.Mvc.UI.Components.Models.PortalHeadlineListModel

	<!-- Render the component -->
	@@Html.DJ().Render(portalHeadlineListModel)

`GetData()` can be any method or service call that returns a valid `PortalHeadlineListDataResult`.
A sample implementation of `GetData()` using Factiva Gateway is shown below:

	using DowJones.Ajax.PortalHeadlineList;
	using DowJones.Assemblers.Headlines;
	using DowJones.Formatters.Globalization.DateTime;
	using DowJones.Managers.Search;
	using DowJones.Session;
	using Factiva.Gateway.Messages.Search.V2_0;
	using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
	using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

	private PortalHeadlineListDataResult GetData()
	{
		const string query = "obama and sc=j";
		const int maxResults = 5;

		// initialize request with arbitrary defaults
		var request = new PerformContentSearchRequest()
			{
				MaxResults = maxResults,
			};
			
		request.StructuredSearch.Query.SearchStringCollection.Add(new SearchString
		{
			Mode = SearchMode.Traditional,
			Value = query,
		});
		request.StructuredSearch.Formatting.ExtendedFields = true;
		request.StructuredSearch.Formatting.MarkupType = MarkupType.Highlight;
		request.StructuredSearch.Formatting.SortOrder = ResultSortOrder.PublicationDateReverseChronological;
		request.StructuredSearch.Formatting.SnippetType = SnippetType.Fixed;
		request.NavigationControl.ReturnHeadlineCoding = true;

		// Perform Search (replace new ControlData() with proper controlData object based on session)
		var searchManager = new SearchManager(new ControlData(), new Preferences.Preferences("en"));
		var results = searchManager.PerformContentSearch<PerformContentSearchResponse>(request);

		// map response to DTO
		var headlineListManager = new HeadlineListConversionManager(new DateTimeFormatter("en"));
		var headlineListDataResult = headlineListManager.Process(results);

		// map relevant fields from headlineListDataResult to portalHeadlineListDataResult
		var portalHeadlineListDataResult = PortalHeadlineConversionManager.Convert(headlineListDataResult);

		// return data
		return portalHeadlineListDataResult;
	}

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/PortalHeadlineList/data/cs")