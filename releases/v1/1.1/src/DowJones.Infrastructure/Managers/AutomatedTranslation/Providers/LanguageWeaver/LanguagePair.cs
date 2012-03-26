// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguagePair.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the LanguagePair type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Utilities.Managers.AutomatedTranslation.Providers.LanguageWeaver
{
    /// <summary>
    /// Language Pair Class
    /// </summary>
    public class LanguagePair
    {
        public LanguagePair(int id, string fromLanguage, string intoLanguage)
        {
            Id = id;
            IntoLanguage = intoLanguage;
            FromLanguage = fromLanguage;
        }

        public int Id { get; private set; }

        /// <summary>
        /// Gets the "From" language.
        /// </summary>
        /// <value>From language.</value>
        public string FromLanguage { get; private set; }

        /// <summary>
        /// Gets the "Into" language.
        /// </summary>
        /// <value>The into language.</value>
        public string IntoLanguage { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return FromLanguage + "." + IntoLanguage;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(LanguagePair) && Equals((LanguagePair) obj);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(LanguagePair other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Id == Id && Equals(other.FromLanguage, FromLanguage) && Equals(other.IntoLanguage, IntoLanguage);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = Id;
                result = (result * 397) ^ (FromLanguage != null ? FromLanguage.GetHashCode() : 0);
                result = (result * 397) ^ (IntoLanguage != null ? IntoLanguage.GetHashCode() : 0);
                return result;
            }
        }
    }
}
