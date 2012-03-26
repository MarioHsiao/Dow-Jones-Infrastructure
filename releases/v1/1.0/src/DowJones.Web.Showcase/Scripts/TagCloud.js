$(function() {
    setTimeout("SetTagCloudData()", 500);
})
function SetTagCloudData() {
    var data = { "result": [{ "code": null, "navigateUrl": "", "snippet": "", "tagWeight": { "displayText": { "value": "34.69" }, "value": 34.6858 }, "text": "business unit", "type": 0, "distributionIndex": 2 }, { "code": null, "navigateUrl": "", "snippet": "", "tagWeight": { "displayText": { "value": "5.29" }, "value": 5.2856 }, "text": "cash flow", "type": 0, "distributionIndex": 4 }, { "code": null, "navigateUrl": "", "snippet": "", "tagWeight": { "displayText": { "value": "4.88" }, "value": 4.8772 }, "text": "corporate credit", "type": 0, "distributionIndex": 5}] };
    //var data = { "result": [] };
    $(".dj_TagCloud").findComponent('dj_TagCloud').setData(data);

    //var data = { code: 123, message: 'Please put correcy keyword' }
    //$(".dj_TagCloud").dj_TagCloud("bindOnError", data);
}



