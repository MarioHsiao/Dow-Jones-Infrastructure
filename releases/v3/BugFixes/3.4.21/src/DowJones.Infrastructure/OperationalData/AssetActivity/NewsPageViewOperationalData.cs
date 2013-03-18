/* 
 * Author: Infosys
 * Date: 10/08/2010
 * Purpose: NewsPageViewOperationalData: to record news page view 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 */

namespace DowJones.OperationalData.AssetActivity
{
    /// <ClassName>NewsPageViewOperationalData</ClassName>
    /// <summary>
    /// To record news page view 
    /// </summary>
    /// <CodeReview Date=[Date] By=[name]>
    /// <ChangeSuggested></ ChangeSuggested>
    /// </CodeReview>
    /// <ChangeSummary Date=[Date]></ ChangeSummary>
    public class NewsPageViewOperationalData : BaseAssetActivityOperationalData
    {

        /// <summary>
        ///  Page Type to set the Asset Type 
        /// </summary>
        public string PageType
        {
            get { return AssetType; }
            set { AssetType = value; }
        }

        public NewsPageViewOperationalData()
        {

        }

        #region EnumRegion
        public enum NewsPageTypeCode
        {
            //PP- Personal page 
            PP,
            //GP - Group page
            GP,
            //FP - Factiva page
            FP
        }
        #endregion
    }
}
