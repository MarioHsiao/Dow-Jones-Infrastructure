@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the Portal Headline List model (either in your controller or model):

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
		Result = GetPortalHeadlineListResult(),   // Something that returns an instance of PortalHeadlineListDataResult
	};
	
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/PortalHeadlineList/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@Html.DJ().Render(portalHeadlineListModel)