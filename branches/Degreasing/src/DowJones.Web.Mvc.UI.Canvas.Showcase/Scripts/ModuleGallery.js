DJ.$dj.require(['DJ', '$dj', '$'], function (DJ, $dj, $) {

    DJ.UI.ModuleGallery = DJ.UI.Component.extend({

        _init: function (el, meta) {
            this._super(el, meta);
        },

        _initializeDelegates: function () {
            this._delegates = {
                onCancel: $dj.delegate(this, this._onCancel),
                onSaved: $dj.delegate(this, this._onSaved),
                save: $dj.delegate(this, this.save)
            };
        },

        _initializeElements: function (el) {
            this._super();

            this._modules = $('.module', el);
            this._editors = $('.editor', el);
            this._cancelButton = $('.cancel', el);
            this._saveButton = $('.save', el);

            this.selectTemplate(1);
        },

        _initializeEventHandlers: function () {
            this._super();

            var gallery = this;

            this._modules.click(function () {
                gallery._modules.removeClass('active');
                $(this).addClass('active');
                gallery._showSelectedEditor();
            });

            this._cancelButton.click(this._delegates.onCancel);
            this._saveButton.click(this._delegates.save);
        },

        save: function () {
            var editor = this._editors.filter('.active').find('.dj_ModuleEditor').findComponent();
            editor.save(this._delegates.onSaved);
        },

        selectTemplate: function (templateId) {
            console.log('Selecting template ' + templateId);
            var moduleSel = '[data-template-id=' + templateId +']';
            this._modules.removeClass('active');
            this._modules.filter(moduleSel).addClass('active');
            
            var editorSel = '.template-' + templateId;
            this._editors.removeClass('active');
            this._editors.filter(editorSel).addClass('active');
        },
        
        _onSaved: function (args) {
            this.publish('moduleSaved.dj.ModuleGallery', { moduleId: args.moduleId });
        },

        _onCancel: function (args) {
            this.publish('canceled.dj.ModuleGallery', { moduleId: args.moduleId });
        },

        _showSelectedEditor: function () {
            var templateId = this._modules.filter('.active').data('template-id');
            this.selectTemplate(templateId);
        },

        EOF: null
    });


    $.plugin('dj_ModuleGallery', DJ.UI.ModuleGallery);
});