/*!
 * BrowserShare
 */

DJ.UI.BrowserShare = DJ.UI.Component.extend({

    selectors: {
        shareChartContainer: '.shareChartContainer'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "BrowserShare" }, meta));

        this._initShareChart();
    },


    _initShareChart: function () {
        var self = this;
        DJ.add('ShareChart', {
            container: this._shareChartContainer[0],
        }).done(function (comp) {
            self.shareChart = comp;
            comp.setOwner(self);
        });
    },


    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._shareChartContainer = this.$element.find(this.selectors.shareChartContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.DeviceTrafficByPage', this._delegates.setData);
    },
    
    _setData: function (data) {
        if (!this.shareChart) {
            $dj.error("ShareChart Component is not initialized. Refresh the page to try again.");
            return;
        }

        data = data[0];
        var total = parseInt(data['All'], 10);
        var browserData = [];
        for(var key in data) {
            if (key !== 'page_name' && key !== 'All')
                browserData.push([key, parseFloat((parseInt(data[key], 10)/total * 100).toFixed(2))]);
        }
        
        this.shareChart.setData({ browserData: browserData });
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_BrowserShare', DJ.UI.BrowserShare);
