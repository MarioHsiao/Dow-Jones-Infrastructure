using System;

namespace DowJones.Security.Interfaces
{
    public interface IAlertsService 
    {
        bool IsTrackAdministrator { get; }
        bool IsDMMUser { get; }
        bool IsSelectFullUser { get; }
        bool IsSelectHeadlinesUser { get; }
        bool IsAlertsUser { get; }

        /// <summary>
        /// Gets MaxFoldersForGlobal.
        /// </summary>
        int MaxFoldersForGlobal { get; }

        /// <summary>
        /// Gets MaxFoldersForSelectHeadline.
        /// </summary>
        int MaxFoldersForSelectHeadline { get; }

        /// <summary>
        /// Gets the max folders for select full text.
        /// </summary>
        /// <value>The max folders for select full text.</value>
        int MaxFoldersForSelectFullText { get; }

        /// <summary>
        /// Gets the max folders for fast alert.
        /// </summary>
        /// <value>The max folders for fast alert.</value>
        int MaxFoldersForFastAlert { get; }

        /// <summary>
        /// Gets the max folders for companies and executives.
        /// </summary>
        /// <value>The max folders for companies and executives.</value>
        int MaxFoldersForCompaniesAndExecutives { get; }

        /// <summary>
        /// Gets MaxFolderForDmm.
        /// </summary>
        [Obsolete]
        int MaxFolderForDmm { get; }

        /// <summary>
        /// Gets MaxFolderForTrigger.
        /// </summary>
        [Obsolete]
        int MaxFolderForTrigger { get; }

        int MaximumPersonalFoldersEmailDelivery { get; }

        bool HasPressClipsEnabled { get; }
        }
}