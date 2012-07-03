DJ.UI.CalendarEventList = DJ.UI.Component.extend({
    defaults: {},
    selectors: {
        contentBody: '.dj_calendarTable',
        news: '.dj_eventNewsTrigger'
    },
    eventNames: { newsClicked: 'newsClicked.dj.CalendarEventList' },
    init: function (element, meta) {
        var $meta = $.extend({ name: "CalendarEventList" }, meta);
        this._super(element, $meta);
    },

    _initializeElements: function () {
        this._renderContainer();
    },
    _initializeEventHandlers: function () {
        this.$element.on('click', this.selectors.news, this, this._onNewsClicked);
    },
    setData: function (data) {
        this._render(data);
    },
    _onNewsClicked: function (ev) {
        var ctx = ev.data;
        var $item = $(ev.currentTarget);
        var o = {};
        o.NewsQueryMode = $item.data('mode');
        o.NewStatisticsCodeQuery = $item.data('query');
        ctx.publish(ctx.eventNames.newsClicked, o);
        return false;
    },
    _renderContainer: function () {
        this.$element.html(this.templates.container(this.options));
    },
    _render: function (data) {
        $(this.selectors.contentBody, this.$element).html(this.templates.success({ Dates: data.Dates, Options: this.options }));
    }
});

DJ.jQuery.plugin('dj_CalendarEventList', DJ.UI.CalendarEventList);
$dj.debug('Registered DJ.UI.CalendarEventList (extends DJ.UI.Component)');


 