using System;
using System.Collections.Generic;
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Results;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Factiva.Currents.Website.Models;
using DowJones.Formatters;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Website.Mocks
{

	public class MockPageAssetsProvider : IPageAssetProvider
	{

		#region Implementation of IPageAssetProvider

		public PageServiceResponse GetPageByName(string pageName)
		{
			return GetPageById("");
		}

		public PageServiceResponse GetPageById(string pageId)
		{
			return new PageServiceResponse
			{
				CustomTopicsNewsPageModuleServiceResult = new CustomTopicsNewsPageModuleServiceResult
				{
					PartResults = new[]
								{
									new CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>
										{
											Package = new CustomTopicsPackage
												{
													Result = GetStubHeadlines().Result
												}
										},
								}
				}
			};

		}

		public IEnumerable<PageListModel> GetPages()
		{
			throw new NotImplementedException();
		}

		#endregion


		private PortalHeadlineListModel GetStubHeadlines()
		{
			return new PortalHeadlineListModel
			{
				Result = new PortalHeadlineListDataResult
				{
					ResultSet = new PortalHeadlineListResultSet
					{
						Headlines = new List<PortalHeadlineInfo>
						{
							new PortalHeadlineInfo
							{
								Authors = new AuthorCollection(new []{ "By Carolyn Cui" }),
								BaseLanguage = "en",
								BaseLanguageDescriptor = "English",
								CodedAuthors = new List<Para>
								{
									new Para
									{
										items = new List<MarkupItem>
										{
											new MarkupItem
											{
												EntityType  = EntityType.Textual,
												value = "By Carolyn Cui"
											}
										}
									}
								},
								ContentCategoryDescriptor = "publication",
								ContentSubCategoryDescriptor = "article",
								HasPublicationTime = false,
								ModificationDateDescriptor = "21 June 2012",
								ModificationDateTime = new DateTime(1340280969000),
								ModificationDateTimeDescriptor = "21 June 2012 12:16 GMT",
								ModificationTimeDescriptor = "12:16 GMT",
								PublicationDateDescriptor = null,
								PublicationDateTime = new DateTime(1340236800000),
								PublicationDateTimeDescriptor = "21 June 2012",
								PublicationTimeDescriptor = null,
								Reference = new Reference
								{
									contentCategory = ContentCategory.Publication,
									contentCategoryDescriptor = "publication",
									contentSubCategory = ContentSubCategory.Article,
									contentSubCategoryDescriptor = "article",
									guid = "J000000020120621e86l0003f",
									mimetype = "text/xml",
									type = "accessionNo"
								},
								Snippets = new SnippetCollection(new [] {"Europe and Syria took center stage at the Group of 20 meeting in Los Cabos, Mexico, this week. But in the oil industry, all eyes were on a report that signaled regulators are backing away from efforts to ratchet up scrutiny of the $2 ..."}),
								SourceCode = "j",
								SourceDescriptor = "The Wall Street Journal",
								ThumbnailImage = new ThumbnailImage(),
								Title = "Regulators Back Off Tougher Curbs on Oil ",
								TruncatedTitle = "Regulators Back Off Tougher Curbs on Oil",
								WordCount = new WholeNumber(962),
								WordCountDescriptor = "962 words"
							}
						}
					}
				}
			};
		}

	}
}