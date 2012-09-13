﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwitterUser.cs" company="">
//   
// </copyright>
// <summary>
//   This data class provides more information than the basic data provided by
//   returned in other calls for friends and followers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Infrastructure.Models.SocialMedia
{
    // using DowJones.Infrastructure.Models.SocialMedia.Responses;

    /// <summary>
    /// This data class provides more information than the basic data provided by
    ///   <see cref="TwitterUser"/> returned in other calls for friends and followers.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class TwitterUser : IComparable<TwitterUser>, IEquatable<TwitterUser>, ITweeter
    {
        private const string TwitterUrl = "http://twitter.com/";

        #region Properties

        /// <summary>
        ///   Gets or sets CreatedDate.
        /// </summary>
        [JsonProperty("created_at")]
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        ///   Gets or sets Description.
        /// </summary>
        [JsonProperty("description")]
        public virtual string Description { get; set; }

        /// <summary>
        ///   Gets or sets FavouritesCount.
        /// </summary>
        [JsonProperty("favourites_count")]
        public virtual int FavouritesCount { get; set; }

        /// <summary>
        ///   Gets or sets the followers count.
        /// </summary>
        /// <value>The followers count.</value>
        [JsonProperty("followers_count")]
        public virtual int FollowersCount { get; set; }

        /// <summary>
        ///   Gets or sets FriendsCount.
        /// </summary>
        [JsonProperty("friends_count")]
        public virtual int FriendsCount { get; set; }

        /// <summary>
        ///   Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        [JsonProperty("id")]
        public virtual int Id { get; set; }

        /// <summary>
        ///   Gets or sets IsGeoEnabled.
        /// </summary>
        [JsonProperty("geo_enabled")]
        public virtual bool? IsGeoEnabled { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether IsProfileBackgroundTiled.
        /// </summary>
        [JsonProperty("profile_background_tile")]
        public virtual bool IsProfileBackgroundTiled { get; set; }

        /// <summary>
        ///   Gets or sets IsProtected.
        /// </summary>
        [JsonProperty("protected")]
        public virtual bool? IsProtected { get; set; }

        /// <summary>
        ///   Gets or sets IsVerified.
        /// </summary>
        [JsonProperty("verified")]
        public virtual bool? IsVerified { get; set; }

        /// <summary>
        ///   Gets or sets Language.
        /// </summary>
        [JsonProperty("lang")]
        public virtual string Language { get; set; }

        /// <summary>
        ///   Gets or sets Location.
        /// </summary>
        [JsonProperty("location")]
        public virtual string Location { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        [JsonProperty("name")]
        public virtual string Name { get; set; }

        /// <summary>
        ///   Gets or sets ProfileBackgroundColor.
        /// </summary>
        [JsonProperty("profile_background_color")]
        public virtual string ProfileBackgroundColor { get; set; }

        /// <summary>
        ///   Gets or sets ProfileBackgroundImageUrl.
        /// </summary>
        [JsonProperty("profile_background_image_url")]
        public virtual string ProfileBackgroundImageUrl { get; set; }

        /// <summary>
        ///   Gets or sets ProfileLinkColor.
        /// </summary>
        [JsonProperty("profile_link_color")]
        public virtual string ProfileLinkColor { get; set; }

        /// <summary>
        ///   Gets or sets ProfileSidebarBorderColor.
        /// </summary>
        [JsonProperty("profile_sidebar_border_color")]
        public virtual string ProfileSidebarBorderColor { get; set; }

        /// <summary>
        ///   Gets or sets ProfileSidebarFillColor.
        /// </summary>
        [JsonProperty("profile_sidebar_fill_color")]
        public virtual string ProfileSidebarFillColor { get; set; }

        /// <summary>
        ///   Gets or sets ProfileTextColor.
        /// </summary>
        [JsonProperty("profile_text_color")]
        public virtual string ProfileTextColor { get; set; }

        /*
        /// <summary>
        ///   Gets or sets Status.
        /// </summary>
        [DataMember(Name = "status")]
        public virtual TwitterStatus Status { get; set; }
        */

        /// <summary>
        ///   Gets or sets StatusesCount.
        /// </summary>
        [JsonProperty("statuses_count")]
        public virtual int StatusesCount { get; set; }

        /// <summary>
        ///   Gets or sets TimeZone.
        /// </summary>
        [JsonProperty("time_zone")]
        public virtual string TimeZone { get; set; }

        /// <summary>
        ///   Gets or sets Url.
        /// </summary>
        [JsonProperty("url")]
        public virtual string Url { get; set; }

        /// <summary>
        ///   Gets or sets UtcOffset.
        /// </summary>
        [JsonProperty("utc_offset")]
        public virtual string UtcOffset { get; set; }

        /// <summary>
        ///   Gets or sets ProfileImageUrl.
        /// </summary>
        [JsonProperty("profile_image_url")]
        public virtual string ProfileImageUrl { get; set; }

        /// <summary>
        ///   Gets or sets ScreenName.
        /// </summary>
        [JsonProperty("screen_name")]
        public virtual string ScreenName { get; set; }

        #endregion

        #region Operators

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TwitterUser left, TwitterUser right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TwitterUser left, TwitterUser right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="user">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        public override bool Equals(object user)
        {
            if (ReferenceEquals(null, user))
            {
                return false;
            }

            if (ReferenceEquals(this, user))
            {
                return true;
            }

            return user.GetType() == typeof(TwitterUser) && Equals((TwitterUser)user);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable<TwitterUser>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. 
        ///   The return value has the following meanings: 
        ///   Less than zero: This object is less than the <paramref name="user"/> parameter.
        ///   Zero: This object is equal to <paramref name="user"/>. 
        ///   Greater than zero: This object is greater than <paramref name="user"/>.
        /// </returns>
        public int CompareTo(TwitterUser user)
        {
            return user.Id == Id ? 0 : user.Id > Id ? -1 : 1;
        }

        #endregion

        #region IEquatable<TwitterUser>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="user"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TwitterUser user)
        {
            if (ReferenceEquals(null, user))
            {
                return false;
            }

            if (ReferenceEquals(this, user))
            {
                return true;
            }

            return user.Id == Id;
        }

        #endregion

        #endregion

        public string ProfileHashUrl
        {
            get { return TwitterUrl + "#!/" + ScreenName; }
        }

        public string ProfileUrl
        {
            get { return TwitterUrl + ScreenName; }
        }
    }
}