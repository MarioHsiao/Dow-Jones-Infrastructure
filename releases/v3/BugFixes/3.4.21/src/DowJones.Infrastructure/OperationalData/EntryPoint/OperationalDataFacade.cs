using System;

namespace DowJones.OperationalData.EntryPoint
{
    /// <summary>
    /// Helps to build operation data DTO for consumer/client page
    /// Default logic is based on entry point which is required parameter.
    /// First create instance of this class and set all properties then
    /// call GetOperationDataDTO to get DTO
    /// </summary>
    public class OperationalDataFacade
    {
        private readonly CommonOperationalData _commonData;
        private readonly string _encryptedOdString = String.Empty;

        public OperationalDataFacade(string entryPoint, string actionType, string encryptedOdString)
        {
            _commonData = new CommonOperationalData();
            _commonData.ActionType = actionType;
            _commonData.EntryPoint = entryPoint;
            _encryptedOdString = encryptedOdString;
        }

        public OperationalDataFacade()
        {
            _commonData = new CommonOperationalData();
        }

        /// <summary>
        /// 
        /// </summary>
        public string InterfaceLanguage
        {
            get { return _commonData.InterfaceLanguage; }
            set { _commonData.InterfaceLanguage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Device
        {
            get { return _commonData.Device; }
            set { _commonData.Device = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DestinationProduct
        {
            get { return _commonData.DestinationProduct; }
            set { _commonData.DestinationProduct = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Browser
        {
            get { return _commonData.Browser; }
            set { _commonData.Browser = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BrowserVersion
        {
            get { return _commonData.BrowserVersion; }
            set { _commonData.BrowserVersion = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Platform
        {
            get { return _commonData.Platform; }
            set { _commonData.Platform = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent
        {
            get { return _commonData.UserAgent; }
            set { _commonData.UserAgent = value; }
        }

        /// <summary>
        /// Returns operatonal data based on entry point value.
        /// Possible dto are,
        ///     EmailOperationalData
        ///     NewsletterArticleOperationalData
        ///     RssOperationalData
        ///     HomePageOperationalData
        ///     DirectURLOperationalData
        /// <remarks>Default dto is DirectURLOperationalData</remarks>
        /// </summary>
        /// <returns>Returns type of AbstractOperationalData</returns>
        public AbstractOperationalData GetOperationDataDTO()
        {
            AbstractOperationalData dto;
            string ep = _commonData.EntryPoint;
            ep = ep.Trim().ToLower() ?? "";
            switch (ep)
            {
                case "smail":
                case "em":
                case "ae":
                case "ode":
                case "la":
                case "ra":
                    dto = new EmailOperationalData();
                    break;
                case "nl":
                    dto = new NewsletterAssetOperationalData();
                    break;
                case "rs":
                case "rss":
                    dto = new RssOperationalData();
                    break;
                case "hp":
                    dto = new HomePageOperationalData();
                    break;
                case "al":
                    dto = new AlertAssetOperationalData();
                    break;
                case "ws":
                    dto = new WorkspaceAssetOperationalData();
                    break;
                default:
                    dto = new DirectURLOperationalData();
                    break;
            }
            dto.SetMemento(_encryptedOdString);
            dto.SetComponent(_commonData);
            return dto;
        }

        /// <summary>
        /// Returns operatonal data based on entry point value.
        /// Possible dto are,
        ///     EmailOperationalData
        ///     NewsletterArticleOperationalData
        ///     RssOperationalData
        ///     HomePageOperationalData
        ///     DirectURLOperationalData
        /// <remarks>Default dto is DirectURLOperationalData</remarks>
        /// </summary>
        /// <returns>Returns type of AbstractOperationalData</returns>
        public AbstractOperationalData GetOperationDataDTO(AbstractOperationalData dto)
        {
            dto.SetMemento(_encryptedOdString);
            dto.SetComponent(_commonData);
            return dto;
        }
    }
}