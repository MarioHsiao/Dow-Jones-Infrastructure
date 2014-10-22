function addDesignerHandlersForAlertHeadlineWidget() 
{
    // On Blur Event Handlers
    $addHandler($get("wName"), "blur", updateAlertHeadlineWidgetPreview);
    $addHandler($get("mainColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    $addHandler($get("mainFontColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    $addHandler($get("accentColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    $addHandler($get("accentFontColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    $addHandler($get("chartBarColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    
    // On Change Event Handlers    
    $addHandler($get("fFmly"), "change", updateAlertHeadlineWidgetPreview);
    $addHandler($get("fSize"), "change", updateAlertHeadlineWidgetPreview);
    $addHandler($get("numHdlnes"), "change", updateAlertHeadlineWidgetPreview);
    $addHandler($get("selTemp"), "change", updateTemplateColorsForAlertPreview);
    
    // On click Event Handlers
    $addHandler($get("hdlnDspl_0"), "click", updateAlertHeadlineWidgetPreview);
    $addHandler($get("hdlnDspl_1"), "click", updateAlertHeadlineWidgetPreview);
    
    // Handlers for updating of Audience Properties
    if ($get("audience_0"))
        $addHandler($get("audience_0"), "click", updateAlertHeadlineWidgetPreview);
    if ($get("audience_1"))
        $addHandler($get("audience_1"), "click", updateAlertHeadlineWidgetPreview);
    if ($get("audience_2"))
        $addHandler($get("audience_2"), "click", updateAlertHeadlineWidgetPreview);
    if ($get("audience_3"))
        $addHandler($get("audience_3"), "click", updateAlertHeadlineWidgetPreview);

    // Add the following handlers
    if ($get("proxyCredUserId")) {
        $addHandler($get("authScheme_0"), "click", updateProxyCredAuthSchemeHandler); 
        $addHandler($get("authScheme_1"), "click", updateProxyCredAuthSchemeHandler); 
        //proxyCredUserIdRow
    }
    
    // Add handlers for HTTP / HTTPS click
    if ($get("http") && $get("https")) {
        $addHandler($get("http"), 'click', addSecurityToTextArea);
        $addHandler($get("https"), 'click', addSecurityToTextArea);
    }
}

function fireCreateAlertHeadlineWidget() {
     // Collect values and update to server.
    var delegate = new EMG.widgets.ui.delegates.input.CreateAlertHeadlineWidgetDelegate();
    var result = FactivaWidgetRenderManager.getInstance().getData();    
    
    if (fillAlertHeadlineWidgetDelegate(delegate,result)) {
        $get("mPopHeader").innerHTML = _translate("saveWidgetDesignNow");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);                
        CommonToolkitScripts.setVisible($get("codeCntr"), false);
        CommonToolkitScripts.setVisible($get("divBase"), false);
        
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.set_Y(100);
        modalPopupBehavior.show(); 
        
        EMG.widgets.services.WidgetDesignerManager.CreateAlertHeadlineWidget(
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

function fireUpdateAlertHeadlineWidget() {
    // Collect values and update to server.
    var delegate = new EMG.widgets.ui.delegates.input.UpdateAlertHeadlineWidgetDelegate();
    var result = FactivaWidgetRenderManager.getInstance().getData();    

    if (fillAlertHeadlineWidgetDelegate(delegate,result,true)) {
        
        $get("mPopHeader").innerHTML = _translate("updateWidgetDesign");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);        
        CommonToolkitScripts.setVisible($get("codeCntr"), false);
        CommonToolkitScripts.setVisible($get("divBase"), false);
        
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.set_Y(100);
        modalPopupBehavior.show(); 
        
        EMG.widgets.services.WidgetDesignerManager.UpdateAlertHeadlineWidget(
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
function fireUpdateAlertHeadlineWidgetScriptCode() {
// Collect values and update to server.
    var delegate = new EMG.widgets.ui.delegates.input.UpdateAlertHeadlineWidgetDelegate();
    var result = FactivaWidgetRenderManager.getInstance().getData();    
        
    if (fillAlertHeadlineWidgetDelegate(delegate,result,true)) {
        // Fill in the widgetId for update
        var wIdHiddenInput = $get("wid");
        if (wIdHiddenInput) {
            $get("mPopHeader").innerHTML = _translate("displayCode");
            CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
            CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);        
            CommonToolkitScripts.setVisible($get("codeCntr"), false);
            CommonToolkitScripts.setVisible($get("divBase"), false);     
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.set_Y(100);
            modalPopupBehavior.show(); 
            
             // Collect values and update to server.
            EMG.widgets.services.WidgetDesignerManager.GetAlertHeadlineWidgetScriptCode(
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
        showError(_translate("noWidgetIdFound"));
    }
}

function fillAlertHeadlineWidgetDelegate(delegate,result,fillWidgetId) {
    if (result.Data != null && result.Data.Count > 0) {
        var ids = [];
        var exKeys = [];
        for (var i=0; i<result.Data.Count;i++){
            var _alert = result.Data.Alerts[i];
            if (_alert.Status.Code == 0) {
                ids[ids.length] = _alert.Id;
                exKeys[exKeys.length] = _alert.ExternalAccessToken;
            }
        }
        
        // Fill in the widgetId for update
        var wIdHiddenInput = $get("wid");
        if (wIdHiddenInput && fillWidgetId) {
            delegate.widgetId = wIdHiddenInput.value;
        }
        
        if (ids.length > 0) {
            delegate.assetIds = ids;
            delegate.externalHashKeys = exKeys;
            delegate.name = cStr($get("wName").value).trim();
            delegate.description = "";
            delegate.maxResultsToReturn = $get("numHdlnes").selectedIndex;
            delegate.mainColor = validateColor($get("mainColor_input").value,"#999999");
            delegate.mainFontColor = validateColor($get("mainFontColor_input").value,"#FFFFFF");
            delegate.accentColor = validateColor($get("accentColor_input").value,"#F0F0F0");
            delegate.accentFontColor = validateColor($get("accentFontColor_input").value,"#000000");
            delegate.chartBarColor = validateColor($get("chartBarColor_input").value,"#DCDADA"); // chart information
            delegate.chartRenderType = 1; // Flash
            delegate.distributionType = getAudience();
            delegate.headlineDisplayType = ($get("hdlnDspl_0").checked) ? 0 : 1;
            delegate.fontFamily = $get("fFmly").selectedIndex;
            delegate.fontSize = $get("fSize").selectedIndex;
            delegate.widgetTemplate = $get("selTemp").selectedIndex; // template information 
            delegate.discoveryTabs = Sys.Serialization.JavaScriptSerializer.deserialize($get("tabStates").value); // tab information 
            
            if ($get("proxyCredUserId")) {
                // AudienceCredentials
                delegate.authenticationScheme = getAuthScheme(); 
                delegate.proxyUserId = cStr($get("proxyCredUserId").value).trim();
                delegate.proxyEmailAddress = cStr($get("proxyCredEmailAddress").value).trim();
                delegate.proxyNamespace = cStr($get("proxyCredNamespace").value).trim();
                delegate.proxyPassword = cStr($get("proxyCredPassword").value).trim();                  
            }
            
            if ($get("profileId")) {
                var selectObj = $get("profileId");
                delegate.profileId = cInt(cStr(selectObj.options[selectObj.selectedIndex].value).trim());  
            }                                  
            
            // Validate Name
            if (delegate.name == null || delegate.name.length == 0) { 
                 showError(_translate("nameIsNotProvided"));
                 return false;
            }            
            
            // Check if all the proxy information has been passed
            if (delegate.distributionType == "2") {
                // Check to see if userid/emailaddress is filled
                switch(delegate.authenticationScheme) {
                    case "0": // userid
                        if (delegate.proxyUserId == null || delegate.proxyUserId.length == 0) {
                            getProxyCredentialsPopup(_translate("editUserID"));
                            return false;
                        }
                        if (delegate.proxyPassword == null || delegate.proxyPassword.length == 0) {
                            getProxyCredentialsPopup(_translate("editPassword"));
                            return false;
                        }
                        if (delegate.proxyNamespace == null || delegate.proxyNamespace.length == 0) {
                            getProxyCredentialsPopup(_translate("editNameSpace"));
                            return false;
                        }
                        break;
                    case "1": // email
                        if (delegate.proxyEmailAddress == null || delegate.proxyEmailAddress.length == 0) {
                            getProxyCredentialsPopup(_translate("editEmailAddress"));
                            return false;
                        }
                        if (delegate.proxyPassword == null || delegate.proxyPassword.length == 0) {
                            getProxyCredentialsPopup(_translate("editPassword"));
                            return false;
                        }
                        break;
                }                
            }
            return true; 
            
         }
         else {
            showError(_translate("noAlertsFound"));  
            return false;
         }
    }    
}


function removeDesignerHandlersForAlertHeadlineWidget() {
    // On Blur Event Handlers
    $removeHandler($get("wName"), "blur", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("mainColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("mainFontColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("accentColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("accentFontColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("chartBarColor_input"), "blur", updateAlertHeadlineWidgetPreview);
    
    // On Change Event Handlers    
    $removeHandler($get("fFmly"), "change", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("fSize"), "change", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("numHdlnes"), "change", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("selTemp"), "change", updateTemplateColorsForAlertPreview);
    
    // On click Event Handlers
    $removeHandler($get("hdlnDspl_0"), "click", updateAlertHeadlineWidgetPreview);
    $removeHandler($get("hdlnDspl_1"), "click", updateAlertHeadlineWidgetPreview);
    
    // Handlers for updating of Audience Properties
    if ($get("audience_0"))
        $removeHandler($get("audience_0"), "click", updateAlertHeadlineWidgetPreview);
    if ($get("audience_1"))
        $removeHandler($get("audience_1"), "click", updateAlertHeadlineWidgetPreview);
    if ($get("audience_2"))
        $removeHandler($get("audience_2"), "click", updateAlertHeadlineWidgetPreview);
    if ($get("audience_3"))
        $removeHandler($get("audience_3"), "click", updateAlertHeadlineWidgetPreview);
    
     // Remove the following handlers
    if ($get("proxyCredUserId")) {
        $removeHandler($get("authScheme_0"), "click", updateProxyCredAuthSchemeHandler); 
        $removeHandler($get("authScheme_1"), "click", updateProxyCredAuthSchemeHandler); 
    }
    
    // Remove handlers for HTTP / HTTPS click
    if ($get("http") && $get("https")) {
        $removeHandler($get("http"), 'click', addSecurityToTextArea);
        $removeHandler($get("https"), 'click', addSecurityToTextArea);
    }
}

function initDesignerForAlertHeadlineWidget() {
    var wIdInput = $get("wid");
    var _wId = trim(cStr(wIdInput.value));

    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show(); 

    $get("mPopHeader").innerHTML = _translate("widgetDesigner");
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), true);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);
    CommonToolkitScripts.setVisible($get("codeCntr"), false);
    CommonToolkitScripts.setVisible($get("divBase"), false);
    CommonToolkitScripts.setVisible($get("wdgtAudienceCntrl"), true);
    
    // Update widget
    if (_wId.length > 0) {
        CommonToolkitScripts.setVisible($get("wdgtCreateCntrl"), false);     
        CommonToolkitScripts.setVisible($get("wdgtUpdateCntrl"), true);
        CommonToolkitScripts.setVisible($get("wdgtCodeGenCntrl"), true);
        
        EMG.widgets.services.WidgetDesignerManager.GetAlertHeadlineWidget(
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

    // Create widget
    var alertIDs = new Array();
    var tAssetIds = $get("assetIds"); 
    if (tAssetIds) {
        var ids = trim(cStr(tAssetIds.value));
        // Display Create Controls
        CommonToolkitScripts.setVisible($get("wdgtCreateCntrl"), true);     
        CommonToolkitScripts.setVisible($get("wdgtUpdateCntrl"), false);
        CommonToolkitScripts.setVisible($get("wdgtCodeGenCntrl"), false);
        
        if ( ids.length > 0 ) {
            alertIDs = ids.split(",");
        }
        if (alertIDs != null && alertIDs.length > 0) {
            EMG.widgets.services.WidgetDesignerManager.GetBaselineAlertHeadlineWidget(
                alertIDs,
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

function updateAlertHeadlineWidgetPreview() {

    if (FactivaWidgetRenderManager.getInstance().getData() != null) {
        var result = FactivaWidgetRenderManager.getInstance().getData();

        /* Validate the Color Inputs */
        result.Definition.MainColor = $get("mainColor_input").value = validateColor($get("mainColor_input").value,"#4945899");
        result.Definition.MainFontColor = $get("mainFontColor_input").value = validateColor($get("mainFontColor_input").value,"#F7F7F7");
        result.Definition.AccentColor = $get("accentColor_input").value = validateColor($get("accentColor_input").value,"#B4B4CC");
        result.Definition.AccentFontColor = $get("accentFontColor_input").value = validateColor($get("accentFontColor_input").value,"#171717");
        result.Definition.ChartBarColor = $get("chartBarColor_input").value = validateColor($get("chartBarColor_input").value,"#DCDADA");        

        // re-populate the data for the entire chart to update chart color
        if (result.Data != null && result.Data.Count > 0) {
            for (var i=0; i<result.Data.Count; i++) {
                if (result.Data.Alerts[i].CompaniesChart != null && result.Data.Alerts[i].CompaniesChart.Chart != null) {
                    result.Data.Alerts[i].CompaniesChart.Chart.chartUri = emg_baseChartingUrl+"?chartType=co&chartBarColor=%23"+result.Definition.ChartBarColor.substr(1);
                }
                if (result.Data.Alerts[i].IndustriesChart != null && result.Data.Alerts[i].IndustriesChart.Chart != null) {
                    result.Data.Alerts[i].IndustriesChart.Chart.chartUri = emg_baseChartingUrl+"?chartType=in&chartBarColor=%23"+result.Definition.ChartBarColor.substr(1);
                }
                if (result.Data.Alerts[i].SubjectsChart != null && result.Data.Alerts[i].SubjectsChart.Chart != null) {
                    result.Data.Alerts[i].SubjectsChart.Chart.chartUri = emg_baseChartingUrl+"?chartType=ns&chartBarColor=%23"+result.Definition.ChartBarColor.substr(1);
                }
                if (result.Data.Alerts[i].ExecutivesChart != null && result.Data.Alerts[i].ExecutivesChart.Chart != null) {
                    result.Data.Alerts[i].ExecutivesChart.Chart.chartUri = emg_baseChartingUrl+"?chartType=ex&chartBarColor=%23"+result.Definition.ChartBarColor.substr(1);
                }
                if (result.Data.Alerts[i].RegionsChart != null && result.Data.Alerts[i].RegionsChart.Chart != null) {
                    result.Data.Alerts[i].RegionsChart.Chart.chartUri = emg_baseChartingUrl + "?chartType=re&chartBarColor=%23" + result.Definition.ChartBarColor.substr(1);
                }
            }
        }
        
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
        
        /* update control that handles number of headlines */
        var numHeadlines = $get("numHdlnes");
        if (numHeadlines) {
            result.Definition.NumOfHeadlines = numHeadlines.selectedIndex;
        }
        
        /* update the widget Name */
        var widgetName = $get("wName");
        if (widgetName) {
            result.Definition.Name = trim(cStr(widgetName.value));
        }
        
        /* update the tab order */
        var tabStates = $get("tabStates");
        if (tabStates) {
            result.Definition.DiscoveryTabs = Sys.Serialization.JavaScriptSerializer.deserialize(trim(cStr(tabStates.value)));
        }
        
        /* update the selected template */
        var selTemp = $get("selTemp");
        if (selTemp) {
            result.Definition.WidgetTemplate = selTemp.selectedIndex;
        }
        
        /* update the get alert link */
        result.Definition.DistributionType =  
            $get("audience_0").checked ? 0 : 
                ( $get("audience_1").checked ? 1 :
                    ( $get("audience_2") && $get("audience_2").checked ? 2 : 3) );
        
        // update Display and Distribution
        //result.Definition.DistributionType = ($get("dstrbtnTyp_0") && $get("dstrbtnTyp_0").checked) ? 0 : 1;
        result.Definition.DisplayType = ($get("hdlnDspl_0") && $get("hdlnDspl_0").checked) ? 0 : 1;

        FactivaWidgetRenderManager.getInstance().xBuildAlertWidget("previewContents",result, true);
    }
}

function updateTemplateColorsForAlertPreview() {
    if (FactivaWidgetRenderManager.getInstance().getColorTemplates() != null) {        
        var fontSize = $get("fSize");
        var fontFamily = $get("fFmly");
        var mainColor = $get("mainColor_input");
        var mainFontColor = $get("mainFontColor_input");
        var accentColor = $get("accentColor_input");
        var accentFontColor = $get("accentFontColor_input");
        var chartBarColor = $get("chartBarColor_input");
        
        var fontSizeRow = $get("fontSizeRow");
        var fontFamilyRow = $get("fontFamilyRow");
        var mainColorRow = $get("mainColorRow");
        var mainFontColorRow = $get("mainFontColorRow");
        var accentColorRow = $get("accentColorRow");
        var accentFontColorRow = $get("accentFontColorRow");
        var chartBarColorRow = $get("chartBarColorRow");

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
            chartBarColorRow.style.visibility = "hidden";
                        
            // set the input box values to the template values
            fontSize.selectedIndex = currentTemplate.fontSize;
            fontFamily.selectedIndex = currentTemplate.fontFamily;
            mainColor.value = currentTemplate.mainColor;
            mainFontColor.value = currentTemplate.mainFontColor;
            accentColor.value = currentTemplate.accentColor;
            accentFontColor.value = currentTemplate.accentFontColor;
            chartBarColor.value = currentTemplate.chartBarColor;
            
            // set the color pickers' colors to the template values 
            $find("mainColor_input_behavior").setColor(currentTemplate.mainColor);
            $find("mainFontColor_input_behavior").setColor(currentTemplate.mainFontColor);
            $find("accentColor_input_behavior").setColor(currentTemplate.accentColor);
            $find("accentFontColor_input_behavior").setColor(currentTemplate.accentFontColor);
            $find("chartBarColor_input_behavior").setColor(currentTemplate.chartBarColor);

        } else {
            // if "Edit Design" un-hide all of the design controls
            fontSizeRow.style.visibility = "visible";
            fontFamilyRow.style.visibility = "visible";
            mainColorRow.style.visibility = "visible";
            mainFontColorRow.style.visibility = "visible";
            accentColorRow.style.visibility = "visible";
            accentFontColorRow.style.visibility = "visible";
            chartBarColorRow.style.visibility = "visible";
        }
        updateAlertHeadlineWidgetPreview();
    }
}

function initDesignerSuccessForAlertHeadlineWidget(result, userContext, methodName) {

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
            $find("chartBarColor_input_behavior").setColor(result.Definition.ChartBarColor);             
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
        
        /* update control that handles number of headlines */
        var fontSize = $get("numHdlnes");
        if (fontSize) {
            fontSize.selectedIndex = result.Definition.NumOfHeadlines;
        }
        
        /* update the widget Name */
        var widgetName = $get("wName");
        if (widgetName) {
            widgetName.value = result.Definition.Name;
        }
        
        /* update headline display*/
        var headlines = $get("hdlnDspl_0");
        var headlinesWSnippets = $get("hdlnDspl_1");
        if ( headlines && headlinesWSnippets ) {
            $get("hdlnDspl_" +  result.Definition.DisplayType).checked = true;
        }
        
        /* update template display*/
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
                $get("chartBarColorRow").style.visibility = "hidden";
            }
        }
        
        /* update the discovery tabs */
        var tabStates = $get("tabStates");
        if (tabStates) {
            tabStates.value = Sys.Serialization.JavaScriptSerializer.serialize(result.Definition.DiscoveryTabs);
            
            // set the value of the tab behavior to the value set in the definition
            var tabBehavior = $find("wDesignerBehavior");
            tabBehavior.dispose();
            $("#wSortContainer").empty();
            
            $create(EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior,
                   {"appendInsideControlID":"wSortContainer",
                    "axis":"y",
                    "discoveryTabs":tabStates.value,
                    "id":"wDesignerBehavior"},
                    {"widgetDesignerUpdate":updateAlertHeadlineWidgetPreview}, null, $get("tabStates"));

        }
        
        /* update audience */
        var tForm = getBForm();
        
        var defaultAudience = $get("audience_0");
        var targetAudience = $get("audience_" +  result.Definition.DistributionType);
        if (targetAudience) {
            targetAudience.checked = true;
        } 
        else if (defaultAudience) {
            defaultAudience.checked = true;
        }   
        // Update the proxy credentials
        // Add the following handlers
        if (result.Definition.AuthenticationCredentials != null) {
            if ($get("proxyCredUserId") ) {
                if (result.Definition.AuthenticationCredentials.AuthenticationScheme != null)
                    $get("authScheme_" +  result.Definition.AuthenticationCredentials.AuthenticationScheme).checked = true;
                
                if (result.Definition.AuthenticationCredentials.ProxyUserId != null &&
                    result.Definition.AuthenticationCredentials.ProxyUserId.length > 0)
                    $get("proxyCredUserId").value = result.Definition.AuthenticationCredentials.ProxyUserId;
                
                if (result.Definition.AuthenticationCredentials.ProxyEmailAddress != null &&
                    result.Definition.AuthenticationCredentials.ProxyEmailAddress.length > 0)
                    $get("proxyCredEmailAddress").value = result.Definition.AuthenticationCredentials.ProxyEmailAddress;
                
                if (result.Definition.AuthenticationCredentials.ProxyPassword != null &&
                    result.Definition.AuthenticationCredentials.ProxyPassword.length > 0)
                    $get("proxyCredPassword").value = result.Definition.AuthenticationCredentials.ProxyPassword;
                
                if (result.Definition.AuthenticationCredentials.ProxyNamespace != null &&
                    result.Definition.AuthenticationCredentials.ProxyNamespace.length > 0)
                    $get("proxyCredNamespace").value = result.Definition.AuthenticationCredentials.ProxyNamespace;
                
                updateProxyCredAuthScheme(true);
            }
            
            var profileIdSelectObj = $get("profileId"); 
            if (profileIdSelectObj &&
                result.Definition.AuthenticationCredentials.ProfileId != null &&
                result.Definition.AuthenticationCredentials.ProfileId.length > 0) {
                
                for (var i=0; i<profileIdSelectObj.options.length; i++) {
                    if (profileIdSelectObj.options[i].value == result.Definition.AuthenticationCredentials.ProfileId) {
                        profileIdSelectObj.selectedIndex = i;
                        break;
                    }
                }
            }            

        }

        FactivaWidgetRenderManager.getInstance().xBuildAlertWidget("previewContents",result,true);

        if (result.ReturnCode == 0) {
            FactivaWidgetRenderManager.getInstance().setInitialized(true);
        }
        if (emg_screenAction == "Publish") {okUpdate();}
    }
    else if (result != null && result.ReturnCode != 0){
        showFrameworkError(result);
        FactivaWidgetRenderManager.getInstance().xBuildAlertWidget("previewContents",result,true);
        return;
    }
    else {
        // unable to find definition widget may not exist, send the user over to the manager
        var widgetIdInput  = $get("wid");
        if (widgetIdInput) {
            widgetIdInput.value = "";
            fireMyWidgets();
        }
        return;
    }
}