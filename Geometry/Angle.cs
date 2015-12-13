using System;
using System.Collections.Generic;

namespace Urb.Geometry {
    public struct Angle {

        internal const float Rad2Deg = (float)(180.0 / System.Math.PI);
        internal const float Deg2Rad = 1f / Rad2Deg;

        private readonly float inDegrees;

        public float Degrees { get { return inDegrees; } }
        public float Radians { get { return inDegrees * Deg2Rad; } }
        
        /// <summary> Creates a new Angle with specified degrees </summary>        
        public Angle(float degrees) { this.inDegrees = degrees; }

        static public Angle FromRadians(float radians) {
            return new Angle( radians * Rad2Deg );
        }

    }
}
