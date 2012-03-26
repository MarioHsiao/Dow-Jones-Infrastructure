using Factiva.Gateway.Messages.FCE.DnB.Utilities.V1_0;

namespace DowJones.Tools.Ajax.FamilyTree
{
    public class Address
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string City { get; set; }
        public string StateProvinceCounty { get; set; }
        public string ZipPostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Url { get; set; }
        
    }

    public class IndustryCollection
    {
        public string Code{ get; set;}
        public string Scheme { get; set; }
        public LangString Descriptor { get; set; }
    }

    public class Numerics
    {
        public string LocalCurrency;
        public string SalesLocalCurrency;
        public string SalesUSD;
        public string TotalEmployees;
    }

    public class FamilyTreeNodeInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string DunsNumber;

        /// <summary>
        /// 
        /// </summary>
        public string GlobalUltimateDunsNumber;

        /// <summary>
        /// 
        /// </summary>
        public string HQParentDunsNumber;

        /// <summary>
        /// 
        /// </summary>
        public string BusinessName;

        /// <summary>
        /// 
        /// </summary>
        public string GlobalUltimateBusinessName;

        /// <summary>
        /// 
        /// </summary>
        public string HierarchyDesignation;


        /// <summary>
        /// 
        /// </summary>
        public int NumberofImmediateBranches;

        /// <summary>
        /// 
        /// </summary>
        public int NumberofImmediateChildren;

        /// <summary>
        /// 
        /// </summary>
        public Address Address;
      
       /// <summary>
        /// 
        /// </summary>
        public IndustryCollection IndustryCollection;

        public Numerics Numerics;

        /// <summary>
        /// 
        /// </summary>
        public int NumberFamilyMembers;

        /// <summary>
        /// 
        /// </summary>
        public int NumberBranches;


    }
}
