using System;
using System.Collections.Generic;

namespace Ur.Geometry {

    public class Vector4 {

        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float W { get; }

        public Vector4(float x, float y, float z, float w) {
            X = x; Y = y; Z = z; W = w;
        }

        public Vector2 XY => new Vector2(X, Y);
    
    }
}
