using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DowJones.Token.Encryption
{
    public class EncryptedTokenParser : IDictionary<string, string>, ITokenParser
    {
        private readonly IDictionary<string, string> m_Items = new Dictionary<string, string>(7, StringComparer.InvariantCultureIgnoreCase);
        private readonly string m_Key = KeyConfiguration.DEFAULT_ENCRYPTION_KEY;

        public EncryptedTokenParser()
        {
        }

        public EncryptedTokenParser(string token)
        {
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(token.Trim()))
            {
                Initialize(token);
            }
        }

        public EncryptedTokenParser(string key, string token)
        {
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(token.Trim()))
            {
                m_Key = key;
            }
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(token.Trim()))
            {
                Initialize(token);
            }
        }

        private void Initialize(string token)
        {
            var encryptor = new Security.Encryption();
            var nvp = encryptor.Decrypt(token, m_Key);
            foreach (string s in nvp)
            {
                UpdateItem(s, nvp[s]);
            }
        }

        #region ITokenParser Members

        public void UpdateItem(string key, string value)
        {
            if (!m_Items.ContainsKey(key))
            {
                m_Items.Add(key, value);
            }
            else
            {
                m_Items[key] = value;
            }
        }

        public string GetItem(string key)
        {
            string temp;
            return m_Items.TryGetValue(key, out temp) ? temp : null;
        }

        /// <summary>
        /// Encrypts this instance.
        /// </summary>
        /// <returns></returns>
        public string Encrypt()
        {
            // return an empty string if nothing to encrypt.
            if (m_Items.Count == 0)
                return string.Empty;

            var encryptor = new Security.Encryption();
            var nvp = new NameValueCollection();
            foreach (KeyValuePair<string, string> keyValuePair in this)
            {
                nvp.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return encryptor.Encrypt(nvp, m_Key);
        }

        /// <summary>
        /// Decrypts the specified token. Clears and resets all the values based on the token
        /// </summary>
        /// <param name="token">The token.</param>
        public void Decrypt(string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(token.Trim()))
                throw new ArgumentNullException("token");

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(token.Trim()))
            {
                Initialize(token);
            }
        }

        #endregion

        #region IDictionary<string,string> Members

        ///<summary>
        ///Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the specified key.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key; otherwise, false.
        ///</returns>
        ///
        ///<param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</param>
        ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool ContainsKey(string key)
        {
            return m_Items.ContainsKey(key);
        }

        ///<summary>
        ///Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        ///</summary>
        ///
        ///<param name="value">The object to use as the value of the element to add.</param>
        ///<param name="key">The object to use as the key of the element to add.</param>
        ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        ///<exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</exception>
        ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
        public void Add(string key, string value)
        {
            Add(key,value);
        }

        ///<summary>
        ///Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        ///</returns>
        ///
        ///<param name="key">The key of the element to remove.</param>
        ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool Remove(string key)
        {
            return Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return TryGetValue(key, out value);
        }

        ///<summary>
        ///Gets or sets the element with the specified key.
        ///</summary>
        ///
        ///<returns>
        ///The element with the specified key.
        ///</returns>
        ///
        ///<param name="key">The key of the element to get or set.</param>
        ///<exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
        ///<exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
        public string this[string key]
        {
            get { return m_Items[key]; }
            set { m_Items[key] = value; }
        }

        ///<summary>
        ///Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        ///</returns>
        ///
        public ICollection<string> Keys
        {
            get { return m_Items.Keys; }
        }

        ///<summary>
        ///Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        ///</returns>
        ///
        public ICollection<string> Values
        {
            get { return m_Items.Values; }
        }

        #endregion

        #region ICollection<KeyValuePair<string,string>> Members

        ///<summary>
        ///Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        ///</summary>
        ///
        ///<param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(KeyValuePair<string, string> item)
        {
            m_Items.Add(item);
        }

        ///<summary>
        ///Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        ///</summary>
        ///
        ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        public void Clear()
        {
            m_Items.Clear();
        }

        ///<summary>
        ///Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        ///</summary>
        ///
        ///<returns>
        ///true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return m_Items.Contains(item);
        }

        ///<summary>
        ///Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        ///</summary>
        ///
        ///<param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        ///<param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        ///<exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        ///<exception cref="T:System.ArgumentNullException">array is null.</exception>
        ///<exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            m_Items.CopyTo(array, arrayIndex);
        }

        ///<summary>
        ///Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        ///</returns>
        ///
        ///<param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return m_Items.Remove(item);
        }

        ///<summary>
        ///Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        ///</summary>
        ///
        ///<returns>
        ///The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        ///</returns>
        ///
        public int Count
        {
            get { return m_Items.Count; }
        }

        ///<summary>
        ///Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.
        ///</returns>
        ///
        public bool IsReadOnly
        {
            get { return m_Items.IsReadOnly; }
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,string>> Members

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        #endregion
    }
}