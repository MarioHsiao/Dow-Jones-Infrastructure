// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourcesNewsPageServicePartResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult
{
    [CollectionDataContract(Name = "sourceCollection", ItemName = "source", Namespace = "")]
    public class SourceCollection : List<string>
    {
        public SourceCollection(IEnumerable<string> collection)
            : base(collection)
        {
        }

        public SourceCollection()
        {
        }
    }
    
    [DataContract(Name = "sourcesNewsPageModuleServicePartResult", Namespace = "")]
    public class SourcesNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> 
        where TPackage : AbstractHeadlinePackage
    {
    }
}