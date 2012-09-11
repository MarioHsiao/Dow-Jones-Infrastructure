/*!
 * ConcurrentVisitsModel
 */

DJ.UI.ConcurrentVisits = DJ.UI.CompositeComponent.extend({

    selectors: {
        gaugeContainer: '.gaugeContainer'
    },
    
    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "ConcurrentVisits" }, meta));

        this._initGauge();
    },
    
    _initGauge: function () {
        var self = this;
        DJ.add('Gauge', {
            container: this._gaugeContainer[0],
            options: {
                max: 65000,
                min: 0,
                angle: 65,
                footer: "",
                gaugeType: 0,
                height: 200,
                width: 200
            },
            data: 90
        }).done(function (comp) {
            self.gauge = comp;
            comp.owner = self;
        });
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._gaugeContainer = this.$element.find(this.selectors.gaugeContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.setData);
    },

    _setData: function (data) {
        if (this.gauge) {
            console.log(data.visits);
            this.gauge.setData(data.visits + Math.floor(Math.random()*5001));
        }
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_ConcurrentVisits', DJ.UI.ConcurrentVisits);
