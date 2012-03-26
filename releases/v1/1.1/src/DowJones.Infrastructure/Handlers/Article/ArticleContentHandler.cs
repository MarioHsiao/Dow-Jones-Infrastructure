using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using EMG.Gateway.Services.V1_0;
using EMG.Tools.Session;

using EMG.Utility.Exceptions;
using EMG.Utility.Loggers;
using EMG.Utility.Managers;
using EMG.Utility.Managers.Core;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.V1_0;


namespace EMG.Utility.Handlers.ArticleHandler
{
    class ArticleContentHandler : BaseHttpHandler
    {
        private string _ContentMimeType;
        private const int ERROR_IMAGE_WIDTH = 150;
        private const int ERROR_IMAGE_HEIGHT = 150;
        public override void HandleRequest(System.Web.HttpContext context)
        {
           try
           {
            string accessionNo = context.Request["accessno"] ?? string.Empty;
            if (string.IsNullOrEmpty(accessionNo))
                return;
            string reference = context.Request["reference"] ?? string.Empty;
            string mimeType = context.Request["mimetype"] ?? string.Empty;
            string imageType = context.Request["imageType"] ?? string.Empty;
            GetBinaryRequest request = new GetBinaryRequest();
            request.accessionNumber = accessionNo;
            request.reference = reference;
            request.mimeType = mimeType;
            request.imageType = imageType;

            ServiceResponse archiveResponse = ArchiveService.GetBinary(ControlDataManager.Clone(SessionData.Instance().SessionBasedControlData), request);
            object objResponse;
            archiveResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);

            GetBinaryResponse binaryResponse = (GetBinaryResponse)objResponse;

             Encoding encoding = new UTF8Encoding();
                    try
                    {
                        string tempStr = encoding.GetString(binaryResponse.binaryData, 0, 8);
                        long tempErrorNum = -1;
                        try
                        {
                            tempErrorNum = Convert.ToInt32(tempStr);
                        }
                        catch
                        {
                        }

                        if (tempErrorNum == -1)
                        {
                            context.Response.ContentType = mimeType;
                            context.Response.BinaryWrite(binaryResponse.binaryData);
                        }
                        else
                        {
                            switch (mimeType)
                            {
                                case "image/gif":
                                case "image/jpeg":
                                case "image/png":
                                    HandleContent(context.Response, tempErrorNum, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
                                    break;
                                case "application/msexcel":
                                case "application/msword":
                                case "application/mspowerpoint":
                                case "application/pdf":
                                    HandleContent(context.Response, tempErrorNum, mimeType,null, 0,0);
                                    break;
                                default:
                                   context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                                   
                                   break;
                            }
                        }
                    }
                    catch (EmgUtilitiesException ex)
                    {
                      //  HandleErrorImage(context.Response, ex.ReturnCode, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
                    }
                    catch (Exception)
                    {
                       // HandleErrorImage(context.Response, -1, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
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
        private static void HandleContent(HttpResponse response, long errorNum, string mimeType, ImageFormat imageFormat, int width, int height)
        {
            response.Clear();
            MemoryStream memoryStream = new MemoryStream(); 
            string contentType = string.Empty;
            switch (mimeType)
            {
                case "application/msexcel":
                    contentType = "application/vnd.ms-excel";
                    break;
                case "application/msword":
                    contentType = "application/msword";
                    break;
                case "application/pdf":
                    contentType = "application/pdf";
                    break;
                case "application/mspowerpoint":
                    contentType = "application/vnd.ms-powerpoint";
                    break;
                case "image/gif":
                case "image/jpeg":
                case "image/png":
                    contentType = mimeType;
                    using (Bitmap bmp = new Bitmap(width, height))
                    {
                        try
                        {
                            using (Graphics g = Graphics.FromImage(bmp))
                            {
                                Rectangle errorMessageRect = new Rectangle(5, 30, width - 10, 40);
                                Rectangle errorCodeRect = new Rectangle(5, height - 18, width - 10, 15);
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
                        }
                        bmp.Save(memoryStream, imageFormat);

                    }
                    break;
            }
                response.ContentType = contentType;
                
                memoryStream.WriteTo(response.OutputStream);
                memoryStream.Flush();
                memoryStream.Close();
        }
        public override bool ValidateParameters(System.Web.HttpContext context)
        {
            return true;
        }

        public override bool RequiresAuthentication
        {
            get { return false; }
        }

        public override string ContentMimeType
        {
            get { return _ContentMimeType; }
        }
    }
}
