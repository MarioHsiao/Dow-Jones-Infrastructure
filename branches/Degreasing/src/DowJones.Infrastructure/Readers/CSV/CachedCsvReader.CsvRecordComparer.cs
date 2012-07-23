using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using DowJones.Readers.CSV.Exceptions;

namespace DowJones.Readers.CSV
{
    public partial class CachedCsvReader
    {
        #region Nested type: CsvRecordComparer

        /// <summary>
        /// Represents a CSV record comparer.
        /// </summary>
        private class CsvRecordComparer
            : IComparer<string[]>
        {
            #region Fields

            /// <summary>
            /// Contains the sort direction.
            /// </summary>
            private readonly ListSortDirection m_Direction;

            /// <summary>
            /// Contains the field index of the values to compare.
            /// </summary>
            private readonly int m_Field;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the CsvRecordComparer class.
            /// </summary>
            /// <param name="field">The field index of the values to compare.</param>
            /// <param name="direction">The sort direction.</param>
            public CsvRecordComparer(int field, ListSortDirection direction)
            {
                if (field < 0)
                    throw new ArgumentOutOfRangeException("field", field, string.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldIndexOutOfRange, field));

                m_Field = field;
                m_Direction = direction;
            }

            #endregion

            #region IComparer<string[]> Members

            public int Compare(string[] x, string[] y)
            {
                Debug.Assert(x != null && y != null && x.Length == y.Length && m_Field < x.Length);

                int result = String.Compare(x[m_Field], y[m_Field], StringComparison.CurrentCulture);

                return (m_Direction == ListSortDirection.Ascending ? result : -result);
            }

            #endregion
        }

        #endregion
    }
}