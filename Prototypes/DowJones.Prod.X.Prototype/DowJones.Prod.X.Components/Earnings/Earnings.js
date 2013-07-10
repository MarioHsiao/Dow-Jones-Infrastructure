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
        
        _getDevice: function () {
            if (Response.band(1200)) {
                return "Desktop";
            }

            if (Response.band(481)) {
                return "Tablet";
            }

            return "Mobile";
        },

        _resize: function () {
            var device = this._getDevice();
            var column = this._findColumn();
            var type = device + " " + column;
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
                case 'Desktop Large':
                case 'Desktop Medium':
                    this.$element.html(this.templates.successLarge(data));
                    this._currentView = type;
                    break;
                case 'Tablet Large':
                case 'Tablet Medium':
                case 'Mobile Large':
                    this.$element.html(this.templates.successMedium(data));
                    this._currentView = type;
                    break;
                case 'Desktop Small':
                case 'Tablet Small':
                case 'Mobile Medium':
                case 'Mobile Small':
                    this.$element.html(this.templates.successSmall(data));
                    this._currentView = type;
                    break;
            }
        },
        
        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_Earnings', DJ.UI.Earnings);
