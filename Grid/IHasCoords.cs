using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urb.Grid {
    public interface IHasPosition {
        Coords Position { get; }
    }
}
