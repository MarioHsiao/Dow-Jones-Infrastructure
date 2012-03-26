/*!
 * TopNewsModuleCanvasModule
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

    DJ.UI.TopNewsModuleCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        /*
        * Properties
        */

        // add client side properties here

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "TopNewsModuleCanvasModule" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code like the following:
            // this._testButton = $('.testButton', element).get(0);
        }


        /*
        * Public methods
        */

        // TODO: Public Methods here


        /*
        * Private methods
        */


    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_TopNewsModuleCanvasModule', DJ.UI.TopNewsModuleCanvasModule);

    $dj.debug('Registered DJ.UI.TopNewsModuleCanvasModule as dj_TopNewsModuleCanvasModule');

})(jQuery);