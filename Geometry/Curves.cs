using System;
using System.Collections.Generic;
using System.Linq;
using crds2 = Ur.Geometry.Vector2;
using static Ur.Utilities.Numbers;

namespace Ur.Geometry {
    public static class Curves {


        /// <summary> To be honest I ported this from Ur and have no idea what is what. 
        /// I am guessing a and b are points and c and d are controls but w/e</summary>        
        static private crds2 CubicInterpolate(crds2 a, crds2 b, crds2 c, crds2 d, float mu) {
            float mu2 = mu * mu;
            var a0 = d - c - a + b;
            var a1 = a - b - a0;
            var a2 = c - a;            
            return a0 * mu * mu2 + a1 * mu2 + a2 * mu + b;
        }

        /// <summary> Given a growing series of numbers L[], and a number N, find N's order in series
        /// (if N is 7.4 it means it is 40% between 7th and 8th member of the series  </summary>
        static private float FloatingPointIndexOfValueInSeries(float[] series, float value) {

            var resultingIndex = Array.BinarySearch(series, value);

            // "If value is not found and value is less than one or more elements in array, 
            // the negative number returned is the bitwise complement of the index of the first element that is larger than value."
            if (resultingIndex < 0) resultingIndex = ~resultingIndex;
            
            var smallerIndex = resultingIndex - 1;
            var remainder = (value - series[smallerIndex]) / (series[resultingIndex] - series[smallerIndex]);
            return remainder + smallerIndex;
                        
        }

        static public crds2[] SmoothPolyline(IEnumerable<crds2> input, float spacing) {
            var s = input.ToArray();
            var n = s.Length;
            var cumula = new float[n];
            var single = new float[n];
            // find individual distances, build a cumulative values array from it.
            for (var i = 0; i < n - 1; i++) {
                single[i+1] = Vectors.Distance(s[i], s[i+1]);
                cumula[i+1] = cumula[i] + single[i+1];
            }
            
            var result = new List<crds2>();
            var cursor = 0f;
            float max = cumula[n-1];
            result.Add(s[0]);

            do {
                cursor += spacing;
                if (cursor >= max - float.Epsilon) break;
                float mu = FloatingPointIndexOfValueInSeries(cumula, cursor);
                int i = mu.Floor();
                var A = s[Max(i - 1, 0)];
                var B = s[i];
                var C = s[i+1];
                var D = s[ (i < n - 2) ? i + 2 : n - 1 ]; // wut.
                var crds = CubicInterpolate(A, B, C, D, mu - i);
                result.Add(crds);
            } while (cursor < max);

            result.Add(s[n-1]);
            return result.ToArray();
            
        }

    }
}
