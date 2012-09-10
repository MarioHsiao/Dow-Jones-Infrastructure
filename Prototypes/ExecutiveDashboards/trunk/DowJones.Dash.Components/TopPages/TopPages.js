/*!
 * TopPages
 */

DJ.UI.TopPages = DJ.UI.CompositeComponent.extend({

    selectors: {
        portalHeadlineListContainer: '.portalHeadlineListContainer'
    },

    init: function (element, meta) {
        // Call the base constructor
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
        });
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            // TODO: Add delegates
            // e.g.  OnHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._portalHeadlinesContainer = this.$element.find(this.selectors.portalHeadlineListContainer);
    },

    _initializeEventHandlers: function () {
        var self = this;
        $dj.subscribe('data.TopPages', function (data) {
            if (!self.portalHeadlines) {
                $dj.error("PortalHeadlinesComponent is not initialized. Refresh the page to try again.");
                return;
            }

            var headlines = _.map(data, function (page) {
                return {
                    title: page.i,
                    headlineUrl: "http://" + page.path,
                    modificationTimeDescriptor: page.visitors
                };
            });

            var result = {
                count: { value: headlines.length },
                headlines: headlines
            };

            self.portalHeadlines.setData({ resultSet: result });
            
        });

        this.subscribe('headlineClick.dj.PortalHeadlineList', function (item) {
            window.open(item.headline.headlineUrl);
        });
    },
});


// Declare this class as a jQuery plugin
$.plugin('dj_TopPages', DJ.UI.TopPages);
