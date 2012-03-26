/*!
* Factiva Canvas
*/

(function ($) {

    DJ.UI.FactivaCanvas = DJ.UI.AbstractCanvas.extend({

        init: function (element, meta) {
            this._super(element, meta);
            this._debug('Token value: <%= Token("HelloWorld") %>');

            this._subscribeToModuleEvents();
        },

        _subscribeToModuleEvents: function () {
            this.subscribe('swap', $dj.delegate(this, this._swapModule));
            this.subscribe('syndicationEdit', $dj.delegate(this, this._onSyndicationEdit));
            //Subscribing to canvas events
            this.subscribe(this.events.canvasModuleRemove, function (module) {
                lbdialog({
                    content: '<%= Token("removeModuleConfirmMsg") %>',
                    cancelButton: {
                        text: '<%=Token("no")%>'
                    },
                    OKButton: {
                        text: '<%=Token("yes")%>',
                        callback: function () {
                            module.remove();
                            return false;
                        }
                     }
                });
                return false;
            });
        },

        _swapModule: function (sender, data) {
            $dj.proxy.invoke({
                url: this.options.swapModuleServiceUrl,
                data: {
                    'moduleIdToRemove': data.moduleId.toString(),
                    'moduleIdToAdd': data.innerData.moduleIdToAdd.toString(),
                    'pageId': data.canvasId.toString()
                },
                method: 'PUT',
                controlData: this.options.ControlData,
                preferences: this.options.Preferences,
                onSuccess: $dj.delegate(this, this._onSwapCallSuccess, { module: sender, props: data }),
                onError: $dj.delegate(this, this._onSwapCallError, { module: sender, props: data })

            });
        },


        _onSwapCallSuccess: function (customData, result) {
            var canvasModule = customData.module;
            canvasModule.set_moduleId(result.package.newsPageModule.id);
            canvasModule.set_moduleTitle(result.package.newsPageModule.title);
            canvasModule.getData();
        },


        _onSwapCallError: function (customData, error) {
            var canvasModule = customData.module;
            canvasModule.showErrorMessage({ returnCode: error.code, statusMessage: error.message });
        },


        _onSyndicationEdit: function (data) {
            $dj.debug('Syndication Edit triggered.', data);
        },


        EOF: null

    });

    $.plugin('dj_factivaCanvas', DJ.UI.FactivaCanvas);

    $dj.debug('Registered DJ.UI.FactivaCanvas as dj_factivaCanvas');

})(jQuery);