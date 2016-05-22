using System;
using System.Collections.Generic;
using System.Linq;

namespace Ur.Geometry {
    public struct Polygon {

        public Vector2[] Vertices { get; }

        public Polygon(IEnumerable<Vector2> points) {
            Vertices = points.ToArray();
        }

        public bool IsConvex() {
            throw new NotImplementedException();
        }

        public bool IsClockwise() {
            throw new NotImplementedException();
        }

        public bool ContainsPoint(Vector2 point) {
            return Calculate.IsPointInConvexPolygon(this, point);
        }

        public float Surface { get {
            throw new NotImplementedException();
        } }

        public Vector2 Average { get {
            var n = Vertices.Length;
            Vector2 sum = Vector2.Zero;
            foreach (var pt in Vertices) sum += pt;
            return sum / n;
        } }

        public Rect BoundingBox { get {
            return Calculate.AABB(Vertices);
        } }

        public Vector2 Centroid { get {
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
                
            }

            x /= 6 * surface;
            y /= 6 * surface;
            return new Vector2(x, y);
            
        } }

        static public Polygon CreateRegular(Vector2 center, float circleRadius, int numPoints) {
            throw new NotImplementedException();
        }
        
        static public implicit operator Polygon(Vector2[] source) {
            return new Polygon(source);
        }

        public Vector2 this[int index] {
            get { return Vertices[index]; }            
        }

    }
}
