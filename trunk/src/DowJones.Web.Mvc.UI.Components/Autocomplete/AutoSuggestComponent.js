/*!
 * AutoSuggestComponent
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

    DJ.UI.AutoSuggestComponent = DJ.UI.Component.extend({

        /*
        * Properties
        */

        // Default options
        defaults: {
            debug: false,
            cssClass: 'AutoSuggestComponent'
            // ,name: value     // add more defaults here separated by comma
        },

        //Events
        events: {
            // jQuery events are namespaced as <event>.<namespace>
            itemSelect: "itemSelect.dj.AutoSuggestComponent",
            viewAllClick: "viewAllClick.dj.AutoSuggestComponent",
            viewMorePrivateMarketsClick: "viewMorePrivateMarketsClick.dj.AutoSuggestComponent",
            infoClick: "infoClick.dj.AutoSuggestComponent",
            promoteClick: "promoteClick.dj.AutoSuggestComponent",
            notClick: "notClick.dj.AutoSuggestComponent",
            discontClick: "discontClick.dj.AutoSuggestComponent"
        },

        // Localization/Templating tokens
        tokens: {
            // name: value     // add more defaults here separated by comma
            companyTkn: "<%= Token("companyTkn") %>",
            executiveTkn: "<%= Token("executiveTkn") %>",
            industryTkn: "<%= Token("industryTkn") %>",
            sourceTkn: "<%= Token("sourceTkn") %>",
            keywordTkn: "<%= Token("keywordTkn") %>",
            privateMarketCompanyHeaderTkn: "<%= Token("privateMarketCompanyHeaderTkn") %>",
            privateMarketIndustryHeaderTkn: "<%= Token("privateMarketIndustryHeaderTkn") %>",
            privateMarketRegionHeaderTkn: "<%= Token("privateMarketRegionHeaderTkn") %>",
            region_allTkn: "<%= Token("region_allTkn") %>",
            region_countryTkn: "<%= Token("region_countryTkn") %>",
            region_stateOrProvinceTkn: "<%= Token("region_stateOrProvinceTkn") %>",
            region_metropolitanAreaTkn: "<%= Token("region_metropolitanAreaTkn") %>",
            region_subNationalRegionTkn: "<%= Token("region_subNationalRegionTkn") %>",
            region_supranationalRegionTkn: "<%= Token("region_supranationalRegionTkn") %>",
            newsSubjectTkn: "<%= Token("newsSubjectTkn") %>",
            infoTitleTknPre: "<%= Token("infoTitleTknPre") %>",
            infoTitleTknPost: "<%= Token("infoTitleTknPost") %>",
            promoteTitleTkn: "<%= Token("promoteTitleTkn") %>",
            notTitleTkn: "<%= Token("notTitleTkn") %>",
            sourcefamilyTkn: "<%= Token("sourcefamilyTkn") %>",
            publicationTkn: "<%= Token("publicationTkn") %>",
            webpageTkn: "<%= Token("webpageTkn") %>",
            multimediaTkn: "<%= Token("multimediaTkn") %>",
            pictureTkn: "<%= Token("pictureTkn") %>",
            blogTkn: "<%= Token("blogTkn") %>",
            disContTkn: "<%= Token("disContTkn") %>",
            viewAllTkn: "<%= Token("viewAllTkn") %>",
            privateMarketCompanyViewMoreTkn: "<%= Token("privateMarketCompanyViewMoreTkn") %>",
            privateMarketIndustryViewMoreTkn: "<%= Token("privateMarketIndustryViewMoreTkn") %>",
            privateMarketRegionViewMoreTkn: "<%= Token("privateMarketRegionViewMoreTkn") %>"
        },


        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "AutoSuggestComponent" }, meta);

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

            alert('TODO: implement AutoSuggestComponent._paint!');
        }


    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_AutoSuggestComponent', DJ.UI.AutoSuggestComponent);
    $dj.debug('Registered DJ.UI.AutoSuggestComponent (extends DJ.UI.Component)');
