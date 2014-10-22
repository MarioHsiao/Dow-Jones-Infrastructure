namespace EMG.widgets.ui.dto
{
    /// <summary>
    /// Widget Type
    /// </summary>
    public enum WidgetType
    {
        /// <summary>
        /// Alert Headline Widget
        /// </summary>
        AlertHeadlineWidget = 0,
        /// <summary>
        /// Automatic WorkspaceWidget
        /// </summary>
        AutomaticWorkspaceWidget,
        /// <summary>
        /// Manaual Workspace Widget
        /// </summary>
        ManualNewsletterWorkspaceWidget,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum IntegrationTarget
    {
        /// <summary>
        /// Intergration Target has not been specified, Defaults to plain HTML page
        /// </summary>
        UnSpecified = 0,
        /// <summary>
        /// Google Personal Page "IGoogle"
        /// </summary>
        IGoogle,
        /// <summary>
        /// PageFlakes Personal Page
        /// </summary>
        PageFlakes,
        /// <summary>
        /// Netvibes Personal Page
        /// </summary>
        Netvibes,
        /// <summary>
        /// Blogger Googles Free Blogging Service
        /// </summary>
        Blogger,
        /// <summary>
        /// Live.com Microsoft portal
        /// </summary>
        LiveDotCom,
        /// <summary>
        /// Spaces.live.com integration point
        /// </summary>
        LiveSpaces,
        /// <summary>
        /// Javascript Code
        /// </summary>
        JavaScriptCode,
        /// <summary>
        /// SharePoint Webpart Integration point
        /// </summary>
        SharePointWebPart,
    }
    /// <summary>
    /// Data Response Type
    /// </summary>
    public enum ResponseFormat
    {
        /// <summary>
        /// JSON - JavaScript Object Notation 
        /// </summary>
        JSON = 0,
        /// <summary>
        /// JSONP - JavaScript Object Notation with Padding
        /// </summary>
        JSONP,
        /// <summary>
        /// XML - Plain Old XML 
        /// </summary>
        XML,
        /// <summary>
        /// ATOM - Atom Syndication Format  
        /// </summary>
        ATOM,
        /// <summary>
        /// RSS - Really Simple Syndication (RSS 2.0) 
        /// </summary>
        RSS,
        /// <summary>
        /// RSS - Really Simple Syndication (RSS 2.0)
        /// Also includes iTunes Specification and Yahoo Media RSS. 
        /// </summary>
        PODCAST_RSS,
    }

    /// <summary>
    /// Management Actions associated with Widget
    /// </summary>
    public enum WidgetManagementAction
    {
        /// <summary>
        /// 
        /// </summary>
        Create = 0,
        /// <summary>
        /// 
        /// </summary>
        Update = 1,
        /// <summary>
        /// 
        /// </summary>
        List = 2,

        /// <summary>
        /// 
        /// </summary>
        Gallery = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WidgetManagementSecondaryAction
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Preview,
        /// <summary>
        /// 
        /// </summary>
        Publish,
    }

    /// <summary>
    /// Product that made call to Widget Designer.
    /// </summary>
    public enum WidgetRefererProduct
    {
        /// <summary>
        /// Factiva Reader
        /// </summary>
        FactivaReader = 0,
        /// <summary>
        /// Baseline IWorks product.
        /// </summary>
        IWorksBasic = 1,
        /// <summary>
        /// IWorks Product
        /// </summary>
        IWorksPlus = 2,
        /// <summary>
        /// Search 2.0 Core Product Integrated into Factiva Corporate Reasearcher Product
        /// </summary>
        IWorksPremium = 3,
        /// <summary>
        /// Factiva Corporate Reaseacher Product
        /// </summary>
        FactivaDotCom = 4,
        /// <summary>
        /// Factiva Companies and Executives Product a.k.a. SalesWorks
        /// </summary>
        FactivaCompaniesAndExecutives = 5,
        /// <summary>
        /// Insight Media Monitoring product
        /// </summary>
        Insight = 6,
        /// <summary>
        /// UnSpecified
        /// </summary>
        UnSpecified = 7,

    }

    /// <summary>
    /// 
    /// </summary>
    public enum WidgetSortBy
    {
        /// <summary>
        /// 
        /// </summary>
        Date,
        /// <summary>
        /// 
        /// </summary>
        Name,
    }
}