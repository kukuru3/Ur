using System;
using System.Collections.Generic;
using Urb.Utilities;

namespace Urb.Geometry {
    /// <summary> All advanced geometry calculations exposed outside Ur will go through here. </summary>
    static public class Calculate {
        public static float DistanceOfPointToSegment(Vector a, Vector b, Vector p) {
            float l2 = Vectors.DistanceSquared(a, b);
            if (l2 < 0.000001f) return a.Distance(p); // case : a == b
            float t = Vectors.Dot(p - a, b - a) / l2;
            if (t < 0f) return a.Distance(p);
            if (t > 1f) return b.Distance(p);
            Vector proj = a.Lerp(b, t);
            return proj.Distance(p);
        }

        public static Rect AABB(params Vector[] points) {
            return AABB((IEnumerable<Vector>)points);
        }

        public static Rect AABB(IEnumerable<Vector> points) {
            var minX = float.MaxValue; var maxX = float.MinValue;
            var minY = float.MaxValue; var maxY = float.MinValue;

            foreach (var point in points) {
                if (point.x < minX) minX = point.x; if (point.x > maxX) maxX = point.x;
                if (point.y < minY) minY = point.y; if (point.y > maxY) maxY = point.y;
            }
            return new Rect(minX, minY, maxX, maxY);

        }

        /// <summary> Returns whether point P is inside triangle (A, B, C) </summary>        
        public static bool IsPointInTriangle(Vector A, Vector B, Vector C, Vector p) {
            // Compute vectors        
            var v0 = C - A;
            var v1 = B - A;
            var v2 = p - A;

            // Compute dot products
            var dot00 = Vectors.Dot(v0, v0);
            var dot01 = Vectors.Dot(v0, v1);
            var dot02 = Vectors.Dot(v0, v2);
            var dot11 = Vectors.Dot(v1, v1);
            var dot12 = Vectors.Dot(v1, v2);

            // Compute barycentric coordinates
            var invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            var v = (dot00 * dot12 - dot01 * dot02) * invDenom;
            // Check if point is in triangle
            return (u > 0f) && (v > 0f) && (u + v < 1f);
        }
                
        /// <returns>true if point p lies to the left of directed infinite line A->B</returns>
        /// <remarks>returns false if point lies on the line!</remarks>
        static public bool IsPointLeftOfLine(Vector lineA, Vector lineB, Vector p ) {
           return LeftnessOfPoint(lineA, lineB, p) > 0;
        }

        /// <summary> Returns "leftness" of point p relative to directed infinite line A->B</summary>        
        /// <returns>-1 if left, 0 if lies on line, 1 if right</returns>
        static public int LeftnessOfPoint(Vector lineA, Vector lineB, Vector p) {
            var g = lineB - lineA;
            var h = p - lineA;
            return (g.x * h.y - g.y * h.x).Sign();
        }

        public static bool IsPointInTriangle(Vector[] triangle, Vector P) {
            return IsPointInTriangle(triangle[0], triangle[1], triangle[2], P);
        }

        public static bool IsPointInConvexPolygon(Polygon poly, Vector point) {
            var g = poly.Centroid;
            var n = poly.Vertices.Length;
            for (var i = 0; i < n; i++) {
                var j = i + 1; if (j == n) j = 0;
                if (IsPointInTriangle(poly[i], poly[j], g, point)) return true;
            }
            return false;
        }
    }
}
