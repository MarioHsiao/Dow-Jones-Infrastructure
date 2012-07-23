using System;
using System.Globalization;
using Newtonsoft.Json;

namespace DowJones.Pages
{
    public class PageReference
    {
        [JsonIgnore]
        public int PageId
        {
            get { return _pageId.Value; }
        }
        private readonly Lazy<int> _pageId;

        [JsonProperty(PropertyName = "pid")]
        public string ParentId
        {
            get { return _parentId.Value; }
        }
        private readonly Lazy<string> _parentId;

        [JsonProperty(PropertyName = "id")]
        public string Value { get; private set; }


        public PageReference(string pageRef)
        {
            Value = pageRef;
            _pageId = new Lazy<int>(GetPageId);
            _parentId = new Lazy<string>(GetParentId);
        }


        protected virtual int GetPageId()
        {
            if (string.IsNullOrWhiteSpace(Value))
                return -1;

            int pageId;
            Int32.TryParse(Value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out pageId);
            return pageId;
        }

        protected virtual string GetParentId()
        {
            var temp = (Value ?? string.Empty).Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (temp.Length == 5)
                return temp[3];

            return null;
        }


        public static implicit operator PageReference(string pageRef)
        {
            return new PageReference(pageRef);
        }

        public static implicit operator string(PageReference pageRef)
        {
            return pageRef.Value;
        }
    }
}