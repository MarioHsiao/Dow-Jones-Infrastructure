// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadSpeaker.cs" company="Dow Jones">
//    © 2010 Dow Jones, Inc. All rights reserved. 	
// </copyright>
// <summary>
//   The read speaker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Web.Mvc.UI.Components
{
    /// <summary>
    /// The read speaker.
    /// </summary>
    public class ReadSpeaker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadSpeaker"/> class.
        /// </summary>
        public ReadSpeaker()
        {
            ShowDownloadLink = false;
            AutoStart = false;
            Volume = 50;
        }

        /// <summary>
        /// Gets or sets a value indicating whether ShowDownloadLink.
        /// </summary>
        public bool ShowDownloadLink { get; set; }

        /// <summary>
        /// Gets or sets Volume.
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AutoStart.
        /// </summary>
        public bool AutoStart { get; set; }
    }
}