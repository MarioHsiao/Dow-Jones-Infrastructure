//Omniture NameSpace
if (typeof (DJOmniture) == 'undefined')
    DJOmniture = {};

DJOmniture.Property = {
    SessionId: "",
    UserId_Ns: "",
    AccountId: "",
    AccessCode: "",
    FullURL: "",
    InterfaceLanguage:"",
    PageName: "",
    SearchType: "",
    FilterType: "",
    FilterValue: "",
    FormatType: "",
    AccessionNumber: "",
    ContentType: "",
    ArticleType: "",
    Headline: "",
    Author: "",
    WordCount: "",
    PublicationDate: "",
    Source: "",
    BaseLanguage: "",
    Type: ""
}

DJOmniture.dateRange = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

DJOmniture.log = function () {
    if (logOmniture) {
        objOmniture.channel = "Factiva Widgets";
        objOmniture.server = "widgets.factiva.com";
        objOmniture.prop6 = DJOmniture.Property.FullURL;
        objOmniture.prop28 = DJOmniture.Property.UserId_Ns;
        objOmniture.prop29 = DJOmniture.Property.AccountId;
        objOmniture.prop30 = DJOmniture.Property.AccessCode;
        objOmniture.prop31 = interfaceLanguage;
        objOmniture.prop25 = DJOmniture.Property.SessionId;
        objOmniture.pageName = DJOmniture.Property.PageName;
//        objOmniture.prop39 = DJOmniture.Property.SearchType;
//        objOmniture.prop34 = DJOmniture.Property.FilterType;
//        objOmniture.prop35 = DJOmniture.Property.FilterValue;
//        objOmniture.prop32 = DJOmniture.Property.FormatType;
//        objOmniture.prop20 = DJOmniture.Property.AccessionNumber;
//        objOmniture.prop19 = DJOmniture.Property.ContentType;
//        objOmniture.prop3 = DJOmniture.Property.ArticleType;
//        objOmniture.prop4 = decodeURIComponent(DJOmniture.Property.Headline).replace(/\+/g, ' ');
//        objOmniture.prop21 = decodeURIComponent(DJOmniture.Property.Author).replace(/\+/g, ' ');
//        objOmniture.prop33 = DJOmniture.Property.WordCount;
//        objOmniture.prop7 = DJOmniture.Property.PublicationDate;
//        objOmniture.prop8 = DJOmniture.Property.Source;
//        objOmniture.prop5 = DJOmniture.Property.BaseLanguage;
//        objOmniture.prop13 = DJOmniture.Property.Type;
        //Capture evars
        var dt = new Date();
        objOmniture.eVar31 = DJOmniture.dateRange[dt.getDay()];
        objOmniture.eVar32 = dt.getHours() + ":00";
//        objOmniture.eVar21 = DJOmniture.Property.AccountId;
//        objOmniture.eVar3 = DJOmniture.Property.UserId_Ns;
//        objOmniture.eVar4 = DJOmniture.Property.PageName;
        objOmniture.eVar11 = objOmniture.channel;
        var s_code = objOmniture.t();
    }
}

DJOmniture.logHeadlineResultsODE = function () {
    try {
        DJOmniture.clearProperties();
        DJOmniture.Property.PageName = "DJ_FF_PostProcessing";
        DJOmniture.Property.FormatType = "Email";
        DJOmniture.log();
    }
    catch (e) { }
}

DJOmniture.logHeadlineResultsCreateAlert = function () {
    DJOmniture.logPageName("DJ_FF_CreateAlert");
}

//For Alert Invite
DJOmniture.logAlertsInvite = function () {
    try {
        DJOmniture.clearProperties();
        DJOmniture.Property.PageName = "DJ_FF_Invite";
        DJOmniture.Property.Type = "Alert";
        DJOmniture.log();
    }
    catch (e) { }
}

DJOmniture.logRelatedInfo = function (type) {
    try {
        //The requirement is to only do this for company overlay.
        switch (type) {
            case "I":
                break;
            case "R":
                break;
            case "N":
                break;
            case "C":
                DJOmniture.logPageName("DJ_FF_CompanyOverlay");
                break;
        }
    }
    catch (e) { }
}

DJOmniture.logArticle = function (articleMetricsObject) {
    try{
        DJOmniture.clearProperties();
        DJOmniture.Property.PageName = "DJ_FF_ArticleView";
        DJOmniture.Property.FormatType = "Inline";
        if (articleMetricsObject) {
            DJOmniture.Property.AccessionNumber = articleMetricsObject.An;
            DJOmniture.Property.ContentType = articleMetricsObject.Ct;
            DJOmniture.Property.ArticleType = articleMetricsObject.At;
            DJOmniture.Property.Headline = articleMetricsObject.Hdl;
            DJOmniture.Property.Author = articleMetricsObject.Au;
            DJOmniture.Property.WordCount = articleMetricsObject.Wc;
            DJOmniture.Property.PublicationDate = articleMetricsObject.Pd;
            DJOmniture.Property.Source = articleMetricsObject.Sc;
            DJOmniture.Property.BaseLanguage = articleMetricsObject.Bl;
        }
        DJOmniture.log();
    }
    catch (e) { }
}

DJOmniture.logMultiArticleView = function () {
    try{
        DJOmniture.clearProperties();
        DJOmniture.Property.PageName = "DJ_FF_ArticleView";
        DJOmniture.Property.FormatType = "Inline";
        var propertyValue = "Multiple";
        DJOmniture.Property.AccessionNumber = propertyValue;
        DJOmniture.Property.ContentType = propertyValue;
        DJOmniture.Property.ArticleType = propertyValue;
        DJOmniture.Property.Headline = propertyValue;
        DJOmniture.Property.Author = propertyValue;
        DJOmniture.Property.WordCount = propertyValue;
        DJOmniture.Property.PublicationDate = propertyValue;
        DJOmniture.Property.Source = propertyValue;
        DJOmniture.Property.BaseLanguage = propertyValue;
        DJOmniture.log();
    }
    catch (e) { }
}

DJOmniture.logPageName = function (pageName) {
    try {
        DJOmniture.clearProperties();
        DJOmniture.Property.PageName = pageName;
        DJOmniture.log();
    }
    catch (e) { }
}

DJOmniture.clearProperties = function () {
    DJOmniture.Property.PageName = "";
    DJOmniture.Property.SearchType = "";
    DJOmniture.Property.FilterType = "";
    DJOmniture.Property.FilterValue = "";
    DJOmniture.Property.FormatType = "";
    DJOmniture.Property.AccessionNumber = "";
    DJOmniture.Property.ContentType = "";
    DJOmniture.Property.ArticleType = "";
    DJOmniture.Property.Headline = "";
    DJOmniture.Property.Author = "";
    DJOmniture.Property.WordCount = "";
    DJOmniture.Property.PublicationDate = "";
    DJOmniture.Property.Source = "";
    DJOmniture.Property.BaseLanguage = "";
    DJOmniture.Property.Type = "";
}