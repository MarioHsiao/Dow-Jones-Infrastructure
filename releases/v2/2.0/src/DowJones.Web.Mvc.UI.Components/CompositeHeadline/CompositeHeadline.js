/*!  CompositeHeadline  */

(function ($) {

    DJ.UI.CompositeHeadline = DJ.UI.CompositeComponent.extend({

        selectors: {
            postProcessing: '.dj_PostProcessing',
            headlineList: '.dj_HeadlineListControl',
            headlineOptions: '.dj_HeadlineOptions .controls .drop-down-button .selected-option',
            headlineOptionsModal: '.dj_HeadlineOptions .controls .drop-down-button .selected-option .options',
            pagerPrev: '.dj_prev',
            pagerNext: '.dj_next',
            optionsOk: '.dj_optionsOk',
            dupSwitch: '.switch',
            optionsMenu: '.dj_options',
            optionItem: '.dj_options .item'

        },

        events: {
            pagerClick: 'pagerClick.dj.CompositeHeadline',
            headlineClick: 'headlineClick.dj.CompositeHeadline',
            optionChange: 'optionChange.dj.CompositeHeadline',
            postProcessingClick: 'postProcessingClick.dj.CompositeHeadline',
            entityClick: 'entityClick.dj.CompositeHeadline'

        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "CompositeHeadline" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this._initializeComponents();

            // state bag for headlines selection
            this._selectedHeadlines = [];

            this._activateToggle();
        },

        _initializeElements: function (ctx) {
            this.$postProcessing = ctx.find(this.selectors.postProcessing);
            this.$headlineList = ctx.find(this.selectors.headlineList);
            this.$headlineOptionsModal = ctx.find(this.selectors.headlineOptionsModal);
            this.$headlineOptions = ctx.find(this.selectors.headlineOptions);
        },

        _initializeComponents: function () {
            if (this.$postProcessing.length !== 0) {
                this.postProcessingComp = this.$postProcessing.findComponent(DJ.UI.PostProcessing);
                if (this.postProcessingComp) {
                    this.postProcessingComp.setOwner(this);
                }
            }

            if (this.$headlineList.length !== 0) {
                this.headlineListComp = this.$headlineList.findComponent(DJ.UI.HeadlineList);
                if (this.headlineListComp) {
                    this.headlineListComp.setOwner(this);
                }
            }

        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                onHeadlineCheck: $dj.delegate(this, this._onHeadlineCheck),
                onHeadlineClick: $dj.delegate(this, this._onHeadlineClick),
                onPrintClick: $dj.delegate(this, this._onPrintClick),
                onEmailClick: $dj.delegate(this, this._onEmailClick),
                onSaveClick: $dj.delegate(this, this._onSaveClick),
                onSelectAllCheck: $dj.delegate(this, this._onSelectAllCheck),
                onEntityClick: $dj.delegate(this, this._onEntityClick)
            });
        },

        _initializeEventHandlers: function () {
            var self = this;
            this.subscribe('headlineCheck.dj.HeadlineList', this._delegates.onHeadlineCheck);
            this.subscribe('headlineClick.dj.HeadlineList', this._delegates.onHeadlineClick);
            this.subscribe('printClick.dj.HeadlineList', this._delegates.onPrintClick);
            this.subscribe('saveClick.dj.HeadlineList', this._delegates.onSaveClick);
            this.subscribe('emailClick.dj.HeadlineList', this._delegates.onEmailClick);

            this.subscribe('readClick.dj.PostProcessing', $dj.delegate(this, this._onPostProcessingClick, 'read'));
            this.subscribe('saveClick.dj.PostProcessing', $dj.delegate(this, this._onPostProcessingClick, 'save'));
            this.subscribe('emailClick.dj.PostProcessing', $dj.delegate(this, this._onPostProcessingClick, 'email'));
            this.subscribe('printClick.dj.PostProcessing', $dj.delegate(this, this._onPostProcessingClick, 'print'));
            this.subscribe('selectAll.dj.PostProcessing', this._delegates.onSelectAllCheck);
            this.subscribe('selectHeadlines.dj.PostProcessing', this._delegates.onSelectAllCheck);

            this.subscribe('entityClick.dj.HeadlineList', this._delegates.onEntityClick);

            //Using bind method instead of delegate for all the below even handlers because we are moving these elements outside 
            //the composite component on headline scroll to maintain there position

            this.$element.find(this.selectors.headlineOptions).bind('click', function (e) {
                self._toggleOptionsMenu(true);
                e.stopPropagation();
            });

            this.$element.find(this.selectors.optionItem).bind('click', function (e) {
                e.stopPropagation();
            });

            this.$element.find(this.selectors.optionsMenu).bind({
                'mouseleave': function () {
                    $('body').unbind('mouseup.dj_CompositeHeadlineOptionsDropDown')
                             .bind('mouseup.dj_CompositeHeadlineOptionsDropDown', function (e) {
                                 self._toggleOptionsMenu(false);
                             });
                },
                'mouseenter': function () {
                    $('body').unbind('mouseup.dj_CompositeHeadlineOptionsDropDown');
                }
            });

            this.$element.find(this.selectors.pagerPrev).bind('click', function () {
                self.publish(self.events.pagerClick, { page: 'prev' });
                return false;
            });

            this.$element.find(this.selectors.pagerNext).bind('click', function () {
                self.publish(self.events.pagerClick, { page: 'next' });
                return false;
            });

            this.$element.find(this.selectors.optionsOk).bind('click', function () {
                // var view = $('input[type=radio][name=view]:checked').val(),
                //   format = $('input[type=radio][name=articleFormat]:checked').val(),
                var sort = $('input[type=radio][name=order]:checked').val(),
                    showDups = self.$element.find(self.selectors.dupSwitch).data("state");

                // hide menu
                self._toggleOptionsMenu(false);

                // some cleanup
                $('body').unbind('mouseup.dj_CompositeHeadlineOptionsDropDown');

                // self.publish(self.events.optionChange, { view: view, articleformat: format, sort: sort, showDuplicates: showDups });
                self.publish(self.events.optionChange, { sort: sort, showDuplicates: showDups });
                return false;
            });
        },

        _onPostProcessingClick: function (cmd, postProcessingData) {
            if (cmd !== 'read' && !postProcessingData) {
                $dj.debug('postProcessingData is null. Cannot proceed with publishing event.');
                return;
            }

            var headlinesData = (postProcessingData && postProcessingData.data) ? [postProcessingData.data] : this.getSelectedHeadlines(),
                eventInfo = { command: cmd, headlines: headlinesData };

            if (cmd !== 'read') { eventInfo.format = postProcessingData.format, eventInfo.hdlnFormat = postProcessingData.hdlnFormat };

            this.publish(this.events.postProcessingClick, eventInfo);
        },

        _onSelectAllCheck: function (args) {
            if (!this.headlineListComp) { $dj.debug('Couldn\'t find Simple Headline ist component'); return; }

            this._enablePostProcessing(args.select || false);
            this.headlineListComp.toggleAllHeadlines(args.select, args.selectDups);

            this._selectedHeadlines = this.headlineListComp.getSelectedHeadlinesData();
        },

        _onHeadlineCheck: function (data) {
            var checked = (data && data.checked) || false;

            this._updateHeadlineSelections(data);

            var enablePP = this._selectedHeadlines && this._selectedHeadlines.length > 0 || false;
            this._enablePostProcessing(enablePP);


        },

        _onHeadlineClick: function (data) {
            this.publish(this.events.headlineClick, { headline: data });
        },

        _onPrintClick: function (data) {
            if (this.postProcessingComp) {
                this.postProcessingComp.showPrintPopup(data);
            }
        },


        _onEmailClick: function (data) {
            if (this.postProcessingComp) {
                this.postProcessingComp.showEmailPopup(data);
            }
        },

        _onSaveClick: function (data) {
            if (this.postProcessingComp) {
                this.postProcessingComp.showSavePopup(data);
            }
        },

        _activateToggle: function () {
            $('.dj_toggle-switch').toggle(function () {
                var $this = $(this),
                    tbl = $this.find('.text-behind .tb-right').html();
                $this.children('.switch').addClass('on').html(tbl).data('state', 'off');

            }, function () {
                var $this = $(this),
                    tbr = $this.find('.text-behind .tb-left').html();
                $this.children('.switch').removeClass('on').html(tbr).data('state', 'on');
            });
        },

        _updateHeadlineSelections: function (args) {
            if (!args) { $dj.debug('Headline selection couldn\'t be updated as data is null or undefined'); return; }

            var checked = args.checked || false,
                guids = _.pluck(this._selectedHeadlines, "guid"),
                foundIndex = _.indexOf(guids, args.data.guid),
                found = foundIndex !== -1;

            if (checked) {
                if (!found) {
                    this._selectedHeadlines.push(args.data);     // add
                }
                else {
                    this._selectedHeadlines[foundIndex] = args.data;     // update
                }
            }
            else {
                if (found) {
                    this._selectedHeadlines.splice(foundIndex, 1);      // remove
                }
            }
        },

        _enablePostProcessing: function (enable) {
            if (this.postProcessingComp) {
                this.postProcessingComp.activateLinks(enable || false);
            }
        },

        _toggleOptionsMenu: function (show) {
            if (show) {
                this.$headlineOptionsModal.addClass('active');
                this.$headlineOptions.addClass('active');
            }
            else {
                this.$headlineOptionsModal.removeClass('active');
                this.$headlineOptions.removeClass('active');
            }
        },

        getSelectedHeadlines: function () {
            return this._selectedHeadlines;
        },

        _onEntityClick: function (data) {
            this.publish(this.events.entityClick, data);
        }

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_CompositeHeadline', DJ.UI.CompositeHeadline);

    $dj.debug('Registered DJ.UI.CompositeHeadline (extends DJ.UI.CompositeComponent)');

} (jQuery));
