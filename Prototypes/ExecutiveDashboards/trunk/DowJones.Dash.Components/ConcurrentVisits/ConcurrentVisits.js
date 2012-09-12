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
        DJ.add('DashGauge', {
            container: this._gaugeContainer[0],
            options: {
                max: 100,
                min: 0,
                angle: 65,
                footer: "",
                gaugeType: 0,
                height: 200,
                width: 200
            },
            data: 0
        }).done(function (comp) {
            self.gauge = comp;
            comp.owner = self;
        });
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
                updateGauge: $dj.delegate(this, this._updateGauge),
                updateGaugeMax: $dj.delegate(this, this._updateGaugeMax),
            });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._gaugeContainer = this.$element.find(this.selectors.gaugeContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateGauge);
        $dj.subscribe('data.HistorialTrafficStats', this._delegates.updateGaugeMax);
        
    },

    _updateGauge: function (data) {
        if (this.gauge) {
            this.gauge.setData(data.visits);
        }
    },
    
    _updateGaugeMax: function (data) {
        var max = data.data['online.wsj.com'].people.max;
        if (this.gauge) {
            this.gauge.updateMax(max);
        }
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_ConcurrentVisits', DJ.UI.ConcurrentVisits);