(function (DJ, $, $dj) {
    //  The "inheritance plugin" model
    //  http://alexsexton.com/?p=51
    // Modified by Framework team
    $.plugin = function (name, object) {
        $.fn[name] = function (options) {
            var instance = $.data(this[0], name, new object(this[0], options));
            return instance;
        };
    };

    // Custom :data() selector
    $.expr[':'].data = function (elem, counter, params) {
        if (!elem || !params) {
            return false;
        }

        var query = params[3];
        if (query) {
            var split = query.split('=');

            var data = $(elem).data(split[0]);
            if (data) {
                // If the query was just checking to see if the
                // field existed, then we're good!
                if (split.length === 1) {
                    return true;
                }

                return (data + '') === split[1];
            }
        }

        return false;
    };

    $.extend($.fn, {
        filterByData: function (key, value) {
            return this.filter([':data("', key, '=', value, '")'].join(''));
        },

        findComponent: function (componentTypeOrName) {
            /// <summary>
            ///     Finds a View Component's instance given the Type or the plugin name.
            /// </summary>
            /// <param name="componentTypeOrName" type="String|DJ.UI.Component">
            ///     An expression to search with.
            /// </param>
            /// <example>
            ///
            ///     // Create a view component such as PortalHeadlineList
            ///     DJ.UI.PortalHeadlineList = DJ.UI.Component.extend({ ... });
            ///     $.plugin('dj_PortalHeadlineList', DJ.UI.PortalHeadlineList);
            ///
            ///     // returns the ViewComponent object by the type
            ///     $('#PortalList1').findComponent(DJ.UI.PortalHeadlineList);
            ///
            ///     // returns the ViewComponent object by plugin name
            ///     $('#PortalList1').findComponent('dj_PortalHeadlineList');
            ///
            /// </example>
            
            if (!componentTypeOrName) 
                return this._findComponent(DJ.UI.Component);
            
            return (DJ.$dj.isString(componentTypeOrName) ?
                this._getComponent(componentTypeOrName) : this._findComponent(componentTypeOrName));
        },

        _findComponent: function (componentType) {
            var component = null;

            try {
                $.each(this.data() || [], function (i, datum) {
                    if (component !== null) {
                        return;
                    }

                    if (datum instanceof componentType) {
                        component = datum;
                    }
                });
            } catch (e) {
            }

            return component;
        },

        _getComponent: function (pluginName) {
            return this.data(pluginName);
        },

        dj_dropDownMenu: function (originalOptions, handler) {
            var options = $.extend({
                trigger: $('.trigger', this),
                menu: $(this)
            }, originalOptions);

            var menu = $(options.menu);

            if (menu.length === 0) {
                throw "Cannot initialize menu without a target menu element";
            }

            menu
                .css({ 'position': 'fixed', "z-index": '100' })
                .mouseleave(function () { $(this).hide("fast"); })
                .click(function () { $(this).hide("fast"); });

            // add click and mouse-over events
            $("li", menu).each(function (i, menuItem) {
                var $menuItem = $(menuItem);
                var command = $menuItem.data('command');

                // If no command was supplied via data, use the href
                if (!command) {
                    var href = $('a', $menuItem).attr('href');
                    if (href && href[0] === '#') {
                        command = href.substring(1);
                        $menuItem.data('command', command);
                        $('a', $menuItem).attr('href', 'void(0)');
                    }
                }

                $menuItem
                    .hover(
                        function (e) { $(this).addClass("dj_mouseover"); },
                        function (e) { $(this).removeClass("dj_mouseover"); }
                    )
                    .click(function (e) {
                        if (handler) {
                            handler(command, $menuItem);
                        } else {
                            $dj.debug('Menu command (with no handler): ' + command);
                        }
                    });
            });

            $(options.trigger).click(function () {
                var offset = $(this).offset() || {};
                var w = (offset.left - menu.outerWidth(true) + $(this).outerWidth(true)) + "px";
                var h = offset.top + $(this).height() + "px";

                menu.css({ "left": w, "top": h }).slideToggle("fast");
            });

            return $(this);
        },

        dj_snippetTooltip: function (className, containerName) {
            return this.each(function () {
                var text = $(this).attr("title");
                $(this).attr("title", "");
                if (text !== undefined && text !== "") {
                    $(this).hover(function (e) {
                        var parentTag = $(this).closest("." + containerName).get(0);
                        $(this).attr("title", "");
                        if (parentTag && $(parentTag).data("options").displaySnippets === "hover" || $(parentTag).data("options").displaySnippets === "inline") {
                            $("body").append("<div id='dj_snippetTooltip' class=\"" + className + "\" style='position: absolute; z-index: 100; display: none;'>" + text + "</div>");
                            var $tObj = $("#dj_snippetTooltip");
                            var cRight = e.pageX + 12 + $tObj.outerWidth() - $dj.getHorizontalScroll();
                            var cBottom = e.pageY + 12 + $tObj.outerHeight() - $dj.getVerticalScroll();
                            var tipX = e.pageX + 12;
                            var tipY = e.pageY + 12;
                            if (cRight >= $dj.getClientWidth()) {
                                tipX = e.pageX - 12 - $tObj.outerWidth();
                            }
                            if (cBottom >= $dj.getClientHeight()) {
                                tipY = e.pageY - 12 - $tObj.outerHeight();
                            }

                            $tObj.css("left", tipX).css("top", tipY).css("z-index", "1000000");
                            $tObj.stop(true, true).fadeIn("fast");
                        }

                    }, function (e) {
                        $("#dj_snippetTooltip").remove();
                        $(this).attr("title", text);
                        e.stopPropagation();
                    });
                    $(this).mousemove(function (e) {
                        var $tObj = $("#dj_snippetTooltip");
                        var cRight = e.pageX + 12 + $tObj.outerWidth() - $tObj.scrollLeft();
                        var cBottom = e.pageY + 12 + $tObj.outerHeight() - $tObj.scrollTop();
                        var tipX = e.pageX + 12;
                        var tipY = e.pageY + 12;
                        if (cRight >= $(window).width()) {
                            tipX = e.pageX - 12 - $tObj.outerWidth();
                        }
                        if (cBottom >= $(window).height()) {
                            tipY = e.pageY - 12 - $tObj.outerHeight();
                        }
                        $tObj.css("left", tipX).css("top", tipY);
                    });
                }
            });
        },

        // Simple Tooltip plugin
        dj_simpleTooltip: function (className) {
            return this.each(function () {
                var sText = $(this).attr("title");
                $(this).attr("title", "");
                if (sText !== undefined) {
                    $(this).hover(function (e) {
                        var enable = $(this).data('enableSimpleTooltip');
                        if (enable === undefined || enable === true) {
                            $(this).attr("title", "");
                            var tipX = e.pageX + 12;
                            var tipY = e.pageY + 12;
                            var tObj = $("#dj_tooltip").get(0);
                            var $tObj = null;
                            if (tObj) {
                                $(tObj).removeClass()
                                    .addClass(className)
                                    .html(sText);
                            } else {
                                $("body").append("<div id='dj_tooltip' class=\"" +
                                    className + "\" style='position: absolute; z-index: 100; display: none;'><div class=\"border\">" +
                                    sText + "</div></div>");
                            }

                            $tObj = $("#dj_tooltip");
                            $tObj.data("triggeringElement", $(this));
                            if ($.fn.bgiframe) {
                                $tObj.bgiframe();
                            }
                            var cRight = e.pageX + 12 + $tObj.outerWidth() - $dj.getHorizontalScroll();
                            var cBottom = e.pageY + 12 + $tObj.outerHeight() - $dj.getVerticalScroll();
                            tipX = e.pageX + 12;
                            tipY = e.pageY + 12;
                            if (cRight >= $dj.getClientWidth()) {
                                tipX = e.pageX - 12 - $tObj.outerWidth();
                            }
                            if (cBottom >= $dj.getClientHeight()) {
                                tipY = e.pageY - 12 - $tObj.outerHeight();
                            }
                            $tObj.css("left", tipX).css("top", tipY).css("z-index", "1000000");
                            $tObj.stop(true, true).fadeIn("fast");
                        }
                    }, function (e) {
                        var enable = $(this).data('enableSimpleTooltip');
                        if (enable === undefined || enable === true) {
                            $("#dj_tooltip").stop(true, true).fadeOut("fast");
                            $(this).attr("title", sText);
                        }
                    });
                    $(this).mousemove(function (e) {
                        var enable = $(this).data('enableSimpleTooltip');
                        if (enable === undefined || enable === true) {
                            var $tObj = $("#dj_tooltip");
                            if ($tObj && $tObj.length > 0) {
                                var cRight = e.pageX + 12 + $tObj.outerWidth() - $dj.getHorizontalScroll();
                                var cBottom = e.pageY + 12 + $tObj.outerHeight() - $dj.getVerticalScroll();
                                var tipX = e.pageX + 12;
                                var tipY = e.pageY + 12;
                                if (cRight >= $dj.getClientWidth()) {
                                    tipX = e.pageX - 12 - $tObj.outerWidth();
                                }
                                if (cBottom >= $dj.getClientHeight()) {
                                    tipY = e.pageY - 12 - $tObj.outerHeight();
                                }
                                $tObj.css("left", tipX).css("top", tipY);
                            }
                        }
                    });
                }
            });
        },

        showLoading: function () {
            var loadingDiv = this._getLoading();
            try {
                loadingDiv.css({ "height": this.outerHeight(), "width": this.outerWidth(), "left": 0, "top": 0 }).show();
                var l = this.offset().left - loadingDiv.offset().left, t = this.offset().top - loadingDiv.offset().top;
                loadingDiv.css({ "left": l, "top": t });
            } catch (e) {
                if (loadingDiv) {
                    loadingDiv.hide();
                }
            }
        },

        hideLoading: function () {
            var loadingDiv = this._getLoading();
            if (loadingDiv) {
                loadingDiv.hide();
            }
        },

        _getLoading: function () {
            var loadingDiv = this.data('loadingDiv');
            if (!loadingDiv) {
                loadingDiv = $('.loadingProgress', this);

                if (loadingDiv.length === 0) {
                    loadingDiv = $('<div class="loadingProgress"></div>').appendTo(this);
                }

                this.data('loadingDiv', loadingDiv);
            }

            return loadingDiv;
        },

        waterMark: function (options) {
            var $this = null;
            return this.each(function () {
                $this = $(this);
                if ($this.val() === options.waterMarkText || $this.val() === "") {
                    $this.addClass(options.waterMarkClass).val(options.waterMarkText);
                }

                $this.focus(function () {
                    $(this).filter(function () {
                        return $(this).val() === "" || $(this).val() === options.waterMarkText;
                    }).val("").removeClass(options.waterMarkClass);

                })
                    .blur(function () {
                        $(this).filter(function () {
                            return $(this).val() === "";
                        })
                            .addClass(options.waterMarkClass)
                            .val(options.waterMarkText);
                    });

            });
        }
    });
})(DJ, DJ.jQuery, DJ.$dj);