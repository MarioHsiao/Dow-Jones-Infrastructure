The following table describes the events available for the Article widget. 
Details covering how to use these events can be found [here](http://widgets.dowjones.com/developer/Widgets/2.0/documentation/Events).

   Event			|  Description											
--------------------|----------------------------------------------------------------------------------------------
dataRequested		| A request for data has been made.
dataReceived		| The data response has been received.
dataTransformed		| The Widget will transform the data received from the web service to match template expectations. dataTransformed is triggered right after these transformations are complete.
error				| An error message has been received by the widget.
headlineClicked		| The headlineClicked event fires immediately after a link to another article within the article is clicked.
widgetInitialized	| The Widget has been retreived from the server and has been loaded into the browser.
widgetRendered		| The Widget has been attached to the document.
viewRendered		| Triggered immediately after any part of the widget template is used to create html but before this html is attached to the document.
