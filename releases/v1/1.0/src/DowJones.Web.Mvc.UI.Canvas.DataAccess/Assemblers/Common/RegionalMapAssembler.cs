// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionalMapAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Utilities.Formatters;
using Factiva.Gateway.Messages.PCM.RegionalMap.V1_0;
using NewsEntityNewsVolumeVariation = DowJones.Web.Mvc.UI.Models.Common.NewsEntityNewsVolumeVariation;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common
{
    public class RegionalMapAssembler : IAssembler<List<NewsEntityNewsVolumeVariation>, PCMRegionalMapResponse>
    {
        #region Implementation of IAssembler<out List<NewsEntityNewsVolumeVariation>,in PCMRegionalMapResponse>

        public List<NewsEntityNewsVolumeVariation> Convert(PCMRegionalMapResponse source)
        {
            var output = new List<NewsEntityNewsVolumeVariation>();

            if (source != null &&
                source.RegionalMapNewsVolumePackage != null &&
                source.RegionalMapNewsVolumePackage.RegionNewsVolume != null &&
                source.RegionalMapNewsVolumePackage.RegionNewsVolume.NewsEntityNewsVolumeVariationCollection.Count > 0)
            {
                output.AddRange(source.RegionalMapNewsVolumePackage.RegionNewsVolume.NewsEntityNewsVolumeVariationCollection
                    .Select(entity => new NewsEntityNewsVolumeVariation
                            {
                                Code = entity.Code, 
                                CurrentTimeFrameNewsVolume = new WholeNumber(entity.CurrentTimeFrameNewsVolume.Value), 
                                NewEntrant = entity.NewEntrant,
                                PercentVolumeChange = new Percent(entity.PercentVolumeChange.Value),
                                PreviousTimeFrameNewsVolume = new WholeNumber(entity.PreviousTimeFrameNewsVolume.Value),
                                SearchContextRef = entity.SearchContextRef,
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
