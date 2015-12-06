using System;
using System.Collections.Generic;
using System.Linq;

namespace Urb.Random {
    static public class Utilities {

        static public T PickRandom<T>(this IEnumerable<T> source, Generator rng) {
            var arr = source.ToArray();
            if (arr.Length == 0) return default(T);
            return arr[rng.Next(0, arr.Length-1)];
        }

        static public T PickRandom<T>(this IList<T> source, Generator rng) {
            if (source?.Count == 0) return default(T);
            return source[rng.Next(0, source.Count-1)];
        }
                
    }
}
