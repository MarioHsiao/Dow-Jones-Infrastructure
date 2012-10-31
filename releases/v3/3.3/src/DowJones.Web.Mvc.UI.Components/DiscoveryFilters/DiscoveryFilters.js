/*!
* DiscoveryFilters
*   e.g. , "this._imageSize" is generated automatically.
*
*   
*  Getters and Setters are generated automatically for every Client Property during init;
*   e.g. if you have a Client Property called "imageSize" on server side code
*        get_imageSize() and set_imageSize() will be generated during init.
*  
*  These can be overriden by defining your own implementation in the script. 
*  You'd normally override the base implementation if you have extra logic in your getter/setter 
*  such as calling another function or validating some params.
*
*/

DJ.UI.DiscoveryFilters = DJ.UI.Component.extend({

    /*
    * Properties
    */

    //Selectors
    selectors: {
        discoveryFiltersList: 'ul.dj_discoveryFilters-list',
        export: 'span.dj_df-export a.export',
        expandBtn: '.dj_df-div .dj_df-expand',
        expandDiv: 'div.dj_df-div div.cd_div_expand',
        collapseDiv: 'div.dj_df-div div.cd_div_collapse'
    },

    // Default options
    defaults: {
        debug: false,
        cssClass: 'DiscoveryFilters',
        sortable: true
        // ,name: value     // add more defaults here separated by comma
    },

    //Events
    events: {
        itemClick: 'itemClick.dj.DiscoveryFilters',
        exportClick: 'exportClick.dj.DiscoveryFilters'
    },

    // Localization/Templating tokens
    tokens: {
        // name: value     // add more defaults here separated by comma
    },

    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "DiscoveryFilters" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        this.discoveryFitlersConfig = this._getDiscoveryFiltersConfig();
        this.bindOnSuccess();

        //Initialize Sortable
        this._initializeSortable();

        //Initialize Expand/Collapse
        this._initializeExpandCollapse();
    },

    /*
    * Public methods
    */

    // Bind the data to the component on Success
    bindOnSuccess: function (data) {
        /*if (!data) {
        $dj.warn("bindOnSuccess:: called with empty data object");
        return;
        }*/

        $(this.$element).html(this.templates.success);

        // bind events and perform other wiring up
        this._initializeDiscoveryFilters(data);
    },

    /*
    * Private methods
    */

    _initializeElements: function (ctx) {

    },

    _initializeEventHandlers: function () {

    },

    //Initialize Sortable
    _initializeSortable: function () {
        if (this._isSortable) {
            $(this.selectors.discoveryFiltersList).sortable({
                placeholder: "ui-state-highlight"
            }).disableSelection();
        }
    },

    //Initialize Expand/Collapse Discovery
    _initializeExpandCollapse: function () {
        var self = this;
        $(this.selectors.expandBtn).bind('click', function () {
            self._expandCollapse(this, false); return false;
        });
    },

    //Expand/Collapse function
    _expandCollapse: function (el, expand) {
        var self = this;
        if (expand) {
            $(el).removeClass('cd_collapse').addClass('dj_df-expand').unbind('click').bind('click', function () {
                self._expandCollapse(this, false); return false;
            });
            $(this.selectors.collapseDiv).removeClass('cd_div_collapse').addClass('cd_div_expand');
            $(this.selectors.export).show();
        } else {
            $(el).removeClass('dj_df-expand').addClass('cd_collapse').unbind('click').bind('click', function () {
                self._expandCollapse(this, true); return false;
            });
            $(this.selectors.expandDiv).removeClass('cd_div_expand').addClass('cd_div_collapse');
            $(this.selectors.export).hide();
        }
    },

    //Check if sortable is enabled
    _isSortable: function () {
        return this.options.sortable;
    },

    //Initialize Delegates
    _initializeDelegates: function () {
        $.extend(this._delegates, {
            OnItemClicked: $dj.delegate(this, this._onItemClicked),
            OnExportClicked: $dj.delegate(this.selectors.export, 'click', this._onExportClicked)
        });
    },

    //Initialize Discovery Filters
    _initializeDiscoveryFilters: function (discoveryEntityObj) {
        var $this = this;
        this._renderDiscoveryFilters();
    },

    //On Discovery Item Click Event Handler
    _onItemClicked: function (evt) {
        var self = this;
        $dj.info(self.events.itemClick + " Event clicked");
        self.publish(self.events.itemClick, { "data": evt.point.options.jsonObj });
    },

    //On Discovery Filters Export Click
    _onExportClicked: function (evt) {
        var self = this;
        $dj.info(self.events.exportClick + " Event clicked");
        self.publish(self.events.exportClick, { "data": "test export" });
    },

    _getDiscoveryFiltersConfig: function () {
        //BEGIN: Discovery Filters Configuration
        return {
            chart: { type: 'bar' },
            title: { text: null },
            subtitle: { text: null },
            xAxis: {
                lineWidth: 0,
                labels: { align: 'left', x: 0, y: 7 },
                tickWidth: 0,
                title: { text: null }
            },
            yAxis: {
                labels: { enabled: false },
                gridLineWidth: 0,
                min: 0,
                title: { text: null }
            },
            plotOptions: {
                series: {
                    color: '#0bb6e4',
                    cursor: 'pointer'
                },
                bar: {
                    dataLabels: { enabled: true },
                    pointWidth: 5,
                    groupPadding: 0,
                    borderWidth: 0,
                    pointPadding: 0,
                    shadow: false
                }
            },
            legend: { enabled: false },
            credits: { enabled: false },
            exporting: { enabled: false }
        };
        //END: Discovery Filters Configuration
    },

    //Render DiscoveryGraph
    _renderDiscoveryFilters: function (discoveryData, idx) {
        return new Highcharts.Chart($.extend(true, {}, this.discoveryFitlersConfig, {
            chart: {
                renderTo: 'hc-container'
            },

            xAxis: {
                categories: ['Microsoft', 'Apple Inc', 'IBM', 'Amazon', 'Samsung', 'Salesforce', 'Google', 'Netflix']
            },
            tooltip: {
                enabled: false,
                formatter: function () {
                    return '' +
                        this.series.name + ': ' + this.y + ' millions';
                }
            },
            series: [{
                name: null,
                data: [{ y: 547, color: 'transparent', dataLabels: { x: -25, y: 1} },
                       { y: 547, color: 'transparent', dataLabels: { x: -25, y: 1, formatter: function () {
                           return 408;
                       }
                       }
                       },
                       { y: 547, color: 'transparent', dataLabels: { x: -25, y: 1, formatter: function () {
                           return 156;
                       }
                       }
                       },
                       { y: 547, color: 'transparent', dataLabels: { x: -25, y: 1, formatter: function () {
                           return 133;
                       }
                       }
                       },
                       { y: 547, color: 'transparent', dataLabels: { x: -25, y: 1, formatter: function () {
                           return 96;
                       }
                       }
                       },
                       { y: 547, color: 'transparent', dataLabels: { x: -25, y: 1, formatter: function () {
                           return 80;
                       }
                       }
                       },
                       { y: 547, color: 'transparent', dataLabels: { x: -25, y: 1, formatter: function () {
                           return 66;
                       }
                       }
                       },
                       { y: 547, color: 'transparent', dataLabels: { x: -25, y: 1, formatter: function () {
                           return 46;
                       }
                       }
                       }]
            }, {
                name: null,
                data: [{
                    dataLabels: {
                        style: {
                            display: 'none'
                        }
                    },
                    y: 547
                }, {
                    dataLabels: {
                        style: {
                            display: 'none'
                        }
                    },
                    y: 408
                }, {
                    dataLabels: {
                        style: {
                            display: 'none'
                        }
                    },
                    y: 156
                }, {
                    dataLabels: {
                        style: {
                            display: 'none'
                        }
                    },
                    y: 133
                }, {
                    dataLabels: {
                        style: {
                            display: 'none'
                        }
                    },
                    y: 96
                }, {
                    dataLabels: {
                        style: {
                            display: 'none'
                        }
                    },
                    y: 80
                }, {
                    dataLabels: {
                        style: {
                            display: 'none'
                        }
                    },
                    y: 66
                }, {
                    dataLabels: {
                        style: {
                            display: 'none'
                        }
                    },
                    y: 46
                }]
            }]
        }));
    }

});


// Declare this class as a jQuery plugin
$.plugin('dj_DiscoveryFilters', DJ.UI.DiscoveryFilters);