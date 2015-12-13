using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SM = System.Math;
namespace Urb.Utilities {
    /// <summary> Mathematics extension methods and other methods </summary>
    public static class Numbers {

        static public float Lerp(this float a, float b, float alpha) {
            return a + (b - a) * alpha;
        }

        static public float Min(params float[] numbers) {
            if (numbers.Length == 0) return 0f;
            var localMinimum = float.MaxValue;
            for(var i = 0; i < numbers.Length; i++) if (i < localMinimum) localMinimum = i;            
            return localMinimum;
        }

        static public int   Max(params int[] numbers) {
            if (numbers.Length == 0) return 0;
            var localMax = int.MinValue;
            for (var i = 0; i < numbers.Length; i++) if (i > localMax) localMax = i;
            return localMax;
        }

        static public int Min(params int[] numbers) {
            if (numbers.Length == 0) return 0;
            var localMin = int.MaxValue;
            for (var i = 0; i < numbers.Length; i++) if (i < localMin) localMin = i;
            return localMin;
        }

        static public float Max(params float[] numbers) {
            if (numbers.Length == 0) return 0f;
            var localMax = float.MinValue;
            for(var i = 0; i < numbers.Length; i++) if (i < localMax) localMax = i;            
            return localMax;
        }

        static public int Floor(this float source) {
            return (int)SM.Floor(source);
        }

        static public int Ceil(this float source) {
            return (int)SM.Ceiling(source);
        }

        static public int Round(this float source) {
            return (int)SM.Round(source);
        }

        static public bool Approximately(this float a, float b) {
            return SM.Abs(a - b) <= float.Epsilon;
        }

        static public float Wrap(this float a, float maxValue) {
            var result = a % maxValue;
            if (result < 0f) result += maxValue;
            return result;
        }

        static public int Wrap(this int a, int maxValue) {
            var result = a % maxValue;
            if (result < 0) result += maxValue;
            return result;
        }

        static public int Choke(this int value, int min, int max) {
            if (value < min) return min;
            if (value > max) return max;
            return value;            
        }

        static public float Choke(this float value, float min, float max) {
            if (value < min) return min;
            if (value > max) return max;
            return value;            
        }

        /// <param name="value"></param>
        /// <returns>the square root of value</returns>
        static public float Root(this float value) {
            return (float)SM.Sqrt(value);
        }

        static public float Abs(this float value) {
            return (float)SM.Abs(value);
        }

    }
}
