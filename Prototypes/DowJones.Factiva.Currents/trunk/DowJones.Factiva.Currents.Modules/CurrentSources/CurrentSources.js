/*!
 * CurrentSources Module
 */

    DJ.UI.CurrentSources = DJ.UI.CompositeComponent.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "CurrentSources" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        },

        _onSourceLogoLoad: function (idx) {
        var att = this.sourceTitles[idx];
        if (att) {
            $(att).hide();
            var at = this.sourceLogos[idx];
            if (at) {
                $(at).show();
            }
        }
    },

    _onSourceLogoError: function (idx, sourceName) {
        var att = this.sourceTitles[idx];
        if (att) {
            $(att).html(sourceName).show();
            var at = this.sourceLogos[idx];
            if (at) {
                $(at).hide();
            }
        }
    }
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CurrentSources', DJ.UI.CurrentSources);

    $dj.debug('Registered DJ.UI.CurrentSources as CurrentSources');
