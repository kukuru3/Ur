using System;
using System.Collections.Generic;
using System.Linq;

namespace Urb.Geometry {
    public struct Polygon {

        public Vector[] Vertices { get; }

        public Polygon(IEnumerable<Vector> points) {
            Vertices = points.ToArray();
        }

        public bool IsConvex() {
            throw new NotImplementedException();
        }

        public bool IsClockwise() {
            throw new NotImplementedException();
        }

        public bool ContainsPoint(Vector point) {
            return Calculate.IsPointInConvexPolygon(this, point);
        }

        public float Surface { get {
            throw new NotImplementedException();
        } }

        public Vector Average { get {
            var n = Vertices.Length;
            Vector sum = Vector.Zero;
            foreach (var pt in Vertices) sum += pt;
            return sum / n;
        } }

        public Rect BoundingBox { get {
            return Calculate.AABB(Vertices);
        } }

        public Vector Centroid { get {
            var n = Vertices.Length;
            float sum = 0f;
            for (var i = 0; i < n; i++) { var j = i + 1; if (j == n) j = 0;                
                sum += this[i].x * this[j].y - this[j].x * this[i].y;
            }
            var surface = sum / 2;
            float x = 0; float y = 0;
            for (var i = 0; i < n; i++) { var j = i + 1; if (j == n) j = 0;
                float ix = this[i].x, iy = this[i].y, jx = this[j].x, jy = this[j].y;
                var k = ix * jy - jx * iy;
                x += (ix + jx) * k;
                y += (iy + jy) * k;
                x /= 6 * surface;
                y /= 6 * surface;
            }
            return new Vector(x, y);
            
        } }

        static public Polygon CreateRegular(Vector center, float circleRadius, int numPoints) {
            throw new NotImplementedException();
        }
        
        static public implicit operator Polygon(Vector[] source) {
            return new Polygon(source);
        }

        public Vector this[int index] {
            get { return Vertices[index]; }            
        }

    }
}
