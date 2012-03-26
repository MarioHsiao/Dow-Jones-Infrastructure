/*
 *  Social Behavior
 *  
 *  12/13/2010: hrusi: refactored Ajax Toolkit based script to jQuery + DJ MVC pattern
 *
 */

DJ.UI.SocialButtons = DJ.UI.Component.extend({

    __socialNetworkTypes: [
            { key: "delicious", name: "Delicious", url: "http://del.icio.us/post?url={URL}&title={TITLE}" },
            { key: "digg", name: "Digg", url: "http://digg.com/submit?phase=2&url={URL}&title={TITLE}" },
            { key: "facebook", name: "Facebook", url: "http://www.facebook.com/share.php?u={URL}" },
            { key: "furl", name: "Furl", url: "http://furl.net/storeIt.jsp?u={URL}&t={TITLE}" },
            { key: "google", name: "Google", url: "http://www.google.com/bookmarks/mark?op=edit&bkmk={URL}&title={TITLE}" },
            { key: "linkedin", name: "Linkedin", url: "http://www.linkedin.com/shareArticle?mini=true&url={URL}&title={TITLE}&summary={DESCRIPTION}&source=" },
            { key: "newsvine", name: "Newsvine", url: "http://www.newsvine.com/_wine/save?u={URL}&h={TITLE}" },
            { key: "reddit", name: "Reddit", url: "http://reddit.com/submit?url={URL}&title={TITLE}" },
            { key: "stumbleupon", name: "StumbleUpon", url: "http://www.stumbleupon.com/submit?url={URL}&title={TITLE}" },
            { key: "technorati", name: "Technorati", url: "http://www.technorati.com/faves?add={URL}" },
            { key: "twitter", name: "Twitter", url: "http://twitter.com/?status={TITLE}%20-%20{URL}" },
            { key: "yahoo", name: "Yahoo", url: "http://myweb2.search.yahoo.com/myresults/bookmarklet?u={URL}&t={TITLE}" },
            { key: "myspace", name: "MySpace", url: "http://www.myspace.com/Modules/PostTo/Pages/?u={URL}&t={TITLE}&c={DESCRIPTION}&l=3" }
        ],

    options: {
        socialNetworks: null,
        imageSize: 0,
        url: null,
        title: null,
        keywords: null,
        description: null,
        target: "_blank"
    },

    init: function (element, meta) {

        var $meta = $.extend({ name: "SocialBehavior" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        this.element = element;

        this.applySocial();
    },



    applySocial: function () {
        var undefined;
        var socLink;
        var index;
        var name;
        var url;
        var imgSize = this.get_imageSize();
        var element = this.element;
        var arrNetworks = this.get_socialNetworks();

        if (arrNetworks === null || arrNetworks === undefined) {
            $dj.debug('SocialBehavior: No Social Networks specified. Make sure you pass at least one social network while initializing.');
            return;
        }
        // Set target attribute
        var target = "target=\"" + this.get_target() + "\"";
        var sb = [];

        for (var i = 0, len = arrNetworks.length; i < len; i++) {
            index = arrNetworks[i];
            var tObj = this.__socialNetworkTypes[index];
            id = tObj.key;
            url = tObj.url;
            name = tObj.name;

            if (imgSize === 0) {
                cName = "social_button social_sm social_sm_" + id;
            } else {
                cName = "social_button social_lg social_lg_" + id;
            }

            if (url !== undefined) {
                if (url !== undefined) {
                    url = url.replace("{TITLE}", this.urlencode(this.options.title));
                    url = url.replace("{URL}", this.urlencode(this.options.url));
                    url = url.replace("{KEYWORDS}", this.urlencode(this.options.keywords));
                    url = url.replace("{DESCRIPTION}", this.urlencode(this.options.description));
                    sb[sb.length] = "<a " + target + " href=\"" + url + "\" class=\"" + cName + "\" title=\"" + name + "\">" + name + "</a>";
                }
            }
        }
        if (sb.length > 0) {
            $(element).html(sb.join(""));
        }

        if (this.options.showCustomTooltip) {
            $(".social_button", element).dj_simpleTooltip("dj_socialTip");
        }
    },

    jsocial_metakeywords: function () {

        jsocial_keywords = this.get_keywords();
        if (jsocial_keywords !== null) { return jsocial_keywords; }

        if (jsocial_description === undefined) {
            metaCollection = document.getElementsByTagName('meta');
            for (i = 0; i < metaCollection.length; i++) {
                nameAttribute = metaCollection[i].name.search(/keywords/);
                if (nameAttribute != -1) {
                    jsocial_keywords = metaCollection[i].content;
                    return jsocial_keywords;
                }
            }
        } else {
            return jsocial_keywords;
        }
    },

    jsocial_metadescription: function () {
        jsocial_description = this.get_description();
        if (jsocial_description !== null) { return jsocial_description; }

        if (jsocial_description === undefined) {
            metaCollection = document.getElementsByTagName('meta');
            for (i = 0; i < metaCollection.length; i++) {
                nameAttribute = metaCollection[i].name.search(/description/);
                if (nameAttribute != -1) {
                    jsocial_description = metaCollection[i].content;
                    return jsocial_description;
                }
            }
        } else {
            return jsocial_description;
        }

    },

    jsocial_title: function () {
        var intgTitle = this.get_title();
        if (intgTitle === null) {
            intgTitle = document.title;
        }

        return intgTitle;
    },

    jsocial_url: function () {
        var intgUrl = this.get_url();
        if (intgUrl === null) {
            intgUrl = document.location.href;
        }

        return intgUrl;
    },

    urlencode: function (string) {
        if (string === undefined) {
            return "";
        }
        return string.replace(/\s/g, '%20').replace('+', '%2B').replace('/%20/g', '+').replace('*', '%2A').replace('/', '%2F').replace('@', '%40');
    },

    dispose: function () {
        // declare a local one so that we're immune to changes to the global javascript 'undefined'
        var undefined;

        if (this.options === undefined || this.options === null)
            return;     // nothing to dispose

        for (var propName in this.options) {
            if (this["get_" + propName]) this["get_" + propName] = null;
            if (this["set_" + propName]) this["set_" + propName] = null;
            this.options[propName] = null;
        };

        this.options = null;
    }
});

    // Declare this class as a jQuery plugin
    $.plugin('dj_SocialButtons', DJ.UI.SocialButtons);
