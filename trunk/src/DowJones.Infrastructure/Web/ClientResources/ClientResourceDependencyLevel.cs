namespace DowJones.Web
{
    public enum ClientResourceDependencyLevel : byte
    {
        /// <summary>
        /// Neither provides nor explictly depends upon
        /// any particular functionality
        /// </summary>
        /// <remarks>
        /// This is the type you'd use for "loose scripts"
        /// and the like.
        /// </remarks>
        Independent = 0,

        /// <summary>
        /// Depends upon higher-level functionality to provide
        /// components with the functionality they require
        /// </summary>
        Component,

        /// <summary>
        /// Both depends upon and provides higher-level functionality
        /// </summary>
        MidLevel,

        /// <summary>
        /// Provides higher-level functionality with no dependencies
        /// of its own
        /// </summary>
        Global,

        /// <summary>
        /// Nothing in the site will work without it!
        /// </summary>
        Core,
    }
}