/*!
 * SimpleSearchControl
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

    DJ.UI.SimpleSearchControl = DJ.UI.Component.extend({

        /*
        * Properties
        */

        // Default options
        defaults: {
            debug: false,
            cssClass: 'SimpleSearchControl'
            // ,name: value     // add more defaults here separated by comma
        },

        // Localization/Templating tokens
        tokens: {
        // name: value     // add more defaults here separated by comma
    },


    /*
    * Initialization (constructor)
    */
    init: function (element, m) {
        var _self = this
                , $control = $(element)
				, $form = $control.find('form')
                , meta = $.extend({ name: 'SimpleSearch' }, m);

        function bindCustomHandlers() {
        }

        function bindDomHandlers() {
            $form.submit(function () {
                var query = $form.find('.page-search-field').val();

                if (query == '') { return false; }

                _self._publish('search/changed', query);

                return false;
            });
        }

        function createAutocomplete() {
            window.itemSSSelect = function () {
                $form.submit();
            }
            window.getSSErrorInfo = function (e) {
                console.log('error', e);
            }

            var url = 'http://utilities.factiva.com/handlers/emg.tools.web.widgets.autosuggest.ashx?390901'
                    , script = document.createElement('script');

            var settings = '{\
                    url: "http://suggest.factiva.com/Search/1.0",\
	                controlId: "page-search-field",\
	                autocompletionType: "KeyWord",\
                    maxResults: "10",\
	                selectFirst: false,\
                    resultsClass: "dj_emg_autosuggest_results",\
                    resultsOddClass: "dj_emg_autosuggest_odd",\
                    resultsEvenClass: "dj_emg_autosuggest_even",\
                    resultsOverClass: "dj_emg_autosuggest_over",\
                    onItemSelect : itemSSSelect,\
                    onError : getSSErrorInfo,\
                    useEncryptedKey: "dHbFNtEsdUE0P5gy5SlWbdefxL6NprcSLc_2F3pPKiUC_2BadRIr_2BIUhdQMZbqPyF5IH|2"}';
            console.log('meta', meta);
            console.log('autosuggestsettings', settings);
            script.text = settings;
            script.setAttribute('type', 'text/javascript');
            script.setAttribute('src', url);

            document.body.appendChild(script);
        }

        // Call the base constructor
        _self._super(element, meta);
        bindCustomHandlers();
        bindDomHandlers();
        //createAutocomplete();
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
$.plugin('dj_SimpleSearchControl', DJ.UI.SimpleSearchControl);


})(jQuery);