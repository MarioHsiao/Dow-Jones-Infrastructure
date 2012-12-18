/*!
 * PageLoadByRegionCanvasModule
 */

DJ.UI.PageLoadByRegionCanvasModule = DJ.UI.AbstractCanvasModule.extend({

    selectors: {
        statsMap: '.dj_StatsMap'    
    },
    

    init: function (element, meta) {
        var $meta = $.extend({ name: "PageLoadByRegionCanvasModule" }, meta);

        // Call the base constructor
        this._super(element, $meta);

    },
    

    _initializeElements: function (ctx) {
        this._super(ctx);
        this.$statsMap = ctx.find(this.selectors.statsMap);
        this.statsMap = this.$statsMap.findComponent(DJ.UI.StatsMap);
    },
    

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData),
            domainChanged: $dj.delegate(this, this._domainChanged)
        });
    },
    

    _initializeEventHandlers: function () {
        if (this.options.mapType !== 'world') {
            $dj.subscribe('data.PageLoadDetailsByType', this._delegates.setData);
        }
        else {
            $dj.subscribe('data.PageLoadDetailsByTypeForWorld', this._delegates.setData);
        }

        $dj.subscribe('data.BasicHostConfiguration', this._delegates.domainChanged);
    },
    

    _domainChanged: function (data) {
        if (!data)
            return;

        if (this.options.mapType !== 'world') {
            this.statsMap._initializeMapData(data.map);
        }

        this.statsMap._initializeChart(data.performanceZones);

        //this._showContent();

        this.activePillId = null;
    },

    setData: function (data) {

        this.statsMap.setData(data);
        this.showContentArea();
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_PageLoadByRegionCanvasModule', DJ.UI.PageLoadByRegionCanvasModule);

$dj.debug('Registered DJ.UI.PageLoadByRegionCanvasModule as dj_PageLoadByRegionCanvasModule');
