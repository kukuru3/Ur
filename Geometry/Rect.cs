using System;
using System.Collections.Generic;
using Urb.Utilities;

namespace Urb.Geometry {
    public struct Rect {
        public float X0 { get; }
        public float Y0 { get; }
        public float W { get;  }
        public float H { get; }
        public float X1 { get { return X0 + W;} }
        public float Y1 { get { return Y0 + H; } }
        public Rect(float x0, float y0, float x1, float y1) {
            X0 = x0; Y0 = y0; W = x1 - x0; H = y1 - y0;
        }

        public Grid.Rect IntegerBounds() {
            return new Grid.Rect(
                X0.Floor(), 
                Y0.Floor(),
                W.Ceil(),
                H.Ceil()
            );
        }

    }
}
