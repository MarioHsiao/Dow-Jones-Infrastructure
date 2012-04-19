using System.Collections.Generic;
using DowJones.Infrastructure.Models.SocialMedia;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tweet = DowJones.Ajax.SocialMedia.Tweet;

namespace DowJones
{


	/// <summary>
	///This is a test class for TweetTest and is intended
	///to contain all TweetTest Unit Tests
	///</summary>
	[TestClass()]
	public class TweetTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}


		/// <summary>
		///A test for Html
		///</summary>
		[TestMethod()]
		public void ShouldParseUrlsInText()
		{
			var target = new Tweet()
							{
								Text = "Credit Unions Quietly Prowling for Bank Acquisitions http://t.co/Y4izB0bN",
								Entities = new TwitterEntities()
											{
												Urls = new List<TwitterUrl>{	
													new TwitterUrl {
														Value = "http://t.co/Y4izB0bN",
														ExpandedValue = "http://shar.es/pg8L0",
														DisplayValue =  "shar.es/pg8L0",
														Indices = new[] {53,73}
													}
												}
											}
							};
			const string Expected = "Credit Unions Quietly Prowling for Bank Acquisitions <a href=\"http://t.co/Y4izB0bN\" target=\"_blank\" data-expanded-url=\"http://shar.es/pg8L0\" title=\"http://shar.es/pg8L0\" rel=\"nofollow\">shar.es/pg8L0</a>";

			Assert.AreEqual(Expected, target.Html);
		}

		/// <summary>
		///A test for Html
		///</summary>
		[TestMethod()]
		public void ShouldParseUserMentionsInText()
		{
			var target = new Tweet()
			{
				Text = "RT @sethtecheditor: A New Era Begins at @AccountingToday, Please Welcome Our New Editor in Chief Daniel Hood",
				Entities = new TwitterEntities()
				{
					Mentions = new List<TwitterMention>{	
													new TwitterMention {
														Name = "seth fineberg",
														Indices = new [] {3, 18},
														ScreenName = "SethTechEditor",
														Id = 235276899
													},
													new TwitterMention {
														Name = "Accounting Today",
														Indices = new [] {40,56},
														ScreenName = "AccountingToday",
														Id = 21792426
													}
												}
				}
			};
			const string Expected = "RT @<a href=\"http://twitter.com/sethtecheditor\" target=\"_blank\" data-id=\"235276899\" data-screen-name=\"sethtecheditor\" rel=\"nofollow\">sethtecheditor</a>: A New Era Begins at @<a href=\"http://twitter.com/AccountingToday\" target=\"_blank\" data-id=\"21792426\" data-screen-name=\"AccountingToday\" rel=\"nofollow\">AccountingToday</a>, Please Welcome Our New Editor in Chief Daniel Hood";

			Assert.AreEqual(Expected, target.Html);
		}

		/// <summary>
		///A test for parsing Urls in Text to Hrefs
		///</summary>
		[TestMethod()]
		public void ShouldParseHashTagsInText()
		{
			var target = new Tweet()
			{
				Text = "Hazell: “What we’re trying to do is trying to install the key risk factors, which are part of the credit rating.” #ERMSummit",
				Entities = new TwitterEntities()
				{
					HashTags = new List<TwitterHashTag>{	
													new TwitterHashTag {
														Text = "ERMSummit",
														Indices = new [] {114, 124}
													}
												}
				}
			};
			const string Expected = "Hazell: “What we’re trying to do is trying to install the key risk factors, which are part of the credit rating.” <a href=\"http://twitter.com/search?q=%23ERMSummit\" title=\"#ERMSummit\" rel=\"nofollow\">#ERMSummit</a>";

			Assert.AreEqual(Expected, target.Html);
		}


		/// <summary>
		///A test for parsing Urls, hash tags, mentions in Text to Hrefs
		///</summary>
		[TestMethod()]
		public void ShouldParseTextToHtml()
		{
			var target = new Tweet()
			{
				Text = "New @BankThink #Homeowners serviced by #Litton were Goldman Sachs' other \"Muppets,\" writes Joel Sucher. $GS #GregSmith http://t.co/AIO3DiiG",
				Entities = new TwitterEntities()
				{
					HashTags = new List<TwitterHashTag>{	
													new TwitterHashTag {
														Text = "Homeowners",
														Indices = new [] {15, 26}
													},
													new TwitterHashTag {
														Text = "Litton",
														Indices = new [] {39, 46}
													},
													new TwitterHashTag {
														Text = "GregSmith",
														Indices = new [] {108, 118}
													}
												},
					Mentions = new List<TwitterMention>{
						new TwitterMention()
						{
							Name = "BankThink",
							Id =  37945091,
							Indices = new [] {4, 14},
							ScreenName = "BankThink"
						}
					},
					Urls = new List<TwitterUrl>() 
					{ 
						new TwitterUrl() 
						{ 
							Value = "http://t.co/AIO3DiiG",
							ExpandedValue = "http://bit.ly/wBlc7j",
							DisplayValue =  "bit.ly/wBlc7j",
							Indices = new[] {119,139}
						} 
					}
				}
			};
			const string Expected = "New @<a href=\"http://twitter.com/BankThink\" target=\"_blank\" data-id=\"37945091\" data-screen-name=\"BankThink\" rel=\"nofollow\">BankThink</a> <a href=\"http://twitter.com/search?q=%23Homeowners\" target=\"_blank\" title=\"#Homeowners\" rel=\"nofollow\">#Homeowners</a> serviced by <a href=\"http://twitter.com/search?q=%23Litton\" target=\"_blank\" title=\"#Litton\" rel=\"nofollow\">#Litton</a> were Goldman Sachs' other \"Muppets,\" writes Joel Sucher. $GS <a href=\"http://twitter.com/search?q=%23GregSmith\" target=\"_blank\" title=\"#GregSmith\" rel=\"nofollow\">#GregSmith</a> <a href=\"http://t.co/AIO3DiiG\" target=\"_blank\" data-expanded-url=\"http://bit.ly/wBlc7j\" title=\"http://bit.ly/wBlc7j\" rel=\"nofollow\">bit.ly/wBlc7j</a>";

			Assert.AreEqual(Expected, target.Html);
		}
	}
}
