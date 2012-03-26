/*!
 * DemoCanvasModuleEditor
 *   e.g. , "this._imageSize" is generated automatically.
 *
 *   
 *  Getters and Setters are generated automatically for every Client Property during init;
 *   e.g. if you have a Client Property called "imageSize" on server side code
 *        get_imageSize() and set_imageSize() will be generated during init.
 *  
 *  These can be overriden by defining your own implementation in the script. 
 *  You'd normally override the base implementation if you have extra logic in your getter/setter 
 *  such as calling another function or validating some params.
 *
 */

(function ($) {

    DJ.UI.DemoCanvasModuleEditor = DJ.UI.Component.extend({

        /*
        * Properties
        */

        // Default options
        defaults: {
            debug: false,
            cssClass: 'DemoCanvasModuleEditor'
            // ,name: value     // add more defaults here separated by comma
        },

        // Localization/Templating tokens
        tokens: {
        // name: value     // add more defaults here separated by comma
        },


        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "DemoCanvasModuleEditor" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code like the following:
            // this._testButton = $('.testButton', element).get(0);
        },


        /*
        * Public methods
        */

        // TODO: Public Methods here


        /*
        * Private methods
        */

        // DEMO: Overriding the base _paint method:
        _paint: function () {

            // "this._super()" is available in all overridden methods
            // and refers to the base method.
            this._super();
        }


    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_DemoCanvasModuleEditor', DJ.UI.DemoCanvasModuleEditor);


})(jQuery);