﻿using System;
using System.Collections.Generic;
using Ur.Utilities;

namespace Ur.Grid {

    public delegate T Advanced2DConstructor<T>(IGrid<T> owner, int x, int y);
    public delegate T Simple2DConstructor<T>(int x, int y);

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
                if (!HasTile(x, y)) return default(T);
                return tiles[x, y];
            }
        }
        #endregion

        #region Constructors
        public Grid2D(int w, int h) {
            W = w; H = h;
            tiles = new T[W, H];
        }

        public Grid2D(int w, int h, Simple2DConstructor<T> c) : this(w, h) {
            foreach (var tile in tiles.Iterate()) tiles[tile.X, tile.Y] = c(tile.X, tile.Y);            
        }

        /// <summary> Careful, will retain the reference to the array</summary>        
        public Grid2D(T[,] sourceValues) {
            if (sourceValues == null) throw new ArgumentNullException("Grid must have a source array in this constructor");
            W = sourceValues.GetLength(0);
            H = sourceValues.GetLength(1);
            tiles = sourceValues;
        } 
        #endregion

        public bool HasTile(int x, int y) {
            return x >= 0 && y >= 0 && x < W && y < H;
        }

        public IEnumerable<T> GetAll() {
            foreach (var t in tiles) yield return t;
        }
               
    }

   


}
