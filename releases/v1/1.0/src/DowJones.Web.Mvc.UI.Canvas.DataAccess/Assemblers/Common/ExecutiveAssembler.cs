// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutiveAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Factiva.Gateway.Messages.FCE.Assets.V1_0;
using Executive = DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.Executive;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common
{
    internal class ExecutiveAssembler : IAssembler<IEnumerable<Executive>, List<ReportExecutive>>
    {
        #region Implementation of IAssembler<out IEnumerable<Executive>,in List<ReportExecutive>>

        public IEnumerable<Executive> Convert(List<ReportExecutive> source)
        {
            return source.Select(reportExecutive => new Executive
                                                        {
                                                            FirstName = reportExecutive.Name.FirstName, 
                                                            MiddleNames = reportExecutive.Name.MiddleNames, 
                                                            LastName = reportExecutive.Name.LastName, 
                                                            CompleteName = reportExecutive.Name.FullName, 
                                                            Suffix = reportExecutive.Name.Suffix, 
                                                            ConsolidationId = reportExecutive.ConsolidationId, 
                                                            JobTitle = reportExecutive.JobTitle.Value, 
                                                            IsEmployee = reportExecutive.IsEmployee, 
                                                            IsOfficer = reportExecutive.IsOfficer ?? false,
                                                        }).ToList();
        }

        #endregion
    }
}
