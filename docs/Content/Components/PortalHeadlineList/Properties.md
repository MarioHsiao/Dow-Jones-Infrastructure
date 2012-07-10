Portal Headline List component's model exposes the following properties that let you tweak many aspects of the component. The following table lists out the properties: 

   Properties				|  Description											
----------------------------|----------------------------------------------------------------------------------------------
MaxNumHeadlinesToShow		| Limits the number of headlines displayed. <span class="label">Default: 5</span>
Layout						| Specifies the headlines layout - `Default`, `Author`, `Timeline`
DisplaySnippets				| Specifes how "Snippets" are displayed - `None`, `Inline`, `Hybrid`, `Hover`, `HybridHover`.
DisplayNoResultsToken		| Show/hide "No Results" token when headline dataset is empty.
SourceClickable				| Specifies whether the "Source" span raises event when clicked.
AuthorClickable				| Specifies whether the "Author" span raises event when clicked.
ShowAuthor					| Shows/hides the "Author" span.
ShowSource					| Shows/hides the "Source" span.
ShowPublicationDateTime		| Shows/hides the "Publication" date and time.
ShowTruncatedTitle			| If enabled, the truncated title is shown, otherwise the full title is shown.
MultimediaMode				| Whether multimedia related icons/divs should be shown.
Data						| Dataset for headlines. Takes a `PortalHeadlineListDataResult`. Click the <span class="btn btn-mini btn-info">View Sample Data</span> button to see examples.
