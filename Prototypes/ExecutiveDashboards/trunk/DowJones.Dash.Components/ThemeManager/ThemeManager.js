/*!
 * ThemeManager
 *
 */

DJ.UI.ThemeManager = DJ.UI.Component.extend({

    // Default options
    defaults: {
        debug: false,
        useGradientsInCharts: true,
    },

    instance: null,

    init: function (element, meta) {

        // loose singleton implementation
        if (DJ.UI.ThemeManager.instance)
            return;

        var $meta = $.extend({ name: "ThemeManager" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        if (this.options.debug) {
            DJ.config.debug = true;
        }

        this._initializeNamedColors();
        this.initializeHighchartsGradient();

        DJ.UI.ThemeManager.instance = this;

    },

    _initializeNamedColors: function () {
        this.colors = {
            blue: this.options.colors[0],
            red: this.options.colors[1],
            green: this.options.colors[2],
            purple: this.options.colors[3],
            ltBlue: this.options.colors[6],
            yellow: this.options.colors[5],
            grey: "#CCCCCC",
            siteBackground: "#3C3C3C",
            
            brighten: function (color, brightness) {
                Highcharts.Color(color).brighten(brightness).get('rgb');
            }
        };
    },

    initializeHighchartsGradient: function () {
        Highcharts.getOptions().colors = $.map(this.options.colors, function (color) {
            return {
                radialGradient: { cx: 0.5, cy: 0.3, r: 0.7 },
                stops: [
		            [0, color],
		            [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
                ]
            };
        });
    }
});

// Declare this class as a jQuery plugin
$.plugin('dj_ThemeManager', DJ.UI.ThemeManager);

