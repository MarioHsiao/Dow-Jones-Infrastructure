using System.Collections.Generic;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Components.Common;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;

namespace DowJones.Web.Mvc.UI.Components.Search
{
    public class SourceGroupToSourceGroupItemMapper : TypeMapper<SourceGroup, SourceGroupItem>
    {
        public override SourceGroupItem Map(SourceGroup sourceGroup)
        {
            SourceGroupItem sourceGroupItem = null;
            if (sourceGroup != null)
            {
                sourceGroupItem = new SourceGroupItem { Code = sourceGroup.PdfCode, Desc = sourceGroup.Descriptor };
                if (sourceGroup.SourceGroupCollection != null && sourceGroup.SourceGroupCollection.Count > 0)
                {
                    sourceGroupItem.SourceGroupCollection = new List<SourceGroupItem>();
                    foreach (var group in sourceGroup.SourceGroupCollection)
                    {
                        sourceGroupItem.SourceGroupCollection.Add(GetSourceGroupItem(group));
                    }
                }
            }
            return sourceGroupItem;
        }

        private static SourceGroupItem GetSourceGroupItem(SourceGroup sourceGroup)
        {
            SourceGroupItem sourceGroupItem = null;
            if (sourceGroup != null)
            {
                sourceGroupItem = new SourceGroupItem { Code = sourceGroup.PdfCode, Desc = sourceGroup.Descriptor };
                if (sourceGroup.SourceGroupCollection != null && sourceGroup.SourceGroupCollection.Count > 0)
                {
                    sourceGroupItem.SourceGroupCollection = new List<SourceGroupItem>();
                    foreach (var group in sourceGroup.SourceGroupCollection)
                    {
                        sourceGroupItem.SourceGroupCollection.Add(GetSourceGroupItem(group));
                    }
                }
            }
            return sourceGroupItem;
        }
    }
}