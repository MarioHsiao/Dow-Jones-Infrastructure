using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using DowJones.DTO.Web;
using DowJones.DTO.Web.Request;
using DowJones.Exceptions;
using DowJones.Globalization;
using DowJones.Loggers;
using DowJones.Session;
using EMG.Gateway.Services.V1_0;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Web.Handlers.Archive
{
    public class GetFileHandler : BaseHttpHandler
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (GetFileHandler));
        private const int ERROR_IMAGE_WIDTH = 150;
        private const int ERROR_IMAGE_HEIGHT = 150;
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
                        g.DrawString(Resources.GetString("error"), new Font("Arial", 9, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 255)), new PointF(5, 5));
                        g.DrawString(Resources.GetErrorMessage(errorNum.ToString()), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)), errorMessageRect);
                        g.DrawString(errorNum.ToString(), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)), errorCodeRect);
                    }
                }
                catch (Exception)
                {
                }

                var memoryStream = new MemoryStream();
                response.ContentType = mimeType;
                bmp.Save(memoryStream, imageFormat);
                memoryStream.WriteTo(response.OutputStream);
                memoryStream.Flush();
                memoryStream.Close();
            }
        }

        #region Overrides of BaseHttpHandler

        public override bool RequiresAuthentication
        {
            get { return false; }
        }

        public override string ContentMimeType
        {
            get { return "image/jpeg"; }
        }

        public override void HandleRequest(HttpContext context)
        {
            try
            {
                using (new TransactionLogger(logger))
                {
                    var formState = new FormState(string.Empty);
                    var archiveFileRequestDTO = (ArchiveFileRequestDTO) formState.Accept(typeof (ArchiveFileRequestDTO), false);
                    var sessionRequestDTO = (SessionRequestDTO) formState.Accept(typeof (SessionRequestDTO), false);

                    new SessionData(sessionRequestDTO.AccessPointCode, sessionRequestDTO.InterfaceLanguage, 0, true, sessionRequestDTO.ProductPrefix, string.Empty);
                    var accessionNo = context.Request["reference"] ?? string.Empty;
                    if (string.IsNullOrEmpty(accessionNo))
                        return;

                    var request = new GetBinaryRequest
                                      {
                                          accessionNumber = archiveFileRequestDTO.AccessionNumber, 
                                          reference = archiveFileRequestDTO.Reference, mimeType = archiveFileRequestDTO.MimeType, imageType = archiveFileRequestDTO.ImageType
                                      };

                    var archiveResponse = ArchiveService.GetBinary(ControlDataManager.Convert(SessionData.Instance().SessionBasedControlData), request);
                    object objResponse;
                    archiveResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);

                    var binaryResponse = (GetBinaryResponse) objResponse;
                    // binary write

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
                            context.Response.ContentType = archiveFileRequestDTO.MimeType;
                            context.Response.BinaryWrite(binaryResponse.binaryData);
                        }
                        else
                        {
                            switch (archiveFileRequestDTO.MimeType)
                            {
                                case "image/gif":
                                case "image/jpeg":
                                case "image/png":
                                    HandleErrorImage(context.Response, tempErrorNum, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
                                    break;
                                default:
                                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                                    break;
                            }
                        }
                    }
                    catch (DowJonesUtilitiesException ex)
                    {
                        HandleErrorImage(context.Response, ex.ReturnCode, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
                    }
                    catch (Exception)
                    {
                        HandleErrorImage(context.Response, -1, "image/png", ImageFormat.Png, ERROR_IMAGE_WIDTH, ERROR_IMAGE_HEIGHT);
                    }
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


        public override bool ValidateParameters(HttpContext context)
        {
            return true;
        }

        #endregion
    }
}
