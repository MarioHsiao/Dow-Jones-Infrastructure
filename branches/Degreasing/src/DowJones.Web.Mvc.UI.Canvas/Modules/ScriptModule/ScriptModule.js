﻿DJ.UI.ScriptModule = DJ.UI.AbstractCanvasModule.extend({

    defaults: {
        cssClass: 'canvas-module script-module',
        menuItems: [
            { id: 'remove', label: "<%= Token('moduleMenuRemove') %>" },
            { id: 'edit', label: "<%= Token('moduleMenuEdit') %>" }
        ],
        scriptOptions: {}
    },
    
    init: function (el, meta) {
        this._super(el, meta);

        this.executeScript();
        this.showContentArea();
    },
    
    _initializeDelegates: function () {
        this._super();
    },

    _initializeElements: function (el) {
        this._super();
        this._scriptContainer = $('.script-component-container', el).get(0);
    },
    
    _initializeEventHandlers: function () {
        this._super();
    },

    executeScript: function () {
        var params = {
            container: this._scriptContainer,
            options: this.options,//.scriptOptions,
            module: this
        };

        function evalClosure(script) {
            $.extend(this, params);
            
            eval(script);
        }

        var scriptUrl = this.options.dataServiceUrl + '/script/' + this.options.templateId;
        
        $.ajax(scriptUrl, { dataType: 'html' }).success(evalClosure);
    },

    EOF: null
});

// Declare this class as a jQuery plugin
$.plugin('dj_ScriptModule', DJ.UI.ScriptModule);