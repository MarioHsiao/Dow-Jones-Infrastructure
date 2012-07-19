@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the `PortalHeadlineList` model:

	using DowJones.Web.Mvc.UI.Components.Common;
	using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

	// Instantiate and fill data
	var data = new PortalHeadlineListDataResult
	{
		// Click on "View Sample Data" to see sample data
	}

	var model = new PortalHeadlineListModel
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
		Data = data
	};

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/PortalHeadlineList/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@model DowJones.Web.Mvc.UI.Components.Models.PortalHeadlineListModel

	@@Html.DJ().Render(Model) 