using System.Collections.Generic;
using System.Linq;

namespace Ur.Geometry {
    /// <summary> All advanced geometry calculations exposed outside Ur will go through here. </summary>
    static public class Calculate {

        public static float DistanceOfPointToSegment(Vector2 a, Vector2 b, Vector2 p) {
            float l2 = Vectors.DistanceSquared(a, b);
            if (l2 < 0.000001f) return a.Distance(p); // case : a == b
            float t = Vectors.Dot(p - a, b - a) / l2;
            if (t < 0f) return a.Distance(p);
            if (t > 1f) return b.Distance(p);
            Vector2 proj = a.Lerp(b, t);
            return proj.Distance(p);
        }

        public static Rect AABB(params Vector2[] points) {
            return AABB((IEnumerable<Vector2>)points);
        }

        public static Rect AABB(IEnumerable<Vector2> points) {
            var minX = float.MaxValue; var maxX = float.MinValue;
            var minY = float.MaxValue; var maxY = float.MinValue;

            foreach (var point in points) {
                if (point.x < minX) minX = point.x; if (point.x > maxX) maxX = point.x;
                if (point.y < minY) minY = point.y; if (point.y > maxY) maxY = point.y;
            }
            return Rect.FromBounds(minX, minY, maxX, maxY);

        }

        /// <summary>A and B define line 1, C and D line 2</summary>        
        public static Vector2? LineLineIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D) {
            var z = (D.y - C.y) * (B.x - A.x) - (D.x - C.x) * (B.y - A.y);
            if (z.Approximately(0f)) return null;
            var U1 = ((D.x - C.x) * (A.y - C.y) - (D.y - C.y) * (A.x - C.x)) / z;
            var U2 = ((B.x - A.x) * (A.y - C.y) - (B.y - A.y) * (A.x - C.x)) / z;
            return A.Lerp(B, U1);
        }

        /// <summary> Returns a list of collisions with the unit grid, in order from A to B</summary>        
        public static Vector2[] IntersectLineWithUnitGrid(Vector2 A, Vector2 B) {
            var D = B - A;

            var intersections = new List<Vector2>();

            if (D.x.Abs() > float.Epsilon) {
                var x0 = Numbers.Min(A.x, B.x).Ceil();
                var x1 = Numbers.Max(A.x, B.x).Floor();
                for (var x = x0; x <= x1; x++) { // intersect our line with vertical line at x
                    var intersection = LineLineIntersection(A, B, new Vector2(x, 0), new Vector2(x, 1));
                    if (intersection.HasValue) {
                        intersections.Add(intersection.Value);
                    }
                }
            }
            if (D.y.Abs() > float.Epsilon) {

                var y0 = Numbers.Min(A.y, B.y).Ceil();
                var y1 = Numbers.Max(A.y, B.y).Floor();
                for (var y = y0; y <= y1; y++) {
                    var intersection = LineLineIntersection(A, B, new Vector2(0, y), new Vector2(1, y));
                    if (intersection.HasValue) {
                        intersections.Add(intersection.Value);
                    }
                }
            }

            intersections = new List<Vector2>(intersections.Distinct());
            intersections.Sort((a, b) => (a.DistanceSquared(A) - b.DistanceSquared(A)).Sign());

            return intersections.ToArray();
        }


        /// <summary> Returns whether point P is inside triangle (A, B, C) </summary>        
        public static bool IsPointInTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 p) {
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
        static public bool IsPointLeftOfLine(Vector2 lineA, Vector2 lineB, Vector2 p) {
            return LeftnessOfPoint(lineA, lineB, p) > 0;
        }

        /// <summary> Returns "leftness" of point p relative to directed infinite line A->B</summary>        
        /// <returns>-1 if left, 0 if lies on line, 1 if right</returns>
        static public int LeftnessOfPoint(Vector2 lineA, Vector2 lineB, Vector2 p) {
            var g = lineB - lineA;
            var h = p - lineA;
            return (g.x * h.y - g.y * h.x).Sign();
        }

        public static bool IsPointInTriangle(Vector2[] triangle, Vector2 P) {
            return IsPointInTriangle(triangle[0], triangle[1], triangle[2], P);
        }

        public static bool IsPointInConvexPolygon(Polygon poly, Vector2 point) {
            var g = poly.Centroid;
            var n = poly.Vertices.Length;
            for (var i = 0; i < n; i++) {
                var j = i + 1; if (j == n) j = 0;
                if (IsPointInTriangle(poly[i], poly[j], g, point)) return true;
            }
            return false;
        }

        static public float Cosine(this Angle a) { return a.Cos; }
        static public float Sine(this Angle a) { return a.Sin; }

        public static float Tangens(this Angle a) { return a.Sin / a.Cos; }
    }
}
