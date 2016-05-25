using System;
using System.Collections.Generic;

namespace Ur.Grid {
    [System.Diagnostics.DebuggerDisplay("[{X},{Y}]")]
    public struct Coords : IEquatable<Coords>, IHasPosition {

        private const int HASH_BASE = 67;

        public readonly int X;
        public readonly int Y;
                
        public override int GetHashCode() {
            unchecked { 
                return (HASH_BASE + Y.GetHashCode()) * HASH_BASE + X.GetHashCode();
            }
        }
        public override bool Equals(object obj) {
            if (!(obj is Coords)) return false;
            return (Coords)obj == this;            
        }
        
        public Coords(int x, int y) { X = x; Y = y; }

        public static bool operator ==(Coords a, Coords b) { return a.X == b.X && a.Y == b.Y; }
        public static bool operator !=(Coords a, Coords b) { return a.X != b.X || a.Y != b.Y; }

        public static Coords operator +(Coords a, Coords b) {  return new Coords(a.X + b.X, a.Y + b.Y); }
        public static Coords operator -(Coords a, Coords b) {  return new Coords(a.X - b.X, a.Y - b.Y); }

        public static Coords operator *(Coords a, int b) { return new Coords(a.X * b, a.Y * b); }
        public static Coords operator /(Coords a, int b) { return new Coords(a.X / b, a.Y / b); }

        public static float  Distance(Coords a, Coords b) {
            var dx = (a.X - b.X); var dy = (a.Y - b.Y);
            return Numbers.Root(dx * dx + dy * dy);
        }

        public static int ManhattanDistance(Coords a, Coords b) {
            var dx = Numbers.Abs(a.X - b.X); var dy = Numbers.Abs(a.Y - b.Y);
            return dx + dy;
        }

        public bool Equals(Coords other) { return this == other; }

        static public IEnumerable<Coords> Iterate(int w, int h) {
            for (var y = 0; y < h; y++)
            for (var x = 0; x < w; x++) {
                yield return new Coords(x, y);
            }
        }

        public Coords Move(int deltaX, int deltaY) {
            return new Coords(this.X + deltaX, this.Y + deltaY);
        }

        Coords IHasPosition.Position { get { return this; } }


    }
}
