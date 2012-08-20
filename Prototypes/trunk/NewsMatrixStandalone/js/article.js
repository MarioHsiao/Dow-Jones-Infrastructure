DJ.Notify(articleWidgetLoaded).on("widgetLoaded").forType('article').attach();
function articleWidgetLoaded(event, data) {
    event.widget.set({
        sourcetype: 'article',
        sourcevalue: unescape(getUrlVars()['articleid'])
    });
}

$("a.djArticleLink").live("click", function() {
    window.open($(this).attr("href"));
    return false;
})