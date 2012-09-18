/*!
 * ConcurrentVisitsModel
 */

DJ.UI.ConcurrentVisits = DJ.UI.CompositeComponent.extend({

    selectors: {
        visitorsGauge: '.visitorsGauge',
        timeCounter: '.dj_DashGaugeChartFooter .time'
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
                footer: "0:0m",
                gaugeType: 0,
                height: 200,
                width: 200
            },
            templates: {
                max: this._maxTemplate,
                min: this._minTemplate,
                footer: this._footerTemplate
            },
            data: 0
        };
    },
    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
                updateStats: $dj.delegate(this, this._updateStats),
                updateDashboard: $dj.delegate(this, this._updateDashboard),
            });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._visitorsGauge = this.$element.find(this.selectors.visitorsGauge);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateStats);
        $dj.subscribe('data.DashboardStats', this._delegates.updateDashboard);
        
    },

    _updateStats: function (data) {
        if (this.visitorsGauge) {
            this.visitorsGauge.setData(data.visits);
        }

        var minutes = (data.engaged_time.avg / 60).toFixed(0);
        var seconds = (data.engaged_time.avg % 60).toFixed(0);
        this.$element.find(this.selectors.timeCounter).html( minutes + ":" + (seconds <10 ? "0" + seconds: seconds) + "m");
    },
    
    _updateDashboard: function (data) {
        if (this.visitorsGauge) {
            this.visitorsGauge.updateMax(data.people_max);
            this.visitorsGauge.updateMin(data.people_min);
        }
    },
    
    _maxTemplate: function (val) {
        return "30-Day Max <span class=\"chartMax\">" + val + "</span>"; 
    },
    
    _minTemplate: function (val) {
        return "30-Day Min <span class=\"chartMin\">" + val + "</span>";
    },
    
    _footerTemplate: function (val) {
        return "<span class=\"label\">Engaged for <span class=\"time\">" + val + "</span> on average";
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_ConcurrentVisits', DJ.UI.ConcurrentVisits);