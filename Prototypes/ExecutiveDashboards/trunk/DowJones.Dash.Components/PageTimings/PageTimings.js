/*!
 * TopReferrer
 */

DJ.UI.PageTimings = DJ.UI.CompositeComponent.extend({

    selectors: {
        portalHeadlineListContainer: '.portalHeadlineListContainer'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "PageTimings" }, meta));
        this._initPortalHeadlines();
    },

    _initPortalHeadlines: function () {
        var self = this;
        DJ.add('PortalHeadlineList', {
            container: this._portalHeadlinesContainer[0],
            options: { layout: 2 }
        }).done(function (comp) {
            self.portalHeadlines = comp;
            comp.owner = self;
        });
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._portalHeadlinesContainer = this.$element.find(this.selectors.portalHeadlineListContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.PageTimings', this._delegates.setData);
    },
    
    _setData: function (data) {
        if (!this.portalHeadlines) {
            $dj.error("PortalHeadlinesComponent is not initialized. Refresh the page to try again.");
            return;
        }

        var headlines = [];
        
        for (var i = 0; i < data.length; i++) {
            headlines[headlines.length] = { title: data[i].page_name, modificationTimeDescriptor: Highcharts.numberFormat(data[i].page_load_time/1000, 3) + "s" };
        }

        var result = {
            count: { value: headlines.length },
            headlines: headlines
        };

        this.portalHeadlines.setData({ resultSet: result });
    },
    
    EOF: null
});


// Declare this class as a jQuery plugin
$.plugin('dj_PageTimings', DJ.UI.PageTimings);
