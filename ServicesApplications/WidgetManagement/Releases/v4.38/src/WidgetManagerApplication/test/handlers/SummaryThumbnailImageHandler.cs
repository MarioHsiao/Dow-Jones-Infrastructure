using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using EMG.Utility.Handlers;
using EMG.Utility.Managers;
using EMG.Utility.Managers.Search;
using EMG.Utility.Managers.Search.Requests;
using EMG.Utility.Managers.Search.Responses;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using factiva.nextgen.ui;

namespace EMG.widgets.ui.test.handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class SummaryThumbnailImageHandler : BaseHttpHandler
    {
        private static AccessionNumberSearchRequestDTO GetAccessionNumberSearchRequestDTO(string accessionNo)
        {
            var accessionNumberSearchRequestDTO = new AccessionNumberSearchRequestDTO();

            accessionNumberSearchRequestDTO.AccessionNumbers = new string[] {accessionNo};
            accessionNumberSearchRequestDTO.SearchCollectionCollection.Add(SearchCollection.Summary);
            accessionNumberSearchRequestDTO.SearchCollectionCollection.Add(SearchCollection.Internal);
            accessionNumberSearchRequestDTO.SearchCollectionCollection.Add(SearchCollection.CustomerDoc);
            return accessionNumberSearchRequestDTO;
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
                        g.DrawString(ResourceText.GetInstance.GetString("error"), new Font("Arial", 9, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 255)), new PointF(5, 5));
                        g.DrawString(ResourceText.GetInstance.GetErrorMessage(errorNum.ToString()), new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(102, 97, 97)), errorMessageRect);
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
            string accessionNo = context.Request["reference"] ?? string.Empty;
            string interfaceLanguage = context.Request["reference"] ?? "en";
            if (string.IsNullOrEmpty(accessionNo))
                return;
            // Get the blob document from archive
            ControlData cData = ControlDataManager.GetLightWeightUserControlData("brians", "passwd", "16");
            SearchManager searchManager = new SearchManager(ControlDataManager.Clone(cData), interfaceLanguage);
            try
            {
                AccessionNumberSearchResponse response = searchManager.PerformAccessionNumberSearch(GetAccessionNumberSearchRequestDTO(accessionNo));
                if (response.AccessionNumberBasedContentItemSet.Count > 0)
                {
                    foreach (AccessionNumberBasedContentItem item in response.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
                    {
                        if (!item.HasBeenFound || item.ContentHeadline.ContentItems == null)
                        {
                            continue;
                        }
                        foreach (ContentItem cItem in item.ContentHeadline.ContentItems.ItemCollection)
                        {
                            if (cItem.Type.ToLower() != "fnail" || string.IsNullOrEmpty(cItem.Mimetype))
                            {
                                continue;
                            }

                            GetBinaryRequest request = new GetBinaryRequest();
                            request.accessionNumber = accessionNo;
                            request.reference = cItem.Ref;
                            request.mimeType = cItem.Mimetype;
                            request.imageType = cItem.Type;

                            if (request.reference != null && request.reference.IndexOf(":") > 0)
                            {
                                request.reference = "probj:" + request.reference.Split(':')[1];
                                request.reference = request.reference.Replace("/" + request.accessionNumber, "");
                            }
                            ServiceResponse archiveResponse = ArchiveService.GetBinary(ControlDataManager.Clone(cData), request);
                            object objResponse;
                            archiveResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);

                            GetBinaryResponse binaryResponse = (GetBinaryResponse) objResponse;
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
                                    context.Response.ContentType = cItem.Mimetype;
                                    context.Response.BinaryWrite(binaryResponse.binaryData);
                                }
                                else
                                {
                                    switch (cItem.Mimetype)
                                    {
                                        case "image/gif":
                                        case "image/jpeg":
                                        case "image/png":
                                            HandleErrorImage(context.Response, tempErrorNum, "image/png", ImageFormat.Png, 50, 20);
                                            break;
                                        default:
                                            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                                            break;
                                    }
                                }
                            }
                            catch
                            {
                            }
                            finally
                            {
                                context.Response.End();
                            }
                        }
                    }
                }
            }
            catch
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
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