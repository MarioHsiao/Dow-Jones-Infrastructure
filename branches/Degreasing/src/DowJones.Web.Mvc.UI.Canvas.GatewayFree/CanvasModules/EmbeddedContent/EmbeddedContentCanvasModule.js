/*!
* EmbeddedContentCanvasModule
*/

DJ.UI.EmbeddedContentCanvasModule = DJ.UI.AbstractCanvasModule.extend({

    defaults: {
        cssClass: 'canvas-module',
        menuItems: [
            { id: 'remove', label: "<%= Token('moduleMenuRemove') %>" },
            { id: 'edit', label: "<%= Token('moduleMenuEdit') %>" }
        ]
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "EmbeddedContentCanvasModule" }, meta);
        this._super(element, $meta);

        this.showContentArea();
    },

    _initializeElements: function (el) {
        this._super();
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
    
    fireOnSaveAndCloseEditArea: function () {
        this._saveProperties($dj.delegate(this, function () {
            this.get_Canvas().reloadModule(this.get_moduleId());
        }));
    },

    EOF: null
});


// Declare this class as a jQuery plugin
$.plugin('dj_EmbeddedContentCanvasModule', DJ.UI.EmbeddedContentCanvasModule);

$dj.debug('Registered DJ.UI.EmbeddedContentCanvasModule as dj_EmbeddedContentCanvasModule');
