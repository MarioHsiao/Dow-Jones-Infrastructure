using System.Runtime.Serialization;

namespace DowJones.Pages.Modules
{
    [DataContract(Name = "moduleState", Namespace = "")]
    public enum ModuleState
    {
        /// <summary>
        /// Minimized Module State.
        /// </summary>
        [EnumMember]
        Minimized = 0,

        /// <summary>
        /// Maximized Module State.
        /// </summary>
        [EnumMember]
        Maximized = 1,
    }
}