// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using DowJones.Utilities.Formatters;

namespace DowJones.Utilities.Ajax.TagCloud
{
    /// <summary>
    /// <para>
    /// Provides static factory methods for generation of IEnumerable&lt;Tag&gt;.
    /// </para>
    /// </summary>
    public class TagGenerator
    {
        #region CreateTags

        /// <summary>
        /// Creates Tag objects from a provided list of string tags.
        /// The more times a tag occurs in the list, the larger weight it will get in the tag cloud.
        /// </summary>
        /// <typeparam name="T">A generic type that supports the ITag interface.</typeparam>
        /// <param name="tags">A string list of tags</param>
        /// <param name="generationRules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>
        /// A list of Tag objects that can be used to create the tag cloud.
        /// </returns>
        public static IEnumerable<T> CreateTags<T>(IEnumerable<string> tags, TagCloudGenerationRules generationRules) where T : ITag, new()
        {
            if (tags == null)
            {
                throw new ArgumentNullException("tags");
            }

            if (generationRules == null)
            {
                throw new ArgumentNullException("generationRules");
            }
            
            // Transform tag string list to list with each distinct tag and its weight
            var tagDict = new Dictionary<string, double>();
            foreach (string t in tags)
            {
                if (!tagDict.ContainsKey(t))
                {
                    tagDict.Add(t, 1);
                }
                else
                {
                    ++tagDict[t];
                }
            }

            return CreateTags<T>(tagDict, generationRules);
        }

        /// <summary>
        /// Creates Tag objects from a provided dictionary of string tags along with integer values indicating the weight of each tag.
        /// This overload is suitable when you have a list of already weighted tags, i.e. from a database query result.
        /// </summary>
        /// <typeparam name="T">A generic type that supports the ITag interface.</typeparam>
        /// <param name="weightedTags">A dictionary that takes a string for the tag text (as the dictionary key) and an integer for the tag weight (as the dictionary value).</param>
        /// <param name="generationRules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>
        /// A list of Tag objects that can be used to create the tag cloud.
        /// </returns>
        public static IEnumerable<T> CreateTags<T>(IDictionary<string, double> weightedTags, TagCloudGenerationRules generationRules) where T : ITag, new()
        {
            if (weightedTags == null)
            {
                throw new ArgumentNullException("weightedTags");
            }

            if (generationRules == null)
            {
                throw new ArgumentNullException("generationRules");
            }

            var tags = new List<KeyValuePair<string, double>>();
            var e = weightedTags.GetEnumerator();
            while (e.MoveNext())
            {
                tags.Add(e.Current);
            }

            return CreateTags<T>(tags, generationRules);
        }

        /// <summary>
        /// Creates Tag objects from a provided list of string tags along with integer values indicating the weight of each tag.
        /// This overload is suitable when you have a list of already weighted tags, i.e. from a database query result.
        /// </summary>
        /// <typeparam name="T">A generic type that supports the ITag interface.</typeparam>
        /// <param name="weightedTags">A list of KeyValuePair objects that take a string for the tag text and an integer for the weight of the tag.</param>
        /// <param name="generationRules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>
        /// A list of Tag objects that can be used to create the tag cloud.
        /// </returns>
        public static IEnumerable<T> CreateTags<T>(IEnumerable<KeyValuePair<string, double>> weightedTags, TagCloudGenerationRules generationRules) where T : ITag, new()
        {
            if (weightedTags == null)
            {
                throw new ArgumentNullException("weightedTags");
            }

            if (generationRules == null)
            {
                throw new ArgumentNullException("generationRules");
            }
            
            // Select all tags that exists "settings.RequiredTagWeight" times or more and order them by weight.
            weightedTags = from weightedTag in weightedTags
                           where weightedTag.Value >= generationRules.RequiredTagWeight
                           orderby weightedTag.Value descending
                           select weightedTag;

            // Crop list if "settings.MaxNumberOfTags" is specified
            if (generationRules.MaxNumberOfTags.HasValue)
            {
                weightedTags = weightedTags.Take(generationRules.MaxNumberOfTags.Value);
            }

            // Change sort order if necessary (the list is already ordered by weight descending)
            switch (generationRules.Order)
            {
                case TagCloudOrder.Alphabetical: // Renders tags alphabetically
                    weightedTags = weightedTags.OrderBy(tag => tag.Key);
                    break;
                case TagCloudOrder.AlphabeticalDescending: // Renders tags alphabetically descending
                    weightedTags = weightedTags.OrderByDescending(tag => tag.Key);
                    break;
                case TagCloudOrder.Weight: // Renders tags with higher weight at the end
                    weightedTags = weightedTags.OrderBy(tag => tag.Value);
                    break;
                case TagCloudOrder.Centralized: // Renders tags with higher weight in the middle
                    weightedTags = weightedTags.OrderBy(tag => tag.Value);
                    weightedTags = weightedTags.Where((kvp, i) => (i % 2 == 0)).Concat(weightedTags.Where((kvp, i) => (i % 2 == 1)).Reverse());
                    break;
                case TagCloudOrder.Decentralized: // Renders tags with higher weight at the edges (start and end)
                    weightedTags = weightedTags.OrderBy(tag => tag.Value);
                    weightedTags = weightedTags.Where((kvp, i) => (i % 2 == 0)).Reverse().Concat(weightedTags.Where((kvp, i) => (i % 2 == 1)));
                    break;
                case TagCloudOrder.Random: // Renders tags randomly
                    weightedTags = weightedTags.OrderBy(tag => tag, RandomComparer.Comparer);
                    break;
            }

            // Retrieve the css class table used to decide the style of the tags
            var distributionIndexTable = GenerateWeightDistributionIndexTable(weightedTags, generationRules.WeightClassPartitioning.ToArray());
            // Transform all the string tags into Tag objects
            var cloudTags = from weightedTag in weightedTags
                            select new T
                            {
                                Text = weightedTag.Key,
                                TagWeight = new DoubleNumber(weightedTag.Value),
                                DistributionIndex =  distributionIndexTable[(int)Math.Ceiling(weightedTag.Value)],
                                NavigateUrl = string.Format(generationRules.TagUrlFormatString, HttpUtility.UrlEncode(weightedTag.Key), weightedTag.Value),
                                Snippet = string.Format(generationRules.TagToolTipFormatString, weightedTag.Value.ToString("#.##", NumberFormatInfo.InvariantInfo)),
                            };
            return cloudTags;
        }

        #endregion

        #region GenerateCssClassTable

        /// <summary>
        /// Generates a weight distribution index table so we can keep track of what tag weights relates to what distribution index.
        /// The result is based upon the provided list of FontSizeOccurrence objects.
        /// </summary>
        /// <param name="weightedTags">The weighted Tags.</param>
        /// <param name="tagWeightDistribution">The tag Weight Distribution.</param>
        /// <returns>A dictionary object of integers and strings</returns>
        private static Dictionary<int, int> GenerateWeightDistributionIndexTable(IEnumerable<KeyValuePair<string, double>> weightedTags, int[] tagWeightDistribution)
        {
            // Group tags with the same weight together and get the number of tags for each weight to produce a list that tells us how many tags has a specific weight.
            var weightOccurrences = from weightedTag in weightedTags
                                    group weightedTag by weightedTag.Value
                                        into weightedTagGroup
                                        orderby weightedTagGroup.Key
                                        select new
                                        {
                                            Weight = weightedTagGroup.Key,
                                            NumberOfTags = weightedTagGroup.Count()
                                        };

            var weightDistributionIndexTable = new Dictionary<int, int>();
            int distinctTagsCount = weightedTags.Count();
            int tagWeightDistributionIndex = tagWeightDistribution.Length, percentageCovered = 0, tagsCovered = 0;

            // Distribute the css classes according to the tagWeightDistribution parameter
            foreach (var weightOccurrence in weightOccurrences)
            {
                weightDistributionIndexTable[(int)Math.Ceiling(weightOccurrence.Weight)] = tagWeightDistributionIndex;

                tagsCovered += weightOccurrence.NumberOfTags;
                if (tagsCovered / (double)distinctTagsCount > (tagWeightDistribution[tagWeightDistributionIndex - 1] + percentageCovered) * 0.01)
                {
                    percentageCovered += tagWeightDistribution[(tagWeightDistributionIndex--) - 1];
                }
            }

            return weightDistributionIndexTable;
        }


        /// <summary>
        /// Generates a css class table so we can keep track of what tag weights relates to what css class.
        /// The result is based upon the provided list of FontSizeOccurrence objects.
        /// </summary>
        /// <param name="weightedTags">The weighted Tags.</param>
        /// <param name="tagCssClassPrefix">The tag css Class Prefix.</param>
        /// <param name="tagWeightDistribution">The tag Weight Distribution.</param>
        /// <returns>A dictionary object of integers and strings</returns>
        private static Dictionary<int, string> GenerateCssClassTable(IEnumerable<KeyValuePair<string, double>> weightedTags, string tagCssClassPrefix, int[] tagWeightDistribution)
        {
            // Group tags with the same weight together and get the number of tags for each weight to produce a list that tells us how many tags has a specific weight.
            var weightOccurrences = from weightedTag in weightedTags
                                    group weightedTag by weightedTag.Value
                                    into weightedTagGroup
                                    orderby weightedTagGroup.Key
                                    select new
                                               {
                                                   Weight = weightedTagGroup.Key, 
                                                   NumberOfTags = weightedTagGroup.Count()
                                               };

            var cssClassTable = new Dictionary<int, string>();
            int distinctTagsCount = weightedTags.Count();
            int tagWeightDistributionIndex = tagWeightDistribution.Length, percentageCovered = 0, tagsCovered = 0;

            // Distribute the css classes according to the tagWeightDistribution parameter
            foreach (var weightOccurrence in weightOccurrences)
            {
                cssClassTable[(int)Math.Ceiling(weightOccurrence.Weight)] = string.Concat(tagCssClassPrefix, tagWeightDistributionIndex);

                tagsCovered += weightOccurrence.NumberOfTags;
                if (tagsCovered / (double)distinctTagsCount > (tagWeightDistribution[tagWeightDistributionIndex - 1] + percentageCovered) * 0.01)
                {
                    percentageCovered += tagWeightDistribution[(tagWeightDistributionIndex--) - 1];
                }
            }

            return cssClassTable;
        }

        #endregion
    }

    /// <summary>
    /// Comparer class that can be used to sort any object in a list randomly.
    /// </summary>
    internal class RandomComparer : IComparer<object>, IComparer
    {
        /// <summary>
        /// The rngcsp.
        /// </summary>
        private static readonly RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider();

        /// <summary>
        /// Gets Comparer.
        /// </summary>
        public static RandomComparer Comparer
        {
            get { return new RandomComparer(); }
        }

        #region IComparer Members

        /// <summary>
        /// The implementation of ICompare.Compare.
        /// </summary>
        /// <param name="x">
        /// The x parameter.
        /// </param>
        /// <param name="y">
        /// The y parameter.
        /// </param>
        /// <returns>
        /// The compare.
        /// </returns>
        int IComparer.Compare(object x, object y)
        {
            return ((IComparer<object>)this).Compare(x, y);
        }

        #endregion

        #region IComparer<object> Members

        /// <summary>
        /// The compare.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The compare.
        /// </returns>
        int IComparer<object>.Compare(object x, object y)
        {
            if (x == y)
            {
                return 0;
            }

            var randomByte = new byte[1];
            rngcsp.GetBytes(randomByte);
            return (randomByte[0] % 2 == 0) ? 1 : -1;
        }

        #endregion
    }
}