// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeywordSetTagConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Ajax.TagCloud.Converters
{
    internal class KeywordSetTagConverter : ITagConverter
    {
        private readonly KeywordSet keywordSet;

        public KeywordSetTagConverter(KeywordSet keywordSet)
        {
            this.keywordSet = keywordSet;
        }

        #region Implementation of ITagConverter

        public IEnumerable<T> Process<T>(TagCloudGenerationRules generationRules) where T : ITag, new()
        {
            var tags = keywordSet.KeywordCollection.Select(keyWord => new KeyValuePair<string, double>(keyWord.Value, keyWord.Weight)).ToList();

            return TagGenerator.CreateTags<T>(tags, generationRules);
        }

        #endregion
    }
}
