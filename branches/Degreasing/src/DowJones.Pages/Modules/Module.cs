using System;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using DowJones.Pages.Common;

namespace DowJones.Pages.Modules
{
    [KnownType("GetKnownTypes")]
    [DataContract(Name = "newsPageModule", Namespace = "")]
    public class Module
    {
        public Module()
        {
            ModuleProperties = new ModuleProperties();
        }

        [DataMember(Name = "description")]
        public virtual string Description { get; set; }

        [DataMember(Name = "id")]
        public virtual int Id { get; set; }

        [DataMember(Name = "rootId")]
        public virtual int RootId { get; set; }

        [DataMember(Name = "lastModifiedDate")]
        public virtual DateTime LastModifiedDate { get; set; }

        [Obsolete("Replaced by Page.Layout")]
        [DataMember(Name = "position")]
        public virtual int Position { get; set; }

        [DataMember(Name = "title")]
        public virtual string Title { get; set; }

        [DataMember(Name = "moduleProperties")]
        public ModuleProperties ModuleProperties { get; set; }

        [DataMember(Name = "moduleQualifier")]
        public virtual AccessQualifier ModuleQualifier { get; set; }

        [DataMember(Name = "queryFilterSet")]
        public virtual QueryFilterSet QueryFilterSet { get; set; }


        public virtual void BeforeDeleteModuleFromPage()
        {
        }

        public static Type[] GetKnownTypes()
        {
            return ServiceLocator.Resolve<IAssemblyRegistry>().GetKnownTypesOf<Module>().ToArray();
        }
    }
}