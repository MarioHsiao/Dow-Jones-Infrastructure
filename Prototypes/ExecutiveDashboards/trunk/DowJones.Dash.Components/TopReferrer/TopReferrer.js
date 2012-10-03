/*!
 * TopReferrer
 */

DJ.UI.TopReferrer = DJ.UI.CompositeComponent.extend({

    selectors: {
        portalHeadlineListContainer: '.portalHeadlineListContainer'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "TopReferrer" }, meta));
        
        this._initPortalHeadlines();
    },

    _initPortalHeadlines: function () {
        var self = this;
        DJ.add('PortalHeadlineList', {
            container: this._portalHeadlinesContainer[0],
            options: { layout: 2 }
        }).done(function (comp) {
            self.portalHeadlines = comp;
            comp.setOwner(self);
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
        $dj.subscribe('data.Referrers', this._delegates.setData);
    },
    
    _setData: function (data) {
        if (!data)
            return;
        
        if (!this.portalHeadlines) {
            $dj.error("PortalHeadlinesComponent is not initialized. Refresh the page to try again.");
            return;
        }

        var headlines = [];
        var tempdata = [];
        for (var prop in data.referrers)
            if (prop.length > 0) {
                tempdata.push( { title: prop, count: data.referrers[prop], countDescriptor: Highcharts.numberFormat(data.referrers[prop], 0) });
            }

        tempdata = _.sortBy(tempdata, function (obj) { return obj.count; }).reverse();

        var len, i;
        for (i = 0, len = tempdata.length; i<len; i++) {
            headlines.push({ title: tempdata[i].title, modificationTimeDescriptor: tempdata[i].countDescriptor });
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
$.plugin('dj_TopReferrer', DJ.UI.TopReferrer);
