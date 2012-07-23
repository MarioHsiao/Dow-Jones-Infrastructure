using System.Runtime.Serialization;

namespace DowJones.Pages.Executive
{
    [DataContract(Name = "executive", Namespace = "")]
    public class Executive
    {
        [DataMember(Name = "consolidationCode")]
        public string ConsolidationId { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "middleNames")]
        public string MiddleNames { get; set; }

        [DataMember(Name = "suffix")]
        public string Suffix { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "completeName")]
        public string CompleteName { get; set; }
        
        [DataMember(Name = "jobTitle")]
        public string JobTitle { get; set; }

        [DataMember(Name = "isOfficer")]
        public bool IsOfficer { get; set; }

        [DataMember(Name = "isEmployee")]
        public bool IsEmployee { get; set; }
    }
}