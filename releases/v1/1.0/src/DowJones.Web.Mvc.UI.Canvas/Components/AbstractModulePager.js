(function ($) {

    DJ.UI.AbstractModulePager = DJ.UI.Component.extend({

        selectors: {
            pager: '.module-pager'
        },


        init: function (element, meta) {
            this._super(element, meta);

            this._validate();

            this._initializePagerComponent();

        },

        _validate: function () {

            var exampleMsg = ['Example of valid initialization is: ',
                              '$(pagerContainer).dj_AbstractModulePager({ ',
                              '    options: { ',
                              '        numPages: <int>,',
                              '        activePage: <int>,',
                              '        getDataHandler: <getData function (e.g. this.getData)>',
                              '        showContentAreaHandler: <this.showContentArea>',
                              '    }',
                              '});'
            ].join('\n');

            if (!this.options) {
                throw ('\'options\' cannot be null or undefined. ' + exampleMsg);
            }

            if (!this.options.numPages || !this.options.activePage) {
                throw ('Both \'options.numPages\' and \'options.activePage\' are required. ' + exampleMsg);
            }
        },


        _initializeElements: function (ctx) {
            this.$pager = $(this.selectors.pager, ctx);
        },


        _initializeDelegates: function () {
            this._super();

            $.extend(this._delegates, {
                OnPagerClick: $dj.delegate(this, this._onPagerClick)
            });
        },


        _initializePagerComponent: function () {
            this.$element.dj_Pager({
                options: {
                    activePage: this.options.activePage,
                    totalPages: this.options.numPages
                }
            });

            this._currentPageIndex = this.options.activePage;

            // plugin is attached to parent container
            // always a good practice as you can use .bind(), .delegate() on parent 
            // without worrying about ajax calls replacing pager markup
            this.pager = this.$element.findComponent(DJ.UI.Pager);

            this._registerChildEventHandlers();
        },


        _setSlideDirection: function (curIndex) {
            this._slideDirection = (curIndex < this._currentPageIndex) ? 'left' : 'right';
            this._currentPageIndex = curIndex;
        },


        _registerChildEventHandlers: function () {
            // register pager click handler
            if (this.$pager && this.pager) {
                this.$element.bind(this.pager.events.pagerClick, this._delegates.OnPagerClick);
            }
        },


        _onPagerClick: function (event, data) {
            if (data && data.index) {
                this._setSlideDirection(data.index);
                this.options.getDataHandler(data.index);
            }
        },


        _hidePager: function () {
            if (this.$pager) { this.$pager.hide(); }
        },


        slideContentArea: function () {
            if (!this.options.showContentAreaHandler) {
                $dj.debug('showContentAreaHandler not set. Cannot show/hide content area.');
                return;
            }

            if (!this._slideDirection) {
                this.options.showContentAreaHandler();
            }
            else {
                this.options.showContentAreaHandler('slide', { direction: this._slideDirection }, 1000);
            }
        },

        setActivePage: function (pageIndex) {
            if (this.pager) {
                this.pager.setActivePage(pageIndex);
            }
        }

    });

    $.plugin('dj_AbstractModulePager', DJ.UI.AbstractModulePager);

    $dj.debug('Registered DJ.UI.AbstractModulePager as dj_AbstractModulePager');

} (jQuery));