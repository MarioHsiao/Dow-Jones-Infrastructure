    DJ.UI.DateSlider = DJ.UI.Component.extend({
        defaults: {},
        selectors: { contentBody: '.dj_dates', date: '.dj_date', activeDate: '.dj_selected', disabledDate: '.dj_disabled' },
        eventNames: { selectedDateChanged: 'selectedDateChanged.dj.dateSlider' },
        init: function (element, meta) {
            var $meta = $.extend({ name: "DateSlider" }, meta);
            this._super(element, $meta);
        },
        _initializeElements: function () {
            this._renderContainer();
        },
        _initializeEventHandlers: function () {
            this.$element.on('click', this.selectors.date, this._delegates.OnDateClicked);
        },
        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnDateClicked: $dj.delegate(this, this._onDateClicked)
            });
        },
        _onDateClicked: function (ev) {
            var $target = $(ev.currentTarget);
            var activeClass = this.selectors.activeDate.replace('.', '');
            if (!$target.hasClass(activeClass)
                && !$target.hasClass(this.selectors.disabledDate.replace('.', ''))) {
                $(this.selectors.date, this.$element).removeClass(activeClass);
                $target.addClass(activeClass);
                var group = $target.data('group');
                this.publish(this.eventNames.selectedDateChanged, { groupId: group });
            }
        },
        setData: function (data) {
            this._render(data);
            this._dates = $(this.selectors.date, this.$element);
        },
        setActiveGroup: function (id) {
            var target = $.grep(this._dates, function (elem) {
                return $(elem).data('group') === id;
            });

            if (target.length) {
                this._dates.removeClass(this.selectors.activeDate.replace('.', ''));
                $(target).first().addClass(this.selectors.activeDate.replace('.', ''));
            }
        },
        _renderContainer: function () {
            this.$element.html(this.templates.container(this.options));
        },
        _render: function (data) {
            $(this.selectors.contentBody, this.$element).html(this.templates.success({ Dates: data.Dates, Options: this.options }));
        }
    });

    DJ.jQuery.plugin('dj_DateSlider', DJ.UI.DateSlider);
    $dj.debug('Registered DJ.UI.DateSlider(extends DJ.UI.Component)');