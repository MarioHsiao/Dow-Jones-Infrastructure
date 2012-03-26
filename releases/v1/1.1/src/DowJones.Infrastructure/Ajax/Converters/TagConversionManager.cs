using System.Collections.Generic;
using DowJones.Utilities.Ajax.TagCloud;
using DowJones.Utilities.Ajax.TagCloud.Converters;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Utilities.Ajax.Converters
{
    public class TagConversionManager
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(TagConversionManager));


        /// <summary>
        /// Processes the specified navigator control.
        /// </summary>
        /// <param name="keywordSet">The keyword set.</param>
		/// <param name="generationRules">The tag generation rules.</param>
        /// <returns></returns>
        public IEnumerable<Tag> Process(KeywordSet keywordSet, TagCloudGenerationRules generationRules)
        {
            var converter = new KeywordSetTagConverter(keywordSet);
            return (IEnumerable<Tag>)converter.Process<Tag>(generationRules);
        }

        /// <summary>
        /// Processes the specified navigator control.
        /// </summary>
        /// <param name="clusterSet">The cluster set.</param>
		/// <param name="generationRules">The tag generation rules.</param>
        /// <returns></returns>
		public IEnumerable<Tag> Process(ClusterSet clusterSet, TagCloudGenerationRules generationRules)
        {
            var converter = new ClusterSetTagConverter(clusterSet);
            return (IEnumerable<Tag>)converter.Process<Tag>(generationRules);
        }
    }
}
