/*!
 * HeadlineCarouselComp
 *   e.g. , "this._imageSize" is generated automatically.
 *
 *   
 *  Getters and Setters are generated automatically for every Client Property during init;
 *   e.g. if you have a Client Property called "imageSize" on server side code
 *        get_imageSize() and set_imageSize() will be generated during init.
 *  
 *  These can be overriden by defining your own implementation in the script. 
 *  You'd normally override the base implementation if you have extra logic in your getter/setter 
 *  such as calling another function or validating some params.
 *
 */

(function ($) {

    DJ.UI.HeadlineListCarousel = DJ.UI.Component.extend({
        selectors: {
            title: '.title',
            headlineItem: '.carouselItem',
            headlineTitle: '.scrollable .title',
            Headline_scrollable: '.scrollable',
            Headline_ticker_selected: '.selected'

        },

        // Default options
        defaults: {
            debug: false,
            cssClass: 'HeadlineListCarousel'
            // ,name: value     // add more defaults here separated by comma
        },

        // Localization/Templating tokens
        tokens: {
            // name: value     // add more defaults here separated by comma
        },

        events: {
            headlineClick: 'headlineClick.dj.HeadlineListCarousel'
        },
        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "HeadlineListCarousel" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            this.setNextPrevEvents();
            // TODO: Add custom initialization code like the following:
            // this._testButton = $('.testButton', element).get(0);
        },
        _initializeEventHandlers: function () {
            this._super();
            var $container = this.$element,
                self = this;

            $container.delegate(self.selectors.headlineTitle, 'click', function (ev) {
                var data = self._getHeadlineData(this);

                self.publish(self.events.headlineClick, data);

                return false;
            });
        },
        /*
        * Public methods
        */
        setNextPrevEvents: function () {
            $(this.selectors.Headline_scrollable, this.$element).scrollable(
            {
                vertical: true,
                mousewheel: true
            });          
            var pagesize = parseInt($(this.$element).find("#carouselItemListID").data("pagesize"), 0);
            var scrollSpeed = parseInt($(this.$element).find("#carouselItemListID").data("scrollspeed"), 0);
            var selectedIndex = parseInt($(this.$element).find('.carouselItem.Selected').data("index"), 0);
            var api = $("#scroller").data("scrollable");
            var moveby = 0;
            if (selectedIndex != NaN && selectedIndex > pagesize) {
                moveby = Math.ceil(selectedIndex / pagesize) - 1;
                api.move(moveby, scrollSpeed);
            }

        },

        _getHeadlineData: function (elem) {
            var $headlineItem = $(elem).closest(this.selectors.headlineItem);
            var titleStr = $headlineItem.children(this.selectors.title).text();
            var data = $headlineItem.data('headlineinfo');
            return $.extend({ title: titleStr }, data, { selectedIndex: $headlineItem.data('index') });
        },
        // TODO: Public Methods here


        /*
        * Private methods
        */

        // DEMO: Overriding the base _paint method:
        _paint: function () {

            // "this._super()" is available in all overridden methods
            // and refers to the base method.
            this._super();

            alert('TODO: implement HeadlineListCarousel._paint!');
        }


    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_HeadlineListCarousel', DJ.UI.HeadlineListCarousel);


})(jQuery);