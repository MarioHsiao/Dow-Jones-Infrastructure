using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "swapModuleEditorPackage", Namespace = "")]
    public class SwapModuleEditorPackage
    {
        public SwapModuleEditorPackage()
        {
            //SelectedCollection = SwapModuleEditorCollection.None;
        }

       [DataMember(Name = "industries")]
        public List<SwapModuleEditorModule> Industries { get; set; }

        [DataMember(Name = "regions")]
        public List<SwapModuleEditorModule> Regions { get; set; }

    }

    //[DataContract(Name = "swapModuleEditorCollection", Namespace = "")]
    //public enum SwapModuleEditorCollection
    //{
    //    [EnumMember] None = -1,
    //    [EnumMember] Industries = 0,
    //    [EnumMember] Regions = 1
    //}

    [DataContract(Name = "swapModuleEditorModule", Namespace = "")]
    public class SwapModuleEditorModule
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}