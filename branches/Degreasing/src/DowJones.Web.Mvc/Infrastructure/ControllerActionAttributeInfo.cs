using System;
using System.Reflection;

namespace DowJones.Web.Mvc.Infrastructure
{
    public class ControllerActionAttributeInfo
    {
        public Type Controller { get; set; }

        public MethodInfo Action { get; set; }

        public Attribute Attribute { get; set; }


        public ControllerActionAttributeInfo()
        {
        }

        public ControllerActionAttributeInfo(Type controller, MethodInfo action, Attribute attribute)
        {
            Controller = controller;
            Action = action;
            Attribute = attribute;
        }
    }
}
