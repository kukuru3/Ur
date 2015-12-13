using System;
using System.Collections.Generic;
using Urb.Utilities;

namespace Urb.Grid {

    public delegate T Constructor<T>(IGrid<T> grid, int x, int y);

    public class Grid2D<T> : IGrid<T> {

        #region Fields
        private T[,] tiles;
        #endregion

        #region Properties
        public int W { get; }
        public int H { get; }

        int IGrid.W {
            get {
                throw new NotImplementedException();
            }
        }

        int IGrid.H {
            get {
                throw new NotImplementedException();
            }
        }

        T IGrid<T>.this[int x, int y] {
            get {
                throw new NotImplementedException();
            }
        }

        public T this[int x, int y] {
            get {
                if (!HasValueAt(x, y)) return default(T);
                return tiles[x, y];
            }
        }
        #endregion

        #region Constructors
        public Grid2D(int w, int h) {
            W = w; H = h;
            tiles = new T[W, H];
        }

        public Grid2D(int w, int h, Constructor<T> c) : this(w, h) {
            foreach (var tile in tiles.Iterate()) tiles[tile.X, tile.Y] = c(this, tile.X, tile.Y);
        }

        /// <summary> Careful, will retain the reference to the array</summary>        
        public Grid2D(T[,] sourceValues) {
            if (sourceValues == null) throw new ArgumentNullException("Grid must have a source array in this constructor");
            W = sourceValues.GetLength(0);
            H = sourceValues.GetLength(1);
            tiles = sourceValues;
        } 
        #endregion

        public bool HasValueAt(int x, int y) {
            return x >= 0 && y >= 0 && x <= W && y <= H;
        }

        IEnumerable<T> IGrid<T>.GetAll() {
            foreach (var t in tiles) yield return t;
        }
               
    }

   


}
