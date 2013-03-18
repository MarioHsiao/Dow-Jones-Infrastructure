/*!
 * AlertSearchBuilder
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


    DJ.UI.AlertSearchBuilder = DJ.UI.QueryBuilder.extend({

        /*
        * Properties
        */

        // Default options
        defaults: {
            debug: false,
            cssClass: 'AlertSearchBuilder'
            // ,name: Data     // add more defaults here separated by comma
        },

        // Localization/Templating tokens
        tokens: {
            // name: Data     // add more defaults here separated by comma
        },


        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var self = this;
            var $meta = $.extend({ name: "AlertSearchBuilder" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            $('input:radio[name=AlertHeadlineViewType]').click(function () {
                var viewType = $('input:radio[name=AlertHeadlineViewType]:checked').val();
                var selectedAlertId = $('.dj_alert-wrapper .dj_menu-wrapper').children(0).attr("selectedalertid"); // $("#ddlAlerts").val();
                var postUrl = $('#hdnPostBackUrl').val() + "?kind=alert&alertId=" + selectedAlertId + "&viewType=" + viewType;
                var searchRequest = self.getSearchRequest();
                if (searchRequest.ShowDuplicates || searchRequest.ShowDuplicates == "0") {
                    postUrl += "&ShowDuplicates=" + searchRequest.ShowDuplicates;
                }
                $(location).attr('href', postUrl);
            });

            $(".dj_menu-wrapper").click(function () {
                self._getData();
            });
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

            alert('TODO: implement AlertSearchBuilder._paint!');
        },

        _getData: function () {
            $('#selectboxLoading').show(),
         $.ajax({
             url: $('#hdnServiceUrl').val(),
             data: {},
             success: function (response, status, jqXHR) {
                 $('#selectboxLoading').hide();
                 $('.dj_alert-wrapper .dj_menu-wrapper').menu(
					 { items: jQuery.parseJSON(response.items),
					     defaultText: jQuery.parseJSON(response.defaultText)
					 },
                     { maxHeight: '300px' },
					 { menuType: 'dropdownMenu' },
					 { menuClass: 'dj_alert-menu' },
					 { itemClick: function (e, item) {
					     var itemId = item.id;
					     var viewType = ($('#' + itemId).hasClass("dj_icon-folder-new")) ? "New" : "All";
					     var postUrl = $('#hdnPostBackUrl').val() + "?alertId=" + itemId + "&viewType=" + viewType;
					     $(location).attr('href', postUrl);
					 }
					 });
             },
             failure: function (response) { alert(DJGlobal.getErrorMessage(response.Error)); }
         });
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_AlertSearchBuilder', DJ.UI.AlertSearchBuilder);

