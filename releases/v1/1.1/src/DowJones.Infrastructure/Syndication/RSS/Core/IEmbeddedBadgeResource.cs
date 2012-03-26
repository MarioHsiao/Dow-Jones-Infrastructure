// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEmbeddedBadgeResource.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the IEmbeddedBadgeResource type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Utilities.Syndication.RSS
{
    public interface IEmbeddedBadgeResource
    {
        /// <summary>
        /// Gets the embedded badget URL.
        /// </summary>
        /// <returns>A string representing the URL.</returns>
        string GetEmbeddedBadgetUrl();
    }
}
