This library extends the core ASP.Net MVC framework and provides the following services:


Services															| Description
--------------------------------------------------------------------|---------------------------------------------------------------------------------------------
Session Timeout handler and redirect to login page					| Provide Filters that will handle interaction with "Factiva Plateform Ecosystem's Login Server"
Script/Stylesheet Registry											| Utility classes used for registrating JavaScript/CSS files with the MVC Site and components.  Helps the framework track added resources so only one copy get brought down to the site as well as optimizing the download experience for the client.
HtmlHelper and ViewData extensions									| Various extensions created to work with the framewrork within *Views*.
Repository for Generic ModelBinder(s)								| Provides out of the box Model Binders for processing request objects that come as JSON *(JsonModelBinder)* or comma-seperated *(CommaStringSplitModelBinder)*
ClientState handling												| Utilities/Abstactions for using *Factiva Platform Services* to store session-based information specific to the site.
`ViewComponent` base class											| Foundational class for building components and utilizing them in the framework.
Threading and COM+ compatibility attributes							| Utilities/Abstactions for handling COM+ calls from controllers.  This is comparable logic `aspcompat` in *Microsoft Asp.Net Web Pages* 
Task Parallel Library												| Generic Library to use ease the use of `.Net 4.0 Tasks` within the framework.