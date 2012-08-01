/*
 * jQuery doTimeout: Like setTimeout, but better! - v1.0 - 3/3/2010
 * http://benalman.com/projects/jquery-dotimeout-plugin/
 * 
 * Copyright (c) 2010 "Cowboy" Ben Alman
 * Dual licensed under the MIT and GPL licenses.
 * http://benalman.com/about/license/
 */
(function ($) { var a = {}, c = "doTimeout", d = Array.prototype.slice; $[c] = function () { return b.apply(window, [0].concat(d.call(arguments))) }; $.fn[c] = function () { var f = d.call(arguments), e = b.apply(this, [c + f[0]].concat(f)); return typeof f[0] === "number" || typeof f[1] === "number" ? this : e }; function b(l) { var m = this, h, k = {}, g = l ? $.fn : $, n = arguments, i = 4, f = n[1], j = n[2], p = n[3]; if (typeof f !== "string") { i--; f = l = 0; j = n[1]; p = n[2] } if (l) { h = m.eq(0); h.data(l, k = h.data(l) || {}) } else { if (f) { k = a[f] || (a[f] = {}) } } k.id && clearTimeout(k.id); delete k.id; function e() { if (l) { h.removeData(l) } else { if (f) { delete a[f] } } } function o() { k.id = setTimeout(function () { k.fn() }, j) } if (p) { k.fn = function (q) { if (typeof p === "string") { p = g[p] } p.apply(m, d.call(n, i)) === true && !q ? o() : e() }; o() } else { if (k.fn) { j === undefined ? e() : k.fn(j === false); return true } else { e() } } } })(jQuery);

/*!
* Radar
*/

    DJ.UI.Radar = DJ.UI.Component.extend({

        /*
        * Properties
        */
        events: {
            nodeClick: "nodeClick.dj.Radar"
        },

        // Default options
        defaults: {
            debug: false,
            cssClass: 'dj_Radar',
            pageSize: 6
        },


        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "Radar" }, meta);

            // Call the base constructor
            this._super(element, $meta);
        },


        /*
        * Public methods
        */

        setData: function (radarData) {
            this.data = radarData;
            this._renderRadar();
        },

        /*
        * Private methods
        */
        _renderRadar: function () {
            var self = this,
                el = $(self.element),
                o = self.options;

            el.empty().html("");
            var $moduleFooter = $('.module-footer', el.parents('.dj_module-core')).empty();
            
            if (self.data) {
                var now = new Date();
                // radar
                var $radarContainer = $("<div class='radar-container'></div>").appendTo(el);
                // 14 and 194 are right and left margins of radar-content, defined in css
                var availableWidth = $radarContainer.width() - 14 - 194;
                var pagesCount = Math.ceil(self.data.parentNewsEntities.length / o.pageSize);
                //Ron: removing the dynamic calculation of the image width, IE7 calculates availableWidth as -208 due to $radarContainer.width() not having a value 
                //var imageWidth = Math.floor(availableWidth / o.pageSize);
                var imageWidth = 128;

                $radarContainer.append(this.templates.success({
                    tokens: self.tokens,
                    data: self.data,
                    pageSize: o.pageSize,
                    pagesCount: pagesCount,
                    radarNodeTotalImageUrl: '<%= WebResource("DowJones.Web.Mvc.UI.Components.Radar.images.bar-graph-ltBlue.png") %>',
                    radarNodeValueImageUrl: '<%= WebResource("DowJones.Web.Mvc.UI.Components.Radar.images.bar-graph-dkBlue.png") %>',
                    //imgWidth: Math.floor(availableWidth / o.pageSize)
                    imgWidth: imageWidth
                }));

                $.each(el.find(".radar-box .radar-node"), function (i, v) {
                    var $radarNode = $(v),
                        x = $radarNode.attr("x"),
                        y = $radarNode.attr("y");
                    $radarNode
                        .unbind()
                        .bind('click', function (event) {
                            self.currentNodeClicked = this;
                            var currentVolumeCount = parseInt(self.data.parentNewsEntities[x].newsEntities[y].currentTimeFrameNewsVolume.value, 10);
                            el.triggerHandler(self.events.nodeClick, {
                                sender: self,
                                target: event.target,
                                title: self.data.parentNewsEntities[x].descriptor + ' - ' + self.data.parentNewsEntities[x].newsEntities[y].descriptor,
                                parentNewsEntitiesSearchContextRef: self.data.parentNewsEntities[x].searchContextRef,
                                newsEntitiesSearchContextRef: self.data.parentNewsEntities[x].newsEntities[y].searchContextRef,
                                "headlineCount": currentVolumeCount
                            });
                        })
                        .bind('mouseover', function () {
                            if ($dj.isCalloutVisible())
                                return;
                            self.clearTrailNodes();
                            self.getInTrailNodes(this).addClass('radar-node-intrail');
                        })
                        .bind('mouseout', function () {
                            if ($dj.isCalloutVisible())
                                return;
                            self.clearTrailNodes();
                            self.getInTrailNodes(this).removeClass('radar-node-intrail');
                        });
                });

                //pagination using jQuery cycle plugin                
                if (pagesCount > 1) {
                    var $radarContentPages = el.find('.radar-content-pages');

                    //pager
                    //updated by Ron to center the pager
                    //TODO: this creates a dependancy of the module markup thus preventing the component pager to live anywhere besides a module

                    var $pager = $("<div class='module-pager'></div>").appendTo($moduleFooter);

                    //kill any previous cycle instances and inline styles
                    $radarContentPages.cycle('destroy').removeAttr('style').children().removeAttr('style').not(':first').hide();

                    el.doTimeout(250, function () {
                        $radarContentPages.cycle({
                            timeout: 0,
                            fx: 'scrollHorz',
                            pager: $pager,
                            activePagerClass: 'pager-active',
                            pagerAnchorBuilder: function (idx, slide) {
                                return '<a class="pager-link pager-' + (idx + 1) + '"></a>';
                            }
                        });
                    });

                    //No device check. If the device supports it will work or else will do nothing
                    //if ($.iDevices.iPad) {
                        $radarContentPages.touchwipe({
                            wipeLeft: function () {
                                $radarContentPages.cycle("next");
                            },
                            wipeRight: function () {
                                $radarContentPages.cycle("prev");
                            },
                            preventDefaultEvents: false
                        });
                    //}
                }
            }
            else {
                //render error template?
            }
        },

        getInTrailNodes: function (radarNode) {
            var $radarNode = $(radarNode),
                $td = $radarNode.closest('td'),
                $tr = $td.closest('tr'),
                $page = $tr.closest('table'),
                $tds = $tr.children().filter('td'),
                $trs = $page.find('tr'),
                tdIdx = $tds.index($td),
                trIdx = $trs.index($tr),
                trail = $tds.slice(0, tdIdx + 1).get(),
                $subjects = $(this.element).find('.radar-subject');

            $trs.slice(0, trIdx + 1).each(function () {
                var td = $(this).children().filter('td').get(tdIdx);
                trail.push(td);
            });

            trail.push($subjects.get(trIdx));

            return $(trail).children().filter('.radar-node');
        },

        clearTrailNodes: function () {
            if (this.currentNodeClicked) {
                this.getInTrailNodes(this.currentNodeClicked).removeClass('radar-node-intrail');
                this.currentNodeClicked = null;
            }
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_Radar', DJ.UI.Radar);
