// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryEntityAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.Common;
using QueryEntity = Factiva.Gateway.Messages.Assets.Pages.V1_0.QueryEntity;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common
{
    public class QueryEntityAssembler : IAssembler<Models.Common.QueryEntity, QueryEntity>
    {
        #region Implementation of IAssembler<out QueryEntity,in QueryEntity>

        public Models.Common.QueryEntity Convert(QueryEntity source)
        {
            return new Models.Common.QueryEntity
                       {
                           ResultSortOrder = (ResultSortOrder)source.ResultSortOrder,
                           DeduplicationType = (DeduplicationType)source.DeduplicationType,
                           Query = new Query
                                       {
                                           Text = FlattenQueryCollection(source.QueryCollection),
                                           SearchMode = FlattenSeachMode(source.QueryCollection),
                                       }
                       };
        }

        #endregion

        private static string FlattenQueryCollection(IList<Factiva.Gateway.Messages.Assets.Pages.V1_0.Query> collection)
        {
            return collection.Count > 0 ? collection[0].Text : string.Empty;
        }

        private static SearchMode FlattenSeachMode(IList<Factiva.Gateway.Messages.Assets.Pages.V1_0.Query> collection)
        {
            if (collection.Count > 0)
            {
                switch (collection[0].SearchMode)
                {
                        case Factiva.Gateway.Messages.Assets.Pages.V1_0.SearchMode.Simple:
                            return SearchMode.Simple;
                    default:
                        return SearchMode.Traditional;
                }
            }

            return SearchMode.Traditional;
        }
    }
}
