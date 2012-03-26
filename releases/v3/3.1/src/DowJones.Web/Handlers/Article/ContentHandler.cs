using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using DowJones.DTO.Web;
using DowJones.DTO.Web.Request;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Requests;
using DowJones.Session;
using EMG.Gateway.Services.V1_0;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Web.Handlers.Article
{
    public class ImageCacheItem
    {
        public byte[] bytes { get; set; }       
        public string mimeType { get; set; }
    }

    public class ContentHandler : BaseHttpHandler
    {
        private readonly string contentMimeType;
        private const int ErrorImageWidth = 150;
        private const int ErrorImageHeight = 150;
        private const string keyformat = "{0}::{1}";
        private const int slidingCache = 12;
        private ControlData controlData;

        public ContentHandler(string contentMimeType)
        {
            this.contentMimeType = contentMimeType;
        }

        public override void HandleRequest(HttpContext context)
        {
           try
           {
                string origAccessionNo;
                var accessionNo = origAccessionNo = context.Request["accessno"] ?? string.Empty;
                if (string.IsNullOrEmpty(accessionNo))
                {
                    context.Response.StatusCode = ( int ) HttpStatusCode.BadRequest;
                    return;
                }

                var isBlob = context.Request["isblob"] ?? string.Empty;
                var reference = context.Request[ "reference" ] ?? string.Empty;     
                var imageType = context.Request[ "imageType" ] ?? string.Empty;
                var mimeType = context.Request[ "mimetype" ] ?? string.Empty;  
                var formState = new FormState( string.Empty );
                var sessionRequestDto = ( SessionRequestDTO ) formState.Accept( typeof( SessionRequestDTO ), false );
                var retrieveBlobItem = isBlob.ToLowerInvariant() == "y";

                controlData = (sessionRequestDto.SessionID.IsNullOrEmpty() && sessionRequestDto.EncryptedToken.IsNullOrEmpty())
                                  ? Factiva.Gateway.Managers.ControlDataManager.GetLightWeightUserControlData( sessionRequestDto.UserID, sessionRequestDto.Password, sessionRequestDto.ProductID )
                                  : sessionRequestDto.GetControlData();

                if( retrieveBlobItem )
                {
                    // Check cache
                    var cacheItem = ( ImageCacheItem ) context.Cache.Get( string.Format( keyformat, origAccessionNo, imageType.ToLowerInvariant() ) );

                    if (cacheItem != null)
                    {
                        context.Response.ContentType = cacheItem.mimeType;
                        context.Response.BinaryWrite(cacheItem.bytes);
                        return;
                    }

                    var infrsControlData = ControlDataManager.Convert(controlData);
                    var sm = new SearchManager( infrsControlData, new Preferences.Preferences( "en" ) );
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
                    dto.SearchCollectionCollection.AddRange( Enum.GetValues( typeof( SearchCollection ) ).Cast<SearchCollection>() );

                    var sr = sm.GetPerformContentSearchResponse<PerformContentSearchRequest, PerformContentSearchResponse>( dto );
                    if( sr != null &&
                           sr.ContentSearchResult != null &&
                           sr.ContentSearchResult.ContentHeadlineResultSet != null )
                    {
                        var temp = sr.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection;
                        if(temp.Count > 0)
                        {
                            var contentHeadline = temp.First();
                            var item = GetThumbNailItem(contentHeadline, imageType );
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
                

                var request = new GetBinaryRequest
                                    {
                                        accessionNumber = accessionNo,
                                        reference = reference,
                                        mimeType = mimeType,
                                        imageType = imageType
                                    };

                ServiceResponse archiveResponse = ArchiveService.GetBinary(controlData, request);
                object objResponse;
                archiveResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);

                var binaryResponse = (GetBinaryResponse)objResponse;
                try
                {
                    switch (mimeType)
                    {
                        case "image/gif":
                        case "image/jpeg":
                        case "image/png":
                            //HandleContent(context.Response, tempErrorNum, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
                            context.Response.ContentType = mimeType;
                            context.Response.BinaryWrite(binaryResponse.binaryData);
                            // add the item to cache to free up the space
                            if (retrieveBlobItem)
                            {
                                context.Cache.Add(
                                    string.Format(keyformat, origAccessionNo, imageType.ToLowerInvariant()),
                                    new ImageCacheItem
                                        {
                                            bytes = binaryResponse.binaryData,
                                            mimeType = mimeType,
                                        },
                                    null, 
                                    Cache.NoAbsoluteExpiration, 
                                    TimeSpan.FromHours(slidingCache), 
                                    CacheItemPriority.Normal, 
                                    null);
                            }
                            break;
                        case "application/msexcel":
                        case "application/msword":
                        case "application/mspowerpoint":
                        case "application/pdf":
                        case "text/html":
                            HandleContent(context.Response, mimeType,binaryResponse.binaryData);
                            break;
                        default:
                            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                                   
                            break;
                    }
                }
                catch (DowJonesUtilitiesException ex)
                {
                    HandleErrorImage(context.Response, ex.ReturnCode, "image/png", ImageFormat.Png, ErrorImageWidth, ErrorImageHeight);
                }
                catch (Exception)
                {
                    HandleErrorImage(context.Response, -1, "image/png", ImageFormat.Png, ErrorImageWidth, ErrorImageHeight);
                }
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            finally
            {
                context.Response.End();
            }
        }

        public ContentItem GetThumbNailItem(ContentHeadline contentHeadline, string imageType)
        {
            if (contentHeadline.ContentItems.ItemCollection == null || contentHeadline.ContentItems.ItemCollection.Count == 0)
            {
                return null;
            }
            return contentHeadline.ContentItems.ItemCollection.Where(item => (item.Mimetype.IsNotEmpty() && item.Ref.IsNotEmpty() && item.Type.ToLower() == imageType)).FirstOrDefault();
        }

        private static void HandleErrorImage(HttpResponse response, long errorNum, string mimeType, ImageFormat imageFormat, int width, int height)
        {
            response.Clear();

            using (var bmp = new Bitmap(width, height))
            {
                try
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        var errorMessageRect = new Rectangle(5, 30, width - 10, 40);
                        var errorCodeRect = new Rectangle(5, height - 18, width - 10, 15);
                        g.SmoothingMode = SmoothingMode.Default;
                        g.Clear(Color.FromArgb(240, 240, 240));
                        g.DrawRectangle(Pens.White, 1, 1, width - 3, height - 3);
                        g.DrawRectangle(Pens.Gray, 2, 2, width - 3, height - 3);
                        g.DrawRectangle(Pens.Black, 0, 0, width, height);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), 3, 3, width - 6, 20);
                        g.DrawString(Resources.GetString("error"), new Font("Arial", 9, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 255)), new PointF(5, 5));
                        g.DrawString(Resources.GetErrorMessage(errorNum.ToString()), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)), errorMessageRect);
                        g.DrawString(errorNum.ToString(), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)), errorCodeRect);
                    }
                }
                catch (Exception)
                {
                    response.StatusCode = ( int ) HttpStatusCode.InternalServerError;
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
            
            
            response.ContentType  = GetContentType(mimeType);
            var contentDisposition = GetContentDisposition(mimeType);
            if (!String.IsNullOrEmpty(contentDisposition))
            {
                response.AddHeader("Content-Disposition", contentDisposition);
                response.AddHeader("Content-Length", binaryData.Length.ToString());
            }
           
            response.BinaryWrite(binaryData);
            response.Flush();
        }
        public override bool ValidateParameters(HttpContext context)
        {
            return true;
        }

        public override bool RequiresAuthentication
        {
            get { return false; }
        }

        public override string ContentMimeType
        {
            get { return contentMimeType;}
        }

        private static string GetContentType(string mimeType)
        {
            if (!String.IsNullOrEmpty(mimeType))
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
            if (!String.IsNullOrEmpty(mimeType))
            {
                switch (mimeType)
                {
                    case "application/msexcel":
                        return "attachment;filename=" +  GetRandomString() + ".xls";
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
           var random = new Random(100);
           var rnd = random.Next();
           return "Temp{" + rnd + DateTime.Now.ToString("MMddyyyyhhmmss") + "}";
        }
    }
}
