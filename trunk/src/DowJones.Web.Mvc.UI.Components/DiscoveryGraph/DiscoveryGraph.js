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
    },

    /*
    * Public methods
    */

    // Bind the data to the component on Success
    bindOnSuccess: function (data) {
        var discoveryGraphMarkup;
        this.$element.html("");
        if (data && data.discovery) {
            var discoveryEntityCount = _.size(data.discovery);
            discoveryGraphMarkup = this.templates.success({ discovery: data.discovery, entityLength: discoveryEntityCount });
            this.$element.append(discoveryGraphMarkup);

            // bind events and perform other wiring up
            this._initializeDiscoveryGraph(data.discovery);
            
            if (this.options.orientation === 'horizontal' && this.options.scrollable) {
                this.scrollable();
            }
            
            if (this.options.orientation === 'vertical' && this.options.sortable) {
                this.sortable();
            }
        }
    },

    //Function to Set Data
    setData: function (discoveryData) {
        this.data = discoveryData;
        this.bindOnSuccess(discoveryData);
    },

    //Initialize Scrollable
    scrollable: function () {
        // sanity check
        if (!this.options.orientation === 'horizontal' || !this.options.scrollable) {
            $dj.info('scrollable: Orientation is not horizontal or Scroll is disabled. No action taken.');
            return;
        }
        
        var $scrollTarget = this.$element.find('.dj_discoveryGraph_page_wrap');


        $scrollTarget
            .before('<a class="prev browse left scrollableArtifact"></a>')     /* "previous page" action */
            .after('<a class="next browse right scrollableArtifact"></a>')     /* "next page" action */
            .addClass("items")                              /* root element for the items */
            .wrap('<div class="scrollable scrollableArtifact"></div>');        /* root element for scrollable */

        this.$element
            .find('.scrollable')
            .scrollable()                                   /* initialize scrollable */
            .navigator('.navi');                            /* browser back handling */

    },


    sortable: function () {
        // sanity check
        if (!this.options.orientation === 'vertical' || !this.options.sortable) {
            $dj.info('sortable: Orientation is not vertical or Sorting is disabled. No action taken.');
            return;
        }
        
        // TODO: Reconsider this. Maybe build a flat list and add pagin during scrollable as that's when its really needed
        this.$element.find('.page-item').unwrap();
        
        var $sortableTarget = this.$element.find('.dj_discoveryGraph_page_wrap');
        
        $sortableTarget.sortable({ containment: 'parent', items: 'page-item' });
        

    },
    
    // resets styles and plugins attached
    reset: function () {
        var $scrollTarget = this.$element.find('.dj_discoveryGraph_page_wrap');

        if ($scrollTarget.parent('.scrollable').length) {
            $scrollTarget
                .removeClass('items')
                .unwrap()
                .siblings('.scrollableArtifact')
                .remove();
        }
    },

        /*
    * Private methods
    */
    //Initialize Discovery Graph
    _initializeDiscoveryGraph: function (data) {
        
        var $this = this;
        var index = 0;
        var discoveryEntityObj = data;
        var discoveryData = {};

        //Build the Discovery Entity Lists
        $.each(discoveryEntityObj, function (key, val) {
            var categoryArr = [];
            var seriesDataArr = [];
            discoveryEntityObjArr = val;
        
            //Construct the graph for the discovery entity objects
            $.each(discoveryEntityObjArr, function (idx, val) {
                var dataObj = {};
                dataObj.y = val.currentTimeFrameNewsVolume.value;
                dataObj.jsonObj = val;
                seriesDataArr[seriesDataArr.length] = dataObj;
                categoryArr[categoryArr.length] = val.descriptor;
            });
            discoveryData.title = $this._processChartTitle(key);
            discoveryData.categories = categoryArr;
            discoveryData.seriesData = seriesDataArr;
            $this._renderDiscoveryGraph(discoveryData, index);
            index++;
        });
    },

    //Render DiscoveryGraph
    _renderDiscoveryGraph: function (discoveryData, idx) {
        var self = this;
        var hasSVG = false;
        var data = discoveryData;

        //BEGIN: Discovery Graph Configuration
        var discoveryGraphConfig = {
            chart: {
                defaultSeriesType: 'bar',
                marginTop: 18,
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
                    groupPadding: 0,
                    borderWidth: 0,
                    stacking: 'percent',
                    shadow: false,
                    marker: {
                        enabled: false
                    }
                }
            }

        };
        //END: Discovery Graph Configuration

        //BEGIN: Discovery Graph Title
        var discoveryGraphTitle = function (chart) { // on complete of chart loading
            var title = discoveryData.title;
            var chartTitle = chart.renderer.text(title, 0, 12);
            chartTitle
        .attr({
            align: 'left'
        })
        .css({
            color: '#3399CC',
            fontSize: '11px',
            fontWeight: 'bold',
            fontFamily: 'arial, helvetica, clean, sans-serif' // default font
        })
        .add();
        }
        //END: Discovery Graph Title

        //$(function () {
        /* --->>> BEGIN: Discovery Graph Initialization <<<--- */
    $('.dj_discoveryGraph-item-' + idx, '.dj_discoveryGraph_page_wrap').each(function () {
            var $chartWrap = $(this);
            $('.dj_discovery-graph', $chartWrap).each(function () {
                var itemCount = $(this).attr('items') || 5,
				seriesData = function () {
				    var temp = discoveryData.seriesData;
				    var returnData = [[], temp];
				    for (var i = 0; i < temp.length; i++) {
				        returnData[0][i] = returnData[1][0].y - returnData[1][i].y;
				    }
				    return returnData;
				} (),
				seriesConfig = [{
				    name: 'faux',
				    data: seriesData[0],
				    color: '#f0f0f0',
				    dataLabels: {
				        enabled: false
				    }
				}, {
				    data: seriesData[1]
				}];

                $(this).data('chart', new Highcharts.Chart($.extend(true, {}, discoveryGraphConfig, {
                    chart: {
                        renderTo: $(this)[0],
                        height: 10 + (20 * itemCount) + 10
                    },
                    xAxis: {
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
                        categories: discoveryData.categories
                    },
                    plotOptions: {
                        bar: {
                            pointWidth: 12,
                            pointPadding: 0,
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
                                y: (hasSVG) ? 4 : 4
                            },
                            events: {
                                click: self._delegates.OndiscoveryItemClicked
                            }
                        }
                    },
                    series: seriesConfig

                }), function (chart) {
                    discoveryGraphTitle(chart);
                })
			);
            });
        });
        /* --->>> END: Discovery Graph Initialization <<<--- */
    },

    //Process Chart Title
    _processChartTitle: function (title) {
        //Map title to the entity names
        switch (title.toLowerCase()) {
            case "companynewsentities":
                title = "Companies";
                break;
            case "industrynewsentities":
                title = "Industries";
                break;
            case "personnewsentities":
                title = "Persons";
                break;
            case "regionnewsentities":
                title = "Regions";
                break;
            case "subjectnewsentities":
                title = "Subjects";
                break;
        }
        return title;
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
        $dj.debug(self.eventNames.discoveryItemClicked + " Event clicked");
        self.publish(self.eventNames.discoveryItemClicked, { "data": evt.point.options.jsonObj });
    }

});

// Declare this class as a jQuery plugin
$.plugin('dj_DiscoveryGraph', DJ.UI.DiscoveryGraph);

