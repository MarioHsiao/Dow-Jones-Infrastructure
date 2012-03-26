using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Web.Navigation;

namespace DowJones.Web.Showcase
{
    public class ControllerActionMenuNode : NavigationMenuNode
    {
        public string Action
        {
            get { return Properties.ContainsKey("Action") ? Properties["Action"] : string.Empty; }
            set { Properties["Action"] = value; }
        }

        public string Controller
        {
            get { return Properties.ContainsKey("Controller") ? Properties["Controller"] : string.Empty; }
            set { Properties["Controller"] = value; }
        }


        public ControllerActionMenuNode(string displayName, string action = "Index", string controller = null) 
            : base(displayName)
        {
            Action = action;
            Controller = controller;
        }


        public string ToUrl(UrlHelper urlHelper)
        {
            return urlHelper.Action(Action, Controller);
        }
    }
}