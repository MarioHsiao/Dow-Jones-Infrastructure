using System.Collections.Generic;
using System.Xml.Serialization;
using DowJones.Utilities.OperationalData.Asset;

namespace DowJones.Utilities.OperationalData.BriefingBookCreation
{
    public class BriefingBookCreationOperationData : BaseAssetOperationalData
    {
       

        /// <summary>
        /// Gets or sets the article origin (from where the article was viewed). This is required.
        /// </summary>
        /// <value>The article origin.</value>
        public PDFDownloadType PDFDownload
        {
            get
            {
                return MapPDFDownloadType(Get(ODSConstants.FCS_OD_PDFDownload));
            }
            set
            {
                Add(ODSConstants.FCS_OD_PDFDownload, MapPDFDownloadType(value));
            }
        }

        public BriefingBookCreationOperationData() { }

        protected BriefingBookCreationOperationData(IDictionary<string, string> list) : base(list) { }

        #region EnumRegion
        public enum PDFDownloadType
        {
            QuickPDF,
            CustomizePDF,
            NotApplicable
        }
        #endregion

        #region MapperRegion
        public static PDFDownloadType MapPDFDownloadType(string type)
        {
            switch (type)
            {
                case "QuickPDF":
                    return PDFDownloadType.QuickPDF;
                case "CustomizePDF":
                    return PDFDownloadType.CustomizePDF;
                default:
                    return PDFDownloadType.NotApplicable;

            }
        }
        public static string MapPDFDownloadType(PDFDownloadType type)
        {
            switch (type)
            {
                case PDFDownloadType.QuickPDF:
                    return "QuickPDF";
                case PDFDownloadType.CustomizePDF:
                    return "CustomizePDF";
                default:
                    return "";
            }
        }
        #endregion
    }
}
