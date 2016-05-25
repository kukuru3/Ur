using System;

namespace Ur.Geometry {
    [System.Diagnostics.DebuggerDisplay("{Print}")]
    public struct Vector3 {
        
        private float x;
        private float y;
        private float z;

        public float X { get { return x; } }
        public float Y { get { return y; } }
        public float Z { get { return z; } }

        public Vector3(float x, float y, float z) {
            this.x = x; this.y = y; this.z = z;
        }

        static public Vector3 Zero { get {  return new Vector3(0,0,0); } }
        static public Vector3 One { get { return new Vector3(1,1,1); } }

        public float LengthSquared { get {
            return x * x + y * y + z * z;
        } }

        public float Length { get {
            return LengthSquared.Root();
        } }

        public Vector3 Normalized { get {
            var d = LengthSquared;
            if (d < float.Epsilon) return new Vector3(1f, 0, 0);
            return this / d.Root();
        } }
        
        static public Vector3 operator + (Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        static public Vector3 operator - (Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        static public Vector3 operator * (Vector3 a, float   b) => new Vector3(a.x * b, a.y * b, a.z * b);
        static public Vector3 operator / (Vector3 a, float   b) => new Vector3(a.x / b, a.y / b, a.z / b);

        static public bool operator == (Vector3 a, Vector3 b) => (a - b).LengthSquared < float.Epsilon;
        static public bool operator != (Vector3 a, Vector3 b) => (a - b).LengthSquared >= float.Epsilon;

        public override bool Equals(object obj) {
            if (!(obj is Vector3)) return false;
            return (Vector3)obj == this;
        }

        public override int GetHashCode() => x.GetHashCode() ^ (y+31f).GetHashCode() ^(z+31f*37f).GetHashCode();
        public string Print => $"[{x.ToString("F")},{y.ToString("F")},{z.ToString("F")}]";
        public override string ToString() => Print;

        public Vector2 Flatten { get {
            return new Vector2(x, y);
        } }
        
    }
}
