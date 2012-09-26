/*!
 * ThemeManager
 *
 */

DJ.UI.ThemeManager = DJ.UI.Component.extend({

    // Default options
    defaults: {
    },
   
    init: function (element, meta) {
        var $meta = $.extend({ name: "ThemeManager" }, meta);

        // Call the base constructor
        this._super(element, $meta);
        this.initializeHighchartsTheme();
        this.initializeHighchartsGradient();
    },

    _initializeDelegates: function () {
    },

    _initializeEventHandlers: function () {
    },

    _initializeElements: function () {
    },
    
    initializeHighchartsGradient: function () {
    
        Highcharts.getOptions().colors = $.map(Highcharts.getOptions().colors, function (color) {
            return {
                radialGradient: { cx: 0.5, cy: 0.3, r: 0.7 },
                stops: [
		            [0, color],
		            [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
                ]
            };
        });
    },
    
    initializeHighchartsTheme: function () {
        var self = this,
            o = self.options;
        var tempOptions = Highcharts.getOptions();
        tempOptions.colors = o.colors;
    },
    
    EOF: null
});


// Declare this class as a jQuery plugin
$.plugin('dj_ThemeManager', DJ.UI.ThemeManager);
