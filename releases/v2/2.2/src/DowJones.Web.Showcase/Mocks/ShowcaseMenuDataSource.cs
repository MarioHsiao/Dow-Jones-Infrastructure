using System.Collections.Generic;
using DowJones.Web.Navigation;

namespace DowJones.Web.Showcase.Mocks
{
    public class ShowcaseMenuDataSource : IMenuDataSource
    {
        private static readonly Menu FactivaMainMenu = new Menu(new [] {
            new FactivaMenuNode("Telecommunications") { IsActive = true },
            new FactivaMenuNode("Marketing"),
        });


        public IMenu GetMenu(string menuID)
        {
            switch(menuID)
            {
                case "factiva-main-menu":
                    return FactivaMainMenu;

                default:
                    return null;
            }
        }


        private class FactivaMenuNode : MenuNode
        {
            public FactivaMenuNode(string displayName, IEnumerable<IMenuNode> children = null) 
                : base(displayName, children)
            {
                ID = displayName;
                DisplayName = displayName;
                Properties.Add("page-id", displayName);
            }
        }
    }
}