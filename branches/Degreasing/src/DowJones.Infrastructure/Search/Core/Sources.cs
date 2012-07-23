// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



using System.Collections.Generic;
using Factiva.Gateway.Messages.Preferences.V1_0;
namespace DowJones.Utilities.Search.Core
{
    public class SourceQueryContainer
    {
        public List<string> Codes = new List<string>();
        public string Query { get; set; }
    }

    public class SourceCodes
    {
        public SourceQueryContainer Pubication = new SourceQueryContainer();
        public SourceQueryContainer WebPage = new SourceQueryContainer();
        public SourceQueryContainer Multimedia = new SourceQueryContainer();
        public SourceQueryContainer Blogs = new SourceQueryContainer();
        public SourceQueryContainer Picture = new SourceQueryContainer();
        public SourceQueryContainer Customer = new SourceQueryContainer();
        public SourceQueryContainer Board = new SourceQueryContainer();

        public bool IsExcluded { get; set; }
    }

    public class Source
    {
        public string Name { get; set; }
        public string SourceCode { get; set; }
        public string GroupCode { get; set; }
        public SourceType Type { get; set; }
    }
}
