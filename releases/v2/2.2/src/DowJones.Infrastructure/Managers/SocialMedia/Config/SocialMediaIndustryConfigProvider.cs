using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;

namespace DowJones.Managers.SocialMedia.Config
{
    public class SocialMediaIndustryConfigProvider: ISocialMediaIndustryProvider 
    {
        IDictionary<string, string> _mappings;
        private Stream _stream;
        public SocialMediaIndustryConfigProvider(Stream stream){
            _stream = stream;
            _mappings = LoadMappings();
        }

        private IDictionary<string, string> LoadMappings()
        {
            IDictionary<string, string> _mappings = new Dictionary<string, string>();
            XElement _x = XElement.Load(_stream);
            var industries = (from industryitem in _x.Elements("IndustryChannelMapping")
                              select new { ItemCode = industryitem.Element("IndustryCode").Value, Channel = industryitem.Element("Channel").Value }
                             );
            foreach(var item in industries)
            {
                _mappings.Add(item.ItemCode , item.Channel);
            }    
            return _mappings;
        }

        public string GetChannelFromIndustryCode(string industrycode){
            var code = industrycode.ToLower();
            if (!(_mappings.ContainsKey(code)))
                throw new ArgumentOutOfRangeException("code", industrycode, "Industry code not found.");
            return _mappings[code];             
        }
    }
}
