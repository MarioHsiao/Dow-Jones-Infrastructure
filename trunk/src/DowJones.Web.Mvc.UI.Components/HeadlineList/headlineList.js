/* 
DJ.UI.HeadlineList plugin
*/

    DJ.UI.HeadlineList = DJ.UI.Component.extend({
        selectors: {
            noResultSpan: 'span.dj_noResults',
            headlineBox: '.headline-box',
            title: '.title',
            headlineItem: '.headlineItem',
            headlineTitle: '.headline-box .title',
            dupWrapper: '.duplicateWrapper',
            dupWrapperHeader: '.duplicateWrapper p',
            icon: '.dj_icon',
            headlineCheckbox: 'input.dj_ChkHeadline',
            dupHeadlineCheckbox: 'input.dj_dupChkHeadline',
            printIcon: '.dj_icon-print',
            saveIcon: '.dj_icon-save',
            emailIcon: '.dj_icon-mail',
            pressClipsIcon: '.dj_icon-pressclips',
            source: '.source',
            author: '.author',
            headlineSelectOptions: '.dj_dupSelectOptions'
        },

        constants: {
            blackArrowExpanded: 'dj_icon-blk-filter-arrow',
            blackArrowCollapsed: 'dj_icon-blk-filter-r-arrow'
        },

        defaults: {
            showDuplicates: false,
            showCheckboxes: false,
            showSortOptions: false,
            headlineClickable: true,
            sortOption: 0,
            showAuthorOptions: false,
            showSourceOptions: false,
            sourceOptions: [],
            showPaging: false,
            pageSize: 0
        },

        events: {
            headlineClick: 'headlineClick.dj.HeadlineList',
            headlineCheck: 'headlineCheck.dj.HeadlineList',
            headlineSortChanged: 'sortChanged.dj.HeadlineList',
            printClick: 'printClick.dj.HeadlineList',
            saveClick: 'saveClick.dj.HeadlineList',
            emailClick: 'emailClick.dj.HeadlineList',
            pressClipsClick: 'pressClipsClick.dj.HeadlineList',
            entityClick: 'entityClick.dj.HeadlineList'
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "HeadlineList" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // state bag. to be populated by composite headline
            this.selections = {};

        },

        _initializeElements: function (ctx) {
            this.$headlineBox = ctx.find(this.selectors.headlineBox);
        },

        // gets called during base.init()
        _initializeEventHandlers: function () {
            this._super();
            var $container = this.$element,
                self = this;


            // toggle duplicates tray
            $container.delegate(self.selectors.dupWrapperHeader, 'click', function (ev) {
                var $this = $(this),
                    $dupList = $this.siblings('ul');

                // show/hide the header section
                //$this.toggle();

                // toggle the arrow in header
                $this.find(self.selectors.icon).toggleClass(self.constants.blackArrowCollapsed + ' ' + self.constants.blackArrowExpanded);

                // toggle the dup list
                $dupList.toggleClass('hide');

                return false;
            });

            // primary click
            $container.delegate(self.selectors.headlineTitle, 'click', function (ev) {
                var data = self._getHeadlineData(this);

                self.publish(self.events.headlineClick, data);

                return false;
            });

            $container.delegate(self.selectors.headlineCheckbox + ',' + self.selectors.dupHeadlineCheckbox, 'click', function (e) {
                if (self.options.maxSelect > 0 && self.selections.length === self.options.maxSelect) {
                    alert('<%= Token("maxHeadlinesSelected") %>');
                    return false;
                }

                var $this = $(this),
                    data = self._getHeadlineData(this);

                var dupsToggled = self.toggleDups(this);

                if (dupsToggled) {
                    //Since we are changing the dup headlines checkbox, publish the headlineCheck event for each dup headline
                    var checked = $this.is(':checked'),
                    dupWrapper = $this.closest('li').children(self.selectors.dupWrapper);
                    $.each(dupWrapper.children('ul').children('li'), function () {//Loop through all the dup headlines
                        self.publish(self.events.headlineCheck, { checked: checked, data: self._getHeadlineData(this) });
                    });
                }

                self.publish(self.events.headlineCheck, { checked: this.checked, data: data });
                e.stopPropagation();
            });

            $container.delegate(self.selectors.headlineBox, 'hover', function (ev) {
                var $item = $(this);

                if (ev.type === 'mouseenter') {
                    $item.addClass('over');
                }
                else {
                    $item.removeClass('over');
                }

                return false;
            });

            this.$headlineBox.delegate(self.selectors.emailIcon, 'click', function () {
                self.publish(self.events.emailClick, self._getHeadlineData(this));
            });

            this.$headlineBox.delegate(self.selectors.printIcon, 'click', function () {
                self.publish(self.events.printClick, self._getHeadlineData(this));
            });

            this.$headlineBox.delegate(self.selectors.saveIcon, 'click', function () {
                self.publish(self.events.saveClick, self._getHeadlineData(this));
            });

            this.$headlineBox.delegate(self.selectors.pressClipsIcon, 'click', function () {
                self.publish(self.events.pressClipsClick, self._getHeadlineData(this));
            });

            this.$headlineBox.delegate(self.selectors.source, 'click', function (e) {
                var $this = $(this);
                self.publish(self.events.entityClick, { event: e, entityType: 'source', entityCode: $this.data('source') });
                return false;
            });

            this.$headlineBox.delegate(self.selectors.author, 'click', function (e) {
                var $this = $(this),
                    entityCode = $this.data('author');
                if (entityCode) {
                    self.publish(self.events.entityClick, { event: e, entityType: 'author', entityCode: entityCode });
                }
                else {
                    $dj.info('Author Code not found. Need not publish event.');
                }
                return false;
            });

            if (self.options.displaySnippets === 3) {//3 = Hover
                this.$headlineBox.delegate(self.selectors.headlineTitle, 'hover', function (e) {
                    var $this = $(this);
                    if (!$this.data("tooltipset")) {
                        $this.data("tooltipset", true);
                        $this.dj_simpleTooltip("tooltip").mouseover();
                    }
                    return false;
                });
            }
        },

        _getHeadlineData: function (elem) {
            var $headlineItem = $(elem).closest(this.selectors.headlineItem);
            var titleStr = $headlineItem.children(this.selectors.title).text();
            //var data = $(elem).closest(this.selectors.headlineBox).data('headlineinfo');
            var data = $headlineItem.data('headlineinfo');
            return $.extend({ title: titleStr, sender: elem }, data);
        },

        _renderSuccess: function () {
            // hrusi: when client templates is implemented, revisit this
            $dj.debug('Client Templating not supported yet');
            /*
            var html;

            try {
            this.$element.empty();

                
            if (this.data && this.data.TotalRecords) {
            //per discussion with jess, for now we're going to manually map the response options into the local options object.
            this.options.showDuplicates = this.data.ShowDuplicates;
            this.options.showCheckboxes = this.data.ShowCheckboxes;
            this.options.showSortOptions = this.data.ShowSortOptions;
            this.options.sortOption = this.data.SortOption;
            this.options.showAuthorOptions = this.data.ShowAuthorOptions;
            this.options.showSourceOptions = this.data.ShowSourceOptions;
            this.options.showPaging = this.data.ShowPaging;
            this.options.pageSize = this.data.PageSize;
            this.options.sortOptions = this.data.SortOptions;
            this.options.authorOptions = this.data.AuthorOptions;
            this.options.sourceOptions = this.data.SourceOptions;

            html = this.templates.success({ headlines: this.data, options: this.options });
            this.$element.append(html);
            }
            else {
            html = this.templates.noData(this.data);
            this.$element.append(html);
            }
            } catch (e) {
            $dj.debug('Error in HeadlineList._renderSuccess');
            $dj.debug(e);
            }
            */
        },

        _renderError: function () {
            $dj.debug('Client Templating not supported yet');
            /*try {
            this.$element.empty();
            this.$element.append(this.templates.error(this.data));
            } catch (e) {
            $dj.debug('Error in HeadlineList._renderError');
            $dj.debug(e);
            }*/
        },

        _dupsTrayOpen: function (dupWrapperHeader) {
            if (!dupWrapperHeader) { $dj.debug('DJ.UI.HeadlineList: dupWrapperHeader is null or undefined'); return; }
            return $(dupWrapperHeader).siblings('ul').is(":visible");
        },

        setData: function (data) {
            this.data = data;

            if (data) {
                this._renderSuccess();
            }
            else {
                this._renderError();
            }
        },

        toggleAllHeadlines: function (select, selectDups) {
            var headlines = this.$element.find(this.selectors.headlineBox);

            // using native for loop for speed (jQuery and underscore each are painfully slow)
            for (var i = 0, len = headlines.length; i < len; i++) {
                this.toggleHeadline($(headlines[i]).find(this.selectors.headlineCheckbox), select, selectDups);
            }
        },

        toggleHeadline: function (headlineChk, select, selectDups) {
            var $chk = (headlineChk.jquery) ? headlineChk : $(headlineChk);

            $chk.attr('checked', select || false);

            this.toggleDups(headlineChk, selectDups);
        },

        toggleDups: function (headlineChk, selectDups) {
            var $chk = (headlineChk.jquery) ? headlineChk : $(headlineChk),
                checked,
                dupWrapper = $chk.closest('li').children(this.selectors.dupWrapper);

            switch (selectDups) {
                // select without dups 
                case 'none': dupWrapper.find(this.selectors.dupHeadlineCheckbox).prop('checked', false);
                    return true;

                    // select ALL dups that are visible
                case 'visible': checked = this._dupsTrayOpen(dupWrapper.children('p'));
                    dupWrapper.find(this.selectors.dupHeadlineCheckbox).prop('checked', checked);
                    return true;

                    // select all dups
                case 'all': dupWrapper.find(this.selectors.dupHeadlineCheckbox).prop('checked', true);
                    return true;

                    // toggle them if visible
                default: if (this._dupsTrayOpen(dupWrapper.children('p'))) {
                        dupWrapper.find(this.selectors.dupHeadlineCheckbox).prop('checked', $chk.is(':checked'));
                        return true;
                    }
            }
            return false;
        },

        setSelections: function (/* object */selections, /* bool */updateUI) {
            this.selections = selections;

            if (updateUI) {
                var headlines = this.$element.find(this.selectors.headlineBox);

                // using native for loop for speed (jQuery and underscore each are painfully slow)
                for (var i = 0, len = headlines.length; i < len; i++) {

                    this.toggleHeadline($(headlines[i]).find(this.selectors.headlineCheckbox), true);
                }

            }
        },

        getSelectedHeadlinesData: function () {
            var data = [], i, len;

            var headlines = this.$element.find(this.selectors.headlineCheckbox + ":checked");

            for (i = 0, len = headlines.length; i < len; i++) {
                data.push(this._getHeadlineData(headlines[i]));
            }

            var dupHeadlines = this.$element.find(this.selectors.dupHeadlineCheckbox + ":checked");

            for (i = 0, len = dupHeadlines.length; i < len; i++) {
                data.push(this._getHeadlineData(headlines[i]));
            }

            return data;
        }

    });

    $.plugin('dj_HeadlineList', DJ.UI.HeadlineList);
    $dj.debug('Registered DJ.UI.HeadlineList (extends DJ.UI.Component)');
