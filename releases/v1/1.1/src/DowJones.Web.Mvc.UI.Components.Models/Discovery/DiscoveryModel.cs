using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Tools.Ajax.HeadlineList;

namespace DowJones.Web.Mvc.UI.Components.Models.Discovery
{
    [Serializable]
    public class DiscoveryModel
    {
        public List<DiscoveryList> DiscoveryCollection { get; set; }

        public DiscoveryModel()
        {
            this.DiscoveryCollection  = new List<DiscoveryList>();
        }       
        public DiscoveryList AddDiscoveryList(string title, DiscoveryChartTypes chartType, IEnumerable<DowJones.Utilities.Ajax.TagCloud.Tag> tags)
        { 
            DiscoveryList list = new DiscoveryList(title, chartType);

            if (tags == null || tags.Count() == 0) return list;
            var tagList = tags.ToList();
            tagList.Sort((a, b) => a.Text.CompareTo(b.Text));
            tagList.ForEach(t => list.DiscoveryItems.Add(new DiscoveryItem()
            {
                Count = (int)t.TagWeight.Value,
                Id = t.Text,
                Value = t.Text,
                ShareCount = t.TagWeight.Value
            }));
            list.CalculateShares();
            this.DiscoveryCollection.Add(list);

            return list;
        
        }
        public DiscoveryList AddDiscoveryList(string title, DiscoveryChartTypes chartType, DowJones.Ajax.Navigator.Navigator navigator)
        {
            return AddDiscoveryList(title, chartType, navigator, string.Empty);
        }
        public DiscoveryList AddDiscoveryList(string title, DiscoveryChartTypes chartType, DowJones.Ajax.Navigator.Navigator navigator, string dataCode)
        {
            DiscoveryList list = new DiscoveryList(title, chartType) { DataTypeCode = dataCode };

            if (navigator == null || navigator.BucketCollection == null) return list;

            Process(list, navigator.BucketCollection);
            list.CalculateShares();

            this.DiscoveryCollection.Add(list);

            return list;


        }
        static void Process(DiscoveryList list, List<Ajax.Navigator.Bucket> bucketlist)
        {
            foreach (var b in bucketlist)
            {
                list.DiscoveryItems.Add(new DiscoveryItem
                {
                     Count = b.HitCount,
                     Id = b.Id,
                     Value = b.Value ?? b.Id,
                     ShareCount=b.HitCount
                });
            }
        }           
    }
}
