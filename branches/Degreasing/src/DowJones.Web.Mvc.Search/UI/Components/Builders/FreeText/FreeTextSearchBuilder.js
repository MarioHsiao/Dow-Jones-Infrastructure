/*!
 * Demo1
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

    DJ.UI.FreeTextSearchBuilder = DJ.UI.QueryBuilder.extend({

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "FreeTextSearchBuilder" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code like the following:
            // this._testButton = $('.testButton', element).get(0);
        },

        /*
        * Public methods
        */

        getQuery: function () {
            return {};
        },

        _initializeElements: function () {
            this._moreLink = this.$element.find('.more');
            this._modifySearch = this.$element.find('.modifySearch');
            this._saveAsBtn = this.$element.find('.save-as_btn');
            this._saveAsMenu = this.$element.find('.saveAsMenu');
        },

        _initializeEventHandlers: function () {
            var me = this;
            
            //More Link
            this._moreLink.click(this._delegates.OnMoreLinkClick);

            //Modify Search
            this._modifySearch.click(this._delegates.OnModifySearchClick);

            // Save as menu
            this._saveAsBtn.click(function (e) {
                e.stopPropagation(); //Need to stop the event propagation
                me._onSaveAsClick(this);
            });

            // Save as menu item select
            this._saveAsMenu.delegate('.label', 'click', function () {
                $dj.publish('saveAs.dj.FreeTextSearchBuilder', { saveAs: $(this).data('saveas') });
            }).appendTo(document.body);
        },

        _initializeDelegates: function () {
            this._super();

            $.extend(this._delegates, {
                OnMoreLinkClick: $dj.delegate(this, this._onMoreLinkClick),
                OnModifySearchClick: $dj.delegate(this, this._onModifySearchClick)
            });
        },

        _onMoreLinkClick: function () {
            $(this._moreLink).popupBalloon({
                height: 400,
                width: 'auto',
                content: $('.dj_query-summary-table').clone().removeClass('hide'),
                jScrollPaneEnabled: false,
                title: '<%=Token("yourSearchQueryBuild")%>',
                popupPosition: 'bottom',
                popupAlign: 'right',
                xAlign: 'center',
                yAlign: 'bottom',
                yOffset: -8,
                animateDisplayEnabled: false,
                animateHideEnabled: true,
                animateMoveEnabled: false
            });
        },

        _onModifySearchClick: function () {
            $dj.publish('modifySearch.dj.FreeTextSearchBuilder');
        },

        _onSaveAsClick: function (elem) {
            var $elem = $(elem), elemOffset = $elem.offset();
            this._saveAsBtn.addClass('active');
            this._saveAsMenu.css({ top: elemOffset.top + $elem.outerHeight(), left: elemOffset.left, position: "absolute" }).show();
            $(document).unbind('mousedown.SaveAs').bind('mousedown.SaveAs').click($dj.delegate(this, function () {
                this._saveAsBtn.removeClass('active');
                this._saveAsMenu.hide();
                $(document).unbind('mousedown.SaveAs');
            }));
        },

        EOF: null
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_FreeTextSearchBuilder', DJ.UI.FreeTextSearchBuilder);
