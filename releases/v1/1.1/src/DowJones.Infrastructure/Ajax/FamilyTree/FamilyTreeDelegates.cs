// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FamilyTreeDelegates.cs" company="">
//   
// </copyright>
// <summary>
//   The family tree action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using DowJones.Tools.Ajax;
using DowJones.Tools.Ajax.FamilyTree;

namespace DowJones.Utilities.Ajax.FamilyTree
{
    /// <summary>
    /// The family tree action.
    /// </summary>
    public enum FamilyTreeAction
    {
        /// <summary>
        /// The get children.
        /// </summary>
        GetChildren
    }

    /// <summary>
    /// The family tree request delegate.
    /// </summary>
    public class FamilyTreeRequestDelegate : IAjaxRequestDelegate
    {
        /// <summary>
        /// The duns number.
        /// </summary>
        public string DunsNumber;

        /// <summary>
        /// Include Branch Locations
        /// </summary>
        public bool IncludeBranchLocations;
    }

    /// <summary>
    /// The family tree response delegate.
    /// </summary>
    public class FamilyTreeResponseDelegate : AbstractAjaxResponseDelegate
    {
        /// <summary>
        /// The family tree data result.
        /// </summary>
        public FamilyTreeDataResult familyTreeDataResult;
    }
}