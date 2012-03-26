

$(function () { // Shorthand for the $(document).ready(function() { - does the same
    $('#myButtonCtrl').live('click', function (ev) { // Takes the event as a parameter
        ev.preventDefault();
        $('#SearchResults').load(($('#pathname').val()) + "AJAX/ShowCreate/");
        return false;
    });
});


//Region Page Garden
$("#clickCustom").click(function () {

    $("#CustomContainer").load(($('#pathname').val()) + "AJAX/ShowCreate/");


});
$("#tabGeographic").click(function () {
    //  $("#tabs-2").load(($('#pathname').val()) + "pages/GetGeographicalList/");
    // alert("loaded");
    //  $("#tabs-2 .labeledscrollpane").labeledScrollPane();
    // alert("after script loaded");


    $("#tabs-2").load(($('#pathname').val()) + "AJAX/GetGeographicalList/", function (response, status, xhr) {
        if (status == "error") {
            alert("error");
            $("#tabs-2").html(msg + xhr.status + " " + xhr.statusText);
        }
        else {
            $("#tabs-2 .labeledscrollpane").labeledScrollPane();

        }

    });








});
$("#tabTopic").click(function () {
    $("#TopicContainer").load(($('#pathname').val()) + "AJAX/GetTopicList/", function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: ";
            $("#TopicContainer").html(msg + xhr.status + " " + xhr.statusText);
        }
        else {
            $("#TopicContainer .labeledscrollpane").labeledScrollPane();

        }

    });
});
$("#tabShared").click(function () {
    $("#SharedContainer").load(($('#pathname').val()) + "AJAX/GetSharedList/", function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: ";
            $("#SharedContainer").html(msg + xhr.status + " " + xhr.statusText);
        }
        else {
            $("#SharedContainer .labeledscrollpane").labeledScrollPane();
           

        }

    });
});