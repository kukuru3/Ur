using System;
using System.Collections.Generic;

namespace Ur.Grid {
    /// <summary> Any 2d grid of type T </summary>
    /// <typeparam name="T">grid tile.</typeparam>
    public interface IGrid<T> : IGrid {
        T this[int x, int y] { get; }
        IEnumerable<T> GetAllTiles();
    }

    /// <summary> A generic grid interface </summary>
    public interface IGrid {
        bool HasTile(int x, int y);
        int W { get; }
        int H { get; }
    }
}
