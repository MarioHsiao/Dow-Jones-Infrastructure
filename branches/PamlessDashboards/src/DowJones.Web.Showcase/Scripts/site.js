$dj.registerNamespace('EMG.DowJones.Rendering.WebServices.CanvasRenderingManagerDataService');

function HeadlineList_OnHeadlineClick(sender, headline) {
    var headlineUrl = headline.reference.externalUri;
    window.open(headlineUrl, "_blank", "");
}

function DemoModule_GlobalTimestampUpdated(sender, timestamp) {
    $dj.debug("DemoModule_GlobalTimestampUpdated: " + timestamp);
}