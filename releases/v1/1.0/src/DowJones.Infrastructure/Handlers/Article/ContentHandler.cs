using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using DowJones.Utilities.DTO.Web;
using EMG.Gateway.Services.V1_0;
using DowJones.Utilities.DTO.Web.Request;

using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Core;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;


namespace DowJones.Utilities.Handlers.ArticleControl
{
   public class ContentHandler : BaseHttpHandler
    {
        private readonly string contentMimeType;
        private const int ErrorImageWidth = 150;
        private const int ErrorImageHeight = 150;
        //private string accessPointCode;
        //private string productPrefix;
        //private string interfaceLang;
        private ControlData controlData;
        //private string productId;

        public ContentHandler(string contentMimeType)
        {
            this.contentMimeType = contentMimeType;
        }

       //public string AccessPointCode
       // {
       //     get { return accessPointCode; }
       // }

       // public string InterfaceLang
       // {
       //     get { return interfaceLang; }
       // }

       // public string ProductPrefix
       // {
       //     get { return productPrefix; }
       // }


        public override void HandleRequest(HttpContext context)
        {
           try
           {
                var accessionNo = context.Request["accessno"] ?? string.Empty;
                if (string.IsNullOrEmpty(accessionNo))
                {
                    return;
                }

                var reference = context.Request["reference"] ?? string.Empty;
                var mimeType = context.Request["mimetype"] ?? string.Empty;
                var imageType = context.Request["imageType"] ?? string.Empty;
                
                var formState = new FormState(string.Empty);
                var sessionRequestDto = (SessionRequestDTO)formState.Accept(typeof(SessionRequestDTO), false);

                if(string.IsNullOrEmpty(sessionRequestDto.SessionID))
                    controlData = Factiva.Gateway.Managers.ControlDataManager.GetLightWeightUserControlData(sessionRequestDto.UserID, sessionRequestDto.Password, sessionRequestDto.ProductID);
                else
                    controlData = sessionRequestDto.GetControlData();
                var request = new GetBinaryRequest
                                    {
                                        accessionNumber = accessionNo,
                                        reference = reference,
                                        mimeType = mimeType,
                                        imageType = imageType
                                    };

                ServiceResponse archiveResponse = ArchiveService.GetBinary(ControlDataManager.Clone(controlData), request);
                object objResponse;
                archiveResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);

                var binaryResponse = (GetBinaryResponse)objResponse;

                //Encoding encoding = new UTF8Encoding();
                try
                {
                    //var tempStr = encoding.GetString(binaryResponse.binaryData, 0, 8);
                    switch (mimeType)
                    {
                        case "image/gif":
                        case "image/jpeg":
                        case "image/png":
                            //HandleContent(context.Response, tempErrorNum, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
                            context.Response.ContentType = mimeType;
                            context.Response.BinaryWrite(binaryResponse.binaryData);
                            break;
                        case "application/msexcel":
                        case "application/msword":
                        case "application/mspowerpoint":
                        case "application/pdf":
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
        private static void HandleErrorImage(HttpResponse response, long errorNum, string mimeType, ImageFormat imageFormat, int width, int height)
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
                        g.DrawString(ResourceTextManager.Instance.GetString("error"), new Font("Arial", 9, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 255)), new PointF(5, 5));
                        g.DrawString(ResourceTextManager.Instance.GetErrorMessage(errorNum.ToString()), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)), errorMessageRect);
                        g.DrawString(errorNum.ToString(), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)), errorCodeRect);
                    }
                }
                catch (Exception)
                {
                    //TODO handle error scenario here
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
            string contentDisposition = GetContentDisposition(mimeType);
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
           int rnd = random.Next();
           return "Temp{" + rnd + DateTime.Now.ToString("MMddyyyyhhmmss") + "}";
       }

    }
}
