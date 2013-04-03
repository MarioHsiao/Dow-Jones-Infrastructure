/* Predefined Function page onload handler */ 
function pageLoad(){
    /* initialize designer handlers */
    if (typeof(emg_baseScreen) != "undefined" && 
        typeof(emg_baseScreen) == "string") {
            if (emg_baseScreen == "designer") {
                addDesignerHandlers();
                /* Get Headlines */
                initDesigner();
            }
            if (emg_baseScreen == "management") {
                initManagement();
            }
    }    
}

/* Predefined Function page onunload handler */ 
function pageUnload() {
    if (typeof(emg_baseScreen) != "undefined" && 
        typeof(emg_baseScreen) == "string") {
            if (emg_baseScreen == "designer") {
                removeDesignerHandlers();
            }
    } 
}

/* Start of TabContainer Methods */
function fireMyWidgetDesigner() {
    hideWidgetList();
    setAction(1);
}

// Fire My widget link
function fireMyWidgets() {
    setAction(2);
}

function fireWidgetGallery() {
    hideWidgetList();
    setAction(3);
}

function setAction(value) {
    var actionInput  = $get("at");
    if (actionInput) {
        actionInput.value = value;
        var form = getBForm();
        form.submit();
    }
    return;
}

function hideWidgetList() {
    if ($get("wPreviewContainer")){
        $("#wPreviewContainer").css({display:"none",visibility:"hidden"});
    }
}

function ActiveTabChanged() {
}
/* End of Tab Container Methods*/

function addDesignerHandlers() {
    
    switch(emg_widgetType) {
        case "AlertHeadlineWidget":
            addDesignerHandlersForAlertHeadlineWidget();
            break;
        case "AutomaticWorkspaceWidget":
            addDesignerHandlersForAutomaticWorkspaceWidget();
            break;
        case "ManualNewsletterWorkspaceWidget":
            addDesignerHandlersForManualNewsletterWorkspaceWidget();            
            break;
    }
}

function removeDesignerHandlers() {
    switch(emg_widgetType) {
        case "AlertHeadlineWidget":
            removeDesignerHandlersForAlertHeadlineWidget();
            break;
        case "AutomaticWorkspaceWidget":
            removeDesignerHandlersForAutomaticWorkspaceWidget();
            break;
        case "ManualNewsletterWorkspaceWidget":
            removeDesignerHandlersForManualNewsletterWorkspaceWidget();        
            break;
    }
}

function initManagement() {
    var tContainer = $find("wTabContainer");
    
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');

    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show(); 
    $get("mPopHeader").innerHTML = _translate("widgetManagement");
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), true);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);        
    CommonToolkitScripts.setVisible($get("codeCntr"), false);
    CommonToolkitScripts.setVisible($get("divBase"), false);
    
    fireInitialSort(true); 
}

function removeManagement() {
}

function onColorPickerUpdate() {
     switch(emg_widgetType) {
        case "AlertHeadlineWidget":
            window.setTimeout("updateAlertHeadlineWidgetPreview()",1);
            break;
        case "AutomaticWorkspaceWidget":
            window.setTimeout("updateAutomaticWorkspaceWidgetPreview()",1);
            break;
        case "ManualNewsletterWorkspaceWidget":
            window.setTimeout("updateManualNewsletterWorkspaceWidgetPreview()",1);            
            break;
    }
}

function updateProxyCredAuthScheme(leavePasswordValue) {
    switch(cInt(getAuthScheme())){   
        case 0:
            CommonToolkitScripts.setVisible($get("proxyCredEmailAddressRow"), false);  
            CommonToolkitScripts.setVisible($get("proxyCredUserIdRow"), true); 
            CommonToolkitScripts.setVisible($get("proxyCredNamespaceRow"), true);
            break;
        default:
            CommonToolkitScripts.setVisible($get("proxyCredEmailAddressRow"), true);  
            CommonToolkitScripts.setVisible($get("proxyCredUserIdRow"), false); 
            CommonToolkitScripts.setVisible($get("proxyCredNamespaceRow"), false);
            break;
    }
    if (!leavePasswordValue){
        $get("proxyCredPassword").value = "";
    }
}

function updateProxyCredAuthSchemeHandler() {
     updateProxyCredAuthScheme(false);
}

function fireInitialSort(doNotShowLoading) {

    var extender = $find('wPreviewExtender');
    if (extender._widgets.length <= 1 && extender._addedWidgets) return;    
    if (!doNotShowLoading) extender.showLoadingArea();
    extender.removeAllWidgets();
    
    EMG.widgets.services.WidgetDesignerManager.GetWidgetList(
        EMG.widgets.ui.dto.WidgetSortBy.Date,
        accessPointCode,
        interfaceLanguage,
        sa_from,
        initManagement_success,
        generic_failure, 
        $find('wPreviewExtender')
    );

}

function initDesigner() {
    switch(emg_widgetType) {
        case "AlertHeadlineWidget":
            initDesignerForAlertHeadlineWidget();
            break;
        case "AutomaticWorkspaceWidget":
            initDesignerForAutomaticWorkspaceWidget();
            break;
        case "ManualNewsletterWorkspaceWidget":
            initDesignerForManualNewsletterWorkspaceWidget();        
            break;
    }
}

function getAudience() {
    var tForm = getBForm();
    var audienceList = tForm.elements["audience"];
                
    for(var i=0; i<audienceList.length; i++) {
        if (audienceList[i].checked) {
            return cInt(audienceList[i].value);
        }
    }
    return "0";
}

function getAuthScheme() {
    var tForm = getBForm();
    var authSchemeList = tForm.elements["authScheme"];
    if (authSchemeList && authSchemeList.length) {
        for (var i = 0; i < authSchemeList.length; i++) {
            if (authSchemeList[i].checked) {
                return authSchemeList[i].value;
            }
        }
    }
    return "0";
}

function hideModalPopupViaClient() {
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.hide();
}

function showFrameworkError(result){
    var message = "-1: Framework Error.";
    if (result != null && result.ReturnCode != 0) {
        var _errorCode = cInt(result.ReturnCode);
        switch(_errorCode) {
            case -2147176633:
            case -2147183530:
                sessTimeOut();
                return;
                break;
             default:
                message = result.ReturnCode + ": " + result.StatusMessage;  
                break;
        }
        message = result.ReturnCode + ": " + result.StatusMessage;            
    }
    showError(message);
    return; 
}

function sessTimeOut() {
    var returnUrlInput  = $get("returnurl");
    if (returnUrlInput) {
        var returnUrlValue = cStr(returnUrlInput.value).trim();
        if (returnUrlValue != null && returnUrlValue.length > 0 ) {
            window.location.href = returnUrlValue;
        }
    }
}

function updateBadgets(result) {
    if (typeof(result) == 'undefined' || result == null){return;}
    if (typeof(result.IntegrationEndPoints) == 'undefined' || 
        result.IntegrationEndPoints == null ||
        result.IntegrationEndPoints.length == 0)
        {return;}
        
    var _badge_sb = new Sys.StringBuilder();
    for (var i=0; i<result.IntegrationEndPoints.length;i++) {
        var IEndPoint = result.IntegrationEndPoints[i];
                
        _badge_sb.append("<div class=\"iBadge\">");
        _badge_sb.append("<a href=\"" + IEndPoint.IntegrationUrl + "\" onclick=\"xWinOpen(this.href);return false;\" title=\"" + _translate("integrate") + "\">");
        if (IEndPoint.HasIcon) _badge_sb.append("<img class=\"iIcon\" src=\"" + IEndPoint.Icon + "\"/>");
        _badge_sb.append("<span class=\"iBadge\">" + IEndPoint.Title + "</span>"); 
        _badge_sb.append("</a>");
        _badge_sb.append("</div>"); 
    }
    $get("publishingContainer").innerHTML = _badge_sb.toString();
}

function updateAccordionPanes(result) {
    if (typeof(result) == 'undefined' || result == null){return;}
    if (typeof(result.IntegrationEndPoints) == 'undefined' || 
        result.IntegrationEndPoints == null ||
        result.IntegrationEndPoints.length == 0)
        {return;}
    for (var i=0; i<result.IntegrationEndPoints.length;i++) {
        var IEndPoint = result.IntegrationEndPoints[i];
        var _header_sb = new Sys.StringBuilder();
        var _content_sb = new Sys.StringBuilder();
        
        _header_sb.append("<div class=\"hBadge\">");
        if (IEndPoint.HasIcon) _header_sb.append("<img class=\"bIcon\" src=\"" + IEndPoint.Icon + "\"/>");
        _header_sb.append("<span class=\"bTitle\">" + IEndPoint.Title + "</span>"); 
        _header_sb.append("</div>"); 
        
        _content_sb.append("<div class=\"bContents\">");
        _content_sb.append("<div class=\"bInstructions\">");
        _content_sb.append(_translate("getPortalWidgetIntructions"));
        _content_sb.append("</div>");
        _content_sb.append("<div class=\"bLink\">");
        _content_sb.append("<a href=\"" + IEndPoint.IntegrationUrl + "\" onclick=\"xWinOpen(this.href);return false;\"><span class=\"iBadge\" title=\"" + _translate("integrate") + "\">" + IEndPoint.Title +  "</span></a>");
        _content_sb.append("</div>");  
         _content_sb.append("<div class=\"bInstructions\">");
        _content_sb.append(_translate("getUrlToPortalWidgetIntructions"));
        _content_sb.append("</div>");   
        _content_sb.append("<div class=\"bLink\">");
        _content_sb.append("<input name=\"integration_" + i + "\" style=\"width:90%\" value=\"" + IEndPoint.IntegrationUrl + "\" />");
        _content_sb.append("</div>");     
        _content_sb.append("</div>");
        addPane(_header_sb.toString(), _content_sb.toString());
    }    
}

function removeAccordionPanes(){
   var behavior = $find('codeAccordion_AccordionExtender');  
   if (behavior) {
       var root = behavior.get_element(); 
       behavior.dispose(); 
       
       if (typeof(root) == 'undefined' || root == null){return;}  
          
       // Clean up DOM
       var len = root.childNodes.length;
       for (var i = len - 1; i >= 2; i--) {
            xRemoveChild(root, root.childNodes[i]);
       }
       $create(AjaxControlToolkit.AccordionBehavior, {"ClientStateFieldID":"codeAccordion_AccordionExtender_ClientState","ContentCssClass":"accordionContent","FadeTransitions":true,"FramesPerSecond":40,"HeaderCssClass":"accordionHeader","SelectedIndex":-1,"id":"codeAccordion_AccordionExtender","requireOpenedPane":false,"suppressHeaderPostbacks":true}, null, null, $get("codeAccordion"));	  
   }
}

function removeWidgetItems() {
    var behavior = $find('wPreviewExtender');
    if (behavior) {
        var root = behavior.get_element();
        behavior.dispose();
        
        if (typeof(root) == 'undefined' || root == null){return;}  
          
        // Clean up DOM
        var len = root.childNodes.length;
        for (var i = len - 1; i >= 2; i--) {
            xRemoveChild(root, root.childNodes[i]);
        }
    }
}

function updateWidgetContentItems(result){
    if (typeof(result) == 'undefined' || result == null){return;}
    if (typeof(result.Widgets) == 'undefined' || 
        result.Widgets == null ||
        result.Widgets.length == 0)
        {return;}
    for (var i=0; i<result.Widgets.length;i++) {
        
    }
}

function addWidgetItem(widgetDelegate){
    var behavior = $find('wPreviewExtender');     
    if (behavior) {   
        var root = behavior.get_element();  
        behavior.addWidget(widgetDelegate);  
    }
}

function addWidgets(widgetDelegate) {
    var behavior = $find("wPreviewExtender");
    if (behavior) {
        var root = behavior.get_element();
        behavior.addWidgets(widgetDelegate);
    }
}

function addPane(headerHTML, contentHTML)    {               
    var behavior = $find('codeAccordion_AccordionExtender');        
    if (behavior) {
        var root = behavior.get_element();        
        var header = document.createElement("div"); 
        header.className = "accordionHeader";
        header.innerHTML = headerHTML;                
        var paneContent = document.createElement("div");
        paneContent.className = "accordionContent";        
        paneContent.innerHTML = contentHTML;                
        root.appendChild(header);        
        root.appendChild(paneContent);        
        behavior.addPane(header,paneContent);
    }    
}

/* Common Message Handling */
function showError(message){
     var modalPopupBehavior = $find('programmaticModalPopupBehavior');
     var modalErrorPopupBehavior = $find('programmaticErrorModalPopupBehavior');
     modalPopupBehavior.hide();
     modalErrorPopupBehavior.hide();
     
     var errorMessageDiv = $get("errorMsg");
     var errorLabel = $get('errorLbl');
     errorLabel.innerHTML = _translate("error"); 
     errorMessageDiv.innerHTML = message;
     modalErrorPopupBehavior.set_Y(100);
     modalErrorPopupBehavior.show(); 
}

function showWarning(message){
     var modalPopupBehavior = $find('programmaticErrorModalPopupBehavior');
     modalPopupBehavior.hide();
     var errorMessageDiv = $get("errorMsg");
     var errorLabel = $get('errorLbl');     
     errorLabel.innerHTML = _translate("warning");     
     errorMessageDiv.innerHTML = message;
     modalPopupBehavior.set_Y(100);
     modalPopupBehavior.show(); 
}

function showPreview(result,userContext){
    
     var modalPopupBehavior = $find('programmaticErrorModalPopupBehavior');
     modalPopupBehavior.hide();
     var errorMessageDiv = $get("errorMsg");
     var errorLabel = $get('errorLbl');     
     errorLabel.innerHTML = _translate("preview"); 
     FactivaWidgetRenderManager.getInstance().xBuildAlertWidget(errorMessageDiv,result,true); 
     
     modalPopupBehavior.set_Y(100);
     modalPopupBehavior.show(); 
}

function showMessage(message){
     var modalPopupBehavior = $find('programmaticErrorModalPopupBehavior');
     modalPopupBehavior.hide();
     var errorMessageDiv = $get("errorMsg");
     var errorLabel = $get('errorLbl');     
     errorLabel.innerHTML = _translate("message");     
     errorMessageDiv.innerHTML = message;
     modalPopupBehavior.set_Y(100);     
     modalPopupBehavior.show(); 
}

function showMovie(){
    var _modalPopupBehavior = $find('programmaticMovieModalPopupBehavior');
    var _movieMessageDiv = $get("movieContainer");       
    if ( typeof(FlashObject) != 'undefined' && movieData) {					
        var fo = new FlashObject(movieData.source, "temp_swf", movieData.width, movieData.height, movieData.targetFlashPlayerVersion);					
        fo.write(_movieMessageDiv);		
    } 
    _modalPopupBehavior.set_Y(10);
    _modalPopupBehavior.show();  
}

function showAutoMovie() {
    if (arguments.callee.done) return;
    arguments.callee.done = true;
    showMovie();
}

// global variable used for the widget preview popup extender
var extender = null;

function updateWidgetsViewPreview(result,userContext){
    
    var wDelegate = userContext.eventArgs.get_WidgetDelegate();
    
    var parent = $get("appendAJAX");
    var previewPopupContainer = document.createElement("div");
    previewPopupContainer.id = "preview_popup_container";
    Sys.UI.DomElement.addCssClass(previewPopupContainer, "ajax__widget_preview_popup_container");
    
    var previewPopupContent = document.createElement("div");
    previewPopupContent.id = "preview_popup_content";
    Sys.UI.DomElement.addCssClass(previewPopupContent, "ajax__widget_preview_popup_content");
    
    var previewPopupHeader = document.createElement("div");
    Sys.UI.DomElement.addCssClass(previewPopupHeader, "ajax__widget_preview_popup_header");

    var previewPopupHeaderText = document.createElement("span");
    Sys.UI.DomElement.addCssClass(previewPopupHeaderText, "ajax__widget_preview_popup_header_text");
    previewPopupHeaderText.innerHTML = "Widget Preview";

    var previewPopupClose = document.createElement("div");
    Sys.UI.DomElement.addCssClass(previewPopupClose, "ajax__widget_preview_popup_close");
    previewPopupClose.href = "javascript:void(0)";
    previewPopupClose.innerHTML = "<a>X Close</a>";
    $addHandler(previewPopupClose, "click", hidePreviewPopup);
    
    previewPopupHeader.appendChild(previewPopupHeaderText);
    previewPopupHeader.appendChild(previewPopupClose);
    previewPopupContainer.appendChild(previewPopupHeader);
    previewPopupContainer.appendChild(previewPopupContent);
    parent.appendChild(previewPopupContainer);

    extender = $create(AjaxControlToolkit.PopupBehavior, 
        { "parentElement":parent, "positioningMode":0, "x":405 , "y":60, "id":"previewPopupBehavior" }, null, null, previewPopupContainer);  
    extender.show();

    switch(wDelegate.Type) {
        case 0:
            FactivaWidgetRenderManager.getInstance().xBuildAlertWidget(previewPopupContent,result,true); 
            break;
        case 1:
            FactivaWidgetRenderManager.getInstance().xBuildAutomaticWorkspaceWidget(previewPopupContent,result,true); 
            break;
        case 2:
            FactivaWidgetRenderManager.getInstance().xBuildManualNewsletterWorkspaceWidget(previewPopupContent,result,true); 
            break;
    }
}

function hidePreviewPopup() {
    
    // hide the popup extender
    extender.hide();
    extender.dispose();
    
    $("select").css({visibility:"visible"});
    
    // remove the preview container div from the appendAJAX parent div
    $(".ajax__widget_preview_popup_container").remove();
}

function loadScriptCode(url,tElement) {           
    var script=document.createElement('script'); // pretty self explanatory
    script.src = url; // sets the scripts source to the specified url
    script.type = "text/javascript";
    script.charset = "utf-8";
    if (tElement) {tElement.appendChild(script);}    
}


/* Validation Routines */

/* Failure requests */
function generic_failure(error, userContext, methodName){
    showFrameworkError();
}

/* Successful Requests */
function initManagement_success(result, userContext, methodName) {
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.hide(); 
    
    if (result != null && result.ReturnCode != 0) {
        showFrameworkError(result);
    }
    else if (result != null && result.ReturnCode == 0) {
        updateWidgets(result, userContext);
        if (result.Widgets != null && result.Widgets.length > 0) {
            renderTableSorter();
        }
    }
    else 
    {
        showFrameworkError();
    }
}

function removeWidgets() {
}

function renderTableSorter() {
    var behavior = $find("wPreviewExtender");
    
    $create(EMG.Toolkit.Web.TableSorterBehavior, 
        { "headers":"{3:{sorter:false}}"
        , "sortList":($get("sortOrder").value!=null) ? $get("sortOrder").value : "[[2,1]]"
        , "widthFixed": true
        , "debug" : false
        , "id": "tableSorterBehavior" }, null, null, $get("widgetListTable"));        
    $create(EMG.Toolkit.Web.TableSorterFilterBehavior,
        { "filterContainer":"searchTextInputCtrl"
        , "filterClearContainer":"clearSearchCtrl"
        , "filterColumns":"[0]"
        , "filterCaseSensitive":false
        , "id":"tableSorterFilterBehavior" }, null, null, $get("widgetListTable"));
    $create(EMG.Toolkit.Web.TableSorterPagingBehavior,
        { "firstImage":behavior.get_firstImage()
        , "lastImage":behavior.get_lastImage()
        , "nextImage":behavior.get_nextImage()
        , "prevImage":behavior.get_prevImage()
        , "firstToken":behavior.get_firstToken()
        , "lastToken":behavior.get_lastToken()
        , "nextToken":behavior.get_nextToken()
        , "previousToken":behavior.get_previousToken()
        , "resultsPerPageToken":behavior.get_resultsPerPageToken()
        , "size":($get("pagesize").value!=null) ? parseInt($get("pagesize").value) : 20
        , "page":($get("page").value!=null) ? parseInt($get("page").value) : 0        
        , "id":"tableSorterPagingBehavior" }, null, null, $get("widgetListTable"));
    $addHandler($get("typeFilterCtrl"), "change", fireTypeFilter);
    $addHandler($get("clearSearchCtrl"), "click", clearTypeFilter);
    
    // bind events to capture page/page size in hidden input control
    $(".pagesize").change(function(){ 
        updatePageSlashPageSizeHiddenInput();
    });
    $(".first").click(function(){
        updatePageSlashPageSizeHiddenInput();
    });
    $(".prev").click(function(){
        updatePageSlashPageSizeHiddenInput();
    });
    $(".next").click(function(){
        updatePageSlashPageSizeHiddenInput();
    });
    $(".last").click(function(){
        updatePageSlashPageSizeHiddenInput();
    });
    // bind events to capture sort order in hidden input control
    $("th").click(function() {
        updateSortOrder();
    });
    
    var timer;
    $("#searchTextInputCtrl").keyup(function(){
        $get("typeFilterCtrl").selectedIndex = 0;
        clearTimeout(timer);
        timer = setTimeout(searchCallback, 1005);
        return false;
    });
    $("#typeFilterCtrl").change(function(){
        $get("searchTextInputCtrl").value = "";
        clearTimeout(timer);
        timer = setTimeout(searchCallback, 1);
    });
    $("#searchButtonCtrl").click(function(){
        var filterText = $get("searchTextInputCtrl").value;
        $("#widgetListTable").trigger("invokeFilter", [filterText]);
        clearTimeout(timer);
        timer = setTimeout(searchCallback, 1);
    });    
}

function disposeTableSorter() {
    $find("tableSorterPagingBehavior").dispose();
    $find("tableSorterFilterBehavior").dispose();
    $find("tableSorterBehavior").dispose();
    $removeHandler($get("typeFilterCtrl"), "change", fireTypeFilter);
    $removeHandler($get("clearSearchCtrl"), "click", clearTypeFilter);
    $(".pagesize").unbind();
    $(".first").unbind();
    $(".prev").unbind();
    $(".next").unbind();
    $(".last").unbind();
    $("th").unbind();
    $("#searchButtonCtrl").unbind();
}

function fireTypeFilter(e) {
    var filterIndex = $get("typeFilterCtrl").selectedIndex;
    var filterText = $get("typeFilterCtrl").options[filterIndex].value;
    $("#widgetListTable").trigger("invokeColumnFilter", [filterText, [1]]);
}

function clearTypeFilter(e) {
    var filterIndex = $get("typeFilterCtrl").selectedIndex = 0;
    $("#pager").css({
        display: "block",
        visibility: "visible"
    });
    $(".ajax__widget_noWidgets").css({
        display: "none",
        visibility: "hidden"
    });
}

function updatePageSlashPageSizeHiddenInput() {
    var value = $(".pagesize").selectedOptions()[0].value;
    var config = $("#widgetListTable").trigger("getConfig")[0].config;
    
    $get("pagesize").value = value;
    $get("page").value = config.page;
}

function updateSortOrder() {
    var config = $("#widgetListTable").trigger("getConfig")[0].config;
    var arraySortOrder = config.sortList;
    var stringSortOrder = "";

    for(var i=arraySortOrder.length-1; i>=0; i--) {
        if (i > 0)
            stringSortOrder += "[" + cStr(arraySortOrder[i]) + "],";
        else
            stringSortOrder += "[" + cStr(arraySortOrder[i]) + "]";
    }

    $get("sortOrder").value = "[" + stringSortOrder + "]";
}

function searchCallback() {
    var table = $get("widgetListTable");
    if (table.rows.length <= 1) {
        $("#pager").css({
            display: "none",
            visibility: "hidden"
        });
        $(".ajax__widget_noWidgets").css({
            display: "block",
            visibility: "visible"
        });
    } else {
        $("#pager").css({
            display: "block",
            visibility: "visible"
        });
        $(".ajax__widget_noWidgets").css({
            display: "none",
            visibility: "hidden"
        });
    }
}

function hideSearchControls() {
    $(".searchTextCtrlContainer").css({display:"block", visibility:"hidden"});
    $(".typeFilterCtrlContainer").css({display:"block", visibility:"hidden"});
}

function validateColor(value, defaultColor){
    var temp = trim(cStr(value));
    if  (/^#[a-f0-9]{6}$/i.test(value)){
        return temp;
    }
    return defaultColor;
}

function updateWidgets(result,userContext) {
    if (typeof(result) == 'undefined' || result == null){return;}
    if (typeof(result.Widgets) == 'undefined' || 
        result.Widgets == null ||
        result.Widgets.length == 0){
        // Show movie 
        var tContainer = $find("wTabContainer");
        var widgetIdInput  = $get("wid");
        if (tContainer && widgetIdInput && (widgetIdInput.value != null && widgetIdInput.value.length > 0)) {
            tContainer.getFirstTab()._hide();
        }
        hideSearchControls();
        userContext.showNoWidgetsArea();
        showAutoMovie();
        return;
    }
    userContext.removeAllWidgets();
    
    // need to modify this logic causes 
    // script loading exception in browser
    /*for (var i=0; i<result.Widgets.length;i++) {
        addWidgetItem(result.Widgets[i]);
    }*/

    if (result.Widgets.length > 0) {
        addWidgets(result.Widgets);
    }
}

function createWidget_success(result, userContext, methodName) {
    if (result != null && result.ReturnCode != 0) {
        showFrameworkError(result);
    }
    else if (result != null && result.ReturnCode == 0) {
        // Display Update and Code Generation Controls
        $get("wdgtUpdateCntrl").style.display = "block";
        $get("wdgtCodeGenCntrl").style.display = "block";   
        // Hide Create Controls
        $get("wdgtCreateCntrl").style.display = "none"; 
        
        var wIdHiddenInput = $get("wid");
        if (wIdHiddenInput) {
            wIdHiddenInput.value = result.WidgetId;
        }
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.hide();
        //showMessage(_translate("widgetHasBeenCreated"));
        return;
    }
    else {
        showFrameworkError();
    }
    
    // Display Update and Code Generation Controls
    CommonToolkitScripts.setVisible($get("wdgtCreateCntrl"), true);     
    CommonToolkitScripts.setVisible($get("wdgtUpdateCntrl"), false);
    CommonToolkitScripts.setVisible($get("wdgtCodeGenCntrl"), false);
    return;
}

function updateWidget_success(result, userContext, methodName) {
    if (result != null && result.ReturnCode != 0) {
        showFrameworkError(result);
    }
    else if (result != null && result.ReturnCode == 0) {
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.hide();
        //showMessage(_translate("widgetHasBeenUpdated"));
    }
    else {
        showFrameworkError();
    }
    // Display Update and Code Generation Controls
     CommonToolkitScripts.setVisible($get("wdgtCreateCntrl"), false);     
     CommonToolkitScripts.setVisible($get("wdgtUpdateCntrl"), true);
     CommonToolkitScripts.setVisible($get("wdgtCodeGenCntrl"), true);
    return;
}

function getPreview_success(result,userContext, methodName) {
    if (result != null && result.ReturnCode != 0) {
        showFrameworkError(result);
        userContext.sender.showContentArea();
    }
    else if (result != null && result.ReturnCode == 0) {
        // Process the return delegate
        updateWidgetsViewPreview(result,userContext);

        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.hide();
        
        $("select").css({visibility:"hidden"});
        
        return;
    } 
    else {
         showFrameworkError();
    }
}

function getScriptCode_success(result, userContext, methodName) {
    
    if (result != null && result.ReturnCode != 0) {
        showFrameworkError(result);
    }
    else if (result != null && result.ReturnCode == 0) {
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        var txtArea = $get("scriptCode");
        var pubArea = $get("publishingContainer");        
        //var http = $get("http");
        //var https = $get("https");
        $get("mPopHeader").innerHTML = _translate("displayCode");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);        
        CommonToolkitScripts.setVisible($get("codeCntr"), true);
        CommonToolkitScripts.setVisible($get("divBase"), true);
        if (txtArea) {
            // Process the return delegate
            txtArea.value = result.JavaSciptWidgetCodeSnippet;
            addSecurityToTextArea();
            var behavior = $find('codeAccordion_AccordionExtender');  
            if (behavior) {
                removeAccordionPanes();
                updateAccordionPanes(result);   
            }
            // just add simple badges
            else {
                updateBadgets(result);
            }      
            txtArea.select();        
        }
        
        // Add handlers for HTTP / HTTPS click
        if ($get("http") && $get("https")) {
            $addHandler($get("http"), 'click', addSecurityToTextArea);
            $addHandler($get("https"), 'click', addSecurityToTextArea);
        }
    } 
    else {
         showFrameworkError();
    }
}

var BASELINE_ERROR = 520400;
var NO_VALID_FOLDERS = BASELINE_ERROR + 10;
var SHARE_PROPERTIES_ON_FOLDER_SET_TO_DENEY = BASELINE_ERROR + 11;
var HAS_
function initDesigner_success(result, userContext, methodName) {

    switch(emg_widgetType) {
        case "AlertHeadlineWidget":
            initDesignerSuccessForAlertHeadlineWidget(result, userContext, methodName);
            break;
        case "AutomaticWorkspaceWidget":
            initDesignerSuccessForAutomaticWorkspaceWidget(result, userContext, methodName);
            break;
        case "ManualNewsletterWorkspaceWidget":
            initDesignerSuccessForManualNewsletterWorkspaceWidget(result, userContext, methodName);
            break;
    }
    
}

function setExternalReaderAudience(){
    var targetAudience = $get("audience_3");
    targetAudience.checked = true;
}

function clientSideValidation() {
   // make sure there is a valid name
   if (delegate.name != null && delegate.name.length > 0) {
   }
   return false;
}

/* Start Link based event handlers */
function getUpdatePopup() {
    // Check to see if application is initialized.
    if (!FactivaWidgetRenderManager.getInstance().getInitialized()) {
        showError(_translate("applicationInitializationError"));
        return;
    }
    
    switch(emg_widgetType) {
        case "AlertHeadlineWidget":
            fireUpdateAlertHeadlineWidget();
            break;
        case "AutomaticWorkspaceWidget":
            fireUpdateAutomaticWorkspaceHeadlineWidget();
            break;
        case "ManualNewsletterWorkspaceWidget":
            fireUpdateManualNewsletterWorkspaceHeadlineWidget();
            break;
    }
    return;     
}

function getCreatePopup() {
    // Check to see if application is initialized.
    if (!FactivaWidgetRenderManager.getInstance().getInitialized()) {
        showError(_translate("applicationInitializationError"));
        return;
    }  

    switch(emg_widgetType) {
        case "AlertHeadlineWidget":
            console.log("getCreatePopup>>AlertHeadlineWidget");
            fireCreateAlertHeadlineWidget();
            break;
        case "AutomaticWorkspaceWidget":
            console.log("getCreatePopup>>AutomaticWorkspaceWidget");
            fireCreateAutomaticWorkspaceHeadlineWidget();
            break;
        case "ManualNewsletterWorkspaceWidget":
            console.log("getCreatePopup>>ManualNewsletterWorkspaceWidget");
            fireCreateManualNewsletterWorkspaceHeadlineWidget();
            break;
    }       
   
    return;       
}

function getProxyCredentialsPopup(errorMessage) {
    if (!FactivaWidgetRenderManager.getInstance().getInitialized()) {
        showError(_translate("applicationInitializationError"));
        return;
    }
    
    var targetAudience = $get("audience_2");
    if (targetAudience) {
        targetAudience.checked = true;
    }
    
    if (errorMessage && errorMessage.length > 0) {
        CommonToolkitScripts.setVisible($get("proxyCredErrorMsg"), true);
        $get("proxyCredErrorMsg").innerHTML = errorMessage;
    }
    else {
        CommonToolkitScripts.setVisible($get("proxyCredErrorMsg"), false);
    }
    
    var wIdInput = $get("wid");
    var _wId = trim(cStr(wIdInput.value));
     
    if (_wId.length > 0) {
        CommonToolkitScripts.setVisible($get("proxyCredLnkClose"), true);
    }
    else {
        CommonToolkitScripts.setVisible($get("proxyCredLnkClose"), false);
    }
    
    var modalPopupBehavior = $find('proxyCredModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show();     
}

function okProxyUpdate() {
    var modalPopupBehavior = $find('proxyConfirmModalPopupBehavior');
    modalPopupBehavior.hide(); 
    
    
    var wIdInput = $get("wid");
    var _wId = trim(cStr(wIdInput.value));
     
    if (_wId.length > 0) {
        getUpdatePopup();
    }
    else 
    {
       getCreatePopup();
    }    
}

function closeProxyUpdate() {
   var modalPopupBehavior = $find('proxyConfirmModalPopupBehavior');
   modalPopupBehavior.hide(); 
}

function okUpdate() {
    var modalPopupBehavior = $find('proxyConfirmModalPopupBehavior');
    modalPopupBehavior.hide(); 
    
    switch(emg_widgetType) {
        case "AlertHeadlineWidget":
            fireUpdateAlertHeadlineWidgetScriptCode();
            break;
        case "AutomaticWorkspaceWidget":
            fireUpdateAutomaticWorkspaceWidgetAndGetScriptCode();
            break;
        case "ManualNewsletterWorkspaceWidget":
            fireUpdateManualNewsletterWorkspaceWidgetAndGetScriptCode();
            break;
    }                
}

function closeUpdate() {

    var modalPopupBehavior = $find('proxyConfirmModalPopupBehavior');
    modalPopupBehavior.hide(); 
    
    // Fill in the widgetId for update
    var wIdHiddenInput = $get("wid");
    if (wIdHiddenInput) {
        $get("mPopHeader").innerHTML = _translate("displayCode");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), true);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);        
        CommonToolkitScripts.setVisible($get("codeCntr"), false);
        CommonToolkitScripts.setVisible($get("divBase"), false); 
        
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.set_Y(100);
        modalPopupBehavior.show(); 
        
        // Collect values and update to server.
        EMG.widgets.services.WidgetDesignerManager.GetWidgetScriptCode(
            wIdHiddenInput.value, 
            accessPointCode,
            interfaceLanguage,
            sa_from,
            getScriptCode_success,
            generic_failure, 
            null);            
        return;
    }    
    
    showError(_translate("noWidgetIdFound"));
}

function closeProxyCredModal() {
    var modalPopupBehavior = $find('proxyCredModalPopupBehavior');
    modalPopupBehavior.hide(); 
}

function openUpdateWidgetModal() {
    
    var modalPopupBehavior = $find('proxyCredModalPopupBehavior');
    modalPopupBehavior.hide(); 
    
       
    // Collect values and update to server.
    var delegate = new EMG.widgets.ui.delegates.input.UpdateAlertHeadlineWidgetDelegate();
    var result = FactivaWidgetRenderManager.getInstance().getData();    
    var wIdInput = $get("wid");
    var _wId = trim(cStr(wIdInput.value));
     
    if (_wId.length > 0) {
        if (!fillAlertHeadlineWidgetDelegate(delegate,result,true)) {
            return false;
        }
    }
    else {
        return false;
    }
    

    var modalPopupBehavior = $find('proxyConfirmModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show();     
    CommonToolkitScripts.setVisible($get("publishConfirmControl"), false);
    CommonToolkitScripts.setVisible($get("proxyConfirmControl"), true);
}

function getCodeGenerationPopup() {
    if (!FactivaWidgetRenderManager.getInstance().getInitialized()) {
        showError(_translate("applicationInitializationError"));
        return;
    }
    
    var modalPopupBehavior = $find('proxyConfirmModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show(); 
    
    CommonToolkitScripts.setVisible($get("publishConfirmControl"), true);
    CommonToolkitScripts.setVisible($get("proxyConfirmControl"), false);
}

// widget preview event handlers
function onWidgetEdit(sender,eventArgs) {
    var wDelegate = eventArgs.get_WidgetDelegate();
    if (wDelegate) {
        var actionInput = $get("at");
        var widgetIdInput = $get("wid");
        if (actionInput && widgetIdInput) { 
            hideWidgetList();  
            actionInput.value = 1;
            widgetIdInput.value = wDelegate.Id; 
            var tForm = document.forms['PageBaseForm'];
            tForm.submit();
        }
    }
    return false;
}

var tempArgs = null;
function onWidgetDelete(sender,eventArgs) {

    var confirmDeletePopupBehavior = $find('modalConfirmDeleteButtonBehavior');
    tempArgs = {"sender":sender,"eventArgs":eventArgs};
    
    confirmDeletePopupBehavior.set_Y(100);
    confirmDeletePopupBehavior.show();
    
    return false;
}

function confirmDeletePopupOK() {
    
    // hide the confirmDeletePopup.
    var confirmDeletePopupBehavior = $find('modalConfirmDeleteButtonBehavior');
    confirmDeletePopupBehavior.hide();
    
    $get("mPopHeader").innerHTML = _translate("delete");
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);                
    CommonToolkitScripts.setVisible($get("codeCntr"), false);
    CommonToolkitScripts.setVisible($get("divBase"), false);
    
    var wDelegate = tempArgs.eventArgs.get_WidgetDelegate();
    
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show();

    if (wDelegate) {
        // Get the preview url from the server.
        EMG.widgets.services.WidgetDesignerManager.DeleteWidget(
            wDelegate.Id,    
            accessPointCode,
            interfaceLanguage,
            sa_from,
            deleteManager_success,
            generic_failure, 
            tempArgs);
    }
    tempArgs = null;
    return false;
}

function deleteManager_success(result, userContext, methodName) {
    // Pull out the original sender
    var sender = userContext.sender;
     // Pull out the event
    var wDelegate = userContext.eventArgs.get_WidgetDelegate();
    
    if (result != null && result.ReturnCode != 0) {
        showFrameworkError(result);
    }
    else if (result != null && result.ReturnCode == 0) {
        sender.removeWidgetById(wDelegate.Id);
        // remove first tab if you remove active widget
        var tContainer = $find("wTabContainer");
        var widgetIdInput  = $get("wid");
        
        // retreive the page size before disposing
        if ($get("widgetListTable") != null && $get("widgetListTable").tBodies[0].rows.length > 0) {
            var copyConfig = $("#widgetListTable").trigger("getConfig");
            size = copyConfig[0].config.size;
            disposeTableSorter();
        }
        
        if ($get("widgetListTable") != null && $get("widgetListTable").tBodies[0].rows.length > 0) {
            renderTableSorter();
        }
        
        if (tContainer && widgetIdInput && (widgetIdInput.value == wDelegate.Id)) {
            tContainer.getFirstTab()._hide();
            widgetIdInput.value = "";
        }
        
        // show nowidget if there are none
        if (sender._widgets == null || sender._widgets.length == 0){
            sender.showNoWidgetsArea();
            hideSearchControls();
            showAutoMovie();
        }
        
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.hide();
    }
    else {
        showFrameworkError();
    }
    
}

function onWidgetPublish(sender, eventArgs) {
    var wDelegate = eventArgs.get_WidgetDelegate();
    if (wDelegate) {
        // Get the preview url from the server.  
        $get("mPopHeader").innerHTML = _translate("displayCode");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), true);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);        
        CommonToolkitScripts.setVisible($get("codeCntr"), false);
        CommonToolkitScripts.setVisible($get("divBase"), false);
       
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.set_Y(100);
        modalPopupBehavior.show(); 
      
        EMG.widgets.services.WidgetDesignerManager.GetWidgetScriptCode(
            wDelegate.Id,  
            accessPointCode,
            interfaceLanguage,
            sa_from,
            getScriptCode_success,
            generic_failure, 
            {"sender":sender,"eventArgs":eventArgs});     
        return;  
    }
    return false;
}

function onWidgetPreview(sender,eventArgs) {
    // Pull out the delegate
    var wDelegate = eventArgs.get_WidgetDelegate();
    // Hide the sort preview function
    CommonToolkitScripts.setVisible($get("sortWidgetsByCntr"), false);
    
    
    $get("mPopHeader").innerHTML = _translate("preview");
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), true);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);                
    CommonToolkitScripts.setVisible($get("codeCntr"), false);
    CommonToolkitScripts.setVisible($get("divBase"), false);
    
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show();
    
    switch(wDelegate.Type) {
        case 0:
            // Get the preview url from the server.
            EMG.widgets.services.WidgetDesignerManager.GetAlertHeadlineWidget(
                wDelegate.Id,  
                accessPointCode,
                interfaceLanguage,
                sa_from,
                getPreview_success,
                generic_failure, 
                {"sender":sender,"eventArgs":eventArgs});   
                break;
        case 1:
            // Get the preview url from the server.
            EMG.widgets.services.WidgetDesignerManager.GetAutomaticWorkspaceWidget(
                wDelegate.Id,  
                accessPointCode,
                interfaceLanguage,
                sa_from,
                getPreview_success,
                generic_failure, 
                {"sender":sender,"eventArgs":eventArgs});   
                break;
        case 2:
            // Get the preview url from the server.
            EMG.widgets.services.WidgetDesignerManager.GetManualNewsletterWorkspaceWidget(
                wDelegate.Id,  
                accessPointCode,
                interfaceLanguage,
                sa_from,
                getPreview_success,
                generic_failure, 
                {"sender":sender,"eventArgs":eventArgs});   
                break;
            break;
    }    
    return false;       
}

function onPreviewBack(sender,eventargs) {
    CommonToolkitScripts.setVisible($get("sortWidgetsByCntr"), true);
    sender.showContentArea();    
}

function closeMovieModal() {
    $find('programmaticMovieModalPopupBehavior').hide();
}

function closeErrorModal() {
    document.body.style.height = "auto";
    $find('programmaticErrorModalPopupBehavior').hide();
}

function closeModal() {
    $find('programmaticModalPopupBehavior').hide();
}

function closeProxyModal() {
    $find('proxyCredModalPopupBehavior').hide();
}

function addSecurityToTextArea(){
    var txtArea = $get("scriptCode");
    var http = $get("http");
    var https = $get("https");

    if (txtArea && http && https) {
        if (http.checked == true) {
            // check if https and replace with http
            if( (txtArea.value).substring(13, 19) == "https:" ) {
                
                txtArea.value = (txtArea.value).replace("https:", "http:");
                return;
            }
        }
        
        if (https.checked == true) {
            // check if http and replace with https
            if( (txtArea.value).substring(13, 18) == "http:" ) {
                txtArea.value = (txtArea.value).replace("http:", "https:");
                return;
            }
        }
    }
}