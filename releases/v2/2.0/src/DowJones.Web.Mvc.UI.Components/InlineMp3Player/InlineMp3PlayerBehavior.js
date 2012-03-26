// README
//
// There are two steps to adding a property:
//
// 1. Create a member variable to store your property
// 2. Add the get_ and set_ accessors for your property
//
// Remember that both are case sensitive!


/// <reference name="MicrosoftAjaxTimer.debug.js" />
/// <reference name="MicrosoftAjaxWebForms.debug.js" />
/// <reference name="AjaxControlToolkit.ExtenderBase.BaseScripts.js" assembly="AjaxControlToolkit" />

DJ.UI.MP3PlayerType = function () {
    //throw Error.invalidOperation();
};

DJ.UI.MP3PlayerType.prototype = {
    Mini: 0,
    Normal: 1,
    Volume: 2,
    Multiple: 3,
    ReadSpeaker: 4,
    DowJones: 5
};


(function ($) {


    DJ.UI.InlineMp3PlayerBehavior = DJ.UI.Component.extend({

        options: {
            speakerImageUrl: "",
            mp3PlayerType: DJ.UI.MP3PlayerType.Volume,
            mp3PlayerUrl: "",
            mp3FilesUrls: "",

            // dewplayer properties
            showDownloadLink: false,
            downloadUrl: "",
            autoStart: false,
            autoReplay: false,
            showTime: false,
            randomPlay: false,
            noPointer: false,
            volume: 100,
            enableTransparency: true,
            backgroundColor: "FFFFFF",

            // flash player properties
            flashPlayerWidth: "",
            flashPlayerHeight: "",
            flashPlayerVersion: "",

            // Version for dewPlayer 
            dewplayerversion: ""
        },

        // Localization/Templating tokens
        tokens: {
            attributionToken: "Powered by ReadSpeaker",
            downloadToken: "Download",
            listenToArticleToken: "Listen to Article"
        },

        _button: null,
        _isOpen: false,

        init: function (element, meta) {

            var $meta = $.extend({ name: "InlineMp3PlayerBehavior" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // generate auto getter/setter for properties in tokens
            this._createAccessors(this.tokens);


            this._buttonClickDelegate = $.proxy(this._buttonClickHandler, this);
            this._buttonMouseoverDelegate = $.proxy(this._buttonMouseoverHandler, this);
            this._buttonMouseoutDelegate = $.proxy(this._buttonMouseoutHandler, this);

            this._button = this._createButton();
            this.$button = $(this._button);
        },

        dispose: function () {
            this._disposeButton();
            this.tokens.attributionToken = null;
            this.options.speakerImageUrl = null;
            this.options.mp3PlayerType = null;
            this.options.mp3PlayerUrl = null;
            this.options.mp3PlayerWidth = null;
            this.options.mp3PlayerHeight = null;
        },

        _disposeButton: function () {
            if (this.$button) {
                 $button.unbind("click", this._buttonClickDelegate);
                 $button.unbind("mouseover", this._buttonMouseoverDelegate);
                 $button.unbind("mouseout", this._buttonMouseoutDelegate);
                 this.$button = null;
                 this._button = null;
            }
        },


        //#region ..:: event handlers ::..

        _buttonClickHandler: function (e) {
            this._createFlashPlayer();
            return false;
        },

        _buttonMouseoverHandler: function (eventArgs) {
            var node = this.findParentElement(eventArgs.target, "a", "");
            if (node) node.className = "emg_speaker_button_hover";
        },

        _buttonMouseoutHandler: function (eventArgs) {
            var node = this.findParentElement(eventArgs.target, "a", "");
            if (node) $(node).removeClass("emg_speaker_button_hover");
        },

        //#endregion

        _createFlashPlayer: function () {

            var element = this.element;

            element.removeChild(this._button);
            this._disposeButton();

            var mainContainer = document.createElement("div");
            mainContainer.className = "emg_speaker_main_ctnr";
            mainContainer.id = element.id + "_flashPlayerCntr";
            element.appendChild(mainContainer);

            var flashPlayerCntr = document.createElement("div");
            flashPlayerCntr.className = "emg_speaker_flash_ctnr";


            if (typeof (FlashObject) != 'undefined') {
                var fo = new FlashObject(this.options.mp3PlayerUrl, "temp_swf", this.options.flashPlayerWidth, this.options.flashPlayerHeight, this.options.flashPlayerVersion);
                fo.addVariable('mp3', this.options.mp3FilesUrls);
                if (this.options.autoStart) {
                    fo.addVariable('autostart', '1');
                }
                if (this.options.autoReplay) {
                    fo.addVariable('autoreplay', '1');
                }
                if (this.options.showTime) {
                    fo.addVariable('showtime', '1');
                }
                if (this.options.randomPlay) {
                    fo.addVariable('randomplay', '1');
                }
                if (this.options.noPointer) {
                    fo.addVariable('nopointer', '1');
                }
                if (this.options.volume != 100) {
                    fo.addVariable('volume', this.options.volume);
                }
                if (this.options.enableTransparency) {
                    fo.addParam("wmode", "transparent");
                }
                if (this.options.backgroundColor != null && this.options.backgroundColor != "") {
                    fo.addParam("bgcolor", this.options.backgroundColor);
                    fo.addVariable("bgcolor", this.options.backgroundColor);
                }
                fo.write(flashPlayerCntr);
            }
            mainContainer.appendChild(flashPlayerCntr);

            var playerExtras = document.createElement("div");
            playerExtras.className = "emg_speaker_extras_cntr";

            if (this.options.showDownloadLink) {
                var downloadCntr = document.createElement("span");
                downloadCntr.className = "emg_speaker_download_cntr";
                var downloadLink = document.createElement("a");
                downloadLink.className = "emg_speaker_download_lnk";
                // unescape because value is encoded 
                downloadLink.href = unescape(this.options.downloadUrl);
                downloadLink.innerHTML = this.tokens.downloadToken;
                downloadCntr.appendChild(downloadLink);
                playerExtras.appendChild(downloadCntr);


                // Add pipe 
                var pipeCntr = document.createElement("span");
                pipeCntr.className = "emg_speaker_pipe_cntr";
                pipeCntr.innerHTML = "|";
                playerExtras.appendChild(pipeCntr);
            }

            // add attribution
            var attributionCntr = document.createElement("span");
            attributionCntr.className = "emg_speaker_attribution_cntr";
            attributionCntr.innerHTML = this.tokens.attributionToken;
            playerExtras.appendChild(attributionCntr);

            mainContainer.appendChild(playerExtras);
            return mainContainer;
        },

        _createButton: function () {
            var element = this.element;
            var button = document.createElement("a")
            button.className = "emg_speaker_button";
            button.href = "javascript:void(0)";
            button.title = this.tokens.listenToArticleToken;
            button.style.color = "#7c7ec9";
            button.style.textDecoration = "none";
            element.appendChild(button);

            // add button image
            var buttonImg = this._buttonImg = document.createElement("img");
            buttonImg.className = "emg_speaker_button_img";
            buttonImg.src = this.options.speakerImageUrl;
            //buttonImg.style.position = "relative";       
            buttonImg.border = "0";
            this.setZIndex(element, buttonImg);
            button.appendChild(buttonImg);

            // add text to button in a span
            var buttonSpan = document.createElement("span");
            buttonSpan.className = "emg_speaker_button_span";
            buttonSpan.innerHTML = this.tokens.listenToArticleToken;
            button.appendChild(buttonSpan);

            // add handlers to button  
            $button = $(button);
            $button.bind("click", this._buttonClickDelegate);
            $button.bind("mouseover", this._buttonMouseoverDelegate);
            $button.bind("mouseout", this._buttonMouseoutDelegate);

            return button;
        },

        setZIndex: function (element, target) {
            if (element && element.style && element.style.zIndex) {
                target.style.zIndex = element.style.zIndex + 1;
            }
            else {
                target.style.zIndex = 1;
            }
        },

        findParentElement: function (node, tagName, className) {
            /* USE: TO FIND THE PARENT OF A NODE USING TAGNAME AND CLASSNAME AS THE SEARCH CRETERIA. */
            /* NOTE: ONLY RETURNS THE FIRST NODE FOUND */
            for (; node != null; node = node.parentNode) {
                if (className && className.length > 0) {
                    if (node.nodeType == 1 &&
			            node.nodeName.toUpperCase() == tagName.toUpperCase() &&
			            (node.className == className || node.id == className)
			            ) {
                        return node;
                    }
                }
                else if (node.nodeType == 1 && node.nodeName.toUpperCase() == tagName.toUpperCase()) {
                    return node;
                }
            }
            return null;
        },

        get_button: function () {
            return _button;
        },

        // Utility
        _getCurrentStyle: function (element) {
            if (element.currentStyle) {
                return element.currentStyle;
            }
            var w = (element.ownerDocument ? element.ownerDocument : element.documentElement).defaultView;
            return ((w && (element !== w) && w.getComputedStyle) ? w.getComputedStyle(element, null) : element.style);
        }


    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_InlineMp3PlayerBehavior', DJ.UI.InlineMp3PlayerBehavior);

    $dj.debug('Registered DJ.UI.InlineMp3PlayerBehavior (extends DJ.UI.Component)'); 

})(jQuery);


//EMG.Toolkit.Web.InlineMp3PlayerBehavior = function (element) {
//    EMG.Toolkit.Web.InlineMp3PlayerBehavior.initializeBase(this, [element]);
//    
//    

//    //Events
//    this._buttonClickDelegate = $.proxy(this, this._buttonClickHandler);
//    this._buttonMouseoverDelegate = $.proxy(this, this._buttonMouseoverHandler);
//    this._buttonMouseoutDelegate = $.proxy(this, this._buttonMouseoutHandler);

//}


//EMG.Toolkit.Web.InlineMp3PlayerBehavior.registerClass('EMG.Toolkit.Web.InlineMp3PlayerBehavior', AjaxControlToolkit.BehaviorBase);




//EMG.Toolkit.Web.MP3PlayerType.registerEnum('EMG.Toolkit.Web.MP3PlayerType');