/*!
 * UsersStatsModel
 */

DJ.UI.UsersStats = DJ.UI.CompositeComponent.extend({

    selectors: {
        newGauge: '.newGauge',
        readGauge: '.readGauge',
        writeGauge: '.writeGauge',
        idleGauge: '.idleGauge'
    },
    
    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "UsersStats" }, meta));
        this._initGauge();
    },
    
    _initGauge: function () {
        var self = this;
        
        DJ.add('DashGauge', this._basicGaugeConfig(this._newGauge[0], "New")).done(function (comp) {
            self.newGauge = comp;
            comp.owner = self;
            comp.updateMax(12000);
        });
        
        DJ.add('DashGauge', this._basicGaugeConfig(this._readGauge[0], "Read")).done(function (comp) {
            self.readGauge = comp;
            comp.owner = self;
            comp.updateMax(12000);
        });
        
        DJ.add('DashGauge', this._basicGaugeConfig(this._writeGauge[0], "Write")).done(function (comp) {
            self.writeGauge = comp;
            comp.owner = self;
            comp.updateMax(1000);
        });
        
        DJ.add('DashGauge', this._basicGaugeConfig(this._idleGauge[0], "Idle")).done(function (comp) {
            self.idleGauge = comp;
            comp.owner = self;
            comp.updateMax(91995);
        });
    },
    
    _basicGaugeConfig: function(element, vFooter) {
        return {
            container: element,
            options: {
                max: 100,
                min: 0,
                angle: 65,
                footer: vFooter,
                gaugeType: 0,
                height: 100,
                width: 100
            },
            data: 0
        };
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
                updateStats: $dj.delegate(this, this._updateStats)
            });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._newGauge = this.$element.find(this.selectors.newGauge);
        this._readGauge = this.$element.find(this.selectors.readGauge);
        this._writeGauge = this.$element.find(this.selectors.writeGauge);
        this._idleGauge = this.$element.find(this.selectors.idleGauge);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateStats);
        
    },

    _updateStats: function (data) {
        this.newGauge.setData(data.new);
        this.readGauge.setData(data.read);
        this.writeGauge.setData(data.write);
        this.idleGauge.setData(data.idle);
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_UsersStats', DJ.UI.UsersStats);