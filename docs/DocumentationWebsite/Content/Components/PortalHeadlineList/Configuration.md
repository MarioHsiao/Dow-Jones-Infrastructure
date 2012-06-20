#### Rendering the component from a Razor View

Add the following lines to render the PortalHeadlineList component from a Razor View:
	<!-- Render the component -->
	@Html.DJ().RenderComponent("PortalHeadlineListControl", Model)