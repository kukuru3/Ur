using System;
using System.Collections.Generic;

namespace Ur.Collections {

    public class Multidict<TKey, TValue> {

        Dictionary<TKey, List<TValue>> lists = new Dictionary<TKey, List<TValue>>();
        Dictionary<TKey, TValue[] > cache = new Dictionary<TKey, TValue[]>();
        
        bool EntryExists(TKey key) {
            return lists.ContainsKey(key);
        }

        void InvalidateCache(TKey key) {
            cache.Remove(key);
        }

        List<TValue> GetList(TKey key, bool create = false) {
            List<TValue> result = null;
            if (!lists.TryGetValue(key, out result) && create) result = lists[key] = new List<TValue>();
            return result;            
        }
        
        TValue[] GetFromCache(TKey key) {
            TValue[] result;
            if (!EntryExists(key)) return new TValue[0];
            if (!cache.TryGetValue(key, out result)) {  result = cache[key] = lists[key].ToArray(); }
            return result;
        }
        
        public void Add(TKey key, TValue value) {
            GetList(key, true).Add(value);
            InvalidateCache(key);
        }

        public TValue GetSingle(TKey key) {
            var arr= GetFromCache(key);
            if (arr?.Length > 0) return arr[0];
            return default(TValue);
        }

        public TValue[] GetAll(TKey key) {
            return GetFromCache(key);
        }
        
        /// <summary> Removes value from all lists. Also removes multiples</summary>
        /// <param name="val">Value to remove</param>
        /// <returns> Number of removed items. </returns>
        public int RemoveValue(TValue val) {
            int counter = 0;
            foreach (var list in lists.Values) if (list.Remove(val)) counter++;
            return counter;
        }
        
        /// <summary> Removes all values stored under [key]</summary>        
        public void RemoveAll(TKey key) {
            lists.Remove(key);
            InvalidateCache(key);
        }

        public void RemoveExact(TKey key, TValue val) {
            GetList(key)?.Remove(val);
            InvalidateCache(key);
        }

        public void Clear() {
            lists.Clear();
            cache.Clear();
        }

        public IEnumerable<TKey> AllKeys() {
            foreach (var item in lists) yield return item.Key;
        }
        
        public IEnumerable<TValue> AllValues() {
            foreach (var list in cache.Values) foreach (var item in list ) yield return item;
        }
        

    }
}
