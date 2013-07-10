dj.productx.core = dj.productx.core || {};

dj.productx.core.autoSuggestHandler = function (data) {
    var searchUrl = dj.productx.core.config.baseAppUrl;
    switch (data.controlType.toLowerCase()) {
        case 'company':
            searchUrl += "company/search?query=fds:" + data.code;
            break;
        case 'industry':
            searchUrl += "industry/search?query=in:" + data.code;
            break;
        case 'executive':
            searchUrl += "executive/search?query=pe:" + data.code;
            break;
        case 'author':
            searchUrl += "author/search?query=au:" + data.code;
            break;
        case 'newssubject':
            searchUrl += "newssubject/search?query=ns:" + data.code;
            break;
        case 'region_all':
            searchUrl += "region/search?query=re:" + data.code;
            break;
        case 'keyword':
        default:
            searchUrl += "keyword/search?query=" + data.word;
            break;
    }
    window.location = searchUrl;
};

dj.productx.core.init = function () {
    var searchUrlInt = "http://suggest.int.factiva.com/Search/1.0";

    // add the autosuggest control
    var categorySuggest = {
        url: searchUrlInt,
        controlId: "autosuggestSearchbox",
        controlClassName: "djAutoComplete",
        autocompletionType: "Categories",
        useSessionId: DJ.config.credentials.token,
        onItemSelect: dj.productx.core.autoSuggestHandler,
        fillInputOnKeyUpDown: "true",
        selectFirst: false,
        tokens: { 'region_allTkn': 'Regions', 'company_allTkn': 'Companies' },
        options: { "maxResults": 3, "interfaceLanguage": "en", "categories": "company|executive|industry|newssubject|region_all|keyword", "companyFilterSet": "newsCodedAbt", "executiveFilterSet": "newsCoded" }
    };
    if (window.djV3) {
        window.djV3.web.widgets.autocomplete(categorySuggest);
    }

    $(function () {
        return $(".search-button-trigger").click(function () {
            return $(".search-bar-nav").toggleClass("open");
        });
    });
    
    $(function () {
        return $(".search-button-trigger").click(function () {
            return $(".search-bar-nav").toggleClass("open");
        });
    });

    $(function () {
        return $('#searchForm').submit(function (e) {
            var query = $('#autosuggestSearchbox').val();
            dj.productx.core.autoSuggestHandler({ 'controlType': 'keyword', 'word': query });
            return false;
        });
    });
};


dj.productx.core.iDashboard = {

    jQuery: window['jQuery'],

    settings: {
        columns: '.column',
        widgetSelector: '.widget',
        handleSelector: '.widget-head',
        contentSelector: '.widget-content',
        widgetDefault: {
            movable: true,
            removable: true,
            collapsible: true,
            editable: true,
            colorClasses: ['color-yellow', 'color-red', 'color-blue', 'color-white', 'color-orange', 'color-green']
        },
        widgetIndividual: {
            intro: {
                movable: false,
                removable: false,
                collapsible: false,
                editable: false
            },
            companyExplainer: {
                movable: false,
                removable: false,
                collapsible: false,
                editable: false
            },
            topStories: {
                movable: false,
                removable: false,
                collapsible: false,
                editable: false
            },
            liveNews: {
                movable: false,
                removable: true,
                collapsible: false,
                editable: true
            },
            filters: {
                movable: false,
                removable: false,
                collapsible: false,
                editable: false
            },
            gallery: {
                colorClasses: ['color-yellow', 'color-red', 'color-white']
            }
        }
    },

    init: function () {
        this.addWidgetControls();
        this.makeSortable();
    },

    getWidgetSettings: function (id) {
        var $ = this.jQuery,
            settings = this.settings;
        return (id && settings.widgetIndividual[id]) ? $.extend({}, settings.widgetDefault, settings.widgetIndividual[id]) : settings.widgetDefault;
    },

    addWidgetControls: function () {
        var djGrid = this,
            $ = this.jQuery,
            settings = this.settings;

        $(settings.widgetSelector).each(function () {
            var thisWidgetSettings = djGrid.getWidgetSettings(this.id);
            if (thisWidgetSettings.removable) {
                $('<a href="javascript:void()" class="remove pull-right dj_action"><i class="icon-remove"></i></a>').mousedown(function (e) {
                    e.stopPropagation();
                }).click(function () {
                    if (confirm('This widget will be removed, ok?')) {
                        $(this).parents(settings.widgetSelector).animate({
                            opacity: 0
                        }, function () {
                            $(this).wrap('<div/>').parent().slideUp(function () {
                                $(this).remove();
                            });
                        });
                    }
                    return false;
                }).appendTo($(settings.handleSelector, this));
            }

            if (thisWidgetSettings.editable) {
                
                $('<a href="javascript:void()" class="edit pull-right dj_action"><i class="icon-cog"></i></a>').mousedown(function (e) {
                    e.stopPropagation();
                }).toggle(function () {
                    $(this)
                        .parents(settings.widgetSelector)
                        .find('.edit-box').show().find('input').focus();
                    $("i", this)
                       .removeClass('icon-cog')
                       .addClass('icon-ok-sign');
                    return false;
                }, function () {
                    $(this)
                        .parents(settings.widgetSelector)
                        .find('.edit-box').hide();
                    $("i", this)
                       .removeClass('icon-ok-sign')
                       .addClass('icon-cog');
                    return false;
                }).appendTo($(settings.handleSelector, this));
                $('<div class="edit-box" style="display:none;"/>')
                    .append('<ul><li class="item"><label>Change the title?</label><input value="' + $('h3', this).text() + '"/></li>')
                    /*.append((function () {
                        var colorList = '<li class="item"><label>Available colors:</label><ul class="colors">';
                        $(thisWidgetSettings.colorClasses).each(function () {
                            colorList += '<li class="' + this + '"/>';
                        });
                        return colorList + '</ul>';
                    })())*/
                    .append('</ul>')
                    .insertAfter($(settings.handleSelector, this));
            }

            if (thisWidgetSettings.collapsible) {
                $('<a href="javascript:void()" class="chevron pull-left dj_action"><i class="icon-chevron-down"></i></a>').mousedown(function (e) {
                    e.stopPropagation();
                }).toggle(function () {
                    $(this)
                        .parents(settings.widgetSelector)
                        .find(settings.contentSelector)
                        .hide();
                    $("i", this)
                        .removeClass('icon-chevron-down')
                        .addClass('icon-chevron-up');
                    return false;
                }, function () {
                    $(this)
                        .parents(settings.widgetSelector)
                        .find(settings.contentSelector)
                        .show();
                    $("i", this)
                       .removeClass('icon-chevron-up')
                       .addClass('icon-chevron-down');
                        DJ.publish("dj.productx.core.collapseFired");
                    return false;
                }).prependTo($(settings.handleSelector, this));
            }
        });

        $('.edit-box').each(function () {
            $('input', this).keyup(function () {
                $(this).parents(settings.widgetSelector).find('h3').text($(this).val().length > 20 ? $(this).val().substr(0, 20) + '...' : $(this).val());
            });
            $('ul.colors li', this).click(function () {

                var colorStylePattern = /\bcolor-[\w]{1,}\b/,
                    thisWidgetColorClass = $(this).parents(settings.widgetSelector).attr('class').match(colorStylePattern);
                if (thisWidgetColorClass) {
                    $(this).parents(settings.widgetSelector)
                        .removeClass(thisWidgetColorClass[0])
                        .addClass($(this).attr('class').match(colorStylePattern)[0]);
                }
                return false;

            });
        });

    },

    makeSortable: function () {
        var djGrid = this,
            $ = this.jQuery,
            settings = this.settings,
            $widgetItems = (function() {
                return $(settings.widgetSelector);
            })(),
            $sortableItems = (function () {
                var notSortable = '';
                $(settings.widgetSelector, $(settings.columns)).each(function (i) {
                    if (!djGrid.getWidgetSettings(this.id).movable) {
                        if (!this.id) {
                            this.id = 'widget-no-id-' + i;
                        }
                        notSortable += '#' + this.id + ',';
                    }
                });

                if (notSortable.length > 0) {
                    return $('> li:not(' + notSortable + ')', settings.columns);
                }
                return $('> li', settings.columns);
            })();

        $sortableItems.find(settings.handleSelector).css({
            cursor: 'move'
        }).mousedown(function (e) {
            $sortableItems.css({ width: '' });
            $(this).parent().css({
                width: $(this).parent().width() + 'px'
            });
        }).mouseup(function () {
            if (!$(this).parent().hasClass('dragging')) {
                $(this).parent().css({ width: '' });
            } else {
                $(settings.columns).sortable('disable');
            }
        });
        
        $widgetItems.find(settings.handleSelector).mouseover(function () {
            $(this).parent().addClass("dj_displayActions");
        }).mouseout(function () {
            $(this).parent().removeClass("dj_displayActions");
        });
        
        $(settings.columns).sortable({
            axis: false,
            items: $sortableItems,
            connectWith: $(settings.columns),
            handle: settings.handleSelector,
            placeholder: 'widget-placeholder',
            forcePlaceholderSize: true,
            revert: 300,
            delay: 100,
            opacity: 0.8,
            containment: false,
            start: function (e, ui) {
                $(ui.helper).addClass('dragging');
            },
            stop: function (e, ui) {
                $(ui.item).css({ width: '' }).removeClass('dragging');
                $(settings.columns).sortable('enable');
            },
            update: function (e, ui) {
                DJ.publish("dj.productx.core.widgetSorted", ui);
            },
            scroll: true
        });
    }

};
