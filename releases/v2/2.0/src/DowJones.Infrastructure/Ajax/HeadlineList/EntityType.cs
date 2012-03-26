
using DowJones.Attributes;

namespace DowJones.Ajax
{
    /// <summary>
    /// 
    /// </summary>
    public enum ContentCategory
    {
        /// <summary>
        /// 
        /// </summary>
        UnSpecified,
        /// <summary>
        /// 
        /// </summary>
        External,
        /// <summary>
        /// 
        /// </summary>
        Publication,
        /// <summary>
        /// 
        /// </summary>
        Website,
        /// <summary>
        /// 
        /// </summary>
        Picture,
        /// <summary>
        /// 
        /// </summary>
        Multimedia,
        /// <summary>
        /// 
        /// </summary>
        Blog,
        /// <summary>
        /// 
        /// </summary>
        Board,
        /// <summary>
        /// 
        /// </summary>
        Internal,
        /// <summary>
        /// 
        /// </summary>
        Summary,
        /// <summary>
        /// 
        /// </summary>
        CustomerDoc,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ContentSubCategory
    {
        /// <summary>
        ///
        /// </summary>
        UnSpecified,
        /// <summary>
        ///
        /// </summary>
        Analyst,
        /// <summary>
        ///
        /// </summary>
        Blog,
        /// <summary>
        ///
        /// </summary>
        Newspaper,
        /// <summary>
        ///
        /// </summary>
        Audio,
        /// <summary>
        ///
        /// </summary>
        Video,
        /// <summary>
        ///
        /// </summary>
        PDF,

        /// <summary>
        ///
        /// </summary>
        HTML,

        /// <summary>
        ///
        /// </summary>
        Graphic,

        /// <summary>
        ///
        /// </summary>
        Article,

        /// <summary>
        ///
        /// </summary>
        Rss,

        /// <summary>
        ///
        /// </summary>
        Atom,

        /// <summary>
        ///
        /// </summary>
        Multimedia,

        /// <summary>
        /// 
        /// </summary>
        Board,
        /// <summary>
        /// 
        /// </summary>
        Internal,
        /// <summary>
        /// 
        /// </summary>
        Summary,
        /// <summary>
        /// 
        /// </summary>
        CustomerDoc,
        /// <summary>
        /// 
        /// </summary>
        File,

        /// <summary>
        /// 
        /// </summary>
        WebPage,
    }


}

namespace DowJones.Ajax.HeadlineList
{
    /// <summary>
    /// 
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// 
        /// </summary>
        UnSpecified,
        /// <summary>
        /// 
        /// </summary>
        Company,
        /// <summary>
        /// 
        /// </summary>
        Person,
        /// <summary>
        /// 
        /// </summary>
        Organization,
        /// <summary>
        /// 
        /// </summary>
        Industry,
        /// <summary>
        /// 
        /// </summary>
        NewsSubject,
        /// <summary>
        /// 
        /// </summary>
        Region,
        /// <summary>
        /// 
        /// </summary>
        City,

        /// <summary>
        /// 
        /// </summary>
        Highlight,

        /// <summary>
        /// 
        /// </summary>
        Textual,

        /// <summary>
        /// 
        /// </summary>
        InsightIssue,

        /// <summary>
        /// 
        /// </summary>
        InsightDiscovery,
        /// <summary>
        /// 
        /// </summary>
        Author,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 
        /// </summary>
        MergersAndAcquisitions,
        /// <summary>
        /// 
        /// </summary>
        Bankrupcy,
        /// <summary>
        /// 
        /// </summary>
        Sale,
        /// <summary>
        /// 
        /// </summary>
        ProductRelease,
    }


    /// <summary>
    /// 
    /// </summary>
    public enum EventPartType
    {
        /// <summary>
        /// 
        /// </summary>
        Company,
        /// <summary>
        /// 
        /// </summary>
        Industry,
        /// <summary>
        /// 
        /// </summary>
        Person,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Importance
    {
        /// <summary>
        /// 
        /// </summary>
        [AssignedToken("importance")]
        Normal,
        /// <summary>
        /// 
        /// </summary>
        [AssignedToken("breaking")]
        Breaking_Hot,
        /// <summary>
        /// 
        /// </summary>
        [AssignedToken("new")]
        New,
        /// <summary>
        /// 
        /// </summary>
        [AssignedToken("mustRead")]
        MustRead

    }
}
