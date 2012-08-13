DJ.UI.HtmlModule = DJ.UI.AbstractCanvasModule.extend({

    defaults: {
        cssClass: 'canvas-module',
        menuItems: [
            { id: 'remove', label: "<%= Token('moduleMenuRemove') %>" },
            { id: 'edit', label: "<%= Token('moduleMenuEdit') %>" }
        ]
    },

    _script: '',

    init: function (el, meta) {
        this._super(el, meta);

        this.showContentArea();
        this.executeScript();
    },
    
    _initializeElements: function (el) {
        this._super();
        this._script = $('.script', el).text();
    },
    
    _initializeEventHandlers: function () {
        this._super();
    },

    executeScript: function () {
        eval(this._script);
    },

    fireOnSaveAndCloseEditArea: function () {
        this._saveProperties($dj.delegate(this, function () {
            this.get_Canvas().reloadModule(this.get_moduleId());
        }));
    },

    EOF: null
});

// Declare this class as a jQuery plugin
$.plugin('dj_HtmlModule', DJ.UI.HtmlModule);
