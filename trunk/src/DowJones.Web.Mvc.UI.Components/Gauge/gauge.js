
DJ.UI.Gauge = DJ.UI.Component.extend({

    defaults: {
        debug: false,
        min: 0,
        max: 100,
        angle: 65,
        center: ["50%","85%"],
        height: 150,
        width: 200,
        cssClass: 'dj_Gauge',
        orientation: 'horizontal',
        dial:{
            radius: '95%',
            backgroundColor: '#333',
            borderColor: '#333',
            rearLength: 0,
            baseWidth: 3,
            borderWidth: 0,
            topWidth: 1,
            baseLength: '70%', // of radius
        },
        spedometerBands :[{
            from: 0,
            to: 6000,
            color: '#99CC00',
            innerRadius: '45%',
            outerRadius: '90%'
        }],
        monitoringBands: [{
            from: 0,
            to: 6000,
            color: '#99CC00',
            innerRadius: '45%',
            outerRadius: '90%'
        },
        {
            from: 0,
            to: 6000,
            color: '#99CC00',
            innerRadius: '45%',
            outerRadius: '90%'
        },
        {
            from: 0,
            to: 6000,
            color: '#99CC00',
            innerRadius: '45%',
            outerRadius: '90%'
        }]
    },

    selectors: {
        chartContainer: ".dj_GaugeChartContainer",
        chartTitle: ".dj_GaugeChartTitle",
        chartValue: ".dj_GaugeChartValue",
        chartFooter: ".dj_GaugeChartFooter"
    },
    
    events: {
       
    },

    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "Gauge" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        this.gaugeConfig = this.getGaugeConfig();
        this.bindOnSuccess();
    },

    _initializeElements: function (ctx) {
        //Bind the layout template
        $(this.$element).html(this.templates.layout({value: this.data}));
    },

    /* Public methods */
    formatNumber: function (n, separator) {
        separator = separator || ",";

        n = n.toString()
            .split("").reverse().join("")
            .replace(/(\d{3})/g, "$1" + separator)
            .split("").reverse().join("");

        // Strings that have a length that is a multiple of 3 will have a leading separator
        return n[0] == separator ? n.substr(1) : n;
    },


    // Bind the data to the component on Success
    bindOnSuccess: function () {
        if (!this.data) {
            $dj.warn("bindOnSuccess:: called with empty data object");
            return;
        }
        
        if (!this.chart) {
            this._renderGauge(this._getChartObject(this.data));
        } else {
            this._updateGauge();
        }
    },
    
    //Function to Set Data
    setData: function (gaugeData) {
        this.data = gaugeData;
        this.bindOnSuccess();
    },
    
 

    /* Private methods */

    //Get Gauge Object
    _getChartObject: function (value) {
        return $.extend(true,{
            chart: { renderTo: $(this.selectors.chartContainer, this.$element)[0] },
            series: [{
                data: [value],
                yAxis: 0
            }]
        }, this.gaugeConfig);
    },

    //Render Gauge
    _renderGauge: function (chartObj) {
        this.chart = new Highcharts.Chart(chartObj);
    },
    
    _updateGauge: function() {
        var point = this.chart.series[0].points[0];
        point.update(this.data, true);
    },

    //Initialize Delegates
    _initializeDelegates: function () {
      
    },
    
    getGaugeConfig: function () {
        //BEGIN: Discovery Graph Configuration
        return {
            chart: {
                type: 'gauge',
                plotBorderWidth: 1,
                plotBackgroundImage: null,
                height: this.options.height,
                width: this.options.width
            },

            title: {
                text: ''
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
                plotBands: this.options.bands ,
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
                    pivot: {
                        radius: 0
                    },
                    states: {
                        hover: { enabled: false }
                    }
                }
            },
            tooltip: {
                enabled: false
            },
            credits: false,
        };//END: Gauge Graph Configuration
    },
    
    EOF: null // Final property placeholder (without a comma) to allow easier moving of functions
});

// Declare this class as a jQuery plugin
$.plugin('dj_Gauge', DJ.UI.Gauge);

