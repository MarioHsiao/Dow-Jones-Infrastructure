dj.productx.core = dj.productx.core || {};

dj.productx.core.init = function () {
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
        var iNettuts = this,
            $ = this.jQuery,
            settings = this.settings;

        $(settings.widgetSelector, $(settings.columns)).each(function () {
            var thisWidgetSettings = iNettuts.getWidgetSettings(this.id);
            if (thisWidgetSettings.removable) {
                $('<a href="javascript:void()" class="remove pull-right"><i class="icon-remove"></i></a>').mousedown(function (e) {
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
                
                $('<a href="javascript:void()" class="edit pull-right"><i class="icon-cog"></i></a>').mousedown(function (e) {
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
                    .append((function () {
                        var colorList = '<li class="item"><label>Available colors:</label><ul class="colors">';
                        $(thisWidgetSettings.colorClasses).each(function () {
                            colorList += '<li class="' + this + '"/>';
                        });
                        return colorList + '</ul>';
                    })())
                    .append('</ul>')
                    .insertAfter($(settings.handleSelector, this));
            }

            if (thisWidgetSettings.collapsible) {
                $('<a href="javascript:void()" class="chevron pull-left"><i class="icon-chevron-down"></i></a>').mousedown(function (e) {
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
        var iNettuts = this,
            $ = this.jQuery,
            settings = this.settings,
            $sortableItems = (function () {
                var notSortable = '';
                $(settings.widgetSelector, $(settings.columns)).each(function (i) {
                    if (!iNettuts.getWidgetSettings(this.id).movable) {
                        if (!this.id) {
                            this.id = 'widget-no-id-' + i;
                        }
                        notSortable += '#' + this.id + ',';
                    }
                });
                return $('> li:not(' + notSortable + ')', settings.columns);
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

        $(settings.columns).sortable({
            items: $sortableItems,
            connectWith: $(settings.columns),
            handle: settings.handleSelector,
            placeholder: 'widget-placeholder',
            forcePlaceholderSize: true,
            revert: 300,
            delay: 100,
            opacity: 0.8,
            containment: 'document',
            start: function (e, ui) {
                $(ui.helper).addClass('dragging');
            },
            stop: function (e, ui) {
                $(ui.item).css({ width: '' }).removeClass('dragging');
                $(settings.columns).sortable('enable');
            }
        });
    }

};
