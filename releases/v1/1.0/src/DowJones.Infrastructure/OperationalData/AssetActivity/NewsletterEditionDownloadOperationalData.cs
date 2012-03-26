using System;

namespace DowJones.Utilities.OperationalData.AssetActivity
{
    public enum DisseminationMethodTypes
    {
        MobileHeadlinesOnly,
        Mobile,
        RTF,
        PDF,
        Embedded,
        HTMLEmailedByFactiva,
        HTML
    }

    public class NewsletterEditionDownloadOperationalData : AbstractOperationalData
    {
        private BaseCommonRequestOperationalData _commonOperationalData;

        //public NewsletterEditionDownloadOperationalData()
        //{

        //    //DownloadDate = DateTime.Now.ToString();
        //}

        //protected string DownloadDate
        //{
        //    get { return Get(ODSConstants.KEY_DOWNLOAD_DATE); }
        //    set { Add(ODSConstants.KEY_DOWNLOAD_DATE, value); }
        //}

        public string EditionId
        {
            get { return Get(ODSConstants.KEY_EDITION_ID); }
            set { Add(ODSConstants.KEY_EDITION_ID, value); }
        }

        public string EditionAction
        {
            get { return Get(ODSConstants.KEY_EDITION_ACTION); }
            set { Add(ODSConstants.KEY_EDITION_ACTION, value); }
        } 

        public DisseminationMethodTypes DisseminationFormat
        {
            get { return (DisseminationMethodTypes)Enum.Parse(typeof(DisseminationMethodTypes), Get(ODSConstants.KEY_DISSEMINATION_FORMAT)); }
            set { Add(ODSConstants.KEY_DISSEMINATION_FORMAT, value.ToString()); }
        }

        public string SavePDFInInsight
        {
            get { return Get(ODSConstants.KEY_SAVE_PDF_IN_INSIGHT); }
            set { Add(ODSConstants.KEY_SAVE_PDF_IN_INSIGHT, value); }
        }

        public string ContainerResultsFormat
        {
            get { return Get(ODSConstants.KEY_CONTAINER_RESULTS_FORMAT); }
            set { Add(ODSConstants.KEY_CONTAINER_RESULTS_FORMAT, value); }
        }

        public BaseCommonRequestOperationalData CommonOperationalData
        {
            get
            {
                if (_commonOperationalData == null)
                {
                    _commonOperationalData = new BaseCommonRequestOperationalData(List);
                }
                return _commonOperationalData;
            }
        }
    }
}
