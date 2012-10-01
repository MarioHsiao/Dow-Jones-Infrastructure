
DJ.UI.DashGauge = DJ.UI.Component.extend({

    defaults: {
        colors: Highcharts.getOptions().colors,
        debug: false,
        min: 0,
        max: 100,
        angle: 65,
        center: ["50%", "85%"],
        height: 150,
        width: 200,
        cssClass: 'dj_Gauge',
        orientation: 'horizontal',
        dial: {
            radius: '95%',
            backgroundColor: '#AEAEAE',
            borderColor: '#AEAEAE',
            rearLength: 0,
            baseWidth: 5,
            borderWidth: 0,
            topWidth: 1,
            baseLength: '70%', // of radius
        },
        pivot: {
            backgroundColor: '#AEAEAE',
            borderColor: '#AEAEAE',
            borderWidth: 0,
            radius: 2.5
        }
    },
    
    selectors: {
        chartContainer: ".dj_DashGaugeChartContainer",
        chartTitle: ".dj_DashGaugeChartTitle",
        chartValue: ".dj_DashGaugeChartValue",
        chartFooter: ".dj_DashGaugeChartFooter",
        chartMax: ".dj_DashGaugeChartMax",
        chartMin: ".dj_DashGaugeChartMin"
    },

    events: {

    },

    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "DashGauge" }, meta);

        // Call the base constructor
        this._super(element, $meta);
        this.bindOnSuccess();
    },

    _initializeElements: function (ctx) {
        //Bind the layout template
        $(this.$element).html(this.templates.layout({
            value: this.data,
            title: this.options.title,
            footer: this.options.footer,
            max: this.options.max,
            min: this.options.min
        }));
    },

    /* Public methods */
    
    // Bind the data to the component on Success
    bindOnSuccess: function () {
        if (!this.chart) {
            this._renderGauge(this._getChartObject(this.data));
        } else {
            $(this.selectors.chartValue, this.$element).counter(this.data);
            this._updateGauge();
        }
    },

    //Function to Set Data
    setData: function (value) {
        this.data = value;
        if (this.options.gaugeType === 0 && this.chart) { // Is a speedometer
            var axis = this.chart.yAxis[0];
            if (axis) {
                var plotBand = this._getSpedometerBands()[1];
                if (this.data <= this.options.min) {
                    plotBand = $.extend(true, plotBand, { to: this.options.min });
                } else if (this.data > this.options.max) {
                    plotBand = $.extend(true, plotBand, { to: this.options.max });
                } else {
                    plotBand = $.extend(true, plotBand, { to: this.data });
                }
                axis.removePlotBand("speed1");
                axis.addPlotBand(plotBand);
            }
        }
        this.bindOnSuccess();
    },

    updateTitle: function (val) {
        this.options.title = val;
        $(this.selectors.chartTitle, this.$element).html(val);
    },
    
    updateMin: function(val) {
        if (this.options.min != val && this.chart) {

            this.options.min = val;
            var axis = this.chart.yAxis[0];

            if (this.options.gaugeType === 0) { // Is a speedometer

                if (axis) {
                    var plotBandSpeed0 = this._getSpedometerBands()[0];
                    var plotBandSpeed1 = this._getSpedometerBands()[1];
                    plotBandSpeed0 = $.extend(true, plotBandSpeed0, { to: this.options.max });
                    if (this.data < this.options.min) {
                        plotBandSpeed1 = $.extend(true, plotBandSpeed1, { to: this.options.min });
                    } else if (this.data > this.options.max) {
                        plotBandSpeed1 = $.extend(true, plotBandSpeed1, { to: this.options.max });
                    } else {
                        plotBandSpeed1 = $.extend(true, plotBandSpeed1, { to: this.data });
                    }

                    axis.removePlotBand("speed0");
                    axis.removePlotBand("speed1");
                    axis.addPlotBand(plotBandSpeed0);
                    axis.addPlotBand(plotBandSpeed1);
                }
            }
            axis.setExtremes(this.options.min, this.options.max, true);
            $(this.selectors.chartMin, this.$element).html(this.templates.min(Highcharts.numberFormat(val, 0)));
        }
    },

    updateMax: function(val) {
        if (this.options.max != val && this.chart) {

            this.options.max = val;
            var axis = this.chart.yAxis[0];
            
            if (this.options.gaugeType === 0) { // Is a speedometer
                
                if (axis) {
                    var plotBandSpeed0 = this._getSpedometerBands()[0];
                    var plotBandSpeed1 = this._getSpedometerBands()[1];
                    plotBandSpeed0 = $.extend(true, plotBandSpeed0, { to: this.options.max });
                    if (this.data < this.options.min) {
                        plotBandSpeed1 = $.extend(true, plotBandSpeed1, { to: this.options.min });
                    } else if (this.data > this.options.max) {
                        plotBandSpeed1 = $.extend(true, plotBandSpeed1, { to: this.options.max });
                    } else {
                        plotBandSpeed1 = $.extend(true, plotBandSpeed1, { to: this.data });
                    }

                    axis.removePlotBand("speed0");
                    axis.removePlotBand("speed1");
                    axis.addPlotBand(plotBandSpeed0);
                    axis.addPlotBand(plotBandSpeed1);
                }
            }
            axis.setExtremes(this.options.min, this.options.max, true);
            $(this.selectors.chartMax, this.$element).html(this.templates.max(Highcharts.numberFormat(val, 0)));
        }
    },
    
    updateFooter: function (val) {
        this.options.footer = val;
        $(this.selectors.chartFooter, this.$element).html(val);
    },


    /* Private methods */

    //Get Gauge Object
    _getChartObject: function (value) {
        return $.extend(true, {
            chart: { renderTo: $(this.selectors.chartContainer, this.$element)[0] },
            series: [{
                data: [value],
                yAxis: 0
            }]
        }, this.getGaugeConfig());
    },

    //Render Gauge
    _renderGauge: function (chartObj) {
        this.chart = new Highcharts.Chart(chartObj);
    },

    _updateGauge: function () {
        var point = this.chart.series[0].points[0];
        point.update(this.data, true, { easing: 'easeInOutElastic' }); // if the type is meter use animation.easeOutElastic
    },

    //Initialize Delegates
    _initializeDelegates: function () {

    },

    _getSpedometerBands: function () {
        return [{
            id: 'speed0',
            from: 0,
            to: this.options.max,
            color: '#CCC',
            innerRadius: '45%',
            outerRadius: '90%'
        }, {
            id: 'speed1',
            from: 0,
            to: this.data,
            color: this.options.colors[6],
            innerRadius: '45%',
            outerRadius: '90%'
        }];
    },

    _getMeterBands: function () {
        var max = this.options.max;
        var segment1 = (max * 80 / 100);
        var segment2 = (max * 90 / 100);
        return [{
            id: 'meter1',
            from: 0,
            to: segment1,
            color: this.options.colors[0],
            innerRadius: '45%',
            outerRadius: '90%'
        },
            {
                id: 'meter2',
                from: segment1,
                to: segment2,
                color: this.options.colors[1],
                innerRadius: '45%',
                outerRadius: '90%'
            },
            {
                id: 'meter3',
                from: segment2,
                to: max,
                color: this.options.colors[2],
                innerRadius: '45%',
                outerRadius: '90%'
            }];
    },

    getGaugeConfig: function () {
        //BEGIN: Discovery Graph Configuration
        return {
            chart: {
                type: 'gauge',
                plotBorderWidth: 0,
                backgroundColor: 'transparent',
                plotBackgroundImage: null,
                height: this.options.height,
                width: this.options.width,
                margin:[0,0,0,0]
            },

            title: {
                text: null
            },

            pane: [{
                startAngle: -1 * this.options.angle,
                endAngle: this.options.angle,
                background: null,
                center: this.options.center,
                size: this.options.height
            }],

            yAxis: [{
                min: this.options.min,
                max: this.options.max,
                minorTickPosition: 'outside',
                minorTickLength: 0,
                tickLength: 0,
                gridLineWidth: 0,
                lineWidth: 0,
                tickPosition: 'inside',
                labels: {
                    rotation: 'auto',
                    distance: 20,
                    enabled: false,
                },
                plotBands: (this.options.gaugeType === 0) ? this._getSpedometerBands() : this._getMeterBands(),
                pane: 0,
                title: {
                    text: '',
                    y: -40
                }
            }],

            plotOptions: {
                gauge: {
                    dataLabels: {
                        enabled: false
                    },
                    dial: this.options.dial,
                    pivot: this.options.pivot,
                    states: {
                        hover: { enabled: false }
                    }
                }
            },
            tooltip: {
                enabled: false
            },
            credits: false
        };//END: Gauge Graph Configuration
    },

    EOF: null // Final property placeholder (without a comma) to allow easier moving of functions
});

// Declare this class as a jQuery plugin
$.plugin('dj_Gauge', DJ.UI.DashGauge);