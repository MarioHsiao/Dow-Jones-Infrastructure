
namespace DowJones.Utilities.OperationalData.AssetActivity
{
    /// <summary>
    /// Represents the OperationalData for the personal home pages (PHP) in iWorks/Search20 product.
    /// This is to capture the Views Only; all other activity is captured on the back end.
    /// 
    /// </summary>
    public class PHPAssetActivityOperationalData : BaseAssetActivityOperationalData
    {
        public PHPAssetActivityOperationalData ()
        {
            AssetType = "PHP";
        }
    }



    /// <summary>
    /// Represents the OperationalData for the saved company and executive screening
    /// criteria in fce product.
    /// This is to capture the Views Only; all other activity is captured on the back end.
    /// </summary>
    public class ScreeningAssetActivityOperationalData : BaseAssetActivityOperationalData
    {
    }

    /// <summary>
    /// Represents the OperationalData for the Radar views in FCE product.
    /// This is to capture the Views Only; all other activity is captured on the back end.
    /// </summary>
    public class RadarViewAssetActivityOperationaData : BaseAssetActivityOperationalData
    {
    }
}
