using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using EMG.Tools.Session;
using EMG.Utility.Exceptions;
using EMG.Utility.Managers.Core;
using Factiva.Gateway.Messages.Assets.Item.V1_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;

namespace EMG.Utility.Handlers.FileUpload
{
    public class FileUploadHandler : BaseHttpHandler
    {
        #region private members

        #region constants

        private const string CONTENT_MIME_TYPE = "text/html";
        private const string FILE_KEY = "file";
        private const long FILE_UPLOAD_SUCCESS = 0;
        private const bool REQUIRES_AUTHENTICATION = false;
        

        #endregion

        #region variables

        private static readonly ILog _mLog =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly FileUploadHandlerConfigSection _fileUploadSection =
            (FileUploadHandlerConfigSection) ConfigurationManager.GetSection("FileUploadHandlerConfig");

        private string _accountId;
        private ControlData _controlData;
        private byte[] _fileContent;
        private string _productId;
        private string _requestType;
        private FileUploadData _uploadData;
        private string _userId;
        

        #endregion

        #region Helper Functions

        private long GetFileMaxSize(string prodPrefix, string mimeType)
        {
            if (_fileUploadSection.Products[prodPrefix].MimeTypes[mimeType] == null &&
                _fileUploadSection.Products[prodPrefix].MimeTypes[mimeType].MaxSize == null)
                throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_CONFIG_MISSING,
                                                     "MaxSize is missing in the config for " + prodPrefix + "," + mimeType);

            long maxFileSize = Convert.ToInt64(_fileUploadSection.Products[prodPrefix].MimeTypes[mimeType].MaxSize);

            if (maxFileSize <= 0)
                throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_CONFIG_INVALID,
                                                     "MaxSize is invalid in the config for " + prodPrefix + "," + mimeType);
            return maxFileSize;
        }

        private string GetBasePath()
        {
            if (_fileUploadSection.BasePath == null || _fileUploadSection.BasePath.Value == null)
                throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_CONFIG_MISSING,
                                                     "BasePath is missing in the config");
            string basePath = _fileUploadSection.BasePath.Value;
            if (string.IsNullOrEmpty(basePath))
                throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_CONFIG_INVALID,
                                                     "BasePath is invalid in the config");
            return basePath;
        }

        private long SaveFile(HttpContext context, bool isUpdate)
        {
            long itemId;
            try
            {
                //Get file Info
                if (string.IsNullOrEmpty(_uploadData.FileAssetName))
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_PARAM_INVALID,
                                                         "File asset name is empty");

                HttpPostedFile file = context.Request.Files[FILE_KEY];
                if (file.ContentLength == 0)
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_PARAM_INVALID,
                                                         "Upload file size is zero");


                string tContentType = file.ContentType;
                string[] arr = tContentType.Split('/');
                string contentType = arr[0].ToUpper();
                string mimeType = arr[1].ToUpper();

                if (mimeType.Equals("JPG") || mimeType.Equals("PJPEG"))
                    mimeType = "JPEG";
                else if (mimeType.Equals("X-PNG"))
                    mimeType = "PNG";

                if (contentType != "IMAGE")
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_FILE_TYPE_INVALID,
                                                         "Upload file type is invalid");

                if (mimeType != "JPEG" && mimeType != "PNG" && mimeType != "GIF")
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_FILE_MIME_TYPE_INVALID,
                                                         "Upload image mime type is invalid");

                // read config
                long maxFileSize = GetFileMaxSize(_uploadData.ProductPrefix, mimeType);
                string basePath = GetBasePath();

                maxFileSize = (_uploadData.ImageMaxSize > 0) ? (Math.Min(maxFileSize, _uploadData.ImageMaxSize)) : maxFileSize;
                if (file.ContentLength > maxFileSize)
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_FILE_SIZE_EXCEEDED,
                                                         "Upload file size exceed the limit");

                // read file content
                _fileContent = new byte[file.ContentLength];
                file.InputStream.Read(_fileContent, 0, file.ContentLength);

                //check image height and width
                if ((contentType == "IMAGE") && (_uploadData.ImageMaxHeight != 0 || _uploadData.ImageMaxWidth != 0))
                {
                    MemoryStream stream = new MemoryStream(_fileContent);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                    if(_uploadData.ImageMaxHeight != 0 && img.Height > _uploadData.ImageMaxHeight)
                        throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_IMAGE_HEIGHT_EXCEEDED,
                                                         "Upload image height exceed the limit");

                    if (_uploadData.ImageMaxWidth != 0 && img.Width > _uploadData.ImageMaxWidth)
                        throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_IMAGE_WIDTH_EXCEEDED,
                                                         "Upload image width exceed the limit");
                }

                string relFilePath;
                string fullPath;

                FileManager fileManager = new FileManager();


                if (isUpdate) // Update File
                {
                    // if update, checks new file and existing file mime types are same
                    string updateContentType;
                    string updateMimeType;
                    relFilePath = GetFile(out updateContentType, out updateMimeType);

                    if (contentType != updateContentType.ToUpper() ||
                        mimeType != updateMimeType.ToUpper())
                    {
                        throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_PARAM_INVALID,
                                                             "Invalid image mime type to update ");
                    }

                    //Save file
                    fullPath = fileManager.GetFullPath(basePath, relFilePath);
                    fileManager.SaveFile(fullPath, _fileContent);
                    itemId = _uploadData.AssetId;
                }
                else // Create new file
                {
                    relFilePath = fileManager.GetRelativePath(_userId, _productId, _accountId, mimeType,
                                                              DateTime.Now.Ticks + "." + mimeType);
                    //Save file
                    fullPath = fileManager.GetFullPath(basePath, relFilePath);
                    fileManager.SaveFile(fullPath, _fileContent);


                    //Create Item in PAM
                    try
                    {
                        PAMItemManager itemManager = new PAMItemManager();

                        Item item = itemManager.getItem(_uploadData.FileAssetName,
                                                        _uploadData.FileAssetDescription,
                                                        relFilePath,
                                                        contentType,
                                                        mimeType);
                        itemId = itemManager.CreateItem(item, _controlData);
                    }
                    catch (PAMItemManagerException ex)
                    {
                        _mLog.Error(ex.Message);
                        //delete the file if any error thrown, when item is created in PAM
                        fileManager.DeleteFile(fullPath);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _mLog.Error(ex.Message);
                throw;
            }

            return itemId;
        }

        private void UpdateFileName()
        {
            //Update item name in PAM
            try
            {
                PAMItemManager itemManager = new PAMItemManager();
                itemManager.UpdateItemName(_uploadData.AssetId, _uploadData.FileAssetName, _controlData);
            }
            catch (Exception ex)
            {
                _mLog.Error(ex.Message);
                throw;
            }
        }

        private string GetItemName(long itemId)
        {
            string itemName = string.Empty;
            PAMItemManager itemManager = new PAMItemManager();
            try
            {
                Item item = itemManager.getItemById(itemId, _controlData);
                if (item != null && item.GetType().Name.Equals("Image"))
                {
                    Image image = (Image) item;
                    itemName = image.Properties.Name;
                }
            }
            catch (PAMItemManagerException ex)
            {
                _mLog.Error(ex.Message);
                itemName = string.Empty;
            }
            return itemName;
        }

        private string GetFile(out string contentType, out string mimeType)
        {
            contentType = string.Empty;
            mimeType = string.Empty;
            string relFilePath = string.Empty;
            //Update item name in PAM
            try
            {
                PAMItemManager itemManager = new PAMItemManager();
                Item item = itemManager.getItemById(_uploadData.AssetId, _controlData);

                if (item.GetType().Name.Equals("Image"))
                {
                    Image image = (Image) item;
                    relFilePath = image.Properties.FilePath;
                    contentType = "IMAGE";
                    mimeType = itemManager.getImageMimeType(image.Properties.ImageMimeType);
                }

                if (string.IsNullOrEmpty(relFilePath))
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_FILE_GET_ERROR,
                                                         "Get file: failed");
                if (string.IsNullOrEmpty(contentType) || string.IsNullOrEmpty(mimeType))
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_FILE_GET_ERROR,
                                                         "Get file: Invlaid content/mime type");


                return relFilePath;
            }
            catch (Exception ex)
            {
                _mLog.Error(ex.Message);
                throw;
            }
        }

        private void DeleteFile()
        {
            try
            {
                string updateContentType;
                string updateMimeType;
                string relFilePath = GetFile(out updateContentType, out updateMimeType);

                //delete item asset
                PAMItemManager itemManager = new PAMItemManager();
                itemManager.DeleteItem(_uploadData.AssetId, _controlData);

                //delete file
                try
                {
                    FileManager fileManager = new FileManager();
                    string fullPath = fileManager.GetFullPath(_fileUploadSection.BasePath.Value, relFilePath);
                    fileManager.DeleteFile(fullPath);
                }
                catch (Exception ex)
                {
                    _mLog.Error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                _mLog.Error(ex.Message);
                throw;
            }
        }

        private void GetUserAuthorisation()
        {
            new SessionData(_uploadData.AccessPointCode, _uploadData.InterfaceLang, 0, true,
                            _uploadData.ProductPrefix, string.Empty);

            _controlData = SessionData.Instance().SessionBasedControlData;
            _userId = SessionData.Instance().UserId;
            _productId = SessionData.Instance().ProductId;
            _accountId = SessionData.Instance().AccountId;

            if (_controlData == null ||
                string.IsNullOrEmpty(_userId) ||
                string.IsNullOrEmpty(_productId) ||
                string.IsNullOrEmpty(_accountId))

                throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_USER_AUTH_FAILED,
                                                     "Get user authorisation failed");
        }

        private static FileUploadHandlerResponseDelegate GetResponseMessage(long returnCode, string statusMessage, string exceptionMessage, long assetId, string assetName, string lastModifiedDate)
        {
            FileUploadHandlerResponseDelegate ajaxResponseDelegate = new FileUploadHandlerResponseDelegate();
            ajaxResponseDelegate.ReturnCode = returnCode;
            ajaxResponseDelegate.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(returnCode.ToString());
            ajaxResponseDelegate.ExceptionMessage = exceptionMessage;
            ajaxResponseDelegate.AssetId = assetId;
            ajaxResponseDelegate.AssetName = assetName;
            ajaxResponseDelegate.LastModifiedDate = lastModifiedDate;


            return ajaxResponseDelegate;
        }

        private FileUploadHandlerResponseDelegate GetResponseMessage(long returnCode, string statusMessage, string exceptionMessage, long assetId)
        {
            string assetName = string.Empty;
            string lastModifiedDate = string.Empty;
            PAMItemManager itemManager = new PAMItemManager();
            Image image;

            if (assetId > 0)
            {
                image = (Image)itemManager.getItemById(assetId, _controlData);
                assetName = image.Properties.Name;
                lastModifiedDate = image.Properties.LastModifiedDate.ToString("G");
            }

            assetName = GetItemName(assetId);
            return GetResponseMessage(returnCode, statusMessage, exceptionMessage, assetId, assetName,lastModifiedDate);
        }

        private void WriteResponseMessage(FileUploadHandlerResponseDelegate ajaxResponseDelegate, HttpResponse response)
        {
            response.Clear();
            if (_uploadData.OutputType.Equals("xml"))
            {
                response.ContentType = "text/xml";
                //response.Write(toXml(ajaxResponseDelegate, typeof(FileUploadHandlerResponseDelegate)));
                response.Write(ajaxResponseDelegate.ToXml());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (_uploadData.OutputType.Equals("json") || _uploadData.OutputType.Equals("iframe") || _uploadData.OutputType.Equals("binary"))
            {
                if (_uploadData.OutputType.Equals("iframe"))
                {
                    response.ContentType = "text/html"; 
                    response.Write(GetIframeContent(ajaxResponseDelegate));
                }
                else
                {
                    response.ContentType = "application/json";
                    response.Write(ajaxResponseDelegate.ToJson());    
                }
                
            }
           

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        private string GetIframeContent(FileUploadHandlerResponseDelegate ajaxResponseDelegate)
        {
            StringBuilder htmlText = new StringBuilder();
            htmlText.Append("<html><head>");
            htmlText.Append("<script type=\"text/javascript\">");
            htmlText.Append("document.domain=\"" + _uploadData.DomainName + "\"");
            htmlText.Append("</script></head>");
            htmlText.Append("<body>");
            htmlText.Append("<div id=\"resultTxt\" >" + ajaxResponseDelegate.ToJson() + "</div>");
            //htmlText.Append("var result=" + ajaxResponseDelegate.ToJson() + ";");
            htmlText.Append("</body>");
            htmlText.Append("</html>");

            return htmlText.ToString();

        }


        #endregion

        #endregion

        /// <summary>
        /// Gets a value indicating whether this handler
        /// requires users to be authenticated.
        /// </summary>
        /// <value>
        ///    <c>true</c> if authentication is required
        ///    otherwise, <c>false</c>.
        /// </value>
        public override bool RequiresAuthentication
        {
            get { return REQUIRES_AUTHENTICATION; }
        }

        /// <summary>
        /// Gets the content MIME type.
        /// </summary>
        /// <value></value>
        public override string ContentMimeType
        {
            get { return CONTENT_MIME_TYPE; }
        }

        public override void HandleRequest(HttpContext context)
        {
            context.Response.Clear();
            try
            {
                _requestType = context.Request.RequestType;

                //Get authorisation
                GetUserAuthorisation();

                // Create File
                if (_requestType.Equals("POST") && _uploadData.AssetId == 0 && context.Request.Files[FILE_KEY] != null)
                {
                    long itemId = SaveFile(context, false);
                    WriteResponseMessage(GetResponseMessage(FILE_UPLOAD_SUCCESS, "", "", itemId), context.Response);
                }
                    // Update File
                else if (_requestType.Equals("POST") && _uploadData.AssetId > 0 && context.Request.Files[FILE_KEY] != null && context.Request.Files[FILE_KEY].ContentLength > 0)
                {
                    long itemId = SaveFile(context, true);
                    WriteResponseMessage(GetResponseMessage(FILE_UPLOAD_SUCCESS, "", "", itemId), context.Response);
                }
                    // Update Name
                else if (_requestType.Equals("POST") && _uploadData.AssetId > 0 && (!string.IsNullOrEmpty(_uploadData.FileAssetName)) && _uploadData.OperationType == 0)
                {
                    UpdateFileName();
                    WriteResponseMessage(GetResponseMessage(FILE_UPLOAD_SUCCESS, "", "", _uploadData.AssetId), context.Response);
                }
                    // Get File By Id
                else if (_requestType.Equals("GET") && _uploadData.AssetId > 0)
                {
                    string contentType;
                    string mimeType;
                    string filePath = Path.Combine(_fileUploadSection.BasePath.Value,
                                                   GetFile(out contentType, out mimeType));

                    if (_uploadData.OutputType.Equals("xml"))
                    {
                        FileManager fm = new FileManager();
                        string fileBinary = Convert.ToBase64String(fm.GetFile(filePath));
                        FileUploadHandlerResponseDelegate responseDelegate = GetResponseMessage(FILE_UPLOAD_SUCCESS, "", "", _uploadData.AssetId);
                        responseDelegate.FileContent.FileBinary = fileBinary;
                        responseDelegate.FileContent.ImageMimeType = contentType + "/" + mimeType;
                        WriteResponseMessage(responseDelegate, context.Response);

                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else if (_uploadData.OutputType.Equals("binary"))
                    {
                        context.Response.ContentType = contentType + "/" + mimeType;
                        context.Response.WriteFile(filePath);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                        throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_PARAM_INVALID,
                                                             "For GetImage, format should be xml/binary");
                }
                    // Delete File
                else if (_requestType.Equals("POST") && _uploadData.AssetId > 0 && _uploadData.OperationType == 1)
                {
                    //string assetName = GetItemName(_uploadData.AssetId);
                    DeleteFile();
                    WriteResponseMessage(GetResponseMessage(FILE_UPLOAD_SUCCESS, "", "", _uploadData.AssetId, "",""), context.Response);
                }
                else
                {
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_PARAM_INVALID,
                                                         "Paramter invalid for file upload operations");
                }
            }
            catch (EmgUtilitiesException ex)
            {
                new EmgUtilitiesException(ex, ex.ReturnCode);
                WriteResponseMessage(GetResponseMessage(ex.ReturnCode, "", ex.Message, _uploadData.AssetId), context.Response);
            }
            catch (FileUploadHandlerException ex)
            {
                new EmgUtilitiesException(ex, ex.ErrorCode);
                WriteResponseMessage(GetResponseMessage(ex.ErrorCode, "", ex.Message, _uploadData.AssetId), context.Response);
            }
            catch (PAMItemManagerException ex)
            {
                new EmgUtilitiesException(ex, EmgUtilitiesException.FILE_UPLOAD_HANDLER_PAM_ERROR);
                WriteResponseMessage(GetResponseMessage(EmgUtilitiesException.FILE_UPLOAD_HANDLER_PAM_ERROR, "", ex.Message, _uploadData.AssetId), context.Response);
            }
            catch (Exception ex)
            {
                new EmgUtilitiesException(ex, EmgUtilitiesException.FILE_UPLOAD_HANDLER_ERROR);
                WriteResponseMessage(GetResponseMessage(EmgUtilitiesException.FILE_UPLOAD_HANDLER_ERROR, "", ex.Message, _uploadData.AssetId), context.Response);
            }
        }

        /// <summary>
        /// Validates the parameters.  Inheriting classes must
        /// implement this and return true if the parameters are
        /// valid, otherwise false.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns><c>true</c> if the parameters are valid,
        /// otherwise <c>false</c></returns>
        public override bool ValidateParameters(HttpContext context)
        {
            try
            {
                _uploadData = new FileUploadData();
                _uploadData.ReadParameters(context.Request.Params);
                return true;
            }
            catch (Exception ex)
            {
                _mLog.Error(ex.Message);
                return false;
            }
            //return true;
        }
    }
}