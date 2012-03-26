(function ($) {

    DJ.UI.EmptyCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "EmptyCanvasModule" }, meta);

            this._super(element, $meta);

            if (this.options.contentUrl) {
                this.$contentArea.html('');
            }

            this._applyPager();
        },

        _initializeElements: function (ctx) {
            this._super(ctx);
            this.$footer = $('.module-footer', ctx);
        },

        _applyPager: function (activePage) {
            var numPages = 2;

            if (numPages > 1) {
                this.modulePager =
                        this.$footer.dj_AbstractModulePager({
                            options: {
                                numPages: numPages,
                                activePage: activePage || 1,
                                getDataHandler: $dj.delegate(this, this._onPager),
                                showContentAreaHandler: $dj.delegate(this, this.showContentArea)
                            }
                        });
            }
            else if (this.modulePager && this.modulePager.pager) {
                this.modulePager.pager._showHidePager(false);
            }
        },

        _onPager: function (pageIndex) {
            this.pageIndex = pageIndex;
            this.getData();
        },

        getData: function (forceCacheRefresh) {
            this._super(forceCacheRefresh);

            var showContentArea = $dj.delegate(this, this.showContentArea);

            if (this.options.contentUrl) {
                this.$contentArea.load(
                    this.options.contentUrl + '?sleep=2',
                    function () { showContentArea(); }
                );
            }
            else {
                setTimeout(showContentArea, 2000);
            }
        },

        EOF: null
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_EmptyCanvasModule', DJ.UI.EmptyCanvasModule);

    $dj.debug('Registered DJ.UI.EmptyCanvasModule as dj_EmptyCanvasModule');

})(jQuery);