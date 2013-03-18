using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace DowJones.Readers.CSV
{
    public partial class CachedCsvReader
    {
        #region Nested type: CsvBindingList

        /// <summary>
        /// Represents a binding list wrapper for a CSV reader.
        /// </summary>
        private class CsvBindingList : IBindingList, ITypedList, IList<string[]>
        {
            #region Fields

            /// <summary>
            /// Contains the linked CSV reader.
            /// </summary>
            private readonly CachedCsvReader m_CachedCsvReader;

            /// <summary>
            /// Contains the cached record count.
            /// </summary>
            private int m_Count;

            /// <summary>
            /// Contains the current sort direction.
            /// </summary>
            private ListSortDirection m_Direction;

            /// <summary>
            /// Contains the cached property descriptors.
            /// </summary>
            private PropertyDescriptorCollection m_Properties;

            /// <summary>
            /// Contains the current sort property.
            /// </summary>
            private CsvPropertyDescriptor m_Sort;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the CsvBindingList class.
            /// </summary>
            /// <param name="csv"></param>
            public CsvBindingList(CachedCsvReader csv)
            {
                m_CachedCsvReader = csv;
                m_Count = -1;
                m_Direction = ListSortDirection.Ascending;
            }

            #endregion

            #region IBindingList members

            public void AddIndex(PropertyDescriptor property)
            {
            }

            public bool AllowNew
            {
                get { return false; }
            }

            public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
            {
                m_Sort = (CsvPropertyDescriptor) property;
                m_Direction = direction;

                m_CachedCsvReader.ReadToEnd();

                m_CachedCsvReader._records.Sort(new CsvRecordComparer(m_Sort.Index, m_Direction));
            }

            public PropertyDescriptor SortProperty
            {
                get { return m_Sort; }
            }

            public int Find(PropertyDescriptor property, object key)
            {
                int fieldIndex = ((CsvPropertyDescriptor) property).Index;
                string value = (string) key;

                int recordIndex = 0;
                int count = Count;

                while (recordIndex < count && m_CachedCsvReader[recordIndex, fieldIndex] != value)
                    recordIndex++;

                if (recordIndex == count)
                    return -1;
                else
                    return recordIndex;
            }

            public bool SupportsSorting
            {
                get { return true; }
            }

            public bool IsSorted
            {
                get { return m_Sort != null; }
            }

            public bool AllowRemove
            {
                get { return false; }
            }

            public bool SupportsSearching
            {
                get { return true; }
            }

            public ListSortDirection SortDirection
            {
                get { return m_Direction; }
            }

            public event ListChangedEventHandler ListChanged
            {
                add { }
                remove { }
            }

            public bool SupportsChangeNotification
            {
                get { return false; }
            }

            public void RemoveSort()
            {
                m_Sort = null;
                m_Direction = ListSortDirection.Ascending;
            }

            public object AddNew()
            {
                throw new NotSupportedException();
            }

            public bool AllowEdit
            {
                get { return false; }
            }

            public void RemoveIndex(PropertyDescriptor property)
            {
            }

            #endregion

            #region IBindingList Members

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public int Count
            {
                get
                {
                    if (m_Count < 0)
                    {
                        m_CachedCsvReader.ReadToEnd();
                        m_Count = (int) m_CachedCsvReader.CurrentRecordIndex + 1;
                    }

                    return m_Count;
                }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public int Add(object value)
            {
                throw new NotSupportedException();
            }

            public bool Contains(object value)
            {
                throw new NotSupportedException();
            }

            public int IndexOf(object value)
            {
                throw new NotSupportedException();
            }

            public void Insert(int index, object value)
            {
                throw new NotSupportedException();
            }

            public bool IsFixedSize
            {
                get { return true; }
            }

            public void Remove(object value)
            {
                throw new NotSupportedException();
            }

            object IList.this[int index]
            {
                get { return this[index]; }
                set { throw new NotSupportedException(); }
            }

            public void CopyTo(Array array, int index)
            {
                m_CachedCsvReader.MoveToStart();

                while (m_CachedCsvReader.ReadNextRecord())
                    m_CachedCsvReader.CopyCurrentRecordTo((string[]) array.GetValue(index++));
            }

            public bool IsSynchronized
            {
                get { return false; }
            }

            public object SyncRoot
            {
                get { return null; }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            #region IList<string[]> Members

            public int IndexOf(string[] item)
            {
                throw new NotSupportedException();
            }

            public void Insert(int index, string[] item)
            {
                throw new NotSupportedException();
            }

            public string[] this[int index]
            {
                get
                {
                    m_CachedCsvReader.MoveTo(index);
                    return m_CachedCsvReader._records[index];
                }
                set { throw new NotSupportedException(); }
            }

            public void Add(string[] item)
            {
                throw new NotSupportedException();
            }

            public bool Contains(string[] item)
            {
                throw new NotSupportedException();
            }

            public void CopyTo(string[][] array, int arrayIndex)
            {
                m_CachedCsvReader.MoveToStart();

                while (m_CachedCsvReader.ReadNextRecord())
                    m_CachedCsvReader.CopyCurrentRecordTo(array[arrayIndex++]);
            }

            public bool Remove(string[] item)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<string[]> GetEnumerator()
            {
                return m_CachedCsvReader.GetEnumerator();
            }

            #endregion

            #region ITypedList Members

            public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
            {
                if (m_Properties == null)
                {
                    PropertyDescriptor[] properties = new PropertyDescriptor[m_CachedCsvReader.FieldCount];

                    for (int i = 0; i < properties.Length; i++)
                        properties[i] = new CsvPropertyDescriptor(((IDataReader) m_CachedCsvReader).GetName(i), i);

                    m_Properties = new PropertyDescriptorCollection(properties);
                }

                return m_Properties;
            }

            public string GetListName(PropertyDescriptor[] listAccessors)
            {
                return string.Empty;
            }

            #endregion
        }

        #endregion
    }
}