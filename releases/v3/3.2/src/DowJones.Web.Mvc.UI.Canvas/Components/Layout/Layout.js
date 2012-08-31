DJ.UI.Canvas.Layout = DJ.Component.extend({
    init: function (canvas, options) {
        this._super({ options: options });
        this.element = canvas.element;

        if (!this.options.canvasId && canvas.options) {
            this.options.canvasId = canvas.options.canvasId;
        }
    },

    add: function (module) {
        $dj.debug('TODO: Implement DJ.UI.Canvas.Layout.add()');
    },

    remove: function (module) {
        $dj.debug('TODO: Implement DJ.UI.Canvas.Layout.remove()');
    },

    save: function () {
        $dj.debug('TODO: Implement DJ.UI.Canvas.Layout.save()');
    },

    EOF: null
});


DJ.UI.Canvas.Layout._layouts = {};

DJ.UI.Canvas.Layout.register = function (name, layout) {
    DJ.UI.Canvas.Layout._layouts[name] = layout;
    $dj.debug('Registered layout ', name, layout);
};

DJ.UI.Canvas.Layout.initialize = function (canvas, options) {
    var layout;
    var name = (options || {}).name;

    if (DJ.UI.Canvas.Layout._layouts[name]) {
        layout = DJ.UI.Canvas.Layout._layouts[name];
    } else {
        layout = DJ.UI.Canvas.Layout._layouts['zone'];
    }

    return new layout(canvas, options);
};

$dj.debug('Registered DJ.UI.Canvas.Layout');