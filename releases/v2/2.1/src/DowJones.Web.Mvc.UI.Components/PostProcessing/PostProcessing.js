
/*
PostProcessing Plugin
*/
(function ($) {

    DJ.UI.PostProcessing = DJ.UI.Component.extend({

        selectors: {
            modalContent: 'div.modal-content',
            modalClose: 'p.dj_modal-close',
            doneBtn: 'span.dj_btn-blue',
            cancelBtn: 'span.dj_btn-drk-gray',
            readIconActive: 'span.dj_icon-active-read',
            saveIconActive: 'span.dj_icon-active-save',
            emailIconActive: 'span.dj_icon-active-email',
            printIconActive: 'span.dj_icon-active-print',
            pressClipsIconActive: 'span.dj_icon-active-pressclips',
            readIcon: 'span.dj_icon-read',
            saveIcon: 'span.dj_icon-save',
            emailIcon: 'span.dj_icon-email',
            printIcon: 'span.dj_icon-print',
            pressClipsIcon: 'span.dj_icon-pressclips',
            selectAll: 'input.selectAll',
            headlineDropDown: '.dj_icon-d-arrow-g',
            headlineDropDownAction: '.select .options li',
            headlineSelectActions: '.dj_selectActions',
            headlineSelectOptions: '.dj_dupSelectOptions'
        },

        events: {
            readClick: 'readClick.dj.PostProcessing',
            saveClick: 'saveClick.dj.PostProcessing',
            printClick: 'printClick.dj.PostProcessing',
            emailClick: 'emailClick.dj.PostProcessing',
            pressClipsClick: 'pressClipsClick.dj.PostProcessing',
            selectAll: 'selectAll.dj.PostProcessing',
            selectHeadlines: 'selectHeadlines.dj.PostProcessing'
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "PostProcessing" }, meta);


            // Call the base constructor
            this._super(element, $meta);
        },

        _initializeDelegates: function () {
            this._super();


        },

        _initializeElements: function (ctx) {
            this.$readIcon = ctx.find(this.selectors.readIcon);
            this.$emailIcon = ctx.find(this.selectors.emailIcon);
            this.$saveIcon = ctx.find(this.selectors.saveIcon);
            this.$printIcon = ctx.find(this.selectors.printIcon);
            this.$pressClipsIcon = ctx.find(this.selectors.pressClipsIcon);
            this.$selectAll = ctx.find(this.selectors.selectAll);
            this.$options = ctx.find(this.selectors.headlineSelectOptions);
        },

        _initializeEventHandlers: function () {
            var self = this;

            this.$element.delegate(this.selectors.headlineSelectActions, {
                'mouseleave': function () {
                    $('body').unbind('mouseup.dj_PostProcessingDupActionsDropDown').bind('mouseup.dj_PostProcessingDupActionsDropDown', function (e) {
                        self.$options.addClass('hide');
                    });
                },
                'mouseenter': function () {
                    $('body').unbind('mouseup.dj_PostProcessingDupActionsDropDown');
                }
            });

            this.$element.delegate(this.selectors.readIconActive, {
                'click': function (e) {
                    self.publish(self.events.readClick);
                    return false;
                },
                'hover': function () {
                    $(this).toggleClass('dj_icon-hover-read');
                }
            });

            this.$element.delegate(this.selectors.saveIconActive, {
                'click': function (e) {
                    self.showSavePopup.apply(self);
                    return false;
                },
                'hover': function () {
                    $(this).toggleClass('dj_icon-hover-save');
                }
            });

            this.$element.delegate(this.selectors.emailIconActive, {
                'click': function (e) {
                    self.showEmailPopup.apply(self);
                    return false;
                },
                'hover': function () {
                    $(this).toggleClass('dj_icon-hover-email');
                }
            });

            this.$element.delegate(this.selectors.printIconActive, {
                'click': function (e) {
                    self.showPrintPopup.apply(self);
                    return false;
                },
                'hover': function () {
                    $(this).toggleClass('dj_icon-hover-print');
                }
            });

            this.$element.delegate(this.selectors.pressClipsIconActive, {
                'click': function (e) {
                    self.publish(self.events.pressClipsClick);
                    return false;
                },
                'hover': function () {
                    $(this).toggleClass('dj_icon-hover-pressclips');
                }
            });

            this.$element.delegate(this.selectors.selectAll, 'click', function () {
                self.publish(self.events.selectAll, { select: this.checked });
            });

            this.$element.delegate(this.selectors.headlineDropDown, 'click', function (ev) {
                self.$options.removeClass('hide');
                return false;
            });

            this.$element.delegate(this.selectors.headlineDropDownAction, 'click', function (ev) {
                var $this = $(this);
                    
                //Uncheck both the radio buttons
                $this.addClass('active')

                .siblings('li').removeClass('active');

                self.$selectAll.attr('checked', true);
                self.publish(self.events.selectHeadlines, { select: true, selectDups: $this.data('selectdups') });

                // hide the dropdown menu
                self.$options.addClass('hide');

                // some cleanup
                $('body').unbind('mouseup.dj_PostProcessingDupActionsDropDown');

                //Not required to return false. Also if we return false IE7 do not set the radio button checked.
                //return false;
                ev.stopPropagation();
            });


        },

        showPrintPopup: function (data) {
            if (!this.$printFormatsContainer) {
                this.$printFormatsContainer = this._getModal("printFormatsContainer",
                                                    $dj.delegate(this, this._onDonePrintClick),
                                                    $dj.delegate(this, this._closeModal, '#' + this.element.id + '_printFormatsContainer'),
                                                    '<%= Token("printArticles") %>', '<%= Token("cancel") %>');

                //Create exclusion content
                this.$printFormatsContainer.addClass('dj_HdlnFmtOptions').children(":last").children().children(this.selectors.modalContent)
            .prepend(this.templates.formatOptions({ name: "printhdlnFmtOptions" }));
                this.$printFormatsContainer.find("input:radio:eq(2)").closest("li").hide();
            }
            if (data === null || data === undefined) {
                this.$printFormatsContainer.data("content", null);
            }
            else {
                this.$printFormatsContainer.data("content", data);
            }

            this.$printFormatsContainer.overlay({ closeOnEsc: true });
        },

        showSavePopup: function (data) {
            if (!this.$saveFormatsContainer) {
                this.$saveFormatsContainer = this._getModal("saveFormatsContainer",
                                                    $dj.delegate(this, this.showSaveContentOptions),
                                                    $dj.delegate(this, this._closeModal, '#' + this.element.id + '_saveFormatsContainer'),
                                                    '<%= Token("saveArticles") %>', '<%= Token("cancel") %>');

                //Create exclusion content
                this.$saveFormatsContainer.addClass('dj_SaveFmtOptions').children(":last").children().children(this.selectors.modalContent)
            .prepend(this.templates.saveOptions({ saveOptions: null, idPrefix: this.element.id }));
            }
            if (data === null || data === undefined) {
                this.$saveFormatsContainer.data("content", null);
            }
            else {
                this.$saveFormatsContainer.data("content", data);
            }
            this.$saveFormatsContainer.overlay({ closeOnEsc: true });
        },

        showSaveContentOptions: function (data) {
            this._closeModal(this.$saveFormatsContainer.selector);
            if (!this.$saveContentOptionsContainer) {
                this.$saveContentOptionsContainer = this._getModal("saveContentOptionsContainer",
                                                    $dj.delegate(this, this._onDoneSaveClick),
                                                    $dj.delegate(this, this._closeModal, '#' + this.element.id + '_saveContentOptionsContainer'),
                                                    '<%= Token("saveArticles") %>', '<%= Token("cancel") %>');

                //Create exclusion content
                this.$saveContentOptionsContainer.addClass('dj_HdlnFmtOptions').children(":last").children().children(this.selectors.modalContent)
            .prepend(this.templates.formatOptions({ name: "savehdlnFmtOptions" }));
            }

            this.$saveContentOptionsContainer.overlay({ closeOnEsc: true });
        },

        showEmailPopup: function (data) {
            if (!this.$emailFormatsContainer) {
                this.$emailFormatsContainer = this._getModal("emailFormatsContainer",
                                                    $dj.delegate(this, this._onDoneEmailClick),
                                                    $dj.delegate(this, this._closeModal, '#' + this.element.id + '_emailFormatsContainer'),
                                                    '<%= Token("emailArticles") %>', '<%= Token("cancel") %>');

                //Create exclusion content
                this.$emailFormatsContainer.addClass('dj_HdlnFmtOptions').children(":last").children().children(this.selectors.modalContent)
            .prepend(this.templates.formatOptions({ name: "emailhdlnFmtOptions" }));
                this.$emailFormatsContainer.find("input:radio:eq(2)").closest("li").hide();
            }
            if (data === null || data === undefined) {
                this.$emailFormatsContainer.data("content", null);
            }
            else {
                this.$emailFormatsContainer.data("content", data);
            }
            this.$emailFormatsContainer.overlay({ closeOnEsc: true });
        },

        showEntityPopup: function (data) {
            if (!this.$entityInfoContainer) {
                this.$entityInfoContainer = this._getModal("entityInfoContainer",
                                                   null,
                                                    $dj.delegate(this, this._closeModal, '#' + this.element.id + '_entityInfoContainer'),
                                                    '', '');

                //Create exclusion content
                this.$entityInfoContainer.addClass('dj_HdlnFmtOptions').children(":last").children().children(this.selectors.modalContent)
            .prepend(this.templates.formatOptions({ formatOptions: null, idPrefix: this.element.id }));
            }

            this.$entityInfoContainer.overlay({ closeOnEsc: true });
        },


        _onDoneEmailClick: function (data) {
            if (data.preventDefault) { data = null; }       // don't need the event object
            if (data == null && (this.$emailFormatsContainer && this.$emailFormatsContainer.data("content") != null))
                data = this.$emailFormatsContainer.data("content");
            this.publish(this.events.emailClick, { format: this.$emailFormatsContainer.find("input:checked").val(), data: data });
            this._closeModal(this.$emailFormatsContainer.selector);
        },

        _onDonePrintClick: function (data) {
            if (data.preventDefault) { data = null; }       // don't need the event object
            if (data == null && (this.$printFormatsContainer && this.$printFormatsContainer.data("content") != null))
                data = this.$printFormatsContainer.data("content");
            this.publish(this.events.printClick, { format: this.$printFormatsContainer.find("input:checked").val(), data: data });
            this._closeModal(this.$printFormatsContainer.selector);
        },

        _onDoneSaveClick: function (data) {
            if (data.preventDefault) { data = null; }       // don't need the event object
            if (data == null && (this.$saveFormatsContainer && this.$saveFormatsContainer.data("content") != null))
                data = this.$saveFormatsContainer.data("content");
            this.publish(this.events.saveClick, { format: this.$saveFormatsContainer.find("input:checked").val(), hdlnFormat: this.$saveContentOptionsContainer.find("input:checked").val(), data: data });
            this._closeModal(this.$saveContentOptionsContainer.selector);
        },

        activateLinks: function (makeActive) {
            if (makeActive) {
                this.$readIcon.removeClass("dj_icon-inactive-read").addClass("dj_icon-active-read");
                this.$saveIcon.removeClass("dj_icon-inactive-save").addClass("dj_icon-active-save");
                this.$printIcon.removeClass("dj_icon-inactive-print").addClass("dj_icon-active-print");
                this.$emailIcon.removeClass("dj_icon-inactive-email").addClass("dj_icon-active-email");
                this.$pressClipsIcon.removeClass("dj_icon-inactive-pressclips").addClass("dj_icon-active-pressclips");
            }
            else {
                this.$readIcon.removeClass("dj_icon-active-read").addClass("dj_icon-inactive-read");
                this.$saveIcon.removeClass("dj_icon-active-save").addClass("dj_icon-inactive-save");
                this.$printIcon.removeClass("dj_icon-active-print").addClass("dj_icon-inactive-print");
                this.$emailIcon.removeClass("dj_icon-active-email").addClass("dj_icon-inactive-email");
                this.$pressClipsIcon.removeClass("dj_icon-active-pressclips").addClass("dj_icon-inactive-pressclips");
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

        _closeModal: function (selector) {
            $().overlay.hide(selector);
        }

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_PostProcessing', DJ.UI.PostProcessing);

    $dj.debug('Registered DJ.UI.PostProcessing (extends DJ.UI.Component)');

} (jQuery));



