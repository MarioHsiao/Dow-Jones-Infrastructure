using System;
using System.Collections.Specialized;
using DowJones.Exceptions;
using Factiva.Gateway.Messages.EmailDeliverySettings.V1_0;

namespace DowJones.Web.Handlers.DJInsider
{
    public class DJInsiderData
    {
        #region private members

        #region constants

        private const string ACCESS_POINT_KEY   = "napc";
        private const string DEBUG_KEY          = "dbg";
        private const string INTERFACE_LANG_KEY = "stil";
        private const string PRODUCT_PREFIX_KEY = "SA_FROM";

        private const string EMAIL_ADDRESS      = "em";
        private const string COUNTRY_CODE       = "country";
        private const string ACCOUNT_ID         = "acid";
        private const string DELIVERY_TIME      = "dtime";
        private const string EMAIL_FORMAT       = "em_frmt";
        private const string EMAIL_LAYOUT       = "em_layt";
        private const string WIRELESS_FRIENDLY  = "wr_frny";
        private const string SUBJECT            = "sbj";
        private const string TIME_ZONE          = "tm_zn";
        private const string SETUP_CODE         = "set_code";
        private const string STATUS             = "status";
        private const string OUTPUT_TYPE_KEY    = "outformat";
        private const string HANDLE             = "handle";
        


        #endregion

        #region variables

        private string _accessPointCode;
        private string _productPrefix;
        private int _debug;
        private string _interfaceLang;
        private readonly DJIRequestDTO _djiRequestDTO = new DJIRequestDTO();
        private string _outputType;
        private string _handle;

        #endregion


        #region Methods

        private static string ReadKeyValue(NameValueCollection parameters, string key)
        {
            if (parameters[key] == null || string.IsNullOrEmpty(parameters[key]))
                throw new DJInsiderHandlerException(DowJonesUtilitiesException.DjindexHandlerParamMissing,
                                                    "DJInsider handler parameter missing: " + key);

            return parameters[key];
        }

        private static EmailFormat getDJIEmailFormat(string emailFormat)
        {
            if (string.IsNullOrEmpty(emailFormat))
            {
                return EmailFormat.UnSpecified;
            }
           
            return (EmailFormat) Convert.ToInt32(emailFormat);
        }

        private static EmailLayout getDJIEmailLayout(string emailLayout)
        {
            if (string.IsNullOrEmpty(emailLayout))
            {
                return EmailLayout.UnSpecified;
            }

            return (EmailLayout)Convert.ToInt32(emailLayout);
        }

        private static DowJonesInsiderState getDJIStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return DowJonesInsiderState.Undefined;
            }

            return (DowJonesInsiderState)Convert.ToInt32(status);
        }

        #endregion

        #endregion

        #region Public Memebrs

        #region Properties

        public string AccessPointCode
        {
            get { return _accessPointCode; }
        }

        public string InterfaceLang
        {
            get { return _interfaceLang; }
        }

        public string ProductPrefix
        {
            get { return _productPrefix; }
        }

        public int Debug
        {
            get { return _debug; }
        }

        public DJIRequestDTO DJIRequestDTO
        {
            get { return _djiRequestDTO; }
        }
        public string OutputType
        {
            get { return _outputType; }
        }
        public string Handle
        {
            get { return _handle; }
        }
      
        #endregion

        #region Methods

        public void ReadParameters(NameValueCollection parameters)
        {
            // Access point code
            _accessPointCode = ReadKeyValue(parameters, ACCESS_POINT_KEY);

            // Interface Language
            _interfaceLang = ReadKeyValue(parameters, INTERFACE_LANG_KEY);

            // Product Prefix
            _productPrefix = ReadKeyValue(parameters, PRODUCT_PREFIX_KEY);

            // Debug
            _debug = Convert.ToInt32(ReadKeyValue(parameters, DEBUG_KEY));

            //Hadle
            _handle = ReadKeyValue(parameters, HANDLE);

              //Status
            if (_handle.Equals("DJIInit") && (parameters[STATUS] == null || string.IsNullOrEmpty(parameters[STATUS])))
                _djiRequestDTO.Status = DowJonesInsiderState.Undefined;
            else
                _djiRequestDTO.Status = getDJIStatus(parameters[STATUS]);


              //SetupCode
            if (parameters[SETUP_CODE] == null || string.IsNullOrEmpty(parameters[SETUP_CODE]))
                _djiRequestDTO.SetupCode = string.Empty;
            else
               _djiRequestDTO.SetupCode = ReadKeyValue(parameters, SETUP_CODE);


             // Output Type
            if (parameters[OUTPUT_TYPE_KEY] == null || string.IsNullOrEmpty(parameters[OUTPUT_TYPE_KEY]))
            {
                _outputType = "json";
            }
            else
            {
                _outputType = ReadKeyValue(parameters, OUTPUT_TYPE_KEY).ToLower();
            }

            //Status
            if (_handle.Equals("DJIInit") &&
                (parameters[STATUS] == null || string.IsNullOrEmpty(parameters[STATUS])))
                _djiRequestDTO.Status = DowJonesInsiderState.Undefined;
            else
                _djiRequestDTO.Status = getDJIStatus(parameters[STATUS]);


            if (_handle.Equals("DJIRequest") && _djiRequestDTO.Status == DowJonesInsiderState.Subscribed)
            {

                // EmailAddress
                _djiRequestDTO.EmailAddress = ReadKeyValue(parameters, EMAIL_ADDRESS);

                // CountryCode
                _djiRequestDTO.CountryCode = ReadKeyValue(parameters, COUNTRY_CODE);

                //DeliveryTime
                _djiRequestDTO.DeliveryTime = ReadKeyValue(parameters, DELIVERY_TIME);

                //AccountId
                _djiRequestDTO.AccountId = ReadKeyValue(parameters, ACCOUNT_ID);

                //EmailFormat
                _djiRequestDTO.EmailFormat = getDJIEmailFormat(ReadKeyValue(parameters, EMAIL_FORMAT));

                //EmailLayout
                _djiRequestDTO.EmailLayout = getDJIEmailLayout(ReadKeyValue(parameters, EMAIL_LAYOUT));

                //IsWirelessFriendly
                _djiRequestDTO.IsWirelessFriendly = (ReadKeyValue(parameters, WIRELESS_FRIENDLY).Equals("1"))
                                                        ? true
                                                        : false;


                //Subject
                _djiRequestDTO.Subject = ReadKeyValue(parameters, SUBJECT);

                //TimeZone
                _djiRequestDTO.TimeZone = ReadKeyValue(parameters, TIME_ZONE);


            }


        }

        #endregion

        #endregion
    }
}