
namespace DowJones.Infrastructure.Alert
{
    /// <summary>
    /// PK[2013-02-13]: this enumerator is the same as the one in .COM
    /// </summary>
    public enum DeliveryContentType
    {
        /// <summary/>
        [AlertRequestBinder("article")]
        Article,
        /// <summary/>
        [AlertRequestBinder("report")]
        Report,
        /// <summary/>
        [AlertRequestBinder("file")]
        File,
        /// <summary/>
        [AlertRequestBinder("picture")]
        Picture,
        /// <summary/>
        [AlertRequestBinder("inventory")]
        Inventory,
        /// <summary/>
        [AlertRequestBinder("coinfo")]
        Coinfo,
        /// <summary/>
        [AlertRequestBinder("webpage")]
        Webpage,
        /// <summary/>
        [AlertRequestBinder("internal")]
        Internal,
        /// <summary/>
        [AlertRequestBinder("publication")]
        Publication,
        /// <summary/>
        [AlertRequestBinder("blog")]
        Blog,
        /// <summary/>
        [AlertRequestBinder("ablog")]
        ABlog,
        /// <summary/>
        [AlertRequestBinder("mscblog")]
        TwitterAndFlickr,
        /// <summary/>
        [AlertRequestBinder("tailblog")]
        TailBlog,
        /// <summary/>
        [AlertRequestBinder("board")]
        Board,
        /// <summary/>
        [AlertRequestBinder("all")]
        All

    }
}
