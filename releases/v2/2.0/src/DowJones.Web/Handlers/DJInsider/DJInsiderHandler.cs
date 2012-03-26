using System;
using System.Diagnostics;
using System.Text;
using System.Web;
using DowJones.Exceptions;
using DowJones.Globalization;
using DowJones.Session;
using Factiva.Gateway.Messages.EmailDeliverySettings.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Web.Handlers.DJInsider
{
    /// <summary>
    /// This class handles the AJAX calls of DowJonesInsiderControl. It invokes Gateway transactions for
    /// Add, Update and Delete operations.
    /// </summary>
    public class DJInsiderHandler : BaseHttpHandler
    {

        private const string CONTENT_MIME_TYPE = "text/html";
        private const bool REQUIRES_AUTHENTICATION = false;
        private const long DJI_SUCCESS = 0;

        private string _accountId;
        private IControlData _controlData;
        private string _productId;
        private DJInsiderData _djInsiderData;
        private string _userId;


        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // private DJIRequestDTO _requestDTO;
        //private NameValueCollection Request;

        

        private void GetUserAuthorisation()
        {
            new SessionData(_djInsiderData.AccessPointCode, _djInsiderData.InterfaceLang, 0, false, _djInsiderData.ProductPrefix, string.Empty);

            _controlData = SessionData.Instance().SessionBasedControlData;
            _userId = SessionData.Instance().UserId;
            _productId = SessionData.Instance().ProductId;
            _accountId = SessionData.Instance().AccountId;
            _djInsiderData.DJIRequestDTO.AccountId = _accountId;

            if (_controlData == null ||
                string.IsNullOrEmpty(_userId) ||
                string.IsNullOrEmpty(_productId) ||
                string.IsNullOrEmpty(_accountId))

                throw new DJInsiderHandlerException(DowJonesUtilitiesException.DjindexHandlerUserAuthFailed, "Get user authorisation failed");
        }


        private static DJInsiderResponseDelegate GetResponseMessage(long returnCode, string exceptionMessage, string statusCode, int djiStatus, string eMailId, string djiPrefStatus)
        {
            var ajaxResponseDelegate = new DJInsiderResponseDelegate
                                           {
                                               ReturnCode = returnCode, 
                                               StatusMessage = ResourceTextManager.Instance.GetErrorMessage(returnCode.ToString()), 
                                               ExceptionMessage = exceptionMessage, 
                                               DJIStatus = djiStatus, 
                                               StatusCode = statusCode, 
                                               EMailID = eMailId, 
                                               DJIPrefItemStatus = djiPrefStatus
                                           };
            return ajaxResponseDelegate;
        }



        private void WriteResponseMessage(DJInsiderResponseDelegate ajaxResponseDelegate, HttpResponse response)
        {
            response.Clear();
            if (_djInsiderData.OutputType.Equals("xml"))
            {
                response.ContentType = "text/xml";
                //response.Write(toXml(ajaxResponseDelegate, typeof(FileUploadHandlerResponseDelegate)));
                response.Write(ajaxResponseDelegate.ToXml());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (_djInsiderData.OutputType.Equals("json") || _djInsiderData.OutputType.Equals("iframe") || _djInsiderData.OutputType.Equals("binary"))
            {
                response.ContentType = "application/json";
                response.Write(ajaxResponseDelegate.ToJson());
            }


            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #region Handler events/functions

        public override void HandleRequest(HttpContext context)
        {
            try
            {
                context.Response.Clear();
                GetUserAuthorisation();

                if (_djInsiderData.Handle.Equals("DJIAuto"))
                {
                    var item = GetDJIPreferenceItem();
                    
                    //long updateResult = -1;
                    var itemStatus = DowJonesInsiderStatePreference.Undefined;
                    if (item != null)
                    {

                        itemStatus = item.DowJonesInsiderState;
                        
                    }
                    DJIResponseDTO responseDTO = null;

                    if (itemStatus == DowJonesInsiderStatePreference.Subscribed)
                    {
                        responseDTO = GetDowJonesInsiderSubscription();
                    }


                    if (responseDTO != null)
                    {
                        if (responseDTO.SetupCode != "")
                        {

                            WriteResponseMessage(
                                GetResponseMessage(DJI_SUCCESS, responseDTO.ErrorDescription, responseDTO.SetupCode,
                                                   1, responseDTO.EmailAddress, Convert.ToString((int)itemStatus)),
                                context.Response);
                        }
                        else
                        {
                            WriteResponseMessage(
                                GetResponseMessage(DJI_SUCCESS, responseDTO.ErrorDescription, responseDTO.SetupCode, 0,
                                                   responseDTO.EmailAddress, Convert.ToString((int)itemStatus)), context.Response);
                        }


                    }
                    else
                    {
                        WriteResponseMessage(
                                   GetResponseMessage(DJI_SUCCESS, String.Empty, String.Empty, MapPreferenceStatusToDJIStatus(itemStatus),
                                                      String.Empty, Convert.ToString((int)itemStatus)), context.Response);
                    }
                }

                if (_djInsiderData.Handle.Equals("DJIInit"))
                {
                    var responseDTO = GetDowJonesInsiderSubscription();
                    var item = GetDJIPreferenceItem();
                    var itemStatus = DowJonesInsiderStatePreference.Undefined;
                    if (item != null)
                    {
                        itemStatus = item.DowJonesInsiderState;
                    }
                    //string str = "{}";
                    if (responseDTO != null)
                    {
                        if (responseDTO.SetupCode != "")

                        {
                            
                            WriteResponseMessage(
                                GetResponseMessage(DJI_SUCCESS, responseDTO.ErrorDescription, responseDTO.SetupCode,
                                                   1,responseDTO.EmailAddress, Convert.ToString((int) itemStatus)),
                                context.Response);
                        }
                        else
                        {
                            WriteResponseMessage(
                                GetResponseMessage(DJI_SUCCESS, responseDTO.ErrorDescription, responseDTO.SetupCode, 0,
                                                   responseDTO.EmailAddress, Convert.ToString((int)itemStatus)), context.Response);
                        }


                    }
                    else
                    {
                        WriteResponseMessage(
                                GetResponseMessage(DJI_SUCCESS, string.Empty, string.Empty, MapPreferenceStatusToDJIStatus(itemStatus),
                                                   string.Empty, "0"), context.Response);
                    }
                }

             
                DJIResponseDTO _responseDTO;

                if (_djInsiderData.Handle.Equals("DJIRequest"))
                {

                    _responseDTO = SaveOrUpdateDJInsiderState(_djInsiderData.DJIRequestDTO);

                    //this will be overriden the application's proxy page...
                    //no-op for me in UpdateDJIStatus..
                    var status = _djInsiderData.DJIRequestDTO.Status;
                    //GetStateEnum(Request["sDJIPreference"]);
                    var item = GetDJIPreferenceItem();
                    //long updateResult = -1;
                    string itemId = item == null ? AddDJIPreferenceItem() : item.ItemID;

                    UpdateDJIPreferenceItem(itemId, Map(status));

                    var setupCode = "";

                    if (_responseDTO != null && !String.IsNullOrEmpty(_responseDTO.SetupCode))
                    {
                        //pre fill the setupcode...
                        setupCode = _responseDTO.SetupCode;
                    }
                    if (status == DowJonesInsiderState.Unsubscribed)
                    {
                        setupCode = "";
                    }


                    WriteResponseMessage(
                        GetResponseMessage(DJI_SUCCESS, string.Empty, setupCode,
                                           (int) _djInsiderData.DJIRequestDTO.Status,
                                           _djInsiderData.DJIRequestDTO.EmailAddress, "0"), context.Response);




                }
            }
            catch (DJInsiderHandlerException ex)
            {
                new DowJonesUtilitiesException(ex, ex.ErrorCode);
                WriteResponseMessage(GetResponseMessage(ex.ErrorCode, ex.Message,
                                                        _djInsiderData.DJIRequestDTO.SetupCode,
                                                        (int) _djInsiderData.DJIRequestDTO.Status,
                                                        _djInsiderData.DJIRequestDTO.EmailAddress, "0"), context.Response);
            }
            catch (DowJonesUtilitiesException ex)
            {
                new DowJonesUtilitiesException(ex, ex.ReturnCode);
                WriteResponseMessage(GetResponseMessage(ex.ReturnCode, ex.Message,
                                                        _djInsiderData.DJIRequestDTO.SetupCode,
                                                        (int)_djInsiderData.DJIRequestDTO.Status,
                                                        _djInsiderData.DJIRequestDTO.EmailAddress, "0"), context.Response);
            }
            catch (Exception ex)
            {
                new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.DjindexHandlerError);
                WriteResponseMessage(GetResponseMessage(DowJonesUtilitiesException.DjindexHandlerError, ex.Message,
                                                        _djInsiderData.DJIRequestDTO.SetupCode,
                                                        (int)_djInsiderData.DJIRequestDTO.Status,
                                                        _djInsiderData.DJIRequestDTO.EmailAddress, "0"), context.Response);
            }

        }

        private static int MapPreferenceStatusToDJIStatus(DowJonesInsiderStatePreference preference)
        {
            switch (preference)
            {
                case DowJonesInsiderStatePreference.Unsubscribed:
                    return 0;
                case DowJonesInsiderStatePreference.Subscribed:
                    return 1;
                default:
                    return -1; //DowJonesInsiderStatePreference.Undefined:
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
                _djInsiderData = new DJInsiderData();
                _djInsiderData.ReadParameters(context.Request.Params);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }

        }

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


        #endregion

        #region protected methods/functions.

        /// <summary>
        /// This function calls SaveOrUpdateDJInsiderState() to perform approriate operation. 
        /// If returns successfully, it invokes consumer's version of UpdateDJIStatus().
        /// If returns with an error, the error code and description is sent to browser.
        /// </summary>
        /// <returns></returns>
        //protected void Save()
        //{
        //    Request = _requestDTO.ClientPage.Request.Form; //Get nameValue pairs.

        //    if (Request["handle"].Equals("DJIInit"))
        //    {
        //        DJIResponseDTO responseDTO = GetDowJonesInsiderSubscription(_requestDTO);
        //        string str = "{}";
        //        if (responseDTO != null)
        //        {
        //            if (!String.IsNullOrEmpty(responseDTO.SetupCode) && !String.IsNullOrEmpty(responseDTO.EmailAddress))
        //            {
        //                StringBuilder sb = new StringBuilder("{code:\"");
        //                sb.Append(responseDTO.SetupCode);
        //                sb.Append("\",email:\"");
        //                sb.Append(responseDTO.EmailAddress);
        //                sb.Append("\",status:1}");
        //                str = sb.ToString();
        //            }
        //        }
        //        WriteResponse(str, true);
        //    }

        //    _requestDTO.Status = GetStateEnum(Request["sDJIPreference"]);
        //    _requestDTO.EmailAddress = Request["sEmailAddress"];
        //    _requestDTO.SetupCode = Request["setupCodeHidden"];
        //    DJIResponseDTO _responseDTO;

        //    if (Request["handle"].Equals("DJIRequest"))
        //    {
        //        try
        //        {
        //            _responseDTO = SaveOrUpdateDJInsiderState(_requestDTO);
        //        }
        //        catch (DowJonesInsiderException e)
        //        {
        //            if(e.ReturnCode  != DowJonesInsiderException.ERROR_INVALID_SESSION_LONG)
        //            {
        //                StringBuilder sbResponse = new StringBuilder();
        //                sbResponse.Append("DJI.handleControlErr(\"");
        //                sbResponse.Append(e.ReturnCode);
        //                sbResponse.Append("\",\"");
        //                //sbResponse.Append(ResourceText.GetInstance.GetErrorMessage(e.ReturnCode.ToString()));
        //                sbResponse.Append(e.ReturnCode.ToString());
        //                sbResponse.Append("\");");
        //                WriteResponse(sbResponse.ToString(), true);
        //                return;
        //            }
        //            throw;
        //        }

        //        //this will be overriden the application's proxy page...
        //        //no-op for me in UpdateDJIStatus..
        //        DowJonesInsiderState status = GetStateEnum(Request["sDJIPreference"]);
        //        UpdateDJIStatus(status);
        //        if (_responseDTO != null && !String.IsNullOrEmpty(_responseDTO.SetupCode))
        //        {
        //            //pre fill the setupcode...
        //            _requestDTO.SetupCode = _responseDTO.SetupCode;
        //        }
        //        if (status == DowJonesInsiderState.Unsubscribed)
        //        {
        //            _requestDTO.SetupCode = "";
        //        }

        //        //Send success as response.
        //        WriteResponse(String.Format("DJI.Success(\"{0}\", \"{1}\")", ((int) status), _requestDTO.SetupCode), true);
        //    }

        //    throw new DowJonesInsiderException("Save error");
        //        //ResourceText.GetInstance.GetErrorMessage(
        //        //    DowJonesInsiderException.ERROR_UNKNOWN_PREFERENCE_REQUEST.ToString()),
        //        //DowJonesInsiderException.ERROR_UNKNOWN_PREFERENCE_REQUEST);
        //}

        /// <summary>
        /// This function call appropraite handler for Add, Updates or Deletes the subscription based of input dto.
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        private DJIResponseDTO SaveOrUpdateDJInsiderState(DJIRequestDTO requestDTO)
        {
            DJIResponseDTO _respDTO = null;
            switch (requestDTO.Status)
            {
                case DowJonesInsiderState.Unsubscribed: //user clicked on No Thanks
                    if (!String.IsNullOrEmpty(requestDTO.SetupCode))
                        _respDTO = DeleteDowJonesInsiderSubscription(requestDTO);
                    break;
                case DowJonesInsiderState.Subscribed: // user clicked on Save Or OK.
                    _respDTO = !String.IsNullOrEmpty(requestDTO.SetupCode) ? UpdateDowJonesInsiderSubscription(requestDTO) : AddDowJonesInsiderSubscription(requestDTO);
                    break;
            }
            return _respDTO;
        }

        /// <summary>
        /// This method will write properly formatted error message to the response stream and ends it.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="endResponse"></param>
        public static void WriteResponse(string response, bool endResponse)
        {
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(response);
            if (endResponse) HttpContext.Current.Response.End();
            return;
        }

        #endregion  protected methods/functions.

        #region virtual methods/functions.

        /// <summary>
        /// This is provided for consumers to update DJIStatus. It is invoked by DJIInsiderHandler automatically
        /// after subscription related save operation is completed successfully.
        /// If there us an error in implemented version then simply call WriteResponse(_responseDTO.ErrorCode) with
        /// appropriate error code.
        /// </summary>
        /// <param name="state">User desired state is passed as input.</param>
        protected virtual void UpdateDJIStatus(DowJonesInsiderState state)
        {
            throw new Exception("The method or operation is to be implemented by consumer.");
        }

        #endregion virtual methods/functions.

        #region Using Gateway trx.

        /// <summary>
        /// Invokes Gateway transaction to delete the existing subscription of DowJones Insider.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DJIResponseDTO DeleteDowJonesInsiderSubscription(DJIRequestDTO dto)
        {
            var responseDTO = new DJIResponseDTO(); 
            try
            {
                var deleteWhatsNewSettingsRequest = new DeleteWhatsNewSettingsRequest
                                                        {
                                                            setupCode = dto.SetupCode
                                                        };

                var serviceResponse = EmailDeliverySettingsService.DeleteWhatsNewSettings(ControlDataManager.Convert(_controlData), deleteWhatsNewSettingsRequest);

                ResponseUtility.GetObject<DeleteWhatsNewSettingsResponse>(serviceResponse);
            }
            catch (DowJonesInsiderException ex)
            {
                _log.Error("DeleteDowJonesInsiderSubscription failure", ex);
                throw new DJInsiderHandlerException(ex.ReturnCode, ex.Message);
            }
            catch (Exception ex)
            {
                _log.Error("DeleteDowJonesInsiderSubscription failure", ex);
                throw new DJInsiderHandlerException(DowJonesInsiderException.DELETE_TRANSACTION_FAILURE,
                                                    "DeleteDowJonesInsiderSubscription failure");
            }
            return responseDTO;
        }

        /// <summary>
        /// Invokes Gateway transaction to update the preferences for existing subscription of DowJones Insider.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DJIResponseDTO UpdateDowJonesInsiderSubscription(DJIRequestDTO dto)
        {
            var responseDTO = new DJIResponseDTO();
            try
            {
                var updateWhatsNewSettingsRequest = new UpdateWhatsNewSettingsRequest
                                                        {
                                                            AccountId = dto.AccountId, 
                                                            CountryCode = dto.CountryCode, 
                                                            setupCode = dto.SetupCode, 
                                                            DeliverySettings = new DeliverySettings
                                                                                   {
                                                                                       DeliveryTime = dto.DeliveryTime, 
                                                                                       EmailAddress = dto.EmailAddress, 
                                                                                       EmailFormat = dto.EmailFormat, 
                                                                                       EmailLayout = dto.EmailLayout, 
                                                                                       IsWirelessFriendly = dto.IsWirelessFriendly, 
                                                                                       Language = _djInsiderData.InterfaceLang, 
                                                                                       Subject = dto.Subject, 
                                                                                       TimeZone = dto.TimeZone
                                                                                   }
                                                        };

                var serviceResponse = EmailDeliverySettingsService.UpdateWhatsNewSettings(ControlDataManager.Convert(_controlData), updateWhatsNewSettingsRequest);
                var addWhatsNewSettingsResponse = ResponseUtility.GetObject<AddWhatsNewSettingsResponse>(serviceResponse);
                if (addWhatsNewSettingsResponse != null)
                {
                    responseDTO.SetupCode = addWhatsNewSettingsResponse.setupCode;
                }
            }
            catch (DowJonesInsiderException ex)
            {
                _log.Error("UpdateDowJonesInsiderSubscription failure", ex);
                throw new DJInsiderHandlerException(ex.ReturnCode, ex.Message);
            }
            catch (Exception ex)
            {
                _log.Error("UpdateDowJonesInsiderSubscription failure", ex);
                throw new DJInsiderHandlerException(DowJonesInsiderException.UPDATE_TRANSACTION_FAILURE,
                                                    "UpdateDowJonesInsiderSubscription failure");
            }
            return responseDTO;
        }

        /// <summary>
        /// Invokes Gateway transaction to add new subscription of DowJones Insider.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private DJIResponseDTO AddDowJonesInsiderSubscription(DJIRequestDTO dto)
        {
            var responseDTO = new DJIResponseDTO();
            try
            {
                var addWhatsNewSettingsRequest = new AddWhatsNewSettingsRequest
                                                     {
                                                         AccountId = dto.AccountId, 
                                                         CountryCode = dto.CountryCode, 
                                                         DeliverySettings = new DeliverySettings
                                                                              {
                                                                                  DeliveryTime = dto.DeliveryTime,
                                                                                  EmailAddress = dto.EmailAddress,
                                                                                  EmailFormat = dto.EmailFormat,
                                                                                  EmailLayout = dto.EmailLayout,
                                                                                  IsWirelessFriendly = dto.IsWirelessFriendly,
                                                                                  Language = _djInsiderData.InterfaceLang,
                                                                                  Subject = dto.Subject,
                                                                                  TimeZone = dto.TimeZone
                                                                              }
                                                     };
                var serviceResponse = EmailDeliverySettingsService.AddWhatsNewSettings(ControlDataManager.Convert(_controlData), addWhatsNewSettingsRequest);

                var addWhatsNewSettingsResponse = ResponseUtility.GetObject<AddWhatsNewSettingsResponse>(serviceResponse);
                if (addWhatsNewSettingsResponse != null)
                {
                    responseDTO.SetupCode = addWhatsNewSettingsResponse.setupCode;
                }
            }
            catch (DowJonesInsiderException ex)
            {
                _log.Error("AddDowJonesInsiderSubscription failure", ex);
                throw new DJInsiderHandlerException(ex.ReturnCode, ex.Message);

            }
            catch (Exception ex)
            {
                _log.Error("AddDowJonesInsiderSubscription failure", ex);
                throw new DJInsiderHandlerException(DowJonesInsiderException.ADD_TRANSACTION_FAILURE, "AddDowJonesInsiderSubscription failure");
            }
            return responseDTO;
        }

        /// <summary>
        /// Invokes Gateway transaction to get the subscription information of DowJones Insider for the user.
        /// </summary>
        /// <returns></returns>
        private DJIResponseDTO GetDowJonesInsiderSubscription()
        {
            var responseDTO = new DJIResponseDTO();
            try
            {
                var getWhatsNewSettingsRequest = new GetWhatsNewSettingsRequest();
                var serviceResponse = EmailDeliverySettingsService.GetWhatsNewSettings(ControlDataManager.Convert(_controlData), getWhatsNewSettingsRequest);
                var response = ResponseUtility.GetObject<GetWhatsNewSettingsResponse>(serviceResponse);
                if (response != null)
                {
                    responseDTO.SetupCode = response.setupCode;
                    responseDTO.EmailAddress = response.deliverySettings.EmailAddress;
                }
                else
                {
                    throw new DowJonesInsiderException(DowJonesInsiderException.GET_TRANSACTION_FAILURE);
                }
            }
            catch (DowJonesInsiderException ex)
            {
                _log.Error("GetDowJonesInsiderSubscription failure", ex);
                throw new DJInsiderHandlerException(ex.ReturnCode, ex.Message);
            }
            catch (Exception ex)
            {
                _log.Error("GetDowJonesInsiderSubscription failure", ex);
                throw new DJInsiderHandlerException(DowJonesInsiderException.GET_TRANSACTION_FAILURE,
                                                    "GetDowJonesInsiderSubscription failure");
            }
            return responseDTO;
        }
       

        public static DJIRequestDTO GetBasicRequestDTO(bool callService, bool incRegInfo)
        {
            var djiRequestDTO = new DJIRequestDTO();
            return djiRequestDTO;
        }
        #endregion Using Gateway trx.

        #region Utility Functions

        /// <summary>
        /// Returns the DowJonesInsiderState value of equivalent enum.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetErrorObject(string number, string message)
        {
            var sb = new StringBuilder();
            sb.Append("new Error(\"");
            sb.Append(message);
            sb.Append("\",\"");
            sb.Append(number);
            sb.Append("\");");
            sb.Replace("\r", "\\r").Replace("\n", "\\n");
            var str = sb.ToString();
            Debug.WriteLine(sb.ToString());
            foreach (char t in str)
            {
                if (t < 32)
                    Debug.WriteLine(t + "\t" + (int)t);
            }
            //if (number == ERR_INVALID_SESSION)
            //{
            //    sb.Append(UiErrorHandler.GetLogoutScript());
            //}
            return sb.ToString();
        }

        /// <summary>
        /// Returns the enum equivalent of DowJonesInsiderState value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DowJonesInsiderState GetStateEnum(string value)
        {
            DowJonesInsiderState state = DowJonesInsiderState.Undefined; //Needs to initialize with some.

            if (value.Equals("-1"))
            {
                state = DowJonesInsiderState.Undefined;
            }
            else if (value.Equals("0"))
            {
                state = DowJonesInsiderState.Unsubscribed;
            }
            else if (value.Equals("1"))
            {
                state = DowJonesInsiderState.Subscribed;
            }
            return state;
        }

        //protected override void UpdateDJIStatus(DowJonesInsiderState state)
        //{

        //    using (DJInsiderService service = new DJInsiderService())
        //    {

        //        IUserSessionData usd = UIRegistry.SessionData;

        //        FVSServiceUtility.SetHeaderAndControl(service);

        //        service.UpdateStatus(Map(state));

        //    }

        //}
        private void UpdateDJIPreferenceItem(string itemID, DowJonesInsiderStatePreference djiState)
        {
            var item = new DowJonesInsiderStatePreferenceItem();
            var request = new UpdateItemRequest();

            item.ItemID = itemID;
            item.DowJonesInsiderState = djiState;
            request.Item = item;
            ServiceResponse serviceResponse = PreferenceService.UpdateItem(ControlDataManager.Convert(_controlData),
                                                                           request);
            if (serviceResponse == null)
            {
                throw new DJInsiderHandlerException(DowJonesUtilitiesException.DjindexHandlerUpdateDjiPreferenceFailed,
                                                    "Update DJI preferenceResponse is null");
            }
            if (serviceResponse.rc != 0)
            {
                throw new DJInsiderHandlerException( serviceResponse.rc,
                                                    "Update DJI preferenceResponse is failed");
            }
        }

        private DowJonesInsiderStatePreferenceItem GetDJIPreferenceItem()
        {
            var request = new GetItemsByClassIDRequest
                              {
                                  ClassID = new PreferenceClassID[1]
                              };
            request.ClassID[0] = PreferenceClassID.DowJonesInsiderState;

            var preferenceResponse = PreferenceService.GetItemsByClassID(ControlDataManager.Convert(_controlData), request);
            if(preferenceResponse == null)
            {
                //throw new DJInsiderHandlerException(EmgUtilitiesException.DJINDEX_HANDLER_GET_DJI_PREFERENCE_FAILED,
                //                                    "Get DJI preferenceResponse is null");
                return null;
            }

            if (preferenceResponse.rc != 0)
            {
                throw new DJInsiderHandlerException(preferenceResponse.rc,
                                                    "Get DJI preferenceResponse is failed");
            }
               
            var item = preferenceResponse.DowJonesInsiderState;
            return item;
        }

        private string AddDJIPreferenceItem()
        {
            var request = new AddItemRequest();

            var item = new DowJonesInsiderStatePreferenceItem
                           {
                               DowJonesInsiderState = DowJonesInsiderStatePreference.Subscribed
                           };

            request.Item = item;

            var serviceResponse = PreferenceService.AddItem(ControlDataManager.Convert(_controlData), request);
            if (serviceResponse == null)
            {
                throw new DJInsiderHandlerException(DowJonesUtilitiesException.DjindexHandlerAddDjiPreferenceFailed,
                                                    "Add DJI preferenceResponse is null");
            }
            if (serviceResponse.rc != 0)
            {
                throw new DJInsiderHandlerException( serviceResponse.rc,
                                                     "Add DJI preferenceResponse is failed");
            }
               
            
            object resposne;
            serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out resposne);
            if (resposne == null)
            {
                throw new DJInsiderHandlerException(DowJonesUtilitiesException.DjindexHandlerAddDjiPreferenceFailed,
                                                     "Add DJI preferenceResponse is failed");
            }
            
            var itemResponse = (AddItemResponse)resposne;
            Console.WriteLine(itemResponse.ItemID);
            var itemID = itemResponse.ItemID;

            if ( itemID == null )
            {
                throw new DJInsiderHandlerException(DowJonesUtilitiesException.DjindexHandlerAddDjiPreferenceFailed,
                                                     "Item id is null");
            }
            
            return itemID;
        }
        private static DowJonesInsiderStatePreference Map(DowJonesInsiderState dto)
        {
            switch (dto)
            {
                case DowJonesInsiderState.Subscribed:
                    return DowJonesInsiderStatePreference.Subscribed;
                case DowJonesInsiderState.Undefined:
                    return DowJonesInsiderStatePreference.Undefined;
                case DowJonesInsiderState.Unsubscribed:
                    return DowJonesInsiderStatePreference.Unsubscribed;
                default:
                    throw new NotSupportedException(dto.ToString());
            }
        }


        #endregion Utility Functions
    }


    /// <summary>
    /// This class handles the exceptions related to DowJones Insider control.
    /// </summary>
    public class DowJonesInsiderException : Exception
    {
        public static readonly int BASE_DJI_ERROR = 520901;
        public static readonly int UNKNOWN_ERROR = BASE_DJI_ERROR + 1;
        public static readonly int ERROR_UNKNOWN_PREFERENCE_REQUEST = BASE_DJI_ERROR + 2;
        public static readonly int GET_TRANSACTION_FAILURE = BASE_DJI_ERROR + 3;
        public static readonly int ADD_TRANSACTION_FAILURE = BASE_DJI_ERROR + 4;
        public static readonly int DELETE_TRANSACTION_FAILURE = BASE_DJI_ERROR + 5;
        public static readonly int UPDATE_TRANSACTION_FAILURE = BASE_DJI_ERROR + 6;
        
        //private static readonly ILog Log = LogManager.GetLogger(typeof(DowJonesInsiderException));
        //public static long ERROR_INVALID_SESSION_LONG = -2147176633;
        //public static string ERROR_INVALID_SESSION_STR = ERROR_INVALID_SESSION_LONG.ToString();

        private readonly long _returnCode = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesInsiderException"/> class.
        /// </summary>
        public DowJonesInsiderException()
        {
            //LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesInsiderException"/> class.
        /// </summary>
        /// <param name="returnCode">The return code.</param>
        public DowJonesInsiderException(long returnCode)
        {
            _returnCode = returnCode;
            //LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesInsiderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="returnCode">The return code.</param>
        public DowJonesInsiderException(string message, long returnCode)
            : base(message)
        {
            _returnCode = returnCode;
            //LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesInsiderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DowJonesInsiderException(string message)
            : base(message)
        {
            //LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesInsiderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DowJonesInsiderException(string message, Exception innerException)
            : base(message, innerException)
        {
            //LogException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DowJonesInsiderException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="returnCode">The return code.</param>
        public DowJonesInsiderException(Exception innerException, long returnCode)
            : base(returnCode.ToString(), innerException)
        {
            //LogException();
        }

        //public virtual ILog Logger
        //{
        //    get { return Log; }
        //}

        //protected long ReturnCodeProtected
        //{
        //    set { _returnCode = value; }
        //}

        public long ReturnCode
        {
            get { return _returnCode; }
        }

        ///// <summary>
        /////  Method that logs exception to <see cref="log4net"/>.
        ///// </summary>
        //protected void LogException()
        //{
        //    if (InnerException != null)
        //    {
        //        Logger.ErrorFormat("Error Message:{0}", InnerException.Message);
        //        Logger.ErrorFormat("Error StacTrace:{0}", InnerException.StackTrace);
        //        Logger.ErrorFormat("Error Source:{0}", InnerException.Source);
        //    }
        //    else if (Message != null && Message.Length > 0)
        //    {
        //        Logger.Error("Message: " + Message);
        //    }
        //    if (Logger.IsDebugEnabled)
        //    {
        //        Logger.DebugFormat("Return Code {0} was returned by service", _returnCode);
        //    }
        //}

        //public bool HasInvalidSessionErrorBeenThrown()
        //{
        //    if (_returnCode == ERROR_INVALID_SESSION_LONG)
        //    {
        //        return true;
        //    }
        //    return false;
        //}




        //public static string HandleException(string errorCode, string message)
        //{
        //    if (!errorCode.Equals("0"))
        //    {
        //        //return "{" + errorCode + ";" + message + "}";
        //        return "dji://errorCode^" + errorCode + ":" + message + "";
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //public static string HandleErrorResponse(string ErrorResponse)
        //{
        //    if (!ErrorResponse.Equals(""))
        //    {
        //        //return "{" + errorCode + ";" + message + "}";
        //        return "dji://errorCode^" + ErrorResponse + "";
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
    }

}