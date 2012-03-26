using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using TagCollection = Factiva.Gateway.Messages.Assets.Pages.V1_0.TagCollection;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public interface IModule : IViewComponentModel
    {
        bool CanEdit { get; set; }
        bool CanRefresh { get; set; }
        Canvas Canvas { get; set; }
        string CanvasId { get; set; }
        string Description { get; set; }
        IViewComponentModel Editor { get; set; }
        int ModuleId { get; set; }
        ModuleState ModuleState { get; set; }
        string ModuleType { get; }
        int Position { get; set; }
        TagCollection TagCollection { get; set; }
        string Title { get; set; }
    }

    public abstract class Module : ViewComponentModel, IModule
    {
        public Factiva.Gateway.Messages.Assets.Pages.V1_0.Module Asset { get; set; }

        protected string BaseDataServiceUrl
        {
            get { return Settings.Default.DashboardServiceBaseUrl; }
        }

        [ClientProperty("canEdit")]
        public bool CanEdit
        {
            get
            {
                return canEdit
                    ?? (Canvas != null && Canvas.CanEdit);
            }
            set { canEdit = value; }
        }
        private bool? canEdit;

        public bool CanRefresh { get; set; }

        public Canvas Canvas { get; set; }

        public string CanvasId { get; set; }

        public IControlData ControlData
        {
            get
            {
                if (_controlData == null && Canvas != null)
                    return Canvas.ControlData;

                return _controlData;
            }
            set { _controlData = value; }
        }
        private IControlData _controlData;

        [ClientProperty("dataServiceUrl")]
        public virtual string DataServiceUrl
        {
            get { return Settings.Default.GetDataServiceUrl(GetType()); }
        }

        public string Description
        {
            get { return HasAsset ? Asset.Description : string.Empty; }
            set { Asset.Description = value; }
        }

        public IViewComponentModel Editor { get; set; }

        // Need this property to relaibly determine module type across all browsers
        [ClientProperty("moduleType")]
        public string ModuleType
        {
            get { return GetType().Name; }
        }

        public ModuleState ModuleState { get; set; }

        public bool HasAsset
        {
            get { return Asset != null; }
        }

        [ClientProperty]
        public bool HasClientData { get; set; }

        public override string ID
        {
            get
            {
                if (base.ID == null && HasAsset)
                {
                    return string.Format("{0}-{1}", ModuleType, ModuleId);
                }

                return base.ID;
            }
            set { base.ID = value; }
        }

        [ClientProperty("moduleId")]
        public int ModuleId
        {
            get
            {
                if (_moduleID == null && HasAsset)
                    return Asset.Id;

                return _moduleID.GetValueOrDefault();
            }
            set { _moduleID = value; }
        }
        private int? _moduleID;

        [ClientProperty("position")]
        public int Position
        {
            get { return HasAsset ? Asset.Position : 0; }
            set { if (HasAsset) Asset.Position = value; }
        }

        public IPreferences Preferences
        {
            get
            {
                if (_preferences == null && Canvas != null)
                    return Canvas.Preferences;

                return _preferences;
            }
            set { _preferences = value; }
        }
        private IPreferences _preferences;

        public TagCollection TagCollection { get; set; }

        public string Title
        {
            get
            {
                if (_title == null && HasAsset)
                    return Asset.Title;

                return _title;
            }
            set { _title = value; }
        }

        private string _title;

        protected Module()
        {
            HasClientData = true;
        }
    }

    public class Module<TFactivaModule> : Module
        where TFactivaModule : Factiva.Gateway.Messages.Assets.Pages.V1_0.Module
    {
        public new TFactivaModule Asset
        {
            get { return (TFactivaModule)base.Asset; }
            set { base.Asset = value; }
        }

    }
}
