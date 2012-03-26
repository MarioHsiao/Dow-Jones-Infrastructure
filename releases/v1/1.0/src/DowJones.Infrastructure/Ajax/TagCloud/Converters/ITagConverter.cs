// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITagConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DowJones.Utilities.Ajax.TagCloud.Converters
{
    public interface ITagConverter
    {
        IEnumerable<T> Process<T>(TagCloudGenerationRules generationRules) where T : ITag, new();
    }
}
