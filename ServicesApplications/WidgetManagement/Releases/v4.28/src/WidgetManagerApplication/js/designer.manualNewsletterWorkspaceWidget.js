function addDesignerHandlersForManualNewsletterWorkspaceWidget() {
    
    // On Blur Event Handlers
    $addHandler($get("wName"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    $addHandler($get("mainColor_input"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    $addHandler($get("mainFontColor_input"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    $addHandler($get("accentColor_input"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    $addHandler($get("accentFontColor_input"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    
    // On Change Event Handlers    
    $addHandler($get("fFmly"), "change", updateManualNewsletterWorkspaceWidgetPreview);
    $addHandler($get("fSize"), "change", updateManualNewsletterWorkspaceWidgetPreview);
    $addHandler($get("numHdlnes"), "change", updateManualNewsletterWorkspaceWidgetPreview);
    $addHandler($get("selTemp"), "change", updateTemplateColorsForNewsletterPreview);
    
    // On click Event Handlers
    $addHandler($get("hdlnDspl_0"), "click", updateManualNewsletterWorkspaceWidgetPreview);
    $addHandler($get("hdlnDspl_1"), "click", updateManualNewsletterWorkspaceWidgetPreview);

    // Add handlers for HTTP / HTTPS click
    if ($get("http") && $get("https")) {
        $addHandler($get("http"), 'click', addSecurityToTextArea);
        $addHandler($get("https"), 'click', addSecurityToTextArea);
    }
}

function removeDesignerHandlersForManualNewsletterWorkspaceWidget() {
    // On Blur Event Handlers
    $removeHandler($get("wName"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    $removeHandler($get("mainColor_input"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    $removeHandler($get("mainFontColor_input"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    $removeHandler($get("accentColor_input"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    $removeHandler($get("accentFontColor_input"), "blur", updateManualNewsletterWorkspaceWidgetPreview);
    
    // On Change Event Handlers    
    $removeHandler($get("fFmly"), "change", updateManualNewsletterWorkspaceWidgetPreview);
    $removeHandler($get("fSize"), "change", updateManualNewsletterWorkspaceWidgetPreview);
    $removeHandler($get("numHdlnes"), "change", updateManualNewsletterWorkspaceWidgetPreview);
    $removeHandler($get("selTemp"), "change", updateTemplateColorsForNewsletterPreview);
    
    // On click Event Handlers
    $removeHandler($get("hdlnDspl_0"), "click", updateManualNewsletterWorkspaceWidgetPreview);
    $removeHandler($get("hdlnDspl_1"), "click", updateManualNewsletterWorkspaceWidgetPreview);

    // Remove handlers for HTTP / HTTPS click
    if ($get("http") && $get("https")) {
        $removeHandler($get("http"), 'click', addSecurityToTextArea);
        $removeHandler($get("https"), 'click', addSecurityToTextArea);
    }
}

function disableManualNewsletterWorkspaceDesignerControls() {
    $get("wName").disabled = true;    
}

function updateTemplateColorsForNewsletterPreview() {
    if (FactivaWidgetRenderManager.getInstance().getColorTemplates() != null) {        
        var fontSize = $get("fSize");
        var fontFamily = $get("fFmly");
        var mainColor = $get("mainColor_input");
        var mainFontColor = $get("mainFontColor_input");
        var accentColor = $get("accentColor_input");
        var accentFontColor = $get("accentFontColor_input");
        
        var fontSizeRow = $get("fontSizeRow");
        var fontFamilyRow = $get("fontFamilyRow");
        var mainColorRow = $get("mainColorRow");
        var mainFontColorRow = $get("mainFontColorRow");
        var accentColorRow = $get("accentColorRow");
        var accentFontColorRow = $get("accentFontColorRow");

        var colorTemplates = FactivaWidgetRenderManager.getInstance().getColorTemplates();
        var index = $get("selTemp").selectedIndex;
                
        if (index != 0) {
            // if not "Edit Design" disable the design controls and 
            // set the input values to the 
            var currentTemplate = colorTemplates[index];
            
            // hide the design controls
            fontSizeRow.style.visibility = "hidden";
            fontFamilyRow.style.visibility = "hidden";
            mainColorRow.style.visibility = "hidden";
            mainFontColorRow.style.visibility = "hidden";
            accentColorRow.style.visibility = "hidden";
            accentFontColorRow.style.visibility = "hidden";
                        
            // set the input box values to the template values
            fontSize.selectedIndex = currentTemplate.fontSize;
            fontFamily.selectedIndex = currentTemplate.fontFamily;
            mainColor.value = currentTemplate.mainColor;
            mainFontColor.value = currentTemplate.mainFontColor;
            accentColor.value = currentTemplate.accentColor;
            accentFontColor.value = currentTemplate.accentFontColor;
            
            // set the color pickers' colors to the template values 
            $find("mainColor_input_behavior").setColor(currentTemplate.mainColor);
            $find("mainFontColor_input_behavior").setColor(currentTemplate.mainFontColor);
            $find("accentColor_input_behavior").setColor(currentTemplate.accentColor);
            $find("accentFontColor_input_behavior").setColor(currentTemplate.accentFontColor);

        } else {
            // if "Edit Design" un-hide all of the design controls
            fontSizeRow.style.visibility = "visible";
            fontFamilyRow.style.visibility = "visible";
            mainColorRow.style.visibility = "visible";
            mainFontColorRow.style.visibility = "visible";
            accentColorRow.style.visibility = "visible";
            accentFontColorRow.style.visibility = "visible";
        }
        updateManualNewsletterWorkspaceWidgetPreview();
    }
}

function initDesignerForManualNewsletterWorkspaceWidget() {
    var wIdInput = $get("wid");
    var _wId = trim(cStr(wIdInput.value));
    
    disableManualNewsletterWorkspaceDesignerControls();
    
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show(); 
    
    $get("mPopHeader").innerHTML = translate("widgetDesigner");
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), true);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);
    CommonToolkitScripts.setVisible($get("codeCntr"), false);
    CommonToolkitScripts.setVisible($get("divBase"), false);
    CommonToolkitScripts.setVisible($get("wdgtAudienceCntrl"), true);
    
    if (_wId.length > 0) {
        CommonToolkitScripts.setVisible($get("wdgtCreateCntrl"), false);     
        CommonToolkitScripts.setVisible($get("wdgtUpdateCntrl"), true);
        CommonToolkitScripts.setVisible($get("wdgtCodeGenCntrl"), true);
        
      
        EMG.widgets.services.WidgetDesignerManager.GetManualNewsletterWorkspaceWidget(
            _wId,
            accessPointCode,
            interfaceLanguage,
            sa_from,
            initDesigner_success,
            generic_failure, 
            null
        );
        return;        
    }
        
    var assetIds = new Array();
    var tAssetIds = $get("assetIds"); 
    if (tAssetIds) {
        var ids = trim(cStr(tAssetIds.value));
        // Display Create Controls
        CommonToolkitScripts.setVisible($get("wdgtCreateCntrl"), true);     
        CommonToolkitScripts.setVisible($get("wdgtUpdateCntrl"), false);
        CommonToolkitScripts.setVisible($get("wdgtCodeGenCntrl"), false);
        
        if ( ids.length > 0 ) {
            assetIds = ids.split(",");
        }
        if (assetIds != null && assetIds.length > 0) {
            EMG.widgets.services.WidgetDesignerManager.GetBaselineManualNewsletterWorkspaceWidget(
                assetIds,
                accessPointCode,
                interfaceLanguage,
                sa_from,
                initDesigner_success,
                generic_failure, 
                null
            );
            return;
        }         
    }    
}

function updateManualNewsletterWorkspaceWidgetPreview() {
    if (FactivaWidgetRenderManager.getInstance().getData() != null) {
        var result = FactivaWidgetRenderManager.getInstance().getData();
        
        /* Validate the Color Inputs */
        result.Definition.MainColor = $get("mainColor_input").value = validateColor($get("mainColor_input").value,"#4945899");
        result.Definition.MainFontColor = $get("mainFontColor_input").value = validateColor($get("mainFontColor_input").value,"#F7F7F7");
        result.Definition.AccentColor = $get("accentColor_input").value = validateColor($get("accentColor_input").value,"#B4B4CC");
        result.Definition.AccentFontColor = $get("accentFontColor_input").value = validateColor($get("accentFontColor_input").value,"#171717");
    
         /* Validate FontFamily Information */
        var fontFamily = $get("fFmly");
        if (fontFamily) {
            result.Definition.FontFamily = fontFamily.selectedIndex;
        }
        /* Validate FontSize Information */
        var fontSize = $get("fSize");
        if (fontSize) {
            result.Definition.FontSize = fontSize.selectedIndex;
        }
        
        /* update control that sets headlines / headlines with snippets */
        var headlinesOnly = $get("hdlnDspl_0");
        if (headlinesOnly) {       
            result.Definition.DisplayType = (headlinesOnly && headlinesOnly.checked) ? 0 : 1;
        }
        
        /* update control that handles number of headlines */
        var fontSize = $get("numHdlnes");
        if (fontSize) {
            result.Definition.NumItemsPerSection = fontSize.selectedIndex + 1;
        }
        
        /* update the widget Name */
        var widgetName = $get("wName");
        if (widgetName) {
            result.Definition.Name = trim(cStr(widgetName.value));
        }
        
        /* update the selected template */
        var selTemp = $get("selTemp");
        if (selTemp) {
            result.Definition.WidgetTemplate = selTemp.selectedIndex;
        }
          
        FactivaWidgetRenderManager.getInstance().xBuildManualNewsletterWorkspaceWidget("previewContents",result, true);
    }
}

function initDesignerSuccessForManualNewsletterWorkspaceWidget(result, userContext, methodName) {

    console.log("initDesignerSuccessForManualNewsletterWorkspaceWidget");
    console.log("result:" + result);    
    FactivaWidgetRenderManager.getInstance().setData(result);  
      
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.hide();
    
    if (result != null && 
        (result.ReturnCode == 0 || result.ReturnCode == NO_VALID_FOLDERS) && 
        result.Definition != null) {
        
        /* Update the background of the colorpicker */  
        if ($find("mainColor_input_behavior")) {       
            $find("mainColor_input_behavior").setColor(result.Definition.MainColor);             
            $find("mainFontColor_input_behavior").setColor(result.Definition.MainFontColor);             
            $find("accentColor_input_behavior").setColor(result.Definition.AccentColor);             
            $find("accentFontColor_input_behavior").setColor(result.Definition.AccentFontColor);             
        }
        
        /* update control that handles fontFamily */
        var fontFamily = $get("fFmly");
        if (fontFamily) {
            fontFamily.selectedIndex = result.Definition.FontFamily;
        }
        
        /* update control that handles font Size */
        var fontSize = $get("fSize");
        if (fontSize) {
            fontSize.selectedIndex = result.Definition.FontSize;
        }
        
        /* update headline display */
        var headlines = $get("hdlnDspl_0");
        var headlinesWSnippets = $get("hdlnDspl_1");
        if ( headlines && headlinesWSnippets ) {
            $get("hdlnDspl_" +  result.Definition.DisplayType).checked = true;
        }
        
        /* update control that handles number of headlines */
        var fontSize = $get("numHdlnes");
        if (fontSize) {
            fontSize.selectedIndex = result.Definition.NumItemsPerSection-1;
        }
        
        /* update the widget Name */
        var widgetName = $get("wName");
        if (widgetName) {
            widgetName.value = result.Definition.Name;
        }
        
        /* update the selected template */
        var selTemp = $get("selTemp");
        if (selTemp) {
            selTemp.selectedIndex = result.Definition.WidgetTemplate;
            
            if (selTemp.selectedIndex != 0) {
                // if not "Edit Design" hide the design controls
                $get("fontSizeRow").style.visibility = "hidden";
                $get("fontFamilyRow").style.visibility = "hidden";
                $get("mainColorRow").style.visibility = "hidden";
                $get("mainFontColorRow").style.visibility = "hidden";
                $get("accentColorRow").style.visibility = "hidden";
                $get("accentFontColorRow").style.visibility = "hidden";
            }
        }
                
        FactivaWidgetRenderManager.getInstance().xBuildManualNewsletterWorkspaceWidget("previewContents",result,true);
        if (result.ReturnCode == 0) {FactivaWidgetRenderManager.getInstance().setInitialized(true);}
        if (emg_screenAction == "Publish") {okUpdate();}
    }
    else if (result != null && result.ReturnCode != 0){
        showFrameworkError(result);
        FactivaWidgetRenderManager.getInstance().xBuildManualNewsletterWorkspaceWidget("previewContents",result,true);
        return;
    }
    else {
        // unable to find definition widget may not exit, send the user over to the manager
        var widgetIdInput  = $get("wid");
        if (widgetIdInput) {
            widgetIdInput.value = "";
            fireMyWidgets();
            return;
        }
    }
}

function fillManualNewsletterWorkspaceWidgetDelegate(delegate,result,fillWidgetId) {
    if (result.Data != null && result.Data.ManualNewsletterWorkspaceInfo != null) {
                
        // Fill in the widgetId for update
        var wIdHiddenInput = $get("wid");
        if (wIdHiddenInput && fillWidgetId) {
            delegate.widgetId = wIdHiddenInput.value;
        }
        var temp = new Array();
        temp[temp.length] = result.Data.ManualNewsletterWorkspaceInfo.Id;
        delegate.assetIds = temp;
        delegate.name = "";
        delegate.description = "";
        delegate.maxResultsToReturn = $get("numHdlnes").selectedIndex + 1;
        delegate.mainColor = validateColor($get("mainColor_input").value,"#999999");
        delegate.mainFontColor = validateColor($get("mainFontColor_input").value,"#FFFFFF");
        delegate.accentColor = validateColor($get("accentColor_input").value,"#F0F0F0");
        delegate.accentFontColor = validateColor($get("accentFontColor_input").value,"#000000");  
        delegate.headlineDisplayType = ($get("hdlnDspl_0").checked) ? 0 : 1;
        delegate.fontFamily = $get("fFmly").selectedIndex;
        delegate.fontSize = $get("fSize").selectedIndex;
        delegate.widgetTemplate = $get("selTemp").selectedIndex;
        
        return true;
    }  
    return false;  
}
    
function fireUpdateManualNewsletterWorkspaceWidgetAndGetScriptCode (){
    var delegate = new EMG.widgets.ui.delegates.input.UpdateManualNewsletterWorkspaceWidgetDelegate();
    var result = FactivaWidgetRenderManager.getInstance().getData();    
        
    if (fillManualNewsletterWorkspaceWidgetDelegate(delegate,result,true)) {
        // Fill in the widgetId for update
        var wIdHiddenInput = $get("wid");
        if (wIdHiddenInput) {
            $get("mPopHeader").innerHTML = translate("displayCode");
            CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
            CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);        
            CommonToolkitScripts.setVisible($get("codeCntr"), false);
            CommonToolkitScripts.setVisible($get("divBase"), false);     
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.set_Y(100);
            modalPopupBehavior.show(); 
            
             // Collect values and update to server.
            EMG.widgets.services.WidgetDesignerManager.GetManualNewsletterWorkspaceWidgetScriptCode(
                wIdHiddenInput.value, 
                delegate,   
                accessPointCode,
                interfaceLanguage,
                sa_from,
                getScriptCode_success,
                generic_failure, 
                null);  
            return;
        }
        showError(translate("noWidgetIdFound"));
    }
    
}

function fireCreateManualNewsletterWorkspaceHeadlineWidget() {
    console.log("fireCreateManualNewsletterWorkspaceHeadlineWidget");
     // Collect values and update to server.
    var delegate = new EMG.widgets.ui.delegates.input.CreateManaualNewsletterWorkspaceWidgetDelegate();
    var result = FactivaWidgetRenderManager.getInstance().getData();    
    console.log("result:"+result);
    
    if (fillManualNewsletterWorkspaceWidgetDelegate(delegate,result)) {
        $get("mPopHeader").innerHTML = translate("saveWidgetDesignNow");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);                
        CommonToolkitScripts.setVisible($get("codeCntr"), false);
        CommonToolkitScripts.setVisible($get("divBase"), false);
        
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.set_Y(100);
        modalPopupBehavior.show(); 
        
        EMG.widgets.services.WidgetDesignerManager.CreateManualNewsletterWorkspaceWidget(
            delegate,    
            accessPointCode,
            interfaceLanguage,
            sa_from,
            createWidget_success,
            generic_failure, 
            null);            
    }
    return;
}

function fireUpdateManualNewsletterWorkspaceHeadlineWidget() {
    console.log("fireUpdateManualNewsletterWorkspaceHeadlineWidget");
     // Collect values and update to server.
    var delegate = new EMG.widgets.ui.delegates.input.UpdateManualNewsletterWorkspaceWidgetDelegate();
    var result = FactivaWidgetRenderManager.getInstance().getData();    
    console.log(result);
    
    if (fillManualNewsletterWorkspaceWidgetDelegate(delegate,result,true)) {
        $get("mPopHeader").innerHTML = translate("saveWidgetDesignNow");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);                
        CommonToolkitScripts.setVisible($get("codeCntr"), false);
        CommonToolkitScripts.setVisible($get("divBase"), false);
        
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.set_Y(100);
        modalPopupBehavior.show(); 
        
        EMG.widgets.services.WidgetDesignerManager.UpdateManualNewsletterWorkspaceWidget(
            delegate,    
            accessPointCode,
            interfaceLanguage,
            sa_from,
            updateWidget_success,
            generic_failure, 
            null);            
    }
    return;
}