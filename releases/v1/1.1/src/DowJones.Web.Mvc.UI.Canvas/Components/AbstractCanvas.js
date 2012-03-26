/// <reference name="common.js" assembly="DowJones.Web.Mvc" />

(function ($) {

	DJ.UI.AbstractCanvas = DJ.UI.Component.extend({

		defaults: {
			NumberOfGroups: 1
		},

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
		},

		_canvasName: null,
		_canvasRenderManager: null,
		_moduleCount: 0,
		_registeredModules: null,
		_moduleSelector: ".dj_module",
		_contentSelector: ".dj_module-body",
		_groupContainerSelector: ".dj_groups",
		_groupSelector: ".dj_group",
		_sortableItems: {},
		_pubSubCache: null,

		$activate$i: 0,
		$deactivate$i: 0,
		$subscribedElements: [],

		init: function (element, meta) {
			var startDate = new Date();
			this._debug("Start Initialize AbstractCanvasRenderManager");

			this._super(element, meta);

			this._selectors = $dj.clone(this._selectors);
			this.sortableSettings = $dj.clone(this.sortableSettings);

			$.extend(this.options, this.getSettings());

			this._numberOfZones = this.options.NumberOfGroups;

			this._canvasRenderManager = $(this.element).get(0);

			this._initializeZones();
			this._initializeModules();
			this._initializeModuleReordering();

			this._pubSubCache = this._pubSubCache || { __hashKey: this._getHashKey() };

			this._debug("End Initialize AbstractCanvasRenderManager: " + (new Date().getTime() - startDate.getTime()));
		},

		addModule: function (moduleId) {
			if (!this.canAddModule(moduleId)) { return; }

			var request = {
				'pageId': this.options.canvasId,
				'id': moduleId
			};

			this._invokeService({
				url: this.options.addModuleUrl + '?' + $.param(request),
				onComplete: this._delegates.fireModuleAdded,
				dataType: 'html'
			});
		},

		deleteModule: function (moduleId, onSuccess, onError) {
			var request = { pageId: this.get_canvasId(), moduleId: moduleId };

			var success =
                $dj.delegate(this,
                    function (obj) {
                    	this._initializeModules();
                    	if (onSuccess) { onSuccess(obj); }
                    }
                );

			this._invokeService({
				url: this.options.deleteModuleUrl + '?' + $.param(request),
				method: 'DELETE',
				onSuccess: success,
				onError: onError
			});
		},


		getModuleIds: function () {
			var moduleIds = [];
			var moduleElements = $(this._moduleSelector, this.$element);
			$.each(moduleElements, function (i, el) {
				var moduleComponent = $(this).findComponent(DJ.UI.AbstractCanvasModule);
				moduleIds[moduleIds.length] = moduleComponent.get_moduleId();
			});

			return moduleIds;
		},

		dispose: function () {
			this._super();

			// Clean up common variables
			this._canvasRenderManager = null;
			this._sortableItems = null;
			this._sortableSettings = null;
			this._registeredModules = null;
			this.options = null;
			this.$deactivate$i = null;
			this.$activate$i = null;
			this.$subscribedElements = null;
		},

		publish: function (eventName, sender, args) {
			if (!this._pubSubCache || !eventName) {
				this._debug('AbstractCanvas.publish: Event not found.', eventName, sender, args);
				return;
			}

			this._debug('AbstractCanvas.publish: using cache #' + this._pubSubCache.__hashKey + ': publishing event ' + eventName);

			if (this._pubSubCache[eventName]) {
				_.each(this._pubSubCache[eventName], function (handler) {
					if ($.isFunction(handler)) { handler.apply(this, [sender, args]); }
				});
			}
		},


		subscribe: function (eventName, handler) {

			if (!eventName) {
				this._debug('AbstractCanvas.subscribe: Event Name cannot be null or empty');
				return;
			}

			if (!$.isFunction(handler)) {
				this._debug('AbstractCanvas.subscribe: Not a valid handler');
				return;
			}

			this._debug('AbstractCanvas.subscribe: using cache #' + this._pubSubCache.__hashKey +
                        ': subscribing to event ' + eventName +
                        ' with handler: ' + ((handler && handler.name) ? handler.name : 'anonymous'));

			this._pubSubCache[eventName] = this._pubSubCache[eventName] || [];
			this._pubSubCache[eventName].push(handler);
			return [eventName, handler];
		},



		getSettings: function () {
			var _temp = window[this.getId() + "_clientState"];
			if (_temp !== null) {
				return _temp;
			}
			return null;
		},

		getZones: function () {
			var zones = $(this._groupSelector, this._canvasRenderManager);
			return zones;
		},

		moveModule: function (module, direction) {
			this._debug('Moving module ' + module.toString() + ' ' + direction);

			var $module = $(module._module);
			if (direction === 'up') {
				$module.insertBefore($module.prev());
			}
			else {
				$module.insertAfter($module.next());
			}

			this._initializeZones();
			this.saveCanvasModulePositions();
		},


		saveCanvasModulePositions: function () {
			var zones = [];
			var moduleSelector = this._moduleSelector;

			$.each(this.getZones(), function () {
				var moduleIds = zones[zones.length] = [];
				var moduleElements = $(moduleSelector, this);
				$.each(moduleElements, function (i, el) {
					var moduleComponent = $(this).findComponent(DJ.UI.AbstractCanvasModule);
					moduleIds[moduleIds.length] = moduleComponent.get_moduleId();
				});
			});

			var request = { pageId: this.get_canvasId(), columns: zones };

			this._invokeService({
				url: this.options.webServiceBaseUrl + '/modules/positions/json',
				method: 'PUT',
				data: JSON.stringify(request)
			});
		},

		canAddModule: function () {
			$dj.debug("Override this in Application with app sepcific logic to control when a module can be added");
			return true;
		},

		_invokeService: function (params) {
			var extendedParams = $.extend(
                {
                	controlData: this.get_ControlData(),
                	preferences: this.get_Preferences()
                },
                params
            );
			$dj.proxy.invoke(extendedParams);
		},

		_fireModuleAdded: function (xhr) {
			var err = $dj.getError(xhr);
			if (err !== null) {
				this.publish('addModuleError.dj.Canvas', this, err);
				return;
			}

			$(this.element).append(xhr.responseText);
			this._initializeModules();

			this.publish('addModuleSuccess.dj.Canvas', this);
		},

		_fireSortableOnActivate: function (e, ui) {
			this._debug('_fireSortableOnActivate: ' + ui.id);
			this.$activate$i++;
			if (this.$activate$i % this.options.NumberOfGroups === 0) {
				this._debug("fireSortableOnActivate");
			}
		},

		_fireSortableOnDeactivate: function (e, ui) {
			this._debug('_fireSortableOnDeactivate: ' + ui.id);

			this.$deactivate$i++;

			// If all of the zones are not done deactivating,
			// ignore this event and wait for the last one
			if (this.$deactivate$i % this._numberOfZones !== 0) {
				this._debug('Ignoring fireSortableOnDeactivate for ' + ui.id +
                            '. Waiting for last zone to deactivate.');
				return;
			}

			this._debug("fireSortableOnDeactivate");
			this.getZones().enableSelection();

			if (document.selection) {
				document.selection.clear();
			}

			this.saveCanvasModulePositions();

			var _groupCntr = $(this._groupContainerSelector, this._canvasRenderManager).get(0);
			if (_groupCntr) {
				var _children = $(_groupCntr).children();
				if ($.browser.msie && $.browser.version === 6.0) {
					_children.css({ 'height': '400px' });
				}
				_children.css({ 'min-height': '400px' });
			}

			this._sortableItems.find(this.sortableSettings.handle + ">H3").data("enableSimpleTooltip", true);
		},

		_fireSortableOnStart: function (e, ui) {
			$(ui.helper).addClass(this.sortableSettings.draggingClass);

			if (document.selection) {
				document.selection.clear();
			}

			this.getZones()
                    .disableSelection()
                    .sortable('refreshPositions');
			this._sortableItems.find(this.sortableSettings.handle + ">H3").data("enableSimpleTooltip", false);
			$("#dj_tooltip").hide();
		},

		_fireSortableOnStop: function (e, ui) {
			$(ui.item).css({ width: '' }).removeClass(this.sortableSettings.draggingClass);
			this.getZones().sortable('enable');
		},

		_initializeDelegates: function () {
			this._delegates = {
				fireModuleAdded: $dj.delegate(this, this._fireModuleAdded),
				fireSortableOnActivate: $dj.delegate(this, this._fireSortableOnActivate),
				fireSortableOnDeactivate: $dj.delegate(this, this._fireSortableOnDeactivate),
				fireSortableOnStart: $dj.delegate(this, this._fireSortableOnStart),
				fireSortableOnStop: $dj.delegate(this, this._fireSortableOnStop)
			};
		},

		_initializeModules: function () {
			var zones = this.getZones();
			var zoneCount = zones.length;

			var modules = $(this._moduleSelector, this.element);

			_.each(modules, function (el) {
				var $el = $(el);
				var module = $el.findComponent(DJ.UI.AbstractCanvasModule);

				if (module === null) { return; }

				module._set_Owner(this);

				if (module.options.HasClientData) {
					module.getData();
					module.options.HasClientData = false;
				}

				if (module.options.position > -1) {
					var zoneIndex = Math.ceil(module.options.position % zoneCount);
					var zone = (zoneIndex > -1) ? zones.get(zoneIndex) : null;
					$(zone).append($el);
				}
				else {
					$dj.debug('hiding module ' + module.toString() + ' because it does not have a Position');
					$(module.element).hide();
				}

				// wire up events
				this.events = this.events || {};
				_.each(module.events, function (value, key) {
					this.events[key] = value;
				}, this);
			}, this);
		},

		_initializeModuleReordering: function () {
			this._debug("_initializeModuleReordering: Started...");

			// Get a copy of the default sort-able settings
			// and update it with our current items and delegates
			var settings = $.extend(this.sortableSettings, {
				start: this._delegates.fireSortableOnStart,
				stop: this._delegates.fireSortableOnStop,
				activate: this._delegates.fireSortableOnActivate,
				deactivate: this._delegates.fireSortableOnDeactivate
			});

			var _zones = this.getZones();

			_zones.sortable($.extend(settings));

			if (_zones.length > 1) {
				_zones.sortable("option", "connectWith", _zones);
			}

			// Initialize Crosshairs
			this._sortableItems = $(settings.items, _zones);
			this._sortableItems.find(this.sortableSettings.handle)
                    .css({ cursor: 'move' })
                    .mousedown(this._fireCrossHairsCursorOnMousedown)
                    .mouseup(this._fireCrossHairsCursorOnMouseup);

			$(">H3", $(this.sortableSettings.handle, this._sortableItems))
                .attr("title", this.tokens.moduleDrag);

			this._debug('_initializeModuleReordering: Initialized ' +
                        this._sortableItems.length + ' modules in ' + this._numberOfZones + ' zones');

			this._numberOfZones = _zones.length;
		},

		_initializeZones: function () {
			var zones = this.getZones();

			if (zones.length === 0) {
				var zoneContainer = $(this._canvasRenderManager);
				var zoneClass = this._groupSelector.substring(1);
				var i;

				for (i = 0; i < this.options.NumberOfGroups; i++) {
					$("<div />")
                            .addClass(zoneClass)
                            .attr('id', 'zone-' + i)
                            .appendTo(zoneContainer);
				}
			}
		}

	});

	$dj.debug('Registered DJ.UI.AbstractCanvas (extends DJ.UI.Component)');

})(jQuery);