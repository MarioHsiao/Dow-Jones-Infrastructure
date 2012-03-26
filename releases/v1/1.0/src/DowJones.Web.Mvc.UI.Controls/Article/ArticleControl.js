/*!
* ArticleControl
*/

(function ($) {

    DJ.UI.ArticleControl = DJ.UI.Component.extend({

        /*
        * Properties
        */

        // Default options
        defaults: {
            debug: false,
            cssClass: 'ArticleControl'
            // ,name: value     // add more defaults here separated by comma
        },

        // Localization/Templating tokens
        tokens: {
        // name: value     // add more defaults here separated by comma
    },

    _proxyCredentials: {
        interfaceLanguage: "en",
        productPrefix: "GL",
        accessPointCode: "7",
        pageCacheKey: ""

    },

    _control: null,
    _fcode: null,
    _companyLinks: null,
    _sourceLink: null,
    _eLinks: null,
    _companyLinkClass: ".dj_emg_article_entity",
    _sourceLinkClass: ".dj_emg_article_source",
    _eLinkClass: ".dj_emg_article_elink",


    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "ArticleControl" }, meta);

        // Call the base constructor
        this._super(element, $meta);

    },

    /*
    * Private methods
    */

    // DEMO: Overriding the base _clear method:
    _clear: function () {

        // "this._super()" is available in all overridden methods
        // and refers to the base method.
        this._super();
    },

    // DEMO: SAMPLE PRIVATE METHOD TO DEMO BEST PRACTICE

    // TODO: add arguments and even change the name of the function if you like. 
    //       prefixing an underscore reminds you that its private
    _initializeArticleControl: function (/* args */) {
        // TODO: add code to initialize your control

    },


    companyCallBack: function (content) {
        $(this._control).triggerHandler("dj.articlecontrol.OnEntityClick", content);
    }

});

// Declare this class as a jQuery plugin
$.plugin('dj_ArticleControl', DJ.UI.ArticleControl);


})(jQuery);