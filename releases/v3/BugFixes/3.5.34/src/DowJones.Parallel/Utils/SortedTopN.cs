using System.Collections.Generic;
using System.Collections;

namespace System.Linq
{
    internal class SortedTopN<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly int n;
        private readonly List<TKey> topNKeys;
        private readonly List<TValue> topNValues;
        private readonly IComparer<TKey> comparer;

        public SortedTopN(int count, IComparer<TKey> comparer)
        {
            if (count < 1) throw new ArgumentOutOfRangeException("count");
            if (comparer == null) throw new ArgumentNullException("comparer");
            n = count;
            topNKeys = new List<TKey>(count);
            topNValues = new List<TValue>(count);
            this.comparer = comparer;
        }

        public bool Add(KeyValuePair<TKey,TValue> item)
        {
            return Add(item.Key, item.Value);
        }

        public bool Add(TKey key, TValue value)
        {
            int position = topNKeys.BinarySearch(key, comparer);
            if (position < 0) position = ~position;
            if (topNKeys.Count < n || position != 0)
            {
                // Empty out an item if we're already full and we need to
                // add another
                if (topNKeys.Count == n)
                {
                    topNKeys.RemoveAt(0);
                    topNValues.RemoveAt(0);
                    position--;
                }

                // Insert or add based on where we're adding
                if (position < n)
                {
                    topNKeys.Insert(position, key);
                    topNValues.Insert(position, value);
                }
                else
                {
                    topNKeys.Add(key);
                    topNValues.Add(value);
                }
                return true;
            }

            // No room for this item
            return false;
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                for (int i = topNKeys.Count - 1; i >= 0; i--)
                {
                    yield return topNValues[i];
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = topNKeys.Count - 1; i>=0; i--)
            {
                yield return new KeyValuePair<TKey, TValue>(topNKeys[i], topNValues[i]);
            }
        }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}
