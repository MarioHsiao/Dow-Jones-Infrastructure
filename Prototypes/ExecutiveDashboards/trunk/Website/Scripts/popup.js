DJ.$dj.require(['DJ', '$dj'], function (DJ, $dj) {
    DJ.UI.DashPopup = DJ.UI.Component.extend({
        
        init: function (el, meta) {
            this._super(el, meta);

            this._groups = new DJ.DashGroupManager();
            this.timeout = null;
            this.interval = 10 * 1000; // 10 seconds

            this._initializeModuleStyles();
        },

        changeDomain: function (domain) {
            this._groups.changeDomain(domain);
            return this;
        },
        
        start: function (domain) {
            this._groups.start(domain);
            return this;
        },

        stop: function () {
            this._groups.stop();
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates || {}, {
                changeDomain: $dj.delegate(this, this.changeDomain),
            });
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

            $('.minimize').hide();
            $('.maximize').hide();
            $('.dj_module-refresh').remove();

            this._initializeInfoIcons();

        },

        _initializeInfoIcons: function () {
            $('.dj_module-title')
                .after('<i class="icon-info-sign icon-white dj_icon-info"/>');

            $('.dj_module-title + .dj_icon-info')
                .each(function () {
                    var el = $(this),
                        tooltipAttached = el.data('tooltipAttached');

                    if (!tooltipAttached) {
                        el.tooltip({
                            title: el.closest('.dj_module').data('description'),
                            delay: { show: 500, hide: 100 },
                            placement: 'bottom'
                        });
                        el.data('tooltipAttached', true);
                    }
                });
        }
        

        
    });
    
    
    DJ.DashGroupManager = DJ.Component.extend({

        defaults: {
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
                oldSources = this._getSources(domain.source);

            // initialize for first time
            this.domain = this.domain || {};

            if (domain.source === this.domain.source)
                return;

            self.subscribedSources = this._getSources(domain.source);

            // fire a reset for the module to know we are recycling the view.
            DJ.publish(communicationPrefix + "domain.changed", domain);

            var handleMessage = this._delegates.messageReceived;
            if (oldSources) {
                $.connection.dashboard.unsubscribe(oldSources)
                    .done($dj.delegate(this, function () {
                        $.connection.dashboard.subscribe(self.subscribedSources)
                            .done(function (messages) {
                                self.domain = domain;
                                for (var i = 0; i < messages.length; i++) {
                                    handleMessage(messages[i]);
                                }
                            });
                    }));
            }
            else {
                $.connection.dashboard.subscribe(self.subscribedSources)
                    .done(function (messages) {
                        self.domain = domain;
                        for (var i = 0; i < messages.length; i++) {
                            handleMessage(messages[i]);
                        }
                    });
            }
        },
        
        stateChanged: function (change) {
            var self = this;
            if (change.newState === $.signalR.connectionState.reconnecting) {
                self.timeout = setTimeout(function () {
                    $("#state").css("background-color", "red")
                    .css("color", "white")
                    .html("[" + new Date().toTimeString() + "]: Connection: -unreachable");
                }, self.interval);
            }
            else if (self.timeout && change.newState === $.signalR.connectionState.connected) {
                $("#state").css("background-color", "cyan")
                           .css("color", "white")
                           .html("[" + new Date().toTimeString() + "]: Connection: -connected");
                clearTimeout(self.timeout);
                self.timeout = null;
            }
        },

        start: function (domain) {
            if (this._started)
                return this;

            $.connection.hub.stateChanged($dj.delegate(this, this.stateChanged));

            $.connection.hub.reconnected(function () {
                $("#state").css("background-color", "yellow")
                           .css("color", "white")
                           .html("[" + new Date().toTimeString() + "]: Connection: -reestablished");
            });
            
            $.connection.hub.start()
                .done($dj.delegate(this, function () {
                    this._started = true;
                    $("#state").css("background-color", "green")
                               .css("color", "white")
                               .html("[" + new Date().toTimeString() + "]: Connection: -online");
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


        _getSources: function (source) {

            var configurationEvents = [
                'BasicHostConfiguration'
            ];

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
                'DeviceTrafficDesktop',
                'DeviceTrafficMobile',
                'PageLoadHistoricalDetails',
                'PageTimings',
                'PageLoadDetailsByType',
                'PageLoadDetailsByTypeForWorld'
            ];

            var events = [];

            events = events.concat(configurationEvents)
                .concat(chartBeatEvents)
                .concat(gomezEvents);

            // Convert to '[domain]-[event name]', e.g. 'online.wsj.com-BrowserStats'
            return _.map(events, function (name) { return source + '-' + name; });
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
        }
    });
})