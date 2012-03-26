// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SummaryRegionalMapAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Utilities.Formatters;
using Factiva.Gateway.Messages.PCM.SummaryRegionalMap.V1_0;
using NewsEntity = DowJones.Web.Mvc.UI.Models.Common.NewsEntity;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common
{
    public class SummaryRegionalMapAssembler : IAssembler<List<NewsEntity>, PCMSummaryRegionalMapResponse>
    {
        #region Implementation of IAssembler<out List<NewsEntity>,in PCMSummaryRegionalMapResponse>

        public List<NewsEntity> Convert(PCMSummaryRegionalMapResponse source)
        {
            var output = new List<NewsEntity>();

            if (source != null &&
                source.SummaryRegionalMapPackage != null &&
                source.SummaryRegionalMapPackage.RegionNewsVolume != null &&
                source.SummaryRegionalMapPackage.RegionNewsVolume.NewsEntityCollection.Count > 0)
            {
                output.AddRange(source.SummaryRegionalMapPackage.RegionNewsVolume.NewsEntityCollection
                    .Select(entity => new NewsEntity
                    {
                        Code = entity.Code,
                        CurrentTimeFrameNewsVolume = new WholeNumber(entity.CurrentTimeFrameNewsVolume.Value),
                        Type = MapEntityType(entity.Type),
                    }));
            }

            return output;
        }

        #endregion

        private static Tools.Ajax.HeadlineList.EntityType MapEntityType(EntityType entityType)
        {
            return (Tools.Ajax.HeadlineList.EntityType)Enum.Parse(typeof(Tools.Ajax.HeadlineList.EntityType), entityType.ToString());
        }
    }
}
