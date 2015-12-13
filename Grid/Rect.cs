using System;
using System.Collections.Generic;

namespace Urb.Grid {

    [System.Diagnostics.DebuggerDisplay("Rect [{X0},{Y0} -> {X1},{Y1}] (W : {Width}, H : {Height})")]
    public struct Rect {
        public readonly int X0;
        public readonly int Y0;
        public readonly int X1;
        public readonly int Y1;

        public Coords BoundsLow { get { return new Coords(X0, Y0); } }
        public Coords BoundsHigh { get {  return new Coords(X1, Y1); } }
        public Coords Dimension { get {  return new Coords(Width, Height); } }

        public int Width { get { return X1 - X0 + 1;} }
        public int Height { get { return Y1 - Y0 + 1; } }

        public Rect(Coords lowest, Coords dimension) {
            X0 = lowest.X; Y0 = lowest.Y;
            X1 = X0 + dimension.X - 1;
            Y1 = Y0 + dimension.Y - 1;
        }
        public Rect(int x0, int y0, int w, int h) {
            X0 = x0; Y0 = y0; 
            X1 = x0 + w - 1; Y1 = y0 + h - 1;
        }

        public IEnumerable<Coords> Enumerate() {
            for (var y = Y0; y <= Y1; y++)
            for (var x = X0; x <= X1; x++) 
                yield return new Coords(x, y);            
        }

        /// <summary> Return true if coordinates supplied fall within this rect. </summary>        
        public bool Contains(Coords crds) {
            return crds.X >= X0 && crds.X <= X1 && crds.Y >= Y0 && crds.Y <= Y1;
        }

        public Rect OffsetBy(int x, int y) {
            return new Rect(X0 + x, Y0 + y, Width, Height );
        }

        public Rect OffsetBy(Coords crds) {
            return new Rect(X0 + crds.X, Y0 + crds.Y, Width, Height );
        }

        public Rect Resize(Coords newDim) {
            return new Rect(X0, Y0, newDim.X, newDim.Y);
        }

        public Rect Resize(int w, int h) {
            return new Rect(X0, Y0, w, h);
        }

        public static Rect FromBounds(int minX, int maxX, int minY, int maxY) {
            return new Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }
    }
}
