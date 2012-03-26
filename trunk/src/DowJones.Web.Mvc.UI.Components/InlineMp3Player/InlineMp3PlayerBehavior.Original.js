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


Type.registerNamespace('EMG.Toolkit.Web');

EMG.Toolkit.Web.InlineMp3PlayerBehavior = function(element) {
    EMG.Toolkit.Web.InlineMp3PlayerBehavior.initializeBase(this, [element]);
    this._button = null;
    this._isOpen = false;
    this._attributionToken = "Powered by ReadSpeaker";
    this._downloadToken = "Download";
    this._listenToArticleToken = "Listen to Article";
    this._speakerImageUrl = "";
    this._mp3PlayerType = EMG.Toolkit.Web.MP3PlayerType.Volume;
    this._mp3PlayerUrl = "";
    this._mp3FilesUrls = "";
    
    // dewplayer properties
    this._showDownloadLink = false;
    this._downloadUrl = "";
    this._autoStart = false;
    this._autoReplay = false;
    this._showTime = false;
    this._randomPlay = false;
    this._noPointer = false;
    this._volume = 100;
    this._enableTransparency = true;
    this._backgroundColor = "FFFFFF";
    
    // flash player properties
    this._falshPlayerWidth = "";
    this._flashPlayerHeight = "";
    this._flashPlayerVersion = "";
    
    // Version for dewPlayer 
    this._dewplayerversion = "";
    
    //Events
    this._buttonClickDelegate = Function.createDelegate(this, this._buttonClickHandler);
    this._buttonMouseoverDelegate = Function.createDelegate(this, this._buttonMouseoverHandler);
    this._buttonMouseoutDelegate = Function.createDelegate(this, this._buttonMouseoutHandler);
    
}

EMG.Toolkit.Web.InlineMp3PlayerBehavior.prototype = {
    initialize : function() {
        EMG.Toolkit.Web.InlineMp3PlayerBehavior.callBaseMethod(this, 'initialize');
        this._button = this._createButton();
    },

    dispose : function() {
        this._disposeButton();
        this._attributionToken = null;
        this._speakerImageUrl = null;
        this._mp3PlayerType = null;
        this._mp3PlayerUrl = null;
        this._mp3PlayerWidth = null;
        this._mp3PlayerHeight = null;
        
        // Call base dispose method
        EMG.Toolkit.Web.InlineMp3PlayerBehavior.callBaseMethod(this, 'dispose');
    },

    _disposeButton : function() {
        if (this._button) {
            $removeHandler(this._button, "click", this._buttonClickDelegate);
            $removeHandler(this._button, "mouseover", this._buttonMouseoverDelegate);
            $removeHandler(this._button, "mouseout", this._buttonMouseoutDelegate);
            this._button = null;
        } 
    },
    
    get_attributionToken : function() {
        return this._attributionToken;
    },
    set_attributionToken : function(value) {
        this._attributionToken = value;
    },
    
    get_downloadToken : function() {
        return this._downloadToken;
    },
    set_downloadToken : function(value) {
        this._downloadToken = value;
    },

    get_showDownloadLink: function() {
        return this._showDownloadLink;
    },
    set_showDownloadLink: function(value) {
        this._showDownloadLink = value;
    },
    
    get_listenToArticleToken: function() {
        return this._listenToArticleToken;
    },
    set_listenToArticleToken: function(value) {
        this._listenToArticleToken = value;
    },
    
    get_downloadUrl: function() {
        return this._downloadUrl;
    },
    set_downloadUrl: function(value) {
        this._downloadUrl = value;
    },

    get_speakerImageUrl : function() {
        return this._speakerImageUrl;
    },
    set_speakerImageUrl : function(value) {
        this._speakerImageUrl = value;
    },

    get_mp3PlayerType : function() {
        return this._mp3PlayerType;
    },
    set_mp3PlayerType : function(value) {
        this._mp3PlayerType = value;
    },

    // Uri of Player
    get_mp3PlayerUrl : function() {
        return this._mp3PlayerUrl;
    },
    set_mp3PlayerUrl : function(value) {
        this._mp3PlayerUrl = value;
    },
    
    // Width of Player
    get_mp3PlayerWidth : function() {
         return this._mp3PlayerWidth;
    },
    set_mp3PlayerWidth : function(value) {
         this._mp3PlayerWidth = value;
    },
    
    // Height of Player
    get_mp3PlayerHeight : function() {
         return this._mp3PlayerHeight;
    },
    set_mp3PlayerHeight : function(value) {
         this._mp3PlayerHeight = value;
    },
    
    // Height of Player
    get_mp3PlayerHeight : function() {
         return this._mp3PlayerHeight;
    },
    set_mp3PlayerHeight : function(value) {
         this._mp3PlayerHeight = value;
    },
    
    // AutoStart
    get_autoStart : function() {
         return this._autoStart;
    },
    set_autoStart : function(value) {
         this._autoStart = value;
    },
    
    // AutoReplay
    get_autoReplay : function() {
         return this._autoReplay;
    },
    set_autoReplay : function(value) {
         this._autoReplay = value;
    },
    
    // ShowTime
    get_showTime : function() {
         return this._showTime;
    },
    set_showTime : function(value) {
         this._showTime = value;
    },
    
    // RandomPlay
    get_randomPlay : function() {
         return this._randomPlay;
    },
    set_randomPlay : function(value) {
         this._randomPlay = value;
    },
    
    // NoPointer
    get_noPointer : function() {
         return this._noPointer;
    },
    set_noPointer : function(value) {
         this._noPointer = value;
    },
    
    // Volume
    get_volume : function() {
         return this._volume;
    },
    set_volume : function(value) {
         this._volume = value;
    },
    
    // EnableTransparency
    get_enableTransparency : function() {
         return this._enableTransparency;
    },
    set_enableTransparency : function(value) {
         this._enableTransparency = value;
    },
    
    // BackgroundColor
    get_backgroundColor : function() {
         return this._backgroundColor;
    },
    set_backgroundColor : function(value) {
         this._backgroundColor = value;
    },
    
    // FlashPlayerWidth
    get_flashPlayerWidth : function() {
         return this._flashPlayerWidth;
    },
    set_flashPlayerWidth : function(value) {
         this._flashPlayerWidth = value;
    },
    
    // FlashPlayerHeight
    get_flashPlayerHeight : function() {
         return this._flashPlayerHeight;
    },
    set_flashPlayerHeight : function(value) {
         this._flashPlayerHeight = value;
    },
    
    // FlashPlayerVersion
    get_flashPlayerVersion : function() {
         return this._flashPlayerVersion;
    },
    set_flashPlayerVersion : function(value) {
         this._flashPlayerVersion = value;
    },
    
    // DewplayerVersion
    get_dewplayerVersion : function() {
         return this._dewplayerVersion;
    },
    set_dewplayerVersion : function(value) {
         this._dewplayerVersion = value;
    },
    
    // Mp3FilesUrls
    get_mp3FilesUrls : function() {
         return this._mp3FilesUrls;
    },
    set_mp3FilesUrls : function(value) {
         this._mp3FilesUrls = value;
    },
    
    _createFlashPlayer : function() {
        
        var element = this.get_element();
        
        element.removeChild(this._button);
        this._disposeButton();
        
        var mainContainer = document.createElement("div");
        Sys.UI.DomElement.addCssClass(mainContainer, "emg_speaker_main_ctnr");
        mainContainer.id = element.id + "_flashPlayerCntr";  
        element.appendChild(mainContainer); 
        
        var flashPlayerCntr = document.createElement("div");
        Sys.UI.DomElement.addCssClass(flashPlayerCntr, "emg_speaker_flash_ctnr");  
                  
        
        if ( typeof(FlashObject) != 'undefined') {
            var fo = new FlashObject(this._mp3PlayerUrl, "temp_swf", this._flashPlayerWidth, this._flashPlayerHeight, this._flashPlayerVersion);
            fo.addVariable('mp3', this._mp3FilesUrls);
            if (this._autoStart){
                fo.addVariable('autostart','1');
            }
            if (this._autoReplay){
                fo.addVariable('autoreplay','1');
            }
            if (this._showTime){
                fo.addVariable('showtime','1');
            }
            if (this._randomPlay){
                fo.addVariable('randomplay','1');
            }
            if (this._noPointer){
                fo.addVariable('nopointer','1');
            }
            if (this._volume != 100){
                fo.addVariable('volume',this._volume);
            }
            if (this._enableTransparency ){
                fo.addParam("wmode","transparent");
            }
            if (this._backgroundColor != null && this._backgroundColor != ""){
                fo.addParam("bgcolor", this._backgroundColor);
                fo.addVariable("bgcolor", this._backgroundColor);
            }
            fo.write(flashPlayerCntr);
        }
        mainContainer.appendChild(flashPlayerCntr);
        
        var playerExtras = document.createElement("div"); 
        Sys.UI.DomElement.addCssClass(playerExtras, "emg_speaker_extras_cntr"); 
        if (this._showDownloadLink){
            var downloadCntr = document.createElement("span");
            Sys.UI.DomElement.addCssClass(downloadCntr, "emg_speaker_download_cntr");              
            var downloadLink = document.createElement("a");
            Sys.UI.DomElement.addCssClass(downloadLink, "emg_speaker_download_lnk"); 
            // unescape because value is encoded 
            downloadLink.href = unescape(this._downloadUrl);
            downloadLink.innerHTML = this._downloadToken;
            downloadCntr.appendChild(downloadLink);
            playerExtras.appendChild(downloadCntr);
            
            
            // Add pipe 
            var pipeCntr = document.createElement("span");
            Sys.UI.DomElement.addCssClass(pipeCntr, "emg_speaker_pipe_cntr");   
            pipeCntr.innerHTML = "|";
            playerExtras.appendChild(pipeCntr);
        }
        
        // add attribution
        var attributionCntr = document.createElement("span");
        Sys.UI.DomElement.addCssClass(attributionCntr, "emg_speaker_attribution_cntr");   
        attributionCntr.innerHTML = this._attributionToken;
        playerExtras.appendChild(attributionCntr); 
        
        mainContainer.appendChild(playerExtras);        
        return mainContainer;
    },

    _createButton : function() {
        var element = this.get_element();
        var button = document.createElement("a")
        Sys.UI.DomElement.addCssClass(button, "emg_speaker_button");
        button.href = "javascript:void(0)";
        button.title = this._listenToArticleToken;
        button.style.color = "#7c7ec9";
        button.style.textDecoration = "none";
        element.appendChild(button);
        
        // add button image
        var buttonImg = this._buttonImg = document.createElement("img");
        Sys.UI.DomElement.addCssClass(buttonImg, "emg_speaker_button_img");
        buttonImg.src = this._speakerImageUrl;
        //buttonImg.style.position = "relative";       
        buttonImg.border = "0";
        this.setZIndex(element,buttonImg);
        button.appendChild(buttonImg);
        
        // add text to button in a span
        var buttonSpan = document.createElement("span");
        Sys.UI.DomElement.addCssClass(buttonSpan, "emg_speaker_button_span");
        buttonSpan.innerHTML = this._listenToArticleToken;
        button.appendChild(buttonSpan);
         
        // add handlers to button       
        $addHandler(button, "click", this._buttonClickDelegate);
        $addHandler(button, "mouseover", this._buttonMouseoverDelegate);
        $addHandler(button, "mouseout", this._buttonMouseoutDelegate);
        return button;
    },

    setZIndex : function(element,target){
        if (element && element.style && element.style.zIndex)
        {
            target.style.zIndex = element.style.zIndex+1;
        }
        else
        {
            target.style.zIndex = 1;
        }
    },
    
    _buttonClickHandler : function(e) {
        this._createFlashPlayer();
        return false;
    },
    
    _buttonMouseoverHandler : function(eventArgs) {
        var node = this.findParentElement(eventArgs.target,"a","");
        if(node)Sys.UI.DomElement.addCssClass(node, "emg_speaker_button_hover");
    },
        
    _buttonMouseoutHandler : function(eventArgs) {
        var node = this.findParentElement(eventArgs.target,"a","");
        if(node)Sys.UI.DomElement.removeCssClass(node, "emg_speaker_button_hover");
    },
    
    findParentElement: function (node,tagName,className){
	    /* USE: TO FIND THE PARENT OF A NODE USING TAGNAME AND CLASSNAME AS THE SEARCH CRETERIA. */
	    /* NOTE: ONLY RETURNS THE FIRST NODE FOUND */
	    for(;node != null; node = node.parentNode) {
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
    
    get_button : function() {
        return this._button;
    },  
    
    // Utility
    _getCurrentStyle : function(element) {
        if(element.currentStyle)
        {
            return element.currentStyle;
        }
        var w = (element.ownerDocument ? element.ownerDocument : element.documentElement).defaultView;
        return ((w && (element !== w) && w.getComputedStyle) ? w.getComputedStyle(element, null) : element.style);
    }
}

EMG.Toolkit.Web.InlineMp3PlayerBehavior.registerClass('EMG.Toolkit.Web.InlineMp3PlayerBehavior', AjaxControlToolkit.BehaviorBase);


EMG.Toolkit.Web.MP3PlayerType = function() {
    throw Error.invalidOperation();
}

EMG.Toolkit.Web.MP3PlayerType.prototype = {
    Mini : 0,
    Normal : 1,
    Volume : 2,
    Multiple : 3,
    ReadSpeaker : 4,
    DowJones : 5
}

EMG.Toolkit.Web.MP3PlayerType.registerEnum('EMG.Toolkit.Web.MP3PlayerType');