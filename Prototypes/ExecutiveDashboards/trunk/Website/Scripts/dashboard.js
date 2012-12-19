DJ.$dj.require(['DJ', '$dj'], function (DJ, $dj) {
    DJ.UI.Dash = DJ.UI.Component.extend({
        init: function (el, meta) {
            this._super(el, meta);

            this._groups = new DJ.DashGroupManager();
            this.timeout = null;
            this.interval = 10 * 1000; // 10 seconds

            this._initializeModuleStyles();
            this._initializeSubscriptions();
            this._initializeTabs();
            this._initializeWorldStatsMap();
            //this._initializeBrowserShare();
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
            return this;
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates || {}, {
                changeDomain: $dj.delegate(this, this.changeDomain),
                callModuleResize: $dj.delegate(this, this._callModuleResize),
                worldViewClick:  $dj.delegate(this, this._worldViewClick),
                platformStatsClick:  $dj.delegate(this, this._platformStatsClick)
            });
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

            $('.maximize')
                .before('<i class="icon-share icon-white dj_icon-pop-out" title="Pop out this module to its own page" />')
                .append($('<i class="icon-chevron-down icon-white"/>'));
            
            $('.dj_module .hide').append($('<i class="icon-remove icon-white"/>')).removeClass('hide');
            $('.dj_module-refresh').remove();

            this._initializeInfoIcons();
            this._initializePopout();
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
        },
        
        _initializePopout: function () {
            var self = this;
            $('.dj_icon-pop-out')
                .click(function () {
                    var el = $(this),
                        name = el.next('.dj_module-title').text(),
                        module = el.closest('.dj_module'),
                        moduleId = module.data('module-id'),
                        moduleHeight = module.outerHeight(true),
                        moduleWidth = module.outerWidth(true),
                        footerHeight = 46 + 20,  /* keep this in sync with _Footer.cshtml's rendered height + margin */
                        height = moduleHeight + footerHeight,
                        pageId = DJ.UI.Canvas.find().get_canvasId(),
                        url = self.options.popUpUrl + "?pid=" + pageId + "&mid=" + moduleId,
                        features = "resizable=no,scrollbars=no,status=no,location=no,height=" 
                                    + height + ",width=" + moduleWidth;

                    if (pageId && moduleId) {
                        // IE doesn't support custom names
                        if ($.browser.msie) name = "_blank";
                        window.open(url, name, features);
                    }
                });
        },

        _initializeTabs: function () {
            var changeDomain = this._delegates.changeDomain;

            var tabs = $('.dashNav li').not('> a[href=#module-gallery]');
            tabs.click(function () {
                var el = $(this);
                tabs.removeClass('active');
                el.addClass('active');
                changeDomain({ source: el.data('source'), map: el.data('map') });
            });
        },

        _getActiveTab: function () {
            return $('.dashNav li.active');
        },

        _initializeSubscriptions: function () {
            DJ.subscribe('resized.dj.CanvasModule', this._delegates.callModuleResize);
            DJ.subscribe('worldView.dj.StatsMap', this._delegates.worldViewClick);
            DJ.subscribe('deviceClick.dj.PlatformStats', this._delegates.platformStatsClick);
        },

        _worldViewClick: function () {
            $('#world-map-module-container').toggleClass('watch-wide');
            $('#world-map-module-inner').toggleClass('medium');
        },
        
        _platformStatsClick: function () {
            //$('#browserShareModal').modal('toggle');
        },
        
        _initializeWorldStatsMap: function () {
            DJ.add("PageLoadByRegionCanvasModule", {
                container: $('.worldMapContainer')[0],
                options: {
                    allowHostConfigurationChange: false,  /* do not respond to domain change events on tab switching */
                    mapType: 'world',
                    map: 'world'
                }
            });

            $('#world-map-module .actions-container .fi .icon-remove').click(this._delegates.worldViewClick);
        },
        
        _initializeBrowserShare: function () {
            DJ.add("BrowserShare", {
                container: $('#browserShareContainer')[0]
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
            $.connection.dashboard.ping = this._delegates.ping;
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

            window.domain = domain;
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
                messageReceived: $dj.delegate(this, this._messageReceived),
                ping: $dj.delegate(this._ping)
            };
        },
        
        _ping: function(data) {
            console.log(data);
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

    (function () {
        window._alert = window.alert;
        window.alert = function (msg) {
            $('#dashAlertModal .modal-body p').text(msg);
            $('#dashAlertModal').modal('show');
            return false;
        };
    })();
})



