
/*
Headline PostProcessing Plugin
*/
(function ($) {

    DJ.UI.HeadlinePostProcessing = DJ.UI.Component.extend({

        selectors: {
            modalContent: 'div.modal-content',
            modalClose: 'p.dj_modal-close',
            doneBtn: 'span.dj_btn-blue',
            cancelBtn: 'span.dj_btn-drk-gray'
        },

        events: {
            onSaveClick: "on.dj.SaveClick",
            onEmailClick: "on.dj.EmailClick",
            onPrintClick: "on.dj.PrintClick",
            onViewArticlesClick: "on.dj.ReadClick"
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "HeadlinePostProcessing" }, meta);


            // Call the base constructor
            this._super(element, $meta);
        },

        initializeDelegates: function () {
            this._super();
        },
        _initializeEventHandlers: function () {
            this.publish(this.events.onEnableActions, this);
            $('span.dj_icon-active-read').click(function (e) {
                //Publish on event
                this.publish(this.events.onViewArticlesClick, this);
                e.stopPropagation();
            });

            $('span.dj_icon-active-email').click($dj.delegate(this, this._showEmailPopup));
            $('span.dj_icon-active-print').click($dj.delegate(this, this._showPrintPopup));
            $('span.dj_icon-active-save').click($dj.delegate(this, this._showSavePopup));
        },


        _showPrintPopup: function () {
            if (!this.$printFormatsContainer) {
                this.$printFormatsContainer = this._getModal("printFormatsContainer",
                                                    $dj.delegate(this, this._onDonePrintClick),
                                                    $dj.delegate(this, this._closeModal, '$printFormatsContainer'),
                                                    "<%= Token('printArticles') %>", "<%= Token('cancel') %>");

                //Create exclusion content
                this.$printFormatsContainer.addClass('dj_HdlnFmtOptions').children(":last").children().children(this.selectors.modalContent)
            .prepend(this.templates.formatOptions({ formatOptions: null, idPrefix: this.$element.attr('id') }));
            }

            this.$printFormatsContainer.overlay({ closeOnEsc: true });
        },

        _showSavePopup: function () {
            if (!this.$saveFormatsContainer) {
                this.$saveFormatsContainer = this._getModal("saveFormatsContainer",
                                                    $dj.delegate(this, this._onDoneSaveClick),
                                                    $dj.delegate(this, this._closeModal, '$saveFormatsContainer'),
                                                    "<%= Token('saveArticles') %>", "<%= Token('cancel') %>");

                //Create exclusion content
                this.$saveFormatsContainer.addClass('dj_SaveFmtOptions').children(":last").children().children(this.selectors.modalContent)
            .prepend(this.templates.saveOptions({ saveOptions: null, idPrefix: this.$element.attr('id') }));
            }

            this.$saveFormatsContainer.overlay({ closeOnEsc: true });
        },


        _showEmailPopup: function () {
            if (!this.$emailFormatsContainer) {
                this.$emailFormatsContainer = this._getModal("emailFormatsContainer",
                                                    $dj.delegate(this, this._onDoneEmailClick),
                                                    $dj.delegate(this, this._closeModal, '$emailFormatsContainer'),
                                                    "<%= Token('emailArticles') %>", "<%= Token('cancel') %>");

                //Create exclusion content
                this.$emailFormatsContainer.addClass('dj_HdlnFmtOptions').children(":last").children().children(this.selectors.modalContent)
            .prepend(this.templates.formatOptions({ formatOptions: null, idPrefix: this.$element.attr('id') }));
            }

            this.$emailFormatsContainer.overlay({ closeOnEsc: true });
        },


        _onDoneEmailClick: function () {

            var selEmailFormat = [];
            this.$emailFormatsContainer.children(":last").children().children(this.selectors.modalContent).find("input:checked")
                .each(function () {
                    selEmailFormat.push($(this).val());
            });
            this.publish(this.events.onEmailClick, selEmailFormat);
            this._closeModal('$emailFormatsContainer');
        },

        _onDonePrintClick: function () {

            var selEmailFormat = [];
            this.$printFormatsContainer.children(":last").children().children(this.selectors.modalContent).find("input:checked")
            .each(function () {
                selEmailFormat.push($(this).val());
            });
            this.publish(this.events.onPrintClick, selEmailFormat);
            this._closeModal('$printFormatsContainer');
        },

        _onDoneSaveClick: function () {

            var selEmailFormat = [];
            this.$saveFormatsContainer.children(":last").children().children(this.selectors.modalContent).find("input:checked")
            .each(function () {
                selEmailFormat.push($(this).val());
            });
            this.publish(this.events.onSaveClick, selEmailFormat);
            this._closeModal('$saveFormatsContainer');
        },

        activateLinks: function (makeActive) {
            if (makeActive) {
                $("span.dj_icon.dj_icon-inactive-read").removeClass("dj_icon-inactive-read").addClass("dj_icon-active-read").hover(function () { $(this).addClass('dj_icon-hover-read'); }, function () { $(this).removeClass('dj_icon-hover-read'); });
                $("span.dj_icon.dj_icon-inactive-save").removeClass("dj_icon-inactive-save").addClass("dj_icon-active-save").hover(function () { $(this).addClass('dj_icon-hover-save'); }, function () { $(this).removeClass('dj_icon-hover-save'); });
                $("span.dj_icon.dj_icon-inactive-print").removeClass("dj_icon-inactive-print").addClass("dj_icon-active-print").hover(function () { $(this).addClass('dj_icon-hover-print'); }, function () { $(this).removeClass('dj_icon-hover-print'); });
                $("span.dj_icon.dj_icon-inactive-email").removeClass("dj_icon-inactive-email").addClass("dj_icon-active-email").hover(function () { $(this).addClass('dj_icon-hover-email'); }, function () { $(this).removeClass('dj_icon-hover-email'); });
                $('span.dj_icon-active-email').click($dj.delegate(this, this._showEmailPopup));
                $('span.dj_icon-active-print').click($dj.delegate(this, this._showPrintPopup));
                $('span.dj_icon-active-save').click($dj.delegate(this, this._showSavePopup));
            }
            else {
                $(".dj_HeadlinePostProcessing  ul.actions li span.dj_icon.dj_icon-active-read").removeClass("dj_icon-active-read").addClass("dj_icon-inactive-read").unbind('mouseenter mouseleave');
                $(".dj_HeadlinePostProcessing  ul.actions li span.dj_icon.dj_icon-active-save").removeClass("dj_icon-active-save").addClass("dj_icon-inactive-save").unbind('mouseenter mouseleave');
                $(".dj_HeadlinePostProcessing  ul.actions li span.dj_icon.dj_icon-active-print").removeClass("dj_icon-active-print").addClass("dj_icon-inactive-print").unbind('mouseenter mouseleave');
                $(".dj_HeadlinePostProcessing  ul.actions li span.dj_icon.dj_icon-active-email").removeClass("dj_icon-active-email").addClass("dj_icon-inactive-email").unbind('mouseenter mouseleave');
            }
        },

        _getModal: function (idSuffix, doneHandler, cancelHandler, title, cancelText, doneText) {
            var id = this.$element.attr("id") + "_" + idSuffix;
            $(this.templates.modalDialog()).attr("id", id).appendTo(this.$element);
            var $modal = $("#" + id);
            var $footer = $modal.children(":last").children().children(this.selectors.modalContent).children(this.selectors.footer);
            var $doneBtn = $footer.children(this.selectors.doneBtn);
            var $cancelBtn = $footer.children(this.selectors.cancelBtn);
            //Done click
            $doneBtn.click(doneHandler);
            //Cancel click
            $cancelBtn.click(cancelHandler);

            if (title) {
                $modal.children(':first').children("h3").html(title);
            }

            if (doneText) {
                $doneBtn.html(doneText);
            }

            if (cancelText) {
                $cancelBtn.html(cancelText);
            }

            return $modal;
        },

        _closeModal: function ($modal) {
            $().overlay.hide("#" + this[$modal].attr("id"));
        }
        
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_HeadlinePostProcessing', DJ.UI.HeadlinePostProcessing);

    $dj.debug('Registered DJ.UI.HeadlinePostProcessing (extends DJ.UI.Component)');

})(jQuery);

