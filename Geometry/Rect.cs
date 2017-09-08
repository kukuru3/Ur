using System;
using System.Collections.Generic;


namespace Ur.Geometry {
    public struct Rect {
        public float X0 { get; }
        public float Y0 { get; }
        public float W { get;  }
        public float H { get; }
        public float X1 { get { return X0 + W;} }
        public float Y1 { get { return Y0 + H; } }
                
        private Rect(float x0, float y0, float x1, float y1) {
            X0 = x0; Y0 = y0; W = x1 - x0; H = y1 - y0;
        }

        public static Rect FromBounds(float x1, float y1, float x2, float y2) {
            return new Rect(x1, y1, x2, y2);
        }

        public static Rect FromBounds(Vector2 lo, Vector2 hi) {
            return new Rect(lo.x, lo.y, hi.x, hi.y);
        }

        public static Rect FromDimensions(float x0, float y0, float w, float h) {
            return new Rect(x0, y0, x0 + w, y0 + h);
        }

        public static Rect FromCenter(Vector2 coords, int halfw, int halfh) {
            return new Rect(                
                coords.x - halfw,
                coords.y - halfh,
                coords.x + 2 * halfw, 
                coords.y + 2 * halfh
            );
            
        }

        public Grid.Rect IntegerBounds() {
            return new Grid.Rect(
                X0.Floor(), 
                Y0.Floor(),
                W.Ceil(),
                H.Ceil()
            );
        }

        public Rect Expand(float amount) {
            return new Rect( X0 - amount, Y0 - amount, X1 + amount, Y1 + amount);
        }

        public bool Contains(Vector2 coords) {
            var x = coords.x - X0;
            var y = coords.y - Y0;
            return x >= 0f && x <= W && y >= 0f && y <= H;
        }

        public Rect Move(float offX, float offY) {
            return new Rect(X0 + offX, Y0 + offY, X1 + offX, Y1 + offY);
        }

        
    }
}
