/*!
 * ConcurrentVisitsModel
 */

DJ.UI.ConcurrentVisits = DJ.UI.CompositeComponent.extend({

    selectors: {
        visitorsGauge: '.visitorsGauge',
    },
    
    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "ConcurrentVisits" }, meta));
        this._initGauge();
    },
    
    _initGauge: function () {
        var self = this;
        DJ.add('DashGauge', this._visitorGaugeConfig()).done(function (comp) {
            self.visitorsGauge = comp;
            comp.owner = self;
            comp.updateMax(91995);
        });
    },
    
    _visitorGaugeConfig: function() {
        return {
            container: this._visitorsGauge[0],
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
        };
    },
    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
                updateStats: $dj.delegate(this, this._updateStats),
                updateGaugeMax: $dj.delegate(this, this._updateGaugeMax),
            });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._visitorsGauge = this.$element.find(this.selectors.visitorsGauge);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateStats);
        $dj.subscribe('data.HistorialTrafficStats', this._delegates.updateGaugeMax);
        
    },

    _updateStats: function (data) {
        if (this.visitorsGauge) {
            this.visitorsGauge.setData(data.visits);
        }
    },
    
    _updateGaugeMax: function (data) {
        var max = data.data['online.wsj.com'].people.max;
        if (this.visitorsGauge) {
            this.visitorsGauge.updateMax(max);
        }
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_ConcurrentVisits', DJ.UI.ConcurrentVisits);