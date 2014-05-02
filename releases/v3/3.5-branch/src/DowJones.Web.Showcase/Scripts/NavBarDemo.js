$(function () {
  
    $('#dj_canvas_nav').bind('tabClick.dj.NavBar', function (evt, args) {
        var parentId = args.data.metaData && args.data.metaData.parentId;
        $dj.debug('Parent ID: ', parentId);
    });

});