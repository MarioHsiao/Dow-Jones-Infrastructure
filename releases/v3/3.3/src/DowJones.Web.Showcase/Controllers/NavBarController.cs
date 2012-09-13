using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.NavBar;
using DowJones.Web.Mvc.UI.Components.Menu;
using DowJones.Web.Showcase.Models;
using DowJones.Web.Showcase.Modules.Empty;

namespace DowJones.Web.Showcase.Controllers
{
    public class NavBarController : CanvasControllerBase
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.NavBar = GetDemoNavBar();
        }

        public ActionResult Index()
        {
            ScriptRegistry.Include("~/scripts/navbardemo.js");
            return Canvas(new EmptyModule());
        }

        public ActionResult MockPage()
        {
            var canvasResult = Canvas(new PortalHeadlineLists());
            canvasResult.Canvas.CanEdit = true;

            return canvasResult;
        }

        private NavBarModel GetDemoNavBar()
        {
            var navBarModel = new NavBarModel()
            {
                Tabs = GetMockTabs(),
                
                // uncomment to see how you can override defaults with your own.
                //ActionItems = GetMockActions()
            };

            navBarModel.ActionItems.Add(new NavItem()
                                            {
                                                Label = "Admin",
                                                Tooltip = "Admin Page",
                                                HasMenuItems = false,    // don't want to show gear icon
                                                IsSelectable =  true,
                                                CssClass = "floatRight",
                                                Id = "admin-tab"
                                            });

            return navBarModel;

        }

        private IEnumerable<NavItem> GetMockActions()
        {
            return new NavItem[]
                       {
                           new NavItem
                               {
                                   IconClass = "fi_circle-plus",
                                   Id = "tab-addpage",
                                   Tooltip = "Add Page"
                               },
                           new NavItem
                               {
                                   IconClass = "fi_d-double-arrow",
                                   Id = "tab-menu"
                               }, 
                           new NavItem
                               {
                                   IconClass = "fi_expand",
                                   Id = "header-toggle",
                                   Tooltip = "Expand"

                               }

        };
        }

        private IEnumerable<NavItem> GetMockTabs()
        {
            var tabs = new NavItem[] {
                    new NavItem()
                    {
                        Id =  "Np_V1_12732_12732_000000",
                        Label =  "HP Trending 1",
                        IsSelected =  true,
                        IsSelectable =  true,
                        Position =  1,
                        MetaData = new { parentId = 1234 },
                        MenuItems = new MenuItem[] {
                                           new MenuItem()
                                               {
                                                   Id = "publish",
                                                   Label = "Publish and Share",
                                                   MetaData = new { PageId = "Np_V1_12732_12732_000000" }

                                               },
                                            new MenuItem()
                                               {
                                                   Id = "edit",
                                                   Label = "Edit"
                                               },
                                            new MenuItem()
                                               {
                                                   Id = "delete",
                                                   Label = "Delete"
                                               },
                                            new MenuItem()
                                               {
                                                   Id = "addModule",
                                                   Label = "Add a Module"
                                               }
                                        }
                    },

                    new NavItem() 
	                {
                        
                        Id =  "Np_V1_14785_14785_020102",
                        Label =  "surya-edit[lz 1.22]",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  2,
                        CssClass = "subscribed",
                        MenuItems = new MenuItem[] {
                                           new MenuItem()
                                               {
                                                   Id = "copy",
                                                   Label = "Copy"
                                               },
                                            new MenuItem()
                                               {
                                                   Id = "delete",
                                                   Label = "Delete"
                                               }
                                        }
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_14673_14673_020102",
                        Label =  "test create page",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  3,
                        CssClass = "subscribed"
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_14647_14647_020102",
                        Label =  "Greater China",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  4,
                        CssClass = "published"
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_14101_14101_000000",
                        Label =  "Accounting/Consulting",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  5,
                        
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_13967_13967_000000",
                        Label =  "Russia",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  6,
                        
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_13902_13902_020102",
                        Label =  "Banking/Credit - PSC Page",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  7,
                        
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_13898_13898_020102",
                        Label =  "Pharmaceuticals",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  8,
                        
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_13608_13608_000000",
                        Label =  "UK",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  9,
                        
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_14669_14669_000000",
                        Label =  "Callout Test - 35743",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  10,
                        
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_14847_14847_000000",
                        Label =  "Etats-Unis",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  11,
                        
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_14848_14848_000000",
                        Label =  "Brésil",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  12,
                        
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_14851_14851_000000",
                        Label =  "US",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  13,
                    
                    },

                    new NavItem() 
                    {
                        
                        Id =  "Np_V1_14884_14884_000000",
                        Label =  "RSS Test",
                        IsSelected =  false,
                        IsSelectable =  true,
                        Position =  14,
                        
                    }
                };



            return tabs;
        }



    }
}
