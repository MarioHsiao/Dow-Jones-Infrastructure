/*!
* DiscoveryGraph
*/

DJ.UI.DiscoveryGraph = DJ.UI.Component.extend({

    defaults: {
        debug: false,
        cssClass: 'dj_DiscoveryGraph',
        orientation: 'horizontal',
        scrollable: true,
        sortable: false
    },


    eventNames: {
        discoveryItemClicked: 'discoveryItemClick.dj.DiscoveryGraph'
    },

    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "DiscoveryGraph" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        this.discoveryGraphConfig = this.getDiscoveryGraphConfig();
    },
    
    _initializeElements: function (ctx) {      
        this.$viewWrapper = ctx.find('.dj_view_wrapper');
    
    },


    /* Public methods */


    // Bind the data to the component on Success
    bindOnSuccess: function (data) {
        if (!data || !data.discovery) {
            $dj.warn("bindOnSuccess:: called with empty data object");
            return;
        }

        var discoveryGraphMarkup = this.templates.success({ discovery: data.discovery });
        this.$element.html("").append(discoveryGraphMarkup);

        // bind events and perform other wiring up
        this._initializeDiscoveryGraph(data.discovery);

        if (this.options.orientation === 'horizontal' && this.options.scrollable) {
            this.scrollable();
        }

        if (this.options.orientation === 'vertical' && this.options.sortable) {
            this.sortable();
        }

    },

    //Function to Set Data
    setData: function (discoveryData) {
        this.data = discoveryData;
        this.bindOnSuccess(discoveryData);
    },

    //Initialize Scrollable
    scrollable: function () {
        this.options.orientation = 'horizontal';
        this.$viewWrapper.removeClass('dj_widget_slimView').addClass('dj_widget_fullView');
        // sanity check
        if (!this.options.scrollable) {
            $dj.info('scrollable:: Scrollable is disabled. No action taken.');
            return;
        }

        var $scrollTarget = this.$element.find('.dj_discoveryGraph_item_wrap');

        $scrollTarget
            .wrapInner('<div class="scrollable scrollableArtifact"><div class="items"></div></div>');        /* root element for scrollable */

        this.$element
            .find('.scrollable')
            .before('<a class="prev browse left scrollableArtifact"></a>')     /* "previous page" action */
            .after('<a class="next browse right scrollableArtifact"></a>')     /* "next page" action */
            .scrollable();                            /* browser back handling */
    },


    sortable: function () {
        this.options.orientation = 'vertical';
        this.$viewWrapper.removeClass('dj_widget_fullView').addClass('dj_widget_slimView');
        // sanity check
        if (!this.options.sortable) {
            $dj.info('sortable:: Sorting is disabled. No action taken.');
            return;
        }

        var $sortableTarget = this.$element.find('.dj_discoveryGraph_item_wrap');

        $sortableTarget.sortable({ containment: 'parent', items: '.page-item' });
        this.$element.find('.page-item h3').click(function () {
            $(this).next().toggle('slow');
            return false;
        }).next().hide();


    },

    // resets styles and plugins attached
    reset: function () {

        if (this.options.orientation === 'horizontal') {
            this.$element
                .find('.page-item')
                .unwrap()           /* break out of items div */
                .unwrap()           /* break out of scrollable div */
                .siblings('.scrollableArtifact').remove();      /* remove previous next links */
        }
        else {
            this.$element.find('.dj_discoveryGraph_item_wrap')
                .removeClass('ui-sortable')
                .unbind()
                .sortable = null;
            this.$element.find('.page-item h3').unbind('click');
            this.$element.find('.dj_discovery-graph').show();
        }
    },

    /* Private methods */

    //Initialize Discovery Graph
    _initializeDiscoveryGraph: function (discoveryEntityObj) {

        var $this = this,
            index = 0,
            discoveryData = {};

        //Build the Discovery Entity Lists
        _.each(discoveryEntityObj, function (entitiesObj) {
            var categoryArr = [],
                seriesDataArr = [],
                discoveryEntityObjArr = entitiesObj.entities;

            //Construct the graph for the discovery entity objects
            _.each(discoveryEntityObjArr, function (entity) {
                var dataObj = {};
                dataObj.y = entity.currentTimeFrameNewsVolume.value;
                dataObj.jsonObj = entity;
                seriesDataArr[seriesDataArr.length] = dataObj;
                categoryArr[categoryArr.length] = entity.descriptor;
            });
            //discoveryData.title = entitiesObj.title;
            discoveryData.categories = categoryArr;
            discoveryData.seriesData = seriesDataArr;
            $this._renderDiscoveryGraph(discoveryData, index);
            index++;
        });
    },


    _extractSeriesData: function (seriesData) {
        var returnData = [[], seriesData];
        for (var i = 0; i < seriesData.length; i++) {
            returnData[0][i] = returnData[1][0].y - returnData[1][i].y;
        }
        return returnData;
    },


    //Render DiscoveryGraph
    _renderDiscoveryGraph: function (discoveryData, idx) {
        var self = this,
            $chartContainer = this.$element.find('.dj_discoveryGraph-item-' + idx).find('.dj_discovery-graph'),
            itemCount = $chartContainer.data('items') || 5,
            seriesData = this._extractSeriesData(discoveryData.seriesData),
            chart;

        chart = new Highcharts.Chart($.extend(true, {}, this.discoveryGraphConfig, {
            chart: {
                renderTo: $chartContainer[0],
                height: 10 + (20 * itemCount) + 10
            },
            xAxis: {
                categories: discoveryData.categories
            },
            series: [{
                data: seriesData[0],
            }, {
                data: seriesData[1]
            }]

        }));
    },


    //Initialize Delegates
    _initializeDelegates: function () {
        $.extend(this._delegates, {
            OndiscoveryItemClicked: $dj.delegate(this, this._onDiscoveryItemClicked)
        });
    },

    //On Discovery Item Click Event Handler
    _onDiscoveryItemClicked: function (evt) {
        var self = this,
                o = self.options,
                el = $(self.element);
        $dj.info(self.eventNames.discoveryItemClicked + " Event clicked");
        self.publish(self.eventNames.discoveryItemClicked, { "data": evt.point.options.jsonObj });
    },


    getDiscoveryGraphConfig: function () {
        //BEGIN: Discovery Graph Configuration
        return {
            chart: {
                defaultSeriesType: 'bar',
                spacingRight: 20,
                spacingBottom: 18,
                spacingLeft: 120,
                style: {
                    fontFamily: 'arial, helvetica, clean, sans-serif' // default font
                }
            },

            legend: {
                enabled: false
            },

            tooltip: {
                enabled: false
            },

            credits: false,

            title: {
                text: null
            },

            subtitle: {
                text: null
            },

            xAxis: {
                title: {
                    text: null
                },
                labels: {
                    y: 3,
                    x: -120,
                    align: "left",
                    style: {
                        color: '#666666',
                        fontFamily: 'arial, helvetica, clean, sans-serif', // default font
                        fontSize: '11px',
                        width: '125px'
                    },
                    formatter: function () {
                        var name = this.value;
                        if (name.length > 20) {
                            name = name.substring(0, 18) + '...';
                        }
                        return name;
                    }
                },
                minorGridLineWidth: 0,
                tickWidth: 0,
                lineWidth: 0
            },

            yAxis: {
                title: {
                    text: null
                },
                gridLineWidth: 0,
                lineWidth: 0,
                tickWidth: 0,
                labels: {
                    enabled: false
                }
            },

            plotOptions: {
                series: {
                    shadow: false,
                    color: '#4da7cc',
                    pointPadding: 0,
                    groupPadding: 0.1,
                    cursor: 'pointer'
                },
                bar: {
                    pointPadding: 0,
                    pointWidth: 12,
                    groupPadding: 0,
                    borderWidth: 0,
                    stacking: 'percent',
                    shadow: false,
                    marker: {
                        enabled: false
                    },
                    dataLabels: {
                        enabled: true,
                        align: 'right',
                        color: '#ffffff',
                        style: {
                            fontFamily: 'arial, helvetica, clean, sans-serif', // default font
                            fontSize: '10px',
                            fontWeight: 'bold'
                        },
                        x: -3,
                        y: 4
                    },
                    events: {
                        click: this._delegates.OndiscoveryItemClicked
                    }
                }
            },

            series: [
                {
                    name: 'faux',
                    color: '#f0f0f0',
                    dataLabels: {
                        enabled: false
                    }
                }
            ]

        };
        //END: Discovery Graph Configuration
    }

});

// Declare this class as a jQuery plugin
$.plugin('dj_DiscoveryGraph', DJ.UI.DiscoveryGraph);

