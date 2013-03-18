// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClusterSetTagConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using DowJones.Ajax.TagCloud;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Assemblers.Search
{
    /// <summary>
    /// The cluster set tag converter.
    /// </summary>
    internal class ClusterSetTagConverter : ITagConverter
    {
        /// <summary>
        /// The labels formatter.
        /// </summary>
        private const string LabelsFormatter = "{0} ";

        /// <summary>
        /// The cluster set.
        /// </summary>
        private readonly ClusterSet clusterSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterSetTagConverter"/> class.
        /// </summary>
        /// <param name="clusterSet">
        /// The cluster set.
        /// </param>
        public ClusterSetTagConverter(ClusterSet clusterSet)
        {
            this.clusterSet = clusterSet;
        }

        #region Implementation of ITagConverter

        /// <summary>
        /// The process.
        /// </summary>
        /// <typeparam name="T">A Type of ITag</typeparam>
        /// <param name="generationRules">The generation rules.</param>
        /// <returns>
        /// A list of Tags
        /// </returns>
        public IEnumerable<T> Process<T>(TagCloudGenerationRules generationRules) where T : ITag, new()
        {
            var tags = new List<KeyValuePair<string, double>>();
            foreach (Cluster cluster in clusterSet.ClusterCollection)
            {
                var labels = new StringBuilder();
                foreach (string lbl in cluster.LabelCollection)
                {
                    labels.AppendFormat(LabelsFormatter, lbl);
                }

                var newPair = new KeyValuePair<string, double>(
                    labels.ToString().Trim(), 
                    cluster.Count);
                tags.Add(newPair);
            }

            return TagGenerator.CreateTags<T>(tags, generationRules);
        }

        #endregion
    }
}