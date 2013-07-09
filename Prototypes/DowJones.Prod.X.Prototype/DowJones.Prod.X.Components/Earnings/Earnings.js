/*!
 * EarningsComponentModel
 */

    DJ.UI.Earnings = DJ.UI.Component.extend({

        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "Earnings" }, meta));
            this._resize();
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                // TODO: Add delegates
                // e.g.  OnHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
            });
        },

        _initializeElements: function () {
            // TODO: Get references to child elements
            // e.g.  this._headlines = this.$element.find('.clear-filters');,
        },

        _initializeEventHandlers: function () {
            DJ.subscribe('dj.productx.core.widgetSorted', $dj.delegate(this, this._resize));
            Response.resize($dj.delegate(this, this._resize));
        },
        
        _getBand: function () {
            if (Response.band(1200)) {
                return "Large";
            }

            if (Response.band(481)) {
                return "Medium";
            }

            return "Small";
        },

        _resize: function () {
            var band = this._getBand();
            var column = this._findColumn();
            var type = band + " " + column;
            this._processTemplate(type, null);
        },

        _findColumn: function () {
            var selector = 'ul.column';
            var column = this.$element.closest(selector);
            if (!column) {
                column = 'ul.xColumn';
            }

            if (column.hasClass('largeColumn')) {
                return 'Large';
            }

            if (column.hasClass('mediumColumn')) {
                return 'Medium';
            }

            return 'Small';

        },

        _processTemplate: function (type, data) {
            if (this._currentView === type) {
                return;
            }

            switch (type) {
                case 'Large Large':
                case 'Large Medium':
                    this.$element.html(this.templates.successLarge(data));
                    this._currentView = type;
                    break;
                case 'Medium Large':
                case 'Medium Medium':
                case 'Small Large':
                    this.$element.html(this.templates.successMedium(data));
                    this._currentView = type;
                    break;
                case 'Large Small':
                case 'Medium Small':
                case 'Small Medium':
                case 'Small Small':
                    this.$element.html(this.templates.successSmall(data));
                    this._currentView = type;
                    break;
            }
        },
        
        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_Earnings', DJ.UI.Earnings);
