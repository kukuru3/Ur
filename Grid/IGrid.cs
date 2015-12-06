using System;
using System.Collections.Generic;

namespace Urb.Grid {
    public interface IGrid<T> : IGrid {
        T this[int x, int y] { get; }        
    }

    public interface IGrid {        
        bool HasValueAt(int x, int y);
        int W { get; }
        int H { get; }
    }
}
