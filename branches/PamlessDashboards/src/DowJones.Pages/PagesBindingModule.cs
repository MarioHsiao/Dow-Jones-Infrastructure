namespace DowJones.Pages
{
    public class DowJonesPagesBindingModule : DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IPageManager>().To<PageAssetsManagerPageManagerAdapter>();
            Bind<IPageSubscriptionManager>().To<PageAssetManagerPageSubscriptionManagerAdapter>();
        }
    }
}
