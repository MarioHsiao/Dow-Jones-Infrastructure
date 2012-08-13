﻿DJ.UI.HtmlModule = DJ.UI.AbstractCanvasModule.extend({

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
        this._maximizeButton = $('<span class="maximize">');
        this._minimizeButton = $('<span class="minimize">');

        $('.actions-container', el)
            .append(this._maximizeButton)
            .append(this._minimizeButton);
    },
    
    _initializeEventHandlers: function () {
        this._super();

        this._minimizeButton.click($dj.delegate(this, this.minimize));
        this._maximizeButton.click($dj.delegate(this, this.maximize));
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
