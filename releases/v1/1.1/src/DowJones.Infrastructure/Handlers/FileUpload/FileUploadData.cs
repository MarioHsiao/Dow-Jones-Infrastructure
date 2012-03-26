using System;
using System.Collections.Specialized;
using EMG.Utility.Exceptions;

namespace EMG.Utility.Handlers.FileUpload
{
    public class FileUploadData
    {
        #region private members

        #region constants

        private const string ACCESS_POINT_KEY = "napc";
        private const string ASSET_DESCRIPTION_KEY = "fad";
        private const string ASSET_ID_KEY = "aid";
        private const string ASSET_NAME_KEY = "fan";
        private const string DEBUG_KEY = "dbg";
        private const string INTERFACE_LANG_KEY = "stil";
        private const string OPER_TYPE_KEY = "op";
        private const string OUTPUT_TYPE_KEY = "outformat";
        private const string PRODUCT_PREFIX_KEY = "SA_FROM";
        private const string IMAGE_MAX_HEIGHT_KEY = "maxht";
        private const string IMAGE_MAX_WIDTH_KEY = "maxwd";
        private const string IMAGE_MAX_SIZE_KEY = "maxsz";
        private const string DOMAIN_NAME_KEY = "dmn_name";


        #endregion

        #region variables

        private string _accessPointCode;
        private int _assetId;
        private int _debug;

        private string _fileAssetDescription;
        private string _fileAssetName;
        private string _interfaceLang;
        private int _operationType;
        private string _outputType;
        private string _productPrefix;
        private int _imageMaxWidth;
        private int _imageMaxHeight;
        private long _imageMaxSize;
        private string _domainName;


        #endregion

        #region Methods

        private static string ReadKeyValue(NameValueCollection parameters, string key)
        {
            if (parameters[key] == null || string.IsNullOrEmpty(parameters[key]))
                throw new FileUploadHandlerException("File upload handler parameter missing: " + key);

            return parameters[key];
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

        public string FileAssetName
        {
            get { return _fileAssetName; }
        }

        public string FileAssetDescription
        {
            get { return _fileAssetDescription; }
        }

        public string OutputType
        {
            get { return _outputType; }
        }

        public int Debug
        {
            get { return _debug; }
        }

        public int OperationType
        {
            get { return _operationType; }
        }

        public int AssetId
        {
            get { return _assetId; }
        }
        public int ImageMaxWidth
        {
            get { return _imageMaxWidth; }
        }
        public int ImageMaxHeight
        {
            get { return _imageMaxHeight; }
        }
        public long ImageMaxSize
        {
            get { return _imageMaxSize; }
        }
        public string DomainName
        {
            get { return _domainName; }
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

            // File Asset Name
            if (parameters[ASSET_NAME_KEY] == null || string.IsNullOrEmpty(parameters[ASSET_NAME_KEY]))
                _fileAssetName = string.Empty;
            else
                _fileAssetName = parameters[ASSET_NAME_KEY];

            // File Asset Description
            if (parameters[ASSET_DESCRIPTION_KEY] == null || string.IsNullOrEmpty(parameters[ASSET_DESCRIPTION_KEY]))
                _fileAssetDescription = string.Empty;
            else
                _fileAssetDescription = parameters[ASSET_DESCRIPTION_KEY];

            // Debug
            _debug = Convert.ToInt32(ReadKeyValue(parameters, DEBUG_KEY));

            // Output Type
            if (parameters[OUTPUT_TYPE_KEY] == null || string.IsNullOrEmpty(parameters[OUTPUT_TYPE_KEY]))
            {
                _outputType = "json";
            }
            else
            {
                _outputType = ReadKeyValue(parameters, OUTPUT_TYPE_KEY).ToLower();
                if (_outputType != "json" && _outputType != "xml" && _outputType != "binary" && _outputType != "iframe")
                    throw new FileUploadHandlerException(EmgUtilitiesException.FILE_UPLOAD_HANDLER_PARAM_INVALID,
                                                         "File upload handler parameter invalid " + OUTPUT_TYPE_KEY);
            }


            // Asset Id
            if (parameters[ASSET_ID_KEY] == null || string.IsNullOrEmpty(parameters[ASSET_ID_KEY]))
                _assetId = 0;
            else
                _assetId = Convert.ToInt32(parameters[ASSET_ID_KEY]);


            //Operation Type
            if (parameters[OPER_TYPE_KEY] == null || string.IsNullOrEmpty(parameters[OPER_TYPE_KEY]))
                _operationType = 0;
            else
                _operationType = Convert.ToInt32(parameters[OPER_TYPE_KEY]);

            //Image Maximum Width
            if (parameters[IMAGE_MAX_WIDTH_KEY] == null || string.IsNullOrEmpty(parameters[IMAGE_MAX_WIDTH_KEY]))
                _imageMaxWidth = 0;
            else
                _imageMaxWidth = Convert.ToInt32(parameters[IMAGE_MAX_WIDTH_KEY]);

            //Image Maximum Height
            if (parameters[IMAGE_MAX_HEIGHT_KEY] == null || string.IsNullOrEmpty(parameters[IMAGE_MAX_HEIGHT_KEY]))
                _imageMaxHeight = 0;
            else
                _imageMaxHeight = Convert.ToInt32(parameters[IMAGE_MAX_HEIGHT_KEY]);

            //Image Maximum Size
            if (parameters[IMAGE_MAX_SIZE_KEY] == null || string.IsNullOrEmpty(parameters[IMAGE_MAX_SIZE_KEY]))
                _imageMaxSize = 0;
            else
                _imageMaxSize = Convert.ToInt64(parameters[IMAGE_MAX_SIZE_KEY]);

            // Target Domain Name
            if (parameters[DOMAIN_NAME_KEY] == null || string.IsNullOrEmpty(parameters[DOMAIN_NAME_KEY]))
                _domainName = string.Empty;
            else
                _domainName = parameters[DOMAIN_NAME_KEY];
        }

        #endregion

        #endregion
    }
}