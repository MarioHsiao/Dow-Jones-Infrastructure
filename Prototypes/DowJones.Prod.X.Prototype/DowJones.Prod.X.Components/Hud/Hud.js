/*!
 * Hud
 */

    DJ.UI.Hud = DJ.UI.Component.extend({
        selectors: {
            clipModuleData: "span.dj-clip-module-data",
            clipPopupArea: ".djProdx-hud-popup-clips .djProdx-hud-data",
            moduleClipContainer: ".moduleClipContainer",
            articleClipContainer: ".articleClipContainer",
            moduleButton: ".modules",
            articleButton: ".articles",
            articleResultsArea: ".articleClipContainer .results",
            moduleResultsArea: ".moduleClipContainer .results",
            articleNoResultsArea: ".articleClipContainer .no-results",
            moduleNoResultsArea: ".moduleClipContainer .no-results",
            clipColumn: ".clipColumn"
        },
        
        init: function (element, meta) {
            // Call the base constructor
            var self = this;
            self._super(element, $.extend({ name: "Hud" }, meta));
            self.clipItems = [];
            self._initializeModuleClipsLayoutManager();
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                // TODO: Add delegates
                // e.g.  OnHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
            });
        },

        _initializeElements: function () {
            var self = this;
            self._baseHtml();
            self.$clipModuleData = $(this.selectors.clipModuleData, this.$element);
            self.$clipPopupArea = $(self.selectors.clipPopupArea, self.$element);
            self.$moduleClipContainer = $(self.selectors.moduleClipContainer, self.$element);
            self.$articleClipContainer = $(self.selectors.articleClipContainer, self.$element);
            self.$moduleClipColumns = $(self.selectors.clipColumn, self.$moduleClipContainer);
            self.$moduleClipColumnFirst = $(self.$moduleClipColumns[0]);
            self.$articleBtn = $(self.selectors.articleButton, self.$clipPopupArea);
            self.$articleResultsArea = $(self.selectors.articleResultsArea, self.$clipPopupArea);
            self.$moduleResultsArea = $(self.selectors.moduleResultsArea, self.$clipPopupArea);
            self.$articleNoResultsArea = $(self.selectors.articleNoResultsArea, self.$clipPopupArea);
            self.$moduleNoResultsArea = $(self.selectors.moduleNoResultsArea, self.$clipPopupArea);
            self.$moduleBtn = $(self.selectors.moduleButton, self.$clipPopupArea);
        },

        _initializeEventHandlers: function () {
            var self = this;
            DJ.subscribe('dj.productx.core.addClip', $dj.delegate(this, this._addClip));
            self.$articleBtn.on('click', $dj.delegate(this, this._articleButtonClick));
            self.$moduleBtn.on('click', $dj.delegate(this, this._moduleButtonClick));
            Response.resize($dj.delegate(this, this._resize));
        },
        
        _articleButtonClick: function () {
            var self = this;
            var articleLi = $($(self.$articleBtn).closest('li'));
            var moduleLi = $($(self.$moduleBtn).closest('li'));
            if (!articleLi.hasClass('active')) {
                moduleLi.removeClass('active');
                articleLi.addClass('active');
                self.$moduleClipContainer.hide();
                self.$articleClipContainer.show();
            }
        },
        
        _moduleButtonClick: function () {
            var self = this;
            var articleLi = $($(self.$articleBtn).closest('li'));
            var moduleLi = $($(self.$moduleBtn).closest('li'));
            if (!moduleLi.hasClass('active')) {
                articleLi.removeClass('active');
                moduleLi.addClass('active');
                self.$moduleClipContainer.show();
                self.$articleClipContainer.hide();
            }
        },
        
        _addClip: function (data) {
            var self = this;
            self.clipItems.push(data);
            var val = parseInt(self.$clipModuleData.text(), 10);
            self.$clipModuleData.html(++val);
            
            switch(data.type) {
                case "module":
                    self._addModuleItem(data);
                    break;
                case "article":
                    self._addArticleItem(data);
                    break;
            }
        },
        
        _removeClip: function(data) {
            
        },
        
        _resize: function () {
            
        },

        _addModuleItem: function (data) {
            var self = this;
            var $item = $("<li class='clip'>module-item-" + self.clipItems.length + "</li>");
            self.$moduleClipColumnFirst.append($item);
            self.$moduleResultsArea.show();
            self.$moduleNoResultsArea.hide();
        },
        
        _addArticleItem: function (data) {
            var self = this;
            var $item = $("<li class='clip'>article-item-" + self.clipItems.length + "</li>");
            self.$articleResultsArea.show();
            self.$articleNoResultsArea.hide();
           // self.$artiClipColumnFirst.append($item);
        },
        
        _initializeArticleClipsLayoutManager: function() {
            var self = this;
            
        },

        _initializeModuleClipsLayoutManager: function () {
            var self = this;
            $(self.selectors.clipColumn, self.$moduleClipContainer).sortable({
                axis: false,
                connectWith:  $(self.selectors.clipColumn, self.$moduleClipContainer),
                placeholder: 'clip-placeholder',
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
                    $(self.selectors.clipColumn, self.$moduleClipContainer).sortable('enable');
                },
                scroll: true
            }).disableSelection();
        },
        
        _baseHtml: function () {
            this.$element.html(this.templates.success());
        },

        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_Hud', DJ.UI.Hud);
