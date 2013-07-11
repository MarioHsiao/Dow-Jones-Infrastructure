using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Prod.X.Models.Site;

namespace DowJones.Prod.X.Web.Models
{
    public interface IMainNavMenuProvider
    {
        List<MenuItem<MainNavigationCategory>> GetMenuItems();
    }

    public class MainNavMenuProvider : IMainNavMenuProvider
    {
        private readonly List<MenuItem<MainNavigationCategory>> _items = new List<MenuItem<MainNavigationCategory>>();
        private readonly UrlHelper _urlHelper;

        public MainNavMenuProvider(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            InitializeList();
           
        }

        public List<MenuItem<MainNavigationCategory>> GetMenuItems()
        {
            return _items;
        }

        private void InitializeList()
        {
            _items.AddRange(new[]
                                {
                                    new MenuItem<MainNavigationCategory>
                                        {
                                            Id = "Home",
                                            IconClass = "icon-home",
                                            Text = "Home",
                                            NavigationCategory = MainNavigationCategory.Home,
                                            Url = _urlHelper.Content("~/")
                                        },
                                    /*new MenuItem<MainNavigationCategory>
                                        {
                                            Id = "Dashboards",
                                            IconClass = "icon-dashboard",
                                            Text = "Dashboards",
                                            NavigationCategory = MainNavigationCategory.Dashboard,
                                        },
                                    new MenuItem<MainNavigationCategory>
                                        {
                                            Id = "Search",
                                            IconClass = "icon-plus-sign",
                                            Text = "Search",
                                            NavigationCategory = MainNavigationCategory.Search,
                                        },
                                    new MenuItem<MainNavigationCategory>
                                        {
                                            Id = "News",
                                            IconClass = "icon-list-alt",
                                            Text = "News",
                                            NavigationCategory = MainNavigationCategory.News,
                                        },
                                    new MenuItem<MainNavigationCategory>
                                        {
                                            Id = "RealTime",
                                            IconClass = "icon-time",
                                            Text = "Real-Time",
                                            NavigationCategory = MainNavigationCategory.RealTime,
                                        },
                                    new MenuItem<MainNavigationCategory>
                                        {
                                            Id = "Preferences",
                                            IconClass = "icon-cog",
                                            Text = "Preferences",
                                            NavigationCategory = MainNavigationCategory.Preferences,
                                        },
                                    new MenuItem<MainNavigationCategory>
                                        {
                                            Id = "Labs",
                                            IconClass = "icon-beaker",
                                            Text = "Labs",
                                            NavigationCategory = MainNavigationCategory.Labs,
                                        },*/
                                });
        }
    }
}