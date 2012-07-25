@using DowJones.Documentation.Website.Extensions

Below is an example of using PortalHeadlineList component with data retrieved from WSJ RSS feed.

In this sample, we also give the component a custom HTML template. For more details on customization, please refer to [Customization](../Components/Customization).

The steps taken to render the portal headline list below are:

* Create custom template
	* Create a custom template
	* Create a compiled template function using doT.js
* Retrieve and assemble data
	* Make an AJAX call to WSJ RSS feed to get data
	* Convert the RSS data into portalHeadlineList data model
* Create the component
	* Use DJ.add to create a portal headline list component on the page
		* Pass converted data from above
		* Pass the compiled template function from above

####Portal Headline List
@Html.DemoFrame(System.Configuration.ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/Home/ClientDemo")
