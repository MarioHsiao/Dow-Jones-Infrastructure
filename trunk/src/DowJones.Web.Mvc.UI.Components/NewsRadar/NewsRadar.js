DJ.UI.NewsRadar = DJ.UI.Component.extend({
    selectors: {

    },

    options: {

    },

    events: {

    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "NewsRadar" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        // call databind if we got data from server
        $dj.debug('this data', this.data);
        if (this.data && this.data.resultSet)
            this.bindOnSuccess(this.data.resultSet);
    }

});

// Declare this class as a jQuery plugin
$.plugin('dj_NewsRadar', DJ.UI.NewsRadar);
$dj.debug('Registered DJ.UI.NewsRadar (extends DJ.UI.Component)');