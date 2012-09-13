namespace DowJones.Security.Interfaces
{
    public interface ICIBsService
    {
        /// <summary>
        /// Gets a value indicating whether this instance has access to usage reports.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has access to usage reports; otherwise, <c>false</c>.
        /// </value>
        bool HasAccessToUsageReports { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has access to other users usage reports.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has access to other users usage reports; otherwise, <c>false</c>.
        /// </value>
        bool HasAccessToOtherUsersUsageReports { get; }
    }
}