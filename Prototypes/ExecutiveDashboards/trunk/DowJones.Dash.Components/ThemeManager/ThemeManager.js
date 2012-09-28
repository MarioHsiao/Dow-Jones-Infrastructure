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

        if (!DJ.UI.ThemeManager.instance) {
            DJ.UI.ThemeManager.instance = this;
        }
        this._initializeObject();
        this.initializeHighchartsTheme();
        this.initializeHighchartsGradient();
    },
    
    _initializeEventHandlers: function () {
    },
    
    _initializeObject: function () {
        this.colors = {
            blue: $dj.delegate(this,function() {
                return this.options.colors[0];
            }),
            red: $dj.delegate(this,function() {
                    return this.options.colors[1];
            }),
            green: $dj.delegate(this,function() {
                    return this.options.colors[2];
            }),
                purple: $dj.delegate(this,function() {
                    return this.options.colors[3];
            }),
            ltBlue: $dj.delegate(this,function() {
                    return this.options.colors[4];
            }),
                yellow: $dj.delegate(this,function() {
                    return this.options.colors[5];
            }),
            grey: $dj.delegate(this,function() {
                    return "#CCCCCC";
            }),
            siteBackground: function () {
                return "3C3C3C";
            },
            brighten: function(color, brightness) {
                Highcharts.Color(color).brighten(brightness).get('rgb');
            }
        };
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

DJ.UI.ThemeManager.instance = null;


// Declare this class as a jQuery plugin
$.plugin('dj_ThemeManager', DJ.UI.ThemeManager);

