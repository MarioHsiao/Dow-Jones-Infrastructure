DJ.UI.HtmlModule = DJ.UI.AbstractCanvasModule.extend({
    _script: '',

    init: function (el, meta) {
        this._super(el, meta);

        this.showContentArea();
        this.executeScript();
    },
    
    _initializeElements: function () {
        this._super();
        this._script = $('.script', this.element).text();
    },
    
    executeScript: function () {
        eval(this._script);
    },

    EOF: null
});

// Declare this class as a jQuery plugin
$.plugin('dj_HtmlModule', DJ.UI.HtmlModule);
