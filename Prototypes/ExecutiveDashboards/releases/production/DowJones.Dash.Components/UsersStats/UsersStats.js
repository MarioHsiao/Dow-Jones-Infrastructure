/*!
 * UsersStatsModel
 */

DJ.UI.UsersStats = DJ.UI.CompositeComponent.extend({

    selectors: {
        percentEngaged: '.counter'
    },
    
    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "UsersStats" }, meta));
    },
    
    _initGauge: function () {
        var self = this;
        this.$element.find('.tip').tooltip();
    },
    
    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            updateStats: $dj.delegate(this, this._updateStats)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this.counter = this.$element.find(this.selectors.percentEngaged);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateStats);
    },
    
    _updateStats: function (data) {
        if (!this.counter) {
            return;
        }

        var stats = {
            read: data.read || 0,
            write: data.write || 0,
            idle: data.idle || 0
        };
        var total = stats.read + stats.write + stats.idle || 100; 
        var percentEngaged = ((stats.read + stats.write) * 100) / total;
        this.counter.html(percentEngaged.toFixed(2) + "%");
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_UsersStats', DJ.UI.UsersStats);