DJ.UI.Canvas.GridLayout = DJ.UI.Canvas.Layout.extend({
    defaults: {
        cells: [],
        width: 4
    },

    init: function (canvas, options) {
        this._super(canvas, options);

        this._gridster =
            $('<div class="gridster"></div>')
                .appendTo(this.element)
                .gridster({
                    autogenerate_stylesheet: true,
                    avoid_overlapped_widgets: true,
                    min_cols: this.options.width,
                    widget_selector: '> ' + this._moduleSelector,
                    draggable: { stop: this._delegates.save }
                })
                .data('gridster');
        
        this._initializeModules();
    },

    add: function (module, cell) {
        var el = (module instanceof DJ.UI.Component) ? module.element : module;
        if(cell)
            this._gridster.add_widget(el, cell.width, cell.height, cell.column, cell.row);
        else
            this._gridster.add_widget(el);
    },

    remove: function (module) {
        var el = (module instanceof DJ.UI.Component) ? module.element : module;
        this._gridster.remove_widget(el);
    },

    save: function () {
        var grid = [];
            
        var modules = $(this._moduleSelector, this.element);
        for (var i = 0; i < modules.length; i++) {
            var module = $(modules[i]);
            var cell = module;
            grid.push({
                moduleId: module.data('module-id'),
                row: cell.data('row'),
                column: cell.data('col'),
                width: cell.data('sizex'),
                height: cell.data('sizey')
            });
        }
        
        var request = { pageId: this.options.canvasId, grid: grid };

        this._debug('Saving positions: ' + JSON.stringify(request));

        $.ajax({
            url: this.options.dataServiceUrl,
            type: 'PUT',
            data: JSON.stringify(request)
        });

        return request;
    },
    
    _initializeDelegates: function () {
        $.extend(this._delegates, {
            save: $dj.delegate(this, this.save)
        });
    },
    
    _initializeModules: function() {
        _.each(
            $(this._moduleSelector, this.element), 
            function(el) {
                var moduleId = $(el).data("module-id");
                var cell = _.find(this.options.cells, function (c) { return c.moduleId == moduleId; });
                this.add(el, cell);
                $(el).show();
            }, 
            this);
    },

    EOF: null
});

DJ.UI.Canvas.Layout.register('grid', DJ.UI.Canvas.GridLayout);

$dj.debug('Registered DJ.UI.Canvas.GridLayout');