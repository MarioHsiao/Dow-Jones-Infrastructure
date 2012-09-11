/*!
 * TopPages
 */

DJ.UI.TopPages = DJ.UI.CompositeComponent.extend({

    selectors: {
        portalHeadlineListContainer: '.portalHeadlineListContainer'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "TopPages" }, meta));
        
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
        $dj.subscribe('data.TopPages', this._delegates.setData);

        this.subscribe('headlineClick.dj.PortalHeadlineList', function (item) {
            window.open(item.headline.headlineUrl);
        });
    },
    
    _setData: function (data) {
        if (!this.portalHeadlines) {
            $dj.error("PortalHeadlinesComponent is not initialized. Refresh the page to try again.");
            return;
        }

        var headlines = _.map(data, function (page) {
            return {
                title: page.i,
                headlineUrl: "http://" + page.path,
                modificationTimeDescriptor: page.visitors.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
            };
        });

        var result = {
            count: { value: headlines.length },
            headlines: headlines
        };

        this.portalHeadlines.setData({ resultSet: result });
    },
    
    EOF: null
});


// Declare this class as a jQuery plugin
$.plugin('dj_TopPages', DJ.UI.TopPages);
