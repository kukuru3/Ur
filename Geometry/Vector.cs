using System;
using System.Collections.Generic;
using Urb.Utilities;

namespace Urb.Geometry {
    [System.Diagnostics.DebuggerDisplay("[{x} : {y}]")]
    public struct Vector {

        public float x;
        public float y;
        
        public Vector(float x, float y) {
            this.x = x; this.y = y;
        }

        static public Vector Zero { get { return new Vector(0, 0); } }
        static public Vector One  { get { return new Vector(1, 1); } }

        public float Length { get { return LengthSquared.Root(); } }        
        public float LengthSquared { get { return x * x + y * y; } }

        public Vector Normalized { get {            
            var d = Length;
            if (d < float.Epsilon) return new Vector(1f, 0);
            return this / d;
        } }
        
        static public Vector operator + (Vector a, Vector b) => new Vector(a.x + b.x, a.y + b.y); 
        static public Vector operator - (Vector a, Vector b) => new Vector(a.x - b.x, a.y - b.y); 
        static public Vector operator * (Vector a, float  b) => new Vector(a.x * b, a.y * b ); 
        static public Vector operator / (Vector a, float  b) => new Vector(a.x / b, a.y / b ); 

        static public bool operator == (Vector a, Vector b) => a.DistanceSquared(b) <  float.Epsilon;
        static public bool operator != (Vector a, Vector b) => a.DistanceSquared(b) >= float.Epsilon;

        public override int GetHashCode() =>  x.GetHashCode() ^ (y + 31f).GetHashCode();

        public string Print => $"[{x.ToString("F")},{y.ToString("F")}]";
        
        public override string ToString() => Print;
        
    }
}
