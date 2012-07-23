// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemHandler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using DowJones.Exceptions;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Item.V1_0;
using log4net;
using Image = System.Drawing.Image;

namespace DowJones.Web.Handlers.Items
{
    /// <summary>
    /// The item handler.
    /// </summary>
    public class ItemHandler : BaseHttpHandler
    {
        #region private members

        #region constants

        /// <summary>
        /// The content_ mim e_ type.
        /// </summary>
        private const string CONTENT_MIME_TYPE = "text/html";

        /// <summary>
        /// The fil e_ key.
        /// </summary>
        private const string FILE_KEY = "file";

        /// <summary>
        /// The item success.
        /// </summary>
        private const long ITEM_SUCCESS = 0;

        /// <summary>
        /// The require s_ authentication.
        /// </summary>
        private const bool REQUIRES_AUTHENTICATION = false;

        /// <summary>
        /// The public user id.
        /// </summary>
        private const string publicUserId = "RSSFEED1";

        /// <summary>
        /// The public user pwd.
        /// </summary>
        private const string publicUserPwd = "RSSFEED1";

        /// <summary>
        /// The public product.
        /// </summary>
        private const string publicProduct = "16";

        #endregion

        #region variables

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The _item section.
        /// </summary>
        private readonly ItemHandlerConfigSection _itemSection =
            (ItemHandlerConfigSection) ConfigurationManager.GetSection("ItemHandlerConfig");

        /// <summary>
        /// The _account id.
        /// </summary>
        private string _accountId;

        /// <summary>
        /// The _control data.
        /// </summary>
        private IControlData _controlData;

        /// <summary>
        /// The _file content.
        /// </summary>
        private byte[] _fileContent;

        /// <summary>
        /// The _item handler data.
        /// </summary>
        private ItemHandlerData _itemHandlerData;

        /// <summary>
        /// The _product id.
        /// </summary>
        private string _productId;

        /// <summary>
        /// The _request type.
        /// </summary>
        private string _requestType;

        /// <summary>
        /// The _user id.
        /// </summary>
        private string _userId;

        #endregion

        #region Helper Functions

        /// <summary>
        /// The get file max size.
        /// </summary>
        /// <param name="prodPrefix">
        /// The prod prefix.
        /// </param>
        /// <param name="mimeType">
        /// The mime type.
        /// </param>
        /// <returns>
        /// The get file max size.
        /// </returns>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        private long GetFileMaxSize(string prodPrefix, string mimeType)
        {
            if (_itemSection.Products[prodPrefix].MimeTypes[mimeType] == null &&
                _itemSection.Products[prodPrefix].MimeTypes[mimeType].MaxSize == null)
                throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerConfigMissing, 
                                               "MaxSize is missing in the config for " + prodPrefix + "," + mimeType);

            long maxFileSize = Convert.ToInt64(_itemSection.Products[prodPrefix].MimeTypes[mimeType].MaxSize);

            if (maxFileSize <= 0)
                throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerConfigInvalid, 
                                               "MaxSize is invalid in the config for " + prodPrefix + "," + mimeType);
            return maxFileSize;
        }

        /// <summary>
        /// The get base path.
        /// </summary>
        /// <returns>
        /// The get base path.
        /// </returns>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        private string GetBasePath()
        {
            if (_itemSection.BasePath == null || _itemSection.BasePath.Value == null)
                throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerConfigMissing, 
                                               "BasePath is missing in the config");
            string basePath = _itemSection.BasePath.Value;
            if (string.IsNullOrEmpty(basePath))
                throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerConfigInvalid, 
                                               "BasePath is invalid in the config");
            return basePath;
        }

        /// <summary>
        /// The save file.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="isUpdate">
        /// The is update.
        /// </param>
        /// <returns>
        /// The save file.
        /// </returns>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        private long SaveFile(HttpContext context, bool isUpdate)
        {
            long itemId;
            try
            {
                // Get file Info
                if (string.IsNullOrEmpty(_itemHandlerData.FileAssetName))
                    throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerParamInvalid, 
                                                   "File asset name is empty");

                HttpPostedFile file = context.Request.Files[FILE_KEY];
                if (file.ContentLength == 0)
                    throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerParamInvalid, 
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
                    throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerFileTypeInvalid, 
                                                   "Upload file type is invalid");

                if (mimeType != "JPEG" && mimeType != "PNG" && mimeType != "GIF")
                    throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerFileMimeTypeInvalid, 
                                                   "Upload image mime type is invalid");

                // read config
                long maxFileSize = GetFileMaxSize(_itemHandlerData.ProductPrefix, mimeType);
                string basePath = GetBasePath();

                maxFileSize = (_itemHandlerData.ImageMaxSize > 0) ? Math.Min(maxFileSize, _itemHandlerData.ImageMaxSize) : maxFileSize;
                if (file.ContentLength > maxFileSize)
                    throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerFileSizeExceeded, 
                                                   "Upload file size exceed the limit");

                // read file content
                _fileContent = new byte[file.ContentLength];
                file.InputStream.Read(_fileContent, 0, file.ContentLength);

                // check image height and width
                if ((contentType == "IMAGE") && (_itemHandlerData.ImageMaxHeight != 0 || _itemHandlerData.ImageMaxWidth != 0))
                {
                    var stream = new MemoryStream(_fileContent);
                    Image img = Image.FromStream(stream);
                    if (_itemHandlerData.ImageMaxHeight != 0 && img.Height > _itemHandlerData.ImageMaxHeight)
                        throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerImageHeightExceeded, 
                                                       "Upload image height exceed the limit");

                    if (_itemHandlerData.ImageMaxWidth != 0 && img.Width > _itemHandlerData.ImageMaxWidth)
                        throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerImageWidthExceeded, 
                                                       "Upload image width exceed the limit");
                }

                string relFilePath;
                string fullPath;

                var fileManager = new FileManager(_itemSection.Domain.Value, _itemSection.User.Value, _itemSection.Password.Value);


                if (isUpdate)
                {
// Update File
                    // if update, checks new file and existing file mime types are same
                    string updateContentType;
                    string updateMimeType;
                    relFilePath = GetFile(out updateContentType, out updateMimeType);

                    if (contentType != updateContentType.ToUpper() ||
                        mimeType != updateMimeType.ToUpper())
                    {
                        throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerParamInvalid, 
                                                       "Invalid image mime type to update ");
                    }

                    // Save file
                    fullPath = fileManager.GetFullPath(basePath, relFilePath);
                    fileManager.SaveFile(fullPath, _fileContent);
                    itemId = _itemHandlerData.AssetId;
                }
                else
                {
// Create new file
                    relFilePath = fileManager.GetRelativePath(_userId, _productId, _accountId, mimeType, 
                                                              DateTime.Now.Ticks + "." + mimeType);

// Save file
                    fullPath = fileManager.GetFullPath(basePath, relFilePath);
                    fileManager.SaveFile(fullPath, _fileContent);


                    // Create Item in PAM
                    try
                    {
                        var itemManager = new PAMItemManager();

                        Item item = itemManager.getItem(_itemHandlerData.FileAssetName, 
                                                        _itemHandlerData.FileAssetDescription, 
                                                        relFilePath, 
                                                        contentType, 
                                                        mimeType);
                        itemId = itemManager.CreateItem(item, _controlData);
                    }
                    catch (PAMItemManagerException ex)
                    {
                        Log.Error(ex.Message);

// delete the file if any error thrown, when item is created in PAM
                        fileManager.DeleteFile(fullPath);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }

            return itemId;
        }

        /// <summary>
        /// The update file name.
        /// </summary>
        private void UpdateFileName()
        {
            // Update item name in PAM
            try
            {
                var itemManager = new PAMItemManager();
                itemManager.UpdateItemName(_itemHandlerData.AssetId, _itemHandlerData.FileAssetName, _controlData);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The get item name.
        /// </summary>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <returns>
        /// The get item name.
        /// </returns>
        private string GetItemName(long itemId)
        {
            string itemName = string.Empty;
            var itemManager = new PAMItemManager();
            try
            {
                Item item = itemManager.getItemById(itemId, _controlData);
                if (item != null && item.GetType().Name.Equals("Image"))
                {
                    var image = (Factiva.Gateway.Messages.Assets.Item.V1_0.Image) item;
                    itemName = image.Properties.Name;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                itemName = string.Empty;
            }

            return itemName;
        }

        /// <summary>
        /// The get file.
        /// </summary>
        /// <param name="contentType">
        /// The content type.
        /// </param>
        /// <param name="mimeType">
        /// The mime type.
        /// </param>
        /// <returns>
        /// The get file.
        /// </returns>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        private string GetFile(out string contentType, out string mimeType)
        {
            contentType = string.Empty;
            mimeType = string.Empty;
            string relFilePath = string.Empty;

// Update item name in PAM
            try
            {
                var itemManager = new PAMItemManager();
                Item item = itemManager.getItemById(_itemHandlerData.AssetId, _controlData);

                if (item.GetType().Name.Equals("Image"))
                {
                    var image = (Factiva.Gateway.Messages.Assets.Item.V1_0.Image) item;
                    relFilePath = image.Properties.FilePath;
                    contentType = "IMAGE";
                    mimeType = itemManager.getImageMimeType(image.Properties.ImageMimeType);
                }

                if (string.IsNullOrEmpty(relFilePath))
                    throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerFileGetError, 
                                                   "Get file: failed");
                if (string.IsNullOrEmpty(contentType) || string.IsNullOrEmpty(mimeType))
                    throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerFileGetError, 
                                                   "Get file: Invlaid content/mime type");


                return relFilePath;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The delete file.
        /// </summary>
        private void DeleteFile()
        {
            try
            {
                string updateContentType;
                string updateMimeType;
                string relFilePath = GetFile(out updateContentType, out updateMimeType);

                // delete item asset
                var itemManager = new PAMItemManager();
                itemManager.DeleteItem(_itemHandlerData.AssetId, _controlData);

                // delete file
                try
                {
                    var fileManager = new FileManager(_itemSection.Domain.Value, _itemSection.User.Value, _itemSection.Password.Value);
                    string fullPath = fileManager.GetFullPath(_itemSection.BasePath.Value, relFilePath);
                    fileManager.DeleteFile(fullPath);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The get user authorization.
        /// </summary>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        private void GetUserAuthorization()
        {
            new SessionData(_itemHandlerData.AccessPointCode, _itemHandlerData.InterfaceLang, 0, false, 
                            _itemHandlerData.ProductPrefix, string.Empty);

            _controlData = SessionData.Instance().SessionBasedControlData;
            _userId = SessionData.Instance().UserId;
            _productId = SessionData.Instance().ProductId;
            _accountId = SessionData.Instance().AccountId;

            if (_controlData == null ||
                string.IsNullOrEmpty(_userId) ||
                string.IsNullOrEmpty(_productId) ||
                string.IsNullOrEmpty(_accountId))

                throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerUserAuthFailed, 
                                               "Get user authorisation failed");
        }

        /// <summary>
        /// The get response message.
        /// </summary>
        /// <param name="returnCode">
        /// The return code.
        /// </param>
        /// <param name="statusMessage">
        /// The status message.
        /// </param>
        /// <param name="exceptionMessage">
        /// The exception message.
        /// </param>
        /// <param name="assetId">
        /// The asset id.
        /// </param>
        /// <param name="assetName">
        /// The asset name.
        /// </param>
        /// <param name="lastModifiedDate">
        /// The last modified date.
        /// </param>
        /// <returns>
        /// </returns>
        private static ItemHandlerResponseDelegate GetResponseMessage(long returnCode, string statusMessage, string exceptionMessage, long assetId, string assetName, string lastModifiedDate)
        {
            var ajaxResponseDelegate = new ItemHandlerResponseDelegate
                                           {
                                               ReturnCode = returnCode, 
                                               StatusMessage = Resources.GetErrorMessage(returnCode.ToString()), 
                                               ExceptionMessage = exceptionMessage, 
                                               AssetId = assetId, 
                                               AssetName = assetName, 
                                               LastModifiedDate = lastModifiedDate
                                           };

            return ajaxResponseDelegate;
        }

        /// <summary>
        /// The get response message.
        /// </summary>
        /// <param name="returnCode">
        /// The return code.
        /// </param>
        /// <param name="statusMessage">
        /// The status message.
        /// </param>
        /// <param name="exceptionMessage">
        /// The exception message.
        /// </param>
        /// <param name="assetId">
        /// The asset id.
        /// </param>
        /// <returns>
        /// </returns>
        private ItemHandlerResponseDelegate GetResponseMessage(long returnCode, string statusMessage, string exceptionMessage, long assetId)
        {
            string assetName = string.Empty;
            string lastModifiedDate = string.Empty;
            var itemManager = new PAMItemManager();
            Factiva.Gateway.Messages.Assets.Item.V1_0.Image image;

            if (assetId > 0 && returnCode == 0)
            {
                image = (Factiva.Gateway.Messages.Assets.Item.V1_0.Image) itemManager.getItemById(assetId, _controlData);
                assetName = image.Properties.Name;
                lastModifiedDate = image.Properties.LastModifiedDate.ToString("G");
            }

            // assetName = GetItemName(assetId);
            return GetResponseMessage(returnCode, statusMessage, exceptionMessage, assetId, assetName, lastModifiedDate);
        }

        /// <summary>
        /// The write response message.
        /// </summary>
        /// <param name="ajaxResponseDelegate">
        /// The ajax response delegate.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        private void WriteResponseMessage(ItemHandlerResponseDelegate ajaxResponseDelegate, HttpResponse response)
        {
            response.Clear();
            if (_itemHandlerData.OutputType.Equals("xml"))
            {
                response.ContentType = "text/xml";

// response.Write(toXml(ajaxResponseDelegate, typeof(FileUploadHandlerResponseDelegate)));
                response.Write(ajaxResponseDelegate.ToXml());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (_itemHandlerData.OutputType.Equals("json") || _itemHandlerData.OutputType.Equals("iframe") || _itemHandlerData.OutputType.Equals("binary"))
            {
                if (_itemHandlerData.OutputType.Equals("iframe"))
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

        /// <summary>
        /// The get iframe content.
        /// </summary>
        /// <param name="ajaxResponseDelegate">
        /// The ajax response delegate.
        /// </param>
        /// <returns>
        /// The get iframe content.
        /// </returns>
        private string GetIframeContent(ItemHandlerResponseDelegate ajaxResponseDelegate)
        {
            var htmlText = new StringBuilder();
            htmlText.Append("<html><head>");
            htmlText.Append("<script type=\"text/javascript\">");
            htmlText.Append("document.domain=\"" + _itemHandlerData.DomainName + "\"");
            htmlText.Append("</script></head>");
            htmlText.Append("<body>");
            htmlText.Append("<div id=\"resultTxt\" >" + ajaxResponseDelegate.ToJson() + "</div>");

// htmlText.Append("var result=" + ajaxResponseDelegate.ToJson() + ";");
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

        /// <summary>
        /// The handle request.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        /// <exception cref="ItemHandlerException">
        /// </exception>
        public override void HandleRequest(HttpContext context)
        {
            context.Response.Clear();
            try
            {
                _requestType = context.Request.RequestType;
                string typeName = Path.GetFileNameWithoutExtension(context.Request.Path);
                char[] splitter = {'.'};
                string[] typeTokens = typeName.Split(splitter);

                // By pass get user info for Get Item. 
                // Use standard(RSSFeed1) user for login.
                if (typeTokens[4].Equals("GetItem"))
                {
                    _controlData = new ControlData
                                       {
                                           UserID = publicUserId, 
                                           UserPassword = publicUserPwd, 
                                           ProductID = publicProduct
                                       };
                }
                else
                {
                    // Get authorisation
                    GetUserAuthorization();
                }


                switch (typeTokens[4])
                {
                    case "GetItem":
                        if (_requestType.Equals("GET") && _itemHandlerData.AssetId > 0)
                        {
                            string contentType;
                            string mimeType;
                            string filePath = Path.Combine(
                                _itemSection.BasePath.Value, 
                                GetFile(out contentType, out mimeType));

                            var fileManager = new FileManager(_itemSection.Domain.Value, _itemSection.User.Value, _itemSection.Password.Value);
                            string fileBinary = Convert.ToBase64String(fileManager.GetFile(filePath));
                            if (_itemHandlerData.OutputType.Equals("xml"))
                            {
                                ItemHandlerResponseDelegate responseDelegate = GetResponseMessage(ITEM_SUCCESS, string.Empty, string.Empty, _itemHandlerData.AssetId);
                                responseDelegate.FileContent.FileBinary = fileBinary;
                                responseDelegate.FileContent.ImageMimeType = contentType + "/" + mimeType;
                                WriteResponseMessage(responseDelegate, context.Response);

                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else if (_itemHandlerData.OutputType.Equals("binary"))
                            {
                                context.Response.ContentType = contentType + "/" + mimeType;
                                context.Response.BinaryWrite(fileManager.GetFile(filePath));
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else
                                throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerParamInvalid, 
                                                               "For GetImage, format should be xml/binary");
                        }
                        else
                        {
                            throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerParamInvalid, 
                                                           "Paramter invalid for file upload operations");
                        }

                        break;
                    case "CreateItem":

// Create File
                        if (_requestType.Equals("POST") && _itemHandlerData.AssetId == 0 && context.Request.Files[FILE_KEY] != null)
                        {
                            long itemId = SaveFile(context, false);
                            WriteResponseMessage(GetResponseMessage(ITEM_SUCCESS, string.Empty, string.Empty, itemId), context.Response);
                        }
                        else
                        {
                            throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerParamInvalid, 
                                                           "Parameter invalid for file upload operations");
                        }

                        break;
                    case "UpdateItem":
                        if (_requestType.Equals("POST") && _itemHandlerData.AssetId > 0 && context.Request.Files[FILE_KEY] != null && context.Request.Files[FILE_KEY].ContentLength > 0)
                        {
                            long itemId = SaveFile(context, true);
                            WriteResponseMessage(GetResponseMessage(ITEM_SUCCESS, string.Empty, string.Empty, itemId), context.Response);
                        }
                        else if (_requestType.Equals("POST") && _itemHandlerData.AssetId > 0 && (!string.IsNullOrEmpty(_itemHandlerData.FileAssetName)) && _itemHandlerData.OperationType == 0)
                        {
                            UpdateFileName();
                            WriteResponseMessage(GetResponseMessage(ITEM_SUCCESS, string.Empty, string.Empty, _itemHandlerData.AssetId), context.Response);
                        }
                        else
                        {
                            throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerParamInvalid, 
                                                           "Parameter invalid for file upload operations");
                        }

                        break;
                    case "DeleteItem":
                        if (_requestType.Equals("POST") && _itemHandlerData.AssetId > 0)
                        {
                            // string assetName = GetItemName(_uploadData.AssetId);
                            DeleteFile();
                            WriteResponseMessage(GetResponseMessage(ITEM_SUCCESS, string.Empty, string.Empty, _itemHandlerData.AssetId, string.Empty, string.Empty), context.Response);
                        }
                        else
                        {
                            throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerParamInvalid, 
                                                           "Paramter invalid for file upload operations");
                        }

                        break;
                    default:
                        throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerError, 
                                                       "Invalid handler name");
                }


                //// Create File
                // if (_requestType.Equals("POST") && _uploadData.AssetId == 0 && context.Request.Files[FILE_KEY] != null)
                // {
                // long itemId = SaveFile(context, false);
                // WriteResponseMessage(GetResponseMessage(ITEM_SUCCESS, "", "", itemId), context.Response);
                // }
                // // Update File
                // else if (_requestType.Equals("POST") && _uploadData.AssetId > 0 && context.Request.Files[FILE_KEY] != null && context.Request.Files[FILE_KEY].ContentLength > 0)
                // {
                // long itemId = SaveFile(context, true);
                // WriteResponseMessage(GetResponseMessage(ITEM_SUCCESS, "", "", itemId), context.Response);
                // }
                // // Update Name
                // else if (_requestType.Equals("POST") && _uploadData.AssetId > 0 && (!string.IsNullOrEmpty(_uploadData.FileAssetName)) && _uploadData.OperationType == 0)
                // {
                // UpdateFileName();
                // WriteResponseMessage(GetResponseMessage(ITEM_SUCCESS, "", "", _uploadData.AssetId), context.Response);
                // }
                // // Get File By Id
                // else if (_requestType.Equals("GET") && _uploadData.AssetId > 0)
                // {
                // string contentType;
                // string mimeType;
                // string filePath = Path.Combine(_fileUploadSection.BasePath.Value,
                // GetFile(out contentType, out mimeType));

                // if (_uploadData.OutputType.Equals("xml"))
                // {
                // FileManager fm = new FileManager();
                // string fileBinary = Convert.ToBase64String(fm.GetFile(filePath));
                // FileUploadHandlerResponseDelegate responseDelegate = GetResponseMessage(ITEM_SUCCESS, "", "", _uploadData.AssetId);
                // responseDelegate.FileContent.FileBinary = fileBinary;
                // responseDelegate.FileContent.ImageMimeType = contentType + "/" + mimeType;
                // WriteResponseMessage(responseDelegate, context.Response);

                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                // }
                // else if (_uploadData.OutputType.Equals("binary"))
                // {
                // context.Response.ContentType = contentType + "/" + mimeType;
                // context.Response.WriteFile(filePath);
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                // }
                // else
                // throw new FileUploadHandlerException(EmgUtilitiesException.ITEM_HANDLER_PARAM_INVALID,
                // "For GetImage, format should be xml/binary");
                // }
                // // Delete File
                // else if (_requestType.Equals("POST") && _uploadData.AssetId > 0 && _uploadData.OperationType == 1)
                // {
                // //string assetName = GetItemName(_uploadData.AssetId);
                // DeleteFile();
                // WriteResponseMessage(GetResponseMessage(ITEM_SUCCESS, "", "", _uploadData.AssetId, "",""), context.Response);
                // }
                // else
                // {
                // throw new FileUploadHandlerException(EmgUtilitiesException.ITEM_HANDLER_PARAM_INVALID,
                // "Paramter invalid for file upload operations");
                // }
            }
            catch (DowJonesUtilitiesException ex)
            {
                new DowJonesUtilitiesException(ex, ex.ReturnCode);
                WriteResponseMessage(GetResponseMessage(ex.ReturnCode, string.Empty, ex.Message, _itemHandlerData.AssetId), context.Response);
            }
            catch (ItemHandlerException ex)
            {
                new DowJonesUtilitiesException(ex, ex.ErrorCode);
                WriteResponseMessage(GetResponseMessage(ex.ErrorCode, string.Empty, ex.Message, _itemHandlerData.AssetId), context.Response);
            }
            catch (PAMItemManagerException ex)
            {
                new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.ItemHandlerPamError);
                WriteResponseMessage(GetResponseMessage(DowJonesUtilitiesException.ItemHandlerPamError, string.Empty, ex.Message, _itemHandlerData.AssetId), context.Response);
            }
            catch (Exception ex)
            {
                new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.ItemHandlerError);
                WriteResponseMessage(GetResponseMessage(DowJonesUtilitiesException.ItemHandlerError, string.Empty, ex.Message, _itemHandlerData.AssetId), context.Response);
            }
        }

        /// <summary>
        /// Validates the parameters.  Inheriting classes must
        /// implement this and return true if the parameters are
        /// valid, otherwise false.
        /// </summary>
        /// <param name="context">
        /// Context.
        /// </param>
        /// <returns>
        /// <c>true</c> if the parameters are valid,
        /// otherwise <c>false</c>
        /// </returns>
        public override bool ValidateParameters(HttpContext context)
        {
            try
            {
                _itemHandlerData = new ItemHandlerData();
                _itemHandlerData.ReadParameters(context.Request.Params);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }

// return true;
        }
    }
}