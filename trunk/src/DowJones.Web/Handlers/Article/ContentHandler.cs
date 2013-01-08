using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using DowJones.Articles;
using DowJones.DTO.Web;
using DowJones.DTO.Web.Request;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Requests;
using DowJones.Session;
using Factiva.Gateway.Messages.Archive;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using ControlDataManager = Factiva.Gateway.Managers.ControlDataManager;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Web.Handlers.Article
{
    public class ImageCacheItem
    {
        public byte[] Bytes { get; set; }
        public string MimeType { get; set; }
    }

    /// <summary>
    /// </summary>
    public class ContentHandler : Page, IHttpAsyncHandler
    {
        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extradata)
        {
            return AspCompatBeginProcessRequest(context, cb, extradata);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            AspCompatEndProcessRequest(result);
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            // Request Hosting permissions
            new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();

            Type handlerType = typeof (BaseContentHandler);
            IHttpHandler handler;
            try
            {
                // Create the handler by calling class abc or class xyz.
                handler = (IHttpHandler) Activator.CreateInstance(handlerType, true);
            }
            catch (Exception ex)
            {
                throw new HttpException("Unable to create handler", ex);
            }

            handler.ProcessRequest(Context);
            Context.ApplicationInstance.CompleteRequest();
        }
    }

    public class BaseContentHandler : BaseHttpHandler
    {
        private const int ErrorImageWidth = 150;
        private const int ErrorImageHeight = 150;
        private const string Keyformat = "{0}::{1}";
        private const int SlidingCache = 12;
        private readonly string _contentMimeType;
        private ControlData _controlData;

        public BaseContentHandler()
        {
        }

        public BaseContentHandler(string contentMimeType)
        {
            _contentMimeType = contentMimeType;
        }

        public override bool RequiresAuthentication
        {
            get { return false; }
        }

        public override string ContentMimeType
        {
            get { return _contentMimeType; }
        }

        public override void HandleRequest(HttpContext context)
        {
            try
            {
                string origAccessionNo;
                string accessionNo = origAccessionNo = context.Request["accessno"] ?? string.Empty;
                if (string.IsNullOrEmpty(accessionNo))
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return;
                }

                string isBlob = context.Request["isblob"] ?? string.Empty;
                string reference = context.Request["reference"] ?? string.Empty;
                string imageType = context.Request["imageType"] ?? string.Empty;
                string mimeType = context.Request["mimetype"] ?? string.Empty;
                string redirect = context.Request["redirect"] ?? string.Empty;

                var formState = new FormState(string.Empty);
                var sessionRequestDto = (SessionRequestDTO) formState.Accept(typeof (SessionRequestDTO), false);
                bool retrieveBlobItem = isBlob.ToLowerInvariant() == "y";
                bool redirectToWebSiteUrl = redirect.ToLowerInvariant() == "y";

                _controlData = (sessionRequestDto.SessionID.IsNullOrEmpty() &&
                                sessionRequestDto.EncryptedToken.IsNullOrEmpty())
                                   ? ControlDataManager.GetLightWeightUserControlData(sessionRequestDto.UserID,
                                                                                      sessionRequestDto.Password,
                                                                                      sessionRequestDto.ProductID)
                                   : sessionRequestDto.GetControlData();

                IControlData infrsControlData = DowJones.Session.ControlDataManager.Convert(_controlData);
                if (redirectToWebSiteUrl)
                {
                    var service = new ArticleService(infrsControlData, new Preferences.Preferences("en"));
                    string webArticeUrl = service.GetWebArticleUrl(accessionNo);

                    context.Response.Buffer = true;
                    //context.Response.Status = "302 Object moved";
                    context.Response.AddHeader("Location", webArticeUrl);
                    context.Response.Write("<HTML><Head>");
                    context.Response.Write("<META HTTP-EQUIV=Refresh CONTENT=\"0;URL=" + webArticeUrl + "\">");
                    context.Response.Write("<Script>window.location='" + webArticeUrl + "';</Script>");
                    context.Response.Write("</Head></HTML>");
                    context.Response.Flush();
                    context.Response.End();
                    return;
                }

                #region RetrieveBlobItem

                if (retrieveBlobItem)
                {
                    // Check cache
                    var cacheItem =
                        (ImageCacheItem)
                        context.Cache.Get(string.Format(Keyformat, origAccessionNo, imageType.ToLowerInvariant()));

                    if (cacheItem != null)
                    {
                        context.Response.ContentType = cacheItem.MimeType;
                        context.Response.BinaryWrite(cacheItem.Bytes);
                        return;
                    }

                    var sm = new SearchManager(infrsControlData, new Preferences.Preferences("en"));
                    var dto = new AccessionNumberSearchRequestDTO
                                  {
                                      SortBy = SortBy.FIFO,
                                      MetaDataController =
                                          {
                                              Mode = CodeNavigatorMode.None
                                          },
                                      DescriptorControl =
                                          {
                                              Mode = DescriptorControlMode.None,
                                              Language = "en"
                                          },
                                      AccessionNumbers = new[] {reference},
                                  };
                    dto.MetaDataController.ReturnCollectionCounts = false;
                    dto.MetaDataController.ReturnKeywordsSet = false;
                    dto.MetaDataController.TimeNavigatorMode = TimeNavigatorMode.None;
                    dto.SearchCollectionCollection.AddRange(
                        Enum.GetValues(typeof (SearchCollection)).Cast<SearchCollection>());

                    IPerformContentSearchResponse sr =
                        sm.GetPerformContentSearchResponse<PerformContentSearchRequest, PerformContentSearchResponse>(
                            dto);
                    if (sr != null &&
                        sr.ContentSearchResult != null &&
                        sr.ContentSearchResult.ContentHeadlineResultSet != null)
                    {
                        ContentHeadlineCollection temp =
                            sr.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection;
                        if (temp.Count > 0)
                        {
                            ContentHeadline contentHeadline = temp.First();
                            ContentItem item = GetThumbNailItem(contentHeadline, imageType);
                            if (item != null)
                            {
                                accessionNo = reference;
                                reference = item.Ref;
                                mimeType = item.Mimetype;
                            }
                            else
                            {
                                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                            }
                        }
                    }
                }

                #endregion

                if (imageType.ToLower() == "tnail" ||
                    imageType.ToLower() == "fnail")
                {
                    HandleRequest<GetBinaryInternalRequest, GetBinaryInternalResponse>(new GetBinaryInternalRequest
                                                                                           {
                                                                                               accessionNumber =
                                                                                                   accessionNo,
                                                                                               reference = reference,
                                                                                               mimeType = mimeType,
                                                                                               imageType = imageType
                                                                                           },
                                                                                       context,
                                                                                       retrieveBlobItem,
                                                                                       origAccessionNo);
                }

                else
                {
                    HandleRequest<GetBinaryRequest, GetBinaryResponse>(new GetBinaryRequest
                                                                           {
                                                                               accessionNumber = accessionNo,
                                                                               reference = reference,
                                                                               mimeType = mimeType,
                                                                               imageType = imageType
                                                                           },
                                                                       context,
                                                                       retrieveBlobItem,
                                                                       origAccessionNo);
                }
            }
            catch
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }
            finally
            {
                context.Response.End();
            }
        }

        private void HandleRequest<TRequest, TResponse>(TRequest request, HttpContext context, bool retrieveBlobItem,
                                                        string origAccessionNo)
            where TRequest : IBinaryRequest, new()
            where TResponse : IBinaryResponse, new()
        {
            ServiceResponse<TResponse> gatewayResponse = FactivaServices.Invoke<TResponse>(_controlData, request);

            object objResponse;
            gatewayResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);

            var response = (IBinaryResponse) objResponse;

            try
            {
                if (gatewayResponse.ReturnCode != 0)
                {
                    throw new DowJonesUtilitiesException(gatewayResponse.ReturnCode);
                }

                byte[] binaryData = (response is GetBinaryResponse)
                                        ? ((GetBinaryResponse) response).binaryData
                                        : ((GetBinaryInternalResponse) response).binaryData;

                switch (request.mimeType)
                {
                    case "image/gif":
                    case "image/jpeg":
                    case "image/png":
                        //HandleContent(context.Response, tempErrorNum, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
                        context.Response.ContentType = request.mimeType;
                        context.Response.BinaryWrite(binaryData);
                        // add the item to cache to free up the space
                        if (retrieveBlobItem)
                        {
                            context.Cache.Add(
                                string.Format(Keyformat, origAccessionNo, request.imageType.ToLowerInvariant()),
                                new ImageCacheItem
                                    {
                                        Bytes = binaryData,
                                        MimeType = request.mimeType,
                                    },
                                null,
                                Cache.NoAbsoluteExpiration,
                                TimeSpan.FromHours(SlidingCache),
                                CacheItemPriority.Normal,
                                null);
                        }
                        break;
                    case "application/msexcel":
                    case "application/msword":
                    case "application/mspowerpoint":
                    case "application/pdf":
                    case "text/html":
                        HandleContent(context.Response, request.mimeType, binaryData);
                        break;
                    default:
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (DowJonesUtilitiesException ex)
            {
                HandleErrorImage(context.Response, ex.ReturnCode, "image/png", ImageFormat.Png, ErrorImageWidth,
                                 ErrorImageHeight);
            }
            catch (Exception)
            {
                HandleErrorImage(context.Response, -1, "image/png", ImageFormat.Png, ErrorImageWidth, ErrorImageHeight);
            }
        }

        public ContentItem GetThumbNailItem(ContentHeadline contentHeadline, string imageType)
        {
            if (contentHeadline.ContentItems.ItemCollection == null ||
                contentHeadline.ContentItems.ItemCollection.Count == 0)
            {
                return null;
            }
            return
                contentHeadline.ContentItems.ItemCollection.FirstOrDefault(
                    item => (item.Mimetype.IsNotEmpty() && item.Ref.IsNotEmpty() && item.Type.ToLower() == imageType));
        }

        private static void HandleErrorImage(HttpResponse response, long errorNum, string mimeType,
                                             ImageFormat imageFormat, int width, int height)
        {
            response.Clear();

            using (var bmp = new Bitmap(width, height))
            {
                try
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        var errorMessageRect = new Rectangle(5, 30, width - 10, 40);
                        var errorCodeRect = new Rectangle(5, height - 18, width - 10, 15);
                        g.SmoothingMode = SmoothingMode.Default;
                        g.Clear(Color.FromArgb(240, 240, 240));
                        g.DrawRectangle(Pens.White, 1, 1, width - 3, height - 3);
                        g.DrawRectangle(Pens.Gray, 2, 2, width - 3, height - 3);
                        g.DrawRectangle(Pens.Black, 0, 0, width, height);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), 3, 3, width - 6, 20);
                        g.DrawString(Resources.GetString("error"), new Font("Arial", 9, FontStyle.Bold),
                                     new SolidBrush(Color.FromArgb(255, 255, 255)), new PointF(5, 5));
                        g.DrawString(Resources.GetErrorMessage(errorNum.ToString(CultureInfo.InvariantCulture)),
                                     new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)),
                                     errorMessageRect);
                        g.DrawString(errorNum.ToString(CultureInfo.InvariantCulture),
                                     new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)),
                                     errorCodeRect);
                    }
                }
                catch (Exception)
                {
                    response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }

                var memoryStream = new MemoryStream();
                response.ContentType = mimeType;
                bmp.Save(memoryStream, imageFormat);
                memoryStream.WriteTo(response.OutputStream);
                memoryStream.Flush();
                memoryStream.Close();
            }
        }

        private static void HandleContent(HttpResponse response, string mimeType, byte[] binaryData)
        {
            response.ClearHeaders();
            response.ClearContent();


            response.ContentType = GetContentType(mimeType);
            string contentDisposition = GetContentDisposition(mimeType);
            if (!String.IsNullOrEmpty(contentDisposition))
            {
                response.AddHeader("Content-Disposition", contentDisposition);
                response.AddHeader("Content-Length", binaryData.Length.ToString(CultureInfo.InvariantCulture));
            }

            response.BinaryWrite(binaryData);
            response.Flush();
        }

        public override bool ValidateParameters(HttpContext context)
        {
            return true;
        }

        private static string GetContentType(string mimeType)
        {
            if (!mimeType.IsNullOrEmpty())
            {
                switch (mimeType)
                {
                    case "application/msexcel":
                        return "application/vnd.ms-excel";
                    case "application/msword":
                        return "application/msword";
                    case "application/mspowerpoint":
                        return "application/vnd.ms-powerpoint";
                    case "application/pdf":
                        return "application/pdf";
                    default:
                        return mimeType;
                }
            }

            return null;
        }

        private static string GetContentDisposition(string mimeType)
        {
            if (!mimeType.IsNullOrEmpty())
            {
                switch (mimeType)
                {
                    case "application/msexcel":
                        return "attachment;filename=" + GetRandomString() + ".xls";
                    case "application/msword":
                        return "attachment;filename=" + GetRandomString() + ".doc";
                    case "application/mspowerpoint":
                        return "attachment;filename=" + GetRandomString() + ".ppt";
                    case "application/pdf":
                        return "attachment;filename=" + GetRandomString() + ".pdf";
                }
            }
            return null;
        }

        private static string GetRandomString()
        {
            return "Temp{" + new Random(100).Next() + DateTime.Now.ToString("MMddyyyyhhmmss") + "}";
        }
    }
}