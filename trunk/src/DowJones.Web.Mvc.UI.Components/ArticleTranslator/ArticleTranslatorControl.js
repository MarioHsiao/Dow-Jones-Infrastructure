/*!
* ArticleTranslatorControl
*/

    DJ.UI.ArticleTranslatorControl = DJ.UI.Component.extend({

        /*
        * Properties
        */
        defaults: {
            debug: false,
            cssClass: 'ArticleTranslatorControl'
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "ArticleTranslatorControl" }, meta);
            this._super(element, $meta);

            this.container = element;
            this.$container = $(element);

            this.initialize();
        },

        initialize: function () {
            var me = this;
            $('.dj_article_translator_langclicklink').each(function (i) {
                $(this).click(function (e) {
                    //me._publish('dj.ArticleTranslatorControl.langClick', $(this).attr('rel'));
                    me.$container.triggerHandler('dj.ArticleTranslatorControl.langClick', { sender: this, data: $(this).attr('rel') });
                    e.stopPropagation();
                    return false;
                });
            });

            this.showArticleTranslatorControl();
            $('#TranslatelinkId').bind('click', function () {
                $('#translateLanguagesDiv').toggle(400);
            });

            $('#translateClose').bind('click', function () {
                $('#translateLanguagesDiv').hide();
            });

        },

        /*
        * Public methods
        */
        showArticleTranslatorControl: function () {
            $('.dj_article_translator_langclicklink').each(function (i) {
                $(this).mouseover(function (e) {
                    $(this).addClass('ui-state-hover');
                }).mouseout(function (e) {
                    $(this).removeClass('ui-state-hover');
                }).mouseup(function(e){
                    $('#translateLanguagesDiv').hide();
                }) ;
            });
         },

         EOF: null

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_ArticleTranslatorControl', DJ.UI.ArticleTranslatorControl);
