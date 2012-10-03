DJ.$dj.require(['DJ','$dj'], function (DJ,$dj) {
    DJ.UI.Dash = DJ.UI.Component.extend({
        
        init: function (el, meta) {
            this._super(el, meta);

            this._groups = new DJ.DashGroupManager();
            
            this._initializeModuleStyles();
            this._initializeModuleResizing();
            this._initializeTabs();
        },
        
        domain: function (domain) {
            this._groups.changeDomain(domain);
            return this;
        },

        start: function (domain) {
            this._groups.start(domain);
            return this;
        },
        
        stop: function () {
            this._groups.stop();
            return this;
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates || { }, {
                domain: $dj.delegate(this, this.domain),
                callModuleResize: $dj.delegate(this, this._callModuleResize)
            });
        },

        _initializeModuleResizing: function () {
            DJ.subscribe('resized.dj.CanvasModule', this._delegates.callModuleResize);
        },
        
        _callModuleResize: function (args) {
            var request = {
                pageId: DJ.UI.Canvas.find().get_canvasId(),
                moduleId: args.moduleId
            };
            $.ajax(this.options.moduleResizeUrl + args.newSize, { data: request });
        },

        _initializeModuleStyles: function () {
            $('.small.module').addClass('span2');
            $('.medium.module').addClass('span4');
            $('.large.module').addClass('span6');
            $('.x-large.module').addClass('span8');

            $('.dj_module').addClass('row-fluid');

            $('.dj_module .hide').click(function () {
                $(this).closest('.dj_module').findComponent().hide();
            });

            $('.minimize').append($('<i class="icon-chevron-up icon-white"/>'));
            $('.maximize').append($('<i class="icon-chevron-down icon-white"/>'));
            $('.dj_module .hide').append($('<i class="icon-remove icon-white"/>')).removeClass('hide');
            $('.dj_module-refresh').remove();

            this._initializeInfoIcons();
        },
        
        _initializeInfoIcons: function () {
            $('.dj_module-title')
                .after('<i class="icon-info-sign icon-white"/>');

            $('.dj_module-title + i.icon-info-sign')
                .each(function () {
                    var el = $(this),
                        tooltipAttached = el.data('tooltipAttached');
                    
                    if(!tooltipAttached) {
                        el.tooltip({
                            title: el.closest('.dj_module').data('description'),
                            delay: { show: 500, hide: 100 },
                            placement: 'bottom'
                        });
                        el.data('tooltipAttached', true);
                    }
                });
            
        },

        _initializeTabs: function () {
            var changeDomain = this._delegates.domain;

            var tabs = $('.trendTabs li').not('> a[href=#module-gallery]');
            tabs.click(function () {
                tabs.removeClass('active');
                $(this).addClass('active');
                var source = $(this).data('source');
                changeDomain(source);
            });
        },

        EOF: null
    });


    DJ.DashGroupManager = DJ.Component.extend({
        
        defaults: {
            domain: null,
            dataPrefix: 'data.',
            communicationPrefix: 'comm.'
        },
        
        init: function (meta) {
            this._super(meta);
            $.connection.dashboard.messageReceived = this._delegates.messageReceived;
        },

        changeDomain: function (domain) {
            var self = this,
                o = self.options,
                communicationPrefix = o.communicationPrefix,
                oldSources = this._getSources(o.domain);

            if (domain === o.domain)
                return;
            
            self.subscribedSources = this._getSources(domain);

            // fire a reset for the module to know we are recycling the view.
            DJ.publish(communicationPrefix + "domain.changed", { domain: domain });
            
            var handleMessage = this._delegates.messageReceived;
            if (oldSources) {
                $.connection.dashboard.unsubscribe(oldSources)
                    .done($dj.delegate(this, function() {
                        $.connection.dashboard.subscribe(self.subscribedSources)
                            .done(function (messages) {
                                o.domain = domain;
                                for (var i = 0; i < messages.length; i++) {
                                    handleMessage(messages[i]);
                                }
                            });
                    }));
            }
            else {
                $.connection.dashboard.subscribe(self.subscribedSources)
                    .done(function (messages) {
                        o.domain = domain;
                        for (var i = 0; i < messages.length; i++) {
                            handleMessage(messages[i]);
                        }
                    });
            }
        },

        start: function (domain) {
            if (this._started)
                return this;
            
            $.connection.hub.start()
                .done($dj.delegate(this, function () {
                    this._started = true;
                    this.changeDomain(domain);
                }));
            
            return this;
        },

        stop: function () {
            if (!this._started)
                return this;

            $.connection.dashboard.unsubscribe(this._currentSources);
            $.connection.hub.stop($dj.delegate(this, function () {
                this._started = false;
            }));
            
            return this;
        },


        _getSources: function (domain) {

            var chartBeatEvents = [
                'QuickStats',
                'DashboardStats',
                'HistorialTrafficSeries',
                'HistorialTrafficSeriesWeekAgo',
                'HistoricalTrafficStats',
                'HistoricalTrafficValues',
                'Referrers',
                'TopPages'
            ];
            
            var gomezEvents = [
                'BrowserStats',
                'DeviceTraffic',
                'DeviceTrafficByPage',
                'PageLoadHistoricalDetails',
                'PageTimings',
                'PageLoadDetailsBySubCountryforCountry'
            ];

            var events = [];
            
            // We've got ChartBeat for pretty much everything
            events = events.concat(chartBeatEvents);
            
            //if (domain == 'online.wsj.com') {
                events = events.concat(gomezEvents);
            //}

            // Convert to '[domain]-[event name]', e.g. 'online.wsj.com-BrowserStats'
            return _.map(events, function (name) { return domain + '-' + name; });
        },

        _initializeDelegates: function () {
            this._delegates = {
                messageReceived: $dj.delegate(this, this._messageReceived)
            };
        },

        _messageReceived: function (message) {
            var self = this,
                o = self.options,
                dataPrefix = o.dataPrefix;

            if (_.contains(self.subscribedSources, message.source)) {

                if (!message || message.error) {
                    dataPrefix = 'dataError.';
                    $dj.error('Data error: ' + message.error, message.data);
                }

                var name = dataPrefix + message.eventName;
                //console.log(name + message.data);
                DJ.publish(name, message.data);
            }
        },
        
        EOF: null,
    });

    (function() {
        window._alert = window.alert;
        window.alert = function (msg) {
            $('#dashAlertModal .modal-body p').text(msg);
            $('#dashAlertModal').modal('show');
            return false;
        };
})();
})



