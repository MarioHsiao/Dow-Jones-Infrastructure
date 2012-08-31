DJ.UI.Canvas.ZoneLayout = DJ.UI.Canvas.Layout.extend({
    defaults: {
        dataServiceUrl: null,
        groups: null,
        sortableSettings: {
            axis: false,
            containment: false,
            delay: 100,
            distance: 30,
            draggingClass: "dj_module-dragging",
            forcePlaceholderSize: true,
            handle: ".dj_module-handle",
            items: "> .dj_module-movable",
            opacity: 0.8,
            placeholder: "dj_module-placeholder",
            revert: 300,
            scroll: true,
            tolerance: "pointer" // pointer || intersect [default]
        }
    },

    _zoneSelector: '.dj_group',
    _moduleSelector: '.dj_module',

    init: function (canvas, options) {
        // Load sane defaults from the canvas
        if (!options)
            options = {};
        if (!options.zoneCount)
            options.zoneCount = canvas.options.NumberOfGroups;
        if (!options.dataServiceUrl)
            options.dataServiceUrl = canvas.options.webServiceBaseUrl + '/modules/positions/json';


        this._super(canvas, options);

        // Map refactored layout methods for backwards compatability
        canvas.getZones = this._delegates.getZones;
        canvas.moveModule = this._delegates.move;
        canvas.saveCanvasModulePositions = this._delegates.save;

        this._initializeModules();
    },

    add: function (module, zone) {
        var el = module;

        if (module instanceof DJ.UI.Component)
            el = module.element;

        zone = zone || 0;

        if (!isNaN(zone))
            zone = this._getZones()[zone];

        $(zone).prepend(el);
    },

    remove: function (module) {
        var el = module;

        if (typeof (module) == DJ.UI.Component)
            el = module.element;

        $(el).remove();
    },

    save: function () {
        var zones = [];

        _.each(this._getZones(), function (zone) {
            var modules = this._getModules(zone);
            var moduleIds = _.map(modules, function (module) { return module.get_moduleId(); });
            zones[zones.length] = moduleIds;
        }, this);

        var request = { pageId: this.options.canvasId, columns: zones };

        this._debug('Saving positions: ' + JSON.stringify(request));

        $.ajax({
            url: this.options.dataServiceUrl,
            type: 'PUT',
            data: JSON.stringify(request)
        });

        return request;
    },


    _getModules: function (zone) {
        var moduleElements = $(this._moduleSelector, zone || this.element);

        var modules = _.map(moduleElements, function (el) {
            var module = $(el).findComponent(DJ.UI.AbstractCanvasModule);
            return module;
        });

        return _.filter(modules, function (module) { return module !== null; });
    },

    _getZones: function () {
        var zoneCount = this.options.zoneCount || (this.options.groups || [[]]).length;
        var zones = $(this._zoneSelector, this.element).get();

        if (zones.length !== zoneCount) {
            var zoneClass = this._zoneSelector.substring(1);

            zoneClass += " span" + (12 / zoneCount);

            for (var i = 0; i < zoneCount; i++) {
                var zone =
                    $("<div />")
                        .addClass(zoneClass)
                        .attr('id', 'zone-' + i)
                        .appendTo(this.element);

                zones.push(zone);
            }
        }

        return $(zones);
    },

    _initializeEventHandlers: function () { },

    _initializeDelegates: function () {
        this._delegates = {
            getZones: $dj.delegate(this, this._getZones),
            move: $dj.delegate(this, this._move),
            save: $dj.delegate(this, this.save)
        };
    },

    _initializeModules: function () {
        $(this._moduleSelector, this.element).hide();

        var groups = this.options.groups;
        var zones = this._getZones();
        var modules = this._getModules();
        var moduleZones = [];

        // Backwards Compatibility: build groups if none are provided
        if (!groups) {
            var zoneCount = this.options.zoneCount || 1;

            groups = new Array(zoneCount);

            _.each(
                 _.sortBy(modules, function (mod) { return mod.options.position; }),
                 function (mod) {
                     var zoneIndex = Math.ceil(mod.options.position % zoneCount);
                     var zone = groups[zoneIndex] || (groups[zoneIndex] = []);
                     zone.push(mod.get_moduleId());
                 }
             );
        }

        for (var zoneIndex = 0; zoneIndex < groups.length; zoneIndex++) {
            var zone = groups[zoneIndex] || [];
            for (var i = 0; i < zone.length; i++) {
                moduleZones.push({ moduleId: zone[i], zone: zoneIndex });
            }
        }

        for (var x = 0; x < moduleZones.length; x++) {
            var moduleZone = moduleZones[x];
            var zone = zones[moduleZone.zone];
            var module = _.find(modules, function (mod) {
                return mod.get_moduleId() == moduleZone.moduleId;
            });

            $(module.element).appendTo(zone).show();
        }

        this._initializeModuleReordering();

        this._debug('Initialized ' + moduleZones.length + ' modules in ' + zones.length + ' zones');
    },

    _initializeModuleReordering: function () {
        // Get a copy of the default sort-able settings
        // and update it with our current items and delegates
        var settings = $.extend({}, this.options.sortableSettings);

        var zones = this._getZones();
        var sortableItems = $(settings.items, zones);
        var tooltipHandles = sortableItems.find(settings.handle + ">H3");

        // Save delegate
        var savePositions = this._delegates.save;

        $.extend(settings, {
            start: function (e, ui) {
                $(ui.helper).addClass(settings.draggingClass);

                if (document.selection) {
                    document.selection.clear();
                }

                zones.disableSelection().sortable('refreshPositions');

                tooltipHandles.data("enableSimpleTooltip", false);
                $("#dj_tooltip").hide();
            },

            stop: function (e, ui) {
                $(ui.item).css({ width: '' }).removeClass(settings.draggingClass);

                zones.enableSelection().sortable('enable');

                if (document.selection) {
                    document.selection.clear();
                }

                savePositions();

                tooltipHandles.data("enableSimpleTooltip", true);
            }
        });

        zones.sortable(settings);

        if (zones.length > 1) {
            zones.sortable("option", "connectWith", zones);
        }

        // Initialize Crosshairs
        sortableItems.find(settings.handle).css({ cursor: 'move' });

        this._debug('Initialized module sorting');
    },

    _move: function (module, direction) {
        this._debug('Moving module ' + module.toString() + ' ' + direction);

        var $module = $(module._module);
        if (direction === 'up') {
            $module.insertBefore($module.prev());
        }
        else {
            $module.insertAfter($module.next());
        }

        this.save();
    },

    EOF: null
});

DJ.UI.Canvas.Layout.register('zone', DJ.UI.Canvas.ZoneLayout);

$dj.debug('Registered DJ.UI.Canvas.ZoneLayout');