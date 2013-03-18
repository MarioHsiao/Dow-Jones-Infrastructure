$(function () {
    var PMGlobal = PMGlobal || {};


    PMGlobal.init = function () {
        DJ.$dj.debug('PMGlobal.init: Entered');
        try {
            //START:Headline List click events
            DJ.$dj.subscribe('headlineClick.dj.HeadlineListCarousel', function (args) {
                PMGlobal.anchorClick(args, 'HeadlineList');
            });
            var data1 = $('div').find('.carouselItem.Selected').data("headlineInfo");
            $('.articlepane').load('/article?acn=' + data1.guid);
        }
        catch (e) {
            DJ.$dj.debug(e);
        }
    }
    PMGlobal.anchorClick = function (data) {

        // alert(data.title + ' ' + data.guid + ' ' + data.selectedIndex);
        $('.articlepane').load('/article?acn=' + data.guid);
    }
    PMGlobal.init();
});