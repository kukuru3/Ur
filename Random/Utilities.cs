using System;
using System.Collections.Generic;
using System.Linq;

namespace Ur.Random {
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
        
        static public T WeightedPick<T>(this IList<T> items, IList<int> weights, Generator rng) {
            var sum = 0; 
            var probs   = new List<int>(32);
            var things  = new List<T>(32);
            
            if (weights.Count != items.Count) throw new Exception("Invalid weighted pick");

            for (var i = 0; i < weights.Count; i++) {
                if (weights[i] > 0) {
                    probs.Add(weights[i]);
                    things.Add(items[i]);
                    sum += weights[i];
                }
            }
            if (sum <= 0) return default(T);
            var roll = rng.Next(0, sum-1);
            sum = 0; 

            for (var i = 0; i < probs.Count;i++) {
                var newSum = sum + probs[i];
                if (roll >= sum && roll < newSum) return things[i];
                sum = newSum;
            }
            throw new Exception("Invalid weighted pick");
        }

        static public T WeightedPick<T>(this IDictionary<T, int> items, Generator rng) {
            var sum = 0; 
            foreach (var item in items)  sum += item.Value;
            if (sum <= 0) return default(T);
            var roll = rng.Next(0, sum-1);
            sum = 0;
            foreach (var item in items) {
                var newSum = sum + item.Value;
                if (roll >= sum && roll < newSum) return item.Key;
                sum = newSum;
            }
            throw new Exception("Invalid weighted pick");
            
        }

        public delegate int WeightedPickScorer<T>(T item);

        static public T WeightedPick<T>(this IEnumerable<T> items, WeightedPickScorer<T> scoreFunction, Generator rng) {
            var dict = new Dictionary<T, int>();
            foreach (var item in items) dict[item] = scoreFunction(item);
            return WeightedPick(dict, rng);
        }
                
    }
}
