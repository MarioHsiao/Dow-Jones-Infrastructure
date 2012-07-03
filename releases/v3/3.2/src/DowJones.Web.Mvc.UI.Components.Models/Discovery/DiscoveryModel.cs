using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DowJones.Ajax.Navigator;
using DowJones.Ajax.TagCloud;

namespace DowJones.Web.Mvc.UI.Components.Discovery
{
    [Serializable]
    public class DiscoveryModel : ViewComponentModel
    {
        public Collection<DiscoveryList> DiscoveryCollection { get; set; }

        public DiscoveryModel()
        {
            DiscoveryCollection = new Collection<DiscoveryList>();
        }

        public DiscoveryList AddDiscoveryList(string title, DiscoveryChartTypes chartType, IEnumerable<Tag> tags)
        {
            var list = new DiscoveryList(title, chartType);

            if (tags == null || tags.Count() == 0) return list;
            var tagList = tags.ToList();
            tagList.Sort((a, b) => a.Text.CompareTo(b.Text));
            tagList.ForEach(t => list.DiscoveryItems.Add(new DiscoveryItem
            {
                Count = (int)t.TagWeight.Value,
                Id = t.Text,
                Value = t.Text,
                ShareCount = t.TagWeight.Value
            }));
            list.CalculateShares();
            DiscoveryCollection.Add(list);

            return list;

        }

        public DiscoveryList AddDiscoveryList(string title, DiscoveryChartTypes chartType, Navigator navigator)
        {
            return AddDiscoveryList(title, chartType, navigator, string.Empty);
        }

        public DiscoveryList AddDiscoveryList(string title, DiscoveryChartTypes chartType, Navigator navigator, string dataCode)
        {
            var list = new DiscoveryList(title, chartType) { DataTypeCode = dataCode };

            if (navigator == null || navigator.BucketCollection == null) return list;

            Process(list, navigator.BucketCollection);
            list.CalculateShares();

            DiscoveryCollection.Add(list);

            return list;
        }

        static void Process(DiscoveryList list, IEnumerable<Bucket> bucketlist)
        {
            foreach (var b in bucketlist)
            {
                list.DiscoveryItems.Add(new DiscoveryItem
                {
                    Count = b.HitCount,
                    Id = b.Id,
                    Value = b.Value ?? b.Id,
                    ShareCount = b.HitCount
                });
            }
        }
    }
}
