using System.Collections.Generic;
using System.Collections.ObjectModel;
using DowJones.OperationalData.AssetActivity;

namespace DowJones.OperationalData.Asset
{

    public class BaseAssetOperationalData : AbstractOperationalData
    {
        private BaseCommonRequestOperationalData _commonOperationalData;

        public ReadOnlyCollection<ODSTranName> GetODSTranName
        {
            get
            {
                if (ODSConstants.KEY_ODS_TRAN_NAME != null && !string.IsNullOrEmpty(Get(ODSConstants.KEY_ODS_TRAN_NAME)))
                {

                    var trsnsNames = Get(ODSConstants.KEY_ODS_TRAN_NAME).Split(new[] { ',' });

                    var odsTranNames = new List<ODSTranName>();

                    foreach (var trsnsName in trsnsNames)
                    {
                        odsTranNames.Add(MapStringToODSTranName(trsnsName));
                    }

                    return odsTranNames.AsReadOnly();

                }
                return null;
            }
        }

        public ODSTranName AddODSTranName
        {
            set
            {
                if (string.IsNullOrEmpty(Get(ODSConstants.KEY_ODS_TRAN_NAME)))
                {
                    Add(ODSConstants.KEY_ODS_TRAN_NAME, MapODSTranNameToString(value));
                }
                else
                {
                    Add(ODSConstants.KEY_ODS_TRAN_NAME, Get(ODSConstants.KEY_ODS_TRAN_NAME) + "," + MapODSTranNameToString(value));
                }
            }
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

        public BaseAssetOperationalData()
        {

        }

        protected BaseAssetOperationalData(IDictionary<string, string> list) : base(list) { }

        public static ODSTranName MapStringToODSTranName(string type)
        {
            switch (type)
            {
                case "ArticleView":
                    return ODSTranName.ArticleView;
                case "AssetAction":
                    return ODSTranName.AssetAction;
                case "DashboardView":
                    return ODSTranName.DashboardView;
                case "EntryPointInfo":
                    return ODSTranName.EntryPointInfo;
                case "RelationMapping":
                    return ODSTranName.RelationMapping;
                case "SearchInfo":
                    return ODSTranName.SearchInfo;
                case "SnapshotInfo":
                    return ODSTranName.SnapshotInfo;
                case "CreateBriefingBook":
                    return ODSTranName.CreateBriefingBook;
                case "CompanySnapshotView":
                    return ODSTranName.CompanySnapshotView;
                case "IndustrySnapshotView":
                    return ODSTranName.IndustrySnapshotView;
                case "EntitySearch":
                    return ODSTranName.EntitySearch;
                case "TryItOptionUsage":
                    return ODSTranName.TryItOptionUsage;
                case "AutoCompleteUsage":
                    return ODSTranName.AutoCompleteUsage;
                case "SimpleSearchPreference":
                    return ODSTranName.SimpleSearchPreference;
                default: // case "":
                    return ODSTranName.UIErrorInfo;
            }
        }

        public static string MapODSTranNameToString(ODSTranName type)
        {
            switch (type)
            {
                case ODSTranName.ArticleView:
                    return "ArticleView";
                case ODSTranName.AssetAction:
                    return "AssetAction";
                case ODSTranName.DashboardView:
                    return "DashboardView";
                case ODSTranName.EntryPointInfo:
                    return "EntryPointInfo";
                case ODSTranName.RelationMapping:
                    return "RelationMapping";
                case ODSTranName.SearchInfo:
                    return "SearchInfo";
                case ODSTranName.SnapshotInfo:
                    return "SnapshotInfo";
                case ODSTranName.CreateBriefingBook:
                    return "CreateBriefingBook";
                case ODSTranName.CompanySnapshotView:
                    return "CompanySnapshotView";
                case ODSTranName.IndustrySnapshotView:
                    return "IndustrySnapshotView";
                case ODSTranName.EntitySearch:
                    return "EntitySearch";
                case ODSTranName.TryItOptionUsage:
                    return "TryItOptionUsage";
                case ODSTranName.AutoCompleteUsage:
                    return "AutoCompleteUsage";
                case ODSTranName.SimpleSearchPreference:
                    return "SimpleSearchPreference";
                default: //PostProcessingAdditional.NotApplicable
                    return "UIErrorInfo";
            }
        }



        public enum FromExtendedUniverseFlag
        {
            Yes,
            No
        }
        public static FromExtendedUniverseFlag MapStringToFromExtendedUniverseFlag(string type)
        {
            switch (type)
            {
                case "Y":
                    return FromExtendedUniverseFlag.Yes;
                case "N":
                    return FromExtendedUniverseFlag.No;
                default:
                    return FromExtendedUniverseFlag.No;

            }
        }
        public static string MapFromExtendedUniverseFlagToString(FromExtendedUniverseFlag type)
        {
            switch (type)
            {
                case FromExtendedUniverseFlag.Yes:
                    return "Y";
                case FromExtendedUniverseFlag.No:
                    return "N";
                default:
                    return "N";
            }
        }

    }

    public enum ODSTranName
    {
        DashboardView,
        SnapshotInfo,
        RelationMapping,
        UIErrorInfo,
        ArticleView,
        SearchInfo,
        AssetAction,
        EntryPointInfo,
        CreateBriefingBook,
        CompanySnapshotView,
        IndustrySnapshotView,
        EntitySearch,
        TryItOptionUsage,
        AutoCompleteUsage,
        SimpleSearchPreference

    }
   }
