using System;
using System.Collections.Generic;

namespace Ur.Grid {

    public delegate T TileProvider<T>(int x, int y);

    public class Grid2D<T> : IGrid<T> {

        #region Fields
        private T[,] tiles;
        #endregion

        #region Properties
        public int W { get; private set; }
        public int H { get; private set; }

        int IGrid.W => W;
        int IGrid.H => H;

        public T this[int x, int y] => HasTile(x, y) ? tiles[x, y] : default(T);

        #endregion

        #region Constructors

        public void Initialize(int w, int h) {
            if (tiles != null) throw new ArgumentException("Grid Already Initialized!");
            W = w; H = h;
            tiles = new T[w, h];
        }

        public void CreateTiles(TileProvider<T> c) {
            foreach (var tile in tiles.Iterate()) tiles[tile.X, tile.Y] = c(tile.X, tile.Y);
        }

        public void FillTiles(T[,] source) {
            tiles = source ?? throw new ArgumentNullException("tiles must be provided");
        }

        #endregion

        public bool HasTile(int x, int y) {
            return x >= 0 && y >= 0 && x < W && y < H;
        }

        public IEnumerable<T> GetAllTiles() {
            foreach (var t in tiles) yield return t;
        }

    }
}
