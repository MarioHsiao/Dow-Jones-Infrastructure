DJ.UI.ShowcaseCanvas = DJ.UI.AbstractCanvas.extend({

    init: function (element, meta) {
        this._super(element, meta);

        this._subscribeToModuleEvents();
    },

    _subscribeToModuleEvents: function () {
        this.subscribe('viewAllClick.dj.PortalHeadlineLists', $dj.delegate(this, this._onViewAllClick));
    },

    _onViewAllClick: function (data) {
        $dj.debug('View All Click triggered.', data);
        $dj.publish('viewAllClick.dj.ShowcaseCanvas', data)
    }

});

$.plugin('dj_ShowcaseCanvas', DJ.UI.ShowcaseCanvas);

$dj.debug('Registered DJ.UI.ShowcaseCanvas as dj_ShowcaseCanvas');
