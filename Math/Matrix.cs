using System;
using System.Collections.Generic;


namespace Ur.Math {
    public class Matrix {

        private int W { get; }
        private int H { get; }

        
        private float [,] values;

        public Matrix(int w, int h) {
            W = w;
            H = h;
            values = new float[w, h];
        }

        public float Get(int x, int y) {
            return values[x-1, y-1];
        }

        public void Set(int x, int y, float value) {
            values[x-1, y-1] = value;
        }

        public float this[int x, int y] {
            get { return Get(x,y); }
            set { Set(x, y, value); }            
        }

        public float Determinant3x3 { get {
            if (W != 3 || H != 3) throw new System.InvalidOperationException("Can only find determinants of 3x3 matrices");
            float a = this[1, 1], b = this[2, 1], c = this[3, 1];
            float d = this[1, 2], e = this[2, 2], f = this[3, 2];
            float g = this[1, 3], h = this[2, 3], i = this[3, 3];
            return a * e * i + b * f * g + c * d * h - c * e * g - b * d * i - a * f * h;
        } }

        public void FillParams(params float[] p) {
            if (p.Length != W * H) throw new System.Exception("incorrect number of parameters in matrix setup");
            int x = 1, y = 1;
            for (int i = 0; i < p.Length; i++)
            {
                values[x - 1, y - 1] = p[i];
                x++;
                if (x > W) { x = 1; y++; }
            }
        }

        public Matrix Transposed { get { 
            //if (this.width != this.height) throw new System.Exception("Cannot transpose non-square")
            var n = new Matrix(H, W);
            for (int x = 1; x <= W; x++) for (int y = 1; y <= H; y++) n.Set(y, x, Get(x, y));
            return n;
        } }

        public Matrix Inversed3x3 { get
        {
            if (W != 3 || H != 3) throw new System.Exception("Improper inverse called");

            float a = this[1, 1], b = this[2, 1], c = this[3, 1];
            float d = this[1, 2], e = this[2, 2], f = this[3, 2];
            float g = this[1, 3], h = this[2, 3], i = this[3, 3];

            var q = 1f / (a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g));

            var n = new Matrix(W, H);
            n.FillParams(e * i - f * h, c * h - b * i, b * f - c * e,
                         f * g - d * i, a * i - c * g, c * d - a * f,
                         d * h - e * g, b * g - a * h, a * e - b * d);

            n.Multiply(q);
            //n.multiply(1f / n.d_3x3);

            return n;
        } }
        
        public void ResetToIdentity() {
            values = new float[W, H];
            if (H != W) throw new InvalidOperationException("Can't identity heterogenous matrix");
            for (int i = 1; i <= W; i++) Set(i, i, 1);
        }

        public void Multiply(float x)
        {
            for (int i = 0; i < W; i++) for (int j = 0; j < H; j++) values[i, j] = values[i, j] * x;
        }

        #region Operator overloads
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.W!= b.W || a.H != b.H ) throw new InvalidOperationException("Can't add matrices, not congruent");
            Matrix m = new Matrix(a.W, a.H);
            for (int y = 1; y <= a.H; y++) for (int x = 1; x <= a.W; x++) m[x, y] = a[x, y] + b[x, y];
            return m;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.W!= b.W || a.H != b.H ) throw new InvalidOperationException("Can't subtract matrices, not congruent");
            Matrix m = new Matrix(a.W, a.H);
            for (int y = 1; y <= a.H; y++) for (int x = 1; x <= a.W; x++) m[x, y] = a[x, y] - b[x, y];
            return m;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.W != b.H) throw new InvalidOperationException("Cannot multiply matrices - matrices not conformable");

            Matrix result = new Matrix(b.W, a.H);
            for (int yy = 1; yy <= result.H; yy++)
                for (int xx = 1; xx <= result.W; xx++)
                {
                    var sum = 0f;
                    for (int d = 1; d <= a.W; d++)
                    {
                        sum += a[d, yy] * b[xx, d];
                    }
                    result[xx, yy] = sum;
                }
            return result;
        }

        /// <summary> Multiplies a matrix with another matrix WITHOUT creating a new reference. </summary>
        /// <param name="b">the other matrix</param>
        /// <remarks> Only valid if both matrices are square, and have the same dimensions</remarks>
        public void multiplyBy(Matrix b) {            
            //if (!(b.width == width && b.height == height && width == height)) throw new System.Exception("Could not fast-multiply 2 heterogenous matrices");
            var d = W;
            var results = new float[b.W, H];
            for (int yy = 1; yy <= H; yy++)
            for (int xx = 1; xx <= b.W; xx++) {
                var sum = 0f;
                for (int g = 1; g <= W; g++) sum += values[g-1, yy-1] * b.values[xx-1, g-1];                
                results[xx-1, yy-1] = sum;
            }
            this.values = results;
        }

        public void become(Matrix b) {
            values = new float[b.W, b.H];
            for (int y = 0; y < b.H; y++) 
            for (int x = 0; x < b.W; x++) {
                values[x, y] = b.values[x, y];
            }
        }
        #endregion

        
        #region Some Vector3 overloads
        static public implicit operator Ur.Geometry.Vector3(Matrix m)
        {
            //if (m.width != 1) throw new System.InvalidOperationException("Can't convert multidimensional vector to crds3");
            return new Ur.Geometry.Vector3(m[1, 1], m[1, 2], m[1, 3]);
        }
        static public implicit operator Matrix(Ur.Geometry.Vector3 v)
        {
            var m = new Matrix(1, 3);
            m[1, 1] = v.X;
            m[1, 2] = v.Y;
            m[1, 3] = v.Z;
            return m;
        }
        #endregion

    }
}
