using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using Factiva.Gateway.Messages.FCE.Assets.V1_0;
using Type = System.Type;

namespace DowJones.Web.Mvc.Infrastructure
{
    public interface IControllerRegistry
    {
        IEnumerable<Type> ControllerTypes { get; }
        IEnumerable<ControllerActionAttributeInfo> ControllerActionAttributes { get; }
    }

    public class ControllerRegistry : IControllerRegistry
    {
        public IEnumerable<Type> ControllerTypes
        {
            get { return _controllerTypes; }
        }
        private readonly IEnumerable<Type> _controllerTypes;

        public IEnumerable<ControllerActionAttributeInfo> ControllerActionAttributes
        {
            get
            {
                return _controllerActionAttributes =
                    _controllerActionAttributes ?? GetControllerActionAttributes(ControllerTypes);
            }
        }
        private IEnumerable<ControllerActionAttributeInfo> _controllerActionAttributes;


        [Inject("Disambiguation: this is the 'real' constructor; the other constructor is for testing")]
        public ControllerRegistry(IAssemblyRegistry assemblyRegistry)
        {
            _controllerTypes = assemblyRegistry.GetConcreteTypesDerivingFrom<IController>();
        }

        public ControllerRegistry(IEnumerable<Type> controllerTypes)
        {
            _controllerTypes = controllerTypes;
        }


        internal static IEnumerable<ControllerActionAttributeInfo> GetControllerActionAttributes(IEnumerable<Type> controllerTypes)
        {
            IEnumerable<Type> controllersWithAttributeDeclared =
                from controller in controllerTypes
                where controller.GetCustomAttributes(true).Any()
                select controller;

            IEnumerable<ControllerActionAttributeInfo> actionsWithAttributeDeclaredAtControllerLevel =
                from controller in controllersWithAttributeDeclared
                from action in GetActionMethods(controller)
                from attribute in 
                    (
                        controller.GetCustomAttributes(true).Cast<Attribute>()
                        .Union(action.GetCustomAttributes(true).Cast<Attribute>())
                    )
                select new ControllerActionAttributeInfo(controller, action, attribute);

            IEnumerable<ControllerActionAttributeInfo> actionsWithAttributeDeclared =
                from controller in controllerTypes.Except(controllersWithAttributeDeclared)
                from action in GetActionMethods(controller)
                from attribute in action.GetCustomAttributes(true).Cast<Attribute>()
                select new ControllerActionAttributeInfo(controller, action, attribute);

            return actionsWithAttributeDeclared
                .Union(actionsWithAttributeDeclaredAtControllerLevel);
        }

        private static IEnumerable<MethodInfo> GetActionMethods(Type controller)
        {
            const BindingFlags actionMethodBindingFlags =
                  BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.InvokeMethod;

            var actionMethods =
                controller
                    .GetMethods(actionMethodBindingFlags)
                    .Where(method => method.DeclaringType == controller);


            var methodInfos = actionMethods as MethodInfo[] ?? actionMethods.ToArray();
            var actions = new List<MethodInfo>(methodInfos);

            // Custom Sorting to ensure that index is the initial action that gets processed
            actions.Sort((s1, s2) =>
                         {
                             if (s1.Name.Equals(s2.Name, StringComparison.OrdinalIgnoreCase))
                             {
                                 return 0;
                             }

                             if (s1.Name.Equals("index", StringComparison.OrdinalIgnoreCase))
                             {
                                 return -1;
                             }
                             return String.Compare(s1.Name, s2.Name, StringComparison.OrdinalIgnoreCase);
                         });
            return methodInfos;
        }
    }
}