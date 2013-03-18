using System;

namespace EMG.Utility.OperationalData
{
    /// <summary>
    /// Helps to build operation data DTO for consumer/client page
    /// Default logic is based on entry point which is required parameter.
    /// First create instance of this class and set all properties then
    /// call GetOperationDataDTO to get DTO
    /// </summary>
    public class OperationalDataFacade
    {
        private readonly CommonOperationalData commonData;
        private readonly string encryptedODString = String.Empty;

        public OperationalDataFacade(string entryPoint, string actionType, string encryptedODString)
        {
            commonData = new CommonOperationalData();
            commonData.ActionType = actionType;
            commonData.EntryPoint = entryPoint;
            this.encryptedODString = encryptedODString;
        }

        public OperationalDataFacade()
        {
            commonData = new CommonOperationalData();
        }

        /// <summary>
        /// 
        /// </summary>
        public string InterfaceLanguage
        {
            get { return commonData.InterfaceLanguage; }
            set { commonData.InterfaceLanguage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Device
        {
            get { return commonData.Device; }
            set { commonData.Device = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DestinationProduct
        {
            get { return commonData.DestinationProduct; }
            set { commonData.DestinationProduct = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Browser
        {
            get { return commonData.Browser; }
            set { commonData.Browser = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BrowserVersion
        {
            get { return commonData.BrowserVersion; }
            set { commonData.BrowserVersion = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Platform
        {
            get { return commonData.Platform; }
            set { commonData.Platform = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent
        {
            get { return commonData.UserAgent; }
            set { commonData.UserAgent = value; }
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
            string ep = commonData.EntryPoint;
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
                    dto = new NewsletterOperationalData();
                    break;
                case "rs":
                case "rss":
                    dto = new RssOperationalData();
                    break;
                case "hp":
                    dto = new HomePageOperationalData();
                    break;
                case "al":
                    dto = new AlertOperationalData();
                    break;
                default:
                    dto = new DirectURLOperationalData();
                    break;
            }
            dto.SetMemento(encryptedODString);
            dto.SetComponent(commonData);
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
            dto.SetMemento(encryptedODString);
            dto.SetComponent(commonData);
            return dto;
        }
    }
}