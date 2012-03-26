// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateController.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Search.Controller
{
    public class DateController
    {
        public DateFormat DateFormat = DateFormat.MMDDCCYY;
        
        public DateQualifier DateQualifier = DateQualifier.ThreeMonths;
        
        //public string Before;

        public int? After { get; set; }

        public int? Before { get; set; }
        
        public string Equal { get; set; }
        
        public DateRange Range { get; set; }
    }
}
