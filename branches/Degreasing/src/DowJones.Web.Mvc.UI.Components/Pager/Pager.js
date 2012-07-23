//
// Pager Control
// 
// Attaches a Pager control to a module
//
//

    DJ.UI.Pager = DJ.UI.Component.extend({

        options: {
            activePage: 1,
            totalPages: 1,
            pagerClass: "module-pager",
            pagerLinkClass: "pager-link",
            activePageClass: "pager-active"
        },


        events: {
            pagerClick: 'pagerClick.dj'
        },



        init: function (element, meta) {

            var $meta = $.extend({ name: "Pager" }, meta);

            // Call the base constructor
            this._super(element, $meta);


            this._applyPager();

            return this;
        },

        //#region Private Methods

        _applyPager: function () {

            this.$element.empty().append(this._createPager());
            this._initializePagerEvents();

            if (this.options.totalPages < 1) {
                this._showHidePager(false);
                return;
            }

            this._showHidePager(true);
        },


        _showHidePager: function (show) {
            show = show || false;

            $('.' + this.options.pagerClass, this.$element).toggle(show);

        },

        _createPager: function () {

            var $pagerDiv = $('<div></div>').addClass(this.options.pagerClass);

            // temp variables for loop
            var activePageClass
                , pagerIndexClass
                , pagerLinkClass = this.options.pagerLinkClass;
            for (var i = 0, totalPages = this.options.totalPages; i < totalPages; i++) {
                activePageClass = (i == this.options.activePage - 1) ? this.options.activePageClass : '';
                pagerIndexClass = 'pager-' + (i + 1);

                $('<a></a>').addClass([pagerLinkClass, pagerIndexClass, activePageClass].join(' '))
                            .data('index', i + 1)
                            .appendTo($pagerDiv);

            }

            return $pagerDiv;
        },


        _initializePagerEvents: function () {
            var me = this;
            // create a single click handler for all pager links
            this.$element.delegate('a.' + this.options.pagerLinkClass, 'click', function () {
                var $this = $(this);

                // do nothing if the current page is clicked again
                if ($this.hasClass(me.options.activePageClass)) return;

                // deselect the previous one
                $('.' + me.options.activePageClass, $this.parent()).removeClass(me.options.activePageClass);
                // make this current
                $this.addClass(me.options.activePageClass);

                // tell the whole world
                $dj.debug('page ' + $this.data('index') + ' clicked');
                me.$element.triggerHandler(me.events.pagerClick, { index: $this.data('index') });
            });
        },


        //#endregion


        //#region Public Methods

        setActivePage: function (pageIndex) {
            pageIndex = pageIndex || 1;
            this.$element.find('.' + this.options.activePageClass).removeClass(this.options.activePageClass);
            this.$element.find('.pager-' + pageIndex).addClass(this.options.activePageClass);
        }
        //#endregion

    });




    // Declare this class as a jQuery plugin
    $.plugin('dj_Pager', DJ.UI.Pager);


    $dj.debug('Registered DJ.UI.Pager (extends DJ.UI.Component)');
