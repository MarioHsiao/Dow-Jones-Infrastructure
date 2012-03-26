using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using Module = DowJones.Pages.Modules.Module;
using DowJones.Pages.Common;

namespace DowJones.Pages
{
    [KnownType("GetKnownTypes")]
    [DataContract(Name = "page", Namespace = "")]
    public class Page
    {
        [DataMember(Name = "accessQualifier")]
        public virtual AccessQualifier AccessQualifier { get; set; }

        [DataMember(Name = "accessScope")]
        public virtual AccessScope AccessScope { get; set; }

        [DataMember(Name = "pageCategoryInfo")]
        public virtual CategoryInfo CategoryInfo { get; set; }

        public virtual string CacheKey { get; set; }

        [DataMember(Name = "description")]
        public virtual string Description { get; set; }

        [DataMember(Name = "id")]
        public virtual string ID
        {
            get
            {
                if (CacheKey.IsNotEmpty())
                    return CacheKey;

                return id;
            }
            set { id = value; }
        }
        private string id;

        [DataMember(Name = "isDefault")]
        public virtual bool IsDefault { get; set; }

        [DataMember(Name = "isActive")]
        public virtual bool IsActive { get; set; }

        [DataMember(Name = "isAvailable")]
        public virtual bool IsAvailable
        {
            get
            {
                if (_isAvailable != null)
                    return _isAvailable.Value;
                else 
                    return AccessScope == AccessScope.AssignedToUser
                        || AccessScope == AccessScope.OwnedByUser 
                        || AccessScope == AccessScope.SubscribedByUser;
            }
            set { _isAvailable = value; }
        }
        private bool? _isAvailable = true;

        [DataMember(Name = "lastModifiedDate")]
        public virtual DateTime LastModifiedDate { get; set; }

        [DataMember(Name = "createdDate")]
        public virtual DateTime CreatedDate { get; set; }

        [DataMember(Name = "metaDataInfo")]
        public virtual MetaData MetaData { get; set; }

        [DataMember(Name = "metaDataCollection")]
        public virtual List<MetaData> MetaDataCollection { get; set; }

        [DataMember(Name = "modules", EmitDefaultValue = false)]
        public virtual List<Module> ModuleCollection { get; set; }

        [DataMember(Name = "ownerNamespace")]
        public virtual string OwnerNamespace { get; set; }

        [DataMember(Name = "ownerUserId")]
        public virtual string OwnerUserId { get; set; }

        [DataMember(Name = "parentId")]
        public virtual string ParentID { get; set; }

        [DataMember(Name = "position")]
        public virtual int Position { get; set; }

        [DataMember(Name = "publishStatusScope")]
        public PublishStatusScope PublishStatusScope { get; set; }

        [DataMember(Name = "title")]
        public virtual string Title { get; set; }

        [DataMember(Name = "queryFilterSet")]
        public virtual QueryFilterSet QueryFilterSet { get; set; }


        public Page()
        {
            IsActive = true;
        }


        public static Type[] GetKnownTypes()
        {
            if (_knownTypes == null)
                _knownTypes = ServiceLocator.Resolve<IAssemblyRegistry>().GetKnownTypesOf<Page>().ToArray();

            return _knownTypes;
        }
        private static volatile Type[] _knownTypes;
    }

    public class Page<TGatewayModel>  : Page
    {
    }
}