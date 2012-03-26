using System.Configuration;

namespace DowJones.Web.Handlers.Items
{
    public class ItemHandlerConfigSection : ConfigurationSection
    {

        [ConfigurationProperty("Products", IsDefaultCollection = true)]
        public ProductCollection Products
        {

            get { return (ProductCollection)base["Products"]; }

        }
        [ConfigurationProperty("BasePath", IsRequired = true, IsKey = false)]
        public BasePathElement BasePath
        {
            get { return (BasePathElement)base["BasePath"]; }
        }

        [ConfigurationProperty("Domain", IsRequired = true, IsKey = false)]
        public DomainElement Domain
        {
            get { return (DomainElement)base["Domain"]; }
        }



        [ConfigurationProperty("User", IsRequired = true, IsKey = false)]
        public UserElement User
        {
            get { return (UserElement)base["User"]; }
        }


        [ConfigurationProperty("Password", IsRequired = true, IsKey = false)]
        public PasswordElement Password
        {
            get { return (PasswordElement)base["Password"]; }
        }
    }
    public sealed class BasePathElement : ConfigurationElement
    {

        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }

    public sealed class DomainElement : ConfigurationElement
    {

        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }

    public sealed class UserElement : ConfigurationElement
    {

        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }


    public sealed class PasswordElement : ConfigurationElement
    {

        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }


    public sealed class ProductElement : ConfigurationElement
    {

        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string Product
        {
            get { return (string)this["id"]; }
            set { this["id"] = value; }
        }
        [ConfigurationProperty("MimeTypes", IsDefaultCollection = true)]
        public MimeTypeCollection MimeTypes
        {

            get { return (MimeTypeCollection)base["MimeTypes"]; }

        }

     
    }
    public sealed class MimeTypeElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        [ConfigurationProperty("MaxSize", IsRequired = true)]
        public string MaxSize
        {
            get { return (string)this["MaxSize"]; }
            set { this["MaxSize"] = value; }
        }
    }
    public sealed class ProductCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProductElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProductElement)element).Product;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "Product"; }
        }

        public ProductElement this[int index]
        {
            get { return (ProductElement)BaseGet(index); }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ProductElement this[string id]
        {
            get { return (ProductElement)BaseGet(id); }
        }

        public bool ContainsKey(string key)
        {
            bool result = false;
            object[] keys = BaseGetAllKeys();
            foreach (object obj in keys)
            {
                if ((string)obj == key)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
    public sealed class MimeTypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MimeTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MimeTypeElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "MimeType"; }
        }

        public MimeTypeElement this[int index]
        {
            get { return (MimeTypeElement)BaseGet(index); }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public MimeTypeElement this[string name]
        {
            get { return (MimeTypeElement)BaseGet(name); }
        }

        public bool ContainsKey(string key)
        {
            bool result = false;
            object[] keys = BaseGetAllKeys();
            foreach (object obj in keys)
            {
                if ((string)obj == key)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
