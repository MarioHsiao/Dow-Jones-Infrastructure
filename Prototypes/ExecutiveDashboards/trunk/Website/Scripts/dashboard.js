DJ.$dj.require(['DJ','$dj'], function (DJ,$dj) {
    DJ.UI.Dash = DJ.UI.Component.extend({

        init: function (el, meta) {
            this._super(el, meta);

            this._initializeModuleStyles();
            this._initializeModuleResizing();
            this._initializeTabs();

            this._groups = new DJ.DashGroupManager().start();
        },

        _initializeModuleResizing: function () {
            DJ.subscribe('resized.dj.CanvasModule', function (args) {
                var request = {
                    pageId: DJ.UI.Canvas.find().get_canvasId(),
                    moduleId: args.moduleId
                };
                $.ajax(this.options.moduleResizeUrl + args.newSize, { data: request });
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

            $('.minimize').append($('<i class="icon-chevron-up icon-white"/>'));
            $('.maximize').append($('<i class="icon-chevron-down icon-white"/>'));
            $('.dj_module .hide').append($('<i class="icon-remove icon-white"/>')).removeClass('hide');
            $('.dj_module-refresh').remove();
        },

        _initializeTabs: function () {
            $('.trendTabs li').not(':first').find('a').not('[href=#module-gallery]')
                .popover({ content: 'Coming soon!', delay: { show: 1000, hide: 1000 } })
                .click(function () {
                    var self = this;
                    setTimeout(function () {
                        $(self).popover('hide');
                    }, 1000);
                });
            $('LI.defaultTab A').tooltip();
        },

        EOF: null
    });


    DJ.DashGroupManager = DJ.Component.extend({
        
        _currentGroups: [],

        init: function (meta) {
            this._super(meta);
            $.connection.dashboard.messageReceived = this._delegates.messageReceived;
        },

        refresh: function () {
            var handleMessage = this._delegates.messageReceived;

            $.connection.dashboard.refresh(this._currentGroups)
                .done(function (messages) {
                    for (var i = 0; i < messages.length; i++) {
                        handleMessage(messages[i]);
                    }
                });
            return this;
        },
        
        start: function () {
            $.connection.hub.start()
                .done(this._delegates.refresh);
            return this;
        },

        stop: function () {
            $.connection.hub.stop();
            return this;
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                messageReceived: $dj.delegate(this, this._messageReceived),
                refresh: $dj.delegate(this, this.refresh)
            });
        },

        _messageReceived: function (message) {
            var prefix = 'data.';

            if (!message || message.error) {
                prefix = 'dataError.';
                $dj.error('Data error: ' + message.error, message.data);
            }

            DJ.publish(prefix + message.eventName, message.data);
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



