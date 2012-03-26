
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "customTopicsPackage", Namespace = "")]
    [KnownType(typeof(CustomTopicsPackage))]
    public class CustomTopicsPackage : AbstractHeadlinePackage, IViewAllSearchContextRef
    {
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        #region Implementation of IViewAllSearchContextRef

        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }

        #endregion
    }
}
