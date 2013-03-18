// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Transaction.cs" company="Dow Jones & Company">
//  © 2011 Factiva, A Dow Jones & Company. 
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Infrastructure.Models.SocialMedia
{ 
    /// <summary>
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class. 
        /// Prevents a default instance of the <see cref="Transaction"/> class from being created.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public Transaction(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets Elapsed Time.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        public long ElapsedTime { get; set; }

        /// <summary>
        /// Gets or sets Details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
