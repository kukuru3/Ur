using System;
using System.Collections.Generic;
using Ur.Utilities;

namespace Ur.Geometry {
    public struct Angle {

        internal const float Rad2Deg = (float)(180.0 / System.Math.PI);
        internal const float Deg2Rad = 1f / Rad2Deg;

        private readonly float inDegrees;

        public float Degrees { get { return inDegrees; } }
        public float Radians { get { return inDegrees * Deg2Rad; } }
        
        /// <summary> Creates a new Angle with specified degrees </summary>        
        public Angle(float degrees) { inDegrees = degrees.Wrap(360f); }

        static public Angle FromRadians(float radians) {
            return new Angle( radians * Rad2Deg );
        }
        
        static public Angle FromDelta(float dx, float dy) {            
            if (dx.Abs() < 0.0001f) return new Angle( (dy > 0.0f) ? 90.0f : 270.0f );
            if (dx > 0.0f)          return new Angle((float)(System.Math.Atan(dy / dx) * Rad2Deg));
            return new Angle((float)(System.Math.Atan(dy / dx) * Rad2Deg) + 180.0f);
        }

        public static object FromDelta(object flatten1, object flatten2) {
            throw new NotImplementedException();
        }

        static public implicit operator Angle(float source) {
            return new Angle(source);
        }


        /// <summary> Retrieves offset between angle and its target</summary>        
        static public float Delta(Angle angle, Angle target, float maxdelta = 360.0f) {
            float psi = target.Degrees - angle.Degrees;
            if (psi > 180.0f) psi -= 360.0f;
            if (psi < -180.0f) psi += 360.0f;
            if (psi.Abs() > maxdelta) return maxdelta * psi.Sign();
            /* else */
            return psi;
        }

        public float Cos { get { return (float)System.Math.Cos(Radians); } }
        public float Sin { get { return (float)System.Math.Sin(Radians); } }

        /// <summary>
        /// Approaches source angle to target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="maxdelta"></param>
        /// <returns></returns>
        public static Angle Approach(Angle source, Angle target, float maxdelta = 360f)
        {
            float psi = target.Degrees - source.Degrees;
            if (psi > 180f) psi -= 360f;
            if (psi < -180f) psi += 360f;
            if (psi.Abs() > maxdelta)   return (source.Degrees + maxdelta * psi.Sign()).Wrap(360f);                
            else                        return (source.Degrees + psi).Wrap(360f);
        }

    }
}
