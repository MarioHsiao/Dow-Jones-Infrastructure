// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InlineMp3Player.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the InlineMp3Player type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.UI;
using DowJones.Web.Mvc.UI.Components.InlineMp3Player;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components;

[assembly: WebResource(InlineMp3PlayerControl.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Components
{
    [ScriptResource("InlineMp3PlayerBehavior", ResourceName = ScriptFile)]
    public class InlineMp3PlayerControl : ViewComponentBase<InlineMp3PlayerModel>
    {

        #region ..:: Constants ::..

        internal const string BaseDirectory = "DowJones.Web.Mvc.UI.Components.InlineMp3Player";
        // The JavaScript file for this module
        internal const string ScriptFile = BaseDirectory + ".InlineMp3PlayerBehavior.js";

        #endregion


        #region ..:: Public Properties ::..

        #region ..:: Tokens ::..

        public string ListenToArticleToken
        {
            get { return Model.ListenToArticleToken; }
            set { Model.ListenToArticleToken = value; }
        }


        public string DownloadToken
        {
            get { return Model.DownloadToken; }
            set { Model.DownloadToken = value; }
        }


        public string AttributionToken
        {
            get { return Model.AttributionToken; }
            set { Model.AttributionToken = value; }
        }

        
        #endregion


        #region ..:: Options ::..


        /// <summary>
        /// Gets or sets the attribution token.
        /// </summary>
        /// <value>The attribution token.</value>
        public string DownloadUrl
        {
            get { return Model.DownloadUrl; }
            set { Model.DownloadUrl = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show download link].
        /// </summary>
        /// <value><c>true</c> if [show download link]; otherwise, <c>false</c>.</value>
        public bool ShowDownloadLink
        {
            get { return Model.ShowDownloadLink; }
            set { Model.ShowDownloadLink = value; }
        }

        /// <summary>
        /// Gets or sets the type of the MP3 player.
        /// </summary>
        /// <value>The type of the MP3 player.</value>
        public MP3PlayerType Mp3PlayerType
        {
            get { return Model.Mp3PlayerType; }
            set { Model.Mp3PlayerType = value; }
        }

        public bool AutoStart
        {
            get { return Model.AutoStart; }
            set { Model.AutoStart = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [auto replay].
        /// </summary>
        /// <value><c>true</c> if [auto replay]; otherwise, <c>false</c>.</value>
        public bool AutoReplay
        {
            get { return Model.AutoReplay; }
            set { Model.AutoReplay = value; }
        }

        /// <summary>
        /// Gets or sets the volume. Values are between 1-100;
        /// </summary>
        /// <value>The volume.</value>
        public int Volume
        {
            get { return Model.Volume; }
            set { Model.Volume = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show time]. Display(mm:ss)
        /// </summary>
        /// <value><c>true</c> if [show time]; otherwise, <c>false</c>.</value>
        public bool ShowTime
        {
            get { return Model.ShowTime; }
            set { Model.ShowTime = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [random play].
        /// </summary>
        /// <value><c>true</c> if [random play]; otherwise, <c>false</c>.</value>
        public bool RandomPlay
        {
            get { return Model.RandomPlay; }
            set { Model.RandomPlay = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [no cursor/pointer].
        /// </summary>
        /// <value><c>true</c> if [no cursor/pointer]; otherwise, <c>false</c>.</value>
        public bool NoPointer
        {
            get { return Model.NoPointer; }
            set { Model.NoPointer = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable transparency]. if transparency is true then background color will not be added
        /// </summary>
        /// <value><c>true</c> if [enable transparency]; otherwise, <c>false</c>.</value>
        public bool EnableTransparency
        {
            get { return Model.EnableTransparency; }
            set { Model.EnableTransparency = value; }
        }

        /// <summary>
        /// Gets or sets the color of the background. (white = FFFFFF, without #)
        /// </summary>
        /// <value>The color of the background.</value>
        public string BackgroundColor
        {
            get { return Model.BackgroundColor; }
            set { Model.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the MP3 file.
        /// Separate URLs (absolute or relative path) with [sep] used to separate multiple Urls.
        /// Example : /media/sons/test.mp3|/media/sons/test2.mp3
        /// </summary>
        /// <value>The URL of the MP3 file.</value>
        public string Mp3FilesUrls
        {
            get { return Model.Mp3FilesUrls; }
            set { Model.Mp3FilesUrls = value; }
        }


        #endregion

        #endregion

        /// <summary>
        /// Gets the inline MP3 player extender.
        /// </summary>
        /// <value>The inline MP3 player extender.</value>
        //private InlineMp3PlayerOptions InlineMp3PlayerExtender
        //{
        //    get
        //    {
        //        if (extender == null)
        //        {
        //            // Create the extender
        //            //extender = new InlineMp3PlayerExtender
        //            //{
        //            //    ID = ID + "_InlineMp3PlayerExtender" //,
        //            //    //BehaviorID = ID + "_InlineMp3PlayerBehavior",
        //            //    //TargetControlID = ID
        //            //};
        //            //Controls.AddAt(0, extender);
        //            extender = new InlineMp3PlayerOptions();
        //        }

        //        return extender;
        //    }
        //}


        public override string ClientPluginName
        {
            get { return "dj_InlineMp3PlayerBehavior"; }
        }



        protected override void WriteContent(HtmlTextWriter writer)
        {

            
            // and then do nothing
            // the UI is created thru JavaScript
        }
    }
}