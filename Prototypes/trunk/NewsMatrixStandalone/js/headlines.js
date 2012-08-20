DJ.Notify(headlinesWidgetLoaded).on("widgetLoaded").forType('headlines').attach();
function headlinesWidgetLoaded(event, data) {
    event.widget.set({
        searchmode: 'simple',
        sourcetype: 'querystring',
        sourcevalue: unescape(getUrlVars()['querystring'])
    });
}

DJ.Notify(headlineClickedHandler).on("headlineClicked").attach();
function headlineClickedHandler(event, data) {
    var url = "article.html?articleid=" + escape(data.data.Article.ArticleId);
    var windowName = "radar90Article";
    window.open(url, windowName, "scrollbars=yes,width=800").focus();
}
