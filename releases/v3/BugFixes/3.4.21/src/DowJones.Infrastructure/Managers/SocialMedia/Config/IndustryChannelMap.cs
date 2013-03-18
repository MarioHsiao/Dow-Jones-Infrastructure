using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Collections.Specialized;
using DowJones.Infrastructure;

namespace DowJones.Managers.SocialMedia.Config
{
    public class IndustryChannelMapping
    {
        public string IndustryCode { get; set; }
        public string Channel { get; set; }
    }


    public partial class IndustryChannelMap
    {
        private static MemoryCache _cache = MemoryCache.Default;

        public IDictionary<string, string> Mappings { get; set; }

        public IndustryChannelMap(string mapFile)
        {
            Guard.IsNotNullOrEmpty(mapFile, "mapFile");
            Mappings = ConfigManager.LoadMapFromConfig(mapFile);
            _cache["IndustryChannelMap"] = Mappings;
        }

        public IndustryChannelMap()
        {
            Mappings = LoadInMemoryMap();
            _cache["IndustryChannelMap"] = Mappings;

        }

        public string GetChannelFromIndustryCode(string code)
        {
            if (!Mappings.ContainsKey(code.ToLower()))
                throw new ArgumentOutOfRangeException("code", code, "Industry code not found.");

            return Mappings[code.ToLower()];
        }
    }

    public class ConfigManager
    {
        public static IDictionary<string, string> LoadMapFromConfig(string path)
        {
            Guard.IsNotNullOrEmpty(path, "path");

            var map = new Dictionary<string, string>();
            // TODO: Implement loading from file
            return map;
        }
    }
}
