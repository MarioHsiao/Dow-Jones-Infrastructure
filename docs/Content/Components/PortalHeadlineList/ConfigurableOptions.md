You can configure many aspects of Portal Headline List component. The following table lists out the configurable options (Properties in ASP.NET MVC and Options in Javascript): 

 Properties 				|  Options					  |  Description											
----------------------------|-----------------------------|----------------------------------------------------------------------------------------------
MaxNumHeadlinesToShow		| maxNumHeadlinesToShow		  | Limits the number of headlines displayed. <span class="label">Default: 5</span>
Layout						| layout					  | Specifies the headlines layout - `Default`, `Author`, `Timeline`
DisplaySnippets				| displaySnippets			  | Specifes how "Snippets" are displayed - `None`, `Inline`, `Hybrid`, `Hover`, `HybridHover`.
DisplayNoResultsToken		| displayNoResultsToken		  | Show/hide "No Results" token when headline dataset is empty.
SourceClickable				| sourceClickable			  | Specifies whether the "Source" span raises event when clicked.
AuthorClickable				| authorClickable			  | Specifies whether the "Author" span raises event when clicked.
ShowAuthor					| showAuthor				  | Shows/hides the "Author" span.
ShowSource					| showSource				  | Shows/hides the "Source" span.
ShowPublicationDateTime		| showPublicationDateTime	  | Shows/hides the "Publication" date and time.
ShowTruncatedTitle			| showTruncatedTitle		  | If enabled, the truncated title is shown, otherwise the full title is shown.
MultimediaMode				| multimediaMode			  | Whether multimedia related icons/divs should be shown.
Data						| data						  | Dataset for headlines. Takes a `PortalHeadlineListDataResult`. Click the <span class="btn btn-mini btn-info">View Sample Data</span> button to see examples.
