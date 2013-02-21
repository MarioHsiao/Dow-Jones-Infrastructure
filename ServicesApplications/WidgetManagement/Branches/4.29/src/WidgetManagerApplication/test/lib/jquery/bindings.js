/* Predefined Function page onload handler */ 
function pageLoad(){
    fireGetHeadlines("leftTarget", "SEC complaint");
    fireGetSummaryHeadlines("mainTopTarget");    
    fireGetRSS("mainBottomTarget", "http://feeds.nytimes.com/nyt/rss/internet");
    fireGetRSS("rightTarget", "http://search.twitter.com/search.atom?q=iraq");
    
    // Atom    "http://search.twitter.com/search.atom?q=kosmix"
}

/* Predefined Function page onunload handler */ 
function pageUnload() {
    
}

var fireGetHeadlinesCount = 0;
function fireGetHeadlines(id, st) {
     
    // Collect values and update to server.
    var searchText = st + " rst:fsp2yespeoc";
   
    if (searchText !== 'undefined' && searchText != null && searchText.length > 0 ) {
            
        $get("mPopHeader").innerHTML = translate("getHeadlines");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);                
        CommonToolkitScripts.setVisible($get("codeCntr"), false);
        CommonToolkitScripts.setVisible($get("divBase"), false);
        
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.set_Y(100);
        modalPopupBehavior.show(); 
        
        emg.widgets.ui.demo.services.HeadlineDelivery.GetHeadlines(
            searchText,    
            accessPointCode,
            interfaceLanguage,
            sa_from,
            getHeadlines_success,
            generic_failure, 
            id);  
        fireGetHeadlinesCount++;          
    }
    return;
}


function fireGetSummaryHeadlines(id) {
            
    $get("mPopHeader").innerHTML = translate("getHeadlines");
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);                
    CommonToolkitScripts.setVisible($get("codeCntr"), false);
    CommonToolkitScripts.setVisible($get("divBase"), false);
    
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show(); 
    
    emg.widgets.ui.demo.services.HeadlineDelivery.GetSummaryHeadlines(
        accessPointCode,
        interfaceLanguage,
        sa_from,
        getHeadlines_success,
        generic_failure, 
        id);  
    fireGetHeadlinesCount++;          
    
    return;
}

function fireGetRSS(id,uri){
    if (uri !== 'undefined' && uri !== null && uri.length > 0 ) {
            
        $get("mPopHeader").innerHTML = translate("getHeadlines");
        CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
        CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), true);                
        CommonToolkitScripts.setVisible($get("codeCntr"), false);
        CommonToolkitScripts.setVisible($get("divBase"), false);
        
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.set_Y(100);
        modalPopupBehavior.show(); 
        
        emg.widgets.ui.demo.services.HeadlineDelivery.GetRssFeed(
            uri,    
            accessPointCode,
            interfaceLanguage,
            sa_from,
            getHeadlines_success,
            generic_failure, 
            id);  
        fireGetHeadlinesCount++;          
    }
    return;
}

function getHeadlines_success(result, userContext, methodName) {
    if (result != null && result.ReturnCode != 0) {
        fireGetHeadlinesCount--;
        showFrameworkError(result);
    }
    else if (result != null && result.ReturnCode == 0) {
        if (fireGetHeadlinesCount > 0)
            fireGetHeadlinesCount--;
        switch(userContext){
            case "rightTarget":
                 $("#" + userContext).dj_emg_headlineList({
                    baseClassName : "right", 
                    displaySnippets : "none", 
                    displayAccessionNumbers : true, 
                    droppableId : "dropTargetUL",
                    displayEntities : false
                },result.Result,{});
                break;
            case "leftTarget":
                 $("#" + userContext).dj_emg_headlineList({
                    baseClassName : "left", 
                    displaySnippets : "inline", 
                    droppableId : "dropTargetUL",
                    displayAccessionNumbers : true
                },result.Result,{});              
                break;
            case "mainTopTarget": 
            case "mainBottomTarget":
                $("#" + userContext).dj_emg_headlineList({
                    maxNumHeadlinesToShow: 10, 
                    baseClassName : "normal", 
                    displaySnippets : "hover", 
                    droppableId : "dropTargetUL",
                    displayAccessionNumbers: false, 
                    displayDeleteTrigger : true,
                    headlineClickCallback : headlineClick_callback, 
                    headlineEntityClickCallback : headlineEntityClick_callback
                },result.Result,{});
                break;
        }
               
        if (fireGetHeadlinesCount == 0)
        {
            $("#leftTarget").bind("dj.emg.headlineList.headlineClick", headlineClick_callback);
            $("#leftTarget").bind("dj.emg.headlineList.entityClick", headlineEntityClick_callback);
            $("#leftTarget").bind("dj.emg.headlineList.headlineDrop", headlineDrop_callback);    
            
            $("#mainTopTarget").bind("dj.emg.headlineList.headlineClick", headlineClick_callback);    
            $("#mainTopTarget").bind("dj.emg.headlineList.headlineDrop", headlineDrop_callback);  
            
            $("#mainBottomTarget").bind("dj.emg.headlineList.headlineClick", headlineClick_callback);       
            $("#mainBottomTarget").bind("dj.emg.headlineList.headlineDrop", headlineDrop_callback);   
            
            $("#rightTarget").bind("dj.emg.headlineList.headlineClick", headlineClick_callback);            
            $("#rightTarget").bind("dj.emg.headlineList.headlineDrop", headlineDrop_callback);
            
            $.dj_emg_headlineList("#leftTarget").setMaxNumHeadlinesToShow(5);
            $.dj_emg_headlineList("#leftTarget").setMaxNumHeadlinesToShow(-1);
            $.dj_emg_headlineList("#leftTarget").getContainer().bind("dj.emg.headlineList.moreLikeThisClick" ,moreLikeThisClick_callback);
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }
        return;
    }
    else {
        if (fireGetHeadlinesCount > 0) 
            fireGetHeadlinesCount--;
        showFrameworkError();
    }
    return;
}

var added = 0;
// ie fix
function headlineDrop_callback(eventObject, data) {
    added++;    
    if (data.headline.contentCategoryDescriptor == "external") {
        $("#" + data.headlineList.getDroppableId()).append("<li><div><b>Uri: </b>" + data.headline.reference.externalUri + "</div></li>");
    }
    else if (data.headline.reference) {
        $("#" + data.headlineList.getDroppableId()).append("<li><div><b>AccessionNo: </b>" + data.headline.reference.guid + "</div></li>");
    }   
}

function headlineClick_callback(eventObject, data) {
    $get("mPopHeader").innerHTML = "Headline";
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);                
    CommonToolkitScripts.setVisible($get("codeCntr"), true);    
    CommonToolkitScripts.setVisible($get("divBase"), true);
    $("#codeCntr").empty();
    $("#codeCntr").css( { padding : "10px 5px", fontSize:"77%" } );
    if (data.headline.contentCategoryDescriptor == "external") {
        $("#codeCntr").append("<div><b>Uri: </b>" + data.headline.reference.externalUri + "</div>");
    }
    else if (data.headline.reference) {
        $("#codeCntr").append("<div><b>AccessionNo: </b>" + data.headline.reference.guid + "</div>");
    }
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show(); 
}

function headlineEntityClick_callback(eventObject, data){
    $get("mPopHeader").innerHTML = "Entity";
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);                
    CommonToolkitScripts.setVisible($get("codeCntr"), true);    
    CommonToolkitScripts.setVisible($get("divBase"), true);
    $("#codeCntr").empty();
    $("#codeCntr").css( { padding : "10px 5px", fontSize:"77%" } );
    
    if (data.entity.guid) {        
        $("#codeCntr").append("<div><b>AccessionNo: </b>" + data.headline.reference.guid + "</div>");
        $("#codeCntr").append("<div><b>Entity Code: </b>" + data.entity.guid + "</div>");
    }
    
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show();
}

function moreLikeThisClick_callback(eventObject, data){
    $get("mPopHeader").innerHTML = "MoreLikeThis";
    CommonToolkitScripts.setVisible($get("codeLoadingCntr"), false);
    CommonToolkitScripts.setVisible($get("codeUpdatingCntr"), false);                
    CommonToolkitScripts.setVisible($get("codeCntr"), true);    
    CommonToolkitScripts.setVisible($get("divBase"), true);
    $("#codeCntr").empty();
    $("#codeCntr").css( { padding : "10px 5px", fontSize:"77%" } );
    
    if (data.headline) {        
        $("#codeCntr").append("<div><b>AccessionNo: </b>" + data.headline.reference.guid + "</div>");
        $("#codeCntr").append("<div><b>DocumentVector: </b>" + data.headline.documentVector + "</div>");
    }
    
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.set_Y(100);
    modalPopupBehavior.show();
}

/* Failure requests */
function generic_failure(error, userContext, methodName){
    showFrameworkError();
}

function showFrameworkError(result){
    if (fireGetHeadlinesCount > 0) fireGetHeadlinesCount--;
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
        var returnUrlValue = CStr(returnUrlInput.value).trim();
        if (returnUrlValue != null && returnUrlValue.length > 0 ) {
            window.location.href = returnUrlValue;
        }
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
     errorLabel.innerHTML = translate("error"); 
     errorMessageDiv.innerHTML = message;
     modalErrorPopupBehavior.set_Y(100);
     modalErrorPopupBehavior.show(); 
}