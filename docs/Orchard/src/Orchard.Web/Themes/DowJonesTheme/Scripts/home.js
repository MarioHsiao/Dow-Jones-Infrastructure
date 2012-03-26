$(function () {

    //Ma: Plan to make it render dynamically.
    // Setup google webfont
//    var fontCssUrl = '@{ Url.Content( "~/Content/css/fonts.css" ) }';
//    window.WebFontConfig = {
//        custom: {
//            families: ['DowJonesCooperCondensed', 'DowJonesCooperMedium', 'DowJonesCooperLight'],
//            urls: [fontCssUrl]
//        }
//    };
		


    $("#slideshow").cycle({
        pager: "#slideshow-pager",
        pagerAnchorBuilder: function (index, element) { return '<a href="#"><!----></a>'; },
        pause: true,
        pauseOnPagerHover: true
    });
});