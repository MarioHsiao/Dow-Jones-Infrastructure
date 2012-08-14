DJ.$dj.require(['DJ', '$dj', '$'], function (DJ, $dj, $) {

    DJ.UI.ModuleGallery = DJ.UI.Component.extend({
        init: function (el, meta) {
            this._super(el, meta);

            if (!el.id) el.id = 'module-gallery';
        },


        _initializeDelegates: function () {
            this._super();

            $.extend(this._delegates, {
                hide: $dj.delegate(this, this.hide),
                onSaved: $dj.delegate(this, this._onSaved),
                save: $dj.delegate(this, this.save),
            });
        },

        _initializeElements: function (el) {
            this._super();

            this._modules = $('.module', el);
            this._editors = $('.editor', el);
            this._cancelButton = $('.cancel', el);
            this._saveButton = $('.save', el);

            this._editors.first().show().addClass('selected');
        },

        _initializeEventHandlers: function () {
            this._super();

            var gallery = this;

            this._modules.click(function () {
                $(this).addClass('selected');
                gallery._modules.removeClass('selected');
                gallery._showSelectedEditor();
            });

            this._cancelButton.click(this._delegates.hide);
            this._saveButton.click(this._delegates.save);
        },

        show: function () {
            this.$element.show().overlay();
        },

        hide: function () {
            $().overlay.hide('.module-gallery');
        },

        save: function () {
            var url = DJ.Dashboards.ModuleGalleryUrl + '?pageId=' + this.options.pageId;
            var data = this._editors.filter('.selected').find('form').serialize();
            $.post(url, data, this._delegates.onSaved);
        },

        selectTemplate: function (templateId) {
            this._editors.removeClass('selected');
            var sel = '.template-' + templateId;
            this._editors.filter(sel).addClass('selected');
        },
        
        _onSaved: function (args) {
            this.publish('moduleSaved.dj.ModuleGallery', { moduleId: args.moduleId });
            this.hide();
        },

        _showSelectedEditor: function () {
            var templateId = this._modules.filter('.selected').data('template-id');
            this.selectTemplate(templateId);
        },

        EOF: null
    });


});