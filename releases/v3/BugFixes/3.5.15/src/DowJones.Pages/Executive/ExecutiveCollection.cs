// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages.Executive
{
    [CollectionDataContract(Name = "executives", Namespace = "")]
    public class ExecutiveCollection : List<Executive>
    {
        public ExecutiveCollection() : base()
        {
        }

        public ExecutiveCollection(IEnumerable<Executive> collection) : base(collection)
        {
        }
    }
}
