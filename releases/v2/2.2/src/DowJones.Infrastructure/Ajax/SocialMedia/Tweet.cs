using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DowJones.Infrastructure.Models.SocialMedia;
using DowJones.Managers.SocialMedia.Twitter.Extensions;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DowJones.Ajax.SocialMedia
{
    [DataContract(Name = "tweet", Namespace = "")]
    public class Tweet
    {
        [DataMember(Name = "user")]
        public User User { get; set; }

        [DataMember(Name = "createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [DataMember(Name = "isoTimeStamp")]
        public string IsoTimeStamp
        {
            get { return CreatedDateTime.ToUniversalTime().ToString("o"); }
            set {  /* Make stupid DataContractSerializer happy */ }
        }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Text with hashtags, entities etc mapped as HTML links.
        /// </summary>
        [DataMember(Name = "html")]
        public string Html
        {
            get { return ParseTwitterageToHtml(Text); }
            set {  /* Make stupid DataContractSerializer happy */ }

        }

    	private string ParseTwitterageToHtml(string text)
    	{
    		

			// at least the tweet will be displayed correctly. 
			// only ODS tracking will miss out values.
    		if(Entities == null || !Entities.Any())
    		{
    			return text.ParseTwitterageToHtml();
    		}

			var html = text;

    		foreach (var url in Entities.Urls)
    		{
				// account for case insensitity 
    			var value = text.Substring(url.StartIndex, url.EndIndex - url.StartIndex);
				html = text.Replace(value, string.Format("<a href=\"{1}\" target=\"_blank\" data-expanded-url=\"{2}\" rel=\"nofollow\">{0}</a>", url.DisplayValue, url.Value, url.ExpandedValue));
    		}


    		foreach (var mention in Entities.Mentions)
    		{
				// account for case insensitity and @ sign
				var value = text.Substring(mention.StartIndex+1, mention.EndIndex - ( mention.StartIndex + 1));
				html = html.Replace("@" + value,
					string.Format("@<a href=\"http://twitter.com/{0}\" data-id=\"{1}\" data-screen-name=\"{0}\" rel=\"nofollow\">{0}</a>",
									value, mention.Id));
    		}

			
			foreach (var hashtag in Entities.HashTags)
			{
				// account for case insensitity 
				var value = text.Substring(hashtag.StartIndex, hashtag.EndIndex - hashtag.StartIndex);
				html = html.Replace(value,
					string.Format("<a href=\"http://twitter.com/search?q=%23{1}\" title=\"{0}\" rel=\"nofollow\">{0}</a>",
									value, hashtag.Text));
			}

    		return html;

    	}

    	[DataMember(Name = "source")]
        public string Source { get; set; }


        [DataMember(Name = "sourceText")]
        public string SourceText
        {
            get
            {
                var match = Regex.Match(Source, @">([^<]+)<");
                return match.Success ? match.Groups[1].Value : string.Empty;
            }
            set {  /* Make stupid DataContractSerializer happy */ }
        }

		public TwitterEntities Entities { get; set; }

    }
}
