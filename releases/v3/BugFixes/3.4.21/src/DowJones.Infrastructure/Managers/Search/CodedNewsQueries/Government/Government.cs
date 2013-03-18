namespace DowJones.Managers.Search.CodedNewsQueries.Government
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PersonSnapShot))]
    public class GovernmentOfficial
    {

        /// <remarks/>
        public string Jobtitle;

        /// <remarks/>
        public string Fname;

        /// <remarks/>
        public string Lname;

        /// <remarks/>
        public string Office;

        /// <remarks/>
        public Address Address;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public int PersonID;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public int OfficeID;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public Datatype DataSet;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public Branch Branch;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute("GovernmentAddress", Namespace = "")]
    public class Address
    {

        /// <remarks/>
        public string Addrstreet;

        /// <remarks/>
        public string Addrcity;

        /// <remarks/>
        public string Addrstate;

        /// <remarks/>
        public string Addrzip;

        /// <remarks/>
        public string Addrroom;

        /// <remarks/>
        public string Physaddrstreet;

        /// <remarks/>
        public string Physaddrcity;

        /// <remarks/>
        public string Physaddrstate;

        /// <remarks/>
        public string Physaddrzip;

        /// <remarks/>
        public string Physaddrroom;

        /// <remarks/>
        public string Phone;

        /// <remarks/>
        public string Email;

        /// <remarks/>
        public string Long;

        /// <remarks/>
        public string Lat;

        /// <remarks/>
        public string URL;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public class People
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("person")]
        public GovernmentOfficial[] Person;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public int Count;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum Datatype
    {

        /// <remarks/>
        Federal,

        /// <remarks/>
        State,

        /// <remarks/>
        County,

        /// <remarks/>
        Municipal,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum Branch
    {

        /// <remarks/>
        Executive,

        /// <remarks/>
        Judicial,

        /// <remarks/>
        Legislative,

        /// <remarks/>
        None,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public class PersonSnapShot : GovernmentOfficial
    {

        /// <remarks/>
        public People People;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute("GovernmentStatus", Namespace = "")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FactivaException))]
    public class Status
    {

        /// <remarks/>
        public string Type;

        /// <remarks/>
        public string Message;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public int Value;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "fcp:carroll:v1_0", IsNullable = false)]
    public class FactivaException : Status
    {

        /// <remarks/>
        public string Operation;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganizationSnapShot))]
    public class Organization
    {

        /// <remarks/>
        public Office OfficeID;

        /// <remarks/>
        public Office ParentOfficeID;

        /// <remarks/>
        public Office UltimateParentOfficeID;

        /// <remarks/>
        public Address Address;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public Datatype DataSet;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public Branch Branch;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public int Level;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public int NoChildren;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public bool HasSnapShot;

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public bool HasSnapShotSpecified;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public class Office
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public int OfficeID;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute]
        public string Value;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public class OrganizationSnapShot : Organization
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("keyFact", IsNullable = false)]
        public KeyFact[] Keyfacts;

        /// <remarks/>
        public People People;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public class KeyFact
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public KeyFactType Code;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute]
        public string Value;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum KeyFactType
    {

        /// <remarks/>
        FCD,

        /// <remarks/>
        UD,

        /// <remarks/>
        LD,

        /// <remarks/>
        MSA,

        /// <remarks/>
        PMSA,

        /// <remarks/>
        FIPS,
    }

}
