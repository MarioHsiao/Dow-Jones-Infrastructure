using System;
using System.ComponentModel;

namespace DowJones.Utilities.Readers.CSV
{
    public partial class CachedCsvReader : CsvReader
    {
        #region Nested type: CsvPropertyDescriptor

        /// <summary>
        /// Represents a CSV field property descriptor.
        /// </summary>
        private class CsvPropertyDescriptor
            : PropertyDescriptor
        {
            #region Fields

            /// <summary>
            /// Contains the field index.
            /// </summary>
            private readonly int m_Index;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the CsvPropertyDescriptor class.
            /// </summary>
            /// <param name="fieldName">The field name.</param>
            /// <param name="index">The field index.</param>
            public CsvPropertyDescriptor(string fieldName, int index)
                : base(fieldName, null)
            {
                m_Index = index;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the field index.
            /// </summary>
            /// <value>The field index.</value>
            public int Index
            {
                get { return m_Index; }
            }

            #endregion

            #region Overrides

            public override Type ComponentType
            {
                get { return typeof (CachedCsvReader); }
            }

            public override bool IsReadOnly
            {
                get { return true; }
            }

            public override Type PropertyType
            {
                get { return typeof (string); }
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override object GetValue(object component)
            {
                return ((string[]) component)[m_Index];
            }

            public override void ResetValue(object component)
            {
            }

            public override void SetValue(object component, object value)
            {
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            #endregion
        }

        #endregion
    }
}