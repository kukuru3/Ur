using System;
using System.Collections.Generic;
using System.Linq;

namespace Urb.Geometry {
    public struct Polygon {

        public Vector[] Points { get; }

        public Polygon(IEnumerable<Vector> points) {
            Points = points.ToArray();
        }

        public bool IsConvex() {
            throw new NotImplementedException();
        }

        public bool IsClockwise() {
            throw new NotImplementedException();
        }

        public bool ContainsPoint(Vector point) {
            throw new NotImplementedException();
        }

        public float Surface { get {
            throw new NotImplementedException();
        } }

        public Vector Average { get {
            throw new NotImplementedException();
        } }

        public Vector Centroid { get {
            throw new NotImplementedException();
        } }

        static public Polygon CreateRegular(Vector center, float circleRadius, int numPoints) {
            throw new NotImplementedException();
        }

    }
}
