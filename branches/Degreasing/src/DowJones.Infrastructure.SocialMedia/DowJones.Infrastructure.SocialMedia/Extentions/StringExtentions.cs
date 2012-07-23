// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtentions.cs" company="Dow Jones & Co">
//   
// </copyright>
// <summary>
//   The string extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using DowJones.Infrastructure.SocialMedia.Models;

namespace DowJones.Infrastructure.SocialMedia.Extentions
{
    /// <summary>
    /// The string extensions.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// The options.
        /// </summary>
        private const RegexOptions Options = RegexOptions.Compiled | RegexOptions.IgnoreCase;

        /// <summary>             
        /// The parse urls. Uses Jon Gruber's URL Regex: http://daringfireball.net/2009/11/liberal_regex_for_matching_urls
        /// </summary>
        private static readonly Regex ParseUrls =
            //// new Regex(@"\b(([\w-]+://?|www[.])[^\s()<>]+(?:\([\w\d]+\)|([^\p{P}\s]|/)))", Options);
            new Regex(
                @"(?i)\b((?:https?://|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))",
                Options);                    

        /// <summary>
        /// The parse mentions. Diego Sevilla's @ Regex: http://stackoverflow.com/questions/529965/how-could-i-combine-these-regex-rules
        /// </summary>
        private static readonly Regex ParseMentions = new Regex(@"(^|\W)@([A-Za-z0-9_]+)", Options);

        /// <summary>
        /// The parse hash-tags.  Simon Whatley's # Regex: http://www.simonwhatley.co.uk/parsing-twitter-usernames-hashtags-and-urls-with-javascript
        /// </summary>
        private static readonly Regex ParseHashtags = new Regex("[#]+[A-Za-z0-9-_]+", Options);

        /// <summary>
        /// The is null or blank.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if [is null or blank] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrBlank(this string input)
        {
            return string.IsNullOrEmpty(input) || input.Trim().Length == 0;
        }

        /// <summary>
        /// The are null or blank.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        ///   <c>true</c> if [is null or blank] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool AreNullOrBlank(this IEnumerable<string> values)
        {
            if (values.Count() == 0 || values == null)
            {
                return false;
            }

            return values.Aggregate(true, (current, value) => current & value.IsNullOrBlank());
        }

        /// <summary>
        /// The parse twitter-age to entities.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>An object of <see cref="TwitterEntities"/></returns>
        public static TwitterEntities ParseTwitterageToEntities(this string text)
        {
            var entities = new TwitterEntities
                {
                    HashTags = new List<TwitterHashTag>(ParseTwitterageToHashTags(text)), 
                    Mentions = new List<TwitterMention>(ParseTwitterageToMentions(text)), 
                    Urls = new List<TwitterUrl>(ParseTwitterageToUrls(text))
                };

            return entities;
        }

        /// <summary>
        /// Parses the twitter-age to parts.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A list of TweetParts</returns>
        public static TweetParts ParseTwitterageToParts(this string input)
        {
            var tempTweetParts = new TweetParts();
            if (input.IsNullOrBlank())
            {
                return tempTweetParts;
            }

            foreach (Match match in ParseUrls.Matches(input))
            {
                input = input.Replace(
                    match.Value,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "^^->:^^:u {0} --:-- {1}",
                        match.Value,
                        match.Value.ToLower().StartsWith("http://") || match.Value.ToLower().StartsWith("https://")
                            ? match.Value
                            : string.Concat("http://", match.Value)));
            }

            foreach (Match match in ParseMentions.Matches(input))
            {
                if (match.Groups.Count != 3)
                {
                    continue;
                }

                var screenName = match.Groups[2].Value;
                var mention = "@" + screenName;

                input = input.Replace(
                    mention,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "^^->:^^:m http://twitter.com/{0} --:-- {1}",
                        screenName,
                        mention));
            }

            foreach (Match match in ParseHashtags.Matches(input))
            {
                var hashtag = Uri.EscapeDataString(match.Value);
                input = input.Replace(
                    match.Value,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "^^->:^^:h http://search.twitter.com/search?q={0} --:-- {1}",
                        hashtag,
                        match.Value));
            }

            var tempParts = input.Split(new[] { "^^->:^^" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in tempParts)
            {
                if (s.StartsWith(":u"))
                {
                    var t = GetValues(s.Substring(3).Trim());
                    tempTweetParts.Add(
                        new TweetPart
                            {
                                Href = t[0],
                                Value = t[1],
                                TweetPartType = TweetPartType.EmbeddedUrl,
                            });
                }
                else if (s.StartsWith(":m"))
                {
                    var t = GetValues(s.Substring(3).Trim());
                    tempTweetParts.Add(
                        new TweetPart
                            {
                                Href = t[0], 
                                Value = t[1], 
                                TweetPartType = TweetPartType.Mention,
                            });
                }
                else if (s.StartsWith(":h"))
                {
                    var t = GetValues(s.Substring(3).Trim());
                    tempTweetParts.Add(
                        new TweetPart
                            {
                                Href = t[0],
                                Value = t[1],
                                TweetPartType = TweetPartType.HashTag,
                            });
                }
                else
                {
                    tempTweetParts.Add(
                        new TweetPart 
                            { 
                                Value = s,
                                TweetPartType = TweetPartType.Text,
                            });
                }
            }

            return tempTweetParts;
        }

        /// <summary>
        /// The parse twitter-age to html.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The html that represents the twitter twitter-age.
        /// </returns>
        public static string ParseTwitterageToHtml(this string input)
        {
            if (input.IsNullOrBlank())
            {
                return input;
            }

            foreach (Match match in ParseUrls.Matches(input))
            {
                input = input.Replace(
                    match.Value, 
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        "<a href=\"{0}\" target=\"_blank\">{0}</a>", 
                        match.Value.ToLower().StartsWith("http://") || match.Value.ToLower().StartsWith("https://") ? match.Value : string.Concat("http://", match.Value)));
            }

            foreach (Match match in ParseMentions.Matches(input))
            {
                if (match.Groups.Count != 3)
                {
                    continue;
                }

                var screenName = match.Groups[2].Value;
                var mention = "@" + screenName;

                input = input.Replace(
                    mention,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "<a href=\"http://twitter.com/{0}\" target=\"_blank\">{1}</a>",
                        screenName,
                        mention));
            }

            foreach (Match match in ParseHashtags.Matches(input))
            {
                var hashtag = Uri.EscapeDataString(match.Value);
                input = input.Replace(
                    match.Value, 
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        "<a href=\"http://search.twitter.com/search?q={0}\" target=\"_blank\">{1}</a>", 
                        hashtag, 
                        match.Value));
            }

            return input;
        }

        /// <summary>
        /// The parse twitter-age to urls.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// An IEnumerable of Twitter urls.
        /// </returns>
        public static IEnumerable<TwitterUrl> ParseTwitterageToUrls(this string input)
        {
            if (input.IsNullOrBlank())
            {
                yield break;
            }

            foreach (Match match in ParseUrls.Matches(input))
            {
                var value = match.Value.ToLower().StartsWith("http://") || match.Value.ToLower().StartsWith("https://") ? match.Value : string.Concat("http://", match.Value);
                
                Uri uri;
                try
                {
                    uri = new Uri(value);
                }
                catch (UriFormatException)
                {
                    continue;
                }

                var url = new TwitterUrl
                    {
                        Value = uri.ToString(), 
                        Indices = new List<int>(new[] { match.Index, match.Index + match.Value.Length })
                    };

                if (!match.Value.EndsWith("/") && url.Value.EndsWith("/"))
                {
                    url.Value = url.Value.Substring(0, url.Value.Length - 1);
                }

                yield return url;
            }
        }

        /// <summary>
        /// The parse twitter-age to mentions.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// An IEnumerable of Twitter Mentions.
        /// </returns>
        public static IEnumerable<TwitterMention> ParseTwitterageToMentions(this string input)
        {
            if (input.IsNullOrBlank())
            {
                yield break;
            }

            foreach (var mention in from Match match in ParseMentions.Matches(input)
                                    where match.Groups.Count == 3
                                    let screenName = match.Groups[2].Value
                                    let startIndex = match.Index + (match.Index == 0 ? 0 : 1)
                                    select new TwitterMention
                                        {
                                            ScreenName = screenName, Indices = new[]
                                                {
                                                    startIndex, 
                                                    startIndex + screenName.Length + 1
                                                }
                                        })
            {
                yield return mention;
            }
        }

        /// <summary>
        /// The parse twitter-age to hash tags.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// An IEnumerable of Twitter Hash-tags.
        /// </returns>
        public static IEnumerable<TwitterHashTag> ParseTwitterageToHashTags(this string input)
        {
            if (input.IsNullOrBlank())
            {
                yield break;
            }

            foreach (var hashtag in from Match match in ParseHashtags.Matches(input)
                                    select new TwitterHashTag
                                        {
                                            Text = match.Value.Substring(1), Indices = new[]
                                                {
                                                    match.Index, 
                                                    match.Index + match.Value.Length
                                                }
                                        })
            {
                yield return hashtag;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A list with two parts</returns>
        private static List<string> GetValues(string input)
        {
            return new List<string>(input.Split(new[] { " --:-- " }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}