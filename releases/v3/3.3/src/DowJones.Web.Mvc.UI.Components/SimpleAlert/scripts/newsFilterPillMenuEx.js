(function ($) {

    $.newsFilterPillMenuEx = function (element, options) {

        var contextMnuSelected = false;
        var popupId = "";
        var defaults =
        {
            popupId: 'fl_pillscontextmenuex',
            removeFilter: null,
            excludeFilter: null,
            includeFilter: null
        };

        var menu = this;
        menu.settings = {};

        var $element = $(element),
            element = element;

        menu.init = function () {
            //alert("initializing..."); 
            menu.settings = $.extend({}, defaults, options);
            popupId = "#" + menu.settings.popupId;
            var pillscontextclassie7 = (($.browser.msie) && ($.browser.version == '7.0')) ? 'pillscontextmenuie7' : '';

            if ($(popupId).size() == 0) {
                $(document.body).append('<div id="' + menu.settings.popupId + '" class="overlaypillscontextmenu ' + pillscontextclassie7 + '"></div>');
            }
            var maxZindex = getMaxZIndex() + 10;
            $(popupId).css("z-index", maxZindex);

            $element.delegate(".addPillOptionsEx", 'mouseout', function () { setTimeout(function () { hideContextMnu(); }, 800); contextMnuSelected = false; });
            $element.delegate(".addPillOptionsEx", 'mouseover', function () { contextMnuSelected = true; });
            $element.delegate(".addPillOptionsEx", 'click', function () { return menu.pillOnClick(this); });
        }

        menu.pillOnClick = function (ele) {

            var $ele = $(ele);
            var arrHtml = [];

            arrHtml[arrHtml.length] = '<div class="pillOptionsList">';

            switch ($ele.attr('mode')) {
                case 'not':
                    arrHtml[arrHtml.length] = ' <div style="cursor: pointer" pilloption="And" class="pillOption and"><span>' + ' <%=Token("andLabel")%>' + '</span></div>';
                    break;
                case 'and':
                    arrHtml[arrHtml.length] = ' <div style="cursor: pointer" pilloption="Not" class="pillOption not"><span>' + '<%=Token("notLabel")%>' + '</span></div>';
                    break;
            }

            arrHtml[arrHtml.length] = ' <div style="cursor: pointer" pilloption="Remove" class="pillOption remove"><span>' + '<%=Token("removeLabel")%>' + '</span></div>';
            arrHtml[arrHtml.length] = '</div>';

            $(popupId).html(arrHtml.join(''));

            $(popupId + ' .pillOption').hover(
                function () {
                    $(this).toggleClass('pillOptionOver');
                },
                function () {
                    $(this).toggleClass('pillOptionOver');
                }
            );

            $(popupId + ' .pillOption').each(function () {
                $(this).click(function () {
                    $(popupId).hide();
                    switch ($(this).attr('pilloption')) {
                        case 'Remove':
                            if (menu.settings.removeFilter && typeof (menu.settings.removeFilter) == 'function')
                                menu.settings.removeFilter.call(this, $ele);
                            break;
                        case 'And':
                            if (menu.settings.includeFilter && typeof (menu.settings.includeFilter) == 'function')
                                menu.settings.includeFilter.call(this, $ele);
                            break;
                        case 'Not':
                            if (menu.settings.excludeFilter && typeof (menu.settings.excludeFilter) == 'function')
                                menu.settings.excludeFilter.call(this, $ele);
                            break;
                    }

                });

            });

            positionPopup(popupId, $ele.offset(), $ele.width());

        }

        var positionPopup = function (popupId, offset, width) {
            var popup = $(popupId);
            var hideDelay = 500;
            var hideDelayTimer;

            popup.css({ top: offset.top + 20, left: offset.left + width - popup.width(), display: 'block' });
            var maxZindex = getMaxZIndex();
            popup.css("z-index", maxZindex + 1);
            popup.mouseover(function (e) {
                contextMnuSelected = true;
            });
            popup.mouseout(function () {
                setTimeout(function () {
                    hideContextMnu();
                }, 800);
                contextMnuSelected = false;
            });
        }

        var getMaxZIndex = function () {
            var maxZ = Math.max.apply(null, $.map($('body > *'), function (e, n) {
                if ($(e).css('position') == 'absolute') {
                    return parseInt($(e).css('z-index')) || 1;
                }
                else
                    return 1000;
            })
            );
            return maxZ;
        }

        var hideContextMnu = function () {
            if (!contextMnuSelected) {
                if ($(popupId).size() > 0) {
                    setTimeout(function () {
                        $(popupId).hide();
                    }, 800);
                }

            }
        }

        menu.init();

    }

    $.fn.newsFilterPillMenuEx = function (options) {

        return this.each(function () {
            if (undefined == $(this).data('newsFilterPillMenuEx')) {
                var plugin = new $.newsFilterPillMenuEx(this, options);
                $(this).data('newsFilterPillMenuEx', plugin);
            }
        });

    }

    
})(jQuery);