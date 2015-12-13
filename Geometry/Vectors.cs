using System;
using System.Collections.Generic;
using Urb.Utilities;

namespace Urb.Geometry {

    static public class Vectors {

        static public float  Dot(Vector a, Vector b) {
            return a.x * b.x + a.y * b.y;
        }
    
        /// <summary> Use this when you want to approach from source to target by a certain distance.</summary>
        /// <returns> A point between source and target, distant maxDistance from the source.</returns>
        /// <remarks>If the target is closer to the source than the max distance, returns target. There is no overshooting.</remarks>
        static public Vector Approach(this Vector source, Vector target, float maxDistance) {
            var delta = target - source; 
            if (delta.LengthSquared < maxDistance * maxDistance) return target;
            return source + delta.Normalized * maxDistance;
        }

        /// <summary>Linearly interpolates between two vectors</summary>        
        /// <param name="amount">coefficient of approach. At 0.0, result = a. At 1.0 , result = b</param>        
        static public Vector Lerp(this Vector a, Vector b, float amount) {
            return a + (b - a) * amount;
        }

        static public float Distance(this Vector a, Vector b) {
            return (a - b).Length;
        }

        static public float DistanceSquared(this Vector a, Vector b) {
            return (a - b).LengthSquared;
        }

        static public Grid.Coords Truncate(this Vector a) {
            return new Grid.Coords(a.x.Floor(), a.y.Floor());
        }

        static public Grid.Coords Round(this Vector a) {
            return new Grid.Coords(a.x.Round(), a.y.Round());
        }
    }
}
