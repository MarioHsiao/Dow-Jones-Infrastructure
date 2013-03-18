// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwitterGeoLocation.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DowJones.Infrastructure.Models.SocialMedia
{
    /// <summary>
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class TwitterGeoLocation : IEquatable<TwitterGeoLocation>
    {
        #region Constants and Fields

        /// <summary>
        /// The none.
        /// </summary>
        private static readonly TwitterGeoLocation NoneInstance = new TwitterGeoLocation();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterGeoLocation"/> class. 
        /// Initializes a new instance of the <see cref="TwitterGeoLocation"/> struct.
        /// </summary>
        /// <param name="latitude">
        /// The latitude.
        /// </param>
        /// <param name="longitude">
        /// The longitude.
        /// </param>
        public TwitterGeoLocation(double latitude, double longitude)
        {
            this.Coordinates = new GeoCoordinates { Latitude = latitude, Longitude = longitude };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterGeoLocation"/> class. 
        ///   Initializes a new instance of the <see cref="TwitterGeoLocation"/> struct.
        /// </summary>
        public TwitterGeoLocation()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets an instance of <see cref = "TwitterGeoLocation" />
        ///   that represents nowhere.
        /// </summary>
        /// <value>The None.</value>
        public static TwitterGeoLocation None
        {
            get
            {
                return NoneInstance;
            }
        }

        /// <summary>
        ///   Gets or sets the inner spatial coordinates.
        /// </summary>
        /// <value>The coordinates.</value>
        public GeoCoordinates Coordinates { get; set; }

        /// <summary>
        /// Gets or sets RawSource.
        /// </summary>
        public string RawSource { get; set; }

        /// <summary>
        ///   Gets or sets the type of location.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        #endregion

        #region Operators

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TwitterGeoLocation left, TwitterGeoLocation right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(null, left))
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TwitterGeoLocation left, TwitterGeoLocation right)
        {
            if (ReferenceEquals(left, right))
            {
                return false;
            }

            if (ReferenceEquals(null, left))
            {
                return true;
            }

            return !left.Equals(right);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="instance">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object instance)
        {
            if (ReferenceEquals(null, instance))
            {
                return false;
            }

            return instance.GetType() == typeof(TwitterGeoLocation) && this.Equals((TwitterGeoLocation)instance);
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
                return (this.Coordinates.Latitude.GetHashCode() * 397) ^ this.Coordinates.Longitude.GetHashCode();
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IEquatable<TwitterGeoLocation>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TwitterGeoLocation other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return other.Coordinates.Latitude == this.Coordinates.Latitude && other.Coordinates.Longitude == this.Coordinates.Longitude;
        }

        #endregion

        #endregion

        /// <summary>
        /// The inner spatial coordinates for this location.
        /// </summary>
        [Serializable]
        public class GeoCoordinates
        {
            #region Properties

            /// <summary>
            ///   Gets or sets the latitude.
            /// </summary>
            /// <value>The latitude.</value>
            public virtual double Latitude { get; set; }

            /// <summary>
            ///   Gets or sets the longitude.
            /// </summary>
            /// <value>The longitude.</value>
            public virtual double Longitude { get; set; }

            #endregion

            #region Operators

            /// <summary>
            ///   Performs an explicit conversion from <see cref = "TwitterGeoLocation.GeoCoordinates" /> to array of <see cref = "System.Double" />.
            /// </summary>
            /// <param name = "location">The location.</param>
            /// <returns>The result of the conversion.</returns>
            public static explicit operator double[](GeoCoordinates location)
            {
                return new[] { location.Latitude, location.Longitude };
            }

            /// <summary>
            ///   Performs an implicit conversion from <see cref = "double" /> to <see cref = "TwitterGeoLocation.GeoCoordinates" />.
            /// </summary>
            /// <param name = "values">The values.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator GeoCoordinates(List<double> values)
            {
                return FromEnumerable(values);
            }

            /// <summary>
            ///   Performs an implicit conversion from array of <see cref = "System.Double" /> to <see cref = "TwitterGeoLocation.GeoCoordinates" />.
            /// </summary>
            /// <param name = "values">The values.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator GeoCoordinates(double[] values)
            {
                return FromEnumerable(values);
            }

            #endregion

            #region Methods

            /// <summary>
            /// From the enumerable.
            /// </summary>
            /// <param name="values">The values.</param>
            /// <returns>A <see cref="GeoCoordinates"/> object.</returns>
            private static GeoCoordinates FromEnumerable(IEnumerable<double> values)
            {
                if (values == null)
                {
                    throw new ArgumentNullException("values");
                }

                var latitude = values.First();
                var longitude = values.Skip(1).Take(1).Single();

                return new GeoCoordinates { Latitude = latitude, Longitude = longitude };
            }

            #endregion
        }
    }
}