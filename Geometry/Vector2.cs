using Ur.Utilities;

namespace Ur.Geometry {
    [System.Diagnostics.DebuggerDisplay("[{x} : {y}]")]
    public struct Vector2 {

        public readonly float x;
        public readonly float y;
        
        public Vector2(float x, float y) {
            this.x = x; this.y = y;
        }

        static public Vector2 Zero { get { return new Vector2(0, 0); } }
        static public Vector2 One  { get { return new Vector2(1, 1); } }

        public static Vector2 FromPolar(float angle, float distance) {
            angle = angle * (float)System.Math.PI / 180f;
            var sin = (float)System.Math.Sin(angle);
            var cos = (float)System.Math.Cos(angle);
            return new Vector2(cos * distance, sin * distance);
        }


        public float Length { get { return LengthSquared.Root(); } }        
        public float LengthSquared { get { return x * x + y * y; } }

        public Vector2 Normalized { get {            
            var d = Length;
            if (d < float.Epsilon) return new Vector2(1f, 0);
            return this / d;
        } }
        
        static public Vector2 operator + (Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y); 
        static public Vector2 operator - (Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        
        static public Vector2 operator * (Vector2 a, float  b) => new Vector2(a.x * b, a.y * b ); 
        static public Vector2 operator / (Vector2 a, float  b) => new Vector2(a.x / b, a.y / b ); 

        static public bool operator == (Vector2 a, Vector2 b) => a.DistanceSquared(b) <  float.Epsilon;
        static public bool operator != (Vector2 a, Vector2 b) => a.DistanceSquared(b) >= float.Epsilon;

        public override bool Equals(object obj) {
            if (!(obj is Vector2)) return false;
            return (Vector2)obj == this;
        }

        public override int GetHashCode() =>  x.GetHashCode() ^ (y + 31f).GetHashCode();

        public string Print => $"[{x.ToString("F")},{y.ToString("F")}]";
        
        public override string ToString() => Print;

    }
}
