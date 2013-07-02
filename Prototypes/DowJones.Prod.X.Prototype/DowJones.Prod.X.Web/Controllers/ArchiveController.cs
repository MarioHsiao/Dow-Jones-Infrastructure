using System;
using System.Linq;
using System.Web.Mvc;
using DowJones.Ajax;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Requests;
using DowJones.Managers.Search.Responses;
using DowJones.Prod.X.Common.Http;
using DowJones.Prod.X.Core.Interfaces;
using DowJones.Prod.X.Core.Services.Archive;
using DowJones.Prod.X.Models.Archive;
using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Models.Site.Archive;
using DowJones.Prod.X.Web.Controllers.Base;
using DowJones.Prod.X.Web.Filters;
using DowJones.Prod.X.Web.Models;
using DowJones.Managers.Multimedia;
using DowJones.Prod.X.Web.Properties;
using DowJones.Url;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.VideoPlayer;
using Factiva.Gateway.Messages.Search.V2_0;
using ContentCategory = DowJones.Ajax.ContentCategory;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Prod.X.Web.Controllers
{
    [RequireAuthentication(Order = 0)]
    public class ArchiveController : BaseController
    {
        private readonly IArticleRetrivalService _archiveRetrivalService;
        private readonly MultimediaManager _multimediaManager;
        private readonly IBrowserDetectionService _browserDetectionService;
        private readonly SearchManager _searchManager;
        private readonly PortalHeadlineConversionManager _portalHeadlineConversionManager;
        private readonly HeadlineListConversionManager _headlineListConversionManager;

        private const string VideoPlayerPath = "~/Content/swf/flowplayer.unlimited-3.2.7.swf";
        private const string VideoRtmpPluginPath = "~/Content/swf/flowplayer.rtmp-3.2.3.swf";
        private const string VideoSplashImagePath = "~/Content/styles/images/play_text_large.png";

        public ArchiveController(IArticleRetrivalService archiveRetrivalService,
                                 IBrowserDetectionService browserDetectionService,
                                 MultimediaManager multimediaManager,
                                 SearchManager searchManager,
                                 PortalHeadlineConversionManager portalHeadlineConversionManager,
                                 HeadlineListConversionManager headlineListConversionManager)
        {
            _archiveRetrivalService = archiveRetrivalService;
            _multimediaManager = multimediaManager;
            _browserDetectionService = browserDetectionService;
            _searchManager = searchManager;
            _portalHeadlineConversionManager = portalHeadlineConversionManager;
            _headlineListConversionManager = headlineListConversionManager;
        }

        public ActionResult Index(string accessionNumber)
        {
            var model = new ArchiveIndexViewModel(BasicSiteRequestDto, ControlData, MainNavigationCategory.Search)
                            {
                                BaseActionModel = GetArticle(accessionNumber)
                            };

            return View("Index", model);
        }

        private AccessionNumberSearchResponse GetHeadlines(string[] accNums)
        {
            var requestDto = new AccessionNumberSearchRequestDTO
                                 {
                                     SortBy = SortBy.FIFO,
                                     MetaDataController =
                                         {
                                             Mode = CodeNavigatorMode.None
                                         },
                                     DescriptorControl =
                                         {
                                             Mode = DescriptorControlMode.None,
                                             Language = "en",
                                         },
                                     AccessionNumbers = accNums
                                 };

            requestDto.MetaDataController.ReturnCollectionCounts = false;
            requestDto.MetaDataController.ReturnKeywordsSet = false;
            requestDto.MetaDataController.TimeNavigatorMode = TimeNavigatorMode.None;

            // add all the search collections to the search.
            requestDto.SearchCollectionCollection.Clear();
            requestDto.SearchCollectionCollection.AddRange(Enum.GetValues(typeof (SearchCollection)).Cast<SearchCollection>());

            return _searchManager.PerformAccessionNumberSearch<PerformContentSearchRequest, PerformContentSearchResponse>(requestDto);
        }

        private ArchiveModel GetArticle(string accessionNumber)
        {
            var model = new ArchiveModel();
            var response = GetHeadlines(new[] {accessionNumber});
            var portalHeadlineResult = _portalHeadlineConversionManager.Map(_headlineListConversionManager.Process(response, true));
            var selectedItem = model.Headline = portalHeadlineResult.ResultSet.Headlines[0];

            if (selectedItem != null)
            {
                var reference = selectedItem.Reference;
                switch (reference.contentCategory)
                {
                    case ContentCategory.Multimedia:
                        var multimediaModel = GetMultimediaModel(selectedItem);
                        if (multimediaModel.MustPlayFromSource)
                        {
                            model.ExternalItemUri = multimediaModel.ExternalUrl;
                        }

                        if (multimediaModel.MultiMediaPlayerModel != null)
                        {
                            model.MultiMediaPlayerModel = multimediaModel.MultiMediaPlayerModel;
                        }


                        break;
                    default:
                        model.ArticleModel = _archiveRetrivalService.GetArticleModel(selectedItem.Reference.guid, new ConvertionOptions
                                                                                                                      {
                                                                                                                          ArticleDisplayOptions = DisplayOptions.Full,
                                                                                                                          EmbededImageType = ImageType.UnSpecified,
                                                                                                                      });
                        break;
                }
            }

            return model;
        }

        private MultiMediaItemModel GetMultimediaModel(PortalHeadlineInfo portalHeadlineInfo)
        {
            var response = _multimediaManager.GetMultiMediaResult(portalHeadlineInfo.Reference.guid, false);
            var multimediaItemModel = new MultiMediaItemModel();

            if (response.Status > 0)
            {
                throw new DowJonesUtilitiesException(response.Status);
            }

            if (response.MultimediaResult != null)
            {
                if (response.MultimediaResult.MustPlayFromSource != null)
                {
                    multimediaItemModel.MustPlayFromSource = response.MultimediaResult.MustPlayFromSource.Status;
                    if (multimediaItemModel.MustPlayFromSource)
                    {
                        multimediaItemModel.ExternalUrl = response.MultimediaResult.MustPlayFromSource.Url;
                    }
                }

                if (response.MultimediaResult.MediaContents != null && response.MultimediaResult.MediaContents.Count > 0)
                {
                    var mediaContents = response.MultimediaResult.MediaContents;
                    var mediaContent = mediaContents.First();
                    if (mediaContents.Count > 1)
                    {
                        mediaContent = mediaContents[1];
                    }

                    switch (portalHeadlineInfo.Reference.contentSubCategory)
                    {
                        case ContentSubCategory.Audio:
                            {
                                var videoPlayerModel = new VideoPlayerModel
                                                           {
                                                               AutoPlay = true,
                                                               Data = new ClipCollection(new[] {Mapper.Map<Clip>(mediaContent)}),
                                                               Width = GetAudioWidth(),
                                                               Height = GetAudioHeight(),
                                                               PlayerKey = Settings.Default.FlowPlayerKey,
                                                               PlayerPath = new UrlBuilder(VideoPlayerPath).ToString(),
                                                               RTMPPluginPath = new UrlBuilder(VideoRtmpPluginPath).ToString(),
                                                               SplashImagePath = new UrlBuilder(VideoSplashImagePath).ToString(),
                                                           };
                                multimediaItemModel.MultiMediaPlayerModel = videoPlayerModel;
                            }
                            break;
                        case ContentSubCategory.Video:
                            {
                                var aspectRatio = GetVideoAspectRatio();
                                var width = (int.Parse(mediaContent.Width)/aspectRatio).ConvertTo<int>();
                                var height = (int.Parse(mediaContent.Height)/aspectRatio).ConvertTo<int>();
                                var videoPlayerModel = new VideoPlayerModel
                                                           {
                                                               AutoPlay = true,
                                                               Width = width,
                                                               Height = height,
                                                               Data = new ClipCollection(new[]
                                                                                             {
                                                                                                 Mapper.Map<Clip>(
                                                                                                     mediaContent)
                                                                                             }),
                                                               PlayerKey = Settings.Default.FlowPlayerKey,
                                                               PlayerPath = new UrlBuilder(VideoPlayerPath).ToString(),
                                                               RTMPPluginPath = new UrlBuilder(VideoRtmpPluginPath).ToString(),
                                                               SplashImagePath = new UrlBuilder(VideoSplashImagePath).ToString(),
                                                           };

                                //videoPlayerModel.PlayList.First().Title = JsonUtility.EncodeStringValue(portalHeadlineInfo.Title);
                                multimediaItemModel.MultiMediaPlayerModel = videoPlayerModel;
                            }
                            break;
                    }
                }
            }
            return multimediaItemModel;
        }

        private static int GetAudioHeight()
        {
            return 30;
        }

        private double GetVideoAspectRatio()
        {
            if (_browserDetectionService.IsTabletSafari)
            {
                return 1.5;
            }

            if (_browserDetectionService.IsMobileAndroid || _browserDetectionService.IsMobileSafari)
            {
                return 2.1;
            }

            return 1.1;
        }

        private int GetAudioWidth()
        {
            if (_browserDetectionService.IsMobileAndroid || _browserDetectionService.IsMobileSafari)
            {
                return 300;
            }

            return 500;
        }
    }
}