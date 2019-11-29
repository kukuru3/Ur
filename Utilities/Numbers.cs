using SM = System.Math;

namespace Ur {
    /// <summary> Mathematics extension methods and other methods </summary>
    public static class Numbers {

        static public float Lerp(this float a, float b, float alpha) {
            return a + (b - a) * alpha;

        }

        static public float Min(params float[] numbers) {
            if (numbers.Length == 0) return 0f;
            var localMinimum = float.MaxValue;
            for (var i = 0; i < numbers.Length; i++) if (numbers[i] < localMinimum) localMinimum = numbers[i];
            return localMinimum;
        }

        static public int Max(params int[] numbers) {
            if (numbers.Length == 0) return 0;
            var localMax = int.MinValue;
            for (var i = 0; i < numbers.Length; i++) if (numbers[i] > localMax) localMax = numbers[i];
            return localMax;
        }

        static public int Min(params int[] numbers) {
            if (numbers.Length == 0) return 0;
            var localMin = int.MaxValue;
            for (var i = 0; i < numbers.Length; i++) if (numbers[i] < localMin) localMin = numbers[i];
            return localMin;
        }

        static public float Max(params float[] numbers) {
            if (numbers.Length == 0) return 0f;
            var localMax = float.MinValue;
            for (var i = 0; i < numbers.Length; i++) if (numbers[i] > localMax) localMax = numbers[i];
            return localMax;
        }

        static public int Floor(this float source) {
            return (int)SM.Floor(source);
        }

        static public float Trunc(this float source) {
            return source - (float)SM.Floor(source);
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

        /// <summary> Wraps and integer value to be between 0 and divisor</summary>        
        static public int Wrap(this int a, int divisor) {
            var result = a % divisor;
            if (result < 0) result += divisor;
            return result;
        }

        static public int Choke(this int value, int min, int max) {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        static public float Choke(this float value, float min = 0f, float max = 1f) {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <param name="value"></param>
        /// <returns>the square root of value</returns>
        static public float Root(this float value) {
            return (float)SM.Sqrt(value);
        }

        static public float Root(this double value) {
            return (float)SM.Sqrt(value);
        }

        static public float Abs(this float value) {
            return SM.Abs(value);
        }

        static public int Abs(this int value) {
            return SM.Abs(value);
        }

        static public int Sign(this int value) {
            return SM.Sign(value);
        }

        static public int Sign(this float value) {
            return SM.Sign(value);
        }

        static public int GreatestCommonDenominator(int a, int b) {
            var g = a % b;
            if (g == 0) return b; else return GreatestCommonDenominator(b, g);
        }

        static public int LeastCommonMultiple(int a, int b) {
            return (a * b) / (GreatestCommonDenominator(a, b));
        }

        static public float Approach(this float a, float target, float maxApproach) {
            float d = (target - a); float v = a;
            if (SM.Abs(d) < maxApproach) v += d;
            else v += SM.Sign(d) * maxApproach;
            return v;
        }

        static public int Approach(this int a, int target, int maxApproach) {
            int d = (target - a); int v = a;
            if (a.Abs() < maxApproach) v += d;
            else v += d.Sign() * maxApproach;
            return v;
        }

    }
}
