using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib.UGUI.Collections.Generic
{
    internal class Hashtable<TKey, TValue>
    {
        private Hashtable _hashtable = new();

        public TValue this[TKey key] { get => (TValue)_hashtable[key]; set => _hashtable[key] = value; }

        //public ICollection<TKey> Keys => _hashtable.Keys;

        //public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)_hashtable).Values;

        public int Count => _hashtable.Count;

        public bool IsReadOnly => _hashtable.IsReadOnly;

        public void Add(TKey key, TValue value)
            => _hashtable.Add(key, value);

        public void Clear()
            => _hashtable.Clear();

        public bool ContainsKey(TKey key)
            => _hashtable.ContainsKey(key);

        public bool ContainsValue(TValue value)
            => _hashtable.ContainsValue(value);

        public bool Remove(TKey key)
        {
            if (_hashtable.ContainsKey(key))
            {
                _hashtable.Remove(key);
                return true;
            }
            return false;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_hashtable.ContainsKey(key))
            {
                value = (TValue)_hashtable[key];
                return true;
            }
            value = default;
            return false;
        }
    }
}
