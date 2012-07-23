// -----------------------------------------------------------------------
// <copyright file="Audit.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DowJones.Infrastructure.Models.SocialMedia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Audit"/> class.
        /// </summary>
        public Audit()
        {
            this.TransactionCollection = new List<Transaction>();
        }

        /// <summary>
        /// Gets the transaction collection.
        /// </summary>
        public List<Transaction> TransactionCollection
        {
            get;                       
            private set; 
        }
    }
}
