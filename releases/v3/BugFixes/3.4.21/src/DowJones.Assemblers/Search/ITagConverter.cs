// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITagConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Ajax.TagCloud;

namespace DowJones.Assemblers.Search
{
    public interface ITagConverter
    {
        IEnumerable<T> Process<T>(TagCloudGenerationRules generationRules) where T : ITag, new();
    }
}
