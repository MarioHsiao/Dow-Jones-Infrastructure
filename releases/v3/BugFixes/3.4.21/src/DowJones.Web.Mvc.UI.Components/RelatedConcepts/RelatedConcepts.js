/*!
* RelatedConcepts
*/

(function ($) {

    DJ.UI.RelatedConcepts = DJ.UI.CompositeComponent.extend({

        // Default options
        defaults: {
            debug: false,
            cssClass: 'RelatedConcepts'
        },

        tokens: {
        },

        selectors: {
            termItem: ".dj_related-concepts-list li.item"
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "RelatedConcepts" }, meta);
            this._super(element, $meta);
            this.getData();
        },

        events: {
            termClick: 'termClick.dj.relatedConcepts'
        },

        _initializeEventHandlers: function () {
            var self = this;
            this.$element.delegate(this.selectors.termItem, 'click', function (e) {
                var target = $(e.currentTarget)
                self.publish(self.events.termClick, { text: target.text(), weight: target.data("weight") });
                e.stopPropagation();
            });
        },

        getData: function (searchText) {
            var self = this,
                o = self.options,
                el = $(self.element);

            if (searchText) {
                o.keywords = searchText
            }

            if (o.keywords) {
                $.ajax({
                    url: this.buildUrl(),
                    type: 'GET',
                    success: $dj.delegate(this, this._getDataOnSuccess),
                    error: $dj.delegate(this, this._getDataOnFailure)
                });
            }
        },

        _getDataOnSuccess: function (data) {
            var self = this,
                o = self.options,
                el = $(self.element);

            if (data && data.terms) {
                self.data = data;
            }
            this.render();
        },

        _getDataOnFailure: function () {
        },

        render: function () {
            var self = this,
                o = self.options,
                el = $(self.element);

            el.empty();
            if (self.data && self.data.terms && self.data.terms.length && self.data.terms.length > 0) {
                el.html(self.templates.success({ data: self.data.terms.slice(0, 6) })); // slice it reduce it to 2 rows
            }
            else {
                el.parents(".dj_related-concepts").hide();
                //el.html('<div class="dj_message">' + "<%= Token("noRelatedConcepts") %>" + '</div>');
            }
        },

        buildUrl: function () {
            var self = this,
                o = self.options,
                el = $(self.element),
                tUrl = [];

            tUrl.push(o.dataServiceUrl);
            tUrl.push(o.dataServiceUrl.indexOf("?") <= 0 ? "?" : "&");
            tUrl.push("t=" + o.keywords);
            tUrl.push("&");
            tUrl.push("mt=" + o.maxNumberOfTerms);
            return tUrl.join("");
        },

        EOF: true
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_RelatedConcepts', DJ.UI.RelatedConcepts);


})(jQuery);